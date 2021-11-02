﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficCourts.Citizen.Service
{
    public static class Keys
    {
        public static string Nothing = "n/a";
        public static int TicketDiscountValidDays =
            Environment.GetEnvironmentVariable("TICKET_DISCOUNT_VALID_DAYS") == null
                ? 30
                : Convert.ToInt32(Environment.GetEnvironmentVariable("TICKET_DISCOUNT_VALID_DAYS"));
        public static int TicketDiscountValue =
          Environment.GetEnvironmentVariable("TICKET_DISCOUNT_VALUE") == null
              ? 25
              : Convert.ToInt32(Environment.GetEnvironmentVariable("TICKET_DISCOUNT_VALUE"));
        public static string DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        public static string PaybcApi_BaseUrl =Environment.GetEnvironmentVariable("PaybcApi__BaseUrl");
        public static string PaybcApi_CallbackBaseUrl = Environment.GetEnvironmentVariable("PaybcApi__CallbackBaseUrl");
    }
}
