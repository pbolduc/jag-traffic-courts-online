using MediatR;

namespace TrafficCourts.Citizen.Service.Features.Disputes.Queries
{
    public class GetDisputeQuery : IRequest<GetDisputeResponse>
    {
        public int DisputeId { get; set; }
    }
}
