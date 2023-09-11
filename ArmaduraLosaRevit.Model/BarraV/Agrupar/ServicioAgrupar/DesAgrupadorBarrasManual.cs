using ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.model;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.ServicioAgrupar
{
    public class DesAgrupadorBarrasManual : AgrupadorBarras
    {
        public List<AdministradorIndividual> ListAdministradorIndiviraul { get; set; }
        public DesAgrupadorBarrasManual(UIApplication uiapp, List<IndependentTag> listIndependentTag) : base(uiapp, listIndependentTag)
        {
            ListAdministradorIndiviraul = new List<AdministradorIndividual>();
        }

        public void DibujarTagDeGrupo()
        {
            //  GenerarNuevaDirectizDTO _generarNuevaDirectizDTO = ObtenerGenerarNuevaDirectizDTO();
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Agrupar directriz-NH");
                    foreach (AdministradorIndividual administradorGrupos in ListAdministradorIndiviraul)
                    {
                        //agregar tag
                        administradorGrupos.M1_ObtenerDatosPreDibujo();
                        administradorGrupos.M2_GenerarNuevaDirectriz();

                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }



        internal void ConfigurarDesAgrupar()
        {
            double DeltaLargo = Util.CmToFoot(2);
            double DeltaZ = Util.CmToFoot(2);

            GenerarNuevaDirectizDTO _generarNuevaDirectizDTO_inicial = ObtenerGenerarNuevaDirectizDTO();
            List<BarraIng> listOrdenada = new List<BarraIng>();
            //analiza caso puntoal
            if (_generarNuevaDirectizDTO_inicial.OrientacionSeleccion == Enumeraciones.Orientacion.derecha)
                listOrdenada = ListBarraIng.OrderByDescending(c => c.distaciaDesdeOrigen).ToList();
            else
                listOrdenada = ListBarraIng.OrderBy(c => c.distaciaDesdeOrigen).ToList();

            XYZ delta = _generarNuevaDirectizDTO_inicial.ptoInserciontag;
            List<BarraIng> listGrupoDiam = new List<BarraIng>();
            for (int j = 0; j < listOrdenada.Count; j++)
            {
                XYZ deltaAux = null;
                if (_generarNuevaDirectizDTO_inicial.OrientacionSeleccion == Enumeraciones.Orientacion.derecha)                    
                     deltaAux = delta.ObtenerCopia() + _view.RightDirection * Util.CmToFoot(30) * j - new XYZ(0, 0, Util.CmToFoot(15)) * (listOrdenada.Count-j);
                else
                    deltaAux = delta.ObtenerCopia() - _view.RightDirection * Util.CmToFoot(30) * j - new XYZ(0, 0, Util.CmToFoot(15)) * (listOrdenada.Count - j);

                _generarNuevaDirectizDTO_inicial = ObtenerGenerarNuevaDirectizDTO();

                _generarNuevaDirectizDTO_inicial.ptoInserciontag = deltaAux;
                BarraIng BarraIngAnal = ListBarraIng[j];
                AdministradorIndividual newAdministradorGrupos = new AdministradorIndividual(_uiapp, BarraIngAnal, _generarNuevaDirectizDTO_inicial);
                ListAdministradorIndiviraul.Add(newAdministradorGrupos);
            }
        }


    }
}
