using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TrafficCourts.Citizen.Service.Features.Tickets.Commands
{
    public class TicketPaymentConfirmCommand : IRequest<TicketPaymentConfirmResponse>
    {
        [FromQuery(Name = "id")]
        [Required]
        public string Guid { get; set; }

        [FromQuery(Name = "status")]
        [Required]
        public string Status { get; set; }

        [FromQuery(Name = "amount")]
        public string Amount { get; set; }

        [FromQuery(Name = "confNo")]
        public string ConfirmationNumber { get; set; }

        [FromQuery(Name = "transId")]
        public string TransactionId { get; set; }

    }
}
