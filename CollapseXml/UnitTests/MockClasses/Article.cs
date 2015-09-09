using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.MockClasses
{
	class Article
	{
		public Article(String name, String unit)
		{
			Name = name;
			Unit = unit;
		}

		public String Name { get; private set; }
		public String Unit { get; private set; }
	}
}
