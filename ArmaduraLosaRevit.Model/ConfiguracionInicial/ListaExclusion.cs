using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial
{
   public class ListaExclusion_
    {

        public static List<string> ListaExclusionLosa()
        {
            List<string> listaExclusionLosa = new List<string>();

            //listaExclusionLosa.Add("Columns");

            //listaExclusionLosa.Add("Floors"); listaExclusionLosa.Add("Common Edges"); listaExclusionLosa.Add("Hidden Lines"); listaExclusionLosa.Add("Interior Edges"); listaExclusionLosa.Add("Slab Edges");
            //listaExclusionLosa.Add("Generic Models"); listaExclusionLosa.Add("Overhead"); listaExclusionLosa.Add("Opening Elevation"); listaExclusionLosa.Add("Hidden Lines");

            //listaExclusionLosa.Add("Lines");

            listaExclusionLosa.Add("Ramps");

            listaExclusionLosa.Add("Rooms");
            listaExclusionLosa.Add("Shaft Openings");

            listaExclusionLosa.Add("Stairs");

            //listaExclusionLosa.Add("Structural Area Reinforcement");
            //listaExclusionLosa.Add("Structural Beam Systems");
            //listaExclusionLosa.Add("Structural Columns");

            //listaExclusionLosa.Add("Structural Foundations");
            //listaExclusionLosa.Add("Structural Framing");
            //listaExclusionLosa.Add("Structural Path Reinforcement");
            //listaExclusionLosa.Add("Structural Rebar");


            //listaExclusionLosa.Add("Walls");

            listaExclusionLosa.AddRange(ListaExclusionElev());

            return listaExclusionLosa;
        }



        public static List<string> ListaExclusionElev()
        {
            List<string> listaExclusioneLEV = new List<string>();

            listaExclusioneLEV.Add("Columns");

            listaExclusioneLEV.Add("Floors"); listaExclusioneLEV.Add("Common Edges"); listaExclusioneLEV.Add("Hidden Lines"); listaExclusioneLEV.Add("Interior Edges"); listaExclusioneLEV.Add("Slab Edges");
            listaExclusioneLEV.Add("Generic Models"); listaExclusioneLEV.Add("Overhead"); listaExclusioneLEV.Add("Opening Elevation"); listaExclusioneLEV.Add("Hidden Lines");

            listaExclusioneLEV.Add("Lines");

           // listaExclusioneLEV.Add("Ramps");

           // listaExclusioneLEV.Add("Rooms");
           // listaExclusioneLEV.Add("Shaft Openings");

           // listaExclusioneLEV.Add("Stairs");
            listaExclusioneLEV.Add("Structural Area Reinforcement");
            listaExclusioneLEV.Add("Structural Beam Systems");
            listaExclusioneLEV.Add("Structural Columns");

            listaExclusioneLEV.Add("Structural Foundations");
            listaExclusioneLEV.Add("Structural Framing");
            listaExclusioneLEV.Add("Structural Path Reinforcement");
            listaExclusioneLEV.Add("Structural Rebar");


            listaExclusioneLEV.Add("Walls");
            return listaExclusioneLEV;
        }



        public static List<string> ListaExclusionLosaAnotatior()
        {
            List<string> listaExclusionLosa = new List<string>();

            listaExclusionLosa.Add("Columns");
            listaExclusionLosa.Add("Reference Lines");

            listaExclusionLosa.Add("Structural Framing Tags");
            listaExclusionLosa.Add("Section Boxes");
            listaExclusionLosa.Add("Structural Rebar Tags");
            listaExclusionLosa.Add("Reference Planes");
            listaExclusionLosa.Add("Rebar Cover References");
            listaExclusionLosa.Add("Structural Annotations");
            listaExclusionLosa.Add("Floor Tags");
            listaExclusionLosa.Add("Grids");
            listaExclusionLosa.Add("Line Load Tags");
            listaExclusionLosa.Add("Structural Path Reinforcement Symbols");
            listaExclusionLosa.Add("Structural Area Reinforcement Tags");
            listaExclusionLosa.Add("Generic Annotations");
            listaExclusionLosa.Add("Structural Column Tags");
            listaExclusionLosa.Add("Sections");
            listaExclusionLosa.Add("Room Tags");
            listaExclusionLosa.Add("Spot Slopes");
            listaExclusionLosa.Add("Rebar Set Toggle");
            listaExclusionLosa.Add("Structural Path Reinforcement Tags");
            listaExclusionLosa.Add("Structural Area Reinforcement Symbols");
            listaExclusionLosa.Add("Reference Points");
            listaExclusionLosa.Add("Spot Elevation Symbols");
            listaExclusionLosa.Add("Elevations");
            listaExclusionLosa.Add("Structural Rebar Coupler Tags");
            listaExclusionLosa.Add("Levels");
            listaExclusionLosa.Add("Structural Foundation Tags");
            listaExclusionLosa.Add("Spot Coordinates");
            listaExclusionLosa.Add("View Titles");
            listaExclusionLosa.Add("Structural Beam System Tags");
            listaExclusionLosa.Add("Generic Model Tags");
            listaExclusionLosa.Add("Text Notes");
            listaExclusionLosa.Add("Spot Elevations");
            listaExclusionLosa.Add("Dimensions");
            listaExclusionLosa.Add("Wall Tags");

            return listaExclusionLosa;
        }
    }
}
