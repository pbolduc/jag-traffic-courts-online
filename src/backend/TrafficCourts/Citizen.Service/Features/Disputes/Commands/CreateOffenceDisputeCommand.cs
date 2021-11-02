using MediatR;
using TrafficCourts.Citizen.Service.Models;

namespace TrafficCourts.Citizen.Service.Features.Disputes.Commands
{
    public class CreateOffenceDisputeCommand : OffenceDispute, IRequest<CreateOffenceDisputeResponse>
    {
    }
}
