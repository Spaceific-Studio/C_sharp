#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;

#endregion

namespace Spaceific_Studio_Panel
{
	class App : IExternalApplication
	{
		public Result OnStartup(UIControlledApplication a)
		{
			AddRibbon(a);
			return Result.Succeeded;
		}

		public Result OnShutdown(UIControlledApplication a)
		{
			return Result.Succeeded;
		}

		static void AddRibbon(UIControlledApplication application)
		{
			application.CreateRibbonTab("Spaceific-Studio");
			RibbonPanel annotateRibbonPanel = application.CreateRibbonPanel("Spaceific-Studio", "Annotate");
			RibbonPanel geometryRibbonPanel = application.CreateRibbonPanel("Spaceific-Studio", "Geometry");
			RibbonPanel paramRibbonPanel = application.CreateRibbonPanel("Spaceific-Studio", "Parameters");

			string revitVersion = application.ControlledApplication.VersionNumber;
			string user = Environment.UserName;          //get the currently logged user name
			string folderFullName = @"C:\users\" + user + @"\AppData\Roaming\Autodesk\Revit\Addins\" + revitVersion + @"\LIB\";  //User LIB folder
																																 //Annotate buttons

			PushButtonData pbd1 = new PushButtonData("pbd1", "Openings Dim", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.OpeningsDimensions");
			PushButton pb1 = annotateRibbonPanel.AddItem(pbd1) as PushButton;
			pbd1.LongDescription = "Adds height of openings to annotation";
			BitmapImage imgPb1 = new BitmapImage(new Uri(folderFullName + "OpeningsDimensions-32.png"));
			pb1.LargeImage = imgPb1;

			//Geometry buttons

			PushButtonData pbd2 = new PushButtonData("pbd2", "Join \nAll", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.JoinAllElementsByPriority");
			PushButton pb2 = geometryRibbonPanel.AddItem(pbd2) as PushButton;
			pbd2.LongDescription = "Joins all selected element by priority table. Must be selected elements before command is run.";
			BitmapImage imgPb2 = new BitmapImage(new Uri(folderFullName + "joinAllElementsByPriority.png"));
			pb2.LargeImage = imgPb2;

			PushButtonData pbd3 = new PushButtonData("pbd3", "Unjoin \nSelected", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.UnJoinSelectedElements");
			PushButton pb3 = geometryRibbonPanel.AddItem(pbd3) as PushButton;
			pbd3.LongDescription = "Unjoin selected elements";
			BitmapImage imgPb3 = new BitmapImage(new Uri(folderFullName + "unJoinSelectedElements.png"));
			pb3.LargeImage = imgPb3;

			PushButtonData pbd4 = new PushButtonData("pbd4", "Change \nOrder", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.ChangeOrderOfJoinedElements");
			PushButton pb4 = geometryRibbonPanel.AddItem(pbd4) as PushButton;
			pbd4.LongDescription = "Changes order of selected joined elements.";
			BitmapImage imgPb4 = new BitmapImage(new Uri(folderFullName + "changeOrderOfJoinedElements.png"));
			pb4.LargeImage = imgPb4;

			PushButtonData pbd5 = new PushButtonData("pbd5", "Get All \nParameters", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.GetParamsOfSelected");
			pbd5.LongDescription = "Lists all availabe parameters.";
			PushButton pb5 = paramRibbonPanel.AddItem(pbd5) as PushButton;
			BitmapImage imgPb5 = new BitmapImage(new Uri(folderFullName + "getParamsOfSelected.png"));
			pb5.LargeImage = imgPb5;


		}
	}
}
