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
    public partial class Frm_Login : Form
    {       
        public int sr, tiempo_servicioa, tiempo_serviciom, tiempo_atencion, tiempo_espera, cierre_tiempo, meses_sin_conectar;
        public int activa;
        public string str_srv, str_bd, str_usr, str_pwd, str_provider;
        public CONSULTORES consultor_selec;
        public SEGUIMIENTO solicitud_selec;
        public USUARIO usuario;

        public List<ver_perfil_sector> perfiles_sector;
        public List<ver_usuarios2> usuarios;
        public List<ver_sectores> sectores;
        public List<ver_perfiles> perfiles;
        public List<ver_consultores> consultores;
        public ver_usuarios2 usuario_loggeado;

        private bmtktp01Entities bdbmtktp01;
        private bool loggeado;
        private Encriptacion crpt;
        private string[] sect2 = new string[6];

        private void txtUser_KeyUp(object sender, KeyEventArgs e)
        {
            if(txtUser.Text != "" && txtPassword.Text != "")
            {
                btnConectar.Enabled = true;
            }
            else
            {
                btnConectar.Enabled = false;
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtUser.Text != "" && txtPassword.Text != "")
            {
                btnConectar.Enabled = true;
            }
            else
            {
                btnConectar.Enabled = false;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public Frm_Login()
        {
            InitializeComponent();
            crpt = new Encriptacion();
            bdbmtktp01 = new bmtktp01Entities();
        }

        private void Frm_Login_Load(object sender, EventArgs e)
        {
            SetSect2();
            btnConectar.Enabled = false;

            //generarlog();
            //verificadll();
            //instancia();

            lblFecha.Text = DateTime.Now.ToString("dd-MM-yyyy");

            //[PRUEBAS]
            txtUser.Text = "MI05332";
            txtPassword.Text = "Edri3010";

        }

        private void SetSect2()
        {
            sect2[0] = "admin_per";
            sect2[1] = "admin_rep";
            sect2[2] = "admin_usu";
            sect2[3] = "actu_sol";
            sect2[4] = "nuev_sol";
            sect2[5] = "bus_sol";
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            InicializaVariables();

            Cargar();
        }

        private void Cargar()
        {
            try
            {
                if(txtUser.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Llenar campo de usuario y password", "Llenar informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //llenamos los colectores correspondientes
                else
                {
                    if(ObtenUsuarios() == null || ObtenSectores() == null || ObtenPerfiles() == null || ObtenConsultores() == null)
                    {
                        txtUser.Text = "";
                        txtPassword.Text = "";
                    }
                    else
                    {
                        usuario_loggeado = usuarios.Where(x => x.login == txtUser.Text).FirstOrDefault();

                        if(usuario_loggeado != null)
                        {                         
                            if(crpt.Decrypt(usuario_loggeado.pwd) != txtPassword.Text)
                            {
                                MessageBox.Show("La contrasena es incorrecta", "Error de Credenciales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                loggeado = true;
                                Frm_PantallaPrincipal frm = new Frm_PantallaPrincipal(this);
                                this.Hide();
                                frm.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("El usuario no existe", "Error de Credenciales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }




                    }
                 
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private object ObtenConsultores()
        {
            try
            {
                consultores = (from c in bdbmtktp01.ver_consultores select c).ToList();
                return consultores;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return consultores;
            }
        }

        private List<ver_usuarios2> ObtenUsuarios()
        {
            try
            {
                usuarios = (from u in bdbmtktp01.ver_usuarios2 select u).ToList();
                return usuarios;
            }
            catch (Exception ex )
            {
                Log.Escribe(ex);
                return usuarios;
            }
        }


        private List<ver_sectores> ObtenSectores()
        {
            try
            {
                sectores = (from s in bdbmtktp01.ver_sectores select s).ToList();
                return sectores;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);

                return sectores;
            }
        }


        private List<ver_perfiles> ObtenPerfiles()
        {
            try
            {
                perfiles = (from p in bdbmtktp01.ver_perfiles select p).ToList();
                return perfiles;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return perfiles;
            }
        }

        private void InicializaVariables()
        {
            try
            {
                sr = Int32.Parse(Funcion.getValueAppConfig("Habilitado", "Conexion-gomac"));
                if (sr == 1)
                {
                    str_srv = (string)Funcion.getValueAppConfig("Servidor", "Conexion-gomac");
                }
                else
                {
                    str_srv = (string)Funcion.getValueAppConfig("Servidor2", "Conexion-gomac");
                }

                tiempo_servicioa = Int32.Parse(Funcion.getValueAppConfig("ServicioA", "Tiempos-gomac"));
                tiempo_serviciom = Int32.Parse(Funcion.getValueAppConfig("ServicioM", "Tiempos-gomac"));
                tiempo_atencion = Int32.Parse(Funcion.getValueAppConfig("Atencion", "Tiempos-gomac"));
                tiempo_espera = Int32.Parse(Funcion.getValueAppConfig("Cierre", "Tiempos-gomac"));
                cierre_tiempo = Int32.Parse(Funcion.getValueAppConfig("Autocierre", "Tiempos-gomac"));
                meses_sin_conectar = Int32.Parse(Funcion.getValueAppConfig("Mesessinconectar", "Tiempos-gomac"));

                //Obsoleto por usar EntityFramework
                str_bd = (string)Funcion.getValueAppConfig("Base", "Conexion-gomac");
                str_usr = (string)Funcion.getValueAppConfig("Usuario", "Conexion-gomac");
                str_pwd = (string)Funcion.getValueAppConfig("Password", "Conexion-gomac");
                str_provider = (string)Funcion.getValueAppConfig("Provider", "Conexion-gomac");


            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }
    }
}
