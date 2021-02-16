using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public enum DriverType
    {
        SIMULATION, REAL_TIME
    }

    public enum AlarmType
    {
        LOW_LIMIT, HIGH_LIMIT
    }

    public enum AlarmPriority
    {
        LOW = 1, MEDIUM, HIGH
    }

    public enum UserType
    {
        ADMIN, REGULAR
    }
}
