using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.UTILES.DTO;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Macros;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CrearTexNote
    {
        // ExternalCommandData commandData;
        UIDocument uidoc;
        Document _doc;
        TextNoteType txtNoteType;
        TextNote txNote;
        private UIApplication _uipp;
        private string nameTipoTexto;
        private TipoCOloresTexto tipoCOloresTexto;
        private int color;

        public CrearTexNote(UIApplication uipp, string nameTipoTexto, TipoCOloresTexto _TipoCOloresTexto)
        {
            uidoc = uipp.ActiveUIDocument;
            _doc = uipp.ActiveUIDocument.Document;

            //obtienen el tipo texto
            txtNoteType = TiposTextNote.ObtenerTextNote(nameTipoTexto, _doc);
            this._uipp = uipp;
            this.nameTipoTexto = nameTipoTexto;
            tipoCOloresTexto = _TipoCOloresTexto;

        }
        public CrearTexNote(Document doc, string nameTipoTexto, TipoCOloresTexto _TipoCOloresTexto)
        {
            //uidoc = uipp.ActiveUIDocument;
            _doc = doc;

            //obtienen el tipo texto
            txtNoteType = TiposTextNote.ObtenerTextNote(nameTipoTexto, _doc);
            // this._uipp = uipp;
            this.nameTipoTexto = nameTipoTexto;
            tipoCOloresTexto = _TipoCOloresTexto;

        }
        public CrearTexNote(UIApplication uipp, string nameTipoTexto)
        {
            uidoc = uipp.ActiveUIDocument;
            _doc = uipp.ActiveUIDocument.Document;

            //obtienen el tipo texto
            txtNoteType = TiposTextNote.ObtenerTextNote(nameTipoTexto, _doc);
            this.nameTipoTexto = nameTipoTexto;
            this._uipp = uipp;
        }



        /// <summary>
        /// obtiene el tipo de texto segun el nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        /// 


        //crea texto 
        public TextNote M1_CrearConTrans(XYZ pto, string Texto, double angle,
            VerticalTextAlignment _VerticalTextAlignment = VerticalTextAlignment.Middle,
            HorizontalTextAlignment _HorizontalTextAlignment = HorizontalTextAlignment.Center)
        {

            ObtenerColor();
            VeriricarNoteTypeIsok();
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("text-NH");

                    TextNoteOptions tno = new TextNoteOptions();

                    tno.Rotation = angle; // in Radians
                    tno.TypeId = txtNoteType.Id; // tipo de texto
                    tno.VerticalAlignment = _VerticalTextAlignment; // in Radians
                    tno.HorizontalAlignment = _HorizontalTextAlignment; // in Radians


                    txNote = TextNote.Create(_doc, _doc.ActiveView.Id, pto, Texto, tno);
                    if (color != -1) M3_cambiarColorsinTrasn(color);
                    t.Commit();

                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }



            return txNote;
        }







        public TextNote M1_CrearCSintrans(XYZ pto, string Texto, double angle,
                                        VerticalTextAlignment _VerticalTextAlignment = VerticalTextAlignment.Middle,
                                        HorizontalTextAlignment _HorizontalTextAlignment = HorizontalTextAlignment.Center)
        {

            ObtenerColor();

            VeriricarNoteTypeIsok();
            try
            {


                TextNoteOptions tno = new TextNoteOptions();

                tno.Rotation = angle; // in Radians
                tno.TypeId = txtNoteType.Id; // tipo de texto
                tno.VerticalAlignment = _VerticalTextAlignment; // in Radians
                tno.HorizontalAlignment = _HorizontalTextAlignment; // in Radians


                txNote = TextNote.Create(_doc, _doc.ActiveView.Id, pto, Texto, tno);
                if (color != -1) M3_cambiarColorsinTrasn(color);

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }



            return txNote;
        }


        private bool ObtenerColor()
        {
            try
            {
                switch (tipoCOloresTexto)
                {
                    case TipoCOloresTexto.magenta:
                        color = Util.ToColorParameterValue(255, 0, 250);
                        return true;
                    case TipoCOloresTexto.azul:
                        return true;
                    case TipoCOloresTexto.verde:
                        color = Util.ToColorParameterValue(0, 255, 0);
                        return true;
                    case TipoCOloresTexto.rojo:
                        color = Util.ToColorParameterValue(255, 0, 0);
                        return true;
                    case TipoCOloresTexto.Blanco:
                        color = Util.ToColorParameterValue(255, 255, 255);
                        return true;
                    case TipoCOloresTexto.ParaMalla:
                        break;
                }

            }
            catch (Exception)
            {
            }
            color = -1;
            return true;
        }


        private bool VeriricarNoteTypeIsok()
        {
            try
            {
                if (txtNoteType == null)
                {
                    if (M2_CrearTipoText_ConTrasn())
                    {
                        txtNoteType = TiposTextNote.ObtenerTextNote(nameTipoTexto, _doc);
                    }
                    else
                    {
                        Util.ErrorMsg($"Error al crear tipoTexnote '{nameTipoTexto}'. Se utiliza ValorPordefecto");
                        txtNoteType =TiposTextNote.ObtenerPrimeroEncontrado(_doc);

                        if (txtNoteType == null)
                        {
                            Util.ErrorMsg($"No se encontro ningun tipo de TEXTNOTE en proyecto");
                            return false;
                        }
                    
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en  'VeriricarNoteTypeIsok'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M2_cambiarColorConTrasn(int color)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Cambiar Color-NH");

                    Element textNoteType = _doc.GetElement(txNote.GetTypeId());
                    Parameter param = textNoteType.get_Parameter(BuiltInParameter.LINE_COLOR);
                    param.Set(color);

                    t.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


        //obs1)
        public bool M2_CrearTipoText_ConTrasn()
        {
            var textNoteElement = TiposTextNote.ObtenerTextNote(nameTipoTexto, _doc);
            if (textNoteElement != null) return true;

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TipoTextNote-NH");

                    TextNoteType textNotetype_ = TiposTextNote.ObtenerPrimeroEncontrado(_doc);
                    //TextNote textNote = _doc.GetElement(textNoteDefol.GetTypeId()) as TextNote;
                    //TextNote textNote = textNoteDefol as TextNote;
                    // Create a duplicate
                    Element ele = textNotetype_.Duplicate(nameTipoTexto);

                    TextNoteType noteType = ele as TextNoteType;

                    if (null != noteType)
                    {
                        int color2 = Util.ToColorParameterValue(255, 0, 0);

                        noteType.get_Parameter(BuiltInParameter.TEXT_FONT).Set("Arial");
                        Parameter param = noteType.get_Parameter(BuiltInParameter.LINE_COLOR);
                        param.Set(color2);
                        //noteType.get_Parameter(BuiltInParameter.TEXT_COLOR).Set(color2);
                        noteType.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(0.00656168);
                        noteType.get_Parameter(BuiltInParameter.TEXT_TAB_SIZE).Set(0.04166667);
                    }

                    t.Commit();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }

            return true;
        }


        public bool M2_CrearListaTipoText_ConTrans(List<TipoTextoDTO> ListaTipoNote)
        {
   
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TipoTextNote-NH");
                    foreach (TipoTextoDTO item in ListaTipoNote)
                    {
                        var textNoteElement = TiposTextNote.ObtenerTextNote(item._Nombre, _doc);
                        if (textNoteElement != null) continue;


                        TextNoteType textNotetype_ = TiposTextNote.ObtenerPrimeroEncontrado(_doc);
                        //TextNote textNote = _doc.GetElement(textNoteDefol.GetTypeId()) as TextNote;
                        //TextNote textNote = textNoteDefol as TextNote;
                        // Create a duplicate
                        Element ele = textNotetype_.Duplicate(item._Nombre);

                        TextNoteType noteType = ele as TextNoteType;

                        if (null != noteType)
                        {
                            int color2 = Util.ToColorParameterValue(item._red, item._green, item._blue);

                            noteType.get_Parameter(BuiltInParameter.TEXT_FONT).Set(item._TEXT_FONT);
                            Parameter param = noteType.get_Parameter(BuiltInParameter.LINE_COLOR);
                            param.Set(color2);
                            //noteType.get_Parameter(BuiltInParameter.TEXT_COLOR).Set(color2);
                            noteType.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(item._TEXT_SIZE);
                            noteType.get_Parameter(BuiltInParameter.TEXT_TAB_SIZE).Set(item._TEXT_TAB_SIZE);
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear 'CrearListaTipoText'. ex:{ex.Message}  ");
                return false;
            }

            return true;
        }

        public bool M3_cambiarColorsinTrasn(int color)
        {
            try
            {
                Element textNoteType = _doc.GetElement(txNote.GetTypeId());

                Parameter param = textNoteType.get_Parameter(BuiltInParameter.LINE_COLOR);
                param.Set(color);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
