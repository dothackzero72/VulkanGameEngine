using Microsoft.Extensions.DependencyInjection;
using System;

namespace VulkanGameEngineLevelEditor
{
    public static class ServiceConfig
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            return services.BuildServiceProvider();
        }
    }
}
