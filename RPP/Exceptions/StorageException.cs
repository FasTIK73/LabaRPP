using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPP.Exceptions;

public class StorageException : Exception
{
    public StorageException(Exception innerException)
        : base($"Error while working in storage: {innerException.Message}", innerException)
    {
    }

    public StorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}