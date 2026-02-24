using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.Enums;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.Infrastructure;
using RPP.Enums;
using RPP.Extensions;
using RPP.Infrastructure;
using System.Text.RegularExpressions;

namespace RPP.DataModels;

public class WorkerDataModel : IValidation
{
    public string Id { get; private set; }
    public string FullName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public WorkerPost Post { get; private set; }
    public DateTime HireDate { get; private set; }
    public DateTime BirthDate { get; private set; }
    public double BaseRate { get; private set; }
    public bool IsDeleted { get; private set; }

    public WorkerDataModel(string id, string fullName, string phoneNumber,
        string email, WorkerPost post, DateTime hireDate, DateTime birthDate,
        double baseRate, bool isDeleted)
    {
        Id = id;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        Post = post;
        HireDate = hireDate;
        BirthDate = birthDate;
        BaseRate = baseRate;
        IsDeleted = isDeleted;
    }

    public void Validate()
    {
        // Проверка Id
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        // Проверка ФИО
        if (FullName.IsEmpty())
            throw new ValidationException("Field FullName is empty");

        // Проверка телефона
        if (PhoneNumber.IsEmpty())
            throw new ValidationException("Field PhoneNumber is empty");

        if (!Regex.IsMatch(PhoneNumber, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"))
            throw new ValidationException("Field PhoneNumber is not a valid phone number");

        // Проверка email
        if (Email.IsEmpty())
            throw new ValidationException("Field Email is empty");

        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ValidationException("Field Email is not a valid email address");

        // Проверка должности
        if (Post == WorkerPost.None)
            throw new ValidationException("Field Post is empty");

        // Проверка базовой ставки
        if (BaseRate <= 0)
            throw new ValidationException("Field BaseRate must be greater than 0");

        // Проверка возраста (не моложе 16 лет)
        var age = DateTime.Now.Year - BirthDate.Year;
        if (BirthDate.Date > DateTime.Now.AddYears(-age))
            age--;

        if (age < 16)
            throw new ValidationException($"Worker is under 16 years old (BirthDate = {BirthDate.ToShortDateString()})");

        // Проверка даты найма
        if (HireDate.Date < BirthDate.Date)
            throw new ValidationException("Hire date cannot be less than birth date");

        if ((HireDate - BirthDate).TotalDays / 365 < 16)
            throw new ValidationException($"Cannot hire a minor (HireDate = {HireDate.ToShortDateString()}, BirthDate = {BirthDate.ToShortDateString()})");
    }
}