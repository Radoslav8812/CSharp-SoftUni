using System;
using System.IO;
using System.Linq;

namespace StreamAndFIles
{
    class Program
    {
        static void Main(string[] args)
        {
            EvenLines();
        }

        private static void EvenLines()
        {
            //Create a program that reads a text file and prints on the console its even lines. Line numbers start from 0.Use StreamReader.Before you print the result replace { "-", ",", ".", "!", "?"} with "@" and reverse the order of the words.

//            text.txt
//             -I was quick to judge him, but it wasn't his fault.
//             - Is this some kind of joke?!Is it ?
//              -Quick, hide here. It is safer.
//
              //Expect result:
//              fault@ his wasn't it but him@ judge to quick was @I
//              safer@ is It here@ hide @Quick@


            StreamReader streamReader = new StreamReader(@"/Users/radoslavbogdanov/Downloads/DEVELOPMENT/C# Projects/Steams And Files/04. CSharp-Advanced-Streams-Files-and-Directories-Exercise-Resources/text.txt"); // get the required file path.
            //string result = streamReader.ReadToEnd(); // read whole lines of the input
            string[] specialSimbols = new[] { "-", ",", ".", "!", "?" };
            bool isEven = true;

            while (true)
            {
                string result = streamReader.ReadLine();

                if (result == null)
                {
                    break;
                }

                if (!isEven)
                {
                    isEven = true;
                    continue;
                }

                foreach (var symbol in specialSimbols)
                {
                    result = result.Replace(symbol, "@");
                }

                Console.WriteLine(string.Join(" ", result.Split().Reverse()));
                isEven = false;
            }
        }
    }
}
