using Revamp.IO.Structs.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NickStrupat;

namespace Revamp.IO.Helpers.Helpers
{
    public class ER_PerformanceMonitor
    {

        public string[] getCurrentCpuUsage(string[] ProcessorList, int HowMany)
        {

            PerformanceCounter theCPUCounter =
            new PerformanceCounter("Processor", "% Processor Time", "_Total");

            for (int i = 0; i < HowMany; i++)
            {
                double ram;
                try
                {
                    if (i % 5 == 0)
                    {
                        Thread.Sleep(100);
                    }
                    ram = theCPUCounter.NextValue();
                    ProcessorList[i] = ((ram).ToString());
                }
                catch
                {
                    ProcessorList[i] = "0";
                }
            }

            return ProcessorList;
        }

        public List<currentMemory> getAvailableRAM(List<currentMemory> MemoryList, int HowMany)
        {
            Process p = Process.GetCurrentProcess();
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            //PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", p.ProcessName);

            for (int i = 0; i < HowMany; i++)
            {
                if (i % 5 == 0)
                {
                    Thread.Sleep(100);
                }
                
                double ram = ramCounter.NextValue();

                MemoryList.Add(new currentMemory { _num = i.ToString(), _result = (ram).ToString() });

                //_ReturnArray[i, 0] = i.ToString();
                //_ReturnArray[i, 1] = (ram / 1024 / 1024).ToString(); 

                //double cpu = cpuCounter.NextValue();
                //Console.WriteLine("RAM: "+(ram/1024/1024)+" MB; CPU: "+(cpu)+" %");
            }
            return MemoryList;
        }

        public ulong GetTotalMemoryInBytes()
        {
            var ci = new ComputerInfo();
            return ci.TotalPhysicalMemory;
        }

        public string[] getAvailableRAM(string[] MemoryList, int HowMany)
        {


            Process p = Process.GetCurrentProcess();
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            //PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", p.ProcessName);

            for (int i = 0; i < HowMany; i++)
            {
                double ram;
                try
                {
                    Thread.Sleep(200);
                    ram = ramCounter.NextValue();

                    MemoryList[i] = ((ram).ToString());
                }
                catch
                {
                    MemoryList[i] = "0";
                }

                //_ReturnArray[i, 0] = i.ToString();
                //_ReturnArray[i, 1] = (ram / 1024 / 1024).ToString(); 

                //double cpu = cpuCounter.NextValue();
                //Console.WriteLine("RAM: "+(ram/1024/1024)+" MB; CPU: "+(cpu)+" %");
            }
            return MemoryList;
        }

        public string GetProcessingUsagebyName(string ProcessName)
        {

            PerformanceCounter theCPUCounter =
                new PerformanceCounter("Process", "% Processor Time",
                    Process.GetCurrentProcess().ProcessName);

            return theCPUCounter.NextValue() + "%";

        }

        //public string[,] getRAMUsagebyName(string Name, string machineName ){

        //    Process[] p = Process.GetProcessesByName(Name);

        //    string[,] ps = new string[p.Length, p.Length];

        //    foreach(Process pi in p)
        //    {
        //        PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set", pi.ProcessName);
        //        PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", pi.ProcessName);
        //        //while (true)
        //       //{
        //            Thread.Sleep(500);
        //            double ram = ramCounter.NextValue();
        //            double cpu = cpuCounter.NextValue();
        //            //Console.WriteLine("RAM: "+(ram/1024/1024)+" MB; CPU: "+(cpu)+" %");
        //    //}
        //    }

        //    return ("RAM: "+(ram/1024/1024)+" MB; CPU: "+(cpu)+" %");
        //}




    }
}