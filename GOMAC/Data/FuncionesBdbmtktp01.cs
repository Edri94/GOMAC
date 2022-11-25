using GOMAC.Helpers;
using GOMAC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOMAC.Data
{
    public class FuncionesBdbmtktp01 : bmtktp01Entities
    {
        public static DateTime fecha_default; 

        public virtual Mac_Inserta_Respuesta Mac_Inserta_Datos(int id_ConsultorMac, int id_Solicitud, int id_Tramite, int puntos, string circuito, string cuenta_Cliente, string sufijo_Kapiti, byte tipo_Persona, string nombre_Cliente, string apellido_Paterno, string apellido_Materno, decimal deposito_Inicial, string numero_Registro, string nombre_Promotor, string banca, string division, string plaza, string sucursal, string status, int num_Solicitud, DateTime fechaRepc_Doc, TimeSpan horaRepc_Doc, DateTime fechaAnalisis_Mac, TimeSpan horaAnalisis_Mac, DateTime fechaFormalizada, TimeSpan horaFormalizada, DateTime fechaRepc_Originales, TimeSpan horaRepc_Originales, DateTime fechaAten_Originales, TimeSpan horaAten_Originales, string originales, decimal deposito_Inicial_Ini, DateTime fecha_Desbloqueo, DateTime fecha_Envio, DateTime fecha_concluida, string existeTKT)
        {
            DateTime FechaHoraCaptura = default;
            decimal Deposito_Inicial_Tkt;
            DateTime Formalizada = default;
            DateTime Repc_Originales = default;
            DateTime Aten_Originales = default;
            DateTime Desbloqueo_Sistemas = default;
            DateTime Envio_Agencia = default;
            int Diferencia;
            DateTime Repc_Doc = default;
            DateTime Analisis_Mac = default;


            using (var context = new bmtktp01Entities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        FechaHoraCaptura = DateTime.Now;

                        if (FechaHoraCaptura > fecha_default)
                        {
                            Repc_Doc = fechaRepc_Doc.Date + horaRepc_Doc;
                        }

                        if (fechaAnalisis_Mac > fecha_default)
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

                        if(fecha_Envio > fecha_default)
                        {
                            Envio_Agencia = fecha_Envio.Date;
                        }

                        if(fecha_concluida > fecha_default)
                        {
                            fecha_concluida = fecha_concluida.Date;
                        }

                        SEGUIMIENTO nuevo_seguimiento = new SEGUIMIENTO
                        {
                            Id_ConsultorMac = id_ConsultorMac,
                            Id_Solicitud = id_Solicitud,
                            Id_Tramite = id_Tramite,
                            Puntos = puntos,
                            Circuito = circuito,
                            Cuenta_Cliente = cuenta_Cliente,
                            Sufijo_Kapiti = sufijo_Kapiti.ToUpper(),
                            Nombre_Cliente = nombre_Cliente.ToUpper(),
                            Apellido_Paterno = apellido_Paterno.ToUpper(),
                            Apellido_Materno = apellido_Materno.ToUpper(),
                            Deposito_InicialTKT = deposito_Inicial,
                            Numero_Registro = numero_Registro,
                            Nombre_Promotor = nombre_Promotor.ToUpper(),
                            Banca = banca.ToUpper(),
                            Division = division.ToUpper(),
                            Plaza = plaza.ToUpper(),
                            Sucursal = sucursal.ToUpper(),
                            Status = status.ToUpper(),
                            Fecha_Captura = DateTime.Now,
                            ExisteTKT = existeTKT
                        };

                        context.SEGUIMIENTO.Add(nuevo_seguimiento);
                        int resp = context.SaveChanges();

                       
                        int seguimiento_identuty =  nuevo_seguimiento.Num_Solicitud;

                        SEGUIMIENTO_DOCTOS nuevo_seguimientodoctos = new SEGUIMIENTO_DOCTOS{ 
                            Num_Solicitud = num_Solicitud,
                            Repc_Doc = Repc_Doc,
                            Formalizada = Formalizada,
                            Repc_Originales = Repc_Originales,
                            Aten_Originales = Aten_Originales,
                            Originales = originales,
                            Deposito_Inicial = deposito_Inicial,
                            Desbloqueo_Sistemas = Desbloqueo_Sistemas,
                            Envio_Agencia = Envio_Agencia,
                            Concluida = fecha_concluida,
                            Analisis_Mac = Analisis_Mac
                        };

                        context.SEGUIMIENTO_DOCTOS.Add(nuevo_seguimientodoctos);
                        context.SaveChanges();

                        int segdoctos_identity = nuevo_seguimiento.Num_Solicitud;

                        TimeSpan diferiencia = TimeSpan.Zero;

                        if (fechaAnalisis_Mac != fecha_default)
                        {
                            diferiencia = (fechaRepc_Doc - fechaAnalisis_Mac);

                        }

                        dbContextTransaction.Commit();


                        return new Mac_Inserta_Respuesta { Codigo = 0, Diferiencia = diferiencia, FechaHora_Captura = DateTime.Now };
                    }
                    catch(DbEntityValidationException  ex)
                    {
                        foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {
                                string error = $"- Property: \"{ ve.PropertyName}\", Value: \"{ eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName)}\", Error: \"{ve.ErrorMessage}\"";
                                Log.Escribe(error, "Error!!!!!!");
                            };
                        }
                        return new Mac_Inserta_Respuesta { Codigo = 1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };
                    }
                    catch (Exception ex)
                    {
                        Log.Escribe(ex);
                        dbContextTransaction.Rollback();
                        return new Mac_Inserta_Respuesta { Codigo = 1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };
                    }

                }

            }
        }

       
    }
}
