//
// (C) Copyright 2003-2017 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ArmaduraLosaRevit.Model.Elementos_viga
{
    /// <summary>
    /// ProfileBeam class contains the information about profile of beam,
    /// and contains method used to create opening on a beam.
    /// </summary>
    public class ProfileOpening : Profile
    {

        public Element m_data { get; set; }

        //lista de putos de face superior
        public List<List<XYZ>> ListaPtoEdgeFaceUP { get; set; }

        //store the transform used to change points in beam coordinate system to Revit coordinate system
        Transform m_beamTransform = null;
        bool m_isZaxis = true; //decide whether to create opening on Zaxis of beam or Yaixs of beam
                               //if m_haveOpening is true means beam has already had opening on it
                               //then the points get from get_Geometry(Option) do not need to be transformed 
                               //by the Transform get from Instance object anymore.
        bool m_haveOpening = false;
        Matrix4 m_MatrixZaxis = null; //transform points to plane whose normal is Zaxis of beam
        Matrix4 m_MatrixYaxis = null; //transform points to plane whose normal is Yaxis of beam

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="beam">beam to create opening on</param>
        /// <param name="commandData">object which contains reference to Revit Application</param>
        public ProfileOpening(Element opening, UIApplication _uiapp) : base(_uiapp)
        {
            ListaPtoEdgeFaceUP = new List<List<XYZ>>();
            m_data = opening;
            getlineasContorno();
            //List<List<Edge>> faces = GetFaces(m_data);

            //otra forma de obtener la cara superior usando arreglos de edge
            //NOTA:  no entrega la normal solo entrega la primera loop
            // m_points = GetNeedPointsNormalUpFace(faces);

            //m_points = GetNeedPoints(faces);
            //m_to2DMatrix = GetTo2DMatrix();
            //m_moveToCenterMatrix = ToCenterMatrix();
        }
        public ProfileOpening(Element opening, UIApplication _uiapp, Level level) : base(_uiapp, level)
        {
            ListaPtoEdgeFaceUP = new List<List<XYZ>>();
            m_data = opening;
           // List<List<Edge>> faces = GetFaces(m_data);
            NivelLosa = level;
            getlineasContorno();
            //otra forma de obtener la cara superior usando arreglos de edge
            //NOTA:  no entrega la normal solo entrega la primera loop
            // m_points = GetNeedPointsNormalUpFace(faces);
            //  m_points = GetNeedPoints(faces);
            //if (m_points.Count > 0)
            //{
            //    m_to2DMatrix = GetTo2DMatrix();
            //    m_moveToCenterMatrix = ToCenterMatrix();
            //}
        }


        public void getlineasContorno()
        {
            //pasa los elemntos a opening
            Opening op = (Opening)m_data;

            //obtiene curva del opening
            CurveArray listaCUrva = op.BoundaryCurves;

            //analiza llinea por linea y los agrega a 
            foreach (Curve item in listaCUrva)
            {
                if (item.GetType().Name == "Arc") continue;
                IList<XYZ> asda = new List<XYZ>();
                asda = item.Tessellate();

                List <XYZ> list = new List<XYZ>();
                list.Add(new XYZ(item.GetEndPoint(0).X, item.GetEndPoint(0).Y, NivelLosa.ProjectElevation));
                list.Add(new XYZ(item.GetEndPoint(1).X, item.GetEndPoint(1).Y, NivelLosa.ProjectElevation));
                ListaPtoFaceUP.Add(list);                
            }


        }
        /// <summary>
        /// Get points of the first face
        /// </summary>
        /// <param name="faces">edges in all faces</param>
        /// <returns>points of first face</returns>
        public override List<List<XYZ>> GetNeedPoints(List<List<Edge>> faces)
        {
            List<List<XYZ>> needPoints = new List<List<XYZ>>();
            for (int i = 0; i < faces.Count; i++)
            {
                foreach (Edge edge in faces[i])
                {
                    List<XYZ> edgexyzs = edge.Tessellate() as List<XYZ>;
                    if (false == m_haveOpening)
                    {
                        List<XYZ> transformedPoints = new List<XYZ>();
                        for (int j = 0; j < edgexyzs.Count; j++)
                        {
                            Autodesk.Revit.DB.XYZ xyz = edgexyzs[j];
                            Autodesk.Revit.DB.XYZ transformedXYZ = m_beamTransform.OfPoint(xyz);
                            transformedPoints.Add(transformedXYZ);
                        }
                        edgexyzs = transformedPoints;
                    }
                    needPoints.Add(edgexyzs);
                }
            }
            return needPoints;
        }

        public List<Line> CrearSeparacionRoom(Document doc)
        {
            Creator creador = new Creator(doc, _uiapp);
            List<Line> poligoSeparacion = creador.DrawLIne(ListaPtoFaceUP, NivelLosa);
            return poligoSeparacion;
        }

      



        /// <summary>
        /// Get the bound of a face
        /// </summary>
        /// <returns>points array stores the bound of the face</returns>
        public override PointF[] GetFaceBounds()
        {
            Matrix4 matrix = m_to2DMatrix;
            Matrix4 inverseMatrix = matrix.Inverse();
            float minX = 0, maxX = 0, minY = 0, maxY = 0;
            bool bFirstPoint = true;

            //get the max and min point on the face
            for (int i = 0; i < m_points.Count; i++)
            {
                List<XYZ> points = m_points[i];
                foreach (Autodesk.Revit.DB.XYZ point in points)
                {
                    Vector4 v = new Vector4(point);
                    Vector4 v1 = inverseMatrix.Transform(v);

                    if (bFirstPoint)
                    {
                        minX = maxX = v1.X;
                        minY = maxY = v1.Y;
                        bFirstPoint = false;
                    }
                    else
                    {
                        if (v1.X < minX)
                        {
                            minX = v1.X;
                        }
                        else if (v1.X > maxX)
                        {
                            maxX = v1.X;
                        }

                        if (v1.Y < minY)
                        {
                            minY = v1.Y;
                        }
                        else if (v1.Y > maxY)
                        {
                            maxY = v1.Y;
                        }
                    }
                }
            }
            //return an array with max and min value of face
            PointF[] resultPoints = new PointF[2] {
                new PointF(minX, minY), new PointF(maxX, maxY) };
            return resultPoints;
        }

        /// <summary>
        /// Get a matrix which can transform points to 2D
        /// </summary>
        /// <returns>matrix which can transform points to 2D</returns>
        public override Matrix4 GetTo2DMatrix()
        {
            //get transform used to transform points to plane whose normal is Zaxis of beam
            Vector4 xAxis = new Vector4(new XYZ(0, 0, 0));
            xAxis.Normalize();
            //Because Y axis in windows UI is downward, so we should Multiply(-1) here
            Vector4 yAxis = new Vector4(new XYZ(0, 0, 0).Multiply(-1));
            yAxis.Normalize();
            Vector4 zAxis = yAxis.CrossProduct(xAxis);
            zAxis.Normalize();

            Vector4 vOrigin = new Vector4(m_points[0][0]);
            Matrix4 result = new Matrix4(xAxis, yAxis, zAxis, vOrigin);
            m_MatrixZaxis = result;

            //get transform used to transform points to plane whose normal is Yaxis of beam
            xAxis = new Vector4(new XYZ(0, 0, 0));
            xAxis.Normalize();
            //zAxis = new Vector4(m_data.FacingOrientation);
            zAxis = new Vector4(new XYZ(0,0,0));
            zAxis.Normalize();
            yAxis = (xAxis.CrossProduct(zAxis)) * (-1);
            yAxis.Normalize();
            result = new Matrix4(xAxis, yAxis, zAxis, vOrigin);
            m_MatrixYaxis = result;
            return m_MatrixZaxis;
        }

        /// <summary>
        /// Get edges of element's profile
        /// </summary>
        /// <param name="elem">selected element</param>
        /// <returns>all the faces in the selected Element</returns>
        public override List<List<Edge>> GetFaces(Autodesk.Revit.DB.Element elem)
        {
            List<List<Edge>> faceEdges = new List<List<Edge>>();
            List<List<Edge>> faceEdgesNormalUP = new List<List<Edge>>();
            Options options = m_appCreator.NewGeometryOptions();
            options.DetailLevel = ViewDetailLevel.Medium;
            //make sure references to geometric objects are computed.
            options.ComputeReferences = true;
            Autodesk.Revit.DB.GeometryElement geoElem = elem.get_Geometry(options);
            //GeometryObjectArray gObjects = geoElem.Objects;
            IEnumerator<GeometryObject> Objects = geoElem.GetEnumerator();

            //Autodesk.Revit.DB.GeometryElement elemGeo = instance.SymbolGeometry;
            //GeometryObjectArray objectsGeo = elemGeo.Objects;
            //IEnumerator<GeometryObject> Objects1 = elemGeo.GetEnumerator();
            while (Objects.MoveNext())
            {
                GeometryObject geoGeo = Objects.Current;

                if (m_beamTransform == null)
                {
                    Autodesk.Revit.DB.GeometryInstance instance = geoGeo as Autodesk.Revit.DB.GeometryInstance;
                    m_beamTransform = instance.Transform;
                }
                //Autodesk.Revit.DB.GeometryElement elemGeo = 

                bool IsNormalUp = false;
                bool IsNormalDown = false;
                Solid solid = geoGeo as Solid;
                if (solid != null)
                {
                    FaceArray faces = solid.Faces;
                    foreach (Face face in faces)
                    {
                        PlanarFace planarFace = face as PlanarFace;
                        if (planarFace == null) continue;
                        //busca una cara superior con normal  0,0,1
                        if ((planarFace != null) && (Util.IsEqual(planarFace.FaceNormal, XYZ.BasisZ, 0.01)))
                        { IsNormalUp = true; }

                        //busca una cara inferior con normal  0,0,-1
                        if ((planarFace != null) && (Util.IsEqual(planarFace.FaceNormal, -1 * XYZ.BasisZ, 0.01)))
                        { IsNormalDown = true; }

                        EdgeArrayArray edgeArrarr = face.EdgeLoops;
                        foreach (EdgeArray edgeArr in edgeArrarr)
                        {
                            List<Edge> edgesList = new List<Edge>();
                            foreach (Edge edge in edgeArr)
                            {
                                edgesList.Add(edge);
                            }
                            faceEdges.Add(edgesList);

                            // IsNormalUp= true --> face superior
                            //lo guarda en  ListaPtoFaceUP
                            if (IsNormalUp)
                            {
                                IsNormalUp = false;
                                //get points array in edges 
                                foreach (Edge edge in edgesList)
                                {

                                    List<XYZ> edgexyzs = edge.Tessellate() as List<XYZ>;
                                    ListaPtoFaceUP.Add(edgexyzs);
                                }
                            }
                            // IsNormalUp= true --> face inferior
                            //lo guarda en  ListaPtoFaceDown
                            else if (IsNormalDown)
                            {
                                IsNormalDown = false;
                                //get points array in edges 
                                foreach (Edge edge in edgesList)
                                {
                                    List<XYZ> edgexyzs = edge.Tessellate() as List<XYZ>;
                                    ListaPtoFaceDown.Add(edgexyzs);
                                }
                            }
                        }
                    }
                }
            }


            //    GeometryObject geo0 = Objects.Current;
            //Autodesk.Revit.DB.GeometryInstance instance = geo0 as Autodesk.Revit.DB.GeometryInstance;
            //m_beamTransform = instance.Transform;

            //get all the edges in the Geometry object
            //foreach (GeometryObject geo in gObjects)

            return faceEdges;
        }


        /// <summary>
        /// Create Opening on beam
        /// </summary>
        /// <param name="points">points used to create Opening</param>
        /// <returns>newly created Opening</returns>
        public override Opening CreateOpening(List<Vector4> points)
        {
            Autodesk.Revit.DB.XYZ p1, p2; Line curve;
            CurveArray curves = m_appCreator.NewCurveArray();
            for (int i = 0; i < points.Count - 1; i++)
            {
                p1 = new Autodesk.Revit.DB.XYZ(points[i].X, points[i].Y, points[i].Z);
                p2 = new Autodesk.Revit.DB.XYZ(points[i + 1].X, points[i + 1].Y, points[i + 1].Z);
                curve = Line.CreateBound(p1, p2);
                curves.Append(curve);
            }

            //close the curve
            p1 = new Autodesk.Revit.DB.XYZ(points[0].X, points[0].Y, points[0].Z);
            p2 = new Autodesk.Revit.DB.XYZ(points[points.Count - 1].X, points[points.Count - 1].Y, points[points.Count - 1].Z);
            curve = Line.CreateBound(p1, p2);
            curves.Append(curve);

            if (false == m_isZaxis)
            {
                return m_docCreator.NewOpening(m_data, curves, Autodesk.Revit.Creation.eRefFace.CenterY);
            }
            else
            {
                return m_docCreator.NewOpening(m_data, curves, Autodesk.Revit.Creation.eRefFace.CenterZ);
            }

        }

        /// <summary>
        /// Change transform matrix used to transform points to 2d.
        /// </summary>
        /// <param name="isZaxis">transform points to which plane.
        /// true means transform points to plane whose normal is Zaxis of beam.
        /// false means transform points to plane whose normal is Yaxis of beam
        /// </param>
        public void ChangeTransformMatrix(bool isZaxis)
        {
            m_isZaxis = isZaxis;
            if (isZaxis)
            {
                m_to2DMatrix = m_MatrixZaxis;
            }
            else
            {
                m_to2DMatrix = m_MatrixYaxis;
            }
            //re-calculate matrix used to move points to center
            m_moveToCenterMatrix = ToCenterMatrix();
        }
    }
}