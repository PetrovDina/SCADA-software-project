using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE
{
    public static class TagValueDatabase
    {
        public static object locker = new object();

        public static bool addTagValueToDatabase(Tag t, double value, DateTime time)
        {
            lock (locker)
            {
                TagValueEntry entry = new TagValueEntry
                {
                    TagId = t.Id,
                    DateTime = time,
                    Value = value,
                };

                using (var db = new TagValueContext())
                {

                    db.TagValueEntries.Add(entry);
                    db.SaveChanges();
                }


                return true;
            }


        }
    }
}
