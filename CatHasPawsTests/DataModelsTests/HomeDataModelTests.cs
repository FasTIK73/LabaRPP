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
public class HomeDataModelTests
{
    [Test]
    public void Validate_IdIsNullOrEmpty_ThrowsValidationException()
    {
        var home = CreateHome(null, Guid.NewGuid().ToString(), "ул. Пушкина, 10", 75.5, HomeType.Apartment, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_IdIsNotGuid_ThrowsValidationException()
    {
        var home = CreateHome("123", Guid.NewGuid().ToString(), "ул. Пушкина, 10", 75.5, HomeType.Apartment, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_ClientIdIsEmpty_ThrowsValidationException()
    {
        var home = CreateHome(Guid.NewGuid().ToString(), null, "ул. Пушкина, 10", 75.5, HomeType.Apartment, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_ClientIdIsNotGuid_ThrowsValidationException()
    {
        var home = CreateHome(Guid.NewGuid().ToString(), "123", "ул. Пушкина, 10", 75.5, HomeType.Apartment, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AddressIsEmpty_ThrowsValidationException()
    {
        var home = CreateHome(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), null, 75.5, HomeType.Apartment, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AreaIsZero_ThrowsValidationException()
    {
        var home = CreateHome(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Пушкина, 10", 0, HomeType.Apartment, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AreaIsNegative_ThrowsValidationException()
    {
        var home = CreateHome(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Пушкина, 10", -10, HomeType.Apartment, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_TypeIsNone_ThrowsValidationException()
    {
        var home = CreateHome(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Пушкина, 10", 75.5, HomeType.None, HomeStatus.Pending);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_StatusIsNone_ThrowsValidationException()
    {
        var home = CreateHome(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "ул. Пушкина, 10", 75.5, HomeType.Apartment, HomeStatus.None);
        Assert.That(() => home.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AllFieldsCorrect_PassesValidation()
    {
        var id = Guid.NewGuid().ToString();
        var clientId = Guid.NewGuid().ToString();
        var address = "ул. Пушкина, 10";
        var area = 75.5;
        var type = HomeType.Apartment;
        var status = HomeStatus.Pending;

        var home = CreateHome(id, clientId, address, area, type, status);

        Assert.That(() => home.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(home.Id, Is.EqualTo(id));
            Assert.That(home.ClientId, Is.EqualTo(clientId));
            Assert.That(home.Address, Is.EqualTo(address));
            Assert.That(home.Area, Is.EqualTo(area));
            Assert.That(home.Type, Is.EqualTo(type));
            Assert.That(home.Status, Is.EqualTo(status));
        });
    }

    private static HomeDataModel CreateHome(string? id, string? clientId,
        string? address, double area, HomeType type, HomeStatus status)
    {
        return new HomeDataModel(id!, clientId!, address!, area, type, status);
    }
}