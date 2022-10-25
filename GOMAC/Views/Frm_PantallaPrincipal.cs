using GOMAC.Views;
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
    public partial class Frm_PantallaPrincipal : Form
    {

        public bool loggeado;

        Frm_NuevaSolicitud frm_nuevasolicitud;

        public Frm_PantallaPrincipal()
        {
            InitializeComponent();
        }

        private void PantallaPrincipal_Load(object sender, EventArgs e)
        {
            loggeado = true;
        }

        private void nuevaSolicitudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(loggeado)
            {
                if(frm_nuevasolicitud != null)
                {
                    frm_nuevasolicitud.Close();
                    frm_nuevasolicitud = null;
                }
               
                frm_nuevasolicitud = new Frm_NuevaSolicitud(this);

                frm_nuevasolicitud.MdiParent = this;
                frm_nuevasolicitud.Show();
            }

            
        }
    }
}
