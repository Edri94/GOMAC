using GOMAC.Helpers;
using GOMAC.Models;
using GOMAC.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        public ver_usuarios2 usuario_loggeado;
        public NumberFormatInfo format;

        private Frm_NuevaSolicitud frm_nuevasolicitud;
        private Frm_ConsultaSolicitud frm_consultasolicitud;
        

        public Frm_PantallaPrincipal(ver_usuarios2 usuario)
        {
            InitializeComponent();

            this.usuario_loggeado = usuario;
            format = (NumberFormatInfo)CultureInfo.CreateSpecificCulture("es-MX").NumberFormat.Clone();
        }
        private void PantallaPrincipal_Load(object sender, EventArgs e)
        {

            bdbmtktp01 = new bmtktp01Entities();

            tmtHora.Enabled = true;

            inact.Interval = 1000 * tiempo_espera;

            Valida();


        }

        private void consultaSolicitudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usuario_loggeado != null)
            {
                if (frm_consultasolicitud != null)
                {
                    frm_consultasolicitud.Close();
                    frm_consultasolicitud = null;
                }

                frm_consultasolicitud = new Frm_ConsultaSolicitud(this);

                frm_consultasolicitud.MdiParent = this;
                frm_consultasolicitud.Show();
            }

        }

        private void nuevaSolicitudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (usuario_loggeado != null)
            {
                if (frm_nuevasolicitud != null)
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


        private void Valida()
        {
            try
            {
                //El codigo aqui manejara los permisos para mostrar u ocultar menus dependiendo el usario/rol
                if(VerPerfilSector() != null) 
                {

                }


            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private List<ver_perfil_sector> VerPerfilSector()
        {
            try
            {
                perfiles_sector = (from p in bdbmtktp01.ver_perfil_sector select p).ToList();
                return perfiles_sector;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return perfiles_sector;
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
