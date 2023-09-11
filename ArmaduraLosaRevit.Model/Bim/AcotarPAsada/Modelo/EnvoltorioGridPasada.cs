using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GRIDS.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo
{
    public class EnvoltorioGridPasada: EnvoltorioGrid
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly Transform transform;

        public EnvoltorioGridPasada(UIApplication uiapp, Element c, Transform transform_):base((Autodesk.Revit.DB.Grid)c)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.transform = transform_;

        }

    }
}

