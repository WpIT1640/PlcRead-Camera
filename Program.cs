using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

/*Version 1.6
 * Added an if test in the while loop to detect if a vent has closed, upon closure we start taking photos.
 * Then we break out of the while loop and take more photos
 * 
 * Photos, photos, everywhere!
 */
/*
Tags would be:
Emergency stacks PLC is 1756-L72 ControlLogix5572, IP 172.30.201.3
dryer 1 stack is opened E321531EVO01.ZSO
dryer 1 stack is closed E321531EVO01.ZSC
dryer 2 stack is opened E322531EVO01.ZSO
dryer 2 stack is closed E322531EVO01.ZSC

Vent stacks PLC is 1756-L71 ControlLogix5571, IP 172.30.201.2
dryer 1 stack is opened D301070EV01.ZSO
dryer 1 stack is closed D301070EV01.ZSC
dryer 2 stack is opened D302070EV01.ZSO
dryer 2 stack is closed D302070EV01.ZSC
*/

namespace PLCRead4
{
        class Program
    {
      
        static void Main()
        {
             // Read the value from the PLC
            //TODO error check and retry the read.
            void CheckStacks()
            {
                try
                {
                    PlcClass.dryerStack1.Read();
                }
                catch (libplctag.LibPlcTagException ex)
                {
                    if (ex.Message == "ErrorTimeout")
                    {
                        Console.WriteLine("Time out error " + ex.Message + "\n");
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
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
                        Console.WriteLine("Time out error " + ex.Message + "\n");
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
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
                        Console.WriteLine("Time out error " + ex.Message + "\n");
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
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
                        Console.WriteLine("Time out error " + ex.Message + "\n");
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
             }
            // Output to Console
            CheckStacks();
            void cameraGrab()
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create("http://localhost/CameraGrab/index.php");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }

            void logToFile()
            {
                string StartupPath = @".\";
                string Year = DateTime.Now.Year.ToString();
                string Month = DateTime.Now.Month.ToString();
                string Day = DateTime.Now.Day.ToString();
                Directory.CreateDirectory(StartupPath + "\\" + Year + "\\" + Month + "\\" + Day);

                string path = Year + @".\" + Month + @"\" + @"\" + Day + @"\" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
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
                            sw.WriteLine($"{DateTime.Now} Vent stack 1 is closed");
                        }
                        else
                        {
                            sw.WriteLine($"{DateTime.Now} Vent stack 1 is open");
                        }
                        if (PlcClass.ventStack2.Value == false)
                        {
                            sw.WriteLine($"{DateTime.Now} Vent stack 2 is closed");
                        }
                        else
                        {
                            sw.WriteLine($"{DateTime.Now} Vent stack 2 is open");
                        }
                    }
                }
            }
            
            if ( PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
            {
                cameraGrab(); //first picture when the stacks open
                //stack is open so now we need to do a while loop
                while (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
                {
                    CheckStacks();
                    logToFile();
                    //as long as stacks are open we loop
                    if (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == false)
                    {
                        Thread.Sleep(30 * 1000);
                        logToFile();
                        cameraGrab(); //initial picture 30 seconds after the stacks are closed
                        for (int i = 0; i < 4; i++)
                        {
                            Thread.Sleep(3 * 60 * 1000);
                            cameraGrab();
                            logToFile();
                        }
                        CheckStacks();
                    }
                }
                //stacks have closed
              
                Thread.Sleep(30 * 1000);
                logToFile();
                cameraGrab(); //initial picture 30 seconds after the stacks are closed
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(3 * 60 * 1000);
                    cameraGrab();
                    logToFile();
                }
                CheckStacks();
            }
            CheckStacks();

            while (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == false)
            {
                //reinitliaze and check the tags
                CheckStacks();        
                //check if stacks have opened
                //true means the stacks have opened.
                if (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
                {
                    cameraGrab();//inital camera grab
                    //stack is open so now we need to do a while loop
                    while (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == true)
                    {
                        CheckStacks();
                        logToFile();
                        //as long as stacks are open we loop
                        if (PlcClass.dryerStack1.Value | PlcClass.dryerStack2.Value | PlcClass.ventStack1.Value | PlcClass.ventStack2.Value == false)
                        {
                            Thread.Sleep(30 * 1000);
                            logToFile();
                            cameraGrab(); //initial picture 30 seconds after the stacks are closed
                            for (int i = 0; i < 4; i++)
                            {
                                Thread.Sleep(3 * 60 * 1000);
                                cameraGrab();
                                logToFile();
                            }
                            CheckStacks();
                        }
                    }
                    //camera grab
                    cameraGrab();
                    logToFile();
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(3 * 60 * 1000);
                        cameraGrab();
                        logToFile();
                    }
                    CheckStacks();
                }
                CheckStacks();
            }
            
        }

     }
}
