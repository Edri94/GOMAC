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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOMAC.Views
{
    public partial class FrmNueva_Solicitud : Form 
    {
        private Frm_PantallaPrincipal frmp;
        private bmtktp01Entities bdbmtktp01;
        private CATALOGOSEntities bdCatalogos;
        private FUNCIONARIOSEntities bdFuncionarios;
        private SEGUIMIENTO seguimiento_doc;

        private List<FUNCIONARIO> funcionarios;
        private List<UNIDAD_ORGANIZACIONAL_RESUMEN> uors;

        private string default_cmb = ". . . ";

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


        public FrmNueva_Solicitud(Frm_PantallaPrincipal frmp)
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


            //***PLACE HOLDERS********************************************************************
            txtNombre.GotFocus += new EventHandler(this.TxtNombreGotFocus);
            txtNombre.LostFocus += new EventHandler(this.TxtNombreLostFocus);

            txtApellidoP.GotFocus += new EventHandler(this.TxtApelllidoPGotFocus);
            txtApellidoP.LostFocus += new EventHandler(this.TxtApellidoPLostFocus);

            txtApellidoM.GotFocus += new EventHandler(this.TxtApelllidoMGotFocus);
            txtApellidoM.LostFocus += new EventHandler(this.TxtApellidoMLostFocus);

            //txtFRecepDoc.GotFocus += new EventHandler(this.TxtFRecepDocGotFocus);
            //txtFRecepDoc.LostFocus += new EventHandler(this.TxtFRecepDocLostFocus);
            //************************************************************************************


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



            if (str_consultor.Trim() != "")
            {
                CargaConsulta();

                if (lblStatus.Text == "Nueva")
                {
                    MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }


            cmbProducto.Enabled = false;
            cmbBanca.Enabled = false;
            cmbDivision.Enabled = false;
            cmbPlaza.Enabled = false;
            cmbSucursal.Enabled = false;
            lblStatus.Text = "Nueva";
            this.Text = "Captura de solicitud";

            if(btnLimpiar.Text == "Limpiar")
            {
                btnLimpiar.Enabled = false;
            }

            if(lblStatus.Text == "En proceso")
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
            else
            {
                if(lblStatus.Text == "Nueva")
                {                
                    MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Warning);                  
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

            for(int i = 1; i <= 12; i++)
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

                    txtCuenta.Text = seguimiento_doc.Cuenta_Cliente;


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


                    //Datos Funcionario
                    cmbNumeroFuncionario.SelectedText = seguimiento_doc.Numero_Registro;
                    cmbBanca.SelectedText = seguimiento_doc.Banca;
                    cmbDivision.SelectedText = seguimiento_doc.Division;
                    cmbPlaza.SelectedText = seguimiento_doc.Plaza;
                    cmbSucursal.SelectedText = seguimiento_doc.Sucursal;



                    //Dato Seguimiento Documento
                    if(seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.HasValue)
                    {
                        if (seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value == DateTime.Parse("01-01-1900"))
                        {
                            dtpFRecepDoc.Text = "";
                            cmbHora1.SelectedText = "00";
                            cmbMinuto1.SelectedText = "00";
                        }
                        else
                        {
                            dtpFRecepDoc.Value = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value;
                            cmbHora1.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value.ToString("hh");
                            cmbMinuto1.SelectedText = seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Doc.Value.ToString("mm");

                            if (lblStatus.Text == "En proceso")
                            {

                            }
                        }
                    }
                   

                    //Analisis_Mac
                    if(seguimiento_doc.SEGUIMIENTO_DOCTOS.Analisis_Mac.HasValue)
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

                            if (lblStatus.Text == "En proceso" && cmbTipoSolicitud.SelectedIndex != 3)
                            {

                            }
                            else if (lblStatus.Text == "En proceso" && cmbTipoSolicitud.SelectedIndex == 3)
                            {

                            }
                        }
                    }
                    


                    //Formalizada
                    if(seguimiento_doc.SEGUIMIENTO_DOCTOS.Formalizada.HasValue)
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

                            if (lblStatus.Text == "En proceso")
                            {

                            }
                        }
                    }
                    


                    //Repc_Originales
                    if(seguimiento_doc.SEGUIMIENTO_DOCTOS.Repc_Originales.HasValue)
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

                            if (lblStatus.Text == "En proceso")
                            {

                            }
                        }
                    }



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

                            if (lblStatus.Text == "En proceso")
                            {

                            }
                        }
                    }
                    


                    if(dtpFAtencion.Text.Trim() =="")
                    {

                    }
                    else
                    {

                    }



                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if(str_consultor == "")
            {
                if(cmbConsultorMac.Text == default_cmb)
                {
                    MessageBox.Show("Debe seleccionar su consultor", "Error Solicitud", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                string query = "";
                int i = 0;
                string str_time;
                string str_docuemntos;


                if(ValidaCampos())
                {
                   if(btnGuardar.Text == "Guardar")
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
                            string circuito = (rbCircuitoAuto.Checked)? "A": "M";
                            string cuenta_Cliente = txtCuenta.Text;
                            string sufijo_Kapiti = cmbProducto.Text;
                            byte tipo_Persona = (byte)((rbPersonaFisica.Checked) ? 0 : 1);
                            string nombre_Cliente = txtNombre.Text;
                            string apellido_Paterno = txtApellidoP.Text;
                            string apellido_Materno = txtApellidoM.Text;
                            string deposito_Inicial = Int32.Parse(TxtDepositoTkt.Text).ToString("C", frmp.format);
                            string numero_Registro = cmbNumeroFuncionario.Text;
                            string nombre_Promotor = cmbPromotor.Text;
                            string banca = cmbBanca.Text;
                            string division = cmbDivision.Text;
                            string plaza = cmbPlaza.Text;
                            string sucursal = cmbSucursal.Text;
                            string status = "1";
                            int num_Solicitud = Int32.Parse(txtSolicitud.Text);
                            string fechaRepc_Doc = dtpFRecepDoc.Value.ToString("dd-MM-yyyy");
                            string horaRepc_Doc = cmbHora1.Text + ":" + cmbMinuto1.Text;
                            string fechaAnalisis_Mac = dtpFAnalisisMac.Value.ToString("dd-MM-yyyy");
                            string horaAnalisis_Mac = cmbHora2.Text + ":" + cmbMinuto2.Text;
                            string fechaFormalizada = dtpFFormalizada.Value.ToString("dd-MM-yyyy");
                            string horaFormalizada = cmbHora3.Text + ":" + cmbMinuto3.Text;
                            string fechaRepc_Originales = dtpFRecepcion.Value.ToString("dd-MM-yyyy");
                            string horaRepc_Originales = cmbHora4.Text + ":" + cmbMinuto4.Text;
                            string fechaAten_Originales = rbExisteCuentaSi.Checked ? "S" : "N";
                            string horaAten_Originales = cmbHora5.Text + ":" + cmbMinuto5.Text;
                            string originales = "-1";
                            decimal deposito_Inicial_Ini = decimal.Parse(txtDepositoIni.Text);
                            string fecha_Desbloqueo = dtpDesbloqueo.Value.ToString("dd-MM-yyyy");
                            string fecha_Envio = dtpEnvio.Value.ToString("dd-MM-yyyy");
                            string fecha_concluida = dtpConcluir.Value.ToString("dd-MM-yyyy");
                            string existeTKT = rbExisteCuentaSi.Checked ? "S" : "N";

                            bdbmtktp01.Mac_Inserta_Datos(
                                id_ConsultorMac,
                                id_Solicitud ,
                                id_Tramite,
                                puntos,
                                circuito,
                                cuenta_Cliente, 
                                sufijo_Kapiti ,
                                tipo_Persona ,
                                nombre_Cliente,
                                apellido_Paterno, 
                                apellido_Materno,
                                deposito_Inicial ,
                                numero_Registro ,
                                nombre_Promotor,
                                banca ,
                                division, 
                                plaza ,
                                sucursal, 
                                status ,
                                 num_Solicitud,
                                 fechaRepc_Doc,
                                 horaRepc_Doc ,
                                 fechaAnalisis_Mac, 
                                 horaAnalisis_Mac,
                                 fechaFormalizada ,
                                 horaFormalizada ,
                                 fechaRepc_Originales, 
                                 horaRepc_Originales,
                                 fechaAten_Originales, 
                                 horaAten_Originales,
                                 originales ,
                                 deposito_Inicial_Ini, 
                                 fecha_Desbloqueo ,
                                 fecha_Envio,
                                 fecha_concluida,
                                 existeTKT 
                            );

                        }
                    }
                }
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
