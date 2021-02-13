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
    public static class TagProcessing
    {

        public static string TAGS_CONFIG_PATH = @"../../scadaConfig.xml";
        public static string ALARMS_CONFIG_PATH = @"../../AlarmConfig.xml";

        public static Dictionary<string, Tag> TagsDictionary = new Dictionary<string, Tag>();
        public static Dictionary<string, Thread> ThreadsDictionary = new Dictionary<string, Thread>();
        public static Dictionary<string, double> OutputAddressValues = new Dictionary<string, double>();
        public static Dictionary<string, List<Alarm>> Alarms = new Dictionary<string, List<Alarm>>();


        public delegate void ValueDelegate(Tag t, double value, DateTime time);
        public static event ValueDelegate onValueRead = null;

        public delegate void AlarmDelegate(Alarm a, double value, DateTime time);
        public static event AlarmDelegate onAlarmActivated = null;


        static TagProcessing()
        {

            
            loadTagsFromXML();
            loadAlarmsFromXML();

        }

        private static void loadAlarmsFromXML()
        {
            XElement xmlData = XElement.Load(ALARMS_CONFIG_PATH);

            foreach(var x in xmlData.Descendants("alarm"))
            {
                string tagId = (string)x.Attribute("tagId");
                AlarmType alarmType = (AlarmType)(int)x.Attribute("alarmType");
                double limit = (double)x.Attribute("limit");
                AlarmPriority priority = (AlarmPriority)(int)x.Attribute("priority");
                string id = (string)x.Attribute("id");

                addTagAlarm(tagId, alarmType, limit, priority, id);

            }
        }

        internal static bool addTagAlarm(string id, AlarmType alarmType, double limit, AlarmPriority priority, string alarmId = null)
        {
            if (!TagsDictionary.ContainsKey(id))
            {
                return false;
            }
            Tag t = TagsDictionary[id];
            if (!t.GetType().IsSubclassOf(typeof(InputTag))){
                Console.WriteLine("Invalid tag type or nonexistant id");
                return false;
            }

            if (t is AnalogInput)
            {
                AnalogInput ait = (AnalogInput)t;
                if (alarmType == AlarmType.LOW_LIMIT)
                {   
                    if (limit < ait.LowLimit)
                    {
                        Console.WriteLine("Low alarm limit cannot be lower than tag lowlimit");
                        return false;
                    }
        
                }
                else
                {
                    if (limit > ait.HighLimit)
                    {
                        Console.WriteLine("High alarm limit cannot be higher than tag highlimit");
                        return false;
                    }
                }


            }
            else 
            {
                //digital input alarm
                DigitalInput dit = (DigitalInput)t;
                if (alarmType == AlarmType.LOW_LIMIT && limit != 0)
                {

                    Console.WriteLine("Low alarm limit cannot be lower than 0");
                    return false;
                    

                }
                else if (alarmType == AlarmType.HIGH_LIMIT && limit != 1)
                {

                    Console.WriteLine("High alarm limit cannot be higher than 1");
                    return false;
                    
                }
            }

            //if we've made it here, we create an alarm
            Alarm alarm = new Alarm
            {
                TagId = id,
                AlarmType = alarmType,
                Limit = limit,
                Priority = priority

            };

            alarm.Id = alarmId == null ? getNewAlarmId() : alarmId;
            
            Alarms[id].Add(alarm);

            saveAlarmsToXml();
            return true;
        }

        private static void saveAlarmsToXml()
        {
            XElement alarmsElement = new XElement("alarms");
            List<Alarm> alarmsList = new List<Alarm>();

            foreach (List<Alarm> list in Alarms.Values)
            {
                list.ForEach(x => alarmsList.Add(x));
            }

            alarmsElement.Add(from a in alarmsList
                                        select new XElement("alarm",
                                            new XAttribute("id", a.Id == null ? "" : a.Id),
                                            new XAttribute("tagId", a.TagId),
                                            new XAttribute("alarmType", (int)a.AlarmType),
                                            new XAttribute("limit", a.Limit),
                                            new XAttribute("priority", (int)a.Priority)
                                            ));

            using (var sw = new StreamWriter(ALARMS_CONFIG_PATH))
            {
                sw.Write(alarmsElement);
            }

        }

        private static string getNewAlarmId()
        {
            List<Alarm> alarmsList = new List<Alarm>();

            foreach (List<Alarm> list in Alarms.Values)
            {
                list.ForEach(x => alarmsList.Add(x));
            }


            return (alarmsList.Select(x => int.Parse(x.Id)).DefaultIfEmpty(0).Max() + 1).ToString();
        }

        private static void loadTagsFromXML()
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
                                                      //<DigitalOutputTag id="1" description="some desc" IOAddress="0" driverType="1" value="500"></DigitalOutputTag>
                                                      Id = (string)di.Attribute("id"),
                                                      Description = (string)di.Attribute("description"),
                                                      IOAddress = (string)di.Attribute("IOAddress"),
                                                      DriverType = (DriverType)(int)di.Attribute("driverType"),
                                                      value = (double)di.Attribute("value"),

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
                                                    //<AnalogOutputTag id="8" description="some desc3" IOAddress="0" driverType="1" value ="50" lowLimit="0" highLimit="100"></AnalogOutputTag>

                                                    Id = (string)di.Attribute("id"),
                                                    Description = (string)di.Attribute("description"),
                                                    IOAddress = (string)di.Attribute("IOAddress"),
                                                    DriverType = (DriverType)(int)di.Attribute("driverType"),
                                                    value = (double)di.Attribute("value"),
                                                    LowLimit = (double)di.Attribute("lowLimit"),
                                                    HighLimit = (double)di.Attribute("highLimit"),

                                                }).ToList();

            analogOutputs.ForEach(x => AddTag(x));



        }

        public static void saveTagsToXml()
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
                                            new XAttribute("value", tag.value)
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
                                           new XAttribute("value", tag.value),
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


        public static bool AddTag(Tag t)
        {
            //todo add check if id exists?

            if (t.Id == null)
            {
                string newId = getNewId();
                t.Id = newId;

            }

            TagsDictionary[t.Id] = t;

            //todo check if this logic is okay
            if (t.GetType().IsSubclassOf(typeof(OutputTag)))
            {
                OutputTag ot = (OutputTag)t;
                OutputAddressValues[t.IOAddress] = ot.value;
                refreshOutputTagValues();


            }


            saveTagsToXml();

            //Create thread if it's an input tag and initialise alarm list
            if (t is AnalogInput || t is DigitalInput)
            {
                Alarms[t.Id] = new List<Alarm>(); //todo check if initialising new alarm list is ok
                Thread thread = new Thread(new ParameterizedThreadStart(inputTagWork));
                ThreadsDictionary[t.Id] = thread;
                thread.Start(t.Id);
            }

            return true;

        }

        private static string getNewId()
        {
            return (TagsDictionary.Keys.Select(x => int.Parse(x)).DefaultIfEmpty(0).Max() + 1).ToString();
        }


        private static void inputTagWork(object o)
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

                if (itag.DriverType == DriverType.SIMULATION)
                {
                    value = SimulationDriver.ReturnValue(itag.IOAddress);

                }

                else
                {
                    //todo runtime simulation
                }

                if (t is AnalogInput)
                {
                    AnalogInput ait = (AnalogInput)t;
                    if (value < ait.LowLimit)
                    {
                        value = ait.LowLimit;
                    }
                    else if (value > ait.HighLimit)
                    {
                        value = ait.HighLimit;
                    }
                }

                else if (t is DigitalInput)
                {
                    if (value <= 0)
                    {
                        value = 0;
                    }
                    else if (value >= 1)
                    {
                        value = 1;
                    }
                    else if (value < 0.5)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = 1;
                    }
                }

                DateTime time = DateTime.Now;
                List<Alarm> activatedAlarms = getActivatedAlarms(id, value);
                //todo save values to db here

                //todo save alarms to db and txt here
                activatedAlarms.ForEach(x => AlarmDatabase.addAlarmToDatabase(x, value, time));

                if (itag.ScanOn)
                {
                    onValueRead?.Invoke(itag, value, time);
                    activatedAlarms.ForEach(x => onAlarmActivated?.Invoke(x, value, time));
                }

                Thread.Sleep(itag.ScanTime * 1000);

            }

        }

        private static List<Alarm> getActivatedAlarms(string id, double value)
        {
            List<Alarm> list = new List<Alarm>();
            InputTag t = (InputTag)TagsDictionary[id];

            foreach (Alarm a in Alarms[id])
            {
                if (a.AlarmType==AlarmType.LOW_LIMIT && value <= a.Limit)
                {
                    list.Add(a);
                }
                else if (a.AlarmType==AlarmType.HIGH_LIMIT && value >= a.Limit)
                {
                    list.Add(a);
                }
            }


            return list;
        }

        private static void refreshOutputTagValues()
        {
            //Getting all output tags:
            List<OutputTag> outputTags = TagsDictionary.Values.Where(x => x.GetType().IsSubclassOf(typeof(OutputTag))).Select(x => (OutputTag)x).ToList();
            
            //Updating their value property
            outputTags.ForEach(x => x.value = OutputAddressValues[x.IOAddress]);
        }

        public static bool RemoveTag(string id)
        {
            if (!TagsDictionary.ContainsKey(id))
            {
                return false;
            }


            if (ThreadsDictionary.ContainsKey(id))
            {
                ThreadsDictionary[id].Abort();

            }
            TagsDictionary.Remove(id);

            saveTagsToXml();
            return true;

        }

        public static bool SetTagScan(string id, bool scan)
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

        internal static bool SetOutputTagValue(string id, double value)
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

        internal static string getOutputTagValues()
        {
            String info = "-------Output Tag values--------\n";

            foreach (Tag t in TagsDictionary.Values)
            {
                if (t.GetType().IsSubclassOf(typeof(OutputTag)))
                {
                    info += t.ToString() + "\n\t\tVALUE: " + ((OutputTag)t).value + "\n";
                }
            }

            info += "--------------------------------\n";

            return info;
        }
    }
}
