using Microsoft.Azure.WebJobs.Extensions.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MyInfo
    {
        public ScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }
}
