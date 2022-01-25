using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public class MyClass
{
	public static void Main()
	{
		DoIt("Hello There");   // CS0120
	}

	private static void DoIt(string sText)
	{
		Console.WriteLine(sText);		
	}
}
