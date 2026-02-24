using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using RPP.Exceptions;
using RPP.Extensions;
using RPP.Infrastructure;
using RPP.Extensions;
using RPP.Infrastructure;
using System.Text.RegularExpressions;

namespace RPP.DataModels;

public class ClientDataModel : IValidation
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime RegistrationDate { get; private set; }

    public ClientDataModel(string id, string name, string address,
        string phoneNumber, DateTime registrationDate)
    {
        Id = id;
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        RegistrationDate = registrationDate;
    }

    public void Validate()
    {
        // Проверка Id
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        // Проверка имени
        if (Name.IsEmpty())
            throw new ValidationException("Field Name is empty");

        if (Name.Length < 2)
            throw new ValidationException("Name is too short (minimum 2 characters)");

        // Проверка адреса
        if (Address.IsEmpty())
            throw new ValidationException("Field Address is empty");

        // Проверка телефона (регулярное выражение)
        if (PhoneNumber.IsEmpty())
            throw new ValidationException("Field PhoneNumber is empty");

        if (!Regex.IsMatch(PhoneNumber, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"))
            throw new ValidationException("Field PhoneNumber is not a valid phone number");

        // Проверка даты регистрации
        if (RegistrationDate > DateTime.Now)
            throw new ValidationException("Registration date cannot be in the future");
    }
}