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
    public partial class Frm_VistaObvservaciones : Form
    {
        public string observaciones;
        public Frm_VistaObvservaciones()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            observaciones = String.Empty;
        }

        private void Frm_VistaObvservaciones_Load(object sender, EventArgs e)
        {         
            txtObservaciones.Text = observaciones;
            txtObservaciones.ReadOnly = true;
        }
    }
}
