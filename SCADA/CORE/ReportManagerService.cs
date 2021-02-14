using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace CORE
{
    class ReportManagerService : IReportManager
    {


        public List<AlarmEntry> getAlarmsByTimestamps(DateTime start, DateTime end)
        {
            using (var db = new AlarmContext())
            {
                return (from alarm in db.AlarmEntries
                        where alarm.DateTime >= start && alarm.DateTime <= end
                        select alarm).OrderByDescending(x => x.Priority).ThenBy(x => x.DateTime).ToList();
            }
        }



        public List<AlarmEntry> getAlarmsByPriority(AlarmPriority priority)
        {
            using (var db = new AlarmContext())
            {
                return (from alarm in db.AlarmEntries
                        where alarm.Priority == priority
                        select alarm).OrderBy(x => x.DateTime).ToList();
            }
        }


        public List<TagValueEntry> getTagValuesByTimestamps(DateTime start, DateTime end)
        {
            using (var db = new TagValueContext())
            {
                return (from entry in db.TagValueEntries
                        where entry.DateTime >= start && entry.DateTime <= end
                        select entry).OrderBy(x => x.DateTime).ToList();
            }
        }

        public List<TagValueEntry> getAnalogInputValues()
        {
            using (var db = new TagValueContext())
            {
                return (from entry in db.TagValueEntries
                        select entry).Where(IsAnalogInput()).OrderBy(x => x.DateTime).ToList();
            }
        }

        public List<TagValueEntry> getDigitalInputValues()
        {
            using (var db = new TagValueContext())
            {
                return (from entry in db.TagValueEntries
                        select entry).Where(IsDigitalInput()).OrderBy(x => x.DateTime).ToList();
            }
        }


        public List<TagValueEntry> getValuesById(string id)
        {
            using (var db = new TagValueContext())
            {
                return  (from entry in db.TagValueEntries
                        where entry.InputTagId == id
                        select entry).OrderBy(x => x.DateTime).ToList();

            }

            
        }

        private static Func<TagValueEntry, bool> IsAnalogInput()
        {
            return x => (
                TagProcessing.getTagById(x.InputTagId) is AnalogInput
                );
        }

        private static Func<TagValueEntry, bool> IsDigitalInput()
        {
            return x => (
                TagProcessing.getTagById(x.InputTagId) is DigitalInput
                );
        }


    }


}
