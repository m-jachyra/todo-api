using ToDo.Data;
using ToDo.Services;

namespace ToDo;

public static class DependencyInstaller
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ToDoService>();
        services.AddSingleton<EventsService>();
        services.AddSingleton<ToDoContext>();

        return services;
    }
}