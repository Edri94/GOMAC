using GOMAC.Helpers;
using GOMAC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOMAC.Views
{
    public partial class Frm_ActualizacionSolicitud : Form
    {
        private Frm_Login frmp;
        private bmtktp01Entities bdbmtktp01;
        private bool okconcluida = false, okproceso = false, okcancelada = false;
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

        private void txtIdSolicitud_KeyPress(object sender, KeyPressEventArgs e)
        {          
            if(! new Regex("^[0-9 \b]+$").Match(e.KeyChar.ToString()).Success)
            {
                e.KeyChar = (char)0;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            int num_solicitud = 0;

            if(int.TryParse(txtIdSolicitud.Text, out num_solicitud))
            {
                btnBuscar.Enabled = false;
                txtIdSolicitud.Enabled = false;

                if (MessageBox.Show($"Va a cambiar el status de la solicitud " + txtIdSolicitud.Text + ", ¿Está seguro que desea efectuar el cambio de status?", "Cambio de status", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    txtIdSolicitud.Enabled = true;

                    ActualixarSolicitud(num_solicitud);

                    if (MessageBox.Show($"¿Desea buscar otra solicitud?", "Nueva busqueda", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        txtIdSolicitud.Enabled = true;
                        txtIdSolicitud.Text = "";
                        txtCuenta.Text = "";
                        txtNombre.Text = "";
                        txtApePat.Text = "";
                        txtApeMat.Text = "";
                        lblStatus.Text = "";

                        txtCuenta.Enabled = false;
                        txtNombre.Enabled = false;
                        txtApePat.Enabled = false;
                        txtApeMat.Enabled = false;
                        btnActualizar.Enabled = false;
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Ha cancelado el cambio de status para la solicitud" + txtIdSolicitud.Text, "Cancelación de cambio de status", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    txtIdSolicitud.Text = "";
                    txtCuenta.Text = "";
                    txtNombre.Text = "";
                    txtApePat.Text = "";
                    txtApeMat.Text = "";
                    lblStatus.Text = "";

                    txtCuenta.Enabled = false;
                    txtNombre.Enabled = false;
                    txtApePat.Enabled = false;
                    txtApeMat.Enabled = false;
                    txtIdSolicitud.Enabled = true;

                    btnActualizar.Enabled = false;
                    btnBuscar.Enabled = false;
                }
            }
            
        }

        private void ActualixarSolicitud(int num_solicitud)
        {

            using (var context = new bmtktp01Entities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        SEGUIMIENTO solicitud = (from s in context.SEGUIMIENTO where s.Num_Solicitud == num_solicitud select s).FirstOrDefault();

                        if (solicitud != null)
                        {
                            solicitud.Status = "1";
                            context.SEGUIMIENTO.Add(solicitud);
                            context.SaveChanges();

                            SEGUIMIENTO_DOCTOS seguimiento_doc = (from sd in context.SEGUIMIENTO_DOCTOS where sd.Num_Solicitud == num_solicitud select sd).FirstOrDefault();

                            if (seguimiento_doc != null)
                            {
                                seguimiento_doc.Concluida = null;
                                seguimiento_doc.Cancelacion = null;
                                context.SEGUIMIENTO_DOCTOS.Add(seguimiento_doc);
                                context.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        Log.Escribe(ex);
                    }
                }
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
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

                                    switch (status.Id_Status)
                                    {
                                        case 1:
                                            okproceso = true;
                                            break;
                                        case 2:
                                            okconcluida = true;
                                            break;
                                        case 3:
                                            okcancelada = true;
                                            break;                                    
                                    }

                                    if (okcancelada == true || okconcluida == true)
                                    {
                                        MessageBox.Show("El status de la solicitud" + txtIdSolicitud.Text + " se cambiara al estado en proceso", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        btnActualizar.Enabled = true;
                                    }

                                    if(okproceso)
                                    {
                                        txtCuenta.Text = "";
                                        txtApePat.Text = "";
                                        txtApeMat.Text = "";
                                        txtNombre.Text = "";
                                        lblStatus.Text = "";

                                        txtCuenta.Enabled = false;
                                        txtApePat.Enabled = false;
                                        txtApeMat.Enabled = false;
                                        txtNombre.Enabled = false;

                                        MessageBox.Show("La solicitud ya se encuentra en proceso, no se puede cambiar su status", "Status en proceso", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        txtIdSolicitud.Text = "";
                                        btnActualizar.Enabled = false;

                                        return;
                                    }
                                }

                            }

                        }
                    }                 
                }

                txtCuenta.Enabled = false;
                txtApePat.Enabled = false;
                txtApeMat.Enabled = false;
                txtNombre.Enabled = false;

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }
    }
}
