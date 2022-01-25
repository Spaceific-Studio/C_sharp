//#error version
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
	public class UniversalPythonCommand : IExternalCommand
	{
		string currentPythonScript;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{

			return Execute(commandData.Application);


		}
		public Result Execute(UIApplication uiapp)
		{
			//Do all sorts of shiny stuff with your command. 
		
			TaskDialog myDialog = new TaskDialog(String.Format("UniversalPythonCommand"));
			myDialog.MainInstruction = String.Format("ButtonPressed: {0}", currentPythonScript);
			myDialog.Show();

			return Result.Succeeded;

		}

		public string GetCurrentPythonScritpt()
		{
			return currentPythonScript;
		}

		public void SetCurrentPythonScritpt(string pyScriptName)
		{
			currentPythonScript = pyScriptName;
		}
	}
}
