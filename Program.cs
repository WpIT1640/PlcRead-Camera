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
            

            var dryerStack1 = new Tag<BoolPlcMapper, bool>()
            {
                Name = "E321531EVO01.ZSO",
                Gateway = "172.30.201.3",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5)
            };
            var dryerStack2 = new Tag<BoolPlcMapper, bool>()
            {
                Name = "E322531EVO01.ZSO",
                Gateway = "172.30.201.3",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5)
            };
            var ventStack1 = new Tag<BoolPlcMapper, bool>()
            {
                Name = "D301070EV01.ZSO",
                Gateway = "172.30.201.2",
                Path = "1,1",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5)
            };
            var ventStack2 = new Tag<BoolPlcMapper, bool>()
            {
                Name = "D302070EV01.ZSO",
                Gateway = "172.30.201.2",
                Path = "1,1",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Read the value from the PLC
            //TODO error check and retry the read.
            void CheckStacks()
            {
                try
                {
                    dryerStack1.Read();
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
                    dryerStack2.Read();
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
                    ventStack1.Read();
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
                    ventStack2.Read();
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
            string path = @".\test.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine($"{DateTime.UtcNow}   {dryerStack1.Value}");
                    sw.WriteLine($"{DateTime.UtcNow}   {dryerStack2.Value}");
                    sw.WriteLine($"{DateTime.UtcNow}   {ventStack1.Value}");
                    sw.WriteLine($"{DateTime.UtcNow}   {ventStack2.Value}");
                }
            }

                        

            if (dryerStack1.Value | dryerStack2.Value | ventStack1.Value | ventStack2.Value == true)
            {
                //stack is open so now we need to do a while loop
                while (dryerStack1.Value | dryerStack2.Value | ventStack1.Value | ventStack2.Value == true)
                {
                    CheckStacks();
                    //as long as stacks are open we loop
                }
                //camera grab
                Thread.Sleep(30 * 1000);
                cameraGrab();
                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(5 * 60 * 1000);
                    cameraGrab();                
                }
                CheckStacks();
            }
            CheckStacks();

            while (dryerStack1.Value | dryerStack2.Value | ventStack1.Value | ventStack2.Value == false)
            {
                //TODO reinitliaze and check the tags
                Console.WriteLine("Stack is closed \n");
                CheckStacks();        
                //check if stacks have opened
                //true means the stacks have opened.
                if (dryerStack1.Value | dryerStack2.Value | ventStack1.Value | ventStack2.Value == true)
                {
                    //stack is open so now we need to do a while loop
                    while (dryerStack1.Value | dryerStack2.Value | ventStack1.Value | ventStack2.Value == true)
                    {
                        CheckStacks();
                        //as long as stacks are open we loop
                    }
                    //camera grab
                    cameraGrab();
                    for (int i = 0; i < 3; i++)
                    {
                        Thread.Sleep(5 * 60 * 1000);
                        cameraGrab();
                    }
                    CheckStacks();
                }
                CheckStacks();
            }

        }

     }
}
