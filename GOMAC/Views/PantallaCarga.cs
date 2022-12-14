using GOMAC.Data;
using GOMAC.Models;
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
    public partial class PantallaCarga : Form
    {
        public int se_carga;
        public List<OBSERVACIONES> lst_observaciones;

        private Frm_Login frml;
        private Frm_PantallaPrincipal frmp;

        private bmtktp01Entities bdbmtktp01;
        private CATALOGOSEntities bdCatalogos;
        private FUNCIONARIOSEntities bdFuncionarios;
        private TICKETEntities bdTickets;
        private FuncionesBdbmtktp01 bdFuncBmtktp01;

        public List<FUNCIONARIO> funcionarios;
        public List<UNIDAD_ORGANIZACIONAL_RESUMEN> uors;
        public SEGUIMIENTO seguimiento;
        public SEGUIMIENTO_DOCTOS seguimiento_doc;
        public List<PRODUCTOS> productos;
        public List<CONSULTORES> consultores;

        private string default_cmb = ". . . ";
        private DateTime default_dtp = DateTimePicker.MinimumDateTime;
        private NumberFormatInfo format_mxn = (NumberFormatInfo)CultureInfo.CreateSpecificCulture("es-MX").NumberFormat.Clone();
        private DataTable dt_observaciones;
        private int intTab;
        private int TiempoServicioA, TiempoServicioM, TiempoAtencion;
        private bool cmbNumeroFuncionario_activo = false, cmbConsultorMac_activo = false, cmbProducto_activo = false, cmbTipoSolicitud_activo = false, cmbTipoTramite_activo = false;

        private void bckWrkConsultas_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cargando(false);
        }

        private void bckWrkConsultas_DoWork(object sender, DoWorkEventArgs e)
        {

            Cargando(true);

            //******************Consulta Funcionarios
            funcionarios = (
               from f in bdFuncionarios.FUNCIONARIO
               join uor in bdFuncionarios.UNIDAD_ORGANIZACIONAL_RESUMEN on f.funcionario1 equals uor.funcionario
               select f
             ).ToList();

            funcionarios.Insert(0, new FUNCIONARIO
            {
                funcionario1 = -1,
                nombre_funcionario = ".",
                apellido_paterno = ".",
                apellido_materno = ".",
                numero_funcionario = default_cmb
            });


            ////******************Consulta Unidades Organizacionales Resumen
            uors = (
                from uor in bdFuncionarios.UNIDAD_ORGANIZACIONAL_RESUMEN
                join f in bdFuncionarios.FUNCIONARIO on uor.funcionario equals f.funcionario1
                select uor
            ).ToList();

            uors.Insert(0, new UNIDAD_ORGANIZACIONAL_RESUMEN
            {
                banca = ". . .  ",
                plaza = ". . .  ",
                division = ". . .  ",
                sucursal = ". . .  "
            });

            ////******************Consulta Productos
            productos = bdCatalogos.PRODUCTOS.OrderBy(o => o.Producto).ToList();
            productos.Insert(0, new PRODUCTOS { Producto = ". . .  " });

            ////******************Consulta Consultores
            consultores = bdbmtktp01.CONSULTORES.OrderBy(o => o.Iniciales_ConsultorMac).ToList();
            consultores.Insert(0, new CONSULTORES { Iniciales_ConsultorMac = default_cmb });


            this.Invoke(new MethodInvoker(delegate {

          

                tmrTraerDatos.Enabled = false;
                FrmNueva_Solicitud frm = new FrmNueva_Solicitud(frml, this);
                frm.MdiParent = frmp;
                frm.Show();
                this.Close();
                

            }));

          
        }

        private int segundos = 0;

        private void tmrTraerDatos_Tick(object sender, EventArgs e)
        {
            segundos += 100;

            this.Opacity += 0.05;

            if (segundos >= 3000)
            {
                this.Opacity = 1;

                if (!bckWrkConsultas.IsBusy)
                {
                    bckWrkConsultas.RunWorkerAsync();
                }
            }
        }

        private void PantallaCarga_Shown(object sender, EventArgs e)
        {
           
        }

        public PantallaCarga(Frm_Login frml, Frm_PantallaPrincipal frmp)
        {
            InitializeComponent();

            tmrTraerDatos.Enabled = true;
            
            bdbmtktp01 = new bmtktp01Entities();
            bdCatalogos = new CATALOGOSEntities();
            bdFuncionarios = new FUNCIONARIOSEntities();
            bdTickets = new TICKETEntities();
            bdFuncBmtktp01 = new FuncionesBdbmtktp01();

            this.frml = frml;
            this.frmp = frmp;
        }

        private void PantallaCarga_Load(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;

            //un poco de matematicas, restando los anchos y dividiendo entre 2
            x = (this.Width / 2) - (loading.Width / 2);
            y = (this.Height / 2) - (loading.Height / 2);

            //asignamos la nueva ubicación
            loading.Location = new System.Drawing.Point(x, y);


        }

        /// <summary>
        /// Mostrar Pantalla Carga
        /// </summary>
        /// <param name="cargando">esta cargando?</param>
        private void Cargando(bool cargando)
        {
            if (loading.InvokeRequired)
            {
                loading.Invoke(new MethodInvoker(delegate {
                    loading.Visible = cargando;
                    //loading.BackColor = Color.FromArgb(100, 63, 127, 191);
                    loading.Refresh();

                }));
            }
            else
            {
                loading.Visible = cargando;
            }

        }
    }
}
