//#error version
#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


#endregion

namespace Spaceific_Studio_Panel
{
	[Transaction(TransactionMode.Manual)]
	public class DefaultCode : IExternalCommand
	{
		string currentPythonScript;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{

			return Execute(commandData.Application);


		}
		public Result Execute(UIApplication uiapp)
		{


			return Result.Succeeded;

		}
	}
}
