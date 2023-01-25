using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;
using Microsoft.Diagnostics.Runtime.Interop;

namespace ClrMdTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] processes = Process.GetProcesses().OrderBy(p=>p.ProcessName).ToArray();
            Process testedProcess = processes.Single(p => p.ProcessName.ToUpper().Contains("ThreadsLockTest".ToUpper()));
            using (DataTarget dataTarget = DataTarget.AttachToProcess(testedProcess.Id, 10*1000, AttachFlag.Invasive))
            {
                ClrRuntime runtime =dataTarget.ClrVersions[0].CreateRuntime();

                foreach (var t in runtime.Threads)
                {
                    Console.WriteLine("--------{0}",t.ManagedThreadId);
                    Console.WriteLine("Current exception: {0}",t.CurrentException);
                    foreach (var bo in t.BlockingObjects)
                    {
                        Console.WriteLine("Blocking object: {0}", bo.Object);
                    }
                }


            }


            Console.ReadLine();
        }
    }
}
