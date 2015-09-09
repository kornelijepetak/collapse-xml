using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MockClasses
{
	class Customer
	{
		public Customer(String name, Address address, Double coefficient, params Order[] orders)
		{
			Name = name;
			Address = address;
			Coefficient = coefficient;
			Orders = new List<Order>(orders);

			RelevantDates = new List<DateTime>();
		}

		public List<DateTime> RelevantDates { get; private set; } 

		public String Name { get; private set; }
		public Address Address { get; private set; }

		public Double Coefficient { get; private set; }

		public List<Order> Orders { get; private set; }
	}
}
