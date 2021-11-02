﻿using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Chapter09.Service.Bootstrap
{
    public static class ControllersConfigurationSetup
    {
        public static IServiceCollection AddControllersConfiguration(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation()
                .AddNewtonsoftJson();
            return services;
        }
    }
}