using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace CORE
{
    class DatabaseManagerService : IDatabaseManager {

        public static TagProcessing TagProcessing { get; set; }

        public DatabaseManagerService()
        {
            if (TagProcessing == null)
            {
                TagProcessing = new TagProcessing();
            }

        }
    

        public bool addTag(Tag t)
        {
            bool success = TagProcessing.AddTag(t);

            if (success)
            {
                Console.WriteLine("Successfully added new tag with id " + t.Id);
            }

            else
            {
                Console.WriteLine("Tag adding failed. Id: " + t.Id);

            }

            return success;
        }



        public bool removeTag(string id)
        {
            bool success = TagProcessing.RemoveTag(id);

            if (success)
            {
                Console.WriteLine("Successfully removed new tag with id " + id );
            }

            else
            {
                Console.WriteLine("Tag removing failed. Id: " + id);

            }
            return success;
        }
        


        public bool setOutputTagValue(string id, double value)
        {
            bool success = TagProcessing.SetOutputTagValue(id, value);

            if (success)
            {
                Console.WriteLine("Successfully set value of output tag with id " + id + " to " + value);
            }

            else
            {
                Console.WriteLine("value change failed. Id: " + id);

            }
            return success;
        }

        public bool setTagScan(string id, bool scan)
        {
            bool success = TagProcessing.SetTagScan(id, scan);

            if (success)
            {
                Console.WriteLine("Successfully set scan of tag with id " + id + " to " + scan);
            }

            else
            {
                Console.WriteLine("Tag scan change failed. Id: " + id);

            }
            return success;
        }

        public string showOutputTagValues()
        {
            return TagProcessing.getOutputTagValues();
        }
    }
}
