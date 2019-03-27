using System;
using System.Collections.Generic;
using System.IO;

namespace Lesson1
{
    class Program
    {
        static void Main(string[] args)
        {
            AllocateAllMemory();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            AllocateObjectInMemory();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.ReadLine();
        }

        public static void AllocateAllMemory()
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
                        Console.WriteLine("Max heap size {0} bytes", GC.GetTotalMemory(false));
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
                    Console.WriteLine("Max object size {0} bytes", GC.GetTotalMemory(false) - memoryBefore);
                    break;
                }
            }
        }
    }
}
