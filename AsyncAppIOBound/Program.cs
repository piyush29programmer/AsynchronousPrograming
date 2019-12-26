﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncAppIOBound
{
    class Program
    {
        private const string URL = "https://docs.microsoft.com/en-us/dotnet/csharp/csharp";
        static void Main(string[] args)
        {
            DoSynchronousWork();
            var someTask = DoSomethingAsync();
            DoSynchronousWorkAfterAwait(); // This method will resume after line number - 42 and DoSomethingAsync() can resume anytime once its complete httpClient.GetStringAsync(URL) call.
                                           // Await keyword will left a marker so that rest of that program can be resume and once async call complete then it will send a callback thread to complete that. 

            Console.ReadLine();


            //DoSynchronousWork();
            //var someTask = DoSomethingAsync(); This method will be in await mode until call to  httpClient.GetStringAsync(URL) complete and left a marker so that rest of the program can resume.
            //someTask.Wait();    // This method will make sure that rest of the program will not continue until DoSomethingAsync(); completes.

            //DoSynchronousWorkAfterAwait();
            //Console.ReadLine();
        }

        public static void DoSynchronousWork()
        {
            // You can do whatever work is needed here
            Console.WriteLine("1. Doing some work synchronously");
        }

        static async Task DoSomethingAsync() //A Task return type will eventually yield a void
        {
            Console.WriteLine("2. Async task has started...");
            await GetStringAsync(); // we are awaiting the Async Method GetStringAsync
        }

        static async Task GetStringAsync()
        {
            using (var httpClient = new HttpClient())
            {
                Console.WriteLine("3. Awaiting the result of GetStringAsync of Http Client...");
                string result = await httpClient.GetStringAsync(URL); //execution pauses here while awaiting GetStringAsync to complete

                //From this line and below, the execution will resume once the above awaitable is done
                //using await keyword, it will do the magic of unwrapping the Task<string> into string (result variable)
                Console.WriteLine("4. The awaited task has completed. Let's get the content length...");
                Console.WriteLine($"5. The length of http Get for {URL}");
                Console.WriteLine($"6. {result.Length} character");
            }
        }

        static void DoSynchronousWorkAfterAwait()
        {
            //This is the work we can do while waiting for the awaited Async Task to complete
            Console.WriteLine("7. While waiting for the async task to finish, we can do some unrelated work...");
            for (var i = 0; i <= 5; i++)
            {
                for (var j = i; j <= 5; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }

        }
    }
}
