using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CORE
{
    public class TagProcessing
    {

        public Dictionary<string, Tag> TagsDictionary { get; set; }
        public Dictionary<string, Thread> ThreadsDictionary { get; set; }


        public TagProcessing()
        {
            TagsDictionary = new Dictionary<string, Tag>();
            ThreadsDictionary = new Dictionary<string, Thread>();
            loadTagsFromXML(@"../../scadaConfig.xml");
            Console.WriteLine("here");
        }

        private void loadTagsFromXML(string path)
        {
            XElement xmlData = XElement.Load(path);
            List<DigitalInput> digitalInputs = (from di in xmlData.Descendants("DigitalInputTag")
                                                select new DigitalInput
                                                {
                                                    //<DigitalInputTag id="1" description="some desc" driverType="1" scanTime="2" scanOn="true"></DigitalInputTag>

                                                    Id = (string)di.Attribute("id"),
                                                    Description = (string)di.Attribute("description"),
                                                    DriverType = (DriverType)(int)di.Attribute("driverType"),
                                                    ScanTime = (int)di.Attribute("scanTime"),
                                                    ScanOn = (bool)di.Attribute("scanOn")

                                                }).ToList();

            digitalInputs.ForEach(x => Console.WriteLine(x.ToString()));
        }

        public bool AddTag(Tag t)
        {
            if (TagsDictionary.ContainsKey(t.Id))
            {
                return false;
            }

            TagsDictionary[t.Id] = t;
            //todo add thread
            return true;

        }

        public bool RemoveTag(string id)
        {
            if (!TagsDictionary.ContainsKey(id))
            {
                return false;
            }

            TagsDictionary.Remove(id);
            //todo kill thread
            return true;

        }

        public bool SetTagScan(string id, bool scan)
        {

            if (!TagsDictionary.ContainsKey(id))
            {
                return false;
            }

            if (!TagsDictionary[id].GetType().IsSubclassOf(typeof(InputTag)))
            {
                return false;
            }

            InputTag tag = (InputTag) TagsDictionary[id];
            tag.ScanOn = scan;
            return true;

        }

        internal bool SetOutputTagValue(string id, double value)
        {
            if (!TagsDictionary.ContainsKey(id))
            {
                return false;
            }

            if (!TagsDictionary[id].GetType().IsSubclassOf(typeof(OutputTag)))
            {
                return false;
            }

            OutputTag tag = (OutputTag)TagsDictionary[id];
            tag.InitialValue = value;
            return true;
        }

        internal string getOutputTagValues()
        {
            String info = "-------Output Tag values--------\n";

            foreach (Tag t in TagsDictionary.Values)
            {
                if (t.GetType() != typeof(OutputTag))
                {
                    info += t.ToString() + "\n";
                }
            }

            info += "--------------------------------\n";

            return info;
        }
    }
}
