using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class WorkerDataModelTests
{
    [Test]
    public void Validate_IdIsNullOrEmpty_ThrowsValidationException()
    {
        var worker = CreateWorker(null, "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());

        worker = CreateWorker(string.Empty, "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_IdIsNotGuid_ThrowsValidationException()
    {
        var worker = CreateWorker("123", "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_FullNameIsEmpty_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), null, "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_PhoneNumberIsInvalid_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "12345", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_PhoneNumberIsEmpty_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", null, "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_EmailIsInvalid_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-111-22-33", "invalid-email",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_EmailIsEmpty_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-111-22-33", null,
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_PostIsNone_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.None, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_BaseRateIsZero_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), 0, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_BaseRateIsNegative_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-25), -1000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_WorkerUnder16_ThrowsValidationException()
    {
        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, DateTime.Now.AddDays(-10), DateTime.Now.AddYears(-15), 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_HireDateBeforeBirthDate_ThrowsValidationException()
    {
        var birthDate = DateTime.Now.AddYears(-25);
        var hireDate = birthDate.AddDays(-5);

        var worker = CreateWorker(Guid.NewGuid().ToString(), "Петр Петров", "+7-999-111-22-33", "petr@mail.com",
            WorkerPost.Master, hireDate, birthDate, 50000, false);
        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_HireMinor_ThrowsValidationException()
    {
        var birthDate = DateTime.Now.AddYears(-15); // Человеку 15 лет
        var hireDate = DateTime.Now; // Нанимают сегодня

        var worker = new WorkerDataModel(
            Guid.NewGuid().ToString(),
            "Петр Петров",
            "+7-999-111-22-33",
            "petr@mail.com",
            WorkerPost.Master,
            hireDate,
            birthDate,
            50000,
            false);

        Assert.That(() => worker.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AllFieldsCorrect_PassesValidation()
    {
        var id = Guid.NewGuid().ToString();
        var name = "Петр Петров";
        var phone = "+7-999-111-22-33";
        var email = "petr@mail.com";
        var post = WorkerPost.Master;
        var hireDate = DateTime.Now.AddDays(-10);
        var birthDate = DateTime.Now.AddYears(-25);
        var baseRate = 50000.0;
        var isDeleted = false;

        var worker = CreateWorker(id, name, phone, email, post, hireDate, birthDate, baseRate, isDeleted);

        Assert.That(() => worker.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(worker.Id, Is.EqualTo(id));
            Assert.That(worker.FullName, Is.EqualTo(name));
            Assert.That(worker.PhoneNumber, Is.EqualTo(phone));
            Assert.That(worker.Email, Is.EqualTo(email));
            Assert.That(worker.Post, Is.EqualTo(post));
            Assert.That(worker.HireDate, Is.EqualTo(hireDate));
            Assert.That(worker.BirthDate, Is.EqualTo(birthDate));
            Assert.That(worker.BaseRate, Is.EqualTo(baseRate));
            Assert.That(worker.IsDeleted, Is.EqualTo(isDeleted));
        });
    }

    private static WorkerDataModel CreateWorker(string? id, string? fullName,
        string? phoneNumber, string? email, WorkerPost post, DateTime hireDate,
        DateTime birthDate, double baseRate, bool isDeleted)
    {
        return new WorkerDataModel(id!, fullName!, phoneNumber!, email!, post,
            hireDate, birthDate, baseRate, isDeleted);
    }
}