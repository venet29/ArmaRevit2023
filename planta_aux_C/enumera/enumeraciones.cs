using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace planta_aux_C.enumera
{
	public class enumeraciones
	{



	}

    
       public enum TipoSeleccionMouse
    {
        HaciaArriba,
        HaciaBajo,
        HaciaDerecha,
        HaciaIzquierda
        
    
    }
    public enum DireccionBarra
    {
        vertical_a,
        vertical_b,
        horizontal_i,
        horizontal_d
        
    
    }

    public enum TipoPosicionTexto
    {
        TodoArriba,
        ArribayBajo


    }

    public enum Tiporefuerzo
    {
        CabezaMuro,
        ConEstribos


    }


    public enum OrientacionEstribo
    { 
        Horizontal,
        Vertical,
        Oblicuo    
    }
    public enum tipoDeBarra
    {
        refuerzoInferior,
        refuerzoInferior_autoInterseccion,
        suple
    }


    public enum UbicacionLosa
    {
        Derecha,
        Izquierda,
        Superior,
        Inferior
    }
}
