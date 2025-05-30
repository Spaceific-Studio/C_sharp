﻿//#error version
#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using IronPython.Compiler;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using RevitPythonShell.RpsRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace Spaceific_Studio_Panel
{
	[Transaction(TransactionMode.Manual)]
	public class GetParamsOfSelected : IExternalCommand
	{
		public Result Execute(
		ExternalCommandData _commandData,
		ref string _message,
		ElementSet _elements)
		{
			UIApplication _revit = _commandData.Application;
			UIDocument uidoc = _revit.ActiveUIDocument;
			Application app = _revit.Application;
			Document doc = uidoc.Document;

			var flags = new Dictionary<string, object>() { { "Frames", true }, { "FullFrames", true } };
			var py = IronPython.Hosting.Python.CreateEngine(flags);
			var scope = IronPython.Hosting.Python.CreateModule(py, "__main__");
			scope.SetVariable("__commandData__", _commandData);
			string csharpInfo = "This script is running under csharp";

			/*
			var path = py.GetSearchPaths();
			string currentPath = Directory.GetCurrentDirectory();
			path.Add(System.IO.Path.GetDirectoryName(currentPath));
			py.SetSearchPaths(path);

			var myScriptExecutor = new ScriptExecutor(_config, _commandData, _message, _elements);
			*/

			// add special variable: __revit__ to be globally visible everywhere:
			var builtin = IronPython.Hosting.Python.GetBuiltinModule(py);
			builtin.SetVariable("__revit__", _revit);
			builtin.SetVariable("__csharp__", csharpInfo);
			py.Runtime.LoadAssembly(typeof(Autodesk.Revit.DB.Document).Assembly);
			py.Runtime.LoadAssembly(typeof(Autodesk.Revit.UI.TaskDialog).Assembly);

			string revitVersion = app.VersionNumber;
			string user = Environment.UserName;          //get the currently logged user name
			string folderFullName = @"C:\users\" + user + @"\AppData\Roaming\Autodesk\Revit\Addins\" + revitVersion + @"\";  //User root folder

			//DirectoryInfo TheFolder = new DirectoryInfo(folderFullName);
			//Console.WriteLine("User folder:" + folderFullName);
			//foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
			//	Console.WriteLine(NextFolder.Name);
			//Console.ReadLine();
			string myCWD = Directory.GetCurrentDirectory();

			//TaskDialog dirDialog = new TaskDialog("Current user directory");
			//dirDialog.MainInstruction = folderFullName;
			//dirDialog.ExpandedContent = folderFullName;
			//dirDialog.Show();

			var scriptOutput = new ScriptOutput();
			scriptOutput.Show();
			var outputStream = new ScriptOutputStream(scriptOutput, py);
			scriptOutput.Hide();
			py.Runtime.IO.SetOutput(outputStream, Encoding.UTF8);
			py.Runtime.IO.SetErrorOutput(outputStream, Encoding.UTF8);
			py.Runtime.IO.SetInput(outputStream, Encoding.UTF8);

			//var assembly = this.GetType().Assembly;
			//string scriptName = "joinAllElementsByPriority.py";
			//var source = new StreamReader(assembly.GetManifestResourceStream(scriptName)).ReadToEnd();

			//var result = myScriptExecutor.ExecuteScript(source, Path.Combine(assembly.Location, scriptName));
			//_message = myScriptExecutor.Message;

			try
			{
				py.ExecuteFile(folderFullName + @"LIB\" + "getParamsOfSelected.py");
				//py.ExecuteFile(folderFullName + "getParamsOfSelected.py");
			}
			catch (Exception ex)
			{
				TaskDialog myDialog = new TaskDialog("IronPython Error");
				myDialog.MainInstruction = "Couldn't execute IronPython script getParamsOfSelected.py: ";
				myDialog.ExpandedContent = ex.Message;
				myDialog.MainContent = ex.Message;

				myDialog.Show();
			}

			return Result.Succeeded;
			/*
			switch (result)
			{
				case (int)Result.Succeeded:
					return Result.Succeeded;
				case (int)Result.Cancelled:
					return Result.Cancelled;
				case (int)Result.Failed:
					return Result.Failed;
				default:
					return Result.Succeeded;
			}
			*/
		}

	}
}
