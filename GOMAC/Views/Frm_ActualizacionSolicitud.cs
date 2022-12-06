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
    public partial class Frm_ActualizacionSolicitud : Form
    {
        private Frm_Login frmp;
        private bmtktp01Entities bdbmtktp01;
        private bool okconcluida, okproceso, okcancelada;
        public Frm_ActualizacionSolicitud(Frm_Login frmp)
        {
            InitializeComponent();

            this.frmp = frmp;
            this.bdbmtktp01 = new bmtktp01Entities();
        }

        private void Frm_ActualizacionSolicitud_Load(object sender, EventArgs e)
        {
            //btnBuscar.Enabled = false;
            btnActualizar.Enabled = false;
            txtCuenta.Enabled = false;

            txtApePat.Enabled = false;
            txtApeMat.Enabled = false;
            txtNombre.Enabled = false;

            lblStatus.Text = "";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarSolicitud();
        }

        private void BuscarSolicitud()
        {
            try
            {
                txtCuenta.Enabled = true;
                txtApePat.Enabled = true;
                txtApeMat.Enabled = true;
                txtNombre.Enabled = true;
                btnActualizar.Enabled = false;
                txtCuenta.Text = String.Empty;
                txtNombre.Text = String.Empty;
                txtApePat.Text = String.Empty;
                txtApeMat.Text = String.Empty;

                if(txtIdSolicitud.Text.Trim() == "")
                {
                    MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    int numero_solicitud = (int.TryParse(txtIdSolicitud.Text, out numero_solicitud)) ? numero_solicitud : -1;

                    if(numero_solicitud > 0)
                    {
                        SEGUIMIENTO solicitud = (
                        from s in bdbmtktp01.SEGUIMIENTO
                        where s.Num_Solicitud == numero_solicitud
                        select s
                    ).FirstOrDefault();

                        if (solicitud != null)
                        {
                            txtCuenta.Text = solicitud.Cuenta_Cliente;
                            txtNombre.Text = solicitud.Nombre_Cliente;
                            txtApePat.Text = solicitud.Apellido_Paterno;
                            txtApeMat.Text = solicitud.Apellido_Materno;

                            int id_status = (int.TryParse(solicitud.Status, out id_status)) ? id_status : -1;

                            if(id_status > 0)
                            {
                                TIPO_STATUS status = (from ts in bdbmtktp01.TIPO_STATUS where ts.Id_Status == id_status select ts).FirstOrDefault();

                                if (status != null)
                                {
                                    lblStatus.Text = status.Descripcion_Status;
                                }
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
    }
}
