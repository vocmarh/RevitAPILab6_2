﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RevitAPILab6_2
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        public List<FamilySymbol> Furniture { get; } = new List<FamilySymbol>();
        public List<Level> Levels { get; } = new List<Level> { };
        public DelegateCommand SaveCommand { get; }
        public XYZ InsertionPoint { get; }
        public FamilySymbol SelectedFurniture { get; set; }
        public Level SelectedLevel { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Furniture = FurnitureUtils.GetFurniture(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(onSaveCommand);
            InsertionPoint = SelectionUtils.GetPoint(_commandData, "Выберите точку", ObjectSnapTypes.Endpoints);
        }
        private void onSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (SelectedFurniture == null || SelectedLevel == null)
                return;

            var oLevel = (Level)doc.GetElement(SelectedLevel.Id);
            var oFamSymb = (FamilySymbol)doc.GetElement(SelectedFurniture.Id);

            FamilyInstanceUtils.CreateFamilyInstance(_commandData, oFamSymb, InsertionPoint, oLevel);

            RaiseCloseRequest();
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}