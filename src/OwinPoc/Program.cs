using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace OwinPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.ReadLine();
            }

        }
    }
}