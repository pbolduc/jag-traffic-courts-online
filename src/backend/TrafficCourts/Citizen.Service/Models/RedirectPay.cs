using System.Diagnostics.CodeAnalysis;
using TrafficCourts.Common;

namespace TrafficCourts.Citizen.Service.Models
{
    [ExcludeFromCodeCoverage(Justification = Justifications.Poco)]
    public class RedirectPay
    {
        public string ViolationTicketNumber { get; set; }
        public string ViolationTime { get; set; }
        public string Counts { get; set; }
        public string CallbackUrl { get; set; }
        public string RedirectUrl { get; set; }
    }
}
