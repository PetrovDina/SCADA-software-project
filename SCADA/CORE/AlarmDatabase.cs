﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.IO;

namespace CORE
{
    public static class AlarmDatabase

    {
        public static object locker = new object();

        public static bool addAlarmToDatabase(Alarm a, double value, DateTime time)
        {
            lock (locker)
            {
                AlarmEntry entry = new AlarmEntry
                {
                    AlarmId = a.Id,
                    DateTime = time,
                    Value = value,
                    InputTagId = a.TagId
                };

                using (var db = new AlarmContext())
                {

                    db.AlarmEntries.Add(entry);
                    db.SaveChanges();
                }

                using (var sw = new StreamWriter("../../AlarmsLog.txt", true))
                {
                    sw.WriteLine(entry.ToString());
                }

                return true;
            }

           
        }


    }
}
