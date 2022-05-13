using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Word_Count
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a program that reads a list of words from the file words.txt and finds how many times each of the words is contained in another
            // file text.txt.Matching should be case-insensitive.Write the results in file actualResults.txt.Sort the words by frequency in descending order,
            // read the expected results from expectedResult.txt and then compare your actual and expected results.Use the File class.


            //words.txt:    text.txt:                                                   actualResult.txt:     expecteResult:
            //quick         /  -I was quick to judge him, but it wasn't his fault.   /  quick - 2          /  is - 3
            //is            /  -Is this some kind of joke?! Is it?                   /  is - 3             /  quick - 2
            //fault         /  -Quick, hide here. It is safer.                       /  fault - 1          /  fault - 1


            var wordsDict = new Dictionary<string, int>();
            var words = File.ReadAllLines(@"/Users/radoslavbogdanov/Downloads/DEVELOPMENT/C# Projects/Steams And Files/04. CSharp-Advanced-Streams-Files-and-Directories-Exercise-Resources/words.txt");
            var textLines = File.ReadAllLines("/Users/radoslavbogdanov/Downloads/DEVELOPMENT/C# Projects/Steams And Files/04. CSharp-Advanced-Streams-Files-and-Directories-Exercise-Resources/text.txt");


            foreach (var item in words)
            {
                if (!wordsDict.ContainsKey(item))
                {
                    wordsDict.Add(item, 0);
                }
            }

            foreach (var text in textLines)
            {
                var currWords = text.Split();

                foreach (var word in currWords)
                {
                    if (wordsDict.ContainsKey(word.ToLower()))
                    {
                        wordsDict[word.ToLower()]++;
                    }
                }
            }

            foreach (var item in wordsDict.OrderByDescending(x => x.Value))
            {
                var result = $"{item.Key} - {item.Value}{Environment.NewLine}";
                File.AppendAllText("actualResult.txt", result);
            }
        }
    }
}

