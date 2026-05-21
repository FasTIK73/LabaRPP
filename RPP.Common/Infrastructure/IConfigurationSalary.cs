namespace RPP.Common.Infrastructure;

public interface IConfigurationSalary
{
    double ExtraSaleSum { get; }
    int MaxParallelThreads { get; }
}