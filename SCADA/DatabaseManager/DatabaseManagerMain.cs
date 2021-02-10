using DatabaseManager.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DatabaseManager
{
    class DatabaseManagerMain
    {
        static DatabaseManagerClient proxy;

        static void Main(string[] args)
        {

            proxy = new DatabaseManagerClient();

            int option = -1;

            while (option != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("1. Add new digital input tag");
                Console.WriteLine("2. Add new digital output tag");
                Console.WriteLine("3. Add new analog input tag");
                Console.WriteLine("4. Add new analog output tag");
                Console.WriteLine("5. Read output tag values");
                Console.WriteLine("6. Delete tag by ID");
                Console.WriteLine("7. Set input tag scan on/off");
                Console.WriteLine("8. Set output tag value");

                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Pick a number: ");
                bool success = int.TryParse(Console.ReadLine(), out option);

                if (success)
                {
                    switch (option)
                    {
                        case 1:
                            addDigitalInput();
                            break;
                        case 2:
                            addDigitalOutput();
                            break;
                        case 3:
                            addAnalogInput();
                            break;
                        case 4:
                            addAnalogOutput();
                            break;
                        case 5:
                            displayOutputValues();
                            break;
                        case 6:
                            deleteTagById();
                            break;
                        case 7:
                            setTagScan();
                            break;
                        case 8:
                            setOutputValue();
                            break;
                    }
                }
            }


            Console.ReadKey();

        }

        private static void setOutputValue()
        {
            Console.WriteLine("Enter output tag id for value: ");
            string id = Console.ReadLine();

            Console.WriteLine("Enter new value: ");
            double value = -1;
            double.TryParse(Console.ReadLine(), out value);
            if (value == -1)
            {
                Console.WriteLine("Bad value");
                return;
            }

            bool success = proxy.setOutputTagValue(id, value);
            if (success)
            {
                Console.WriteLine("Successfully changed value");
            }
            else
            {
                Console.WriteLine("Failed to change output tag value");
            }

        }

        private static void setTagScan()
        {
            Console.WriteLine("Enter tag id for scan change: ");
            string id = Console.ReadLine();

            int option = -1;
            Console.WriteLine("Enter 1 for scan on or 0 for scan off");
            bool parsed = int.TryParse(Console.ReadLine(), out option);

            if (option != 0 && option != 1)
            {
                Console.WriteLine("Incorrect scan on off input");
                return;
            }
            bool scanOn = option == 1 ? true : false;

            bool success = proxy.setTagScan(id, scanOn);
            if (success)
            {
                Console.WriteLine("Successfully changed scan for tag with id  " + id);
            }
            else
            {
                Console.WriteLine("Failed to change scan for tag with id " + id);
            }
        }

        private static void deleteTagById()
        {
            Console.WriteLine("Enter tag id for deletion: ");
            string id = Console.ReadLine();
            bool success = proxy.removeTag(id);
            if (success)
            {
                Console.WriteLine("Successfully removed tag with id " + id);
            }
            else
            {
                Console.WriteLine("Failed to remove id with id " + id);
            }
        }

        private static void displayOutputValues()
        {
            Console.WriteLine(proxy.showOutputTagValues()); 
        }

        private static void addAnalogOutput()
        {
            Console.WriteLine("Enter description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Enter IOAddress: ");
            string IOAddress = Console.ReadLine();

            Console.WriteLine("Enter 0 for Simulation Driver or 1 for Real Time Driver: ");
            int driverTypeInt = -1;
            int.TryParse(Console.ReadLine(), out driverTypeInt);
            if (driverTypeInt != 0 && driverTypeInt != 1)
            {
                Console.WriteLine("Wrong driver number!");
                return;
            }



            Console.WriteLine("Enter low limit: ");
            double lowLimit = -1;
            double.TryParse(Console.ReadLine(), out lowLimit);
            if (lowLimit == -1)
            {
                Console.WriteLine("Bad low limit!");
                return;
            }

            Console.WriteLine("Enter high limit: ");
            double highLimit = -1;
            double.TryParse(Console.ReadLine(), out highLimit);
            if (highLimit == -1)
            {
                Console.WriteLine("Bad high limit!");
                return;
            }

            double initialValue = -1;
            Console.WriteLine("Enter initial value: ");
            double.TryParse(Console.ReadLine(), out initialValue);
            if (initialValue == -1)
            {
                Console.WriteLine("Bad initial value");
                return;
            }

            //todo maybe allow this but set it to the limits?
            if (initialValue < lowLimit || initialValue > highLimit)
            {
                Console.WriteLine("Initial value must be within low and high limit range!");
                return;
            }


            AnalogOutput tag = new AnalogOutput
            {
                Description = description,
                IOAddress = IOAddress,
                DriverType = (DriverType)driverTypeInt,
                InitialValue = initialValue,
                LowLimit = lowLimit,
                HighLimit = highLimit
            };

            proxy.addTag(tag);
            Console.WriteLine("Successfully added new Analog output tag!");
        }


        private static void addAnalogInput()
        {
            Console.WriteLine("Enter description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Enter IOAddress: ");
            string IOAddress = Console.ReadLine();

            Console.WriteLine("Enter 0 for Simulation Driver or 1 for Real Time Driver: ");
            int driverTypeInt = -1;
            int.TryParse(Console.ReadLine(), out driverTypeInt);
            if (driverTypeInt != 0 && driverTypeInt != 1)
            {
                Console.WriteLine("Wrong driver number!");
                return;
            }

            Console.WriteLine("Enter scanTime: ");
            int scanTime = -1;
            int.TryParse(Console.ReadLine(), out scanTime);
            if (scanTime <= 0)
            {
                Console.WriteLine("Bad seconds!");
                return;
            }

            Console.WriteLine("Enter 1 for scanOn = true or 0 for scanOn = false: ");
            int scanOnInt = -1;
            int.TryParse(Console.ReadLine(), out scanOnInt);
            if (scanOnInt != 0 && scanOnInt != 1)
            {
                Console.WriteLine("Bad scanOn!");
                return;
            }
            bool scanOn = scanOnInt == 1 ? true : false;

            Console.WriteLine("Enter low limit: ");
            double lowLimit = -1;
            double.TryParse(Console.ReadLine(), out lowLimit);
            if (lowLimit == -1)
            {
                Console.WriteLine("Bad low limit!");
                return;
            }

            Console.WriteLine("Enter high limit: ");
            double highLimit = -1;
            double.TryParse(Console.ReadLine(), out highLimit);
            if (highLimit == -1)
            {
                Console.WriteLine("Bad high limit!");
                return;
            }

            AnalogInput tag = new AnalogInput
            {
                Description = description,
                IOAddress = IOAddress,
                DriverType = (DriverType)driverTypeInt,
                ScanTime = scanTime,
                ScanOn = scanOn,
                LowLimit = lowLimit,
                HighLimit = highLimit

            };

            proxy.addTag(tag);
            Console.WriteLine("Successfully added new analog input tag!");
        }


        private static void addDigitalOutput()
        {
            Console.WriteLine("Enter description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Enter IOAddress: ");
            string IOAddress = Console.ReadLine();

            Console.WriteLine("Enter 0 for Simulation Driver or 1 for Real Time Driver: ");
            int driverTypeInt = -1;
            int.TryParse(Console.ReadLine(), out driverTypeInt);
            if (driverTypeInt != 0 && driverTypeInt != 1)
            {
                Console.WriteLine("Wrong driver number!");
                return;
            }

            double initialValue = -1;
            Console.WriteLine("Enter initial value: (0 or 1) ");
            double.TryParse(Console.ReadLine(), out initialValue);
            if (initialValue != 0 && initialValue != 1)
            {
                Console.WriteLine("Bad initial value!");
                return;
            }



            DigitalOutput tag = new DigitalOutput
            {
                Description = description,
                IOAddress = IOAddress,
                DriverType = (DriverType)driverTypeInt,
                InitialValue = initialValue
            };

            proxy.addTag(tag);
            Console.WriteLine("Successfully added new Digital output tag!");
        }

        private static void addDigitalInput()
        {
            Console.WriteLine("Enter description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Enter IOAddress: ");
            string IOAddress = Console.ReadLine();

            Console.WriteLine("Enter 0 for Simulation Driver or 1 for Real Time Driver: ");
            int driverTypeInt = -1;
            int.TryParse(Console.ReadLine(), out driverTypeInt);
            if (driverTypeInt != 0 && driverTypeInt != 1)
            {
                Console.WriteLine("Wrong driver number!");
                return;
            }

            Console.WriteLine("Enter scanTime: ");
            int scanTime = -1;
            int.TryParse(Console.ReadLine(), out scanTime);
            Console.WriteLine("Entered scan time is: " + scanTime);
            if (scanTime <= 0)
            {
                Console.WriteLine("Bad seconds!");
                return;
            }

            Console.WriteLine("Enter 1 for scanOn = true or 0 for scanOn = false: ");
            int scanOnInt = -1;
            int.TryParse(Console.ReadLine(), out scanOnInt);
            if (scanOnInt != 0 && scanOnInt != 1)
            {
                Console.WriteLine("Bad scanOn!");
                return;
            }
            bool scanOn = scanOnInt == 1 ? true : false;

            DigitalInput tag = new DigitalInput
            {
                Description = description,
                IOAddress = IOAddress,
                DriverType = (DriverType)driverTypeInt,
                ScanTime = scanTime,
                ScanOn = scanOn
            };

            proxy.addTag(tag);
            Console.WriteLine("Successfully added new Digital input tag!");



        }
    }
}
