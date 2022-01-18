using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spaceific_Studio_Panel
{
	[Transaction(TransactionMode.Manual)]
	public class OpeningsDimensions : IExternalCommand
	{
		public Result Execute(
		  ExternalCommandData commandData,
		  ref string message,
		  ElementSet elements)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Application app = uiapp.Application;
			Document doc = uidoc.Document;

			double foot = 0.3048;
			FilteredElementCollector col = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Dimensions);
			Element getElement;
			foreach (Dimension d in col)
			{
				using (Transaction tx = new Transaction(doc))
				{
					tx.Start("Adding text to dimension");
					if (d.Segments.Size == 0)
					{
						if (d.References.get_Item(0).ElementId == d.References.get_Item(1).ElementId)
						{
							getElement = doc.GetElement(d.References.get_Item(0));
							//DOOR ELEMENTS CONDITION
							if (getElement.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
							{
								d.Below = UnitUtils.ConvertFromInternalUnits(getElement.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble(), DisplayUnitType.DUT_MILLIMETERS).ToString();
							}
							//WINDOW ELEMENTS CONDITION
							if (getElement.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows)
							{
								double headHeight = UnitUtils.ConvertFromInternalUnits(getElement.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble(), DisplayUnitType.DUT_MILLIMETERS);
								string headHeightStr = headHeight.ToString();
								double sillHeight = Math.Round(UnitUtils.ConvertFromInternalUnits(getElement.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM).AsDouble(), DisplayUnitType.DUT_MILLIMETERS), 0);
								double windowHeight = Math.Round(headHeight - sillHeight, 0);
								sillHeight = Math.Round(sillHeight, 0);
								string sillHeightStr = sillHeight.ToString();
								string windowHeightStr = windowHeight.ToString();
								d.Below = windowHeightStr + " (" + sillHeightStr + ")";
							}
						}
					}
					for (int i = 0; i < d.Segments.Size; i++)
					{
						DimensionSegment ds = d.Segments.get_Item(i);

						//BOTH REFERENCE OF SEGMENT MUST HAVE THE SAME ID
						if (d.References.get_Item(i).ElementId == d.References.get_Item(i + 1).ElementId)
						{
							getElement = doc.GetElement(d.References.get_Item(i));
							//DOOR ELEMENTS CONDITION
							if (getElement.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
							{
								ds.Below = UnitUtils.ConvertFromInternalUnits(getElement.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble(), DisplayUnitType.DUT_MILLIMETERS).ToString();
							}
							//WINDOW ELEMENTS CONDITION
							if (getElement.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows)
							{
								double headHeight = UnitUtils.ConvertFromInternalUnits(getElement.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM).AsDouble(), DisplayUnitType.DUT_MILLIMETERS);
								string headHeightStr = headHeight.ToString();
								double sillHeight = Math.Round(UnitUtils.ConvertFromInternalUnits(getElement.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM).AsDouble(), DisplayUnitType.DUT_MILLIMETERS), 0);
								double windowHeight = Math.Round(headHeight - sillHeight, 0);
								sillHeight = Math.Round(sillHeight, 0);
								string sillHeightStr = sillHeight.ToString();
								string windowHeightStr = windowHeight.ToString();
								ds.Below = windowHeightStr + " (" + sillHeightStr + ")";
							}
						}
					}

					tx.Commit();
				}
			}
			return Result.Succeeded;
		}
	}
}
