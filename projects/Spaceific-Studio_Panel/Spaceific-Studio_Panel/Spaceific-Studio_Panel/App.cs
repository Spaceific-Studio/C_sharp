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

		static void	AddRibbon(UIControlledApplication application)
		{
			application.CreateRibbonTab("Spaceific-Studio");
			RibbonPanel annotateRibbonPanel = application.CreateRibbonPanel("Spaceific-Studio", "Annotate");
			RibbonPanel geometryRibbonPanel = application.CreateRibbonPanel("Spaceific-Studio", "Geometry");

			//Annotate buttons

			PushButtonData pbd1 = new PushButtonData("pbd1", "Openings Dim", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.OpeningsDimensions");
			PushButton pb1 = annotateRibbonPanel.AddItem(pbd1) as PushButton;
			BitmapImage imgPb1 = new BitmapImage(new Uri(@"h:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\OpeningsDimensions-32.png"));
			pb1.LargeImage = imgPb1;

			//Geometry buttons

			PushButtonData pbd2 = new PushButtonData("pbd2", "Join \nAll", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.JoinAllElementsByPriority");
			PushButton pb2 = geometryRibbonPanel.AddItem(pbd2) as PushButton;
			BitmapImage imgPb2 = new BitmapImage(new Uri(@"h:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\joinAllElementsByPriority.png"));
			pb2.LargeImage = imgPb2;

			PushButtonData pbd3 = new PushButtonData("pbd3", "Unjoin \nSelected", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.UnJoinSelectedElements");
			PushButton pb3 = geometryRibbonPanel.AddItem(pbd3) as PushButton;
			BitmapImage imgPb3 = new BitmapImage(new Uri(@"h:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\unJoinSelectedElements.png"));
			pb3.LargeImage = imgPb3;

			PushButtonData pbd4 = new PushButtonData("pbd4", "Change \nOrder", @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.ChangeOrderOfJoinedElements");
			PushButton pb4 = geometryRibbonPanel.AddItem(pbd4) as PushButton;
			BitmapImage imgPb4 = new BitmapImage(new Uri(@"h:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\changeOrderOfJoinedElements.png"));
			pb4.LargeImage = imgPb4;


		}
	}
}
