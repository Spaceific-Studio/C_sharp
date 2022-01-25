using System;
using System.IO;
using System.Reflection;
using System.Linq;

namespace DirectoryReadConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Program progInst = new Program();
			
			foreach(string dir in progInst.ReadDirs())
			{
				string[] splittedDir = dir.Split(@"\");
				Console.WriteLine(splittedDir[splittedDir.Length-1]);
				
			}

			foreach (string file in progInst.ReadFiles())
			{
				string[] splittedPath = file.Split(@"\");
				Console.WriteLine(splittedPath[splittedPath.Length - 1]);

			}

			var to = typeof(Environment);
			//var propertyName = "CurrentDirectory";
			var propertyName = "Item";
			var pi = to.GetProperty(propertyName);
			if(pi != null)
			{
				Console.WriteLine(String.Format("Object has property {0}", propertyName));
			}
			else
			{
				Console.WriteLine(String.Format("Object Doesn't have property {0}", propertyName));
			}
			//var hasAttr = progInst.CheckPropertyAttribute(pi, t, "Item");
			Console.WriteLine(String.Format("typeOf(Environment) {0} propertyInfo {1}", to, pi));

		}

		public bool CheckPropertyAttribute(PropertyInfo prop, Type attributeType,
								   string attProp)
		{
			object[] vs = prop.GetCustomAttributes(attributeType, true);
			var att = vs;
			if (att == null || !att.Any()) return false;
			var attProperty = attributeType.GetProperty(attProp);
			if (attProperty == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public string[] ReadDirs()
		{
			//string[] myDirs = new string[] { "Dir1", "Dir2"};


			//string revitVersion = app.VersionNumber;
			string revitVersion = "2020";
			string user = Environment.UserName;          //get the currently logged user name
			string folderFullName = @"C:\users\" + user + @"\AppData\Roaming\Autodesk\Revit\Addins\" + revitVersion + @"\";  //User root folder
			string[] myDirs = System.IO.Directory.GetDirectories(folderFullName);
			Console.WriteLine(String.Format("Length of myDirs: {0, 10}", myDirs.Length));
			return myDirs;
		}

		public string[] ReadFiles()
		{
			//string[] myDirs = new string[] { "Dir1", "Dir2"};


			//string revitVersion = app.VersionNumber;
			string revitVersion = "2020";
			string user = Environment.UserName;          //get the currently logged user name
			string folderFullName = @"C:\users\" + user + @"\AppData\Roaming\Autodesk\Revit\Addins\" + revitVersion + @"\";  //User root folder
			string[] myFiles = System.IO.Directory.GetFiles(folderFullName);
			Console.WriteLine(String.Format("Length of myFiles: {0, 10}", myFiles.Length));
			return myFiles;
		}

		
	}
}

