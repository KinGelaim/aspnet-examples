namespace Scheduler.Quartz.Application;

public sealed class FeatureToggleOptions
{
    public const string Position = "FeatureToggle";

    public bool ManualJobRunner { get; set; }
}