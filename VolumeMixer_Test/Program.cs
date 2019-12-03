using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolumeMixer_Lib;

namespace VolumeMixer_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            VolumeMixer serialComms = new VolumeMixer();
            serialComms.Start("COM2");
            Console.WriteLine("Any key...");
            Console.ReadLine();
            serialComms.Stop();

        }

    }
}