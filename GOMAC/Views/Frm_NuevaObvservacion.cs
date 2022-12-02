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
    public partial class Frm_NuevaObvservacion : Form
    {
        private FrmNueva_Solicitud frmp;
        public string observacion = "";

        public Frm_NuevaObvservacion(FrmNueva_Solicitud frmp)
        {
            InitializeComponent();

            this.frmp = frmp;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            observacion = txtCapturaObserv.Text;
            this.Close();
        }

        private void Frm_NuevaObvservacion_Load(object sender, EventArgs e)
        {

        }
    }
}
