﻿using AutoFixture;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficCourts.Arc.Dispute.Service.Mappings;
using TrafficCourts.Arc.Dispute.Service.Models;
using Xunit;

namespace TrafficCourts.Test.Arc.Dispute.Service.Mappings
{
    public class MappingTests
    {
        [Fact]
        public void Test_convert_data_from_TcoDisputeTicket_to_ArcFileRecord_list()
        {
            // Arrange
            var fixture = new Fixture();
            var tcoDisputeTicket = fixture.Create<TcoDisputeTicket>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();

            // Act
            var actual = mapper.Map<List<ArcFileRecord>>(tcoDisputeTicket);

            // Assert
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);

            foreach (var actualRec in actual)
            {
                Assert.Equal(tcoDisputeTicket.TicketIssuanceDate, actualRec.TransactionDate);
                Assert.Equal(tcoDisputeTicket.TicketFileNumber, actualRec.FileNumber);
                Assert.Equal(tcoDisputeTicket.DriversLicence, actualRec.MvbClientNumber);

                if (actualRec is AdnotatedTicket)
                {
                    AdnotatedTicket actualAdnotatedTicket = (AdnotatedTicket)actualRec;
                    Assert.Equal(tcoDisputeTicket.CitizenName, actualAdnotatedTicket.Name);
                    Assert.Contains(tcoDisputeTicket.TicketDetails, _ => _.Section == actualAdnotatedTicket.Section);
                    Assert.Contains(tcoDisputeTicket.TicketDetails, _ => _.Subsection == actualAdnotatedTicket.Subsection);
                    Assert.Contains(tcoDisputeTicket.TicketDetails, _ => _.Paragraph == actualAdnotatedTicket.Paragraph);
                    Assert.Contains(tcoDisputeTicket.TicketDetails, _ => _.Act == actualAdnotatedTicket.Act);
                    Assert.Contains(tcoDisputeTicket.TicketDetails, _ => _.Amount == actualAdnotatedTicket.OriginalAmount);
                    Assert.Equal(tcoDisputeTicket.IssuingOrganization, actualAdnotatedTicket.Organization);
                    Assert.Equal(tcoDisputeTicket.IssuingLocation, actualAdnotatedTicket.OrganizationLocation);
                }
                else
                {
                    DisputedTicket actualDisputedTicket = (DisputedTicket)actualRec;
                    Assert.Equal(tcoDisputeTicket.CitizenName, actualDisputedTicket.Name);
                    Assert.Equal(tcoDisputeTicket.StreetAddress, actualDisputedTicket.StreetAddress);
                    Assert.Equal(tcoDisputeTicket.City, actualDisputedTicket.City);
                    Assert.Equal(tcoDisputeTicket.Province, actualDisputedTicket.Province);
                    Assert.Equal(tcoDisputeTicket.PostalCode, actualDisputedTicket.PostalCode);
                }
            }  
        }

        [Fact]
        public void can_parse_full_section_with_valid_adnotated_ticket_and_full_section()
        {
            // Arrange
            var fixture = new Fixture();
            AdnotatedTicket at = fixture.Create<AdnotatedTicket>();
            string fullSection = "127(1)(a)(ii)";

            // Act
            AdnotatedTicket? actual = CustomMap.ParseFullSection(at, fullSection);

            // Assert
            actual.Section.Equals("127");
            actual.Subsection.Equals("1");
            actual.Paragraph.Equals("a");
        }
    }
}
