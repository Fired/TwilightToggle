﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace TwilightToggle
{
    public class TwilightToggle
    {
        public String Main(String passedUserDayTime, String passedUserNightTime)
        {
            ChromeHelper chromeHelper = new ChromeHelper();
            FileHelper fileHelper = new FileHelper();
            bool chromeRunState;
            int sleepyTime = 1000;
            string userName = "";
            string localStateLocation = "";
            string readLocalState = "";
            string localStateLocator = "enable-force-dark@";

            string readUserDayTime = "";
            string readUserNightTime = "";

            try
            {
                //gets the current Windows user
                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1]; ;

                localStateLocation = "C:/Users/" + userName + "/AppData/Local/Google/Chrome/User Data/Local State";
                Console.WriteLine("Current user Local State directory: " + localStateLocation);

                readLocalState = fileHelper.readFileToString(localStateLocation);
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.ToString());
            }

            while (true)
            {
                DateTime currentDateTime = DateTime.Now;
                string currentDateTimeString = currentDateTime.ToString();
                int currentDateTimeHourInt = currentDateTime.Hour;
                //some date time logic if statement here to nest everything below into..

                //main variable
                chromeRunState = chromeHelper.GetChromeRunState();
                if (chromeRunState)
                {
                    Console.WriteLine("Chrome is currently running...");
                    DateTime now = DateTime.Now;
                    Console.WriteLine(now);
                    readLocalState = fileHelper.readFileToString(localStateLocation);
                    //read LastState.config here
                    //-logic to see if its time for the user to close chrome so TT can write
                    //popup reminding user to close chrome
                }
                
                else
                {
                    try
                    {
                        
                        if (readLocalState.Contains(localStateLocator))
                        {
                            Console.WriteLine("Attempting write to user's Local State file...");
                            //find substring "enable-force-dark", jump x amount of chars to number flag, create two substrings, 0-->firstCharOfFlag        then    after_flag-->endOfString .     Finally, do Substring1 + desiredFlag + Substring2 and write to Local State

                            int identifierPosition = readLocalState.LastIndexOf(localStateLocator);
                            //Console.WriteLine("Position: " + readLocalState.LastIndexOf("enable-force-dark@"));
                            
                            string writeLocalStatePartOne = (readLocalState.Substring(0, identifierPosition + 18));
                            //Console.WriteLine(writeLocalStatePartOne);

                            string writeLocalStatePartTwo = readLocalState.Substring(writeLocalStatePartOne.Length + 1, readLocalState.Length - (writeLocalStatePartOne.Length + 1));
                            //Console.WriteLine(writeLocalStatePartTwo);

                            string writeLocalStateFinal = writeLocalStatePartOne + "3" + writeLocalStatePartTwo;

                            //write to user's Local State
                            using (StreamWriter outputFile = new StreamWriter(localStateLocation))
                            {
                                outputFile.WriteLine(writeLocalStateFinal);
                            }

                            //after successful write to file, change sleepyTime so the thread now waits for 45 seconds at the end instead of only 1000ms
                            sleepyTime = 45000;
                        }
                        else
                        {
                            Console.WriteLine("[!] FATAL! Could NOT find enable-force-dark flag in user's Local State : " + localStateLocation + " ! Skipping write to Local State file!!!");
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("[!] Fatal exception, please report this on GitHub : " + e);
                        Console.WriteLine(e);
                    }
                }
                Thread.Sleep(sleepyTime);
            }
            
            return "reached the end of TwilightToggle.Main()";
        }

    }
}
