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
        public int se_carga;       
        public List<OBSERVACIONES> lst_observaciones;

        private Frm_Login frmp;
        private bmtktp01Entities bdbmtktp01;
        private CATALOGOSEntities bdCatalogos;
        private FUNCIONARIOSEntities bdFuncionarios;
        private TICKETEntities bdTickets;
        private FuncionesBdbmtktp01 bdFuncBmtktp01;

        private List<FUNCIONARIO> funcionarios;
        private List<UNIDAD_ORGANIZACIONAL_RESUMEN> uors;
        private SEGUIMIENTO seguimiento_doc;

        private string default_cmb = ". . . ";
        private DateTime default_dtp = DateTimePicker.MinimumDateTime;
        private NumberFormatInfo format_mxn = (NumberFormatInfo)CultureInfo.CreateSpecificCulture("es-MX").NumberFormat.Clone();
        private DataTable dt_observaciones;
        private int intTab;
        private int TiempoServicioA, TiempoServicioM, TiempoAtencion;
        private bool cmbNumeroFuncionario_activo = false, cmbConsultorMac_activo = false, cmbProducto_activo = false, cmbTipoSolicitud_activo = false, cmbTipoTramite_activo = false;

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

                if (frmp.consultor_selec == null)
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
                            cmbBanca.SelectedIndex = -1;
                        }


                        if (unidad_organizacional.division != null)
                        {
                            cmbDivision.SelectedValue = unidad_organizacional.division;                          
                        }
                        else
                        {
                            cmbDivision.SelectedIndex = -1;
                        }


                        if (unidad_organizacional.plaza != null)
                        {
                            cmbSucursal.SelectedValue = unidad_organizacional.plaza;
                            cmbPlaza.SelectedValue = unidad_organizacional.plaza;
                        }
                        else
                        {
                            cmbSucursal.SelectedIndex = -1;
                            cmbPlaza.SelectedIndex = -1;
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
                if (frmp.consultor_selec == null)
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

                if(frmp.consultor_selec == null)
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

                if (frmp.consultor_selec == null)
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
            DialogResult dg = MessageBox.Show($"Esta seguro de querer cancelar la solicitud No. {frmp.solicitud_selec.Num_Solicitud}", "Confirmar cancelacion", MessageBoxButtons.YesNo);
            if(dg == DialogResult.Yes)
            {
                //Investigar como ejecutar un update con EntityFramework donde haya retorno de un valor...
               using(var context = new bmtktp01Entities())
               {
                    using(var dbContextTransaction = context.Database.BeginTransaction())
                    {
                       
                        try
                        {                
                            SEGUIMIENTO seguimiento = context.SEGUIMIENTO.Where(w => w.Num_Solicitud == frmp.solicitud_selec.Num_Solicitud).Select(s => s).FirstOrDefault();

                            if (seguimiento != null)
                            {
                                seguimiento.Status = "3";

                                SEGUIMIENTO_DOCTOS seguimiento_doctos = context.SEGUIMIENTO_DOCTOS.Where(w => w.Num_Solicitud == frmp.solicitud_selec.Num_Solicitud).Select(s => s).FirstOrDefault();

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
                            MessageBox.Show("Solicitud No. " + frmp.solicitud_selec.Num_Solicitud + "no pudo ser CANCELADA", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    Calendario.SelectionStart = DateTime.Now;
                    grpCalendario.Tag = 1;
                    grpCalendario.Visible = true;
                    Calendario.Focus();
                }
                else
                {
                    DateTime fecha_servidor = DateTime.Now;

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
                    Calendario.SelectionStart = DateTime.Now;
                    grpCalendario.Tag = 2;
                    grpCalendario.Visible = true;
                    Calendario.Focus();
                }
                else
                {
                    DateTime fecha_servidor = DateTime.Now;

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
                    Calendario.SelectionStart = DateTime.Now;
                    grpCalendario.Tag = 3;
                    grpCalendario.Visible = true;
                    Calendario.Focus();
                }
                else
                {
                    DateTime fecha_servidor = DateTime.Now;

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
                    Calendario.SelectionStart = DateTime.Now;
                    grpCalendario.Tag = 4;
                    grpCalendario.Visible = true;
                    Calendario.Focus();
                }
                else
                {
                    DateTime fecha_servidor = DateTime.Now;

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
                    Calendario.SelectionStart = DateTime.Now;
                    grpCalendario.Tag = 5;
                    grpCalendario.Visible = true;
                    Calendario.Focus();
                }
                else
                {
                    DateTime fecha_servidor = DateTime.Now;

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
                    Calendario.SelectionStart = DateTime.Now;
                    grpCalendario.Tag = 7;
                    grpCalendario.Visible = true;
                    Calendario.Focus();
                }
                else
                {
                    DateTime fecha_servidor = DateTime.Now;

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
                    Calendario.SelectionStart = DateTime.Now;
                    grpCalendario.Tag = 8;
                    grpCalendario.Visible = true;
                    Calendario.Focus();
                }
                else
                {
                    DateTime fecha_servidor = DateTime.Now;

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
            try
            {
                if (frmp.consultor_selec == null)
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
                        SSTabSeg.Enabled = false;
                        frmp.solicitud_selec = new SEGUIMIENTO();
                        frmp.solicitud_selec.Num_Solicitud = Mac_Consecutivo();

                        if (frmp.solicitud_selec.Num_Solicitud <= 0)
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

                            txtSolicitud.Text = frmp.solicitud_selec.Num_Solicitud.ToString();
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
                            string sufijo_Kapiti = cmbProducto.Text.Trim();
                            byte tipo_Persona = (byte)((rbPersonaFisica.Checked) ? 0 : 1);
                            string nombre_Cliente = txtNombre.Text;
                            string apellido_Paterno = txtApellidoP.Text;
                            string apellido_Materno = txtApellidoM.Text;
                            decimal deposito_Inicial = decimal.Parse(TxtDepositoTkt.Text.Replace("$", ""));
                            string numero_Registro = cmbNumeroFuncionario.Text;
                            string nombre_Promotor = cmbPromotor.Text;
                            string banca = cmbBanca.Text;
                            string division = cmbDivision.Text;
                            string plaza = cmbPlaza.Text;
                            string sucursal = cmbSucursal.Text;
                            string status = "1";
                           
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
                            decimal deposito_Inicial_Ini = decimal.Parse(txtDepositoIni.Text.Replace("$", ""));
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
                                frmp.solicitud_selec.Num_Solicitud,
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

                            if (respuesta.Codigo > 0)
                            {
                                if (cmbTipoSolicitud.SelectedText != default_cmb)
                                {
                                    lblDeposito.Visible = false;
                                    txtDepositoIni.Visible = false;
                                }
                                else
                                {

                                    lblDeposito.Visible = true;
                                    txtDepositoIni.Visible = true;
                                }

                                if (rbCircuitoAuto.Checked)
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


                                ////Opcion 1
                                //foreach (DataGridViewRow fila in dtgvwObservaciones.Rows)
                                //{
                                //    int insertado = 0;
                                //    OBSERVACIONES observacion = new OBSERVACIONES { Num_Solicitud = Int32.Parse(txtSolicitud.Text), Observaciones1 = fila.Cells[""].Value.ToString(), Fecha_Observ = DateTime.Now };

                                //    bdbmtktp01.OBSERVACIONES.Add(observacion);

                                //    insertado = bdbmtktp01.SaveChanges();
                                //}

                                //Opcion 2
                                context.OBSERVACIONES.AddRange(lst_observaciones);
                                context.SaveChanges();

                                txtSolicitud.Enabled = false;
                                lblStatus.Text = "En proceso";

                                MessageBox.Show("El Número de folio es: " + frmp.solicitud_selec.Num_Solicitud, "Numero de folio", MessageBoxButtons.OK, MessageBoxIcon.Information);

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


                                    if (lblStatus.Text == "En proceso")
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

                                        if (LblDesbloquep.Visible == true)
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

                                        grpOriginales.Enabled = false;
                                        lblDeposito.Visible = false;
                                        txtDepositoIni.Visible = false;

                                        txtDepositoIni.Enabled = false;
                                    }
                                    if (lblStatus.Text == "En proceso")
                                    {
                                        grpCircuito.Enabled = true;

                                        HabilitarRbFechas(true);
                                        HabilitarCmbsTiempo(true);
                                        VisibleRbFechas(true);

                                        if (LblDesbloquep.Visible == true)
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

                                        cmbTipoSolicitud.Enabled = true;
                                        lblDeposito.Visible = true;
                                        txtDepositoIni.Visible = true;
                                        txtDepositoIni.Enabled = true;
                                        txtApellidoP.Enabled = true;
                                        txtNombre.Enabled = true;
                                        txtApellidoM.Enabled = true;
                                    }
                                }

                                if (lblStatus.Text == "En proceso")
                                {
                                    grpCircuito.Enabled = true;

                                    HabilitarRbFechas(true);
                                    HabilitarCmbsTiempo(true);
                                    VisibleRbFechas(true);

                                    if (LblDesbloquep.Visible == true)
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

                                    cmbTipoSolicitud.Enabled = true;
                                    lblDeposito.Visible = true;
                                    txtDepositoIni.Visible = true;
                                    txtDepositoIni.Enabled = true;
                                    txtApellidoP.Enabled = true;
                                    txtNombre.Enabled = true;
                                    txtApellidoM.Enabled = true;

                                }

                                tmtValidarBoton.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Error al ingresar la operacion " + respuesta.FechaHora_Captura, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            tmtValidarBoton.Enabled = true;
                        }
                    }
                    else if (btnGuardar.Text == "Modificar")
                    {
                        if (MessageBox.Show($"Esta seguro de querer cancelar la solicitud No. {frmp.solicitud_selec.Num_Solicitud}", "Confirmar cancelacion", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (dtpFAnalisisMac.Value != default_dtp)
                            {
                                TimeSpan hora_FRecepDoc = new TimeSpan(Int32.Parse(cmbHora1.SelectedValue.ToString()), Int32.Parse(cmbMinuto1.SelectedValue.ToString()), 0);
                                TimeSpan hora_FAnalisisMac = new TimeSpan(Int32.Parse(cmbHora2.SelectedValue.ToString()), Int32.Parse(cmbMinuto2.SelectedValue.ToString()), 0);

                                if ((dtpFRecepDoc.Value.Date + hora_FRecepDoc) > dtpFAnalisisMac.Value.Date + hora_FAnalisisMac)
                                {
                                    MessageBox.Show("La fecha y hora de Analisis Mac no puede ser menor o igual a la fecha y hora de Expediente Único", "Analisis Mac", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                }
                            }

                            if (dtpFFormalizada.Value != default_dtp)
                            {
                                if (rbCircuitoAuto.Checked)
                                {

                                }
                                else
                                {
                                    TimeSpan hora_FAnalisisMac = new TimeSpan(Int32.Parse(cmbHora2.SelectedValue.ToString()), Int32.Parse(cmbMinuto2.SelectedValue.ToString()), 0);
                                    TimeSpan hora_FFormalizada = new TimeSpan(Int32.Parse(cmbHora3.SelectedValue.ToString()), Int32.Parse(cmbMinuto3.SelectedValue.ToString()), 0);

                                    if ((dtpFAnalisisMac.Value.Date + hora_FAnalisisMac) > dtpFFormalizada.Value.Date + hora_FFormalizada)
                                    {
                                        MessageBox.Show("La fecha y hora de Formalización no puede ser menor o igual a la fecha y hora de Analisis Mac", "Formalizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    }
                                }
                            }


                            if (dtpFAtencion.Value != default_dtp)
                            {
                                TimeSpan hora_FRecepcion = new TimeSpan(Int32.Parse(cmbHora4.SelectedValue.ToString()), Int32.Parse(cmbMinuto4.SelectedValue.ToString()), 0);
                                TimeSpan hora_FAtencion = new TimeSpan(Int32.Parse(cmbHora5.SelectedValue.ToString()), Int32.Parse(cmbMinuto5.SelectedValue.ToString()), 0);

                                if ((dtpFRecepcion.Value.Date + hora_FRecepcion) > dtpFAtencion.Value.Date + hora_FAtencion)
                                {
                                    MessageBox.Show("La fecha y hora de Atención de originales no puede ser menor o igual a la fecha y hora de Recepción", "Atención de originales", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                }

                            }

                            if (rbcorrectos.Checked == true || rbIncorrectos.Checked == true)
                            {
                                str_docuemntos = (rbcorrectos.Checked == true) ? "0" : "1";
                            }
                            else
                            {
                                str_docuemntos = "-1";
                            }


                            dtpEnvio.Enabled = true;

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

                            Mac_Actualiza_Respuesta respuesta = context.Mac_Actualiza_Datos(
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
                                frmp.solicitud_selec.Num_Solicitud,
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

                            dtpEnvio.Enabled = true;

                            if (respuesta.Codigo > 0)
                            {
                                if (dtpFAnalisisMac.Value != default_dtp)
                                {
                                    TimeSpan diferiencia = ((dtpFRecepDoc.Value.Date + (new TimeSpan(Int32.Parse(cmbHora1.SelectedValue.ToString()), Int32.Parse(cmbMinuto1.SelectedValue.ToString()), 0)))) - ((dtpFAnalisisMac.Value.Date + (new TimeSpan(Int32.Parse(cmbHora2.SelectedValue.ToString()), Int32.Parse(cmbMinuto2.SelectedValue.ToString()), 0))));
                                    if (diferiencia.Days < 46)
                                    {
                                        txtNivelTiempo.Text = "EN TIEMPO";
                                    }
                                    else
                                    {
                                        txtNivelTiempo.Text = "FUERA DE TIEMPO";
                                    }
                                }

                                if (dtpFAtencion.Value != default_dtp)
                                {
                                    TimeSpan diferiencia = ((dtpFRecepcion.Value.Date + (new TimeSpan(Int32.Parse(cmbHora4.SelectedValue.ToString()), Int32.Parse(cmbMinuto4.SelectedValue.ToString()), 0)))) - ((dtpFAtencion.Value.Date + (new TimeSpan(Int32.Parse(cmbHora5.SelectedValue.ToString()), Int32.Parse(cmbMinuto5.SelectedValue.ToString()), 0))));
                                    if (diferiencia.Days < 24)
                                    {
                                        txtNivelDias.Text = "EN TIEMPO";
                                    }
                                    else
                                    {
                                        txtNivelDias.Text = "FUERA DE TIEMPO";
                                    }
                                }

                                txtSolicitud.Enabled = true;

                                foreach (DataGridViewRow fila in dtgvwObservaciones.Rows)
                                {
                                    int insertado = 0;
                                    OBSERVACIONES observacion = new OBSERVACIONES { Num_Solicitud = frmp.solicitud_selec.Num_Solicitud, Observaciones1 = fila.Cells[""].Value.ToString(), Fecha_Observ = DateTime.Now };

                                    bdbmtktp01.OBSERVACIONES.Add(observacion);

                                    insertado = bdbmtktp01.SaveChanges();
                                }

                                txtSolicitud.Enabled = false;

                                MessageBox.Show("El No. de folio:" + frmp.solicitud_selec.Num_Solicitud + " Ha sido modifidado satisfactoriamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                //DoEvents
                                tmrTraerDatos.Enabled = true;
                                //SSTabSeg.Tab = intTab
                                //Me.MousePointer = vbArrow
                            }
                            else
                            {
                                //Me.MousePointer = vbArrow
                            }

                        }
                        tmtValidarBoton.Enabled = true;
                        //SSTabSeg.Enabled = True
                        //Me.MousePointer = vbArrow
                    }
                }

                tmtValidarBoton.Enabled = true;
                //tmrTab.Tag = intTab
                tmrTab.Enabled = true;


                //SSTabSeg.Enabled = true
                //opersolisitud.cerrarrecordset
                //opersolisitud.cerrarconexionsql
                //Me.MousePointer = vbArrow
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }         
        }

        /// <summary>
        /// Se ejecuta al presionar el boton de limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            frmp.consultor_selec = null;
            Limpiar();
            SSTabSeg.TabIndex = (btnLimpiar.Text == "Nuevo") ? 0 : 1;

            btnLimpiar.Text = "Limpiar";
        }

        /// <summary>
        /// Se ejecuta al presionar boton de nueva observacion 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevaObservacion_Click(object sender, EventArgs e)
        {
            Frm_NuevaObvservacion frm = new Frm_NuevaObvservacion(this);
            frm.ShowDialog();
            if(frm.observacion != "")
            {
             
                lst_observaciones.Add(new OBSERVACIONES { Num_Solicitud = frmp.solicitud_selec.Num_Solicitud, Fecha_Observ = DateTime.Now, Observaciones1 = frm.observacion });
                

                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Observaciones");
                dt.Columns.Add("Fecha Observacion");

                foreach (OBSERVACIONES observacion in lst_observaciones)
                {
                    DataRow dr = dt.NewRow();

                    dr["Observaciones"] = observacion.Observaciones1;
                    dr["Fecha Observacion"] = observacion.Fecha_Observ;

                    dt.Rows.Add(dr);
                }

                dtgvwObservaciones.DataSource = null;
                dtgvwObservaciones.DataSource = dt;
                dtgvwObservaciones.Columns["Observaciones"].Width = 800;
                dtgvwObservaciones.Refresh();
            }
        }

        /// <summary>
        /// Se ejecuta al presionar el boton de Salir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Se ejecuta al presionar boton concluir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConcluirSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                string str_fecha;
                int DepositoIni = 0;
                
                if((rbcorrectos.Checked == true && int.TryParse(txtDepositoIni.Text, out DepositoIni) == true) || ((TIPO_SOLICITUD)cmbTipoSolicitud.SelectedItem).Id_Solicitud == 3)
                {
                    if(DepositoIni > 0)
                    {
                        if(MessageBox.Show($"Esta seguro de querer concluir la solicitud No. {frmp.solicitud_selec.Num_Solicitud}", "Confirmar concluir solicitud", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if(FuncionesBdbmtktp01.Actualiza_Docs(frmp.solicitud_selec.Num_Solicitud) > 0)
                            {
                                MessageBox.Show("Solicitud concluida satisfactoriamente", "Actualizacion hecha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                VisibleRbFechas(false);

                                tmrTraerDatos.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("No se pudo actualizar el status de de la solicitud " + frmp.solicitud_selec.Num_Solicitud, "Error de actualizacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Se ejecuta al presionar boton de ver obvservaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerObservacion_Click(object sender, EventArgs e)
        {
            try
            {
                string datos_ob = "";
                Frm_VistaObvservaciones frm = new Frm_VistaObvservaciones();


                foreach (DataGridViewRow fila in dtgvwObservaciones.Rows)
                {
                    if(fila.Selected)
                    {
                        datos_ob +=Environment.NewLine + fila.Cells["Observaciones"].Value;
                        //datos_ob +=Environment.NewLine + fila.Cells["Observacion"].Value;
                    }
                }

                if(datos_ob == "")
                {
                    frm.observaciones = datos_ob;
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("!!  Debe de seleccionar la observacion que desea visualizar.  ¡¡¡", "Error de visualizacion de observacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                foreach (DataGridViewRow fila in dtgvwObservaciones.Rows)
                {
                    fila.Selected = false;
                }



            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Se ejecuta al activarse el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_NuevaSolicitud_Activated(object sender, EventArgs e)
        {
            //Me.MousePointer = vbHourglass

            //Calendario.DayFont.Size = 9
            //Calendario.GridLinesColor = vbBlack
            //Calendario.ShowDays = True
            //Calendario.ShowVerticalGrid = True
            //Calendario.ShowHorizontalGrid = True
            //frmLogin.Caption = "Generacion de solicitudes GO-MAC"
            //LblNivelServicio(0).Alignment = vbCenter
            //LblNivelServicio(0).Caption = "Nivel de Servicio" & vbCrLf & "Formalizada / Análisis Mac"
            //LblNivelServicio(1).Alignment = vbCenter
            //LblNivelServicio(1).Caption = "Nivel de Servicio" & vbCrLf & "Recepción / Atención de Originales"



            if (frmp.consultor_selec == null)
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

            if (frmp.activa == 1)
            {
                btnLimpiar.Visible = false;
                btnGuardar.Visible = true;
                btnCancelarSolicitud.Visible = true;

                btnNuevaObservacion.Visible = true;
                btnVerObservacion.Visible = true;

                btnLimpiar.Enabled = false;
                btnGuardar.Enabled = true;
                btnCancelarSolicitud.Enabled = true;

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

            if (frmp.consultor_selec != null)
            {
                if (lblStatus.Text == "Nueva")
                {
                    MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Llena campo de numeor de solicitud con el consecutivo siguiente y el cmapo fecha con la fecha actual
        /// </summary>
        private void LlenarFechaNumero()
        {
            try
            {
                if (frmp.consultor_selec == null)
                {
                    int consecutivo = Mac_Consecutivo();

                    if (consecutivo == -1)
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

        /// <summary>
        /// Habilita/deshabilita controles y trae datos para la solicitud
        /// </summary>
        private void CargaConsulta()
        {
            try
            {
                if (frmp.consultor_selec != null)
                {
                    se_carga = 0;
                    tmrTraerDatos.Enabled = false;

                    txtSolicitud.Text = frmp.solicitud_selec.Num_Solicitud.ToString();
                    TraerDatos();

                    tmtValidarBoton.Enabled = true;

                    switch (lblStatus.Text)
                    {
                        case "En proceso":
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

                    }
                }

                dtpFechaCaptura.Enabled = false;
                txtPuntos.Enabled = false;
                dtpFechaCancelada.Enabled = false;

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Se ejecuta al  cargar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            lst_observaciones = new List<OBSERVACIONES>();

            //***PLACE HOLDERS********************************************************************
            txtNombre.GotFocus += new EventHandler(this.TxtNombreGotFocus);
            txtNombre.LostFocus += new EventHandler(this.TxtNombreLostFocus);

            txtApellidoP.GotFocus += new EventHandler(this.TxtApelllidoPGotFocus);
            txtApellidoP.LostFocus += new EventHandler(this.TxtApellidoPLostFocus);

            txtApellidoM.GotFocus += new EventHandler(this.TxtApelllidoMGotFocus);
            txtApellidoM.LostFocus += new EventHandler(this.TxtApellidoMLostFocus);

            txtCuenta.LostFocus += new EventHandler(this.TxtCuentaLostFocus);
            txtDepositoIni.LostFocus += new EventHandler(this.TxtDepositoIniLostFocus);
            TxtDepositoTkt.LostFocus += new EventHandler(this.TxtDepositoTktLostFocus);
            //************************************************************************************

            if (frmp.consultor_selec != null)
            {
                LlenarCombos();

                CargaConsulta();

                if (lblStatus.Text == "Nueva")
                {
                    MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            if(frmp.consultor_selec == null)
            {
                LlenarCombos();

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
            else
            {
                switch (lblStatus.Text)
                {
                    case "Nueva":
                        MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    break;
                }
            }


            if (frmp.activa == 1)
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

            if (frmp.consultor_selec != null)
            {
                tmrTraerDatos.Enabled = false;

                //Me.MousePointer = 11
                //Frame2.MousePointer = 11
                //fraGeneral.MousePointer = 11
                //SSTabSeg.MousePointer = ssHourglass
                //FrmTicket.MousePointer = 11
                //FrmTipoPersona.MousePointer = 11
                //Frame1.MousePointer = 11
                //FrmNombreCliente.MousePointer = 11

                txtSolicitud.Text = frmp.solicitud_selec.Num_Solicitud.ToString();
                CargaConsulta();

                //Me.MousePointer = 0
                //Frame2.MousePointer = 0
                //fraGeneral.MousePointer = 0
                //SSTabSeg.MousePointer = ssDefault
                //FrmTicket.MousePointer = 0
                //FrmTipoPersona.MousePointer = 0
                //Frame1.MousePointer = 0
                //FrmNombreCliente.MousePointer = 0
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

        private void LlenarCombos()
        {
            try
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

                //LlenaComboNumFunc();               
                LlenaCombo(cmbNumeroFuncionario, funcionarios.OrderBy(o => o.numero_funcionario).ToList(), "numero_registro", "numero_funcionario");
                //LlenaComboNombreFunc();
                LlenaCombo(cmbPromotor, funcionarios.OrderBy(o => o.nombre_funcionario).ThenBy(o => o.apellido_paterno).ThenBy(o => o.apellido_materno).ToList(), "funcionario1", "_nombreCompleto");


                //LlenaComboBanca();
                LlenaCombo(cmbBanca, uors.OrderBy(o => o.banca).Where(w => w.banca != null).ToList(), "banca", "banca");
                //LlenaComboplaza();
                LlenaCombo(cmbPlaza, uors.OrderBy(o => o.plaza).Where(w => w.plaza != null).ToList(), "plaza", "plaza");
                //LlenaComboDivision();
                LlenaCombo(cmbDivision, uors.OrderBy(o => o.division).Where(w => w.division != null).ToList(), "division", "division");
                //LlenaComboSucursal();
                LlenaCombo(cmbSucursal, uors.OrderBy(o => o.plaza).Where(w => w.plaza != null).ToList(), "plaza", "plaza");
                //LlenaComboProducto();
                List<PRODUCTOS> productos = bdCatalogos.PRODUCTOS.OrderBy(o => o.Producto).ToList();
                productos.Insert(0, new PRODUCTOS { Producto = ". . .  " });
                LlenaCombo(cmbProducto, productos, "Id_Producto", "Producto");


                //LlenaComboTipoSolicitud();
                LlenaCombo(cmbTipoSolicitud, bdbmtktp01.TIPO_SOLICITUD.OrderBy(o => o.Descripcion_Solicitud).ToList(), "Id_Solicitud", "Descripcion_Solicitud");
                LlenaComboTipoTramite();
                //LlenarComboConsultor();
                List<CONSULTORES> consultores = bdbmtktp01.CONSULTORES.OrderBy(o => o.Iniciales_ConsultorMac).ToList();
                consultores.Insert(0, new CONSULTORES { Iniciales_ConsultorMac = default_cmb });
                LlenaCombo(cmbConsultorMac, consultores, "Id_ConsultorMac", "Iniciales_ConsultorMac");
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaCombo<T>(ComboBox cmb, List<T> lista, string value_member, string display_member)
        {
            try
            {
                if (lista != null)
                {
                    cmb.DataSource = lista;
                    cmb.ValueMember = value_member;
                    cmb.DisplayMember = display_member;
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

     
       


        /// <summary>
        /// Llena combobox con los tipos de tramites
        /// </summary>
        /// <param name="ts"></param>
        private void LlenaComboTipoTramite(TIPO_SOLICITUD ts = default)
        {
            try
            {
                List<TIPO_TRAMITE> tipos_tramite =
                    (from tt in bdbmtktp01.TIPO_TRAMITE orderby tt.Descripcion_Tramite ascending select tt).ToList();

                if (ts != default)
                {
                    tipos_tramite = tipos_tramite
                        .Where(w => w.Descripcion_Tramite.ToUpper().Contains(ts.Descripcion_Solicitud.ToUpper().Substring(0, 5).Trim()))
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

        /// <summary>
        /// Se ejecuta al seleccionar un radiobutton del grupo Originales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbcorrectos_CheckedChanged(object sender, EventArgs e)
        {
            if(rbIncorrectos.Checked == true)
            {
                btnConcluirSolicitud.Enabled = false;
            }
            else if(rbcorrectos.Checked == true)
            {
                btnConcluirSolicitud.Enabled = true;
                int DepositoIni = 0; 
                if(int.TryParse(txtDepositoIni.Text, out DepositoIni))
                {
                    if(DepositoIni > 0)
                    {
                        btnConcluirSolicitud.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Se ejecuta al seleccionar un radiobutton del grupo Originales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbIncorrectos_CheckedChanged(object sender, EventArgs e)
        {
           
        }


        /// <summary>
        /// Se ejecuta al seleccionar un radiobutton del grupo de tipo de persona fisica / moral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPersonaFisica_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Se ejecuta al seleccionar un radiobutton del grupo de tipo de persona fisica / moral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPersonaMoral_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Se ejecuta al seleccionar un radiobutton del grupo de existe ticket si/no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbExisteCuentaSi_CheckedChanged(object sender, EventArgs e)
        {
            if(rbExisteCuentaSi.Checked)
            {
                cmbProducto.Enabled = false;
                txtNombre.Enabled = false;
                txtApellidoP.Enabled = false;
                txtApellidoM.Enabled = false;
                TxtDepositoTkt.Enabled = true;
                rbPersonaFisica.Enabled = false;
                rbPersonaMoral.Enabled = false;
            }
            else if(rbExisteCuentaNo.Checked)
            {
                cmbProducto.Enabled = true;
                txtNombre.Enabled = true;
                txtApellidoP.Enabled = true;
                txtApellidoM.Enabled = true;
                TxtDepositoTkt.Enabled = true;
                rbPersonaFisica.Enabled = true;
                rbPersonaMoral.Enabled = true;
            }

            if (txtCuenta.Enabled) txtCuenta.Focus();


            cmbProducto.SelectedText = default_cmb;
            txtNombre.Text = "";
            txtApellidoP.Text = "";
            txtApellidoM.Text = "";
            TxtDepositoTkt.Text = "";
        }

        /// <summary>
        /// Se ejecuta al seleccionar un radiobutton del grupo de existe ticket si/no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbExisteCuentaNo_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Se ejecuta al cambiar de pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SSTabSeg_TabIndexChanged(object sender, EventArgs e)
        {
            intTab = SSTabSeg.SelectedIndex;
        }


        private void tmrTab_Tick(object sender, EventArgs e)
        {
            //SSTabSeg.Tab = IIf(tmrTab.Tag = "", 0, tmrTab.Tag)
            //tmrTab.Enabled = False
        }

        /// <summary>
        /// Carga datos de la solicitud
        /// </summary>
        private void TraerDatos()
        {
            try
            {
                int inttiemposervicio, intContador;

                this.Text = (frmp.consultor_selec == null) ? "Captura de solicitud" : "Consulta de solicitud";

                if (VerSeguimientoDoc(frmp.solicitud_selec.Num_Solicitud) != null)
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

                        VisibleRbFechas(false);
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

                            VisibleRbFechas(false);
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

                    //SSTabSeg.TabEnabled(1) = True
                    //SSTabSeg.TabEnabled(2) = True

                    if (rbCircuitoAuto.Checked)
                    {
                        inttiemposervicio = TiempoServicioA;
                    }
                    else
                    {
                        inttiemposervicio = TiempoServicioM;
                    }


                    if (dtpFRecepDoc.Value != default_dtp && dtpFAnalisisMac.Value != default_dtp)
                    {
                        TimeSpan diferiencia = ((dtpFRecepDoc.Value.Date + (new TimeSpan(Int32.Parse(cmbHora1.SelectedValue.ToString()), Int32.Parse(cmbMinuto1.SelectedValue.ToString()), 0)))) - ((dtpFAnalisisMac.Value.Date + (new TimeSpan(Int32.Parse(cmbHora2.SelectedValue.ToString()), Int32.Parse(cmbMinuto2.SelectedValue.ToString()), 0))));
                        if (diferiencia.Days < inttiemposervicio)
                        {
                            txtNivelTiempo.Text = "EN TIEMPO";
                        }
                        else
                        {
                            txtNivelTiempo.Text = "FUERA DE TIEMPO";
                        }
                    }

                    if (dtpFAtencion.Value != default_dtp && dtpFAnalisisMac.Value != default_dtp)
                    {
                        TimeSpan diferiencia = ((dtpFRecepcion.Value.Date + (new TimeSpan(Int32.Parse(cmbHora4.SelectedValue.ToString()), Int32.Parse(cmbMinuto4.SelectedValue.ToString()), 0)))) - ((dtpFAtencion.Value.Date + (new TimeSpan(Int32.Parse(cmbHora5.SelectedValue.ToString()), Int32.Parse(cmbMinuto5.SelectedValue.ToString()), 0))));
                        if (diferiencia.Days < TiempoAtencion)
                        {
                            txtNivelDias.Text = "EN TIEMPO";
                        }
                        else
                        {
                            txtNivelDias.Text = "FUERA DE TIEMPO";
                        }
                    }

                    if(lblStatus.Text != "En proceso")
                    {
                        grpOriginales.Enabled = false;
                        btnLimpiar.Visible = false;
                        btnLimpiar.Enabled = false;
                        dtpConcluir.Enabled = false;

                        VisibleRbFechas(false);
                    }

                    if(((TIPO_SOLICITUD)cmbTipoSolicitud.SelectedItem).Descripcion_Solicitud != "ACTUALIZACION")
                    {
                        int DepositoIni = 0;
                        if(rbcorrectos.Checked == true && int.TryParse(txtDepositoIni.Text, out DepositoIni))
                        {
                            if(DepositoIni > 0)
                            {
                                btnDesbloqueo.Visible = true;
                                LblDesbloquep.Visible = true;
                                dtpDesbloqueo.Visible = true;

                                if (dtpDesbloqueo.Value != default_dtp)
                                {
                                    if (dtpConcluir.Value != default_dtp)
                                    {
                                        btnConcluirSolicitud.Enabled = true;
                                    }
                                    else
                                    {
                                        btnDesbloqueo.Visible = false;
                                        btnConcluirSolicitud.Enabled = false;
                                    }
                                }
                                else
                                {
                                    btnConcluirSolicitud.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            btnConcluirSolicitud.Visible = false;
                            LblDesbloquep.Visible = false;
                            dtpDesbloqueo.Visible = false;

                            if (((TIPO_SOLICITUD)cmbTipoSolicitud.SelectedItem).Id_Solicitud != 3) btnConcluirSolicitud.Enabled = false;
                        }
                    }
                    else
                    {
                        if(dtpConcluir.Value != default_dtp)
                        {
                            btnConcluirSolicitud.Enabled = true;
                        }
                        else
                        {
                            btnConcluirSolicitud.Enabled = false;
                        }
                    }

                    if(lblStatus.Text != "En proceso")
                    {
                        txtNombre.Enabled = false;
                        txtApellidoP.Enabled = false;
                        txtApellidoM.Enabled = false;
                        TxtDepositoTkt.Enabled = false;
                    }
                }
                else
                {
                    //SSTabSeg.TabEnabled(1) = False
                    //SSTabSeg.TabEnabled(2) = False
                    //Me.MousePointer = vbArrow
                    MessageBox.Show("No existe la solicitud", "Obtencion de valores", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSolicitud.Text = "";
                    if(txtSolicitud.Enabled)
                    {
                        txtSolicitud.Focus();
                    }
                    //SSTabSeg.Tab = 0
                }

                //APAGAR TIMER
                btnLimpiar.Visible = true;
                tmrTraerDatos.Enabled = false;

                txtSolicitud.Enabled = false;
                //Me.MousePointer = vbArrow

                if(lblStatus.Text == "En proceso")
                {
                    //Me.MousePointer = vbArrow
                    grpCircuito.Enabled = true;

                    HabilitarRbFechas(true);
                    HabilitarCmbsTiempo(true);
                    VisibleRbFechas(true);

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

                    cmbTipoSolicitud.Enabled = true;
                    lblDeposito.Visible = true;
                    txtDepositoIni.Visible = true;
                    txtDepositoIni.Enabled = true;
                    txtApellidoP.Enabled = true;
                    txtNombre.Enabled = true;
                    txtApellidoM.Enabled = true;
                    //Me.MousePointer = vbArrow
                }
                else
                {
                    //Me.MousePointer = vbArrow
                    grpCircuito.Enabled = false;
                    btnLimpiar.Visible = false;
                    HabilitarRbFechas(true);
                    HabilitarCmbsTiempo(true);
                    VisibleRbFechas(true);

                    btnDesbloqueo.Visible = false;
                    btnDesbloqueo.Enabled = false;

                    grpOriginales.Enabled = false;
                    lblDeposito.Visible = false;
                    txtDepositoIni.Visible = false;

                    txtDepositoIni.Enabled = false;
                    txtApellidoP.Enabled = false;
                    txtNombre.Enabled = false;
                    txtApellidoM.Enabled = false;
                }
                //Me.MousePointer = vbArrow

                if(lblStatus.Text == "En proceso" || lblStatus.Text == "Cancelada" || lblStatus.Text == "Concluida")
                {
                    lblDeposito.Visible = true;
                    txtDepositoIni.Visible = true;
                    //Me.MousePointer = vbArrow
                }

                dtpFechaCaptura.Enabled = false;
                dtpFechaCancelada.Enabled = false;
                //Me.MousePointer = vbArrow

                switch (lblStatus.Text)
                {
                    case "En proceso":
                        dtgvwObservaciones.Enabled = true;
                        btnCancelarSolicitud.Visible = true;
                        btnGuardar.Text = "Modificar";
                        btnGuardar.Visible = true;

                        btnLimpiar.Visible = Visible;
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
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        /// <summary>
        /// Se ejecuta por lapso de tiempo parea traer datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrTraerDatos_Tick(object sender, EventArgs e)
        {
            TraerDatos();
        }

        /// <summary>
        /// Se ejecutapor lapso de tiempo para validar estado de ciertos xontroles del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmtValidarBoton_Tick(object sender, EventArgs e)
        {
            if(rbPersonaFisica.Checked)
            {
                if(cmbConsultorMac.SelectedIndex > -1 
                    && cmbTipoSolicitud.SelectedIndex > -1 
                    && cmbTipoTramite.SelectedIndex > -1 
                    && txtCuenta.Text != ""
                    && cmbProducto.SelectedIndex > -1
                    && txtNombre.Text != ""
                    && txtApellidoP.Text != "" 
                    && txtApellidoM.Text != "" 
                    && TxtDepositoTkt.Text != "")
                {
                    btnGuardar.Enabled = true;
                }
                else
                {
                    btnGuardar.Enabled = false;
                }
            }
            else
            {
                if (cmbConsultorMac.SelectedIndex > -1 
                    && cmbTipoSolicitud.SelectedIndex > -1 
                    && cmbTipoTramite.SelectedIndex > -1 
                    && txtCuenta.Text != ""
                    && cmbProducto.SelectedIndex > -1
                    && txtNombre.Text != ""
                    && TxtDepositoTkt.Text != "")
                {
                    btnGuardar.Enabled = true;
                }
                else
                {
                    btnGuardar.Enabled = false;
                }
            }
        }


        private void txtCuenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            int numero_cuenta;

            if (e.KeyChar == (char)13 && cmbProducto.Enabled == false && int.TryParse(txtCuenta.Text, out numero_cuenta))
            {
                if (txtCuenta.Tag == null)
                {
                    CLIENTE cliente = bdCatalogos.CLIENTE.Where(w => w.cuenta_cliente == txtCuenta.Text).Select(s => s).FirstOrDefault();

                    if (cliente != null)
                    {
                        PRODUCTO_CONTRATADO producto_contratado =
                            bdTickets.PRODUCTO_CONTRATADO
                            .Join(bdTickets.CUENTA_EJE, pc => pc.producto_contratado1, ce => ce.producto_contratado, (pc, ce) => pc)
                            .Join(bdTickets.TIPO_CUENTA_EJE, ce => ce.CUENTA_EJE.tipo_cuenta_eje, tce => tce.tipo_cuenta_eje1, (ce, tce) => ce)
                            .Where(w => w.cuenta_cliente == cliente.cuenta_cliente)
                            .FirstOrDefault();

                        if (producto_contratado != null)
                        {
                            cmbProducto.Enabled = true;

                            cmbProducto.SelectedItem = bdCatalogos.PRODUCTOS.Where(w => w.Producto.Contains(producto_contratado.CUENTA_EJE.TIPO_CUENTA_EJE1.sufijo_kapiti)).FirstOrDefault();

                            if (cliente.persona_moral == 0)
                            {
                                if (rbPersonaFisica.Enabled)
                                {
                                    rbPersonaFisica.Checked = true;
                                }
                            }
                            else if (cliente.persona_moral == 1)
                            {
                                if (rbPersonaMoral.Enabled)
                                {
                                    rbPersonaMoral.Checked = true;
                                }
                            }

                            TxtDepositoTkt.Text = "";
                            txtNombre.Text = cliente.nombre_cliente.TrimEnd();
                            txtApellidoP.Text = cliente.apellido_paterno.TrimEnd();
                            txtApellidoM.Text = cliente.apellido_materno.TrimEnd();

                            txtCuenta.Enabled = true;
                            cmbProducto.Enabled = false;
                            txtNombre.Enabled = false;
                            txtApellidoP.Enabled = false;
                            txtApellidoM.Enabled = false;
                            TxtDepositoTkt.Enabled = true;

                        }
                        else
                        {
                            MessageBox.Show("No se pudo cargar los datos del cliente con cuenta " + txtCuenta.Text, "Error de carga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                    }
                    txtCuenta.Tag = "";
                }
            }
            if ((e.KeyChar < (char)48 || e.KeyChar > (char)57) && e.KeyChar != (char)8)
            {
                e.KeyChar = (char)0;
            }

        }

        public void TxtCuentaLostFocus(object sender, EventArgs e)
        {
            txtCuenta_KeyPress(this, new KeyPressEventArgs((char)(Keys.Enter)));

            if (frmp.consultor_selec == null)
            {
                if (txtCuenta.Text == "")
                {
                    cmbProducto.Enabled = false;
                }
                else
                {
                    cmbProducto.Enabled = true;
                }
            }
        }

        private void txtDepositoIni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < (char)48 || e.KeyChar > (char)57) && e.KeyChar != (char)8)
            {
                if (e.KeyChar == (char)46)
                {
                    if (txtDepositoIni.Text.Count(f => (f == '.')) > 0)
                    {
                        e.KeyChar = (char)0;
                    }
                }
                else
                {
                    e.KeyChar = (char)0;
                }

            }
        }

        public void TxtDepositoIniLostFocus(object sender, EventArgs e)
        {
            decimal deposito = 0;

            if(decimal.TryParse(txtDepositoIni.Text, out deposito))
            {
                txtDepositoIni.Text = deposito.ToString("C", format_mxn);
            }   
        }

        private void TxtDepositoTkt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < (char)48 || e.KeyChar > (char)57) && e.KeyChar != (char)8)
            {
                if (e.KeyChar == (char)46)
                {
                    if (TxtDepositoTkt.Text.Count(f => (f == '.')) > 0)
                    {
                        e.KeyChar = (char)0;
                    }
                }
                else
                {
                    e.KeyChar = (char)0;
                }

            }
        }

        public void TxtDepositoTktLostFocus(object sender, EventArgs e)
        {
            decimal deposito = 0;

            if(decimal.TryParse(TxtDepositoTkt.Text,out deposito))
            {
                TxtDepositoTkt.Text = deposito.ToString("C", format_mxn);
            }
            
        }

        private void dtpDesbloqueo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)0;
        }

        private void dtpEnvio_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)0;
        }

        private void dtpFAtencion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)0;
        }

        private void dtpFFormalizada_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)0;
        }

        private void dtpFRecepcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)0;
        }

        /// <summary>
        /// Limpia y resetea controles del form
        /// </summary>
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

        private void txtSolicitud_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                TraerDatos();
                if(frmp.activa == 1)
                {
                    btnLimpiar.Visible = false;
                    btnLimpiar.Enabled = false;
                }
                else
                {
                    btnLimpiar.Visible = true;
                    btnLimpiar.Enabled = true;
                }
            }

            if ((e.KeyChar < (char)48 || e.KeyChar > (char)57) && e.KeyChar != (char)8)
            {
                e.KeyChar = (char)0;
            }
        }


        private void dtpConcluir_ValueChanged(object sender, EventArgs e)
        {
            if (dtpConcluir.Value != dtpConcluir.MinDate)
            {
                DateTime fecha_servidor = DateTime.Now;
                dtpConcluir.Value = fecha_servidor;

                grpCalendario.Tag = 6;
                grpCalendario.Visible = true;
            }
        }


        public FrmNueva_Solicitud(Frm_Login frmp)
        {      
            InitializeComponent();
          
            this.frmp = frmp;
            this.bdbmtktp01 = new bmtktp01Entities();
            this.bdCatalogos = new CATALOGOSEntities();
            this.bdFuncionarios = new FUNCIONARIOSEntities();
            this.bdTickets = new TICKETEntities();

            txtNombre.Text = "Nombre";
            txtApellidoP.Text = "Primer Apellido";
            txtApellidoM.Text = "Segundo Apellido";
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

        

        public void TxtNombreLostFocus(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                txtNombre.Text = "Nombre";
                txtNombre.ForeColor = Color.LightGray;
            }
            else
            {
                txtNombre.Text = txtNombre.Text.ToUpper();
                txtNombre.ForeColor = Color.Black;
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
            else
            {
                txtApellidoP.Text = txtApellidoP.Text.ToUpper();
                txtApellidoP.ForeColor = Color.Black;
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
            else
            {
                txtApellidoM.Text = txtApellidoM.Text.ToUpper();
                txtApellidoM.ForeColor = Color.Black;
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

        

       
        

        private int LlenaDtgdwObservaciones()
        {
            try
            {
                SEGUIMIENTO observaciones = (
                       from s in bdbmtktp01.SEGUIMIENTO
                       join o in bdbmtktp01.OBSERVACIONES on s.Num_Solicitud equals o.Num_Solicitud
                       where s.Num_Solicitud == frmp.solicitud_selec.Num_Solicitud
                       select s
                   ).FirstOrDefault();

                if (observaciones == null)
                {
                    MessageBox.Show("No se pudieron cargar las observaciones para la solicitud " + frmp.solicitud_selec.Num_Solicitud, "Error de carga.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
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

                return dt.Rows.Count;


            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return -1;
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
