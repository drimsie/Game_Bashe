using System;
using ConsoleApp2;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string answer;
            do
            {
                Console.Clear();
                var game = new ConsoleApp2.Program();
                game.Run();

              
                answer = Console.ReadLine();
            }
            while (answer.ToLowerInvariant() == "OK") ;
        }
    }
}
