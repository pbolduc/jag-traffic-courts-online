using AutoMapper;
using Gov.TicketSearch;
using System;
using TrafficCourts.Citizen.Service.Features.Tickets.Commands;
using TrafficCourts.Citizen.Service.Features.Tickets.DBModel;
using TrafficCourts.Citizen.Service.Models;
using DisputeOffence = TrafficCourts.Citizen.Service.Models.Offence;
using TicketSearchOffence = Gov.TicketSearch.Offence;

namespace TrafficCourts.Citizen.Service.Features.Tickets.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TicketSearchResponse, TicketDispute>()
                //.ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom<TicketDisputeDiscountAmountResolver>())
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => Keys.TicketDiscountValue))
                .ForMember(dest => dest.DiscountDueDate, opt => opt.MapFrom<TicketDisputeDiscountDueDateResolver>())
                ;

            CreateMap<TicketSearchOffence, DisputeOffence>()
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => Keys.TicketDiscountValue))
                ;

            CreateMap<CreateShellTicketCommand, Ticket>()
                .ForMember(dest => dest.Offences, opt => opt.MapFrom<TicketOffencesResolver>())
                ;

            CreateMap<DisputeOffence, TrafficCourts.Citizen.Service.Features.Tickets.DBModel.Offence>();

            CreateMap<Ticket, TicketSearchResponse>()
                .ForMember(dest => dest.Offences, opt => opt.MapFrom(src=>src.Offences))
                ;

            CreateMap<Ticket, Disputant>()
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Birthdate))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.Address))
                ;

            CreateMap<DBModel.Offence, TicketSearchOffence>();

            CreateMap<TicketPaymentCommand, Payment>()
                .ForMember(dest => dest.ViolationTicketNumber, opt => opt.MapFrom(src => src.TicketNumber))
                .ForMember(dest => dest.ViolationTime, opt => opt.MapFrom(src => src.Time))
                .ForMember(dest => dest.RequestedCounts, opt => opt.MapFrom(src => src.Counts))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.RequestedAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.RequestDateTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => PaymentStatus.InProgress))
                ;

            CreateMap<TicketPaymentConfirmCommand, Payment>()
                .ForMember(dest => dest.CompletedDateTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => string.Equals("cancelled", src.Status, StringComparison.InvariantCultureIgnoreCase)? PaymentStatus.Cancelled: PaymentStatus.Success))
                ;
        }
    }
}
