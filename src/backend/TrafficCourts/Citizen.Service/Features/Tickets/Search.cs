﻿using Grpc.Core;
using Grpc.Net.Client;
using MediatR;
using OneOf;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TrafficCourts.Citizen.Service.Logging;
using TrafficCourts.Citizen.Service.Models.Tickets;
using TrafficCourts.Ticket.Search.Service;

namespace TrafficCourts.Citizen.Service.Features.Tickets
{
    public static class Search
    {
        public class Request : IRequest<Response>
        {
            public const string TicketNumberRegex = "^[A-Z]{2}[0-9]{8}$";
            public const string TimeRegex = "^(2[0-3]|[01]?[0-9]):([0-5]?[0-9])$";

            public string TicketNumber { get; }

            /// <summary>
            /// The 24 hour clock
            /// </summary>
            public int Hour { get; }
            public int Minute { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ticketNumber"></param>
            /// <param name="time"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public Request(string ticketNumber, string time)
            {
                ArgumentNullException.ThrowIfNull(ticketNumber);
                ArgumentNullException.ThrowIfNull(time);

                if (!Regex.IsMatch(ticketNumber, TicketNumberRegex))
                {
                    throw new ArgumentException("ticketNumber must start with two upper case letters and 6 or more numbers", nameof(ticketNumber));
                }

                if (!Regex.IsMatch(time, TimeRegex))
                {
                    throw new ArgumentException("time must be properly formatted 24 hour clock", nameof(time));
                }

                TicketNumber = ticketNumber;
                Hour = int.Parse(time[0..2]);
                Minute = int.Parse(time[3..5]);
            }
        }

        public class Response
        {
            private Response()
            {
            }

            public Response(SearchReply reply)
            {
                ArgumentNullException.ThrowIfNull(reply);
                if (reply.ViolationTicketNumber is null) throw new ArgumentException("Search reply does not contain a violation ticket", nameof(reply));
                if (reply.ViolationDate is null) throw new ArgumentException("Search reply does not contain a violation date", nameof(reply));
                if (reply.ViolationTime is null) throw new ArgumentException("Search reply does not contain a violation time", nameof(reply));

                ViolationTicket ticket = new()
                {
                    TicketNumber = reply.ViolationTicketNumber,
                    IssuedDate = new DateTime(reply.ViolationDate.Year, reply.ViolationDate.Month, reply.ViolationDate.Day, reply.ViolationTime.Hour, reply.ViolationTime.Minute, 0, DateTimeKind.Unspecified),
                };

                foreach (Offence offence in reply.Offences)
                {
                    ViolationTicketCount count = new()
                    {
                        AmountDue = offence.AmountDue / 100m,
                        Description = offence.OffenceDescription,
                        Count = (short)offence.OffenceNumber,
                        TicketedAmount = offence.TicketedAmount / 100m
                    };

                    ticket.Counts.Add(count);
                }

                Result = ticket;
            }

            public Response(Exception exception)
            {
                ArgumentNullException.ThrowIfNull(exception);
                Result = exception;
            }

            /// <summary>
            /// The result value.
            /// </summary>
            public OneOf<ViolationTicket, Exception> Result { get; }

            public static readonly Response Empty = new();
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly GrpcChannel _grpcChannel;
            private readonly ILogger<Handler> _logger;

            public Handler(GrpcChannel grpcChannel, ILogger<Handler> logger)
            {
                _grpcChannel = grpcChannel ?? throw new ArgumentNullException(nameof(logger));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                ArgumentNullException.ThrowIfNull(request);

                using Activity? activity = Diagnostics.Source.StartActivity("Ticket Search");

                using IDisposable? requestScope = _logger.BeginScope(new Dictionary<string, object> { { "Request", request } });
                _logger.LogTrace("Begin handler");

                TicketSearch.TicketSearchClient client = new(_grpcChannel);

                _logger.LogDebug("Creating SearchRequest");

                SearchRequest? searchResult = new()
                {
                    Number = request.TicketNumber,
                    Time = new TimeOfDay
                    {
                        Hour = request.Hour,
                        Minute = request.Minute
                    }
                };

                try
                {
                    _logger.LogDebug("Searching for ticket");
                    SearchReply searchReply = await client.SearchAsync(searchResult, cancellationToken: cancellationToken);
                    using var replyScope = _logger.BeginScope(new Dictionary<string, object> { { "SearchReply", searchReply } });
                    _logger.LogDebug("Search complete");
                    
                    activity?
                        .SetStatus(System.Diagnostics.ActivityStatusCode.Ok);
                    
                    return new Response(searchReply);
                }
                catch (RpcException exception) when (exception.StatusCode is StatusCode.NotFound)
                {
                    // it will be normal for the ticket not to be found in the service
                    _logger.LogDebug(exception, "Not found");
                    
                    activity?
                        .SetStatus(ActivityStatusCode.Ok);
                    
                    return Response.Empty;
                }
                catch (RpcException exception)
                {
                    activity?
                        .SetStatus(ActivityStatusCode.Error)
                        .AddTag("grpc.status_code", exception.StatusCode);

                    _logger.LogError(exception, "Error searching for ticket");
                    return Response.Empty;
                }
            }
        }
    }
}
