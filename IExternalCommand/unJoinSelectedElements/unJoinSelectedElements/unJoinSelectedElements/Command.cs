#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Diagnostics;
using IronPython.Hosting;
using IronPython.Compiler;
using IronPython.Runtime.Exceptions;

#endregion

namespace UnJoinSelectedElements
{
	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
	{
		public Result Execute(
		ExternalCommandData commandData,
		ref string message,
		ElementSet elements)
		{
			UIApplication _revit = commandData.Application;
			UIDocument uidoc = _revit.ActiveUIDocument;
			Application app = _revit.Application;
			Document doc = uidoc.Document;

			var flags = new Dictionary<string, object>() { { "Frames", true }, { "FullFrames", true } };
			var py = IronPython.Hosting.Python.CreateEngine(flags);
			var scope = IronPython.Hosting.Python.CreateModule(py, "__main__");
			scope.SetVariable("__commandData__", commandData);
			string csharpInfo = "This script is running under csharp";

			// add special variable: __revit__ to be globally visible everywhere:
			var builtin = IronPython.Hosting.Python.GetBuiltinModule(py);
			builtin.SetVariable("__revit__", _revit);
			builtin.SetVariable("__csharp__", csharpInfo);
			py.Runtime.LoadAssembly(typeof(Autodesk.Revit.DB.Document).Assembly);
			py.Runtime.LoadAssembly(typeof(Autodesk.Revit.UI.TaskDialog).Assembly);

			try
			{
				py.ExecuteFile("unJoinSelectedElements.py");
			}
			catch (Exception ex)
			{
				TaskDialog myDialog = new TaskDialog("IronPython Error");
				myDialog.MainInstruction = "Couldn't execute IronPython script unJoinSelectedElements.py: ";
				myDialog.ExpandedContent = ex.Message;
				myDialog.Show();
			}

			return Result.Succeeded;
		}
	}
}
