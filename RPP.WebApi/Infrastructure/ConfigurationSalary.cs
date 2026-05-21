using RPP.Common.Infrastructure;

namespace RPP.WebApi.Infrastructure;

public class ConfigurationSalary : IConfigurationSalary
{
    private readonly SalarySettings _settings;

    public ConfigurationSalary(IConfiguration configuration)
    {
        _settings = configuration.GetSection("SalarySettings").Get<SalarySettings>()
            ?? throw new InvalidOperationException("SalarySettings not configured");
    }

    public double ExtraSaleSum => _settings.ExtraSaleSum;
    public int MaxParallelThreads => _settings.MaxParallelThreads;
}