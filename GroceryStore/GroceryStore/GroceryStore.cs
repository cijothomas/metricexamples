using Prometheus;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore
{
    public class GroceryStore
    {
        private static Dictionary<string, double> priceList = new Dictionary<string, double>()
        {
            { "potato", 1.00 },
            { "tomato", 3.00 },
        };

        private string storeName;
        private static readonly Gauge ItemsSold = Metrics.CreateGauge("itemssold",
            "Items sold",
            "storename",
            "customername",
            "itemName");
        private static readonly Summary Orders = Metrics.CreateSummary("orders",
            "Total orders",
            "storename",
            "customername"
            );

        public GroceryStore(string storeName)
        {
            this.storeName = storeName;
        }

        public void ProcessOrder(string customerName, params (string itemName, int itemQty)[] itemsBought)
        {
            double totalPrice = 0;

            foreach (var item in itemsBought)
            {
                totalPrice += item.itemQty * priceList[item.itemName];
                ItemsSold.WithLabels(this.storeName, customerName, item.itemName).Inc(item.itemQty);
            }

            Orders.WithLabels(this.storeName, customerName).Observe(totalPrice);
        }
    }
}
