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

            //txtFRecepDoc.Text = "Expediente Unico";
            //txtFAnalisisMac.Text = "Analisis MAC";
            //txtFFormalizada.Text = "Formalizada";
            //txtFRecepcion.Text = "Recepcion de Originales";
            //txtFAtencion.Text = "Atencion de Originales";

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
                    tipos_tramite.Insert(0, new ver_Tipo_Tramite { Descripcion_Tramite = "" });
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
                    tipos_solicitud.Insert(0, new ver_Tipo_Solicitud { Descripcion_Solicitud= "" });
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
                if(cmbConsultorMac.Text == "...")
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

        private bool ValidaCampos()
        {
            try
            {
                if(cmbConsultorMac.SelectedIndex <= -1)
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
                if(txtCuenta.Text == "")
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
