﻿using System;
using System.IO;
using System.Net;
using System.Threading;

namespace PLCRead4
{
    class Program
    {
        static void Main()
        {

            string StartupPath = @".\";
            string Year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.Month.ToString();
            string Day = DateTime.Now.Day.ToString();
            Directory.CreateDirectory(StartupPath + "\\" + Year + "\\" + Month + "\\" + Day);
            string path = Year + @".\" + Month + @"\" + @"\" + Day + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            //start by checking the stacks state
            CheckStacks();
            while (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == false)
            {
                //reinitliaze and check the tags
                CheckStacks();
                LogToFile(path);
                //check if stacks have opened
                //true means the stacks have opened.
                if (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
                {
                    cameraGrab();//inital camera grab
                    //stack is open so now we need to do a while loop
                    while (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
                    {
                        CheckStacks();
                        LogToFile(path);
                        //as long as stacks are open we loop
                        if (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == false)
                        {
                            Thread.Sleep(30 * 1000);
                            LogToFile(path);
                            cameraGrab(); //initial picture 30 seconds after the stacks are closed
                            for (int i = 0; i < 4; i++)
                            {
                                Thread.Sleep(3 * 60 * 1000);
                                cameraGrab();
                                LogToFile(path);
                            }
                            CheckStacks();
                        }
                    }
                    //camera grab
                    cameraGrab();
                    LogToFile(path);
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(3 * 60 * 1000);
                        cameraGrab();
                        LogToFile(path);
                    }
                    CheckStacks();
                }
                CheckStacks();
            }
        }

        // Read the value from the PLC
        private static void CheckStacks()
        {
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
                try
                {
                    PlcClass.dryerStack1.Read();
                }
                catch (libplctag.LibPlcTagException ex)
                {
                    if (ex.Message == "ErrorTimeout")
                    {

                        //debugLog.WriteLine(ex.Message);
                        debugLog.WriteLine("Time out error " + ex.Message + "For Dryer Stack 1 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));

                    }

                    else
                    {
                        debugLog.WriteLine(ex.Message + "For Dryer Stack 1 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                        //Console.WriteLine(ex.Message);
                    }

                }
                try
                {
                    PlcClass.dryerStack2.Read();
                }
                catch (libplctag.LibPlcTagException ex)
                {
                    if (ex.Message == "ErrorTimeout")
                    {
                        debugLog.WriteLine("Time out error " + ex.Message + "For Dryer Stack 2 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                    }
                    else
                    {
                        debugLog.WriteLine(ex.Message + "For Dryer Stack 2 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                    }

                }
                try
                {
                    PlcClass.ventStack1.Read();
                }
                catch (libplctag.LibPlcTagException ex)
                {
                    if (ex.Message == "ErrorTimeout")
                    {
                        debugLog.WriteLine("Time out error " + ex.Message + "For Vent Stack 1 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                    }
                    else
                    {
                        debugLog.WriteLine(ex.Message + "For Vent Stack 1 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                    }

                }
                try
                {
                    PlcClass.ventStack2.Read();
                }
                catch (libplctag.LibPlcTagException ex)
                {
                    if (ex.Message == "ErrorTimeout")
                    {
                        debugLog.WriteLine("Time out error " + ex.Message + "For Vent Stack 2 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                    }
                    else
                    {
                        debugLog.WriteLine(ex.Message + "For Vent Stack 2 at" + DateTime.Now.ToString("yyyy MM dd hh:mm:ss"));
                    }
                }
            }
        }
        private static void cameraGrab()
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create("http://localhost/CameraGrab/index.php");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        private static void LogToFile(string filepath)
        {

            StreamWriter sw;
            if (!File.Exists(filepath))
            {
                using (sw = File.CreateText(filepath))
                {
                    //file is created
                }
            }

                using (sw = File.AppendText(filepath))
                {
                        sw.WriteLine($"Begin Log");

                        if (PlcClass.dryerStack1.Value == false)
                        {
                            sw.WriteLine($"{DateTime.Now} Dryer stack 1 is closed");
                        }
                        else
                        {
                            sw.WriteLine($"{DateTime.Now} Dryer stack 1 is open");
                        }

                        if (PlcClass.dryerStack2.Value == false)
                        {
                            sw.WriteLine($"{DateTime.Now} Dryer stack 2 is closed");
                        }
                        else
                        {
                            sw.WriteLine($"{DateTime.Now} Dryer stack 2 is open");
                        }
                        if (PlcClass.ventStack1.Value == false)
                        {
                            sw.WriteLine($"{DateTime.Now} Furnace stack 1 is closed");
                        }
                        else
                        {
                            sw.WriteLine($"{DateTime.Now} Furnace stack 1 is open");
                        }
                        if (PlcClass.ventStack2.Value == false)
                        {
                            sw.WriteLine($"{DateTime.Now} Furnace stack 2 is closed");
                        }
                        else
                        {
                            sw.WriteLine($"{DateTime.Now} Furnace stack 2 is open");
                        }
                    }
                }
            }
        
    }

