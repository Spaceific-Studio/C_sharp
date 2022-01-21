# -*- coding: utf-8 -*-
# Copyright(c) 2021, Daniel Gercak
#Revit Python Shell script for multiple unjoining elements
#resource_path: https://github.com/Spaceific-Studio/_WORK/REVIT_API/unJoinSelectedElements.py
import sys
if "IronPython" in sys.prefix:
	pytPath = r'C:\Program Files (x86)\IronPython 2.7\Lib'
	sys.path.append(pytPath)
import os
import platform
import time

try:
	sys.modules['__main__']
	hasMainAttr = True	
except:
	hasMainAttr = False

if hasMainAttr:
	#import clr
	if "pydroid" in sys.prefix:
		pass
	elif "Python38" in sys.prefix:
		pass
	else:
		from Autodesk.Revit.UI.Selection import *
		import Autodesk.Revit.DB as DB
		import Autodesk.Revit.UI as UI
		import Autodesk
		from Autodesk.Revit.DB import IFailuresPreprocessor, BuiltInFailures
		import System
		import threading
		from System.Collections.Generic import List as Clist
		from System import Type
		#from System.Collections.Generic import Ilist
		#import System.Drawing
		import clr
		clr.AddReferenceByPartialName('System.Windows.Forms')
		clr.AddReference("System.Drawing")
		clr.AddReference('System')
		#import System.Windows.Forms
		from System.Threading import ThreadStart, Thread
		from System.Windows.Forms import *
		from System.Drawing import *
		doc = __revit__.ActiveUIDocument.Document
		uidoc = __revit__.ActiveUIDocument
		#clr.AddReference("RevitServices")
		#import RevitServices
		#from RevitServices.Transactions import TransactionManager
		pass

else:
	if "pydroid" in sys.prefix:
		pass
	elif "Python38" in sys.prefix:
		pass
	else:
		import clr
		clr.AddReference('ProtoGeometry')
		from Autodesk.DesignScript.Geometry import *
		clr.AddReference("RevitAPI")
		import Autodesk
		import Autodesk.Revit.DB as DB
		clr.AddReference("RevitServices")
		import RevitServices
		from RevitServices.Persistence import DocumentManager
		from RevitServices.Transactions import TransactionManager
		doc = DocumentManager.Instance.CurrentDBDocument

# clr.AddReference("RevitAPI")
# import Autodesk
# import Autodesk.Revit.DB as DB

try:
	import Autodesk
	sys.modules['Autodesk']
	hasAutodesk = True	
except:
	hasAutodesk = False

try:
	runFromCsharp = True if "__csharp__" in dir() else False
	#UI.TaskDialog.Show("Run from C#", "Script is running from C#")
except:
	runFromCsharp = False


print("module : {0} ; hasMainAttr = {1}".format(__file__, hasMainAttr))
print("module : {0} ; hasAutodesk = {1}".format(__file__, hasAutodesk))

if sys.platform.startswith('linux'):
	libPath = r"/storage/emulated/0/_WORK/REVIT_API/LIB"
elif sys.platform.startswith('win') or sys.platform.startswith('cli'):
	#scriptDir = "\\".join(__file__.split("\\")[:-1])
	cwd = os.getcwd()
	#scriptDisk = __file__.split(":")[0]
	scriptDisk = cwd.split(":")[0]
	if scriptDisk == "B" or scriptDisk == "b":
		libPath = r"B:/Podpora Revit/Rodiny/141/_STAVEBNI/_REVITPYTHONSHELL/LIB"
	elif scriptDisk == "C" or scriptDisk == "c":
		libPath = r"C:/_WORK/PYTHON/REVIT_API/LIB"
	elif scriptDisk == "H" or scriptDisk == "h":
		libPath = r"H:/_WORK/PYTHON/REVIT_API/LIB"

if sys.platform.startswith('linux'):
	pythLibPath = r"/storage/emulated/0/_WORK/LIB"
#elif sys.platform.startswith('win') or sys.platform.startswith('cli'):
#	pythLibPath = r"C:/_WORK/PYTHON/LIB"

sys.path.append(libPath)
#sys.path.append(pythLibPath)



uidoc = __revit__.ActiveUIDocument
doc = __revit__.ActiveUIDocument.Document

class InfoDialog(Form):
	def __init__(self, inText):
		cwd = os.getcwd()
		#self.scriptDir = "\\".join(__file__.split("\\")[:-1])
		#print(self.scriptDir)
		iconFilename = os.path.join(cwd, 'LIB\\spaceific_64x64_sat_X9M_icon.ico')
		icon = Icon(iconFilename)
		self.Icon = icon	

		self.InfoText = inText
		
		self.InitializeComponent()

	def InitializeComponent(self):
		self.Text = "Setup of join priority for categories by Spaceific-Studio"
		self.Width = 500
		self.Height = 200
		self.StartPosition = FormStartPosition.CenterScreen
		self.TopMost = True
		screenSize = Screen.GetWorkingArea(self)
		self.Height = screenSize.Height / 2
		self.Width = screenSize.Width / 3
		self.panelHeight = self.ClientRectangle.Height * 0.75
		self.panelWidth = self.ClientRectangle.Width / 3
		self.bgColor = Color.CadetBlue
		self.textColor = Color.White
		self.setup()
	
	def setup(self):
		self.label1 = Label()
		self.label1.Text = self.InfoText
		self.label1.Size = Size(200, 50)

		self.Controls.Add(self.label1)

class ProgressBarDialog(Form):
	def __init__(self, inMax):
		cwd = os.getcwd()
		#self.scriptDir = "\\".join(__file__.split("\\")[:-1])
		#print(self.scriptDir)
		iconFilename = os.path.join(cwd, 'LIB\\spaceific_64x64_sat_X9M_icon.ico')
		icon = Icon(iconFilename)
		self.Icon = icon	

		self.Text = 'Unjoin All Selected Elements - Script by Spaceific-Studio'
		self.TopMost = True
		screenSize = Screen.GetWorkingArea(self)
		self.Height = 150
		self.Width = 800
		self.StartPosition = FormStartPosition.CenterScreen
		self.panelHeight = self.ClientRectangle.Height * 0.75
		self.panelWidth = self.ClientRectangle.Width / 3
		self.bgColor = Color.CadetBlue
		self.textColor = Color.White

		self.pb = ProgressBar()
		self.pb.Minimum = 1
		#self.pb.IsIndeterminate = False
		self.pb.Maximum = inMax + 1
		self.pb.Step = 0
		self.pb.Value = 1
		self.pb.Width = self.Width
		self.pb.Height = 30
		self.pb.Location = (Point(0,30))
		self.Controls.Add(self.pb)

		self.progressLabel = Label()
		self.progressLabel.Width = self.pb.Width
		self.progressLabel.Height = 30
		self.progressLabel.Text = "0"
		self.progressLabel.TextAlign = ContentAlignment.MiddleCenter
		self.progressLabel.Location = (Point(0,60))
		self.Controls.Add(self.progressLabel)

		self.infoLabel = Label()
		self.infoLabel.Width = self.pb.Width
		self.infoLabel.Height = 30
		self.infoLabel.TextAlign = ContentAlignment.MiddleCenter
		self.infoLabel.Text = "Unjoining elements..."
		self.infoLabel.Location = (Point(0,0))
		self.Controls.Add(self.infoLabel)
		#self.Shown += self.start
		#System.Threading.Dispatcher.Run(self.start())

	#def start(self, s, e):
	def start(self):
		for i in range(100):
			self.UpdateProgress()
			#System.Threading.Dispatcher.Run(UpdateProgress())
			#self.pb.Dispatcher.Invoke(ProgressBarDelegate(self.UpdateProgress), DispatcherPriority.Background)
			Thread.Sleep(15)
		#t = Thread(ThreadStart(update))
		#t.Start()
	def updateProgressLabel(self, inText):
		self.progressLabel.Text = str(inText)

	def UpdateProgress(self):
		self.pb.Value +=1

firstSelection = [doc.GetElement(elId) for elId in __revit__.ActiveUIDocument.Selection.GetElementIds()]
#firstSelectionElements = [doc.GetElement(x) for x in firstSelection]

openedForms = list(Application.OpenForms)
for i, oForm in enumerate(openedForms):
	print(str(i))
	print(oForm)
	if "RevitPythonShell" in str(oForm):
		#print("Totot je oForm {0}".format(oForm))
		rpsOutput = oForm
	else:
		rpsOutput = None
print("__main__.OpenForms {}".format(list(Application.OpenForms)))
#rpsOutput = list(Application.OpenForms)[0]

if rpsOutput:
	rpsOutput.Hide()
else:
	pass

Application.EnableVisualStyles()
#Application.ProgressBarDialog
pBar = ProgressBarDialog(len(firstSelection))
fThread = Thread(ThreadStart(pBar))
pBar.Show()

t = DB.Transaction(doc, "Select element to join with")
t.Start()
for i, el1 in enumerate(firstSelection):
	try:
		jElementsCol = DB.JoinGeometryUtils.GetJoinedElements(doc,el1)
		jElements = [doc.GetElement(x) for x in list(jElementsCol)]
		#print("{0} - len(jElements) - {1}".format(len(jElements), el1.Id.IntegerValue))
 	except:
		jElementsCol = None
		jElements = None
		#print("No Elements") 
		
	if hasattr(jElements, "__iter__"):
		for j, el2 in enumerate(jElements):
			areJoined = DB.JoinGeometryUtils.AreElementsJoined(doc, el1, el2)
			if areJoined:
				try:
					DB.JoinGeometryUtils.UnjoinGeometry(doc, el1, el2)
					print("{0}-{1} Unjoining el1 - {2} and el2 {3} Done:".format(i, j, el1.Id.IntegerValue, el2.Id.IntegerValue))
				except:
					print("{0}-{1} Unjoining el1 - {2} and el2 {3} error: {2}".format(i, j, el1.Id.IntegerValue, el2.Id.IntegerValue, sys.exc_info()))
	pBar.updateProgressLabel("processing {0} of {1}".format(i, len(firstSelection)))
	pBar.UpdateProgress()
t.Commit()
pBar.Close()
if rpsOutput:
	rpsOutput.Show()
	rpsOutput.TopMost = True

