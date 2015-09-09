using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MockClasses
{
	class Repository
	{
		public Repository()
		{
			Customers = new List<Customer>();

			Customer mark = new Customer(
				"Mark Thompson",
				new Address("14 Thornton Av", "Cauldron City", "Leveland"),
				1.4);

			Customer john = new Customer(
				"John Redstone",
				new Address("76 Homecoming drive", "Buildville", "Land of stones"),
				-2.76);

			mark.RelevantDates.AddRange(new[] { 
				DateTime.Parse("2014-01-01"),
				DateTime.Parse("2014-01-02"),
				DateTime.Parse("2014-01-03")
			});

			Customers.AddMany(mark, john);

			Article bread = new Article("Bread", "g");
			Article milk = new Article("Milk", "ml");
			Article ball = new Article("Ball", "pcs");
			Article apple = new Article("Apple", "kg");

			mark.Orders.AddMany(
				new Order(DateTime.Parse("2014-01-01"),
					new OrderItem(bread, 700),
					new OrderItem(milk, 1000)),
				new Order(DateTime.Parse("2014-02-04"),
					new OrderItem(milk, 500),
					new OrderItem(ball, 1)),
				new Order(DateTime.Parse("2014-03-05"))
				);

			john.Orders.AddMany(
				new Order(DateTime.Parse("2014-01-01"),
					new OrderItem(bread, 1000)),
				new Order(DateTime.Parse("2014-02-08"),
					new OrderItem(ball, 1),
					new OrderItem(apple, 1.2))
				);

			PayingCustomers = Customers.Where(c => c.Coefficient > 0).ToList();
		}

		public List<Customer> Customers { get; private set; }
		public List<Customer> PayingCustomers { get; private set; }

		public static Customer SampleCustomer
		{
			get
			{
				Repository rep = new Repository();
				return rep.Customers.First();
			}
		}

	}
}