using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RPP.Exceptions;

public class NullListException : Exception
{
    public NullListException() : base("The returned list is null")
    {
    }

    public NullListException(string message) : base(message)
    {
    }
}