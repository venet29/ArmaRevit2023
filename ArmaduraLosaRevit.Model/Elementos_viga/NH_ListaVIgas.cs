using System;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.IO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Elementos_viga;
using System.Linq;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.Elementos_viga
{
    public class NH_ListaVIgas
    {
        #region 1) prop y atributos

        #region ATributos de documento
        /// <summary>
        /// object which contains reference to Revit Application
        /// </summary>
        protected Autodesk.Revit.UI.ExternalCommandData m_commandData;
        /// <summary>
        /// Revit UI document
        /// </summary>
        Autodesk.Revit.UI.UIDocument m_rvtUIDoc;
        /// <summary>
        /// Revit DB document
        /// </summary>
        protected Autodesk.Revit.DB.Document _doc;
        private ElementId PhaseId;

        #endregion

        /// <summary>
        /// Lista con los puntos de los poligonos de losa
        /// </summary>
        public List<List<XYZ>> ListaPoligoLosa { get; set; }

        public Level nivelActual { get; set; }

        public List<ProfileBeam> ListaProfileBeam { get; set; }

        #endregion




        #region 2)contructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandData"></param>
        public NH_ListaVIgas(ExternalCommandData commandData)
        {
            m_commandData = commandData;
            m_rvtUIDoc = m_commandData.Application.ActiveUIDocument;
            ListaPoligoLosa = new List<List<XYZ>>();
            ListaProfileBeam = new List<ProfileBeam>();
            _doc = m_rvtUIDoc.Document;

            View view = _doc.ActiveView;
            nivelActual = view.GenLevel;

        }

        #endregion


        #region 3) metodos

        /// <summary>
        /// 1)obtiene lista de vigas en el nivel de trabajo 
        /// 2)obtiene su  geometria ProfileBeam

        /// </summary>
        public void GetVigaPoligonos(View _view)
        {
            var listaViga = SeleccionarViga.GetVigaFromLevelv2(_doc, _view);

            foreach (var familyInstanceViga in listaViga)
            {

                if (null != familyInstanceViga)
                {

                    //nivel de la viga seleccionado
                    // Level nivel = (Level)familyInstanceViga.Host;

                    ProfileBeam profileBeam = new ProfileBeam(familyInstanceViga, m_commandData, _view.GenLevel);
                    Debug.Print(profileBeam.m_data.Name);
                    if (profileBeam != null)
                    {
                        if (profileBeam != null) { ListaProfileBeam.Add(profileBeam); }
                    }
                }
            }

        }


        /// <summary>
        /// borras las  lineas Room separation ---> 
        /// </summary>
        public void BorrarTodas()
        {


            ICollection<ElementId> linesId = new FilteredElementCollector(_doc)
                                                .OfClass(typeof(CurveElement)).Where(q => q.Category.Id == new ElementId(BuiltInCategory.OST_RoomSeparationLines) &&
                                                TipoPhases_.IsFasesExistenete(q)
                                                && (q as ModelLine).LevelId == (nivelActual as Element).Id).Select(rr => rr.Id).ToList();

            if (linesId != null)
                _doc.Delete(linesId);

        }

        /// <summary>
        /// Genera LAs lineas de separacion de rooms
        /// 
        /// Utiliza la lista de ListaVigas de 'ProfileBeam' que contiene las poligonos de la
        /// parte superior de cada viga.
        /// 
        /// Crea Una linea y despues con 'm_document.Create.NewRoomBoundaryLines' lo transforma en
        /// 'SeparateRoom'
        /// </summary>
        public void DibujarLineasSeparacicionRoom()
        {
            ICollection<ElementId> listt = new List<ElementId>();

            
            if (!TipoPhases_.ObtenerFasesExistenete(_doc)) return;
            PhaseId = TipoPhases_.idPhase;

            foreach (var profileBeam in ListaProfileBeam)
            {
                // se sibujam las lines
                List<Line> listaRoomSeparator = profileBeam.CrearSeparacionRoom(_doc);

                CurveArray cArray = new CurveArray();
                foreach (Curve item in listaRoomSeparator)
                {
                    cArray.Append(item);
                }
                if (cArray.Size == 0) continue;
                SketchPlane skP = SketchPlane.Create(_doc, profileBeam.NivelLosa.Id);
                var linea = _doc.Create.NewRoomBoundaryLines(skP, cArray, _doc.ActiveView);
                if (linea == null) continue;
                //        cambiar paramatro PHASE_CREATED =Existing


                foreach (var item in linea)
                {
                    Element line_e = item as Element;
                    // get the phase id "New construction"

                    line_e.get_Parameter(BuiltInParameter.PHASE_CREATED).Set(PhaseId);
                }


            }

        }

        /// <summary>
        /// borra elemntos 'ProfileBeam' de la lista  ListaProfileBeam
        /// </summary>
        public void ClearProfileBeam()
        {
            ListaProfileBeam.Clear();
        }




    

        #endregion




    }
}
