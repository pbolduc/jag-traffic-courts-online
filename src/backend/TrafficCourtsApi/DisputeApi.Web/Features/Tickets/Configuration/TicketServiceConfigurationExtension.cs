﻿using DisputeApi.Web.Features.Tickets.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DisputeApi.Web.Features.Tickets.Configuration
{
    /// <summary>
    /// Extension to inject ticket configuration service in service collection
    /// </summary>
    public static class TicketServiceConfigurationExtension
    {
        public static void AddTicketService(this IServiceCollection collection)
        {
            collection.AddTransient<ITicketService, TicketService>();
        }
    }
}
