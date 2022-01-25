using System;

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
