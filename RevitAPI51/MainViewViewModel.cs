using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI51
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand ShowPipesCommand { get; }
        public DelegateCommand ShowWallVolumeCommand { get; }
        public DelegateCommand ShowDoorsCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            ShowPipesCommand = new DelegateCommand(OnSelectCommand1);
            ShowWallVolumeCommand = new DelegateCommand(OnSelectCommand2);
            ShowDoorsCommand = new DelegateCommand(OnSelectCommand3);

        }

        private void OnSelectCommand1()
        {
            RaiseCloseRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var pipeList = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Cast<Pipe>()
                .ToList();

            TaskDialog.Show("Сообщение", $"Количество элементов труб: {pipeList.Count.ToString()}");
        }

        private void OnSelectCommand2()
        {
            RaiseCloseRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var wallList = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();

            double totalValume = 0;

            foreach (Wall wall in wallList)
            {
                var volume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                totalValume += volume;
            }

            TaskDialog.Show("Сообщение", $"Объем всех стен: {totalValume}");
        }

        private void OnSelectCommand3()
        {
            RaiseCloseRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<FamilyInstance> doorList = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .ToList();

            TaskDialog.Show("Сообщение", $"Количество дверей в модели: {doorList.Count.ToString()}");
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        //private void OnSelectCommand()
        //{
        //    RaiseHideRequest();

        //    Autodesk.Revit.DB.Element oElement = SelectionUtils.PickObject(_commandData);

        //    TaskDialog.Show("Сообщение", $"ID: {oElement.Id}");

        //    RaiseShowRequest();
        //}

    }
}
