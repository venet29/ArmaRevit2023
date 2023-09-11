using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.AyudasView
{
    public class CrearViewNH
    {
        private Document _doc;
        private readonly double anchocm;
        private readonly double largocm;
        private View _view;
        private BoundingBoxXYZ sectionBox;

        public ViewSection section { get; private set; }
        public string _name { get; }

        ViewFamilyType vft;
        private XYZ p1;
        private XYZ p2;
        public CrearViewNH(Document doc, string name = "")
        {


            this._doc = doc;
            this._view = _doc.ActiveView;
            this._name = name;
        }
        public CrearViewNH(Document doc, double anchTransViewocm, double largoProfundViewcm, string name = "")
        {


            _doc = doc;
            this.anchocm = anchTransViewocm;
            this.largocm = largoProfundViewcm;
            this._view = _doc.ActiveView;
            this._name = name;
        }

        public bool M1_CrearDetailViewConTrasn(XYZ p1, XYZ p2)
        {
            try
            {
                p1 = p1 - _view.ViewDirection * Util.CmToFoot(10);
                p2 = p2 - _view.ViewDirection * Util.CmToFoot(10);
                Line cc = Line.CreateBound(p2, p1);
                vft = TiposViewFamily.ObtenerTiposViewFamily(ViewFamily.Detail, _doc);
                sectionBox = AyudaGenerarBoundingBoxXYZ.GetSectionViewPerpendiculatToWall(cc, Util.CmToFoot(40), Util.CmToFoot(100),_view);
                using (Transaction tr = new Transaction(_doc, "CrearDeteilView-NH"))
                {
                    tr.Start();

                    ViewSection.CreateSection(_doc, vft.Id, sectionBox);
                    tr.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        public bool M2_CrearDetailViewConTrasn2(List<XYZ> ListaPtos)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "CrearDeteilView-NH"))
                {
                    tr.Start();
                    M2_1_CrearDetailViewSinTrans(ListaPtos);
                    tr.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
      

        public bool M2_1_CrearDetailViewSinTrans(List<XYZ> ListaPtos)
        {
            try
            {
                p1 = ListaPtos[0];
                p2 = ListaPtos[1];


                Line cc = Line.CreateBound(p2, p1);
                vft = TiposViewFamily.ObtenerTiposViewFamily(ViewFamily.Detail, _doc);
                //seccionbox  horizontal y se dibuja hacia abajo -z
                sectionBox = AyudaGenerarBoundingBoxXYZ.GetSectionViewPerpendiculatToWall(cc, p1.DistanceTo(p2)+ Util.CmToFoot(anchocm), Util.CmToFoot(largocm),_view);
                section = ViewSection.CreateDetail(_doc, vft.Id, sectionBox);
                section.Scale = 20;
               // section.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION_VISIBLE).Set(0);
                if (section.IsSplitSection())
                {
                 //   section.get_Parameter(BuiltInParameter.VIEWER_BOUND_FAR_CLIPPING).Set(_name);
                }

                // noi se puede repetir
                if (_name != "")
                {
                    section.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION).Set(_name);
                    section.get_Parameter(BuiltInParameter.VIEW_NAME).Set(_name);
                }

            }
            catch (Exception ex)
            {

                UtilDesglose.ErrorMsg($"Error al generar COrte de vista  ex:{ex.Message}");

                return false;
            }
            return true;
        }




        public bool M3_CrearDetailViewConTrasn2(BoundingBoxXYZ _BoundingBoxXYZ)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "CrearDeteilView-NH"))
                {
                    tr.Start();
                    M3_2_CrearDetailViewSinTrans(_BoundingBoxXYZ);
                    tr.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public bool M3_2_CrearDetailViewSinTrans(BoundingBoxXYZ _BoundingBoxXYZ)
        {
            try
            {
                vft = TiposViewFamily.ObtenerTiposViewFamily(ViewFamily.Detail, _doc);
                section = ViewSection.CreateDetail(_doc, vft.Id, _BoundingBoxXYZ);
                section.Scale = 20;
                // section.get_Parameter(BuiltInParameter.VIEWER_CROP_REGION_VISIBLE).Set(0);
                if (section.IsSplitSection())
                {
                    //   section.get_Parameter(BuiltInParameter.VIEWER_BOUND_FAR_CLIPPING).Set(_name);
                }

                // noi se puede repetir
                if (_name != "")
                {
                    section.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION).Set(_name);
                    section.get_Parameter(BuiltInParameter.VIEW_NAME).Set(_name);
                }

            }
            catch (Exception ex)
            {

                UtilDesglose.ErrorMsg($"Error al generar COrte de vista  ex:{ex.Message}");

                return false;
            }
            return true;
        }

    }
}
