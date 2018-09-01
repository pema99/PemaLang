using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Interpreter Interpreter = new Interpreter();

            try
            {
                if (args.Length == 0)
                {
                    throw new Exception("No program passed in as command line argument.");
                }
                Interpreter.DoString(File.ReadAllText(args[0]));
                Interpreter.Call("main");
            }
            catch (Exception E)
            {
                Console.WriteLine("\n"+E.Message);
            }

            Console.WriteLine("\nProgram terminated. Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}
