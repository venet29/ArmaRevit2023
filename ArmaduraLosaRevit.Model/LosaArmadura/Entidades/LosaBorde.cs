using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LosaArmadura.Geometria
{
    public class LosaBorde
    {
#pragma warning disable CS0169 // The field 'LosaBorde.reference' is never used
        private Reference reference;
#pragma warning restore CS0169 // The field 'LosaBorde.reference' is never used

        public XYZ ptoInicial { get; set; }
        public XYZ ptofinal { get; set; }
        public Curve roomSeparateCurve { get; set; }

#pragma warning disable CS0169 // The field 'LosaBorde._doc' is never used
        private readonly Document _doc;
#pragma warning restore CS0169 // The field 'LosaBorde._doc' is never used
        private Element elem;
        private string tipoSelecionado;

        public LosaBorde(XYZ ptoInicial, XYZ ptofinal, TipoExtSeparateRoom extendert)
        {
            if (extendert == TipoExtSeparateRoom.Izquierda)// extiende inicio
            { ptoInicial = Util.ExtenderPuntoCOnRespeco2ptos(ptofinal, ptoInicial, Util.CmToFoot(2)); }
            else if (extendert == TipoExtSeparateRoom.Derecha) //extiende fin
            { ptofinal = Util.ExtenderPuntoCOnRespeco2ptos(ptoInicial, ptofinal, Util.CmToFoot(2)); }
            else if (extendert == TipoExtSeparateRoom.Ambos)//extiende fin
            {
                ptoInicial = Util.ExtenderPuntoCOnRespeco2ptos(ptofinal, ptoInicial, Util.CmToFoot(2));
                ptofinal = Util.ExtenderPuntoCOnRespeco2ptos(ptoInicial, ptofinal, Util.CmToFoot(2));
            }

            this.ptoInicial = ptoInicial;
            this.ptofinal = ptofinal;


            this.roomSeparateCurve = Line.CreateBound(ptoInicial, ptofinal);
            ConstNH.sbLog.AppendLine("          Recorrido sin elementos p1: " + ptoInicial + "p2: " + ptofinal);
        }

        public LosaBorde(XYZ ptoInicial, XYZ ptofinal, Element el, TipoExtSeparateRoom extendert)
        {
            if (extendert == TipoExtSeparateRoom.Izquierda)// extiende inicio
            { ptoInicial = Util.ExtenderPuntoCOnRespeco2ptos(ptofinal, ptoInicial, Util.CmToFoot(2)); }
            else if (extendert == TipoExtSeparateRoom.Derecha) //extiende fin
            { ptofinal = Util.ExtenderPuntoCOnRespeco2ptos(ptoInicial, ptofinal, Util.CmToFoot(2)); }
            else if (extendert == TipoExtSeparateRoom.Ambos)//extiende fin
            {
                ptoInicial = Util.ExtenderPuntoCOnRespeco2ptos(ptofinal, ptoInicial, Util.CmToFoot(2));
                ptofinal = Util.ExtenderPuntoCOnRespeco2ptos(ptoInicial, ptofinal, Util.CmToFoot(2));
            }
            this.ptoInicial = ptoInicial;
            this.ptofinal = ptofinal;
            this.elem = el;
            this.roomSeparateCurve = Line.CreateBound(this.ptoInicial, this.ptofinal);
            if (el is Wall)
            { tipoSelecionado = "Wall"; }
            else if (el is FamilyInstance)
            { tipoSelecionado = "Beam"; }

            ConstNH.sbLog.AppendLine("          Recorrido sin elementos p1: " + this.ptoInicial + "p2: " + this.ptofinal + " Tipo:" + tipoSelecionado);

        }




    }
}
