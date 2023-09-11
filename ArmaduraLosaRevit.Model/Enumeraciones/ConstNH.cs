using ArmaduraLosaRevit.Model.Prueba;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Enumeraciones
{
    public class ConstNH
    {
        public static void corte() { }
        public static void VersionSObre2022() { }

        public static string CONST_FILTRAR_HORMIGON_POBRE = "h.p.";  //"para filtrar hormigon pobre"
        public static double CONST_LARGOMIN_DIMENSION_PASADA = Util.CmToFoot(2);

        public static double CONST_FACT_TOLERANCIA_SketchPlane = 0.000001;
        public static double CONST_FACT_TOLERANCIA_3Deci = 0.001;

        //busque barra caebeza muro
        public static double CONST_DESPLAMIENTO_RESPECTO_POSTERIOR_FOOT = Util.CmToFoot(40);
        public static double CONST_DESPLAMIENTO_RESPECTO_ANTERIOR_FOOT = Util.CmToFoot(40);

        public static bool CONST_CORTO = true;
        // constantes  refuezo de losa
        public static char CARACTER_PTO = '.';

        public static string ERROR_CONEXION = "Error CX";  //"Error al validad credencial (No conexion)"
        public static string ERROR_OBTENERDATOS = "Error datos";  //"Error al validad credencial (No conexion)"

        public static string NOMBRE_VIEW_TEMPLATE_LOSA = "200_VISTA ARMADURAS DE LOSAS";
        public static string NOMBRE_VIEW_TEMPLATE_ELEV = "300_ARMADURA DE ELEVACIONES";
        public static string NOMBRE_VIEW_TEMPLATE_ESTRUC = "PLANTA ESTRUCTURA CIELO (VT)";
        public static string NOMBRE_VIEW_TEMPLATE_FUND = "PLANTA FUNDACIONES (VT)";




        public static int CONST_CANTIDAD_TRABA_MALLA_XM2 = 6; // 6 po metro cuadrado
        public static int CONST_CANTIDAD_PatABarraFundaciOn_XM2 = 3; // 3 po metro cuadrado
        public static int CONST_CANTIDAD_PatABarralOSA_XM2 = 4; // 3 po metro cuadrado
        public static double CONST_ESPACIAMIENTO_BARRREPARTICION_CM = 25;
        public static double LARGO_LINEA_AUXILIAR_FOOT = 400f;
        public static int REFLOSA_ESPACIEMIENTO_ENTREBARRA_CM = 10;
        public static int REFLOSA_ESPACIEMIENTO_BORDELOSA_CM = 5;
        //
        public static string NOMBRE_BARRA_PRINCIPAL = "i";
        public static string NOMBRE_BARRA_SECUNADARIA = "s";
        public static string NOMBRE_PLANOLOSA_INF = "INFERIOR";
        public static string NOMBRE_PLANOLOSA_SUP = "SUPERIOR Y REFUERZOS";
        public static double PORCENTAJE_LARGO_PATA = 0.15;
        public static double TOLERANCIACERO = 1E-05;
        public static double TOLERANCIACERO_3mm = 0.01;
        public static StringBuilder sbLog = new StringBuilder();
        public static string rutaLogNh = @"J:\_revit\PROYECTOS_REVIT\REVITArmaduraLosaRevit\ArmaduraLosaRevit_2022\ArmaduraLosaRevit.Model\bin\Debug";

        public static string rutaPArameterCOmpartido = @"\\SERVER-CDV\Programas Ingenieria\RevitGeneral\ParametroCompartidos\Parametros Compartidos.txt";
        public static string rutaFamilias = @"\\SERVER-CDV\Programas Ingenieria\APi\FAMILY\";

        public static string BASE_FAMILIA_PATHREINF_TAG = "M_Path Reinforcement Tag(ID_cuantia_largo)";
        public static string BASE_FAMILIA_REBAR_TAG = "M_Path Reinforcement Tag(ID_cuantia_largo)";

        public static double CONST_EXTRADESDFASE_FOOT = Util.CmToFoot(20); //  // este parametro aumente el largo de ahoro en el path reinformen para sumular espesor de muro 
                                                                           // public static double CONST_LARGO_EXTRA_DESDFASEAHORRA_PORMURO = Util.CmToFoot(20); // este parametro aumente el largo de ahoro en el path reinformen para sumular espesor de muro 

        // recubrimieto se utiza para todos los elemtos 
        public static int CONST_RECUBRIMIENTO_BAse2cm_MM = 20;
        public static int CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM = 100;

        public static double CONST_BAJOCIELO_DETAIL_FOOT = Util.CmToFoot(60);

        public static double LARGO_MIN_PATH_S4_FOOT = Util.CmToFoot(125);

        public static double CONST_ANCHO_CORTE_DESGLOSE = Util.CmToFoot(10);
        public static double CONST_FACTOR_DISMINUIR_1BARRA = 0.35;  // si laparte decimal del recorrido es menor que esto se resta una linea de barra

        public static double CONST_FACTOR_LARGOSUPLE = 0.25;
        public static double CONST_PORCENTAJE_LARGOAHORRO = 0.15;
        public static double CONST_PORCENTAJE_LARGOPATA = 0.15;
        public static string CONST_TAGLOSAESTRUCTURAL = "PELOTA DE LOSAS (NH)";
        public static string CONST_TAGLOSAARMA = "PELOTA DE LOSAS (NH)ARMA";
        public static string CONST_TAGROOMFAMILIA = "TagRoomFAmilia (NH)";
        // Constante que define la separacion entre barra por traslapo entre rebar shape , se aprecia en el 3D
        public static double CONST_DESPLA_ENTRE_BARRAS_TRASLAPO_CM = 1.5;
        public static double CONST_DESPLA_ENTRE_BARRAS_F1_SUP_CM = 2;
        // Constante que define la separacion entre barra por traslapo del path reinforment symbol
        public static int CONST_PATH_SYMBOL_CM_PATH = 3;
        public static double aumentarAnchoEstribo_foot = Util.CmToFoot(3);
        public static double RECUBRIMIENTO_BORDE_LOSA_LATERAL = Util.CmToFoot(2) + aumentarAnchoEstribo_foot; // 2 recubrimiento
        public static double RECUBRIMIENTO_LOSA_SUP_cm = 2;
        public static double RECUBRIMIENTO_LOSA_MURO_cm = 2; // SE ITILIZA CUANDO SE ENCUENTRA MURO EN ANAILZIA DE LOSA
        public static double RECUBRIMIENTO_LOSA_INF_cm = 2;

        public static double CONST_DESPLAZA_PORLUZSECUNDARIO_FOOT = Util.CmToFoot(1);
        public static double CONST_DESPLAZA_PORLUZSECUNDARIO_CM = 1;


        public static double DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT = 1.46282;
        public static double DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT = Util.CmToFoot(2.5);
        public static double DESPLAZAMIENTO_ARRIBA_Z_REBAR_FOOT = Util.CmToFoot(2.5);


        public static double RECUBRIMIENTO_BARRA_VERT_CM = 2 + 1; // 2: recub , 1: espesor estribo o malla
        public static double RECUBRIMIENTO_BARRA_MALLA_CM = 2; // 2: recub , 1.5: espesor estribo o malla
        public static double RECUBRIMIENTO_BUSCAR_BARRACABEZA_MALLA_FOOT = Util.CmToFoot(3.9); // 2: recub , 1.5: espesor estribo o malla
        public static double RECUBRIMIENTO_BARRA_HORI_CM = 2 + 1; // 2: recub , 1: espesor estribo o malla
        public static double RECUBRIMIENTO_MURO_CM = 2;
        public static double CONST_LARGO_PATAMALLA_FOOT = Util.CmToFoot(35);

        public static double DESPLAZA_LATERALES_TRASLAPO = Util.CmToFoot(1.5);


        public static double RECUBRIMIENTO_FUNDACIONES_foot = Util.CmToFoot(5);

        public static int LARGO_PATA_FUNDACIONES_CM = 30;

        public static double RECUBRIMIENTO_VERTICAL_FUND_foot = Util.CmToFoot(5);

        public static double RECUBRIMIENTO_ESTRIBO_FOOT = Util.CmToFoot(2);
        public static double RECUBRIMIENTO_PATA_BARRAV_Foot = Util.CmToFoot(2);

        public static double DESPLAMIENTOS1_SUP = 4; //desplaza el pathsymbol del caso f1_sup

        // modo para mostrar formulario con resulatados  graficados
        public static bool MODO_DISEÑO = false;

        //distacian en porcentaje , de la ubicicion de la directriz para caso automatico
        //se considera un porcentaje del largo desde el lado izq_inf
        public static double DistanciaDirectrizEnporcentaje = 0.25;


        public static double ESPESORMURO_Izq_abajoFOOT = Util.CmToFoot(20);
        public static double ESPESORMURO_Dere_SupFOOT = Util.CmToFoot(20);

        public static int VIEWRANGE_TOP = 2;
        public static int VIEWRANGE_CORTE = 1;
        public static int VIEWRANGE_BOTTON = -1;
        public static int VIEWRANGE_DEPTH = -1;

        public static double CONST_PATA_SX = 15;////valor no definido, conversacion conversacion 04/08/2021

        public static XYZ CONST_SOBRE_LEVEL_SELECCION_LOSAFOOT = new XYZ(0, 0, Util.CmToFoot(15));

        public static int CONST_ESCALA_BASE = 50;

        public static double DIFERENCIA_ALTURA_PATHSYMBOLFALSOFOOT = Util.CmToFoot(140);

        public static string CONST_NOMBRE_3D_PARA_BUSCAR = VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR;// "3D_NoEditar";//"{3D}"
        public static string CONST_NOMBRE_3D_VISUALIZAR = "{3D}";

        //la distancia para buscar muro mas al centro del muro, pq el muro del borde puede ser de otro espesor o el que vienen entrando o saliendo
        public static double CONST_DISTANCIA_BUSQUEDA_MUROFOOT = Util.CmToFoot(35);
        public static double CONST_DESPLZAMIENTO_BUSQUEDA_MUROFOOT = 0.2; // intervalo de desplazamietno
        public static double CONST_DESVIACION_TRASLAPOFOOT = Util.CmToFoot(0.8);
        public static double CONST_RECUBRIMIENTO_PRIMERABARRAFOOT = Util.CmToFoot(2 + 1);// 2: recubirmeito + 2 : espesor estribo, se debe optimizar
        public static double CONST_RECUBRIMIENTO_PRIMERABARRAFOOT_MALLA = Util.CmToFoot(2);// 2: recubirmeito + 2 
        public static double CONST_MOVER_PTO_BUSCAR_MURO_FOOT = Util.CmToFoot(10); //cuando se busca un muro y este es perpendicular ala vista, se mover 10 cm el pto de b


        //distacia que baja el tag respecto al nivel superior de la barra, sin contar el traslapo
        public static double CONST_DISTANCIA_BAJATAG_Foot = Util.CmToFoot(100);
        public static double RECUBRIMIENTO_MALLA_foot = Util.CmToFoot(2);




         public const string CONST_COT = @"\\SERVER-CDV\Programas Ingenieria";//@"C:\";
        //public const string CONST_COT = @"C:\"; 

        public static double CONST_DISTANCIA_BUSQUEDA_MUROFOOT_PERPENDICULAR_VIEW = Util.CmToFoot(45);
        public static double CONST_DISTANCIA_RETROCESO_BUSQUEDA_FOOT_PERPENDICULAR_VIEW = Util.CmToFoot(20);

        public static double CONST_DISTANCIA_MIN_SELECCIO_MOUSE_FOOT = Util.CmToFoot(50);

        //obser2)
        public static double RECUBRIMIENTO_PATA_BARRAV_LINEAANALIZADO = Util.CmToFoot(2);

        public static double ESPACIA_BARRA_REFUERZO_BORDELOSA = Util.CmToFoot(10);

        public static TipoTerminacionCambioMuro TipoTerminacionCambioMuro = TipoTerminacionCambioMuro.SinPAta;

        public static double CONST_LARGOMIN_PATA = Util.CmToFoot(30);
        public static double CONST_LARGOMIN_BARRA_FOOT = Util.CmToFoot(10);

        public static int CONST_REDONDEAR_VIEW = 8;

        public static double RECUBRIMIENTO_FUNDACIONES_CONFINAMINETO_ABAJO_foot = Util.CmToFoot(30);

        public static string CONST_EMBEBER = "fefAv6LUDKqQJZMrK4ZrvQ==";


        // tipo de objetos anonimos para generar configuracion iniciales en muros , sepued extender a vigas y losas, rooms etc

        public static string CONST_TIPO_ANONIMO_ESTRIBOCORONACION = "TIPO_ANONYMO_CORONACION"; //tipoAnononimo=ConstNH.CONST_TIPO_ANONIMO_ESTRIBOCORONACION

    }
}