using System;
using System.IO;
using System.Net;
using System.Threading;

namespace PLCRead4
{
    class Program
    {
        static void Main()
        {
            PlcClass.listOfTags.Add(PlcClass.dryerStack1);
            PlcClass.listOfTags.Add(PlcClass.dryerStack2);
            PlcClass.listOfTags.Add(PlcClass.ventStack1);
            PlcClass.listOfTags.Add(PlcClass.ventStack2);

            string StartupPath = @".\";
            string Year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.Month.ToString();
            string Day = DateTime.Now.Day.ToString();
            Directory.CreateDirectory(StartupPath + "\\" + Year + "\\" + Month + "\\" + Day);
            string path = Year + @".\" + Month + @"\" + @"\" + Day + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string[] tagNames;
            tagNames = new string[4] { "Dryer1", "Dryer2", "Furnace Stack1", "Furnace Stack2" };
            //start by checking the stacks state
            CheckStacks(tagNames);
            while (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == false)
            {
                //reinitliaze and check the tags
                CheckStacks(tagNames);
                LogToFile(path, tagNames);
                //check if stacks have opened
                //true means the stacks have opened.
                if (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
                {
                    cameraGrab();//inital camera grab
                    //stack is open so now we need to do a while loop
                    while (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
                    {
                        CheckStacks(tagNames);
                        LogToFile(path, tagNames);
                        //as long as stacks are open we loop
                        if (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == false)
                        {
                            Thread.Sleep(30 * 1000);
                            LogToFile(path, tagNames);
                            cameraGrab(); //initial picture 30 seconds after the stacks are closed
                            for (int i = 0; i < 4; i++)
                            {
                                Thread.Sleep(3 * 60 * 1000);
                                cameraGrab();
                                LogToFile(path, tagNames);
                            }
                            CheckStacks(tagNames);
                        }
                    }
                    //camera grab
                    cameraGrab();
                    LogToFile(path, tagNames);
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(3 * 60 * 1000);
                        cameraGrab();
                        LogToFile(path, tagNames);
                    }
                    CheckStacks(tagNames);
                }
                CheckStacks(tagNames);
            }
        }

        // Read the value from the PLC
        private static void CheckStacks(string[] tags)
        {
            string[] tagNames = tags;
            int i;
            String dateToday = DateTime.Now.ToString("dd.MM.yyy");
            //Setup Debugging
            string debugPath = @".\ " + dateToday + ".txt";
            StreamWriter debugLog;
            if (!File.Exists(debugPath))
            {
                using (debugLog = File.CreateText(debugPath))
                {
                    if (File.Exists(debugPath))
                    {
                        debugLog.WriteLine($"Begin debug log.");
                    }
                }
            }
            using (debugLog = File.AppendText(@".\ " + dateToday + ".txt"))
            {
                for (i = 0; i < tagNames.Length; i++)
                {
                    try
                    {
                        PLCRead4.PlcClass.listOfTags[i].Read();
                    }
                    catch (libplctag.LibPlcTagException ex)
                    {
                        if (ex.Message == "ErrorTimeout")
                        {
                            debugLog.WriteLine($"Time out error  {ex.Message} For {tagNames[i]} at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                        }

                        else
                        {
                            debugLog.WriteLine(ex.Message + $"For {tagNames[i]} at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                        }
                    }
                }
            }
        }
        private static void cameraGrab()
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create("http://localhost/CameraGrab/index.php");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        private static void LogToFile(string filepath, string[] tags)
        {
            string[] tagNames = tags;
            int i;
            StreamWriter sw;
            if (!File.Exists(filepath))
            {
                using (sw = File.CreateText(filepath))
                {
                    sw.WriteLine($"Begin Log");
                    //file is created
                }
            }

            using (sw = File.AppendText(filepath))
            {

                for (i = 0; i < tagNames.Length; i++)
                {
                    if (PlcClass.listOfTags[i].Value == false)
                    {
                        sw.WriteLine($"{DateTime.Now} {tagNames[i]} is closed");
                    }
                    else
                    {
                        sw.WriteLine($"{DateTime.Now} {tagNames[i]} is open");
                    }
                }
            }
        }
    }
}

