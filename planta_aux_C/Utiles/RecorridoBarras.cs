
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VARIOS;
using Microsoft.VisualBasic;
using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using Autodesk.Windows;
using planta_aux_C;


using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using planta_aux_C.Elementos;



namespace planta_aux_C.Utiles
{
    public class RecorridoBarras
    {
        #region 0)propiedades


        CODIGOS_GRUPOS _planta_agrupa;
        ObjectIdCollection ObjectIdCollection_;
        ObjectId[] acObjId_grup__;


        public Point3d xline1_tp { get; set; }
        public Point3d xline2_tp { get; set; }
        public ObjectId ObjectId_recorrido { get; set; }
        public ObjectId ObjectId_pelota { get; set; }
        public List<ObjectId> ListaObjectIdMover { get; set; }
        #endregion


        #region 1)CONSTRUCTOR

        public RecorridoBarras()
        {
            ListaObjectIdMover = new List<ObjectId>();
            ObjectIdCollection_ = new ObjectIdCollection();
            _planta_agrupa = new CODIGOS_GRUPOS();
            xline1_tp = new Point3d();
            xline2_tp = new Point3d();
        }
        #endregion


        #region 2)metodos

        public Point3dCollection ObtenerCoordenadasRecorrido(ObjectId ObjectId)
        {

            Database db;
            Point3dCollection collec = new Point3dCollection();
            db = AcApp.DocumentManager.MdiActiveDocument.Database;
            acObjId_grup__ = _planta_agrupa.buscar_grupo(ObjectId);

            if (acObjId_grup__ == null) return collec;
            if (acObjId_grup__.Length == 0) return collec;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId idObj in acObjId_grup__)
                {
                    if (idObj.ObjectClass.DxfName.ToString() == "DIMENSION")
                    {
                        ListaObjectIdMover.Add(idObj);
                        ObjectId_recorrido = idObj;
                        // este ultimo no es necesario --> Dimension acEnt_barra_aux = tr.GetObject(idObj, OpenMode.ForRead) as Dimension;
                        // RotatedDimension hereda de Dimension
                        RotatedDimension acEnt_barra_aux2 = tr.GetObject(idObj, OpenMode.ForRead) as RotatedDimension;

                        xline1_tp = acEnt_barra_aux2.XLine1Point;
                        xline2_tp = acEnt_barra_aux2.XLine2Point;
                        collec.Add(xline1_tp);
                        collec.Add(xline2_tp);
                    }
                    else if (idObj.ObjectClass.DxfName.ToString() == "CIRCLE")
                    {

                        ObjectId_pelota = idObj;
                        ListaObjectIdMover.Add(idObj);

                    }

                }

                tr.Commit();
            }

            return collec;
        }

        public bool MOdificarCoordenadasRecorrido(ObjectId ObjectId, Point3d aux_xline1_tp, Point3d aux_xline2_tp)
        {
            bool result = false;
            Database db;
            Point3dCollection collec = new Point3dCollection();
            db = AcApp.DocumentManager.MdiActiveDocument.Database;
            acObjId_grup__ = _planta_agrupa.buscar_grupo(ObjectId);

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId idObj in acObjId_grup__)
                {
                    if (idObj.ObjectClass.DxfName.ToString() == "DIMENSION")
                    {
                        ListaObjectIdMover.Add(idObj);
                        ObjectId_recorrido = idObj;
                        // este ultimo no es necesario --> Dimension acEnt_barra_aux = tr.GetObject(idObj, OpenMode.ForRead) as Dimension;
                        // RotatedDimension hereda de Dimension
                        RotatedDimension acEnt_barra_aux2 = tr.GetObject(idObj, OpenMode.ForWrite) as RotatedDimension;

                        xline1_tp = aux_xline1_tp;
                        xline2_tp = aux_xline2_tp;
                        acEnt_barra_aux2.XLine1Point = xline1_tp;
                        acEnt_barra_aux2.XLine2Point = xline2_tp;
                        result = true;
                    }
                }

                tr.Commit();
            }

            return result;
        }

        public bool ExtenderCoordenadasRecorrido(ObjectId ObjectId, Point3d p1, Point3d p2)
        {
            bool result = false;
            Database db;
            db = AcApp.DocumentManager.MdiActiveDocument.Database;
            acObjId_grup__ = _planta_agrupa.buscar_grupo(ObjectId);

            if (acObjId_grup__ == null) return result;
            if (acObjId_grup__.Length == 0) return result;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId idObj in acObjId_grup__)
                {
                    if (idObj.ObjectClass.DxfName.ToString() == "DIMENSION")
                    {
                        // Dim excur As Entity = tr.GetObject(idObj, OpenMode.ForWrite)
                        Dimension acEnt_barra_aux = tr.GetObject(idObj, OpenMode.ForWrite) as Dimension;
                        RotatedDimension acEnt_barra_aux2 = tr.GetObject(idObj, OpenMode.ForWrite) as RotatedDimension;

                        acEnt_barra_aux.Dimexe = 10;
                        acEnt_barra_aux2.XLine1Point = p1;
                        acEnt_barra_aux2.XLine2Point = p2;
                        result = true;
                    }
                }

                tr.Commit();
            }

            return result;
        }
        public bool ExtenderCoordenadasRecorrido(Point3d p1, Point3d p2)
        {
            bool result = false;
            Database db;
            db = AcApp.DocumentManager.MdiActiveDocument.Database;


            using (Transaction tr = db.TransactionManager.StartTransaction())
            {

                // Dim excur As Entity = tr.GetObject(idObj, OpenMode.ForWrite)
                Dimension acEnt_barra_aux = tr.GetObject(ObjectId_recorrido, OpenMode.ForWrite) as Dimension;
                RotatedDimension acEnt_barra_aux2 = tr.GetObject(ObjectId_recorrido, OpenMode.ForWrite) as RotatedDimension;

                acEnt_barra_aux.Dimexe = 10;
                acEnt_barra_aux2.XLine1Point = p1;
                acEnt_barra_aux2.XLine2Point = p2;
                result = true;


                tr.Commit();
            }

            return result;
        }

        public PtosCrearSuples aux_UnirRecorridos()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;



            PtosCrearSuples newSuple = new PtosCrearSuples();
            //************************************
            //a) selecionar
            PromptEntityOptions peo = new PromptEntityOptions("\n1)Seleccionar Recorrido 1 ");
            peo.SetRejectMessage("SELECCIONAR SOLO RECORRIDO");
            peo.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult supleBorrado1_seleccionado = acDocEd.GetEntity(peo);
            if (supleBorrado1_seleccionado.Status != PromptStatus.OK) return newSuple;

            RecorridoBarras recorrdioReferencia_ = new RecorridoBarras();
            //b) Obtener puntos barra
            var listaPtos1 = DatosBarras.TIPO_caso_BARRA_LOSA_ind_1barra_v2(supleBorrado1_seleccionado.ObjectId, "s1");


            //c)obtener cuantia


            //d)borrar todos los elemtos
            BorrarElementos BorrarElementos_ = new BorrarElementos();

            var pto_1 = listaPtos1[4].Split(new char[] { ',', '_' });
            BorrarElementos_.p1 = new Point2d(Convert.ToDouble(pto_1[0]), Convert.ToDouble(pto_1[1]));
            BorrarElementos_.p2 = new Point2d(Convert.ToDouble(pto_1[2]), Convert.ToDouble(pto_1[3]));
            BorrarElementos_.DiamtroEspaciamientoCuantiaElementos(supleBorrado1_seleccionado.ObjectId);

            //*******************************************
            //a) selecionar
            PromptEntityOptions peo2 = new PromptEntityOptions("\n2)Seleccionar Recorrido2 ");
            peo2.SetRejectMessage("SELECCIONAR SOLO RECORRIDO");
            peo2.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult supleBorrado2_seleccionado = acDocEd.GetEntity(peo2);
            if (supleBorrado2_seleccionado.Status != PromptStatus.OK) return newSuple;

            if (supleBorrado1_seleccionado.ObjectId == supleBorrado2_seleccionado.ObjectId)
            {
                System.Windows.Forms.MessageBox.Show("No se puede seleccionar el mismo elemento");
                return newSuple;
            }

            RecorridoBarras recorridioDBorrado_ = new RecorridoBarras();
            //b) Obtener puntos barra
            var listaPtos2 = DatosBarras.TIPO_caso_BARRA_LOSA_ind_1barra_v2(supleBorrado2_seleccionado.ObjectId, "s1");

            //c)botener cuantia


            //d)borrar todos los elemtos
            BorrarElementos BorrarElementos2_ = new BorrarElementos();

            var pto_2 = listaPtos2[4].Split(new char[] { ',', '_' });
            BorrarElementos2_.p1 = new Point2d(Convert.ToDouble(pto_2[0]), Convert.ToDouble(pto_2[1]));
            BorrarElementos2_.p2 = new Point2d(Convert.ToDouble(pto_2[2]), Convert.ToDouble(pto_2[3]));
            BorrarElementos2_.DiamtroEspaciamientoCuantiaElementos(supleBorrado2_seleccionado.ObjectId);


            // revisar angulos
            double angle1 = comunes.coordenada__angulo_p1_p2_losa(BorrarElementos_.p1, BorrarElementos_.p2);
            double angle2 = comunes.coordenada__angulo_p1_p2_losa(BorrarElementos2_.p1, BorrarElementos2_.p2);

            if (Math.Abs(angle2 - angle1) > 0.1)
            {
                System.Windows.Forms.MessageBox.Show("Barras deben tener similar direccion (angulo)");
                return newSuple;
            }

            if (BorrarElementos_.cuantia > BorrarElementos2_.cuantia)
            {
                newSuple.diametro = BorrarElementos_.diamtro;
                newSuple.espaciamiento = BorrarElementos_.espaciamiento;
                newSuple.cuantia = "@" + BorrarElementos_.diamtro.ToString() + "a" + BorrarElementos_.espaciamiento.ToString();
            }
            else
            {
                newSuple.diametro = BorrarElementos2_.diamtro;
                newSuple.espaciamiento = BorrarElementos2_.espaciamiento;
                newSuple.cuantia = "@" + BorrarElementos2_.diamtro.ToString() + "a" + BorrarElementos2_.espaciamiento.ToString();
            }

            Point2d pto_perpendicular;
            if (BorrarElementos_.p1.GetDistanceTo(BorrarElementos2_.p1) > BorrarElementos_.p2.GetDistanceTo(BorrarElementos2_.p1))
            {
                newSuple.pto1 = new Point3d(BorrarElementos_.p1.X, BorrarElementos_.p1.Y, 0);


                if (BorrarElementos_.p1.GetDistanceTo(BorrarElementos2_.p1) > BorrarElementos_.p1.GetDistanceTo(BorrarElementos2_.p2))
                {
                    pto_perpendicular = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(BorrarElementos_.p1, BorrarElementos_.p2, BorrarElementos2_.p1);
                }
                else
                {
                    pto_perpendicular = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(BorrarElementos_.p1, BorrarElementos_.p2, BorrarElementos2_.p2);
                }

            }
            else
            {

                newSuple.pto1 = new Point3d(BorrarElementos_.p2.X, BorrarElementos_.p2.Y, 0);

                if (BorrarElementos_.p2.GetDistanceTo(BorrarElementos2_.p1) > BorrarElementos_.p2.GetDistanceTo(BorrarElementos2_.p2))
                {
                    pto_perpendicular = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(BorrarElementos_.p1, BorrarElementos_.p2, BorrarElementos2_.p1);
                }
                else
                {
                    pto_perpendicular = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(BorrarElementos_.p1, BorrarElementos_.p2, BorrarElementos2_.p2);
                }


            }
            newSuple.pto2 = new Point3d(pto_perpendicular.X, pto_perpendicular.Y, 0);

            //borrar elemntos
            BorrarElementos_.BorrarTodosElementos(supleBorrado1_seleccionado.ObjectId);
            BorrarElementos2_.BorrarTodosElementos(supleBorrado2_seleccionado.ObjectId);

            return newSuple;


        }


        public void aux_BorrarYRecorridoExtenderOtro()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;




            //************************************
            PromptEntityOptions peo = new PromptEntityOptions("\n1)Seleccionar Recorrido a Extender ");
            peo.SetRejectMessage("SELECCIONAR SOLO RECORRIDO");
            peo.AddAllowedClass(typeof(RotatedDimension), true);
            PromptEntityResult recorrdioRefencia_selecionado = acDocEd.GetEntity(peo);
            if (recorrdioRefencia_selecionado.Status != PromptStatus.OK) return;

            RecorridoBarras recorrdioReferencia_ = new RecorridoBarras();
            recorrdioReferencia_.ObtenerCoordenadasRecorrido(recorrdioRefencia_selecionado.ObjectId);
            //*******************************************
            int i = 2;
            while (true)
            {


                PromptEntityOptions peo2 = new PromptEntityOptions("\n" + i + ")Seleccionar Recorrido a Borrar ");
                peo2.SetRejectMessage("SELECCIONAR SOLO RECORRIDO");
                peo2.AddAllowedClass(typeof(RotatedDimension), true);
                PromptEntityResult RecorridoDBorrado_seleccionado = acDocEd.GetEntity(peo2);
                if (RecorridoDBorrado_seleccionado.Status != PromptStatus.OK) return;

                if (recorrdioRefencia_selecionado.ObjectId == RecorridoDBorrado_seleccionado.ObjectId)
                {
                    System.Windows.Forms.MessageBox.Show("No se puede seleccionar el mismo elemento");
                    return;
                }

                RecorridoBarras recorridioDBorrado_ = new RecorridoBarras();
                recorridioDBorrado_.ObtenerCoordenadasRecorrido(RecorridoDBorrado_seleccionado.ObjectId);


                // revisar angulos
                double angle1 = comunes.coordenada__angulo_p1_p2_losa(recorrdioReferencia_.xline1_tp, recorrdioReferencia_.xline2_tp);
                double angle2 = comunes.coordenada__angulo_p1_p2_losa(recorridioDBorrado_.xline1_tp, recorridioDBorrado_.xline2_tp);

                if (Math.Abs(angle2 - angle1) > 0.1)
                {
                    System.Windows.Forms.MessageBox.Show("Recorridos deben tener similar direccion (angulo)");
                    return;
                }


                //*****************************************
                Point3d pto_analisas;
                if (recorrdioReferencia_.xline1_tp.DistanceTo(recorridioDBorrado_.xline1_tp) <
                    recorrdioReferencia_.xline2_tp.DistanceTo(recorridioDBorrado_.xline1_tp))
                {
                    if (recorrdioReferencia_.xline1_tp.DistanceTo(recorridioDBorrado_.xline1_tp) <
                        recorrdioReferencia_.xline1_tp.DistanceTo(recorridioDBorrado_.xline2_tp))
                    { pto_analisas = recorridioDBorrado_.xline2_tp; }
                    else
                    { pto_analisas = recorridioDBorrado_.xline1_tp; }

                }
                else
                {
                    if (recorrdioReferencia_.xline2_tp.DistanceTo(recorridioDBorrado_.xline1_tp) <
                         recorrdioReferencia_.xline2_tp.DistanceTo(recorridioDBorrado_.xline2_tp))
                    { pto_analisas = recorridioDBorrado_.xline2_tp; }
                    else
                    { pto_analisas = recorridioDBorrado_.xline1_tp; }
                }

                var pto_perpendicular = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(recorrdioReferencia_.xline1_tp, recorrdioReferencia_.xline2_tp, pto_analisas);

                //a) borrar 

                BorrarElementos BorrarElementos_ = new BorrarElementos();
                BorrarElementos_.BorrarTodosElementos(recorridioDBorrado_.ObjectId_recorrido);

                //b) extebder
                if (recorrdioReferencia_.xline1_tp.DistanceTo(pto_perpendicular) <
                      recorrdioReferencia_.xline2_tp.DistanceTo(pto_perpendicular))
                { recorrdioReferencia_.ExtenderCoordenadasRecorrido(recorrdioReferencia_.xline2_tp, pto_perpendicular); }
                else
                { recorrdioReferencia_.ExtenderCoordenadasRecorrido(recorrdioReferencia_.xline1_tp, pto_perpendicular); }




                i += 1;
            }

        }


        public void aux_AlinearDosReccorido()
        {
            Editor acDocEd = AcApp.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database; //HostApplicationServices.WorkingDatabase;// ;




            //************************************
            PromptEntityOptions peo = new PromptEntityOptions("\n1)Seleccionar recorrido Referencia ");
            peo.SetRejectMessage("SELECCIONAR SOLO RECORRIDO");
            peo.AddAllowedClass(typeof(RotatedDimension), true);
            PromptEntityResult recorrdioRefencia_selecionado = acDocEd.GetEntity(peo);
            if (recorrdioRefencia_selecionado.Status != PromptStatus.OK) return;

            RecorridoBarras recorrdioReferencia_ = new RecorridoBarras();
            recorrdioReferencia_.ObtenerCoordenadasRecorrido(recorrdioRefencia_selecionado.ObjectId);
            int i = 2;
            //*******************************************
            while (true)
            {


                PromptEntityOptions peo2 = new PromptEntityOptions("\n" + i + ")Seleccionar recorrido a Despalzado ");
                peo2.SetRejectMessage("SELECCIONAR SOLO RECORRIDO");
                peo2.AddAllowedClass(typeof(RotatedDimension), true);
                PromptEntityResult RecorridoDesplazdo_seleccionado = acDocEd.GetEntity(peo2);
                if (RecorridoDesplazdo_seleccionado.Status != PromptStatus.OK) return;

                // VALIDACION
                if (recorrdioRefencia_selecionado.ObjectId == RecorridoDesplazdo_seleccionado.ObjectId)
                {
                    System.Windows.Forms.MessageBox.Show("No se puede seleccionar el mismo elemento");
                    return;
                }

 

                RecorridoBarras recorrdioDesplazado_ = new RecorridoBarras();
                recorrdioDesplazado_.ObtenerCoordenadasRecorrido(RecorridoDesplazdo_seleccionado.ObjectId);


                // revisar angulos
                double angle1 = comunes.coordenada__angulo_p1_p2_losa(recorrdioReferencia_.xline1_tp, recorrdioReferencia_.xline2_tp);
                double angle2 = comunes.coordenada__angulo_p1_p2_losa(recorrdioDesplazado_.xline1_tp, recorrdioDesplazado_.xline2_tp);

                if (Math.Abs(angle2 - angle1) > 0.1)
                {
                    System.Windows.Forms.MessageBox.Show("Recorridos deben tener similar direccion (angulo)");
                    return;
                }

                //*****************************************
                Point3d pto_analisas;
                Point3d pto_inicial_SoloParaEstechar;
                Point3d pto_analisas_SoloParaEstechar;
                if (recorrdioReferencia_.xline1_tp.DistanceTo(recorrdioDesplazado_.xline1_tp) <
                    recorrdioReferencia_.xline2_tp.DistanceTo(recorrdioDesplazado_.xline1_tp))
                {
                    pto_inicial_SoloParaEstechar = recorrdioReferencia_.xline1_tp;
                    if (recorrdioReferencia_.xline1_tp.DistanceTo(recorrdioDesplazado_.xline1_tp) <
                        recorrdioReferencia_.xline1_tp.DistanceTo(recorrdioDesplazado_.xline2_tp))
                    {
                       // pto_analisas = recorrdioDesplazado_.xline1_tp;
                        pto_analisas_SoloParaEstechar = recorrdioDesplazado_.xline2_tp;
                    }
                    else
                    {
                        //pto_analisas = recorrdioDesplazado_.xline2_tp;
                        pto_analisas_SoloParaEstechar = recorrdioDesplazado_.xline1_tp;
                    }

                }
                else
                {
                    pto_inicial_SoloParaEstechar = recorrdioReferencia_.xline2_tp;
                    if (recorrdioReferencia_.xline2_tp.DistanceTo(recorrdioDesplazado_.xline1_tp) <
                         recorrdioReferencia_.xline2_tp.DistanceTo(recorrdioDesplazado_.xline2_tp))
                    {
                       // pto_analisas = recorrdioDesplazado_.xline1_tp;
                        pto_analisas_SoloParaEstechar = recorrdioDesplazado_.xline2_tp;
                    }
                    else
                    {
                        //pto_analisas = recorrdioDesplazado_.xline2_tp;
                        pto_analisas_SoloParaEstechar= recorrdioDesplazado_.xline1_tp;
                    }
                }

                pto_analisas = recorrdioReferencia_.xline1_tp;

                var pto_perpendicular = Intersecciones.PuntoQueIntersectaPerpendicularmenteConLinea_desdeUnPunto(recorrdioDesplazado_.xline1_tp, recorrdioDesplazado_.xline2_tp, pto_analisas);


                if (pto_perpendicular.DistanceTo(pto_analisas) > 1.0)
                {
                    foreach (var objId in recorrdioDesplazado_.ListaObjectIdMover)
                    {
                        if (recorrdioDesplazado_.ObjectId_recorrido != null)
                            Transform.MoveObject(objId, pto_perpendicular, pto_analisas);

                    }
                }
                // que punto analizado analizado ( de barras a trasladar) no se  igual a algun puunto de la barra de referencia
                else// if(pto_perpendicular.DistanceTo(pto_analisas_SoloParaEstechar) > 1.0 && pto_perpendicular.DistanceTo(pto_inicial_SoloParaEstechar) > 1.0)
                {

                    foreach (var objId in recorrdioDesplazado_.ListaObjectIdMover)
                    {
                        if (recorrdioDesplazado_.ObjectId_recorrido != null && objId.ObjectClass.DxfName.ToString() == "DIMENSION")
                        { 
                            MOdificarCoordenadasRecorrido(RecorridoDesplazdo_seleccionado.ObjectId, pto_analisas_SoloParaEstechar, pto_inicial_SoloParaEstechar);
                        }
                    }
                }
                i += 1;
            }
        }


        #endregion

    }
}
