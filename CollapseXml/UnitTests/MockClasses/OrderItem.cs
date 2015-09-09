using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MockClasses
{
	class OrderItem
	{
		public OrderItem(Article article, double amount)
		{
			Article = article;
			Amount = amount;
		}

		public Article Article { get; private set; }
		public double Amount { get; private set; }
	}
}
