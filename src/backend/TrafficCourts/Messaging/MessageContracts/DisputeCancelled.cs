﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficCourts.Messaging.MessageContracts
{
    public class DisputeCancelled : IMessage
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string? Email { get; set; }
    }
}
