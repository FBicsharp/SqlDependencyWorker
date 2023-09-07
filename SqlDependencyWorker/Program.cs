
using System;
namespace SqlDependencyWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start listening!");
            SQL_Event_monitor service = new SQL_Event_monitor("<MyConnection string>","<myquery>");			

			service.StartAll();
            Console.WriteLine("Listening started!, premi un tasto per terminare ");
            Console.ReadLine();
        }



    }
}
