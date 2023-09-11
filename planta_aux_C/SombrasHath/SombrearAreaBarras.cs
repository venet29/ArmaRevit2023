

using System.Collections.Generic;

using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.AutoCAD.EditorInput;

using Autodesk.AutoCAD.Runtime;

using Autodesk.AutoCAD.Geometry;

using Autodesk.AutoCAD.GraphicsInterface;

using Autodesk.AutoCAD.Colors;

using Autodesk.AutoCAD.BoundaryRepresentation;

using BrFace = Autodesk.AutoCAD.BoundaryRepresentation.Face;

using BrException = Autodesk.AutoCAD.BoundaryRepresentation.Exception;


namespace planta_aux_C.SombrasHath
{
    public class SombrearAreaBarras
    {
        // Static color index for auto-incrementing
        static int _index = 1;
        // Keep a list of the things we've drawn
        // so we can undraw them

        List<Drawable> _drawn = new List<Drawable>();


        [CommandMethod("SHADEIDX")]
        public void ResetShadingIndex()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            PromptIntegerOptions pio = new PromptIntegerOptions("\nEnter start vaue for color index shading: ");
            pio.LowerLimit = 1;
            pio.UpperLimit = 256;
            pio.DefaultValue = _index;
            pio.UseDefaultValue = true;

            PromptIntegerResult pir = ed.GetInteger(pio);

            if (pir.Status == PromptStatus.OK)
            {
                _index = pir.Value;
            }

        }


        [CommandMethod("SHADECLEAR")]
        public void ClearShading()
        {
            // Clear any graphics we've drawn with the transient
            // graphics API, then clear the list
            TransientManager tm = TransientManager.CurrentTransientManager;
            IntegerCollection ic = new IntegerCollection();
            foreach (Drawable d in _drawn)
            {
                tm.EraseTransient(d, ic);
            }
            _drawn.Clear();
        }


       //************************************************************************************************
        public void SHADEnh2(int _indexnh)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {


                PromptEntityOptions peo = new PromptEntityOptions("\nSelect a Line >>");
                peo.SetRejectMessage("\nYou have to select the Line only >>");
                peo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Polyline), false);
                PromptEntityResult res;
                res = ed.GetEntity(peo);
                if (res.Status != PromptStatus.OK)
                    return;
                Autodesk.AutoCAD.DatabaseServices.Polyline pline = (Autodesk.AutoCAD.DatabaseServices.Polyline)tr.GetObject(res.ObjectId, OpenMode.ForWrite);
                CreateHatch2(pline, _indexnh);
            }
        }

        public void CreateHatch2(Autodesk.AutoCAD.DatabaseServices.Polyline pline, int _indexnh)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {

                BlockTable acBlkTbl = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Point3dCollection verts = new Point3dCollection();
                for (int i = 0; i < pline.NumberOfVertices; i++)
                {
                    verts.Add(pline.GetPoint3dAt(i));
                }

                Hatch hat = CreateFromVerticesNH_2(tr, acBlkTblRec, verts, _indexnh);
                // If we get some back, get drawables for them and
                // pass them through to the transient graphics API
                if (hat != null)
                {
                    TransientManager tm = TransientManager.CurrentTransientManager;
                    IntegerCollection ic = new IntegerCollection();
                    tm.AddTransient(hat, TransientDrawingMode.Main, 0, ic);
                    _drawn.Add(hat);
                }
                tr.Commit();
            }
        }


        public void CreateHatch2(Point3dCollection verts, int _indexnh)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {

                BlockTable acBlkTbl = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Hatch hat = CreateFromVerticesNH_2(tr, acBlkTblRec, verts, _indexnh);
                // If we get some back, get drawables for them and
                // pass them through to the transient graphics API
                if (hat != null)
                {
                    TransientManager tm = TransientManager.CurrentTransientManager;
                    IntegerCollection ic = new IntegerCollection();
                    tm.AddTransient(hat, TransientDrawingMode.Main, 0, ic);
                    _drawn.Add(hat);
                }
                tr.Commit();
            }
        }


        private Hatch CreateFromVerticesNH_2(Transaction tr, BlockTableRecord btr, Point3dCollection verts, int _indexnh)
        {
            if (verts.Count > 2)
            {
                // Create our first plane based on the first
                // three points in our list (hopefully are not
                // co-linear... maybe ought to check for this)
                Vector3d u = verts[1] - verts[0];
                Vector3d v = verts[2] - verts[0];
                Point3d origin = verts[0];
                Plane plane = new Plane(origin, u.DivideBy(u.Length), v.DivideBy(v.Length));

                // Now recreate our plane from the first point and
                // the normal of the temporary one (seems a little
                // lazy - maybe there's a more elegant way to create
                // an unbounded plane)
                plane = new Plane(origin, plane.Normal);
                // Create our polyline boundary, setting the normal
                Autodesk.AutoCAD.DatabaseServices.Polyline pl = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                pl.Normal = plane.Normal;
                // Add our various vertices projected onto the plane
                // of the polyline
                foreach (Point3d vert in verts)
                {
                    Point2d pt = vert.Convert2d(plane);
                    pl.AddVertexAt(pl.NumberOfVertices, pt, 0.0, 0.0, 0.0);
                }
                // Close our polyline and add it to the owning
                // block table record (we'll soon erase it) and the
                // transaction
                pl.Closed = true;

                ObjectIdCollection ids = new ObjectIdCollection();
                ids.Add(btr.AppendEntity(pl));
                tr.AddNewlyCreatedDBObject(pl, true);
                // Create our hatch
                Hatch hat = new Hatch();
                hat.Normal = pl.Normal;
                // Solid fill of our auto-incremented colour index
                hat.SetHatchPattern(HatchPatternType.PreDefined, "SOLID");
                hat.ColorIndex = _indexnh;
                // Set our transparency to 25% (=127)
                // Alpha value is Truncate(255 * (100-n)/100)
                hat.Transparency = new Transparency(63);
                // Add the hatch loop
                hat.AppendLoop(HatchLoopTypes.Default, ids);
                // Erase our polyline boundary
                pl.Erase();
                // Complete the hatch
                hat.EvaluateHatch(true);
                // Transform the hatch away from the origin
                Matrix3d mat = Matrix3d.Displacement(origin - Point3d.Origin);
                hat.TransformBy(mat);
                return hat;

            }

            return null;

        }



    }
}
