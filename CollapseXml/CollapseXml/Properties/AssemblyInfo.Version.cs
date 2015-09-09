using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("1.3.3.0")]
[assembly: AssemblyFileVersion("1.3.3.0")]

/*
 *	CHANGELOG
 * 
 *			1.3.3
 *				(2015-01-11)
 *					- FIXED. cx:foreach was throwing an InvalidForeachPathException
 *							 if collection identifier contained the word 'in'.
 *			1.3.2*
 *				(2014-11-27)
 *					- Scope class changed named into ScopeItem
 *			1.3.1*
 *				(2014-11-27)
 *					- OPTIMIZATION. Minor stohastic optimization. 
 *			1.3.0.			
 *				(2014-11-16)
 *					- FIXED. Fixed a bug in SetDateTimeFormat(IFormatProvider).
 *					- ADDED. Filter arguments can now be retrieved by name.
 *					- ADDED. Exceptions now contain a lot more details about an error.
 *			
 *			1.1.2.
 *				(2014-11-11)
 *					- FIXED. Collections of value types (ex. List<DateType>) were not treated as collections.
 *			
 *			1.1.1.
 *				(2014-11-11)
 *					- ADDED. Static Collapse.Export method
 *			
 *			1.1.0.
 *				(2014-11-11)
 *					- Several exceptions were moved from Kaisean.Tools.Collapse namespace into Kaisean.Tools.Collapse.Errors namespace
 *					- Possible breaking change. To fix the problems caused by this change in API, 
 *							please update the usings to the correct namespace. If you did not catch 
 *							any CollapseXML exceptions, you are safe to update to this version without 
 *							modifications. We apologize for the inconvenience.
 *			1.0.0.
 *				(2014-11-04)
 *					- Initial version.
 * 
 */