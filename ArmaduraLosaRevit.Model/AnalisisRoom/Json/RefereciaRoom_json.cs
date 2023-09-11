using System;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.IO;
using ArmaduraLosaRevit.Model.Elemento_Losa;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Json
{
    public class RefereciaRoom_json
    {
        #region 0)propiedades


        public string nombreLosa { get; set; }
        public float Pp { get; set; }
        public float Sc { get; set; }
        public float PpEx { get; set; }
        public float espesor { get; set; }
        public XYZ posicionPelota { get; set; }

        public float LargoHorizontal { get; set; }
        public float LargoVertical { get; set; }

        public string direccionHorizontal { get; set; }
        public string direccionVertical { get; set; }

        public string cuantiaHorizontal { get; set; }
        public string cuantiaVertical { get; set; }
        public string HandlePoligonoLosa { get; set; }
        public string HandlePelotaLosa { get; set; }
        public double anguloPelotaLosa { get; set; }
        public int tipoEmpotramiento { get; set; }

        public TipoEstado estado { get; set; }

        public List<string> ListaPtoLineaDireccionLosa { get; set; }
        public List<string> ListaPtoLineaDireccionPerpLosa { get; set; }

        public List<string> ListaSuplesVerticalLosa { get; set; }
        public List<string> ListaSuplesHorizontalLosa { get; set; }
        public List<string> Lista_Suples_Todos { get; set; }

        public List<string> ListaPtos_Vertical_Barra { get; set; }
        public List<string> ListaPtos_Horizontal_Barra { get; set; }

        public List<string> ListaVerticesPoligonoLosa { get; set; }




        #endregion

        #region 1)Contructor


        public RefereciaRoom_json()
        {
      

            ListaPtoLineaDireccionLosa = new List<string>();
            ListaPtoLineaDireccionPerpLosa = new List<string>();

            ListaSuplesVerticalLosa = new List<string>();
            ListaSuplesHorizontalLosa = new List<string>();

            Lista_Suples_Todos = new List<string>();

            ListaPtos_Vertical_Barra = new List<string>();
            ListaPtos_Horizontal_Barra = new List<string>();


            ListaVerticesPoligonoLosa = new List<string>();
        }

  
        public static RefereciaRoom_json ctro_NH_RefereciaLosa_json_enCM(ReferenciaRoom obj)
        {

            RefereciaRoom_json newRefe = new RefereciaRoom_json();

            ReferenciaRoomDatos refereciaRoomDatos_aux = obj.RefereciaRoomDatos;
            newRefe.LargoHorizontal = (float)Util.FootToCm(refereciaRoomDatos_aux.LargoHorizontal);
            newRefe.LargoVertical = (float)Util.FootToCm(refereciaRoomDatos_aux.LargoVertical);
            newRefe.nombreLosa = (refereciaRoomDatos_aux.nombreLosa_1!=null? refereciaRoomDatos_aux.nombreLosa_1:"");
            newRefe.espesor = refereciaRoomDatos_aux.espesorCM_1;
            newRefe.posicionPelota = new XYZ((float)Util.FootToCm(refereciaRoomDatos_aux.posicionPelota.X), (float)Util.FootToCm(refereciaRoomDatos_aux.posicionPelota.Y), (float)Util.FootToCm(refereciaRoomDatos_aux.posicionPelota.Z));
            newRefe.direccionHorizontal = refereciaRoomDatos_aux.direccionHorizontal;
            newRefe.direccionVertical = refereciaRoomDatos_aux.direccionVertical;
            newRefe.cuantiaHorizontal = refereciaRoomDatos_aux.cuantiaHorizontal;//@8a15
            newRefe.cuantiaVertical = refereciaRoomDatos_aux.cuantiaVertical;//@8a15
            newRefe.HandlePoligonoLosa = refereciaRoomDatos_aux.Room1.Id.ToString();
            //var asdasd = obj.room.GetGeometricObjects2();

            //var asdas = obj.room as FamilyInstance;
            //Element e = doc.GetElement(obj.PelotaLosa.Id);

            newRefe.HandlePelotaLosa = refereciaRoomDatos_aux.PelotaLosa.Id.ToString();
            newRefe.anguloPelotaLosa = refereciaRoomDatos_aux.anguloPelotaLosaGrado_1;
            newRefe.tipoEmpotramiento = refereciaRoomDatos_aux.tipoEmpotramiento;
            newRefe.estado = TipoEstado.SinCalcular;

            newRefe.Sc = 0.5f;
            newRefe.PpEx = 0.05f;
            newRefe.Pp = (float)(newRefe.espesor * 2.5 / 100);

            foreach (var item in obj.ListaPtoLineaDireccionLosa)
            {
                newRefe.ListaPtoLineaDireccionLosa.Add(Util.FootToCm(item.X).ToString() + "," + Util.FootToCm(item.Y).ToString());
            }
            foreach (var item in obj.ListaPtoLineaDireccionPerpLosa)
            {
                newRefe.ListaPtoLineaDireccionPerpLosa.Add(Util.FootToCm(item.X).ToString() + "," + Util.FootToCm(item.Y).ToString());
            }
            foreach (var item in obj.ListaVerticesPoligonoLosa)
            {
                newRefe.ListaVerticesPoligonoLosa.Add(Util.FootToCm(item.X).ToString() + "," + Util.FootToCm(item.Y).ToString());
            }
            //*****************************************************************************
            foreach (var item in obj.ListaSuplesVerticalLosa)
            {
                newRefe.ListaSuplesVerticalLosa.Add(newRefe.ObtenerStringSupleEnCm(item));
            }

            foreach (var item in obj.ListaSuplesHorizontalLosa)
            {
                newRefe.ListaSuplesHorizontalLosa.Add(newRefe.ObtenerStringSupleEnCm(item));
            }

            //foreach (var item in obj.ListaPtos_Suple_superior)
            //{
            //    newRefe.Lista_Suples_Todos.Add(newRefe.ObtenerStringSupleEnCm(item));
            //}

            foreach (var item in obj.ListaPtos_Vertical_Barra)
            {
                newRefe.ListaPtos_Vertical_Barra.Add(newRefe.ObtenerStringBarraEnCm(item));
            }

            foreach (var item in obj.ListaPtos_Horizontal_Barra)
            {
                newRefe.ListaPtos_Horizontal_Barra.Add(newRefe.ObtenerStringBarraEnCm(item));
            }

            return newRefe;
        }

        #endregion

        #region 2)MEtodo

        private string ObtenerStringBarra(NH_RefereciaCrearBarra item)
        {
            return item.nombreLosa.ToString() + "," + item.espesor.ToString() + "," + item.anguloPelotaLosa.ToString() + "," +
                                                      item.PosicionPelotaLosa.X.ToString() + ":" + item.PosicionPelotaLosa.Y.ToString() + "," +
                                                      item.PosicionPto_Barra.X.ToString() + ":" + item.PosicionPto_Barra.Y.ToString() + "," +
                                                      item.UbicacionEnLosa + "," + item.PoligonoLosa.Id.ToString() + "," + item.PelotaLosa.Id.ToString() + "," + item.LargoRecorridoX.ToString() + "," + item.LargoRecorridoY.ToString();
            // return "";
        }

        private string ObtenerStringBarraEnCm(NH_RefereciaCrearBarra item)
        {
            return item.nombreLosa.ToString() + "," + item.espesor.ToString() + "," + item.anguloPelotaLosa.ToString() + "," +
                                                      Util.FootToCm(item.PosicionPelotaLosa.X).ToString() + ":" + Util.FootToCm(item.PosicionPelotaLosa.Y).ToString() + ":" + Util.FootToCm(item.PosicionPelotaLosa.Z).ToString() + "," +
                                                      Util.FootToCm(item.PosicionPto_Barra.X).ToString() + ":" + Util.FootToCm(item.PosicionPto_Barra.Y).ToString() + ":" + Util.FootToCm(item.PosicionPto_Barra.Z).ToString() + "," +
                                                      item.UbicacionEnLosa + "," + item.PoligonoLosa.Id.ToString() + "," + item.PelotaLosa.Id.ToString() + "," + Util.FootToCm(item.LargoRecorridoX).ToString() + "," + Util.FootToCm(item.LargoRecorridoY).ToString();
            // return "";
        }
        private string ObtenerStringSuple(NH_RefereciaCrearSuple item)
        {
            return item.nombreLosa1.ToString() + "," + item.nombreLosa2.ToString() + "," +
                                                        item.PosicionPtoSupleInicial.X.ToString() + ":" + item.PosicionPtoSupleInicial.Y.ToString() + "," +
                                                        item.PosicionPtoSupleFinal.X.ToString() + ":" + item.PosicionPtoSupleFinal.Y.ToString() + "," +
                                                        item.UbicacionEnLosa.ToString() + "," + item.diametro + "," + item.espaciamiento + "," + item.cuantia;
        }

        private string ObtenerStringSupleEnCm(NH_RefereciaCrearSuple item)
        {
            return item.nombreLosa1.ToString() + "," + item.nombreLosa2.ToString() + "," +
                                                        Util.FootToCm(item.PosicionPtoSupleInicial.X).ToString() + ":" + Util.FootToCm(item.PosicionPtoSupleInicial.Y).ToString() + ":" + Util.FootToCm(item.PosicionPtoSupleInicial.Z).ToString() + "," +
                                                        Util.FootToCm(item.PosicionPtoSupleFinal.X).ToString() + ":" + Util.FootToCm(item.PosicionPtoSupleFinal.Y).ToString() + ":" + Util.FootToCm(item.PosicionPtoSupleFinal.Z).ToString() + "," +
                                                        item.UbicacionEnLosa.ToString() + "," + item.diametro + "," + item.espaciamiento + "," + item.cuantia;
        }




        #endregion

    }


}
