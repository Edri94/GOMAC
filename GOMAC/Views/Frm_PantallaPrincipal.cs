using GOMAC.Helpers;
using GOMAC.Models;
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
        public int sr, tiempo_servicioa, tiempo_serviciom, tiempo_atencion, tiempo_espera, cierre_tiempo, meses_sin_conectar;
        public string str_srv, str_bd, str_usr, str_pwd, str_provider;
        public bmtktp01Entities bdbmtktp01;
        public List<ver_perfil_sector> perfiles_sector;

        Frm_NuevaSolicitud frm_nuevasolicitud;

        public Frm_PantallaPrincipal()
        {
            InitializeComponent();
        }

        private void PantallaPrincipal_Load(object sender, EventArgs e)
        {

            bdbmtktp01 = new bmtktp01Entities();

            loggeado = true; //[PRUEBAS]

            tmtHora.Enabled = true;

            inact.Interval = 1000 * tiempo_espera;

            Valida();


        }

        private void Valida()
        {
            try
            {
                VerPerfilSector();


            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void VerPerfilSector()
        {
            try
            {
                perfiles_sector = (from p in bdbmtktp01.ver_perfil_sector select p).ToList();
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
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
                frm_nuevasolicitud.str_consultor = "";
                frm_nuevasolicitud.Show();
            }

            
        }

        private void inact_Tick(object sender, EventArgs e)
        {

        }

        private void tmtHora_Tick(object sender, EventArgs e)
        {

        }
    }
}
