using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace CORE
{
    public static class AlarmDatabase
    {
        public static bool addAlarmToDatabase(Alarm a, double value, DateTime time)
        {

            using (var db = new AlarmContext())
            {
                AlarmEntry entry = new AlarmEntry
                {
                    AlarmId = a.Id,
                    DateTime = time,
                    Value = value,
                    InputTagId = a.TagId
                };

                db.AlarmEntries.Add(entry);
                db.SaveChanges();
            }

            return false;
        }
    }
}
