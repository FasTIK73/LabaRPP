using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class BuyerDataModel : IValidation
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime RegistrationDate { get; private set; }

    public BuyerDataModel(string id, string name, string address, string phoneNumber, DateTime registrationDate)
    {
        Id = id;
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        RegistrationDate = registrationDate;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("Id is not a valid GUID");
        if (Name.IsEmpty())
            throw new ValidationException("Field Name is empty");
        if (Name.Length < 2)
            throw new ValidationException("Name is too short");
        if (Address.IsEmpty())
            throw new ValidationException("Field Address is empty");
        if (PhoneNumber.IsEmpty())
            throw new ValidationException("Field PhoneNumber is empty");
        if (!PhoneNumber.IsPhoneNumber())
            throw new ValidationException("Invalid phone number");
        if (RegistrationDate > DateTime.Now)
            throw new ValidationException("Registration date cannot be in the future");
    }
}