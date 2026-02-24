using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPP.Enums;

public enum ReportStatus
{
    None = 0,
    Planned = 1,       // Запланирована
    InProgress = 2,    // В процессе
    Completed = 3,     // Завершена
    Cancelled = 4      // Отменена
}