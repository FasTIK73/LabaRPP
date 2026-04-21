using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPP.Exceptions;

public class ElementNotFoundException : Exception
{
    public string Value { get; private set; }

    public ElementNotFoundException(string value)
        : base($"Element not found for value = '{value}'")
    {
        Value = value;
    }

    public ElementNotFoundException(string message, string value) : base(message)
    {
        Value = value;
    }
}