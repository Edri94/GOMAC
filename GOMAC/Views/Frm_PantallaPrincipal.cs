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

        //public bool loggeado;
        //public int sr, tiempo_servicioa, tiempo_serviciom, tiempo_atencion, tiempo_espera, cierre_tiempo, meses_sin_conectar;
        //public string str_srv, str_bd, str_usr, str_pwd, str_provider;
        //public ver_usuarios2 usuario_loggeado;
        public bmtktp01Entities bdbmtktp01;
        public List<ver_perfil_sector> perfiles_sector;
        public NumberFormatInfo format;

        private Frm_Login frml;
        private PantallaCarga frm_nuevasolicitud;
        private Frm_ConsultaSolicitud frm_consultasolicitud;
        private Frm_ActualizacionSolicitud frm_actualizacionsolicitud;
        

        public Frm_PantallaPrincipal(Frm_Login frml)
        {
            InitializeComponent(); 
            format = (NumberFormatInfo)CultureInfo.CreateSpecificCulture("es-MX").NumberFormat.Clone();
            this.frml = frml;
        }
        private void PantallaPrincipal_Load(object sender, EventArgs e)
        {

            bdbmtktp01 = new bmtktp01Entities();

            tmtHora.Enabled = true;

            inact.Interval = 1000 * frml.tiempo_espera;

            Valida();


        }

        private void consultaSolicitudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frml.usuario_loggeado != null)
            {
                if (frm_consultasolicitud != null)
                {
                    frm_consultasolicitud.Close();
                    frm_consultasolicitud = null;
                }

                frm_consultasolicitud = new Frm_ConsultaSolicitud(frml, this);

                frm_consultasolicitud.MdiParent = this;
                frm_consultasolicitud.Show();
            }

        }

        private void nuevaSolicitudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frml.usuario_loggeado != null)
            {
                if (frm_nuevasolicitud != null)
                {
                    frm_nuevasolicitud.Close();
                    frm_nuevasolicitud = null;
                }

                frm_nuevasolicitud = new PantallaCarga(frml, this);

                //frm_nuevasolicitud.MdiParent = this;
                frml.consultor_selec = null;
                frm_nuevasolicitud.Height = this.Height - 33;
                frm_nuevasolicitud.Width = this.Width;
                frm_nuevasolicitud.ShowDialog();   
            }
        }


        private void Valida()
        {
            try
            {
                //El codigo aqui manejara los permisos para mostrar u ocultar menus dependiendo el usario/rol
                if(VerPerfilSector() != null) 
                {
                    frml.activa = 1;
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

        private void actualizarSolicitudToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frml.usuario_loggeado != null)
            {
                if (frm_actualizacionsolicitud != null)
                {
                    frm_actualizacionsolicitud.Close();
                    frm_actualizacionsolicitud = null;
                }

                frm_actualizacionsolicitud = new Frm_ActualizacionSolicitud(frml);

                frm_actualizacionsolicitud.MdiParent = this;
                //frm_actualizacionsolicitud.str_consultor = "";
                frm_actualizacionsolicitud.Show();
            }
        }
    }
}
