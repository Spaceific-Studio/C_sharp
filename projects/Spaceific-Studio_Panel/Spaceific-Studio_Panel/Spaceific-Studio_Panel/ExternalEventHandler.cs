//#error version
#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPythonShell.RpsRuntime;
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Spaceific_Studio_Panel
{
	[Transaction(TransactionMode.Manual)]
	class ExternalEventHandler : IExternalEventHandler
	{
		public string currentPythonScript;

		public void Execute(UIApplication uiapp)
		{
			UIApplication _revit = uiapp;
			UIDocument uidoc = _revit.ActiveUIDocument;
			Application app = _revit.Application;

			//TaskDialog myDialog = new TaskDialog(String.Format("ExternalEventHandler"));
			//myDialog.MainInstruction = String.Format("ButtonPressed From External event: {0}", currentPythonScript);
			//myDialog.Show();

			//if (null == uidoc)
			//{
			//	return; // no document, nothing to do
			//}
			//Document doc = uidoc.Document;
			//using (Transaction tx = new Transaction(doc))
			{
				//tx.Start("MyEvent");
				// Action within valid Revit API context thread

				var flags = new Dictionary<string, object>() { { "Frames", true }, { "FullFrames", true } };
				var py = IronPython.Hosting.Python.CreateEngine(flags);
				var scope = IronPython.Hosting.Python.CreateModule(py, "__main__");
				scope.SetVariable("__commandData__", uiapp);
				string csharpInfo = "This script is running under csharp";

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


				try
				{
					py.ExecuteFile(currentPythonScript);
					//py.ExecuteFile(folderFullName + "getParamsOfSelected.py");
				}
				catch (Exception ex)
				{
					TaskDialog myExDialog = new TaskDialog("IronPython Error");
					myExDialog.MainInstruction = "Couldn't execute IronPython script getParamsOfSelected.py: ";
					myExDialog.ExpandedContent = ex.Message;
					myExDialog.MainContent = ex.Message;
					myExDialog.Show();
				}

				//tx.Commit();
			}
		}

		public string GetCurrentPythonScritpt()
		{
			return currentPythonScript;
		}

		public void SetCurrentPythonScritpt(string pyScriptName)
		{
			currentPythonScript = pyScriptName;
		}

		public string GetName()
		{
			return "External event handler";
		}
	}
}
