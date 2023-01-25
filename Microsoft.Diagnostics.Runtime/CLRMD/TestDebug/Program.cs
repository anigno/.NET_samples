using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime;

namespace TestDebug
{
    class Program
    {
        public Program()
        {
            DataTarget dataTarget = DataTarget.AttachToProcess(7576, 1000, AttachFlag.NonInvasive);
            IList<ClrInfo> clrVersions = dataTarget.ClrVersions;
            foreach (ClrInfo clrInfo in clrVersions)
            {
                Console.WriteLine("Found CLR Version:" + clrInfo.Version.ToString());

                // This is the data needed to request the dac from the symbol server:
                ModuleInfo dacInfo = clrInfo.DacInfo;
                Console.WriteLine("Filesize:  {0:X}", dacInfo.FileSize);
                Console.WriteLine("Timestamp: {0:X}", dacInfo.TimeStamp);
                Console.WriteLine("Dac File:  {0}", dacInfo.FileName);

                // If we just happen to have the correct dac file installed on the machine,
                // the "TryGetDacLocation" function will return its location on disk:
                string dacLocation = clrInfo.TryGetDacLocation();
                if (!string.IsNullOrEmpty(dacLocation))
                    Console.WriteLine("Local dac location: " + dacLocation);

                // You may also download the dac from the symbol server, which is covered
                // in a later section of this tutorial.
            }
            ClrInfo runtimeInfo = dataTarget.ClrVersions[0];  // just using the first runtime
            ClrRuntime runtime = runtimeInfo.CreateRuntime();

            //create a runtime from a dac location 
            //ClrInfo runtimeInfo = dataTarget.ClrInfo[0];  // just using the first runtime
            //ClrRuntime runtime = runtimeInfo.CreateRuntime(@"C:\work\mscordacwks.dll");

            Console.WriteLine("runtime.ServerGC={0}", runtime.ServerGC);
            Console.WriteLine("runtime.HeapCount={0}", runtime.HeapCount);

            Console.WriteLine("AppDomains");

            foreach (ClrAppDomain domain in runtime.AppDomains)
            {
                Console.WriteLine("ID:      {0}", domain.Id);
                Console.WriteLine("Name:    {0}", domain.Name);
                Console.WriteLine("Address: {0}", domain.Address);
            }

            foreach (ClrThread thread in runtime.Threads)
            {
                if (!thread.IsAlive)
                    continue;

                Console.WriteLine("Thread {0:X}:", thread.OSThreadId);

                foreach (ClrStackFrame frame in thread.StackTrace)
                {
                    Console.WriteLine("StackPtr:{0,12:X} InstPtr:{1,12:X} frame:{2}", frame.StackPointer, frame.InstructionPointer, frame.ToString());
                }

                Console.WriteLine();
            }

            foreach (var region in (from r in runtime.EnumerateMemoryRegions()
                                    where r.Type != ClrMemoryRegionType.ReservedGCSegment
                                    group r by r.Type into g
                                    let total = g.Sum(p => (uint)p.Size)
                                    orderby total descending
                                    select new
                                    {
                                        TotalSize = total,
                                        Count = g.Count(),
                                        Type = g.Key
                                    }))
            {
                Console.WriteLine("{0,6:n0} {1,12:n0} {2}", region.Count, region.TotalSize, region.Type.ToString());
            }

            foreach (ulong obj in runtime.EnumerateFinalizerQueueObjectAddresses())
            {
            }

            EnumerateHandlers(runtime);
            Heap01(runtime);
            Heap02(runtime);
            Heap03(runtime);
            dataTarget.Dispose();
        }

        public void Heap03(ClrRuntime p_runtime)
        {
            Console.WriteLine("******** Heap03");

            ClrHeap heap = p_runtime.GetHeap();

            if (!heap.CanWalkHeap)
            {
                Console.WriteLine("Cannot walk the heap!");
            }
            else
            {
                foreach (ulong obj in heap.EnumerateObjects())
                {
                    ClrType type = heap.GetObjectType(obj);

                    // If heap corruption, continue past this object.
                    if (type == null)
                        continue;

                    ulong size = type.GetSize(obj);
                    Console.WriteLine("{0,12:X} {1,8:n0} {2,1:n0} {3}", obj, size, heap.GetGeneration(obj), type.Name);
                }
            }
        }

        public void Heap02(ClrRuntime p_runtime)
        {
            Console.WriteLine("Heap02");
            var heap = p_runtime.GetHeap();

            if (!heap.CanWalkHeap)
            {
                Console.WriteLine("Cannot walk the heap!");
            }
            else
            {
                foreach (ClrSegment seg in heap.Segments)
                {
                    for (ulong obj = seg.FirstObject; obj != 0; obj = seg.NextObject(obj))
                    {
                        ClrType type = heap.GetObjectType(obj);

                        // If heap corruption, continue past this object.
                        if (type == null)
                            continue;

                        ulong size = type.GetSize(obj);
                        Console.WriteLine("{0,12:X} {1,8:n0} {2,1:n0} {3}", obj, size, seg.GetGeneration(obj), type.Name);
                    }
                }
            }
        }

        public void Heap01(ClrRuntime p_runtime)
        {
            ClrHeap heap = p_runtime.GetHeap();
            Console.WriteLine("{0,12} {1,12} {2,12} {3,12} {4,4} {5}", "Start", "End", "Committed", "Reserved", "Heap", "Type");
            foreach (var segment in heap.Segments)
            {
                string type;
                if (segment.Ephemeral)
                    type = "Ephemeral";
                else if (segment.Large)
                    type = "Large";
                else
                    type = "Gen2";

                Console.WriteLine("{0,12:X} {1,12:X} {2,12:X} {3,12:X} {4,4} {5}", segment.Start, segment.End, segment.Committed, segment.Reserved, segment.ProcessorAffinity, type);
            }
            foreach (var item in (from seg in heap.Segments
                                  group seg by seg.ProcessorAffinity into g
                                  orderby g.Key
                                  select new
                                  {
                                      Heap = g.Key,
                                      Size = g.Sum(p => (uint)p.Length)
                                  }))
            {
                Console.WriteLine("Heap {0,2}: {1:n0} bytes", item.Heap, item.Size);
            }
        }

        public void EnumerateHandlers(ClrRuntime p_runtime)
        {
            foreach (ClrHandle handle in p_runtime.EnumerateHandles())
            {
                string objectType = p_runtime.GetHeap().GetObjectType(handle.Object).Name;
                Console.WriteLine("{0,12:X} {1,12:X} {2,12} {3} {4}", handle.Address, handle.Object, handle.Type.ToString(), objectType, handle.RefCount);
            }
        }



        static void Main(string[] args)
        {
            Program p = new Program();
            GC.KeepAlive(p);
            Console.ReadLine();
        }
    }
}
