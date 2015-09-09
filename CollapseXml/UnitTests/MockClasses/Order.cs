using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MockClasses
{
	class Order
	{
		public Order(DateTime timestamp, params OrderItem[] items)
		{
			Timestamp = timestamp;
			Items = new List<OrderItem>(items);
		}

		public bool IsValid()
		{
			return Items.Count > 0;
		}

		public DateTime Timestamp { get; private set; }
		public List<OrderItem> Items { get; private set; }
	}
}