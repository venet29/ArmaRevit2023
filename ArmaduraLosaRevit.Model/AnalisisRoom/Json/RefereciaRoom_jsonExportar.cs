using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using Newtonsoft.Json;

using System.Windows.Forms;
using Autodesk.Revit.DB;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Elemento_Losa;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Json
{
    [System.Runtime.InteropServices.Guid("B58086CE-9EE4-4FFF-A451-9439A24607A1")]

    //tarea1 : leer desde un archivo json las barras para importar y generar la lista List<RefereciaRoom> ListaLosas para dibujar
    //refuerzo inferio y superior
    public class RefereciaRoom_jsonExportar
    {
        #region 0)Propiedades




        public UIDocument uidoc { get; set; }
        public ReferenciaRoomHandler referenciaRoomHandler { get; set; }
        #endregion

        #region 1)Constructores


        public RefereciaRoom_jsonExportar(UIDocument uidoc, ReferenciaRoomHandler referenciaRoomHandler)
        {
            this.uidoc = uidoc;
            this.referenciaRoomHandler = referenciaRoomHandler;

        }


        #endregion
        #region 2) metodos

        #region Metodo para pasar a json


        public bool LeerArchivoJson()
        {
            //  string ruta = @"..\..\1-1-noparameters_deIngeneria.json";
            string ruta = "";  // @"J:\vlous\union.net\PLANTA\XXX) barra -XX-XX-XXXX -MODIFCANDO\barra\barra\1-noparameters_deIngeneria.json";
            string destino = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "json files (*.json) |*.json";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ruta = openFileDialog1.FileName;
                destino = Path.GetFileName(ruta);
            }
            else
                return false;


            ///obtiene todos los RefereciaRoom
            referenciaRoomHandler.ReferenciaRoomListas.GetLista_RefereciaRoom("GetSelectionAllNivelActual");

            List<RefereciaRoom_json> items;
            if (ruta == "") return false;
            using (StreamReader r = new StreamReader(ruta))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<RefereciaRoom_json>>(json);
            }

            List<ReferenciaRoom> ListaLosas = new List<ReferenciaRoom>();


            try
            {


                foreach (var item in items)
                {

                    // Busca y ANALISIS room con el dato POLIGONO LOSA
                    var poligonoLosa = referenciaRoomHandler.ReferenciaRoomListas.Lista_RefereciaRoom.
                                                Where(c => c.RefereciaRoomDatos.Room1.Id.ToString() == item.HandlePoligonoLosa).Select(m => m.RefereciaRoomDatos.Room1).FirstOrDefault();
                    if (poligonoLosa == null) continue;

                    if (poligonoLosa.IsValidObject == false && item.posicionPelota != null)
                    {
                        poligonoLosa = referenciaRoomHandler.ReferenciaRoomListas.Lista_RefereciaRoom.
                                                   Where(c => Math.Abs(c.RefereciaRoomDatos.posicionPelota.X - item.posicionPelota.X) < 5 && Math.Abs(c.RefereciaRoomDatos.posicionPelota.Y - item.posicionPelota.Y) < 5).
                                                   Select(m => m.RefereciaRoomDatos.Room1).FirstOrDefault();
                    }
                    else if (poligonoLosa.IsValidObject == false && item.posicionPelota == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Losa " + item.nombreLosa + " sin posicion de punto ubicacion", "Mensaje");
                    }

                    CopiarNuevosParametrosARoom(ref poligonoLosa, item);


                    // Busca las floor con dato PELOTA de losa
                    var FloorLosa = referenciaRoomHandler.ReferenciaRoomListas.Lista_RefereciaRoom.
                                               Where(c => c.RefereciaRoomDatos.PelotaLosa.Id.ToString() == item.HandlePelotaLosa).Select(m => m.RefereciaRoomDatos.PelotaLosa).FirstOrDefault();
                    ListaLosas.Add(new ReferenciaRoom(item, poligonoLosa, FloorLosa));
                }




                referenciaRoomHandler.ReferenciaRoomListas.Lista_RefereciaRoom.Clear();
                referenciaRoomHandler.ReferenciaRoomListas.Lista_RefereciaRoom.AddRange(ListaLosas);


                referenciaRoomHandler.ReferenciaRoomListas.ListaPtos_Horizontal_Barra.AddRange(ListaLosas.SelectMany(c => c.ListaPtos_Horizontal_Barra).ToList());
                referenciaRoomHandler.ReferenciaRoomListas.ListaPtos_Vertical_Barra.AddRange(ListaLosas.SelectMany(c => c.ListaPtos_Vertical_Barra).ToList());
                //ListasSuples_Todos.AddRange(ListaLosas.SelectMany(c => c.ListaPtos_Suple_superior).ToList());

                referenciaRoomHandler.ListasSuples_Horizontal.AddRange(ListaLosas.SelectMany(c => c.ListaSuplesHorizontalLosa).ToList());
                referenciaRoomHandler.ListasSuples_Vertical.AddRange(ListaLosas.SelectMany(c => c.ListaSuplesVerticalLosa).ToList());

                referenciaRoomHandler.ListasSuples_Todos.AddRange(referenciaRoomHandler.ListasSuples_Horizontal);
                referenciaRoomHandler.ListasSuples_Todos.AddRange(referenciaRoomHandler.ListasSuples_Vertical);

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"errror al exportar {ex.Message}");
                return false;

            }
            return true; ;
        }
        /// <summary>
        /// cambia los datos internos del room
        /// NOmbre
        /// espesor
        /// Angulo
        /// direcion
        /// cuantia
        /// 
        /// </summary>
        /// <param name="idObj_"> room al que se cambia los datos</param>
        /// <param name="item">datos de losa en formatos json para cambiar datros</param>
        /// <returns></returns>
        public void CopiarNuevosParametrosARoom(ref Room idObj_, RefereciaRoom_json item)
        {

            try
            {
                using (Transaction t = new Transaction(uidoc.Document))
                {
                    t.Start("Agregando los parametros internos-NH");
                    var nm = ParameterUtil.SetParaInt(idObj_, "Numero Losa", item.nombreLosa);
                    var espesor = ParameterUtil.SetParaInt(idObj_, "Espesor", item.espesor);
                    var angle = ParameterUtil.SetParaInt(idObj_, "Angulo", Util.GradosToRadianes(item.anguloPelotaLosa));
                    var CH = ParameterUtil.SetParaInt(idObj_, "Cuantia Horizontal", item.cuantiaHorizontal);
                    var CV = ParameterUtil.SetParaInt(idObj_, "Cuantia Vertical", item.cuantiaVertical);
                    var DH = ParameterUtil.SetParaInt(idObj_, "Direccion Horizontal", item.direccionHorizontal);
                    var DV = ParameterUtil.SetParaInt(idObj_, "Direccion Vertical", item.direccionVertical);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }
        #endregion

        #endregion

    }



}
