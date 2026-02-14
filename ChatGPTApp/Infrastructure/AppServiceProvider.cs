using ChatGPTApp.Context;
using ChatGPTApp.Repositories;
using ChatGPTApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTApp.Infrastructure
{
    public static class AppServiceProvider
    {
        public static ServiceProvider ServiceProvider { get; private set; }
        public static void Initialize()
        {
            var services = new ServiceCollection();

            //Services
            services.AddSingleton<APIService>();
            services.AddSingleton<ChatService>();

            //Repositories
            services.AddSingleton<ChatRepository>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
