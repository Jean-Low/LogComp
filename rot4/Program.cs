using System;

namespace rot1
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "--+-(((+3) * (-(3) + 1)) + 12 / (6 * -1))"; //insert expression here!

            Console.WriteLine("Running program");
            int result = Parser.Run(input);
            Console.WriteLine("resultado = " + result);
            //Stress test i used while coding. left here for test easyness (does easyness is a real word?)
            /*
            Console.WriteLine("STRESS TEST  -  Exprected outputs: 2 - 3 - 1 - 0 - 66 - 1011 - 4095 - 228");
            result = Parser.Run("  2 + 1+2-  3"); // 2
            Console.WriteLine(result);
            result = Parser.Run("1+2"); //3
            Console.WriteLine(result);
            result = Parser.Run("3-2"); //1
            Console.WriteLine(result);
            result = Parser.Run("1+2-3"); //0
            Console.WriteLine(result);
            result = Parser.Run("11+22+33"); //66
            Console.WriteLine(result);
            result = Parser.Run("789    +345   -     123"); //1011
            Console.WriteLine(result);
            result = Parser.Run("1+2+4+8+16+32+64+128+256+512+1024+2048"); //4095
            Console.WriteLine(result);
            result = Parser.Run(" 100 +100 +100 -100 +50 +50 -25 -25    -    2   5 + 3"); //228
            Console.WriteLine(result);
            */

        }
    }
}
