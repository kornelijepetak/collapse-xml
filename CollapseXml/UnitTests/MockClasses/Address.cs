using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MockClasses
{
	class Address
	{
		public Address(String street, String city, String country)
		{
			Street = street;
			City = city;
			Country = country;
		}

		public String Street { get; private set; }
		public String City { get; private set; }
		public String Country { get; private set; }
	}
}
