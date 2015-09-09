using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnitTests
{
	static class Res
	{
		private readonly static Assembly assembly;
		private readonly static String[] resourceNames;

		static Res()
		{
			assembly = Assembly.GetExecutingAssembly();
			resourceNames = assembly.GetManifestResourceNames();
		}

		internal static String FileContent(String resourceName)
		{
			resourceName = 
				resourceNames.FirstOrDefault(name => name.Contains(resourceName));

			Stream stream = assembly.GetManifestResourceStream(resourceName);
			using (StreamReader reader = new StreamReader(stream))
				return reader.ReadToEnd().Trim();
		}
	}
}
