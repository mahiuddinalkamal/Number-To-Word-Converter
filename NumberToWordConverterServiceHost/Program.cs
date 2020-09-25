using System;
using System.ServiceModel;

namespace NumberToWordConverterServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(NumberToWordConverterService.NumberToWordConverterService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Host Started...@" + DateTime.Now.ToString());
                    Console.ReadLine();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error occured! Please open the solution again in Visual Studio with Admin priviledge");
                }
            }
        }
    }
}
