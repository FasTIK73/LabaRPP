using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPP.Enums;

public enum HomeStatus
{
    None = 0,
    Pending = 1,       // Ожидает ремонта
    InProgress = 2,    // В процессе ремонта
    Completed = 3,     // Ремонт завершен
    Suspended = 4      // Ремонт приостановлен
}