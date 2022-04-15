﻿using System.Text.Json.Serialization;
using TrafficCourts.Common.Utils;

namespace TrafficCourts.Citizen.Service.Models.Dispute
{
    [Obsolete("Use NoticeOfDispute")]
    public class TicketDispute
    {
        public string TicketNumber { get; set; }
        public string CourtLocation { get; set; }
        public DateTime ViolationDate { get; set; }
        public string DisputantSurname { get; set; }
        public string GivenNames { get; set; }
        public string StreetAddress { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string HomePhone { get; set; }
        public string DriversLicence { get; set; }
        public string DriversLicenceProvince { get; set; }
        public string WorkPhone { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DateOfBirth { get; set; }
        public string EnforcementOrganization { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly ServiceDate { get; set; }
        public List<TicketCount> TicketCounts { get; set; }
        public bool LawyerRepresentation { get; set; }
        public string InterpreterLanguage { get; set; }
        public bool WitnessIntent { get; set; }
    }
}
