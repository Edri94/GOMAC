using GOMAC.Data;
using GOMAC.Helpers;
using GOMAC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOMAC.Views
{
    public partial class FrmNueva_Solicitud : Form 
    {
        private Frm_Login frmp;
        private bmtktp01Entities bdbmtktp01;
        private CATALOGOSEntities bdCatalogos;
        private FUNCIONARIOSEntities bdFuncionarios;
        private SEGUIMIENTO seguimiento_doc;

        private List<FUNCIONARIO> funcionarios;
        private List<UNIDAD_ORGANIZACIONAL_RESUMEN> uors;

        private string default_cmb = ". . . ";
        private DateTime default_dtp = DateTimePicker.MinimumDateTime;
        private NumberFormatInfo format_mxn = (NumberFormatInfo)CultureInfo.CreateSpecificCulture("es-MX").NumberFormat.Clone();
        private DataTable dt_observaciones;

        public string str_consultor;
        public int se_carga;
        public int num_solicitud;

        bool cmbNumeroFuncionario_activo = false, cmbConsultorMac_activo = false, cmbProducto_activo = false, cmbTipoSolicitud_activo = false, cmbTipoTramite_activo = false;

        /// <summary>
        /// Se ejecuta al escoger una fecha en el calendario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendario_DateSelected(object sender, DateRangeEventArgs e)
        {
            try
            {
                string str_fecha = Calendario.SelectionRange.Start.ToShortDateString();
                DateTime fecha_selec = Calendario.SelectionRange.Start;

                if ((fecha_selec >= DateTime.Now) || Int32.Parse(grpCalendario.Tag.ToString()) == 5)
                {
                    DIAS_FERIADOS dia_feriado = (from df in bdCatalogos.DIAS_FERIADOS where df.fecha == fecha_selec select df).FirstOrDefault();

                    if (dia_feriado == null)
                    {
                        MessageBox.Show("FAVOR DE SELECCIONAR UNA FECHA VALIDA", "VALIDACION DE DIAS FERIADOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        int tag = Int32.Parse(grpCalendario.Tag.ToString());

                        switch (tag)
                        {
                            case 1:
                                dtpFFormalizada.Value = Calendario.SelectionRange.Start;
                            break;

                            case 2:
                                dtpFRecepcion.Value = Calendario.SelectionRange.Start;
                            break;

                            case 3:
                                dtpFAtencion.Value = Calendario.SelectionRange.Start;
                            break;

                            case 4:
                                dtpDesbloqueo.Value = Calendario.SelectionRange.Start;
                            break;

                            case 5:
                                dtpEnvio.Value = Calendario.SelectionRange.Start;
                            break;
                            
                            case 6:
                                dtpConcluir.Value = Calendario.SelectionRange.Start;
                            break;
                            
                            case 7:
                                dtpFRecepDoc.Value = Calendario.SelectionRange.Start;
                            break;

                            case 8:
                                dtpFAnalisisMac.Value = Calendario.SelectionRange.Start;
                            break;                              
                        }
                        grpCalendario.Tag = null;
                        grpCalendario.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("La fecha no puede ser menor al dia", "VALIDACION DE FECHA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Se ejecuta al escoger un Consultor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbConsultorMac_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbConsultorMac_activo)
            {
                if (frmp.activa == 1)
                {
                    btnLimpiar.Visible = true;
                    btnGuardar.Visible = true;
                    btnCancelarSolicitud.Visible = true;

                    btnVerObservacion.Visible = true;
                    btnNuevaObservacion.Visible = true;
                    btnVerObservacion.Enabled = true;
                    btnLimpiar.Enabled = true;
                    btnGuardar.Enabled = true;
                    btnCancelarSolicitud.Enabled = true;

                }
                else
                {
                    btnLimpiar.Visible = false;
                    btnGuardar.Visible = false;
                    btnCancelarSolicitud.Visible = false;

                    btnVerObservacion.Visible = false;
                    btnNuevaObservacion.Visible = false;
                    btnVerObservacion.Enabled = false;
                    btnLimpiar.Enabled = false;

                    btnCancelarSolicitud.Enabled = false;

                    btnGuardar.Enabled = false;
                    btnNuevaObservacion.Enabled = false;
                    btnVerObservacion.Enabled = false;
                }

                if (btnGuardar.Text == "Guaradar" && lblStatus.Text == "")
                {
                    if (cmbTipoSolicitud.Enabled == true && btnGuardar.Visible == true)
                    {
                        cmbTipoSolicitud.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// Se ejecuta al escoger un numero de funcionario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNumeroFuncionario_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
            if (cmbNumeroFuncionario_activo)
            {
                FUNCIONARIO funcionario_selec = (FUNCIONARIO)cmbNumeroFuncionario.SelectedItem;

                if (str_consultor == "")
                {
                    if (cmbNumeroFuncionario.Text == default_cmb)
                    {
                        MessageBox.Show("Debe de proporcionar o selectcionar un numero de funcionario", "Numero no valido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                try
                {
                    FUNCIONARIO funcionario = (
                        from f in bdFuncionarios.FUNCIONARIO
                        join uor in bdFuncionarios.UNIDAD_ORGANIZACIONAL_RESUMEN on f.funcionario1 equals uor.funcionario
                        where f.numero_registro == funcionario_selec.numero_registro
                        select f
                    ).FirstOrDefault();

                    UNIDAD_ORGANIZACIONAL_RESUMEN unidad_organizacional = (
                        from uor in bdFuncionarios.UNIDAD_ORGANIZACIONAL_RESUMEN
                        join f in bdFuncionarios.FUNCIONARIO on uor.funcionario equals f.funcionario1
                        where f.numero_registro == funcionario_selec.numero_registro
                        select uor
                    ).FirstOrDefault();

                    if (funcionario == null || unidad_organizacional == null)
                    {
                        MessageBox.Show("No existe el funcionario", "Error de obtencion de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        cmbPromotor.Enabled = true;
                        cmbBanca.Enabled = true;
                        cmbDivision.Enabled = true;
                        cmbPlaza.Enabled = true;
                        cmbSucursal.Enabled = true;

                        cmbNumeroFuncionario.Tag = funcionario;
                        cmbPromotor.SelectedValue = funcionario.funcionario1;


                        if (unidad_organizacional.banca != null)
                        {
                            cmbBanca.SelectedValue = unidad_organizacional.banca;
                        }
                        else
                        {
                            cmbBanca.SelectedIndex = 0;
                        }


                        if (unidad_organizacional.division != null)
                        {
                            cmbDivision.SelectedValue = unidad_organizacional.division;                          
                        }
                        else
                        {
                            cmbDivision.SelectedIndex = 0;
                        }


                        if (unidad_organizacional.plaza != null)
                        {
                            cmbSucursal.SelectedValue = unidad_organizacional.plaza;
                            cmbPlaza.SelectedValue = unidad_organizacional.plaza;
                        }
                        else
                        {
                            cmbSucursal.SelectedIndex = 0;
                            cmbPlaza.SelectedIndex = 0;
                        }


                        cmbPromotor.Enabled = false;
                        cmbBanca.Enabled = false;
                        cmbDivision.Enabled = false;
                        cmbSucursal.Enabled = false;
                        cmbPlaza.Enabled = false;

                    }
                }
                catch (Exception ex)
                {
                    Log.Escribe(ex);
                }
            }
        }

        /// <summary>
        /// Se ejecuta cuando se esocge un producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducto_activo)
            {
                if (str_consultor == "")
                {
                    if (cmbProducto.SelectedText == default_cmb)
                    {
                        grpTicket.Enabled = false;
                        grpTipoPersona.Enabled = false;
                    }
                    else
                    {
                        grpTicket.Enabled = true;
                        grpTipoPersona.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Se ejecuta cuando se escoge un tipo de solicitud
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoSolicitud_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbTipoSolicitud_activo)
            {
                TIPO_SOLICITUD tiposolicitud_selec = (TIPO_SOLICITUD)cmbTipoSolicitud.SelectedItem;

                LlenaComboTipoTramite(tiposolicitud_selec);

                if(str_consultor == "")
                {
                    if(tiposolicitud_selec.Descripcion_Solicitud == default_cmb)
                    {
                        cmbTipoTramite.Enabled = false;
                        grpCircuito.Enabled = false;
                        grpTicket.Enabled = false;
                        grpTipoPersona.Enabled = false;
                    }
                    else
                    {
                        cmbTipoTramite.Enabled = true;
                        grpCircuito.Enabled = true;
                    }
                }
            }
            
        }

        /// <summary>
        /// Se ejecuta cuando se escoge un tipo de tramite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbTipoTramite_activo)
            {
                TIPO_TRAMITE tipotramite_selec = (TIPO_TRAMITE)cmbTipoTramite.SelectedItem;

                if (str_consultor == "")
                {
                    if (tipotramite_selec.Descripcion_Tramite == default_cmb)
                    {
                        grpTicket.Enabled = false;
                        grpTipoPersona.Enabled = false;
                    }
                    else
                    {
                        grpTicket.Enabled = true;
                        txtPuntos.Text = tipotramite_selec.Puntos.ToString();
                    }
                }
            }
        }
        /// <summary>
        /// Se ejecuta al presionar boton de cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelarSolicitud_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show($"Esta seguro de querer cancelar la solicitud No. {txtSolicitud.Text}", "Confirmar cancelacion", MessageBoxButtons.YesNo);
            if(dg == DialogResult.Yes)
            {
                //Investigar como ejecutar un update con EntityFramework donde haya retorno de un valor...
               using(var context = new bmtktp01Entities())
               {
                    using(var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        int num_solicitud = Int32.Parse(txtSolicitud.Text);
                        try
                        {                
                            SEGUIMIENTO seguimiento = context.SEGUIMIENTO.Where(w => w.Num_Solicitud == num_solicitud).Select(s => s).FirstOrDefault();

                            if (seguimiento != null)
                            {
                                seguimiento.Status = "3";

                                SEGUIMIENTO_DOCTOS seguimiento_doctos = context.SEGUIMIENTO_DOCTOS.Where(w => w.Num_Solicitud == num_solicitud).Select(s => s).FirstOrDefault();

                                if (seguimiento_doctos != null)
                                {
                                    seguimiento_doctos.Cancelacion = DateTime.Now;

                                    context.SaveChanges();
                                    dbContextTransaction.Commit();

                                    MessageBox.Show("Solicitud No. " + seguimiento.Num_Solicitud + " CANCELADA", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    dtpFechaCancelada.Value = seguimiento_doctos.Cancelacion.Value;
                                    lblStatus.Text = "Cancelada";
                                    btnNuevaObservacion.Enabled = false;

                                    btnFRecepDoc.Visible = false;
                                    btnFAnalisisMac.Visible = false;
                                    btnFFormalizada.Visible = false;
                                    btnFRecepcion.Visible = false;
                                    btnFAtencion.Visible = false;
                                    btnDesbloqueo.Visible = false;
                                    btnEnvio.Visible = false;

                                    btnGuardar.Visible = false;

                                    tmrTraerDatos.Enabled = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Escribe(ex);
                            dbContextTransaction.Rollback();
                            MessageBox.Show("Solicitud No. " + num_solicitud + "no pudo ser CANCELADA", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }

        }


        /// <summary>
        /// se ejecuta al checkear radiobutton de Formalizada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFFormalizada_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFFormalizada.Checked)
            {
                if (dtpFFormalizada.Value != dtpFFormalizada.MinDate)
                {
                    grpCalendario.Tag = 1;
                }
                else
                {
                    DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                    dtpFFormalizada.Value = fecha_servidor;
                    cmbHora3.SelectedValue = fecha_servidor.ToString("hh");
                    cmbMinuto3.SelectedValue = fecha_servidor.ToString("mm");

                }
            }
        }

        /// <summary>
        /// se ejecuta al checkear radiobutton de Recepcion de originales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFRecepcion_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFRecepcion.Checked)
            {
                if (dtpFRecepcion.Value != dtpFRecepcion.MinDate)
                {
                    grpCalendario.Tag = 2;
                }
                else
                {
                    DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                    dtpFRecepcion.Value = fecha_servidor;
                    cmbHora4.SelectedValue = fecha_servidor.ToString("hh");
                    cmbMinuto4.SelectedValue = fecha_servidor.ToString("mm");

                }
            }
        }

        /// <summary>
        /// se ejecuta al checkear radiobutton de Atencion de originales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFAtencion_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFAtencion.Checked)
            {
                if (dtpFAtencion.Value != dtpFAtencion.MinDate)
                {
                    grpCalendario.Tag = 3;
                }
                else
                {
                    DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                    dtpFAtencion.Value = fecha_servidor;
                    cmbHora5.SelectedValue = fecha_servidor.ToString("hh");
                    cmbMinuto5.SelectedValue = fecha_servidor.ToString("mm");

                }
            }

        }

        /// <summary>
        /// se ejecuta al checkear radiobutton de Desbloqueo Sistemas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDesbloqueo_CheckedChanged(object sender, EventArgs e)
        {
            if (btnDesbloqueo.Checked)
            {
                if (dtpDesbloqueo.Value != dtpDesbloqueo.MinDate)
                {
                    grpCalendario.Tag = 4;
                }
                else
                {
                    DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                    dtpDesbloqueo.Value = fecha_servidor;
                }
            }
        }

        /// <summary>
        /// se ejecuta al checkear radiobutton de Envio Agencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnvio_CheckedChanged(object sender, EventArgs e)
        {
            if (btnEnvio.Checked)
            {
                if (dtpEnvio.Value != dtpEnvio.MinDate)
                {
                    grpCalendario.Tag = 5;
                }
                else
                {
                    DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                    dtpEnvio.Value = fecha_servidor;
                }
            }
        }

        /// <summary>
        /// se ejecuta al checkear radiobutton de Expediente unico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFRecepDoc_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFRecepDoc.Checked)
            {
                if (dtpFRecepDoc.Value != dtpFRecepDoc.MinDate)
                {
                    grpCalendario.Tag = 7;
                }
                else
                {
                    DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                    dtpFRecepDoc.Value = fecha_servidor;
                    cmbHora1.SelectedValue = fecha_servidor.ToString("hh");
                    cmbMinuto1.SelectedValue = fecha_servidor.ToString("mm");

                }
            }
        }

        /// <summary>
        /// se ejecuta al checkear radiobutton de Analisis MAC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFAnalisisMac_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFAnalisisMac.Checked)
            {
                if (dtpFAnalisisMac.Value != dtpFAnalisisMac.MinDate)
                {
                    grpCalendario.Tag = 8;
                }
                else
                {
                    DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                    dtpFAnalisisMac.Value = fecha_servidor;
                    cmbHora2.SelectedValue = fecha_servidor.ToString("hh");
                    cmbMinuto2.SelectedValue = fecha_servidor.ToString("mm");

                }
            }
        }

        /// <summary>
        /// Metodo validacion de campos importantes
        /// </summary>
        /// <returns></returns>
        private bool ValidaCampos()
        {
            try
            {
                if (cmbConsultorMac.SelectedIndex <= -1)
                {
                    MessageBox.Show("Favor de Asignar Consultor", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (cmbTipoSolicitud.SelectedIndex <= -1)
                {
                    MessageBox.Show("Favor de seleccionar el Tipo de Solicitud", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (cmbTipoTramite.SelectedIndex <= -1)
                {
                    MessageBox.Show("Favor de seleccionar el Tipo de Tramite", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (txtCuenta.Text == "")
                {
                    MessageBox.Show("Favor de capturar cuenta", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (cmbProducto.SelectedIndex <= -1)
                {
                    MessageBox.Show("Favor de sekeccionar un producto", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (txtNombre.Text == "")
                {
                    MessageBox.Show("Favor de caputrar el Nombre del cliente", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (txtApellidoP.Text == "")
                {
                    MessageBox.Show("Favor de caputrar el primer apellido del cliente", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (txtApellidoM.Text == "")
                {
                    MessageBox.Show("Favor de caputrar el segundo apellido del cliente", "Validacion de campos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return false;
            }
        }

        /// <summary>
        /// Se ejecuta al presionar el boton de guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (str_consultor == "")
            {
                if (cmbConsultorMac.Text == default_cmb)
                {
                    MessageBox.Show("Debe seleccionar su consultor", "Error Solicitud", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
          
            string query = "";
            int i = 0;
            string str_time;
            string str_docuemntos;


            if (ValidaCampos())
            {
                if (btnGuardar.Text == "Guardar")
                {
                    tabControl1.Enabled = false;
                    int consecutivo = Mac_Consecutivo();

                    if (consecutivo <= 0)
                    {
                        MessageBox.Show("No se pudo definir el numero de consecutivo que se le asignara a su solicitud. " + (char)13 + "¡¡¡ No se puede generar la solicitud. !!!",
                            "Error al Guardar",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                            );

                    }
                    else
                    {
                        txtSolicitud.Enabled = true;
                        dtpFechaCaptura.Enabled = true;

                        txtSolicitud.Text = consecutivo.ToString();
                        dtpFechaCaptura.Value = DateTime.Now;

                        dtpEnvio.Enabled = true;
                        btnCancelarSolicitud.Enabled = false;
                        btnGuardar.Enabled = false;

                        int id_ConsultorMac = frmp.usuario_loggeado.id_usuario;
                        int id_Solicitud = cmbTipoSolicitud.SelectedIndex;
                        int id_Tramite = cmbTipoTramite.SelectedIndex;
                        int puntos = Int32.Parse(txtPuntos.Text);
                        string circuito = (rbCircuitoAuto.Checked) ? "A" : "M";
                        string cuenta_Cliente = txtCuenta.Text;
                        string sufijo_Kapiti = cmbProducto.Text;
                        byte tipo_Persona = (byte)((rbPersonaFisica.Checked) ? 0 : 1);
                        string nombre_Cliente = txtNombre.Text;
                        string apellido_Paterno = txtApellidoP.Text;
                        string apellido_Materno = txtApellidoM.Text;
                        decimal deposito_Inicial = decimal.Parse(TxtDepositoTkt.Text);
                        string numero_Registro = cmbNumeroFuncionario.Text;
                        string nombre_Promotor = cmbPromotor.Text;
                        string banca = cmbBanca.Text;
                        string division = cmbDivision.Text;
                        string plaza = cmbPlaza.Text;
                        string sucursal = cmbSucursal.Text;
                        string status = "1";
                        int num_Solicitud = Int32.Parse(txtSolicitud.Text);
                        DateTime fechaRepc_Doc = dtpFRecepDoc.Value;
                        TimeSpan horaRepc_Doc = new TimeSpan(Int32.Parse(cmbHora1.SelectedValue.ToString()), Int32.Parse(cmbMinuto1.SelectedValue.ToString()), 0); 
                        DateTime fechaAnalisis_Mac = dtpFAnalisisMac.Value;
                        TimeSpan horaAnalisis_Mac = new TimeSpan(Int32.Parse(cmbHora2.SelectedValue.ToString()), Int32.Parse(cmbMinuto2.SelectedValue.ToString()), 0);
                        DateTime fechaFormalizada = dtpFFormalizada.Value;
                        TimeSpan horaFormalizada = new TimeSpan(Int32.Parse(cmbHora3.SelectedValue.ToString()), Int32.Parse(cmbMinuto3.SelectedValue.ToString()), 0);
                        DateTime fechaRepc_Originales = dtpFRecepcion.Value;
                        TimeSpan horaRepc_Originales = new TimeSpan(Int32.Parse(cmbHora4.SelectedValue.ToString()), Int32.Parse(cmbMinuto4.SelectedValue.ToString()), 0);
                        DateTime fechaAten_Originales = dtpFAtencion.Value; ;
                        TimeSpan horaAten_Originales = new TimeSpan(Int32.Parse(cmbHora5.SelectedValue.ToString()), Int32.Parse(cmbMinuto5.SelectedValue.ToString()), 0);
                        string originales = "-1";
                        decimal deposito_Inicial_Ini = decimal.Parse(txtDepositoIni.Text);
                        DateTime fecha_Desbloqueo = dtpDesbloqueo.Value;
                        DateTime fecha_Envio = dtpEnvio.Value;
                        DateTime fecha_concluida = dtpConcluir.Value;
                        string existeTKT = rbExisteCuentaSi.Checked ? "S" : "N";


                        FuncionesBdbmtktp01 context = new FuncionesBdbmtktp01();

                        Mac_Inserta_Respuesta respuesta = context.Mac_Inserta_Datos(
                            id_ConsultorMac,
                            id_Solicitud,
                            id_Tramite,
                            puntos,
                            circuito,
                            cuenta_Cliente,
                            sufijo_Kapiti,
                            tipo_Persona,
                            nombre_Cliente,
                            apellido_Paterno,
                            apellido_Materno,
                            deposito_Inicial,
                            numero_Registro,
                            nombre_Promotor,
                            banca,
                            division,
                            plaza,
                            sucursal,
                            status,
                            num_Solicitud,
                            fechaRepc_Doc,
                            horaRepc_Doc,
                            fechaAnalisis_Mac,
                            horaAnalisis_Mac,
                            fechaFormalizada,
                            horaFormalizada,
                            fechaRepc_Originales,
                            horaRepc_Originales,
                            fechaAten_Originales,
                            horaAten_Originales,
                            originales,
                            deposito_Inicial_Ini,
                            fecha_Desbloqueo,
                            fecha_Envio,
                            fecha_concluida,
                            existeTKT
                        );

                        dtpEnvio.Enabled = false;

                        if(respuesta.Codigo == 0)
                        {
                            if(cmbTipoSolicitud.SelectedText != default_cmb)
                            {
                                lblDeposito.Visible = false;
                                txtDepositoIni.Visible = false;
                            }
                            else
                            {

                                lblDeposito.Visible = true;
                                txtDepositoIni.Visible = true;
                            }

                            if(rbCircuitoAuto.Checked)
                            {
                                btnDesbloqueo.Visible = true;
                                LblDesbloquep.Visible = true;
                                dtpDesbloqueo.Visible = true;
                            }
                            else
                            {
                                btnDesbloqueo.Visible = false;
                                LblDesbloquep.Visible = false;
                                dtpDesbloqueo.Visible = false;
                            }

                            dtpFechaCaptura.Value = respuesta.FechaHora_Captura;
                            txtDepositoIni.Text = "0.00";


                            txtSolicitud.Enabled = true;

                            foreach (DataGridViewRow fila in dtgvwObservaciones.Rows)
                            {
                                int insertado = 0;
                                OBSERVACIONES observacion = new OBSERVACIONES { Num_Solicitud = Int32.Parse(txtSolicitud.Text), Observaciones1 = fila.Cells[""].Value.ToString(), Fecha_Observ = DateTime.Now };

                                bdbmtktp01.OBSERVACIONES.Add(observacion);

                                insertado = bdbmtktp01.SaveChanges();
                            }

                            txtSolicitud.Enabled = false;
                            lblStatus.Text = "En proceso";

                            MessageBox.Show("El Número de folio es: " + txtSolicitud.Text, "Numero de folio", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            DialogResult dialogResult = MessageBox.Show("¿Desea dar de alta otra solicitud?", "Nueva Solicitud", MessageBoxButtons.YesNo);

                            if (dialogResult == DialogResult.Yes)
                            {
                                Limpiar();


                                btnLimpiar.Text = "Nuevo";

                                cmbConsultorMac.Enabled = false;
                                cmbTipoSolicitud.Enabled = false;
                                cmbTipoTramite.Enabled = false;
                                txtCuenta.Enabled = false;
                                cmbProducto.Enabled = false;
                                txtNombre.Enabled = false;
                                txtApellidoP.Enabled = false;
                                txtApellidoM.Enabled = false;
                                TxtDepositoTkt.Enabled = false;
                                rbcorrectos.Checked = false;
                                rbIncorrectos.Checked = false;

                                //DoEvents
                                //tmrTraerDatos.Enabled = True

                                btnCancelarSolicitud.Enabled = true;

                            }
                            else
                            {
                                //SSTabSeg.Enabled = True
                                //SSTabSeg.TabEnabled(1) = True
                                //SSTabSeg.TabEnabled(2) = True
                                btnGuardar.Text = "Modificar";
                                grpOriginales.Enabled = false;
                                btnGuardar.Enabled = true;
                                lblStatus.Text = "En proceso";


                                if(lblStatus.Text == "En proceso")
                                {
                                    grpCircuito.Enabled = true;

                                    HabilitarRbFechas(true);
                                    HabilitarCmbsTiempo(true);

                                    VisibleRbFechas(true);
                                    lblDeposito.Visible = true;
                                    txtDepositoIni.Visible = true;

                                    txtDepositoIni.Enabled = true;

                                    txtApellidoP.Enabled = true;
                                    txtNombre.Enabled = true;
                                    txtApellidoM.Enabled = true;

                                    if(LblDesbloquep.Visible == true)
                                    {
                                        btnDesbloqueo.Visible = true;
                                        btnDesbloqueo.Enabled = true;
                                    }
                                    else
                                    {
                                        btnDesbloqueo.Visible = false;
                                        btnDesbloqueo.Enabled = false;
                                    }

                                    grpOriginales.Enabled = true;
                                    rbcorrectos.Enabled = true;
                                    rbIncorrectos.Enabled = true;

                                    btnConcluirSolicitud.Enabled = true;

                                    lblDeposito.Visible = true;
                                    txtDepositoIni.Visible = true;
                                    txtDepositoIni.Enabled = true;
                                    txtApellidoP.Enabled = true;
                                    txtNombre.Enabled = true;
                                    txtApellidoM.Enabled = true;
                                }
                                else
                                {
                                    grpCircuito.Enabled = false;
                                    btnLimpiar.Visible = false;

                                    HabilitarRbFechas(false);
                                    HabilitarCmbsTiempo(false);

                                    btnDesbloqueo.Visible = false;
                                    btnDesbloqueo.Enabled = false;
                                }
                            }


                        }
                    }
                }
            }          
        }

        private void Limpiar()
        {
            cmbNumeroFuncionario_activo = false;
            cmbConsultorMac_activo = false;
            cmbProducto_activo = false;
            cmbTipoSolicitud_activo = false; 
            cmbTipoTramite_activo = false;

            cmbConsultorMac.SelectedText = default_cmb;
            cmbTipoSolicitud.SelectedText = default_cmb;
            cmbTipoTramite.SelectedText = default_cmb;

            cmbNumeroFuncionario.Tag = String.Empty;
            txtSolicitud.Text = String.Empty;
            dtpFechaCaptura.Value = default_dtp;

            txtPuntos.Text = String.Empty;
            rbCircuitoAuto.Checked = true;
            txtCuenta.Text = String.Empty;
            cmbProducto.SelectedText = default_cmb;
            txtNombre.Text = String.Empty;
            txtApellidoP.Text = String.Empty;
            txtApellidoM.Text = String.Empty;
            TxtDepositoTkt.Text = String.Empty;
            cmbNumeroFuncionario.SelectedText = default_cmb;
            cmbPromotor.SelectedText = String.Empty;
            cmbBanca.SelectedText = String.Empty;
            cmbDivision.SelectedText = String.Empty;
            cmbPlaza.SelectedText = String.Empty;
            lblStatus.Text = String.Empty;         
            cmbHora1.SelectedText = "00";
            cmbHora2.SelectedText = "00";
            cmbHora3.SelectedText = "00";
            cmbHora4.SelectedText = "00";
            cmbHora5.SelectedText = "00";
            cmbMinuto1.SelectedText = "00";
            cmbMinuto2.SelectedText = "00";
            cmbMinuto3.SelectedText = "00";
            cmbMinuto4.SelectedText = "00";
            cmbMinuto5.SelectedText = "00";
            dtpFFormalizada.Value = default_dtp;
            dtpFRecepDoc.Value = default_dtp;
            dtpFAnalisisMac.Value = default_dtp;
            dtpFRecepcion.Value = default_dtp;
            dtpFAtencion.Value = default_dtp;
            dtpDesbloqueo.Value = default_dtp;
            dtpEnvio.Value = default_dtp;
            dtpConcluir.Value = default_dtp;

            LlenarFechaNumero();


            //TxtNivelTiempo.text = ""
            //TxtNivelDias.text = ""

            txtDepositoIni.Text = String.Empty;
            dtgvwObservaciones.DataSource = null;
            dtgvwObservaciones.Refresh();
            cmbConsultorMac.Enabled = true;
            cmbTipoSolicitud.Enabled = true;
            btnGuardar.Text = "Guardar";
            btnCancelarSolicitud.Visible = false;

            btnGuardar.Visible = true;
            grpCircuito.Enabled = true;
            cmbTipoTramite.Enabled = true;
            grpTicket.Enabled = true;
            grpTipoPersona.Enabled = true;

            //If tmrTab.Tag = "" Then
            //    SSTabSeg.Tab = 0
            //Else
            //    tmrTab.Tag = ""
            //End If

            rbExisteCuentaSi.Checked = false;
            rbExisteCuentaNo.Checked = false;

            cmbNumeroFuncionario.Enabled = true;
            txtCuenta.Enabled = false;
            btnNuevaObservacion.Enabled = true;
            btnVerObservacion.Enabled = true;
            txtDepositoIni.Enabled = true;
            grpTipoPersona.Enabled = true;

        }

        private void dtpConcluir_ValueChanged(object sender, EventArgs e)
        {
            if (dtpConcluir.Value != dtpConcluir.MinDate)
            {
                grpCalendario.Tag = 6;
            }
            else
            {
                DateTime fecha_servidor = bdbmtktp01.Mac_Obtiene_FechaServidor().FirstOrDefault().Value;

                dtpConcluir.Value = fecha_servidor;
                cmbHora1.SelectedValue = fecha_servidor.ToString("hh");
                cmbMinuto1.SelectedValue = fecha_servidor.ToString("mm");

            }
        }


        public FrmNueva_Solicitud(Frm_Login frmp)
        {      
            InitializeComponent();
          
            this.frmp = frmp;
            this.bdbmtktp01 = new bmtktp01Entities();
            this.bdCatalogos = new CATALOGOSEntities();
            this.bdFuncionarios = new FUNCIONARIOSEntities();

            txtNombre.Text = "Nombre";
            txtApellidoP.Text = "Primer Apellido";
            txtApellidoM.Text = "Segundo Apellido";
        }

        private void Frm_NuevaSolicitud_Load(object sender, EventArgs e)
        {
            //***Definiendo T<ooltips ************************************************************
            ToolTip tt1 = new ToolTip();

            tt1.AutoPopDelay = 5000;
            tt1.InitialDelay = 1000;
            tt1.ReshowDelay = 500;
            tt1.ShowAlways = true;

            tt1.SetToolTip(this.cmbNumeroFuncionario, "Presiona ENTER para efectuar busqueda con el numero de funcionario");

            FuncionesBdbmtktp01.fecha_default = DateTimePicker.MinimumDateTime;


            //***PLACE HOLDERS********************************************************************
            txtNombre.GotFocus += new EventHandler(this.TxtNombreGotFocus);
            txtNombre.LostFocus += new EventHandler(this.TxtNombreLostFocus);

            txtApellidoP.GotFocus += new EventHandler(this.TxtApelllidoPGotFocus);
            txtApellidoP.LostFocus += new EventHandler(this.TxtApellidoPLostFocus);

            txtApellidoM.GotFocus += new EventHandler(this.TxtApelllidoMGotFocus);
            txtApellidoM.LostFocus += new EventHandler(this.TxtApellidoMLostFocus);
            //************************************************************************************

            if (str_consultor.Trim() != "")
            {
                CargaConsulta();

                if (lblStatus.Text == "Nueva")
                {
                    MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                CargarcomboHora(cmbHora1);
                CargarcomboHora(cmbHora2);
                CargarcomboHora(cmbHora3);
                CargarcomboHora(cmbHora4);
                CargarcomboHora(cmbHora5);


                CargarcomboMinuto(cmbMinuto1);
                CargarcomboMinuto(cmbMinuto2);
                CargarcomboMinuto(cmbMinuto3);
                CargarcomboMinuto(cmbMinuto4);
                CargarcomboMinuto(cmbMinuto5);


                LlenarFechaNumero();

                funcionarios = (
                  from f in bdFuncionarios.FUNCIONARIO
                  join uor in bdFuncionarios.UNIDAD_ORGANIZACIONAL_RESUMEN on f.funcionario1 equals uor.funcionario
                  select f
                ).ToList();

                funcionarios.Insert(0, new FUNCIONARIO
                {
                    funcionario1 = -1,
                    nombre_funcionario = ".",
                    apellido_paterno = ".",
                    apellido_materno = ".",
                    numero_funcionario = default_cmb
                });

                uors = (
                    from uor in bdFuncionarios.UNIDAD_ORGANIZACIONAL_RESUMEN
                    join f in bdFuncionarios.FUNCIONARIO on uor.funcionario equals f.funcionario1
                    select uor
                ).ToList();

                uors.Insert(0, new UNIDAD_ORGANIZACIONAL_RESUMEN { banca = ". . .  ", plaza = ". . .  ", division = ". . .  ", sucursal = ". . .  " });

                LlenaComboNumFunc();
                LlenaComboNombreFunc();

                LlenaComboBanca();
                LlenaComboplaza();
                LlenarComboDivision();
                LlenaComboSucursal();
                LlenarComboProducto();


                LlenaComboTipoSolicitud();
                LlenaComboTipoTramite();
                LlenarComboConsultor();

                dtpFRecepDoc.Value = dtpFRecepDoc.MinDate;
                dtpFAnalisisMac.Value = dtpFAnalisisMac.MinDate;
                dtpFFormalizada.Value = dtpFFormalizada.MinDate;
                dtpFRecepcion.Value = dtpFRecepcion.MinDate;
                dtpFAtencion.Value = dtpFAtencion.MinDate;

                cmbProducto.Enabled = false;
                cmbBanca.Enabled = false;
                cmbDivision.Enabled = false;
                cmbPlaza.Enabled = false;
                cmbSucursal.Enabled = false;
                lblStatus.Text = "Nueva";
                this.Text = "Captura de solicitud";

                if (btnLimpiar.Text == "Limpiar")
                {
                    btnLimpiar.Enabled = false;
                }


                if (lblStatus.Text == "En proceso")
                {
                    btnFRecepDoc.Visible = true;
                    btnFRecepDoc.Enabled = true;

                    btnFAnalisisMac.Visible = true;
                    btnFAnalisisMac.Enabled = true;

                    btnFFormalizada.Visible = true;
                    btnFFormalizada.Enabled = true;

                    btnFRecepcion.Visible = true;
                    btnFRecepcion.Enabled = true;

                    btnFAtencion.Visible = true;
                    btnFAtencion.Enabled = true;

                }

            }


            if(frmp.activa == 1)
            {
                btnLimpiar.Visible = false;
                btnGuardar.Visible = true;
                btnCancelarSolicitud.Visible = true;

                btnVerObservacion.Visible = true;
                btnLimpiar.Visible = true;
                btnVerObservacion.Enabled = true;
                btnLimpiar.Enabled = true;
                btnGuardar.Enabled = true;
                btnCancelarSolicitud.Enabled = true;
                btnLimpiar.Visible = false;

                if (btnLimpiar.Text == "Limpiar")
                {
                    btnLimpiar.Enabled = false;
                }
            }
            else
            {
                btnLimpiar.Visible = false;
                btnGuardar.Visible = false;
                btnCancelarSolicitud.Visible = false;

                btnVerObservacion.Visible = false;
                btnLimpiar.Visible = false;
                btnVerObservacion.Enabled = false;
                btnLimpiar.Enabled = false;
                btnGuardar.Enabled = false;
                btnCancelarSolicitud.Enabled = false;

            }

            if (str_consultor != "")
            {
                tmrTraerDatos.Enabled = false;

                txtSolicitud.Text = num_solicitud.ToString();
                CargaConsulta();



                tmtValidarBoton.Enabled = true;
                switch (lblStatus.Text)
                {
                    case "En Proceso":
                        dtgvwObservaciones.Enabled = true;
                        btnCancelarSolicitud.Visible = true;
                        btnGuardar.Text = "Modificar";
                        btnGuardar.Visible = true;

                        btnLimpiar.Visible = false;
                        btnLimpiar.Enabled = false;
                        btnVerObservacion.Enabled = true;
                        break;
                    case "Concluida":
                        dtgvwObservaciones.Enabled = true;
                        btnGuardar.Visible = false;
                        btnCancelarSolicitud.Visible = false;
                        cmbTipoSolicitud.Enabled = false;
                        btnConcluirSolicitud.Enabled = false;
                        btnLimpiar.Visible = false;
                        btnLimpiar.Enabled = false;
                        cmbProducto.Enabled = false;
                        grpTipoPersona.Enabled = false;
                        btnVerObservacion.Enabled = false;
                        btnGuardar.Enabled = false;
                        btnCancelarSolicitud.Visible = false;
                        btnGuardar.Visible = false;
                        btnCancelarSolicitud.Visible = false;
                        break;
                    case "Cancelada":
                        lblStatus.Text = "Cancelada";
                        btnGuardar.Visible = false;
                        btnCancelarSolicitud.Visible = false;
                        cmbTipoSolicitud.Enabled = false;
                        btnConcluirSolicitud.Enabled = false;
                        btnLimpiar.Visible = false;
                        cmbProducto.Enabled = true;
                        btnVerObservacion.Enabled = true;
                        grpTipoPersona.Enabled = false;
                        dtgvwObservaciones.Enabled = true;
                        btnGuardar.Enabled = false;
                        btnCancelarSolicitud.Enabled = false;
                        btnGuardar.Visible = false;
                        btnCancelarSolicitud.Visible = false;
                        break;
                    default:

                        break;
                }
            }

            dtpFechaCaptura.Enabled = false;
            txtPuntos.Enabled = false;
            dtpFechaCancelada.Enabled = false;
        }

        private void LlenarFechaNumero()
        {
            try
            {
                if(str_consultor == "" )
                {
                    int consecutivo = Mac_Consecutivo();

                    if(consecutivo == -1)
                    {
                        MessageBox.Show("No se pudo obtener el consecutivo para la solicitud", "Error de obtencion consecutivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                
                    else
                    {
                        dtpFechaCaptura.Enabled = false;
                        dtpFechaCaptura.Value = DateTime.Now;
                        txtSolicitud.Enabled = false;
                        txtSolicitud.Text = consecutivo.ToString();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void Frm_NuevaSolicitud_Activated(object sender, EventArgs e)
        {
            if(str_consultor.Trim() == "")
            {
                this.Text = "Captura de Solicitudes Sistema GO-MAC";
            }
            else
            {
                this.Text = "Consulta de Solicitudes Sistema GO-MAC";

                switch (lblStatus.Text)
                {
                    case "Nueva":
                        CargaConsulta();
                        break;
                   
                    default:
                        MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                        break;
                }
            }

            if(frmp.activa == 1)
            {
                btnLimpiar.Visible = false;
                btnGuardar.Visible = true;
                btnCancelarSolicitud.Visible = true;

                btnNuevaObservacion.Visible = true;
                btnVerObservacion.Visible = true;

                btnLimpiar.Enabled = false;
                btnGuardar.Enabled = true;
                btnCancelarSolicitud.Enabled = true;

                if(btnLimpiar.Text == "Limpiar")
                {
                    btnLimpiar.Enabled = false;
                }

            }
            else
            {
                btnLimpiar.Visible = false;
                btnGuardar.Visible = false;
                btnCancelarSolicitud.Visible = false;

                btnNuevaObservacion.Visible = false;
                btnVerObservacion.Visible = false;
                btnLimpiar.Enabled = false;
                btnGuardar.Enabled = false;
                btnCancelarSolicitud.Enabled = false;

                btnNuevaObservacion.Enabled = false;
                btnVerObservacion.Enabled = false;
            }


            switch (lblStatus.Text)
            {
                case "En Proceso":
                    dtgvwObservaciones.Enabled = true;
                    btnCancelarSolicitud.Visible = true;
                    btnGuardar.Text = "Modificar";
                    btnGuardar.Visible = true;

                    btnLimpiar.Visible = false;
                    btnLimpiar.Enabled = false;
                    btnVerObservacion.Enabled = true;
                break;
                case "Concluida":
                    dtgvwObservaciones.Enabled = true;
                    btnGuardar.Visible = false;
                    btnCancelarSolicitud.Visible = false;
                    cmbTipoSolicitud.Enabled = false;
                    btnConcluirSolicitud.Enabled = false;
                    btnLimpiar.Visible = false;
                    btnLimpiar.Enabled = false;
                    cmbProducto.Enabled = false;
                    grpTipoPersona.Enabled = false;
                    btnVerObservacion.Enabled = true;
                    btnGuardar.Enabled = false;
                    btnCancelarSolicitud.Enabled = false;
                    btnGuardar.Visible = false;
                    btnCancelarSolicitud.Visible = false;
                break;
                case "Cancelada":
                    btnGuardar.Visible = false;
                    btnCancelarSolicitud.Visible = false;
                    cmbTipoSolicitud.Enabled = false;
                    btnConcluirSolicitud.Enabled = false;
                    btnLimpiar.Visible = false;
                    cmbProducto.Enabled = true;
                    btnVerObservacion.Enabled = true;
                    grpTipoPersona.Enabled = false;
                    dtgvwObservaciones.Enabled = true;
                    btnGuardar.Enabled = false;
                    btnCancelarSolicitud.Enabled = false;
                break;
            }

            if(str_consultor != "")
            {
                if(lblStatus.Text == "Nueva")
                {
                    MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }



        }

        private void CargarcomboMinuto(ComboBox cmbMinuto)
        {
            List<Map> minutos = new List<Map>();

            for (int i = 0; i <= 60; i++)
            {
                minutos.Add(new Map
                {
                    Key = i.ToString("00"),
                    Value = i.ToString("00")
                });
            }
            
            cmbMinuto.DataSource = minutos;
            cmbMinuto.DisplayMember = "Key";
            cmbMinuto.ValueMember = "Value";
        }

        private void CargarcomboHora(ComboBox cmbHora)
        {
            List<Map> horas = new List<Map>();

            for(int i = 0; i <= 24; i++)
            {
                horas.Add(new Map
                {
                    Key  = i.ToString("00"),
                    Value = i.ToString("00")
                });
            }

            cmbHora.DataSource = horas;
            cmbHora.DisplayMember = "Key";
            cmbHora.ValueMember = "Value";
            
        }

      

        private void LlenaComboNumFunc()
        {
            try
            {
                funcionarios = (
                from f in funcionarios
                orderby f.numero_funcionario
                select f
            ).ToList();

                if (funcionarios != null)
                {
                    cmbNumeroFuncionario.DataSource = funcionarios;
                    cmbNumeroFuncionario.ValueMember = "numero_registro";
                    cmbNumeroFuncionario.DisplayMember = "numero_funcionario";
                }  
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaComboNombreFunc()
        {
            try
            {
                funcionarios = (from f in funcionarios orderby f.nombre_funcionario, f.apellido_paterno, f.apellido_materno select f).ToList();

                if (funcionarios != null)
                {
                    cmbPromotor.DataSource = funcionarios;
                    cmbPromotor.ValueMember = "funcionario1";
                    cmbPromotor.DisplayMember = "_nombreCompleto";
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaComboplaza()
        {
            try
            {
                uors = (from uor in uors where uor.plaza != null orderby uor.plaza select uor).ToList();

                if (uors != null)
                {
                                  
                    cmbPlaza.DataSource = uors;
                    cmbPlaza.ValueMember = "plaza";
                    cmbPlaza.DisplayMember = "plaza";
                }
            }
            catch(Exception ex)
            {
                Log.Escribe(ex);
            }
        }


        private void LlenaComboSucursal()
        {
            try
            {
                uors = (from uor in uors where uor.plaza != null orderby uor.plaza select uor).ToList();

                if (uors != null)
                {
               
                    cmbSucursal.DataSource = uors;
                    cmbSucursal.ValueMember = "plaza";
                    cmbSucursal.DisplayMember = "plaza";
                }    
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenarComboProducto()
        {
            try
            {
                List<PRODUCTOS> productos = (from p in bdCatalogos.PRODUCTOS orderby p.Producto select p).ToList();

                if (productos != null)
                {
                    productos.Insert(0, new PRODUCTOS { Producto = ". . .  "});

                    cmbProducto.DataSource = productos;
                    cmbProducto.ValueMember = "Id_Producto";
                    cmbProducto.DisplayMember = "Producto";
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenarComboDivision()
        {
            try
            {
                try
                {
                    uors = (from uor in uors where uor.division != null orderby uor.division select uor).ToList();

                    if (uors != null)
                    {
                                           
                        cmbDivision.DataSource = uors;
                        cmbDivision.ValueMember = "division";
                        cmbDivision.DisplayMember = "division";
                    }              
                }
                catch (Exception ex)
                {
                    Log.Escribe(ex);
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaComboBanca()
        {
            try
            {
                uors = (from uor in uors where uor.banca != null orderby uor.banca select uor).ToList();

                if (uors != null)
                {
                                   
                    cmbBanca.DataSource = uors;
                    cmbBanca.ValueMember = "banca";
                    cmbBanca.DisplayMember = "banca";
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }


        private void LlenarComboConsultor()
        {
            //[NOTA]Corregir esta funcion y hacerlo como esta en el codigo viejo
            try
            {
                List<CONSULTORES> consultores =
                    (from c in bdbmtktp01.CONSULTORES orderby c.Iniciales_ConsultorMac ascending select c).ToList();

                if (consultores != null)
                {
                    consultores.Insert(0, new CONSULTORES { Iniciales_ConsultorMac = default_cmb });
                    cmbConsultorMac.DataSource = consultores;
                    cmbConsultorMac.ValueMember = "Id_ConsultorMac";
                    cmbConsultorMac.DisplayMember = "Iniciales_ConsultorMac";
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaComboTipoTramite(TIPO_SOLICITUD ts = default)
        {
            try
            {
                List<TIPO_TRAMITE> tipos_tramite =
                    (from tt in bdbmtktp01.TIPO_TRAMITE orderby tt.Descripcion_Tramite ascending select tt).ToList();

                if(ts != default)
                {
                    tipos_tramite = tipos_tramite
                        .Where(w => w.Descripcion_Tramite.ToUpper().Contains(ts.Descripcion_Solicitud.ToUpper().Substring(0,5).Trim()))
                        .Select(s => s)
                        .ToList();
                }

                if (tipos_tramite != null)
                {
                    tipos_tramite.Insert(0, new TIPO_TRAMITE { Descripcion_Tramite = default_cmb });
                    cmbTipoTramite.DataSource = tipos_tramite;
                    cmbTipoTramite.ValueMember = "Id_Tramite";
                    cmbTipoTramite.DisplayMember = "Descripcion_Tramite";
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaComboTipoSolicitud()
        {
            try
            {
                List<TIPO_SOLICITUD> tipos_solicitud = 
                    (from ts in bdbmtktp01.TIPO_SOLICITUD orderby ts.Descripcion_Solicitud ascending select ts).ToList();

                if(tipos_solicitud != null)
                {
                    tipos_solicitud.Insert(0, new TIPO_SOLICITUD { Descripcion_Solicitud = default_cmb });
                    cmbTipoSolicitud.DataSource = tipos_solicitud;
                    cmbTipoSolicitud.ValueMember = "Id_Solicitud";
                    cmbTipoSolicitud.DisplayMember = "Descripcion_Solicitud";
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void CargaConsulta()
        {
            try
            {
                if(str_consultor.Trim() != "")
                {
                    se_carga = 0;
                    tmrTraerDatos.Enabled = false;

                    txtSolicitud.Text = num_solicitud.ToString();
                    TraerDatos();
                }
               
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
            throw new NotImplementedException();
        }

        public void TxtNombreLostFocus(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                txtNombre.Text = "Nombre";
                txtNombre.ForeColor = Color.LightGray;
            }
        }

        public void TxtNombreGotFocus(object sender, EventArgs e)
        {
            if(txtNombre.Text == "Nombre")
            {
                txtNombre.Text = "";
                txtNombre.ForeColor = Color.Black;
            }
           
        }


        public void TxtApellidoPLostFocus(object sender, EventArgs e)
        {
            if (txtApellidoP.Text == "")
            {
                txtApellidoP.Text = "Primer Apellido";
                txtApellidoP.ForeColor = Color.LightGray;
            }
        }

        public void TxtApelllidoPGotFocus(object sender, EventArgs e)
        {
            if (txtApellidoP.Text == "Primer Apellido")
            {
                txtApellidoP.Text = "";
                txtApellidoP.ForeColor = Color.Black;
            }

        }


        public void TxtApellidoMLostFocus(object sender, EventArgs e)
        {
            if (txtApellidoM.Text == "")
            {
                txtApellidoM.Text = "Segundo Apellido";
                txtApellidoM.ForeColor = Color.LightGray;
            }
        }

        public void TxtApelllidoMGotFocus(object sender, EventArgs e)
        {
            if (txtApellidoM.Text == "Segundo Apellido")
            {
                txtApellidoM.Text = "";
                txtApellidoM.ForeColor = Color.Black;
            }

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tmrTraerDatos_Tick(object sender, EventArgs e)
        {
            TraerDatos();
        }

        private void TraerDatos()
        {
            try
            {
                this.Text = (str_consultor.Trim() == "") ? "Captura de solicitud" : "Consulta de solicitud";

                if (VerSeguimientoDoc(Int32.Parse(txtSolicitud.Text)) != null)
                {

                    if (seguimiento_doc.Status != "1")
                    {
                        btnGuardar.Visible = false;
                        btnCancelarSolicitud.Visible = false;
                        cmbConsultorMac.Enabled = false;
                        grpCircuito.Enabled = false;
                        grpTicket.Enabled = false;
                        TxtDepositoTkt.Enabled = true;
                        cmbNumeroFuncionario.Enabled = false;
                        cmbPromotor.Enabled = false;
                        cmbBanca.Enabled = false;
                        cmbDivision.Enabled = false;
                        cmbPlaza.Enabled = false;
                        cmbSucursal.Enabled = false;
                        dtpFRecepcion.Enabled = false;
                        dtpFAnalisisMac.Enabled = false;
                        dtpFRecepcion.Enabled = false;
                        txtDepositoIni.Enabled = false;
                        dtpDesbloqueo.Enabled = false;
                        dtpEnvio.Enabled = false;
                        grpOriginales.Enabled = false;
                        btnNuevaObservacion.Enabled = false;

                        VisibleBtnsFecha(false);
                        HabilitarCmbsTiempo(false);
                    }

                    //Datos Cuenta
                    dtpFechaCaptura.Value = (seguimiento_doc.Fecha_Captura.HasValue) ? seguimiento_doc.Fecha_Captura.Value : dtpFechaCaptura.MinDate;

                    cmbConsultorMac.SelectedIndex = (seguimiento_doc.Id_ConsultorMac.HasValue) ? seguimiento_doc.Id_ConsultorMac.Value : -1;

                    cmbTipoSolicitud.SelectedIndex = (seguimiento_doc.Id_Solicitud.HasValue) ? seguimiento_doc.Id_Solicitud.Value : -1;


                    if (seguimiento_doc.ExisteTKT.ToUpper() == "S")
                    {
                        rbExisteCuentaSi.Checked = true;
                    }
                    else
                    {
                        rbExisteCuentaNo.Checked = true;
                    }

                    switch (Int32.Parse(seguimiento_doc.Status))
                    {
                        case 1:
                            dtgvwObservaciones.Enabled = true;
                            lblStatus.Text = "En proceso";
                            btnCancelarSolicitud.Visible = true;
                            btnGuardar.Text = "Modificar";
                            btnGuardar.Visible = true;

                            btnNuevaObservacion.Visible = false;

                            cmbTipoSolicitud.Enabled = false;
                            txtCuenta.Enabled = false;
                            cmbProducto.Enabled = false;
                            txtNombre.Enabled = false;
                            txtApellidoP.Enabled = false;
                            txtApellidoM.Enabled = false;
                            TxtDepositoTkt.Enabled = true;
                            grpTipoPersona.Enabled = false;

                            if (dtpFRecepDoc.Text == "")
                            {
                                btnFRecepDoc.Visible = true;
                                btnFRecepDoc.Enabled = true;
                            }

                            if (dtpFAnalisisMac.Text == "")
                            {
                                btnFAnalisisMac.Visible = true;
                                btnFAnalisisMac.Enabled = true;

                            }

                            if (dtpFFormalizada.Text == "")
                            {
                                btnFFormalizada.Visible = true;
                                btnFFormalizada.Enabled = true;
                            }

                            if (dtpFRecepcion.Text == "")
                            {
                                btnFRecepcion.Visible = true;
                                btnFRecepcion.Enabled = true;
                            }

                            if (dtpFAtencion.Text == "")
                            {
                                btnFAtencion.Visible = true;
                                btnFAtencion.Enabled = true;
                            }


                            if (dtpDesbloqueo.Text == "")
                            {
                                btnDesbloqueo.Visible = true;
                                btnDesbloqueo.Enabled = true;
                            }

                            if (dtpEnvio.Text == "")
                            {
                                btnEnvio.Visible = true;
                                btnEnvio.Enabled = true;
                            }

                            btnVerObservacion.Enabled = true;
                            break;

                        case 2:
                            dtgvwObservaciones.Enabled = true;
                            lblStatus.Text = "Concluida";
                            btnGuardar.Visible = false;
                            btnCancelarSolicitud.Visible = false;
                            cmbTipoSolicitud.Enabled = false;
                            btnConcluirSolicitud.Enabled = false;
                            btnNuevaObservacion.Visible = false;
                            btnNuevaObservacion.Enabled = false;
                            cmbProducto.Enabled = false;
                            grpTipoPersona.Enabled = false;
                            btnVerObservacion.Enabled = true;
                            break;

                        case 3:
                            lblStatus.Text = "Cancelada";
                            btnGuardar.Visible = false;
                            btnCancelarSolicitud.Visible = false;
                            cmbTipoSolicitud.Enabled = false;
                            btnConcluirSolicitud.Enabled = false;
                            cmbProducto.Enabled = false;
                            btnVerObservacion.Enabled = true;
                            grpTipoPersona.Enabled = false;
                            dtgvwObservaciones.Enabled = true;
                            break;
                        default:
                            break;
                    }

                    txtPuntos.Text = (seguimiento_doc.Puntos.HasValue) ? seguimiento_doc.Puntos.Value.ToString() : "";

                    if (seguimiento_doc.Circuito == "A")
                    {
                        if (rbCircuitoAuto.Enabled)
                        {
                            rbCircuitoAuto.Checked = true;
                            rbCircuitoManual.Checked = false;
                        }
                    }
                    else
                    {
                        if (rbCircuitoManual.Enabled)
                        {
                            rbCircuitoAuto.Checked = false;
                            rbCircuitoManual.Checked = true;
                        }
                    }

                    grpCircuito.Enabled = false;
                    grpTicket.Enabled = false;

                    txtCuenta.Tag = 1;
                    txtCuenta.Text = seguimiento_doc.Cuenta_Cliente;

                    cmbProducto.SelectedText = seguimiento_doc.Sufijo_Kapiti;


                    if (seguimiento_doc.Tipo_Persona == 0)
                    {
                        //PERSONA FISICA
                        if (rbPersonaFisica.Enabled == true)
                        {
                            rbPersonaFisica.Checked = true;
                            rbPersonaMoral.Checked = false;
                        }
                    }
                    else
                    {
                        //PERSONA MORAL
                        if (rbPersonaMoral.Enabled == true)
                        {
                            rbPersonaFisica.Checked = false;
                            rbPersonaMoral.Checked = true;
                        }
                    }

                    txtNombre.Text = seguimiento_doc.Nombre_Cliente;
                    txtApellidoP.Text = seguimiento_doc.Apellido_Paterno;
                    txtApellidoM.Text = seguimiento_doc.Apellido_Materno;

                    TxtDepositoTkt.Text = seguimiento_doc.Deposito_InicialTKT.ToString();
                    TxtDepositoTkt.Tag = seguimiento_doc.Deposito_InicialTKT.ToString();


                    //Datos Funcionario
                    cmbNumeroFuncionario.SelectedText = seguimiento_doc.Numero_Registro;
                    cmbNumeroFuncionario.Tag = seguimiento_doc.Numero_Registro;
                    cmbPromotor.Text = seguimiento_doc.Nombre_Promotor;
                    cmbPromotor.Enabled = false;
                    cmbBanca.SelectedText = seguimiento_doc.Banca;
                    cmbBanca.Enabled = false;
                    cmbDivision.SelectedText = seguimiento_doc.Division;
                    cmbDivision.Enabled = false;
                    cmbPlaza.SelectedText = seguimiento_doc.Plaza;
                    cmbPlaza.Enabled = false;
                    cmbSucursal.SelectedText = seguimiento_doc.Sucursal;
                    cmbSucursal.Enabled = false;



                    //Dato Seguimiento Documento
                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.HasValue)
                    {
                        if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value == DateTime.Parse("01-01-1900"))
                        {
                            dtpFRecepDoc.Text = "";
                            cmbHora1.SelectedText = "00";
                            cmbMinuto1.SelectedText = "00";

                            grpOriginales.Enabled = true;

                            VisibleBtnsFecha(false);
                        }
                        else
                        {
                            dtpFRecepDoc.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value;
                            cmbHora1.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value.ToString("hh");
                            cmbMinuto1.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value.ToString("mm");

                            grpOriginales.Enabled = false;

                            btnFRecepDoc.Visible = false;
                            btnFAtencion.Visible = false;
                            btnEnvio.Visible = false;


                            if (lblStatus.Text == "En proceso")
                            {
                                btnFAnalisisMac.Visible = false;
                                btnDesbloqueo.Visible = false;

                            }
                        }

                        cmbHora1.Enabled = false;
                        cmbMinuto1.Enabled = false;
                    }


                    //Analisis_Mac
                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Analisis_Mac.HasValue)
                    {
                        if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Analisis_Mac.Value == DateTime.Parse("01-01-1900"))
                        {
                            dtpFAnalisisMac.Text = "";
                            cmbHora2.SelectedText = "00";
                            cmbMinuto2.SelectedText = "00";
                        }
                        else
                        {
                            dtpFAnalisisMac.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Analisis_Mac.Value;
                            cmbHora2.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Analisis_Mac.Value.ToString("hh");
                            cmbMinuto2.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Analisis_Mac.Value.ToString("mm");

                            grpOriginales.Enabled = false;
                            dtpFAnalisisMac.Enabled = false;
                            btnFAnalisisMac.Visible = false;
                            btnFAtencion.Visible = false;
                            btnEnvio.Visible = false;

                            if (lblStatus.Text == "En proceso" && cmbTipoSolicitud.SelectedIndex != 3)
                            {
                                btnFFormalizada.Visible = true;
                                btnFAtencion.Visible = true;
                                btnDesbloqueo.Visible = true;

                            }
                            else if (lblStatus.Text == "En proceso" && cmbTipoSolicitud.SelectedIndex == 3)
                            {
                                btnConcluirSolicitud.Enabled = true;
                                btnEnvio.Visible = true;
                            }
                        }

                    }


                    cmbHora2.Enabled = false;
                    cmbMinuto2.Enabled = false;

                    //Formalizada
                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Formalizada.HasValue)
                    {
                        if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Formalizada.Value == DateTime.Parse("01-01-1900"))
                        {
                            dtpFFormalizada.Text = "";
                            cmbHora3.SelectedText = "00";
                            cmbMinuto3.SelectedText = "00";
                        }
                        else
                        {
                            dtpFFormalizada.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Formalizada.Value;
                            cmbHora3.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Formalizada.Value.ToString("hh");
                            cmbMinuto3.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Formalizada.Value.ToString("mm");

                            dtpFFormalizada.Enabled = false;
                            btnFFormalizada.Visible = false;

                            if (lblStatus.Text == "En proceso")
                            {
                                btnEnvio.Visible = false;
                            }
                        }
                    }

                    cmbHora3.Enabled = false;
                    cmbMinuto3.Enabled = false;




                    //Repc_Originales
                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Originales.HasValue)
                    {
                        if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Originales.Value == DateTime.Parse("01-01-1900"))
                        {
                            dtpFRecepcion.Text = "";
                            cmbHora4.SelectedText = "00";
                            cmbMinuto4.SelectedText = "00";
                        }
                        else
                        {
                            dtpFRecepcion.Text = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Originales.Value.ToString("dd-MM-yyyy");
                            cmbHora4.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Originales.Value.ToString("hh");
                            cmbMinuto4.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Originales.Value.ToString("mm");

                            dtpFRecepcion.Enabled = false;
                            btnFRecepcion.Enabled = false;


                            if (lblStatus.Text == "En proceso")
                            {
                                btnFAtencion.Visible = true;
                            }
                        }
                    }

                    cmbHora4.Enabled = false;
                    cmbMinuto4.Enabled = false;




                    //Aten_Originales
                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Aten_Originales.HasValue)
                    {
                        if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Aten_Originales.Value == DateTime.Parse("01-01-1900"))
                        {
                            dtpFAtencion.Text = String.Empty;
                            cmbHora5.SelectedText = "00";
                            cmbMinuto5.SelectedText = "00";
                        }
                        else
                        {
                            dtpFAtencion.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Aten_Originales.Value;
                            cmbHora5.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Aten_Originales.Value.ToString("hh");
                            cmbMinuto5.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Aten_Originales.Value.ToString("mm");

                            dtpFAtencion.Enabled = false;
                            btnFAtencion.Enabled = false;

                            if (lblStatus.Text == "En proceso")
                            {
                                grpOriginales.Enabled = true;
                            }
                        }
                    }

                    cmbHora5.Enabled = false;
                    cmbMinuto5.Enabled = false;



                    if (dtpFAtencion.Value == default_dtp)
                    {
                        rbcorrectos.Enabled = false;
                        rbcorrectos.Checked = false;
                        rbIncorrectos.Enabled = false;
                        rbIncorrectos.Checked = false;
                    }
                    else
                    {
                        rbcorrectos.Enabled = true;
                        rbcorrectos.Checked = true;
                        rbIncorrectos.Enabled = true;
                        rbIncorrectos.Checked = true;
                    }


                    TxtDepositoTkt.Text = (seguimiento_doc.Deposito_InicialTKT.HasValue) ? seguimiento_doc.Deposito_InicialTKT.Value.ToString("C", format_mxn) : "0.00";
                    txtDepositoIni.Text = (seguimiento_doc.SEGUIMIENTO_DOCTOS.Deposito_Inicial.HasValue) ? seguimiento_doc.SEGUIMIENTO_DOCTOS.Deposito_Inicial.Value.ToString("C", format_mxn) : "0.00";

                    if (txtDepositoIni.Text == "0.00")
                    {
                        txtDepositoIni.Enabled = false;
                    }

                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Desbloqueo_Sistemas.Value == new DateTime(1900, 1, 1))
                    {
                        dtpDesbloqueo.Enabled = true;
                        dtpDesbloqueo.Value = default_dtp;
                    }
                    else
                    {
                        dtpDesbloqueo.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Desbloqueo_Sistemas.Value;
                        dtpDesbloqueo.Enabled = false;

                        btnDesbloqueo.Visible = false;
                    }


                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Envio_Agencia.Value == new DateTime(1900, 1, 1))
                    {
                        dtpEnvio.Value = default_dtp;
                        dtpEnvio.Enabled = false;
                        btnEnvio.Visible = true;
                    }
                    else
                    {
                        dtpEnvio.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Envio_Agencia.Value;
                        dtpEnvio.Enabled = false;
                    }


                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Cancelacion.Value == new DateTime(1900, 1, 1))
                    {
                        dtpFechaCancelada.Value = default_dtp;
                    }
                    else
                    {
                        dtpFechaCancelada.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Cancelacion.Value;
                    }


                    if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Concluida.Value == new DateTime(1900, 1, 1))
                    {
                        dtpConcluir.Value = default_dtp;
                    }
                    else
                    {
                        dtpConcluir.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Concluida.Value;
                    }


                    cmbTipoTramite.SelectedValue = seguimiento_doc.Id_Tramite;


                    if (cmbTipoSolicitud.SelectedIndex != 1)
                    {
                        lblDeposito.Visible = false;
                        txtDepositoIni.Visible = false;
                    }
                    else
                    {
                        lblDeposito.Visible = true;
                        txtDepositoIni.Visible = true;
                    }


                    LlenaDtgdwObservaciones();

                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaDtgdwObservaciones()
        {
            try
            {
                SEGUIMIENTO observaciones = (
                       from s in bdbmtktp01.SEGUIMIENTO
                       join o in bdbmtktp01.OBSERVACIONES on s.Num_Solicitud equals o.Num_Solicitud
                       where s.Num_Solicitud == Int32.Parse(txtSolicitud.Text)
                       select s
                   ).FirstOrDefault();

                if (observaciones == null)
                {
                    MessageBox.Show("No se pudieron cargar las observaciones para la solicitud " + txtSolicitud.Text, "Error de carga.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dtgvwObservaciones.Enabled = true;


                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Observaciones");
                dt.Columns.Add("Fecha Observacion");

                foreach (OBSERVACIONES observacion in observaciones.OBSERVACIONES)
                {
                    DataRow dr = dt.NewRow();

                    dr["Observaciones"] = observacion.Observaciones1;
                    dr["Fecha Observacion"] = observacion.Fecha_Observ;

                    dt.Rows.Add(dr);
                }

                dtgvwObservaciones.DataSource = dt;
                dtgvwObservaciones.Refresh();



            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void HabilitarRbFechas(bool v)
        {
            btnFRecepDoc.Enabled = v;
            btnFAnalisisMac.Enabled = v;
            btnFFormalizada.Enabled = v;
            btnFRecepcion.Enabled = v;
            btnFAtencion.Enabled = v;
            btnDesbloqueo.Enabled = v;
            btnEnvio.Enabled = v;
        }

        private void VisibleRbFechas(bool v)
        {
            btnFRecepDoc.Visible = v;
            btnFAnalisisMac.Visible = v;
            btnFFormalizada.Visible = v;
            btnFRecepcion.Visible = v;
            btnFAtencion.Visible = v;
            btnDesbloqueo.Visible = v;
            btnEnvio.Visible = v;
        }

        private void HabilitarCmbsTiempo(bool v)
        {
            cmbHora1.Enabled = v;
            cmbHora2.Enabled = v;
            cmbHora3.Enabled = v;
            cmbHora4.Enabled = v;
            cmbHora5.Enabled = v;

            cmbMinuto1.Enabled = v;
            cmbMinuto2.Enabled = v;
            cmbMinuto3.Enabled = v;
            cmbMinuto4.Enabled = v;
            cmbMinuto5.Enabled = v;

        }

        private void VisibleBtnsFecha(bool v)
        {
            btnFRecepDoc.Visible = v;
            btnFAnalisisMac.Visible = v;
            btnFFormalizada.Visible = v;
            btnFRecepcion.Visible = v;
            btnFAtencion.Visible = v;
            btnDesbloqueo.Visible = v;
            btnEnvio.Visible = v;
        }

        private SEGUIMIENTO VerSeguimientoDoc(int numero_solicitud)
        {
            try
            {
                seguimiento_doc = 
                    (from s in bdbmtktp01.SEGUIMIENTO
                     join sd in bdbmtktp01.SEGUIMIENTO_DOCTOS on s.Num_Solicitud equals sd.Num_Solicitud
                     where s.Num_Solicitud == numero_solicitud
                     select s)
                    .FirstOrDefault();
                return seguimiento_doc;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return seguimiento_doc;
            }
        }

       

        public int Mac_Consecutivo()
        {
            try
            {
                return (bdbmtktp01.SEGUIMIENTO.Max(x => x.Num_Solicitud)) + 1;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return -1;
            }
           
        }

        private void cmbNumeroFuncionario_Click(object sender, EventArgs e)
        {
            cmbNumeroFuncionario_activo = true;
        }

        private void cmbNumeroFuncionario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                string busqueda = cmbNumeroFuncionario.Text.Trim();
                cmbNumeroFuncionario_activo = true;

                //cmbNumeroFuncionario.DataSource = funcionarios.Where(w => w.numero_funcionario.Contains(busqueda)).Select(x => x).ToList();
                cmbNumeroFuncionario.DataSource = funcionarios
                    .Where(w => w.numero_funcionario.StartsWith(busqueda))
                    .OrderBy(o => o.numero_funcionario)
                    .Select(x => x)
                    .ToList();

                cmbNumeroFuncionario.DisplayMember = "numero_funcionario";
                cmbNumeroFuncionario.ValueMember = "numero_registro";
                cmbNumeroFuncionario.Refresh();
            }

        }

        private void cmbSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmbConsultorMac_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void rbExisteCuentaSi_CheckedChanged(object sender, EventArgs e)
        {
            if(str_consultor == "")
            {
                if(rbExisteCuentaSi.Checked)
                {
                    txtCuenta.Enabled = false;
                }
            }
        }

        private void rbExisteCuentaNo_CheckedChanged(object sender, EventArgs e)
        {
            if (str_consultor == "")
            {
                if (rbExisteCuentaNo.Checked)
                {
                    txtCuenta.Enabled = true;
                }
            }

            string ticket;

            txtCuenta.Enabled = true;


        }

        private void cmbTipoTramite_Click(object sender, EventArgs e)
        {
            cmbTipoTramite_activo = true;
        }

        private void cmbTipoSolicitud_Click(object sender, EventArgs e)
        {
            cmbTipoSolicitud_activo = true;
        }

        private void cmbProducto_Click(object sender, EventArgs e)
        {
            cmbProducto_activo = true;
        }

        private void cmbConsultorMac_Click(object sender, EventArgs e)
        {
            cmbConsultorMac_activo = true;
        }
    }
}
