using System;

namespace CodeSample
{
    public class HelloWorld
    {
        public struct aaa{
            public int message;
        } 

        private string message;
        public HelloWorld(string msg)
        {
            message = msg;
        }
        public void PrintMessage()
        {
            // Conditional statement
            if (message != null)
            {
                Console.WriteLine("Message: " + message);
            }
            else
            {
                Console.WriteLine("No message provided.");
            }
        }
        static void Main(string[] args)
        {
            aaa.message = args[0]; 
            HelloWorld hello = new HelloWorld("Hello, C# World!");
            hello.PrintMessage();

            // Example of a loop
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Loop iteration: " + i);
            }

            string sampleString = "This is a string example.";
        }
    }
}
