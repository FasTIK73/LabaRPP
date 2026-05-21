using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class SalaryDataModel : IValidation
{
    public string Id { get; private set; }
    public string WorkerId { get; private set; }
    public DateTime SalaryDate { get; private set; }
    public double Salary { get; private set; }

    public SalaryDataModel(string id, string workerId, DateTime salaryDate, double salary)
    {
        Id = id;
        WorkerId = workerId;
        SalaryDate = salaryDate;
        Salary = salary;
    }

    public SalaryDataModel(string workerId, DateTime salaryDate, double salary)
        : this(Guid.NewGuid().ToString(), workerId, salaryDate, salary)
    {
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("Id is not a valid GUID");
        if (WorkerId.IsEmpty())
            throw new ValidationException("Field WorkerId is empty");
        if (!WorkerId.IsGuid())
            throw new ValidationException("WorkerId is not a valid GUID");
        if (Salary <= 0)
            throw new ValidationException("Salary must be greater than 0");
    }
}