using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lesson1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available memory: {0} MB",
                new PerformanceCounter("Memory", "Available MBytes").NextValue());

            AllocateMemory();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            AllocateObjectInMemory();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.ReadLine();
        }

        public static void AllocateMemory()
        {
            using (var memoryStream = new MemoryStream())
            {
                while (true)
                {
                    try
                    {
                        memoryStream.WriteByte(default(byte));

                    }
                    catch (OutOfMemoryException)
                    {
                        Console.WriteLine("Memory stream allocation {0} bytes", GC.GetTotalMemory(false));
                        break;
                    }
                }
            }
            
           
        }
        
        public static void AllocateObjectInMemory()
        {
            var memoryBefore = GC.GetTotalMemory(false);
            var list = new List<byte[]>();
            while (true)
            {
                try
                {
                    var bytes = new byte[1024*1024]; // 1 MB
                    list.Add(bytes);
                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine("List allocation {0} bytes", GC.GetTotalMemory(false) - memoryBefore);
                    break;
                }
            }
        }
    }
}
