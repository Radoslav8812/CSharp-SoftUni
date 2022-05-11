using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _02.Line_Numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            // INPUT:
            //-I was quick to judge him, but it wasn't his fault.
            //- Is this some kind of joke?!Is it ?
            //-Quick, hide here. It is safer.

            // OUTPUT
            //Line 1: -I was quick to judge him, but it wasn't his fault. (37)(4)
            //Line 2: -Is this some kind of joke?!Is it ? (24)(4)
            //Line 3: -Quick, hide here. It is safer. (22)(4)


            var lines = File.ReadAllLines(@"/Users/radoslavbogdanov/Downloads/DEVELOPMENT/C# Projects/Steams And Files/04. CSharp-Advanced-Streams-Files-and-Directories-Exercise-Resources/text.txt");
            var stringsList = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                int letterCount = lines[i].Count(x => char.IsLetter(x));
                int punctuationCount = lines[i].Count(x => char.IsPunctuation(x));

                stringsList.Add($"Line: {i + 1}: {lines[i]} ({letterCount})({punctuationCount})");
            }

            File.WriteAllLines("output.txt", stringsList);
        }
    }
}

