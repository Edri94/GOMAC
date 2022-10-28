using GOMAC.Helpers;
using GOMAC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOMAC.Views
{
    public partial class Frm_NuevaSolicitud : Form
    {
        private Frm_PantallaPrincipal frmp;
        private bmtktp01Entities bdbmtktp01;
        private SEGUIMIENTO seguimiento_doc;

        public string str_consultor;
        public int se_carga;
        public int num_solicitud;

        public Frm_NuevaSolicitud(Frm_PantallaPrincipal frmp)
        {
            InitializeComponent();

            this.frmp = frmp;
            this.bdbmtktp01 = new bmtktp01Entities();

        txtNombre.Text = "Nombre";
            txtApellidoP.Text = "Primer Apellido";
            txtApellidoM.Text = "Segundo Apellido";

            txtFRecepDoc.Text = "Expediente Unico";
            txtFAnalisisMac.Text = "Analisis MAC";
            txtFFormalizada.Text = "Formalizada";
            txtFRecepcion.Text = "Recepcion de Originales";
            txtFAtencion.Text = "Atencion de Originales";

        }

        private void Frm_NuevaSolicitud_Load(object sender, EventArgs e)
        {

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

            LlenaComboTipoSolicitud();
            LlenaComboTipoTramite();

            if(str_consultor.Trim() != "")
            {
                CargaConsulta();
            }




            if (str_consultor.Trim() != "")
            {
                if(lblStatus.Text == "Nueva")
                {
                    switch (se_carga)
                    {
                        case 0:
                            CargaConsulta();
                            se_carga = 1;
                            break;

                        default:
                            MessageBox.Show("!!!  No se pudo cargar la informacion, intente nueva mente.  ¡¡¡", "Error de visualizacion de informacion.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        
                    }
                }
            }






        }

        private void LlenaComboTipoTramite()
        {
            try
            {
                List<ver_Tipo_Tramite> tipos_tramite =
                    (from tt in bdbmtktp01.ver_Tipo_Tramite orderby tt.Id_Tramite ascending select tt).ToList();

                if (tipos_tramite != null)
                {
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
                List<ver_Tipo_Solicitud> tipos_solicitud = 
                    (from ts in bdbmtktp01.ver_Tipo_Solicitud orderby ts.Id_Solicitud ascending select ts).ToList();

                if(tipos_solicitud != null)
                {
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

                if(VerSeguimientoDoc(Int32.Parse(txtSolicitud.Text)) != null)
                {
                    //Datos Cuenta
                    dtpFechaCaptura.Value = (seguimiento_doc.Fecha_Captura.HasValue)? seguimiento_doc.Fecha_Captura.Value : dtpFechaCaptura.MinDate;

                    cmbConsultorMac.SelectedIndex = (seguimiento_doc.Id_ConsultorMac.HasValue) ? seguimiento_doc.Id_ConsultorMac.Value : -1;

                    cmbTipoSolicitud.SelectedIndex = (seguimiento_doc.Id_Solicitud.HasValue)? seguimiento_doc.Id_Solicitud.Value : -1;


                    if(seguimiento_doc.ExisteTKT.ToUpper() == "S")
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

                            if(txtFRecepDoc.Text == "")
                            {
                                btnFRecepDoc.Visible = true;
                                btnFRecepDoc.Enabled = true;
                            }

                            if(txtFAnalisisMac.Text == "")
                            {
                                btnFAnalisisMac.Visible = true;
                                btnFAnalisisMac.Enabled = true;

                            }

                            if (txtFFormalizada.Text == "")
                            {
                                btnFFormalizada.Visible = true;
                                btnFFormalizada.Enabled = true;
                            }

                            if (txtFRecepcion.Text == "")
                            {
                                btnFRecepcion.Visible = true;
                                btnFRecepcion.Enabled = true;
                            }

                            if (txtFAtencion.Text == "")
                            {
                                btnFAtencion.Visible = true;
                                btnFAtencion.Enabled = true;
                            }


                            if(txtDesbloqueo.Text == "")
                            {
                                btnDesbloqueo.Visible = true;
                                btnDesbloqueo.Enabled = true;
                            }

                            if(txtEnvio.Text == "")
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

                    txtPuntos.Text = (seguimiento_doc.Puntos.HasValue)? seguimiento_doc.Puntos.Value.ToString() : "";

                    if(seguimiento_doc.Circuito == "A")
                    {
                        if(rbCircuitoAuto.Enabled)
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


                    if(seguimiento_doc.Tipo_Persona == 0)
                    {
                        //PERSONA FISICA
                        if(rbPersonaFisica.Enabled == true)
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


        //public void TxtFRecepDocGotFocus(object sender, EventArgs e)
        //{
        //    if (txtFRecepDoc.Text == "")
        //    {
        //        txtFRecepDoc.Text = "Expediente Unico";
        //        txtFRecepDoc.ForeColor = Color.LightGray;
        //    }
        //}

        //public void TxtFRecepDocLostFocus(object sender, EventArgs e)
        //{
        //    if (txtFRecepDoc.Text == "Expediente Unico")
        //    {
        //        txtFRecepDoc.Text = "";
        //        txtFRecepDoc.ForeColor = Color.Black;
        //    }

        //}
    }
}
