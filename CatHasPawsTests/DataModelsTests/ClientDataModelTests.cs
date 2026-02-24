using RPP.DataModels;
using RPP.Exceptions;

namespace CatHasPawsTests.DataModelsTests;

[TestFixture]
public class ClientDataModelTests
{
    [Test]
    public void Validate_IdIsNullOrEmpty_ThrowsValidationException()
    {
        var client = new ClientDataModel(null!, "Иван Иванов", "ул. Ленина, 1", "+7-999-123-45-67", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());

        client = new ClientDataModel(string.Empty, "Иван Иванов", "ул. Ленина, 1", "+7-999-123-45-67", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_IdIsNotGuid_ThrowsValidationException()
    {
        var client = new ClientDataModel("123", "Иван Иванов", "ул. Ленина, 1", "+7-999-123-45-67", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_NameIsNullOrEmpty_ThrowsValidationException()
    {
        var client = new ClientDataModel(Guid.NewGuid().ToString(), null!, "ул. Ленина, 1", "+7-999-123-45-67", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());

        client = new ClientDataModel(Guid.NewGuid().ToString(), string.Empty, "ул. Ленина, 1", "+7-999-123-45-67", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_NameIsTooShort_ThrowsValidationException()
    {
        var client = new ClientDataModel(Guid.NewGuid().ToString(), "И", "ул. Ленина, 1", "+7-999-123-45-67", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AddressIsEmpty_ThrowsValidationException()
    {
        var client = new ClientDataModel(Guid.NewGuid().ToString(), "Иван Иванов", null!, "+7-999-123-45-67", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_PhoneNumberIsInvalid_ThrowsValidationException()
    {
        var client = new ClientDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "ул. Ленина, 1", "12345", DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_PhoneNumberIsEmpty_ThrowsValidationException()
    {
        var client = new ClientDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "ул. Ленина, 1", null!, DateTime.Now.AddDays(-30));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_RegistrationDateInFuture_ThrowsValidationException()
    {
        var client = new ClientDataModel(Guid.NewGuid().ToString(), "Иван Иванов", "ул. Ленина, 1", "+7-999-123-45-67", DateTime.Now.AddDays(5));
        Assert.That(() => client.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void Validate_AllFieldsCorrect_PassesValidation()
    {
        var id = Guid.NewGuid().ToString();
        var name = "Иван Иванов";
        var address = "ул. Ленина, 1";
        var phone = "+7-999-123-45-67";
        var regDate = DateTime.Now.AddDays(-30);

        var client = new ClientDataModel(id, name, address, phone, regDate);

        Assert.That(() => client.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(client.Id, Is.EqualTo(id));
            Assert.That(client.Name, Is.EqualTo(name));
            Assert.That(client.Address, Is.EqualTo(address));
            Assert.That(client.PhoneNumber, Is.EqualTo(phone));
            Assert.That(client.RegistrationDate, Is.EqualTo(regDate));
        });
    }
}