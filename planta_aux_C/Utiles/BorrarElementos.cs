
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

namespace planta_aux_C.Utiles
{
    public class BorrarElementos
    {
        #region 0)propiedades

        public int diamtro { get; set; }
        public int espaciamiento { get; set; }
        public int cantidad { get; set; }
        public double cuantia { get; set; }
        public Point2d p1 { get; set; }
        public Point2d p2 { get; set; }


        CODIGOS_GRUPOS _planta_agrupa = new CODIGOS_GRUPOS();
        ObjectIdCollection ents = new ObjectIdCollection();
        ObjectId[] acObjId_grup__;
        Database db;







        #endregion


        #region 1)CONSTRUCTOR

        #endregion


        #region 2) metodos

        //borrar todos los elemtnos de un agrupo
        public void DiamtroEspaciamientoCuantiaElementos(ObjectId ObjectId)
        {

            db = AcApp.DocumentManager.MdiActiveDocument.Database;
            acObjId_grup__ = _planta_agrupa.buscar_grupo(ObjectId);

            if (acObjId_grup__ == null) return;
            if (acObjId_grup__.Length == 0) return;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId idObj in acObjId_grup__)
                {
                    if (idObj.ObjectClass.DxfName.ToString() == "TEXT")
                    { 
                       DBText acEnt  = tr.GetObject(idObj, OpenMode.ForRead) as DBText;
                       if (acEnt.TextString.ToLower().IndexOf("c", 0) != -1 && acEnt.TextString.ToLower().IndexOf("a", 0) != -1)
                        {
                             //Dim split_valor As String() = Replace(acEnt.TextString, "%%c", ",").Split(New [Char]() {","c})
                            var pto = acEnt.TextString.ToLower().Split(new char[] { 'c', 'a'});
                            diamtro = Convert.ToInt32(pto[1]);
                            espaciamiento = Convert.ToInt32(pto[2]);
                            cuantia = (Math.PI * ((double)diamtro / 20) * ((double)diamtro / 20)) /( (double)espaciamiento / 100);
                        }
               
                 
                    }

                }

                tr.Commit();
            }



        }


        //borrar todos los elemtnos de un agrupo
        public void BorrarTodosElementos(ObjectId ObjectId)
        {

            db = AcApp.DocumentManager.MdiActiveDocument.Database;
            acObjId_grup__ = _planta_agrupa.buscar_grupo(ObjectId);

            if (acObjId_grup__ == null) return;
            if (acObjId_grup__.Length == 0) return;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId idObj in acObjId_grup__)
                {
                    Entity acEnt_barra_aux = tr.GetObject(idObj, OpenMode.ForWrite) as Entity;
                    if (!(acEnt_barra_aux.IsErased)) acEnt_barra_aux.Erase();
                }

                tr.Commit();
            }



        }

        #endregion

    }
}
