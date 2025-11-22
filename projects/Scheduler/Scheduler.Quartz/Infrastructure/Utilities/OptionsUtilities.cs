namespace Scheduler.Quartz.Infrastructure.Utilities;

public static class OptionsUtilities
{
    public static TOptions GetOptionsAndBindOrThrow<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string position) where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .Bind(
                configuration.GetSection(position),
                options => { options.ErrorOnUnknownConfiguration = true; });

        return configuration.GetSection(position).Get<TOptions>() ??
               throw new Exception($"Failed to find {position} options");
    }
}