#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Diagnostics;
using IronPython.Hosting;
using IronPython.Compiler;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using RevitPythonShell.RpsRuntime;

#endregion

namespace ChangeOrderOfJoinedElements
{
	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
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
				py.ExecuteFile("changeOrderOfJoinedElements.py");
			}
			catch (Exception ex)
			{
				TaskDialog myDialog = new TaskDialog("IronPython Error");
				myDialog.MainInstruction = "Couldn't execute IronPython script totalSelectedVolume.py: ";
				myDialog.ExpandedContent = ex.Message;
				myDialog.EnableMarqueeProgressBar = true;
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
