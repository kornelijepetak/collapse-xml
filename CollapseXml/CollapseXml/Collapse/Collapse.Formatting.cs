using Kaisean.Tools.CollapseXml.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Kaisean.Tools.CollapseXml
{
	public partial class Collapse
	{
		/// <summary>
		/// Sets a DateTime formatter to be used for outputting the DateTime values
		/// </summary>
		/// <param name="formatProvider">Format provider</param>
		/// <returns>A Collapse instance, used for chaining</returns>
		public Collapse SetDateTimeFormat(IFormatProvider formatProvider)
		{
			if(formatProvider == null)
				throw new ArgumentNullException("formatProvider", "Format provider was not supplied.");

			DateTimeFormatInfo formatInfo =
				formatProvider.GetFormat(typeof(DateTimeFormatInfo)) as DateTimeFormatInfo;

			if (formatInfo == null)
				throw new DateTimeFormatInvalidException();

			SetDateTimeFormat(
				String.Format("{0} {1}", formatInfo.ShortDatePattern, formatInfo.LongTimePattern));

			return this;
		}

		/// <summary>
		/// Sets a DateTime format to be used for outputting the DateTime values
		/// </summary>
		/// <param name="format">DateTime format</param>
		/// <returns>A Collapse instance, used for chaining</returns>
		public Collapse SetDateTimeFormat(String format)
		{
			try
			{
				DateTime.Now.ToString(format);
			}
			catch
			{
				throw new DateTimeFormatInvalidException(format);
			}

			dateTimeFormat = format;
			return this;
		}

		/// <summary>
		/// Sets a number format to be used for outputting the numerical values
		/// </summary>
		/// <param name="formatProvider">Format provider for numbers</param>
		/// <returns>A Collapse instance, used for chaining</returns>
		public Collapse SetNumberFormat(IFormatProvider formatProvider)
		{
			numberFormatProvider =
				(IFormatProvider)(formatProvider ?? CultureInfo.CurrentCulture);

			return this;
		}
	}
}
