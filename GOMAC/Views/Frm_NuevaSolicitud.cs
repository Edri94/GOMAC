using GOMAC.Helpers;
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
        
        public string str_consultor;
        public int se_carga;
        public int num_solicitud;

        public Frm_NuevaSolicitud(Frm_PantallaPrincipal frmp)
        {
            InitializeComponent();

            this.frmp = frmp;

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

            if(str_consultor.Trim() != "")
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

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
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
