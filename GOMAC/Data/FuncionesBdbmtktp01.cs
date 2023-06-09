﻿using GOMAC.Helpers;
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

        internal virtual Mac_Inserta_Respuesta Mac_Inserta_Datos(int id_ConsultorMac, int id_Solicitud, int id_Tramite, int puntos, string circuito, string cuenta_Cliente, string sufijo_Kapiti, byte tipo_Persona, string nombre_Cliente, string apellido_Paterno, string apellido_Materno, decimal deposito_Inicial, string numero_Registro, string nombre_Promotor, string banca, string division, string plaza, string sucursal, string status, int num_Solicitud, DateTime fechaRepc_Doc, TimeSpan horaRepc_Doc, DateTime fechaAnalisis_Mac, TimeSpan horaAnalisis_Mac, DateTime fechaFormalizada, TimeSpan horaFormalizada, DateTime fechaRepc_Originales, TimeSpan horaRepc_Originales, DateTime fechaAten_Originales, TimeSpan horaAten_Originales, string originales, decimal deposito_Inicial_Ini, DateTime fecha_Desbloqueo, DateTime fecha_Envio, DateTime fecha_concluida, string existeTKT, List<OBSERVACIONES> lstObservaciones)
        {
            DateTime FechaHoraCaptura = default;
            decimal Deposito_Inicial_Tkt;
            DateTime Formalizada = fecha_default;
            DateTime Repc_Originales = fecha_default;
            DateTime Aten_Originales = fecha_default;
            DateTime Desbloqueo_Sistemas = fecha_default;
            DateTime Envio_Agencia = fecha_default;
            TimeSpan diferencia = TimeSpan.Zero;
            DateTime Repc_Doc = fecha_default;
            DateTime Analisis_Mac = fecha_default;
            int afectados = -1;


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
                            Tipo_Persona = tipo_Persona,
                            Nombre_Cliente = nombre_Cliente.ToUpper(),
                            Apellido_Paterno = apellido_Paterno.ToUpper(),
                            Apellido_Materno = apellido_Materno.ToUpper(),
                            Deposito_InicialTKT = deposito_Inicial,
                            Numero_Registro = numero_Registro,
                            Nombre_Promotor = nombre_Promotor.ToUpper(),
                            Banca = banca,
                            Division = division,
                            Plaza = plaza,
                            Sucursal = sucursal,
                            Status = status,
                            Fecha_Captura = DateTime.Now,
                            ExisteTKT = existeTKT,
                        };

                        context.SEGUIMIENTO.Add(nuevo_seguimiento);
                        afectados = context.SaveChanges();

                        SEGUIMIENTO seguimiento_guardado = context.SEGUIMIENTO.Where(w => w.Num_Solicitud == nuevo_seguimiento.Num_Solicitud).FirstOrDefault();


                        if (afectados > 0)
                        {
                            if (seguimiento_guardado != null)
                            {
                                SEGUIMIENTO_DOCTOS nuevo_seguimientodoctos = new SEGUIMIENTO_DOCTOS
                                {
                                    Num_Solicitud = seguimiento_guardado.Num_Solicitud,
                                    Repc_Doc = Repc_Doc,
                                    Formalizada = Formalizada,
                                    Repc_Originales = Repc_Originales,
                                    Aten_Originales = Aten_Originales,
                                    Originales = originales,
                                    Deposito_Inicial = deposito_Inicial_Ini,
                                    Desbloqueo_Sistemas = Desbloqueo_Sistemas,
                                    Envio_Agencia = Envio_Agencia,
                                    Concluida = fecha_concluida,
                                    Analisis_Mac = Analisis_Mac,
                                    SEGUIMIENTO = seguimiento_guardado
                                };

                                context.SEGUIMIENTO_DOCTOS.Add(nuevo_seguimientodoctos);
                                afectados = context.SaveChanges();

                                int segdoctos_identity = nuevo_seguimiento.Num_Solicitud;


                                if (fechaAnalisis_Mac != fecha_default)
                                {
                                    diferencia = (fechaRepc_Doc - fechaAnalisis_Mac);

                                }

                                dbContextTransaction.Commit();

                                return new Mac_Inserta_Respuesta { Codigo = afectados, Diferiencia = diferencia, FechaHora_Captura = DateTime.Now };
                            }


                        }
                        dbContextTransaction.Rollback();
                        return new Mac_Inserta_Respuesta { Codigo = -1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };

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
                        return new Mac_Inserta_Respuesta { Codigo = -1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };
                    }
                    catch (Exception ex)
                    {
                        Log.Escribe(ex);
                        Log.Escribe(ex.InnerException);
                        dbContextTransaction.Rollback();
                        return new Mac_Inserta_Respuesta { Codigo = -1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };
                    }

                }

            }
        }

        internal Mac_Actualiza_Respuesta Mac_Actualiza_Datos(int id_ConsultorMac, int id_Solicitud, int id_Tramite, int puntos, string circuito, string cuenta_Cliente, string sufijo_Kapiti, byte tipo_Persona, string nombre_Cliente, string apellido_Paterno, string apellido_Materno, decimal deposito_Inicial, string numero_Registro, string nombre_Promotor, string banca, string division, string plaza, string sucursal, string status, int num_Solicitud, DateTime fechaRepc_Doc, TimeSpan horaRepc_Doc, DateTime fechaAnalisis_Mac, TimeSpan horaAnalisis_Mac, DateTime fechaFormalizada, TimeSpan horaFormalizada, DateTime fechaRepc_Originales, TimeSpan horaRepc_Originales, DateTime fechaAten_Originales, TimeSpan horaAten_Originales, string originales, decimal deposito_Inicial_Ini, DateTime fecha_Desbloqueo, DateTime fecha_Envio, DateTime fecha_concluida, string existeTKT, List<OBSERVACIONES> lstObservaciones)
        {
            DateTime FechaHoraCaptura = fecha_default;
            decimal Deposito_Inicial_Tkt;
            DateTime Formalizada = fecha_default;
            DateTime Repc_Originales = fecha_default;
            DateTime Aten_Originales = fecha_default;
            DateTime Desbloqueo_Sistemas = fecha_default;
            DateTime Envio_Agencia = fecha_default;
            TimeSpan diferencia = TimeSpan.Zero;
            DateTime Repc_Doc = fecha_default;
            DateTime Analisis_Mac = fecha_default;
            int afectados = -1;


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

                        if (fecha_Envio > fecha_default)
                        {
                            Envio_Agencia = fecha_Envio.Date;
                        }

                        if (fecha_concluida > fecha_default)
                        {
                            fecha_concluida = fecha_concluida.Date;
                        }


                        SEGUIMIENTO seguimiento_actualizar = (from s in context.SEGUIMIENTO where s.Num_Solicitud == num_Solicitud select s).FirstOrDefault();

                        if(seguimiento_actualizar != null)
                        {
                            SEGUIMIENTO_DOCTOS seguimientodoc_actualizar = seguimiento_actualizar.SEGUIMIENTO_DOCTOS.FirstOrDefault();
                           
                            seguimientodoc_actualizar.Repc_Doc = Repc_Doc;
                            seguimientodoc_actualizar.Formalizada = Formalizada;
                            seguimientodoc_actualizar.Repc_Originales = Repc_Originales;
                            seguimientodoc_actualizar.Aten_Originales = Aten_Originales;
                            seguimientodoc_actualizar.Originales = originales;
                            seguimientodoc_actualizar.Deposito_Inicial = deposito_Inicial_Ini;
                            seguimientodoc_actualizar.Desbloqueo_Sistemas = Desbloqueo_Sistemas;
                            seguimientodoc_actualizar.Envio_Agencia = Envio_Agencia;
                            seguimientodoc_actualizar.Concluida = fecha_concluida;
                            seguimientodoc_actualizar.Analisis_Mac = Analisis_Mac;
                            

                            context.Entry(seguimientodoc_actualizar).State = System.Data.Entity.EntityState.Modified;

                            //context.SEGUIMIENTO_DOCTOS.Add(actualizar_seguimientodoctos);
                            afectados = context.SaveChanges();

                            if (lstObservaciones.Count > 0)
                            {
                                context.OBSERVACIONES.AddRange(lstObservaciones);
                                afectados = context.SaveChanges();

                                if (afectados > 0)
                                {
                                    List<SEGUIMIENTO_OBSERVACIONES> observaciones_seg = new List<SEGUIMIENTO_OBSERVACIONES>();

                                    foreach (OBSERVACIONES observacion in lstObservaciones)
                                    {
                                        observaciones_seg.Add(new SEGUIMIENTO_OBSERVACIONES { Num_Solicitud = seguimientodoc_actualizar.Num_Solicitud, Id_Observacion = observacion.Id_Observacion });
                                    }

                                    context.SEGUIMIENTO_OBSERVACIONES.AddRange(observaciones_seg);
                                    afectados = context.SaveChanges();

                                    if (afectados > 0)
                                    {
                                        seguimiento_actualizar.Id_ConsultorMac = id_ConsultorMac;
                                        seguimiento_actualizar.Id_Solicitud = id_Solicitud;
                                        seguimiento_actualizar.Id_Tramite = id_Tramite;
                                        seguimiento_actualizar.Puntos = puntos;
                                        seguimiento_actualizar.Circuito = circuito;
                                        seguimiento_actualizar.Cuenta_Cliente = cuenta_Cliente;
                                        seguimiento_actualizar.Sufijo_Kapiti = sufijo_Kapiti.ToUpper();
                                        seguimiento_actualizar.Tipo_Persona = tipo_Persona;
                                        seguimiento_actualizar.Nombre_Cliente = nombre_Cliente.ToUpper();
                                        seguimiento_actualizar.Apellido_Paterno = apellido_Paterno.ToUpper();
                                        seguimiento_actualizar.Apellido_Materno = apellido_Materno.ToUpper();
                                        seguimiento_actualizar.Deposito_InicialTKT = deposito_Inicial;
                                        seguimiento_actualizar.Numero_Registro = numero_Registro;
                                        seguimiento_actualizar.Nombre_Promotor = nombre_Promotor.ToUpper();
                                        seguimiento_actualizar.Banca = banca.ToUpper();
                                        seguimiento_actualizar.Division = division.ToUpper();
                                        seguimiento_actualizar.Plaza = plaza.ToUpper();
                                        seguimiento_actualizar.Sucursal = sucursal.ToUpper();
                                        seguimiento_actualizar.Status = status.ToUpper();
                                        seguimiento_actualizar.Fecha_Captura = DateTime.Now;
                                        seguimiento_actualizar.ExisteTKT = existeTKT;


                                        context.Entry(seguimiento_actualizar).State = System.Data.Entity.EntityState.Modified;
                                        //context.SEGUIMIENTO.Add(seguimiento_actualizar);
                                        afectados = context.SaveChanges();


                                        if (fechaAnalisis_Mac != fecha_default)
                                        {
                                            diferencia = (fechaRepc_Doc - fechaAnalisis_Mac);

                                        }

                                        dbContextTransaction.Commit();
                                        return new Mac_Actualiza_Respuesta { Codigo = afectados, Diferiencia = diferencia, FechaHora_Captura = DateTime.Now };

                                    }
                                }
                            }
                          
                        }

                        dbContextTransaction.Rollback();
                        return new Mac_Actualiza_Respuesta { Codigo = -1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };

                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult eve in ex.EntityValidationErrors)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {
                                string error = $"- Property: \"{ ve.PropertyName}\", Value: \"{ eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName)}\", Error: \"{ve.ErrorMessage}\"";
                                Log.Escribe(error, "Error!!!!!!");
                            };
                        }
                        return new Mac_Actualiza_Respuesta { Codigo = -1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };
                    }
                    catch (Exception ex)
                    {
                        Log.Escribe(ex);
                        dbContextTransaction.Rollback();
                        return new Mac_Actualiza_Respuesta { Codigo = -1, Diferiencia = TimeSpan.Zero, FechaHora_Captura = DateTime.Now };
                    }

                }

            }
        }

        internal static int Actualiza_Docs(int numero_solicitud)
        {
            using (var context = new bmtktp01Entities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    int afectados = -1;

                    try
                    {
                        SEGUIMIENTO_DOCTOS seguimeinto_docto = (from sd in context.SEGUIMIENTO_DOCTOS where sd.Num_Solicitud == numero_solicitud select sd).FirstOrDefault();
                        DateTime fecha_servidor = DateTime.Now;
                        seguimeinto_docto.Concluida = fecha_servidor; 
                        context.Entry(seguimeinto_docto).State = System.Data.Entity.EntityState.Modified;

                        afectados = context.SaveChanges();

                        SEGUIMIENTO seguimiento = (from s in context.SEGUIMIENTO where s.Num_Solicitud == numero_solicitud select s).FirstOrDefault();
                        seguimiento.Status = "2";
                        context.Entry(seguimiento).State = System.Data.Entity.EntityState.Modified;

                        afectados = context.SaveChanges();

                        if (afectados > 0)
                        {
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                        }
                        return afectados;
                    }
                    catch (Exception ex)
                    {
                        Log.Escribe(ex);
                        dbContextTransaction.Rollback();
                        return -1;
                    }
                   
                }
            }
            
        }

        internal object Mac_Obtiene_FechaServidor()
        {
            throw new NotImplementedException();
        }
    }
}
