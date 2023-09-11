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
using ArmaduraLosaRevit.Model.AnalisisRoom.Json;

namespace ArmaduraLosaRevit.Model.AnalisisRoom
{
    [System.Runtime.InteropServices.Guid("B58086CE-9EE4-4FFF-A451-9439A24607A1")]
    public class ReferenciaRoomHandler
    {
        #region 0)Propiedades

        
        public RefereciaRoom_jsonExportar refereciaRoom_jsonExportar { get; set; }
        // public List<RefereciaRoom> Lista_RefereciaRoom { get; set; }
        //public List<NH_RefereciaRoom> ListaPOlilineaYEsferaLosa { get; set; }

        //lista  que al importar las datos de json  almacenan las referencias de suples

        public List<NH_RefereciaCrearSuple> ListasSuples_Todos { get; set; }
        public List<NH_RefereciaCrearSuple> ListasSuples_Vertical { get; set; }
        public List<NH_RefereciaCrearSuple> ListasSuples_Horizontal { get; set; }

        //public List<NH_RefereciaSuple> ListasSuples_Vertical { get; set; }  // barra en sentido del pelota de losa
       // public List<NH_RefereciaSuple> ListasSuples_Horizontal { get; set; }



        //lista  que al importar las datos de json  almacenan las referencias de barra verticales
        //public List<NH_RefereciaCrearBarra> ListaPtos_Vertical_Barra { get; set; }
        //lista  que al importar las datos de json  almacenan las referencias de barra horizontal
        public List<NH_RefereciaCrearBarra> ListaPtos_Horizontal_Barra { get; set; }

        public UIDocument uidoc { get; set; }

        private UIApplication _uiapp;

        public bool IsImprimirAyuda { get; set; } = false;


        private ReferenciaRoomListas referenciaRoomListas { get; set; }
        public ReferenciaRoomListas ReferenciaRoomListas
        {
            get
            {
               return referenciaRoomListas;
            }

            set
            {
                referenciaRoomListas = value;
            }
        }
        #endregion

        #region 1)Constructores


        public ReferenciaRoomHandler(UIApplication _uiapp)
        {
            this.uidoc = _uiapp.ActiveUIDocument;
            this._uiapp = _uiapp;
            //Lista_RefereciaRoom = new List<RefereciaRoom>();



            //ListaPtos_Vertical_Barra = new List<NH_RefereciaCrearBarra>();
            ListaPtos_Horizontal_Barra = new List<NH_RefereciaCrearBarra>();

            ListasSuples_Vertical = new List<NH_RefereciaCrearSuple>();
            ListasSuples_Horizontal = new List<NH_RefereciaCrearSuple>();

            ListasSuples_Todos = new List<NH_RefereciaCrearSuple>();

            referenciaRoomListas = new ReferenciaRoomListas(_uiapp);
            refereciaRoom_jsonExportar = new RefereciaRoom_jsonExportar(uidoc, this);  
        }


        #endregion

        //#region 2)Metodos
        #region 2)metodos


        /// <summary>
        /// calcula todos los datos de la losa, datos losa, largos minimos, suples y tipo de losa
        /// rutina que a) obtiene los room a analizar
        ///             b) btiene las barras inferiores
        ///             c)obtiene los suples
        /// </summary>
        /// <param name="largominimo"></param>
        /// <param name="porcentajeTramosBarraInferior"></param>
        /// <param name="porcentajeTramosBarraSuple"></param>
        /// <param name="dibujar_pto_horizontal"></param>
        /// <param name="dibujar_pto_vertical"></param>
        /// <param name="uidoc"></param>
        public void GetLista_RefereciaRoom( float porcentajeTramosBarraInferior, float porcentajeTramosBarraSuple, UIDocument uidoc)
        {


            //a)crea listas con losas y sis datos (seleccionados) y genera objetos  'List<NH_RefereciaLosa>'
            referenciaRoomListas.IsImprimirayuda = IsImprimirAyuda;
            //obtiene los refereneroom
            referenciaRoomListas.GetLista_RefereciaRoom("SelectConMouse");


            if (referenciaRoomListas.Lista_RefereciaRoom.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Por algun motivo no se puedo Agregar referecias de losa  a seleccion: \n Posibles errore \n -" +
                                                     "Error layer Referencia losa y poligono de losas \n - No agrupados o mas de 1 grupo por referencia de losa ");
                return;
            }
        }


        /// <summary>
        ///  crea el obj:'NH_RefereciaLosaParaBarra'  y lo agrega a lista ' NH_RefereciaLosa_.ListaPtos_Vertical_Barra'
        /// 
        /// nota: utliza los obj 'NH_RefereciaLosaParaSuple' de la lista 'AnalisisPelotaLosa.ListaPtos_Vertical_Suples'
        /// </summary>
        public void CrearBarrasInferiores()
        {

            //BARRA VERTICAL

        
            foreach (ReferenciaRoom NH_RefereciaLosa_ in referenciaRoomListas.Lista_RefereciaRoom)
            {
                //BARRA VERTICAL
                NH_RefereciaLosa_.ListaPtos_Vertical_Barra.Clear();
                //NH_RefereciaLosa_.ListaPtos_Vertical_Barra.AddRange(NH_RefereciaLosa_.ListaPtos_Barra_inferior_H);
                //se vuvlen a creas los obj: 'NH_RefereciaLosaParaBarra' por la lista 'ListaPtos_Barra_inferior_H' tiene obj:'NH_RefereciaLosaParaBarra'
                //pero solo con los datos de 
                //PosicionPto_Barra
                //LargoRecorridoX 
                //LargoRecorridoY 
                #region borrar porredundate
                if (true)
                {
                    foreach (var pt_barra in NH_RefereciaLosa_.ListaPtos_Barra_inferior_H)
                    {

                        #region borrar por hacer lo msimo
                        XYZ pt_Suple = pt_barra.PosicionPto_Barra;
                        //angulo entre pto de dibijo de barra y pelota de losa
                        var valorAngle = Util.angulo_entre_pt_Rad_XY0(NH_RefereciaLosa_.RefereciaRoomDatos.posicionPelota, pt_barra.PosicionPto_Barra);
                        var sin_Aux = Math.Sin(valorAngle);

                        //BARRA INFERIOR VERTICAL
                        NH_RefereciaCrearBarra aux_ListaPtos_Vertical_Barra = null;
                        if (sin_Aux > 0)
                        { aux_ListaPtos_Vertical_Barra = new NH_RefereciaCrearBarra(NH_RefereciaLosa_.RefereciaRoomDatos.nombreLosa_1, NH_RefereciaLosa_.RefereciaRoomDatos.espesorCM_1, NH_RefereciaLosa_.RefereciaRoomDatos.anguloPelotaLosaGrado_1, NH_RefereciaLosa_.RefereciaRoomDatos.posicionPelota, pt_barra.PosicionPto_Barra, UbicacionLosa.Superior, NH_RefereciaLosa_.RefereciaRoomDatos.Room1, NH_RefereciaLosa_.RefereciaRoomDatos.Room1) { LargoRecorridoX = pt_barra.LargoRecorridoX, LargoRecorridoY = pt_barra.LargoRecorridoY }; }
                        else
                        { aux_ListaPtos_Vertical_Barra = new NH_RefereciaCrearBarra(NH_RefereciaLosa_.RefereciaRoomDatos.nombreLosa_1, NH_RefereciaLosa_.RefereciaRoomDatos.espesorCM_1, NH_RefereciaLosa_.RefereciaRoomDatos.anguloPelotaLosaGrado_1, NH_RefereciaLosa_.RefereciaRoomDatos.posicionPelota, pt_barra.PosicionPto_Barra, UbicacionLosa.Inferior, NH_RefereciaLosa_.RefereciaRoomDatos.Room1, NH_RefereciaLosa_.RefereciaRoomDatos.Room1) { LargoRecorridoX = pt_barra.LargoRecorridoX, LargoRecorridoY = pt_barra.LargoRecorridoY }; }

                        //ListaPtos_Vertical_Barra.Add(aux_ListaPtos_Vertical_Barra);
                        NH_RefereciaLosa_.ListaPtos_Vertical_Barra.Add(aux_ListaPtos_Vertical_Barra);

                        #endregion
                       
                    }  
                }
                #endregion


                //BARRA HORIZONTAL                          ListasLineBordesRoomOffset = lista lineas offset
                NH_RefereciaLosa_.ListaPtos_Horizontal_Barra.Clear();
                //NH_RefereciaLosa_.ListaPtos_Horizontal_Barra.AddRange(NH_RefereciaLosa_.ListaPtos_Barra_inferior_V);
                //se vuvlen a creas los obj: 'NH_RefereciaLosaParaBarra' por la lista 'ListaPtos_Barra_inferior_H' tiene obj:'NH_RefereciaLosaParaBarra'
                //pero solo con los datos de 
                //PosicionPto_Barra
                //LargoRecorridoX 
                //LargoRecorridoY 
                #region borrarPOrredundate
                if (true)
                {
                    foreach (var pt_barra in NH_RefereciaLosa_.ListaPtos_Barra_inferior_V)
                    {

                        XYZ pt_Suple = pt_barra.PosicionPto_Barra; ;
                        //angulo entre pto de dibijo de barra y pelota de losa
                        var valorAngle = Util.angulo_entre_pt_Rad_XY0(NH_RefereciaLosa_.RefereciaRoomDatos.posicionPelota, pt_barra.PosicionPto_Barra);
                        var sin_Aux = Math.Sin(valorAngle);

                        //BARRA INFERIOR VERTICAL
                        NH_RefereciaCrearBarra aux_ListaPtos_Horizontal_Barra = null;
                        if (sin_Aux > 0)
                        { aux_ListaPtos_Horizontal_Barra = new NH_RefereciaCrearBarra(NH_RefereciaLosa_.RefereciaRoomDatos.nombreLosa_1, NH_RefereciaLosa_.RefereciaRoomDatos.espesorCM_1, NH_RefereciaLosa_.RefereciaRoomDatos.anguloPelotaLosaGrado_1, NH_RefereciaLosa_.RefereciaRoomDatos.posicionPelota, pt_barra.PosicionPto_Barra, UbicacionLosa.Derecha, NH_RefereciaLosa_.RefereciaRoomDatos.Room1, NH_RefereciaLosa_.RefereciaRoomDatos.Room1) { LargoRecorridoX = pt_barra.LargoRecorridoX, LargoRecorridoY = pt_barra.LargoRecorridoY }; }
                        else
                        { aux_ListaPtos_Horizontal_Barra = new NH_RefereciaCrearBarra(NH_RefereciaLosa_.RefereciaRoomDatos.nombreLosa_1, NH_RefereciaLosa_.RefereciaRoomDatos.espesorCM_1, NH_RefereciaLosa_.RefereciaRoomDatos.anguloPelotaLosaGrado_1, NH_RefereciaLosa_.RefereciaRoomDatos.posicionPelota, pt_barra.PosicionPto_Barra, UbicacionLosa.Izquierda, NH_RefereciaLosa_.RefereciaRoomDatos.Room1, NH_RefereciaLosa_.RefereciaRoomDatos.Room1) { LargoRecorridoX = pt_barra.LargoRecorridoX, LargoRecorridoY = pt_barra.LargoRecorridoY }; }

                        //ListaPtos_Horizontal_Barra.Add(aux_ListaPtos_Horizontal_Barra);
                        NH_RefereciaLosa_.ListaPtos_Horizontal_Barra.Add(aux_ListaPtos_Horizontal_Barra);


                    }  
                }
                #endregion



            }
        }

     
        //porcentajeUbicacion = 0.5
        /// <summary>
        /// crea el obj:'PtosCrearSuples'  y lo agrega a lista 'ListasSuples_Vertical'
        /// 
        /// nota: utliza los obj 'NH_RefereciaLosaParaSuple' de la lista 'nH_RefereciaLosa.ListaPtos_Suple_superior'
        /// </summary>
        /// <param name="largominimo"></param>
        /// <param name="porcentajeUbicacion"></param>
        /// <param name="dibujar_pto_horizontal"></param>
        /// <param name="dibujar_pto_vertical"></param>
        public void CrearSuples()
        {

            foreach (ReferenciaRoom nH_RefereciaLosa in referenciaRoomListas.Lista_RefereciaRoom)
            {           
                foreach (NH_RefereciaCrearSuple refLosaSuple in nH_RefereciaLosa.ListaSuplesHorizontalLosa)
                {
                   //  PtosCrearSuples nuevoSuple = new PtosCrearSuples(refLosaSuple.PosicionPtoSupleInicial, refLosaSuple.PosicionPtoSupleFinal,"","", refLosaSuple.diametro, refLosaSuple.espaciamiento);
                    ListasSuples_Horizontal.Add(refLosaSuple);
                }
                foreach (NH_RefereciaCrearSuple refLosaSuple in nH_RefereciaLosa.ListaSuplesVerticalLosa)
                {
                    // PtosCrearSuples nuevoSuple = new PtosCrearSuples(refLosaSuple.PosicionPtoSupleInicial, refLosaSuple.PosicionPtoSupleFinal,"","", refLosaSuple.diametro, refLosaSuple.espaciamiento);
                    ListasSuples_Vertical.Add(refLosaSuple);
                }
            }
        }



        #region Metodo para pasar a json


        public bool LeerArchivoJson()
        {

           return refereciaRoom_jsonExportar.LeerArchivoJson();

        }

        #endregion

        #endregion
        //#endregion

    }



}
