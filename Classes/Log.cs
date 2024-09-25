using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Classes
{
    public class Log
    {
        public static void Normal(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[MSG] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[MSG] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[MSG] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[MSG] - {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
