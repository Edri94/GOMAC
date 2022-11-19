using GOMAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOMAC.Data
{
    public class FuncionesBdbmtktp01 : bmtktp01Entities
    {
        public static DateTime fecha_default;

        public static void Mac_Inserta_Datos(int id_ConsultorMac, int id_Solicitud, int id_Tramite, int puntos, string circuito, string cuenta_Cliente, string sufijo_Kapiti, byte tipo_Persona, string nombre_Cliente, string apellido_Paterno, string apellido_Materno, string deposito_Inicial, string numero_Registro, string nombre_Promotor, string banca, string division, string plaza, string sucursal, string status, int num_Solicitud, DateTime fechaRepc_Doc, TimeSpan horaRepc_Doc, DateTime fechaAnalisis_Mac, TimeSpan horaAnalisis_Mac, DateTime fechaFormalizada, TimeSpan horaFormalizada, DateTime fechaRepc_Originales, TimeSpan horaRepc_Originales, DateTime fechaAten_Originales, TimeSpan horaAten_Originales, string originales, decimal deposito_Inicial_Ini, DateTime fecha_Desbloqueo, DateTime fecha_Envio, DateTime fecha_concluida, string existeTKT)
        {
            DateTime FechaHoraCaptura;
            decimal Deposito_Inicial_Tkt;
            DateTime Formalizada;
            DateTime Repc_Originales;
            DateTime Aten_Originales;
            DateTime Desbloqueo_Sistemas;
            string Envio_Agencia;
            int Diferencia;
            DateTime Repc_Doc;
            DateTime Analisis_Mac;


            FechaHoraCaptura = DateTime.Now;

            if(FechaHoraCaptura > fecha_default)
            {
                Repc_Doc = fechaRepc_Doc.Date + horaRepc_Doc;           
            }

            if(fechaAnalisis_Mac > fecha_default)
            {
                Analisis_Mac = fechaAnalisis_Mac.Date + horaAnalisis_Mac;
            }

            if (fechaFormalizada > fecha_default)
            {
                Formalizada = fechaFormalizada.Date + horaFormalizada;
            }

            if (fechaRepc_Originales > fecha_default)
            {
                Repc_Originales = fechaRepc_Originales.Date + horaRepc_Originales;
            }


            if (fechaAten_Originales > fecha_default)
            {
                Aten_Originales = fechaAten_Originales.Date + horaAten_Originales;
            }


            if (fecha_Desbloqueo > fecha_default)
            {
                Desbloqueo_Sistemas = fecha_Desbloqueo.Date;
            }

        }
    }
}
