using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPP.Exceptions;

public class IncorrectDatesException : Exception
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public IncorrectDatesException(DateTime start, DateTime end)
        : base($"The end date must be later than the start date. Start: {start:dd.MM.yyyy}, End: {end:dd.MM.yyyy}")
    {
        StartDate = start;
        EndDate = end;
    }
}