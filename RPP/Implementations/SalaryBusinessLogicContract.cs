using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;
using RPP.Common.Infrastructure.PostConfigurations;
using RPP.DataModels;
using RPP.StoragesContracts;

namespace RPP.Implementations;

public class SalaryBusinessLogicContract : ISalaryBusinessLogicContract
{
    private readonly ISalaryStorageContract _salaryStorage;
    private readonly ISaleStorageContract _saleStorage;
    private readonly IPostStorageContract _postStorage;
    private readonly IWorkerStorageContract _workerStorage;
    private readonly ILogger _logger;
    private readonly IConfigurationSalary _salaryConfiguration;
    private readonly object _lockObject = new object();

    public SalaryBusinessLogicContract(
        ISalaryStorageContract salaryStorage,
        ISaleStorageContract saleStorage,
        IPostStorageContract postStorage,
        IWorkerStorageContract workerStorage,
        ILogger<SalaryBusinessLogicContract> logger,
        IConfigurationSalary salaryConfiguration)
    {
        _salaryStorage = salaryStorage;
        _saleStorage = saleStorage;
        _postStorage = postStorage;
        _workerStorage = workerStorage;
        _logger = logger;
        _salaryConfiguration = salaryConfiguration;
    }

    public List<SalaryDataModel> GetAllSalariesByPeriod(DateTime fromDate, DateTime toDate)
    {
        if (fromDate >= toDate)
            throw new IncorrectDatesException(fromDate, toDate);
        return _salaryStorage.GetList(fromDate, toDate) ?? throw new NullListException();
    }

    public List<SalaryDataModel> GetAllSalariesByPeriodByWorker(DateTime fromDate, DateTime toDate, string workerId)
    {
        if (fromDate >= toDate)
            throw new IncorrectDatesException(fromDate, toDate);
        if (string.IsNullOrEmpty(workerId))
            throw new ArgumentNullException(nameof(workerId));
        if (!workerId.IsGuid())
            throw new ValidationException("WorkerId is not a valid GUID");
        return _salaryStorage.GetList(fromDate, toDate, workerId) ?? throw new NullListException();
    }

    public void CalculateSalaryByMounth(DateTime date)
    {
        _logger.LogInformation("CalculateSalaryByMounth: {date}", date);

        var startDate = new DateTime(date.Year, date.Month, 1);
        var finishDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

        var workers = _workerStorage.GetList() ?? throw new NullListException();

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = _salaryConfiguration.MaxParallelThreads
        };

        Parallel.ForEach(workers, parallelOptions, worker =>
        {
            try
            {
                CalculateSalaryForWorker(worker, startDate, finishDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating salary for worker {WorkerId}", worker.Id);
            }
        });
    }

    private void CalculateSalaryForWorker(WorkerDataModel worker, DateTime startDate, DateTime finishDate)
    {
        var sales = _saleStorage.GetList(startDate, finishDate, workerId: worker.Id) ?? throw new NullListException();
        var post = _postStorage.GetElementById(worker.PostId) ?? throw new NullListException();

        var salary = post.ConfigurationModel switch
        {
            null => 0,
            CashierPostConfiguration cpc => CalculateSalaryForCashier(sales, startDate, finishDate, cpc),
            SupervisorPostConfiguration spc => CalculateSalaryForSupervisor(startDate, finishDate, spc),
            PostConfiguration pc => pc.Rate
        };

        _logger.LogDebug("The employee {WorkerId} was paid a salary of {Salary}", worker.Id, salary);
        _salaryStorage.AddElement(new SalaryDataModel(worker.Id, finishDate, salary));
    }

    private double CalculateSalaryForCashier(List<SaleDataModel> sales, DateTime startDate, DateTime finishDate, CashierPostConfiguration config)
    {
        var calcPercent = 0.0;
        var days = new List<DateTime>();

        for (var date = startDate; date < finishDate; date = date.AddDays(1))
            days.Add(date);

        Parallel.ForEach(days, new ParallelOptions { MaxDegreeOfParallelism = _salaryConfiguration.MaxParallelThreads }, day =>
        {
            var salesInDay = sales.Where(x => x.SaleDate.Date == day.Date).ToList();
            if (salesInDay.Count > 0)
            {
                var dayPercent = (salesInDay.Sum(x => x.Sum) / salesInDay.Count) * config.SalePercent;
                lock (_lockObject) { calcPercent += dayPercent; }
            }
        });

        var extraSales = sales.Where(x => x.Sum > _salaryConfiguration.ExtraSaleSum).Sum(x => x.Sum);
        var calcBonus = extraSales * config.BonusForExtraSales;

        return config.Rate + calcPercent + calcBonus;
    }

    private double CalculateSalaryForSupervisor(DateTime startDate, DateTime finishDate, SupervisorPostConfiguration config)
    {
        try
        {
            var workerTrend = _workerStorage.GetWorkerTrend(startDate, finishDate);
            return config.Rate + config.PersonalCountTrendPremium * workerTrend;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating supervisor salary");
            return 0;
        }
    }
}