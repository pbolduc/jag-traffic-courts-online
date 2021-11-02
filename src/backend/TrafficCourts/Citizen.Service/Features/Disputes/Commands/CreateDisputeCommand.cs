using MediatR;
using System.ComponentModel.DataAnnotations;
using TrafficCourts.Citizen.Service.Models;

namespace TrafficCourts.Citizen.Service.Features.Disputes.Commands
{
    public class CreateDisputeCommand : TicketDispute,IRequest<CreateDisputeResponse>
    {

    }

 
}
