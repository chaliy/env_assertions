namespace SampleConsole
{
    using System;
    using EnvAssertions.Servers;

    class Program
    {
        static void Main(string[] args)
        {            
            var local = Server.Connect("localhost");

            Console.WriteLine("Free space: {0}", local.Disks["C"].FreeSpace);
        }
    }
}
