using System;
using System.IO;

namespace rot1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Quick user guide:
            /*
                You can expecify the file on the terminal with the path of the file as argument
                Otherwise:
                Make a script on folder ./scripts
                Script extension need to be .cl
                input the name of the script on the console when prompted
             */

             
            //get filepath from arg or terminal input
            string filepath;
            if(args.Length <= 2){
                Console.WriteLine("No file selected.\nType the file name: (in folder scripts, without the '.cl' extension) ");
                filepath = "./scripts/" + Console.ReadLine() + ".cl";
            } else {
                filepath = args[1];
            }
            if(!File.Exists(filepath)){
                Console.WriteLine("File don't exist!\nExiting");
                return;
            }

            //read all code and buffer the text
            string code = File.ReadAllText(filepath);

            //flag for debug prints
            bool debug = true;

            //run code and check
            Console.WriteLine("Running script!\n");
            Writer.WriteHeader();
            int result_sign = Parser.Run(code,debug);
            Writer.WriteFile();
            if(result_sign != 1){
                Console.WriteLine($"Script returned an unsuccessful sign. [Code - {result_sign}]");
            }
            Console.WriteLine("\nDone");

        }
    }
}
