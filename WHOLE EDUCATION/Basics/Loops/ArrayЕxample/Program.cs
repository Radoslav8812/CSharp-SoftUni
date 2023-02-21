using System;

namespace ArrayЕxample
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 10;

            for (int i = 0; i < n; i++)
            {
                var num = i + 1;

                if (num % 2 != 0 || num == 10)
                {
                    continue;
                }

                Console.WriteLine(num);
            }
        }
    }
}

