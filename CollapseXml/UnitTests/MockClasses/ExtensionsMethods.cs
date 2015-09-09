using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.MockClasses
{
	public static class ExtensionsMethods
	{
		public static void AddMany<T>(this List<T> collection, params T[] args)
		{
			collection.AddRange(args);
		}
	}
}
