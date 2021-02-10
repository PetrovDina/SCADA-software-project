using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CORE
{
    public class TagProcessing
    {

        public string TAGS_CONFIG_PATH = @"../../scadaConfig.xml";
        public Dictionary<string, Tag> TagsDictionary { get; set; }
        public Dictionary<string, Thread> ThreadsDictionary { get; set; }
        public Dictionary<string, double> OutputAddressValues { get; set; }



        public TagProcessing()
        {

            TagsDictionary = new Dictionary<string, Tag>();
            ThreadsDictionary = new Dictionary<string, Thread>();
            OutputAddressValues = new Dictionary<string, double>();

            loadTagsFromXML();
        }

        private void loadTagsFromXML()
        {

            XElement xmlData = XElement.Load(TAGS_CONFIG_PATH);
            List<DigitalInput> digitalInputs = (from di in xmlData.Descendants("DigitalInputTag")
                                                select new DigitalInput
                                                {
                                                    //<DigitalInputTag id="1" description="some desc" IOAddress="0" driverType="1" scanTime="2" scanOn="true"></DigitalInputTag>

                                                    Id = (string)di.Attribute("id"),
                                                    Description = (string)di.Attribute("description"),
                                                    IOAddress = (string)di.Attribute("IOAddress"),
                                                    DriverType = (DriverType)(int)di.Attribute("driverType"),
                                                    ScanTime = (int)di.Attribute("scanTime"),
                                                    ScanOn = (bool)di.Attribute("scanOn")

                                                }).ToList();

            digitalInputs.ForEach(x => AddTag(x));

            List<DigitalOutput> digitalOutputs = (from di in xmlData.Descendants("DigitalOutputTag")
                                                  select new DigitalOutput
                                                  {
                                                      //<DigitalOutputTag id="1" description="some desc" IOAddress="0" driverType="1" initialValue="500"></DigitalOutputTag>
                                                      Id = (string)di.Attribute("id"),
                                                      Description = (string)di.Attribute("description"),
                                                      IOAddress = (string)di.Attribute("IOAddress"),
                                                      DriverType = (DriverType)(int)di.Attribute("driverType"),
                                                      InitialValue = (double)di.Attribute("initialValue"),

                                                  }).ToList();

            digitalOutputs.ForEach(x => AddTag(x));


            List<AnalogInput> analogInputs = (from di in xmlData.Descendants("AnalogInputTag")
                                              select new AnalogInput
                                              {
                                                  //<AnalogInputTag id="5" description="some desc3" IOAddress="0" driverType="1" scanTime="2" scanOn="true" lowLimit="0" highLimit="100"></AnalogInputTag>                 
                                                  Id = (string)di.Attribute("id"),
                                                  Description = (string)di.Attribute("description"),
                                                  IOAddress = (string)di.Attribute("IOAddress"),
                                                  DriverType = (DriverType)(int)di.Attribute("driverType"),
                                                  ScanTime = (int)di.Attribute("scanTime"),
                                                  ScanOn = (bool)di.Attribute("scanOn"),
                                                  LowLimit = (double)di.Attribute("lowLimit"),
                                                  HighLimit = (double)di.Attribute("highLimit"),

                                              }).ToList();

            analogInputs.ForEach(x => AddTag(x));

            List<AnalogOutput> analogOutputs = (from di in xmlData.Descendants("AnalogOutputTag")
                                                select new AnalogOutput
                                                {
                                                    //<AnalogOutputTag id="8" description="some desc3" IOAddress="0" driverType="1" initialValue ="50" lowLimit="0" highLimit="100"></AnalogOutputTag>

                                                    Id = (string)di.Attribute("id"),
                                                    Description = (string)di.Attribute("description"),
                                                    IOAddress = (string)di.Attribute("IOAddress"),
                                                    DriverType = (DriverType)(int)di.Attribute("driverType"),
                                                    InitialValue = (double)di.Attribute("initialValue"),
                                                    LowLimit = (double)di.Attribute("lowLimit"),
                                                    HighLimit = (double)di.Attribute("highLimit"),

                                                }).ToList();

            analogOutputs.ForEach(x => AddTag(x));



        }

        public void saveTagsToXml()
        {
            XElement tagsElement = new XElement("tags");

            XElement digitalInputTagsElement = new XElement("DigitalInputTags");
            List<DigitalInput> DItags = TagsDictionary.Values.Where(x => x.GetType() == typeof(DigitalInput)).Select(x => (DigitalInput)x).ToList();
            digitalInputTagsElement.Add(from tag in DItags
                                        select new XElement("DigitalInputTag",
                                            new XAttribute("id", tag.Id),
                                            new XAttribute("description", tag.Description),
                                            new XAttribute("IOAddress", tag.IOAddress),
                                            new XAttribute("driverType", (int)tag.DriverType),
                                            new XAttribute("scanTime", tag.ScanTime),
                                            new XAttribute("scanOn", tag.ScanOn)
                                            ));


            XElement digitalOutputTagsElement = new XElement("DigitalOutputTags");
            List<DigitalOutput> DOtags = TagsDictionary.Values.Where(x => x.GetType() == typeof(DigitalOutput)).Select(x => (DigitalOutput)x).ToList();
            digitalOutputTagsElement.Add(from tag in DOtags
                                         select new XElement("DigitalOutputTag",
                                            new XAttribute("id", tag.Id),
                                            new XAttribute("description", tag.Description),
                                            new XAttribute("IOAddress", tag.IOAddress),
                                            new XAttribute("driverType", (int)tag.DriverType),
                                            new XAttribute("initialValue", tag.InitialValue)
                                            ));


            XElement analogInputTagsElement = new XElement("AnalogInputTags");
            List<AnalogInput> AItags = TagsDictionary.Values.Where(x => x.GetType() == typeof(AnalogInput)).Select(x => (AnalogInput)x).ToList();
            analogInputTagsElement.Add(from tag in AItags
                                       select new XElement("AnalogInputTag",
                                           new XAttribute("id", tag.Id),
                                           new XAttribute("description", tag.Description),
                                           new XAttribute("IOAddress", tag.IOAddress),
                                           new XAttribute("driverType", (int)tag.DriverType),
                                           new XAttribute("scanTime", tag.ScanTime),
                                           new XAttribute("scanOn", tag.ScanOn),
                                           new XAttribute("lowLimit", tag.LowLimit),
                                           new XAttribute("highLimit", tag.HighLimit)
                                           ));


            XElement analogOutputTagsElement = new XElement("AnalogOutputTags");
            List<AnalogOutput> AOtags = TagsDictionary.Values.Where(x => x.GetType() == typeof(AnalogOutput)).Select(x => (AnalogOutput)x).ToList();

            analogOutputTagsElement.Add(from tag in AOtags
                                        select new XElement("AnalogOutputTag",
                                           new XAttribute("id", tag.Id),
                                           new XAttribute("description", tag.Description),
                                           new XAttribute("IOAddress", tag.IOAddress),
                                           new XAttribute("driverType", (int)tag.DriverType),
                                           new XAttribute("initialValue", tag.InitialValue),
                                           new XAttribute("lowLimit", tag.LowLimit),
                                           new XAttribute("highLimit", tag.HighLimit)
                                           ));

            tagsElement.Add(digitalInputTagsElement);
            tagsElement.Add(digitalOutputTagsElement);
            tagsElement.Add(analogInputTagsElement);
            tagsElement.Add(analogOutputTagsElement);

            //Console.WriteLine(tagsElement);
            using (var sw = new StreamWriter(TAGS_CONFIG_PATH))
            {
                sw.Write(tagsElement);
            }

        }


        public bool AddTag(Tag t)
        {
            //todo maybe check if id is none first to prevent possible clashes with xml ids
            string newId = (TagsDictionary.Count + 1).ToString();
            t.Id = newId;
            TagsDictionary[newId] = t;

            //todo check if this logic is okay
            if (t.GetType().IsSubclassOf(typeof(OutputTag)))
            {
                OutputTag ot = (OutputTag)t;
                OutputAddressValues[t.IOAddress] = ot.InitialValue;
                refreshOutputTagValues();


            }

            saveTagsToXml();

            //Create thread if it's an input tag
            if (t is AnalogInput || t is DigitalInput)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(inputTagWork));
                ThreadsDictionary[t.Id] = thread;
                thread.Start(t.Id);
            }

            return true;

        }

        private void inputTagWork(object o)
        {
            string id = (string)o;
            Tag t = TagsDictionary[id];

            if (!t.GetType().IsSubclassOf(typeof(InputTag)))
            {
                Console.WriteLine("Wrong tag type for thread!");
                return;
            }

            InputTag itag = (InputTag)t;
            double value = 0;

            while (true)
            {
                //if scan is off, we dont write them to trending
                //todo but do we read values anyway?? for the part 2 database? PROBABLY! FIX THIS!
                if (!itag.ScanOn)
                {
                    Thread.Sleep(itag.ScanTime);
                    continue;
                }

                if (itag.DriverType == DriverType.SIMULATION)
                {
                    value = SimulationDriver.ReturnValue(itag.IOAddress);
                    //todo make value in limit range
                    //todo INVOKE EVENT

                    Thread.Sleep(itag.ScanTime);

                }

                //todo runtime simulation
                return;



            }

        }
        private void refreshOutputTagValues()
        {
            //Getting all output tags:
            List<OutputTag> outputTags = TagsDictionary.Values.Where(x => x.GetType().IsSubclassOf(typeof(OutputTag))).Select(x => (OutputTag)x).ToList();
            
            //Updating their value property
            outputTags.ForEach(x => x.InitialValue = OutputAddressValues[x.IOAddress]);
        }

        public bool RemoveTag(string id)
        {
            if (!TagsDictionary.ContainsKey(id))
            {
                return false;
            }

            TagsDictionary.Remove(id);
            //todo kill thread
            saveTagsToXml();
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
            saveTagsToXml();
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

            Tag tag = TagsDictionary[id];
            if (tag.GetType() == typeof(AnalogOutput))
            {
                AnalogOutput aot = (AnalogOutput)tag;
                if (value < aot.LowLimit)
                {
                    value = aot.LowLimit;
                }
                else if (value > aot.HighLimit)
                {
                    value = aot.HighLimit;
                }

            }
            else if (tag.GetType() == typeof(DigitalOutput))
            {
                //todo da li da uopste dozvolim nesto sto je razlicito od 0 i 1?????????? ne i cao
                if (value != 0 && value != 1)
                    return false;

            }
           
            OutputAddressValues[tag.IOAddress] = value;
            refreshOutputTagValues(); //todo what if the new value doesnt fit the leves for another output tag with the same address?
            saveTagsToXml();
            return true;
        }

        internal string getOutputTagValues()
        {
            String info = "-------Output Tag values--------\n";

            foreach (Tag t in TagsDictionary.Values)
            {
                if (t.GetType().IsSubclassOf(typeof(OutputTag)))
                {
                    info += t.ToString() + "\n\t\tVALUE: " + ((OutputTag)t).InitialValue + "\n";
                }
            }

            info += "--------------------------------\n";

            return info;
        }
    }
}
