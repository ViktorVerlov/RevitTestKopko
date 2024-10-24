﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitTest.ComponentRevit.Extensions.ExtenstionSelections;
using RevitTest.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

<<<<<<<< HEAD:ComponentRevit/Handlers/IPickElementHandler.cs
namespace RevitTest.Handlers
========
namespace RevitTest.ComponentRevit
>>>>>>>> 6c556d0a42a0eac62ebf137197112da9d73bf5b9:ComponentRevit/PickElementHandler.cs
{
    internal class PickElementHandler : IExternalEventHandler
    {
        private readonly MainViewModel _mainViewModel;

        public PickElementHandler(MainViewModel viewModel)
        {
            _mainViewModel = viewModel;
        }

        public void Execute(UIApplication app)
        {
            var uidoc = app.ActiveUIDocument;
            var doc = uidoc.Document;

            try
            {

                var references = uidoc.Selection.PickObjects(ObjectType.Element, new SelectionsFilter(
                    e => e is FamilyInstance fi &&
                         fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows
                ));

                var selectedElementIds = references.Select(r => r.ElementId).ToList();


                _mainViewModel.ClearRevitElements();

                HashSet<ElementId> uniqueTypes = new HashSet<ElementId>();

                foreach (var elementId in selectedElementIds)
                {
                    var windowFromDoc = doc.GetElement(elementId);
                    var windowTypeFromDoc = windowFromDoc.GetTypeId();
                    uniqueTypes.Add(windowTypeFromDoc);
                }

                foreach (var uniqueTypeId in uniqueTypes)
                {

                    var windowTypeElement = doc.GetElement(uniqueTypeId) as FamilySymbol;

                    if (windowTypeElement != null)
                    {
                        string name = windowTypeElement.Name;
                        double width = windowTypeElement.get_Parameter(BuiltInParameter.WINDOW_WIDTH).AsDouble();
                        double height = windowTypeElement.get_Parameter(BuiltInParameter.WINDOW_HEIGHT).AsDouble(); 

                        var viewModel = new WindowFamilyTypeViewModel(name, uniqueTypeId, false, width, height);
                        _mainViewModel.AddRevitElement(viewModel);
                    }
                }


<<<<<<<< HEAD:ComponentRevit/Handlers/IPickElementHandler.cs



========
                // _mainViewModel.SelectedItems.Clear();
                // foreach (var element in _mainViewModel.RevitElements)
                // {
                //     _mainViewModel.SelectedItems.Add(element);
                // }
>>>>>>>> 6c556d0a42a0eac62ebf137197112da9d73bf5b9:ComponentRevit/PickElementHandler.cs
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {

            }
        }

        public string GetName()
        {
            return "Выбор элементов";
        }
    }

}