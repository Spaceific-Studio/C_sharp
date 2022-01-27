#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System;
using System.Reflection;
using System.Collections.Generic;
using adWin = Autodesk.Windows;
using System.Linq;
using IronPython.Runtime.Operations;

#endregion

namespace Spaceific_Studio_Panel
{
	class App : IExternalApplication
	{
		UIControlledApplication uIContrApp;
		UIApplication uIApp;
		Application app;
		string[] scriptRootDirs;		
		bool hasSpaceificStudioDir = false;
		string[] spaceificStudioDirs = new string[] { };
		System.Collections.ArrayList panelNames = new System.Collections.ArrayList();
		System.Collections.ArrayList panelContentNames = new System.Collections.ArrayList();
		System.Collections.ArrayList ribbonPanels = new System.Collections.ArrayList();
		Dictionary<string, string[]> buttonData = new Dictionary<string, string[]>();
		string tabName = "Spaceific-Studio";
		string defaultIconPath = "";
		ExternalEventHandler exEvHandler = new ExternalEventHandler();
		ExternalEvent commandEvent;
		Dictionary<string, string[]> commandInfo = new Dictionary<string, string[]>();



		public Result OnStartup(UIControlledApplication a)
		{

			commandInfo.Add("joinAllElementsByPriority.py", new string[] { "Join \nAll", "Joins all selected element by priority table. Must be selected elements before command is run." });
			
			adWin.ComponentManager.UIElementActivated += new EventHandler<Autodesk.Windows.UIElementActivatedEventArgs>(ComponentManager_UIElementActivated);
			a.ControlledApplication.ApplicationInitialized += ControlledApplication_ApplicationInitialized;
			commandEvent = ExternalEvent.Create(exEvHandler);
			scriptRootDirs = ReadScriptRootDirs(a);

			//AddRibbon(a);
			uIContrApp = a;
			return Result.Succeeded;
		}

		public Result OnShutdown(UIControlledApplication a)
		{
			return Result.Succeeded;
		}

		void ComponentManager_UIElementActivated(object sender, Autodesk.Windows.UIElementActivatedEventArgs e)
		{
			var to = typeof(Environment);
			//var propertyName = "CurrentDirectory";
			var propertyName = "Item";
			var eType = e.GetType();
			//tabName = "Spaceific-Studio";


			try
			{
				var pi = eType.GetProperty(propertyName);
				if (pi != null)
				{
					string itemId = e.Item.Id;
					string[] splittedId = itemId.Split('%');					
					if (splittedId.Length > 3)
					{
						string pbName = splittedId[splittedId.Length - 1];
						string panelGroup = splittedId[splittedId.Length - 2];
						string ribbonName = splittedId[splittedId.Length - 3];
						if(ribbonName == tabName)
						{
							Console.WriteLine(String.Format("Object has property {0}", propertyName));
							//var itemName = e.Item.Name;
							//TaskDialog myDialog = new TaskDialog("itemPressed");
							//myDialog.MainInstruction = String.Format("eType: {0}", eType.ToString());
							//myDialog.ExpandedContent = (String.Format("e.Item {0}\n" +
							//	"										Name{1}\n" +
							//	"										tag {2}\n" +
							//	"										text {3}\n" +
							//	"										UID {4}\n" +
							//	"										Id {5}\n" +
							//	"										groupName {6}\n" +
							//	"										description {7}\n" +
							//	"										AutomationName {8}\n" +
							//	"										pbName {9}\n" +
							//	"										panelGroup {10}\n" +
							//	"										ribbonName {11}\n",
							//							e.Item.ToString(),
							//							e.Item.Name,
							//							e.Item.Tag,
							//							e.Item.Text,
							//							e.Item.UID,
							//							e.Item.Id,
							//							e.Item.GroupName,
							//							e.Item.Description,
							//							e.Item.AutomationName,
							//							pbName,
							//							panelGroup,
							//							ribbonName
							//							));

							//myDialog.ExpandedContent = (String.Format("external event raised {0}", exEvHandler.GetName()));
							//myDialog.Show();
							//exEvHandler.SetCurrentPythonScritpt(buttonData[pbName][0]);
							
							exEvHandler.SetCurrentPythonScritpt(buttonData[pbName][0]);
							commandEvent.Raise();
							
							//var command = new UniversalPythonCommand();
							//command.SetCurrentPythonScritpt(buttonData[pbName][0]);

							//command.Execute(uIApp);
						}
					}
					

				}
				else
				{
					Console.WriteLine(String.Format("Object Doesn't have property {0}", propertyName));
				}
			}
			catch
			{

			}
			
			
		}

		private void ControlledApplication_ApplicationInitialized(object sender, Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs e)
		{
			//Not sure if the sender is Application or UIApplication
			//TaskDialog myDialog = new TaskDialog("ControlledApplication_ApplicationInitialized");
			//myDialog.MainInstruction = ("ControlledApplication_ApplicationInitialized");
			//myDialog.Show();

			if (sender is UIApplication)
			{
				uIApp = sender as UIApplication;
				app = uIApp.Application;
				//TaskDialog myDialog2 = new TaskDialog("sender is UIApplication");
				//myDialog2.MainInstruction = "UIApplication";
				//myDialog2.Show();
			}
			else
			{
				uIApp = new UIApplication(sender as Application);
				app = sender as Application;
				//TaskDialog myDialog2 = new TaskDialog("sender is not  UIApplication");
				//myDialog2.MainInstruction = "Not UIApplication";
				//myDialog2.Show();
			}
		}

		public string[] ReadScriptRootDirs(UIControlledApplication application)
		{
			//string[] myDirs = new string[] { "Dir1", "Dir2"};
			//tabName = "Spaceific-Studio";

			string revitVersion = "";
			try
			{
				revitVersion = application.ControlledApplication.VersionNumber;
				//TaskDialog myDialogRv = new TaskDialog("RevitVersion");
				//myDialogRv.MainInstruction = String.Format("{0}", revitVersion);
				//myDialogRv.Show();
			}
			catch
			{
				revitVersion = "2020";
				//TaskDialog myDialogRvE = new TaskDialog("RevitVersion setup manually");
				//myDialogRvE.MainInstruction = String.Format("{0}", revitVersion);
				//myDialogRvE.Show();				
			}

			//string revitVersion = "2020";
			string user = "";
			try
			{
				user = Environment.UserName;          //get the currently logged user name
				//TaskDialog myDialogUn = new TaskDialog("UserName");
				//myDialogUn.MainInstruction = String.Format("{0}", user);
				//myDialogUn.ExpandedContent = String.Format("{0}", user);
				//myDialogUn.Show();
			}
			catch
			{
			}
			string folderFullName = "";
			try
			{
				folderFullName = @"C:\users\" + user + @"\AppData\Roaming\Autodesk\Revit\Addins\" + revitVersion + @"\";  //User root folder
				//TaskDialog myDialogFn = new TaskDialog("FolderFullName");
				//myDialogFn.MainInstruction = String.Format("{0}", folderFullName);
				//myDialogFn.ExpandedContent = String.Format("{0}", folderFullName);
				//myDialogFn.Show();
			}
			catch
			{
			}
			string[] myDirs = new string[] {};

			try
			{
				myDirs = System.IO.Directory.GetDirectories(folderFullName);
			}
			catch
			{

			}
			
			string dirs = "";
			try
			{
				dirs = String.Join("\\n", myDirs);
			}
			catch
			{
				dirs = "";
			}
			

			//TaskDialog myDialog = new TaskDialog("myDirs");

			//myDialog.MainInstruction = String.Format("{0}", dirs);
			//myDialog.ExpandedContent = String.Format("{0}", dirs);
			//myDialog.Show();
			string ssDirs = "";
			foreach (string dir in myDirs)
			{
				string[] splittedPath = dir.Split('\\');
				if(splittedPath.Length > 0)
				{
					if (splittedPath[splittedPath.Length-1] == tabName)
					{
						hasSpaceificStudioDir = true;
						spaceificStudioDirs = System.IO.Directory.GetDirectories(dir);
						defaultIconPath = System.IO.Path.Combine(dir, "default.png");

						//create Tab
						application.CreateRibbonTab(tabName);
						int i = 0;
						foreach (string panelDirName in spaceificStudioDirs)
						{
							string[] splittedDirName = panelDirName.Split('\\');
							string[] contentFiles = System.IO.Directory.GetFiles(panelDirName);
							if (splittedDirName.Length > 0)
							{
								panelNames.Add(splittedDirName[splittedDirName.Length - 1]);
								//add Ribbon Pannel
								//RibbonPanel myRibbonPanel = application.CreateRibbonPanel(String.Format("{0}", i), splittedDirName[splittedDirName.Length - 1]);
								RibbonPanel myRibbonPanel = application.CreateRibbonPanel(tabName, splittedDirName[splittedDirName.Length - 1]);
								//System.Collections.ArrayList pyFiles = new System.Collections.ArrayList();
								//System.Collections.ArrayList pyFileNames = new System.Collections.ArrayList();
								Dictionary<string, string[]> pyFiles = new Dictionary<string, string[]>();
								foreach (string file in contentFiles)
								{
									string[] splittedFile = file.Split('\\');
									string[] splittedFileName = splittedFile[splittedFile.Length-1].Split('.');
									string[] fileAttachments = new string[3];

									if(splittedFileName.Length > 1)
									{
										string fileExt = splittedFileName[splittedFileName.Length - 1];
										string fileName = splittedFileName[splittedFileName.Length - 2];

										if (!pyFiles.ContainsKey(fileName))
										{
											if (fileExt == "py")
											{
												string[] cSplittedFileName = new string[splittedFileName.Length];
												Array.Copy(splittedFileName, cSplittedFileName, splittedFileName.Length);
												cSplittedFileName[cSplittedFileName.Length - 1] = "png";
												string[] pngFullPath = new string[splittedFile.Length];
												Array.Copy(splittedFile, pngFullPath, splittedFile.Length);
												 
												string pngSearchName = String.Join(".", cSplittedFileName);
												pngFullPath[pngFullPath.Length - 1] = pngSearchName;
												string pngSearchFile = String.Join("\\", pngFullPath);
												string[] contentToPanel = new string[2];
												//TaskDialog pngSearchDialog = new TaskDialog("png search");

												//pngSearchDialog.MainInstruction = String.Format("{0}", pngSearchFile);
												//pngSearchDialog.ExpandedContent = String.Format("{0}", pngSearchFile);
												//pngSearchDialog.Show();
												if (contentFiles.Contains(pngSearchFile))
												{
													//pyFiles.Add(splittedFileName[splittedFileName.Length - 2], new string[]{file, pngSearch});
													contentToPanel[0] = file;
													contentToPanel[1] = pngSearchFile;
												}
												else
												{
													//pyFiles.Add(splittedFileName[splittedFileName.Length - 2], new string[] {file, defaultIconPath});
													contentToPanel[0] = file;
													contentToPanel[1] = defaultIconPath;
												}
												pyFiles.Add(splittedFileName[splittedFileName.Length - 2], contentToPanel);
												buttonData.Add(splittedFileName[splittedFileName.Length - 2], contentToPanel);
												string buttonText = "";
												string longDescr = "";
												if (commandInfo.ContainsKey(splittedFileName[splittedFileName.Length - 2]))
												{
													buttonText = commandInfo[splittedFileName[splittedFileName.Length - 2]][0];
													longDescr = commandInfo[splittedFileName[splittedFileName.Length - 2]][1];
												}
												else
												{
													buttonText = splittedFileName[splittedFileName.Length - 2];
													longDescr = "XXX";
												}
												PushButtonData pbd = new PushButtonData(splittedFileName[splittedFileName.Length - 2], buttonText, @"H:\_WORK\C#\projects\Spaceific-Studio_Panel\Spaceific-Studio_Panel\Spaceific-Studio_Panel\bin\Debug\Spaceific-Studio_Panel.dll", "Spaceific_Studio_Panel.DefaultCode");
												PushButton pb = myRibbonPanel.AddItem(pbd) as PushButton;
												pbd.LongDescription = longDescr;
												BitmapImage imgPb = new BitmapImage(new Uri(contentToPanel[1]));
												pb.LargeImage = imgPb;
											}
										}
									}
									
								}
								panelContentNames.Add(pyFiles);
								ssDirs += splittedDirName[splittedDirName.Length - 1] + "\n";
							}
							i++;
						}
					}
				}
			}
			//TaskDialog spaceificStudioDirsDialog = new TaskDialog("Spaceific_Studio_Dirs");

			//spaceificStudioDirsDialog.MainInstruction = String.Format("{0}", ssDirs);
			//spaceificStudioDirsDialog.ExpandedContent = String.Format("{0}", ssDirs);
			//spaceificStudioDirsDialog.Show();
			//Console.WriteLine(String.Format("Length of myDirs: {0, 10}", myDirs.Length));
			return myDirs;
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
