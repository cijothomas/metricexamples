using Prometheus;
using System;
using System.Threading;

namespace GroceryStore
{
    class Program
    {
        private static readonly Counter TickTock =
           Metrics.CreateCounter("sampleapp_ticks_total", "Just keeps on ticking");

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Metrics.SuppressDefaultMetrics();

            var server = new MetricServer(hostname: "localhost", port: 1234);
            server.Start();

            var store = new GroceryStore("Seattle");
            store.ProcessOrder("customerA", ("potato", 2), ("tomato", 3));
            store.ProcessOrder("customerB", ("tomato", 10));
            store.ProcessOrder("customerC", ("potato", 2));
            store.ProcessOrder("customerA", ("tomato", 1));

            while (true)
            {
                TickTock.Inc();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
