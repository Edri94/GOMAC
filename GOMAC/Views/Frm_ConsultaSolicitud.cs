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
using ClosedXML.Excel;
using System.IO;

namespace GOMAC.Views
{
    public partial class Frm_ConsultaSolicitud : Form
    {
        public Frm_Login frml;
        public Frm_PantallaPrincipal frmp;

        private bmtktp01Entities bdbmtktp01;
        private DataTable dt;
        private bool dtgvw_cargado = false;

        private DateTime default_dtp = DateTimePicker.MinimumDateTime;
        private string default_cmb = "...";

        public Frm_ConsultaSolicitud(Frm_Login frml, Frm_PantallaPrincipal frmp)
        {
            InitializeComponent();

            this.frml = frml;
            this.frmp = frmp;

            this.bdbmtktp01 = new bmtktp01Entities();
        }

        private void Frm_ConsultaSolicitud_Load(object sender, EventArgs e)
        {
            LlenaComboBanca();
            LlenaComboStatus();
            LlenaComboConsultor();

            dtpFecha1.Value = dtpFecha1.MinDate;
            dtpFecha2.Value = dtpFecha2.MinDate;

            dtpFecha1.Enabled = false;
            dtpFecha2.Enabled = false;



        }

        private void LlenaComboConsultor()
        {
            try
            {
                List<ver_consultores> consultores =
                    (from c in bdbmtktp01.ver_consultores orderby c.Id_ConsultorMac ascending select c).ToList();

                if (consultores != null)
                {
                    //Anadiendo default
                    consultores.Insert(0, new ver_consultores { Iniciales_ConsultorMac = default_cmb });

                    cmbConsultor.DataSource = consultores;
                    cmbConsultor.ValueMember = "Id_ConsultorMac";
                    cmbConsultor.DisplayMember = "Iniciales_ConsultorMac";
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaComboStatus()
        {
            try
            {
                List<TIPO_STATUS> statuses =
                    (from s in bdbmtktp01.TIPO_STATUS orderby s.Id_Status select s).ToList();

                if (statuses != null)
                {
                    //Anadiendo default
                    statuses.Insert(0, new TIPO_STATUS { Descripcion_Status = default_cmb });

                    cmbStatus.DataSource = statuses;
                    cmbStatus.ValueMember = "Id_Status";
                    cmbStatus.DisplayMember = "Descripcion_Status";
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void LlenaComboBanca()
        {
            try
            {
                List<string> funcionarios =
                    bdbmtktp01.VER_FUNCIONARIOS.Select(b => b.BANCA).Distinct().ToList();


                if (funcionarios != null)
                {
                    //Anadiendo default
                    funcionarios.Insert(0, default_cmb);
                    cmbBanca.DataSource = funcionarios;
                }

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!bckWrkBusquedas.IsBusy)
            {
                bckWrkBusquedas.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Ya hay una tarea ejecutandose. Favor de Esperar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dtpFecha1_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFecha1.Value > DateTime.Now)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor al dia de hoy.", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(dtpFecha1.Value > dtpFecha2.Value)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor al dia final.", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkFechas_CheckedChanged(object sender, EventArgs e)
        {
            if(chkFechas.Checked)
            {
                dtpFecha1.Enabled = true;
                dtpFecha2.Enabled = true;
            }
            else
            {
                dtpFecha1.Enabled = false;
                dtpFecha2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel |*.xlsx";
            saveFileDialog1.Title = "Save an Excel File";
            saveFileDialog1.ShowDialog();

            string folderPath = "C:\\temp\\Interfaz_GOMAC\\";

            if(saveFileDialog1.FileName != "")
            {
                folderPath = saveFileDialog1.FileName;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Seguimientos");

                    wb.SaveAs(folderPath);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una ubicacion y nombre de archivo correcto", "Error al Exportsr archivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cargando(false);
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
                    loading.BackColor = Color.FromArgb(100, 63, 127, 191);
                    loading.Refresh();

                }));
            }
            else
            {
                loading.Visible = cargando;
            }

        }

        /// <summary>
        /// Se ejecuta al hacer una consulta de solicitud al dar click en el datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bckWrkConsultaSolicitud_DoWork(object sender, DoWorkEventArgs e)
        {

          
        }

        private void bckWrkConsultaSolicitud_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cargando(false);
        }

        

       
        /// <summary>
        /// Se ejecuta al dar click en buscar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bckWrkBusquedas_DoWork(object sender, DoWorkEventArgs e)
        {
            Cargando(true);
            try
            {
                List<SEGUIMIENTO> seguimientos = (
                    from s in bdbmtktp01.SEGUIMIENTO
                    join ts in bdbmtktp01.TIPO_STATUS on s.Status equals ts.Id_Status.ToString()
                    select s
                ).ToList();



                if (ControlText(cmbConsultor) == default_cmb && ControlText(txtCuenta) == "" && ControlText(txtFolio) == "" && ControlChecked(chkFechas) == false)
                {
                    MessageBox.Show("Debe introducir una cuenta de cliente o seleccionar un consultor o introducir un numero de folio de solicitud o un rango de fechas para realizar la busqueda.", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (ControlText(cmbConsultor) == default_cmb && ControlText(txtCuenta) == "" && ControlChecked(chkFechas) == true)
                {
                    if (ControlValue(dtpFecha1) == default_dtp  || ControlValue(dtpFecha2) == default_dtp)
                    {
                        MessageBox.Show("Debe introducir un rango de fehcas para poder realizar la busqueda.", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }


                //Filtro para FOLIO
                if (ControlText(txtFolio) != "")
                {
                    seguimientos = seguimientos.Where(w => w.Num_Solicitud == Int32.Parse(txtFolio.Text)).ToList();
                }

                //Filtro para CONSULTOR
                if (ControlText(cmbConsultor) != default_cmb)
                {
                    seguimientos = seguimientos.Where(w => w.Id_ConsultorMac == Int32.Parse(ControlValue(cmbConsultor).ToString())).ToList();
                }

                //Filtro para BANCA
                if (ControlText(cmbBanca) != default_cmb)
                {
                    VER_FUNCIONARIOS funcionario = bdbmtktp01.VER_FUNCIONARIOS.Where(w => w.BANCA == ControlValue(cmbBanca).ToString()).FirstOrDefault();
                    seguimientos = seguimientos.Where(w => w.Banca == funcionario.BANCA).ToList();
                }

                //Filtro para STATUS
                if (ControlText(cmbStatus) != default_cmb)
                {
                    seguimientos = seguimientos.Where(w => w.Status == ControlValue(cmbStatus).ToString()).ToList();
                }

                //Filtro para NOMBRE CLIENTE
                if (ControlText(txtNombre) != "")
                {
                    seguimientos = seguimientos.Where(w => w.Nombre_Cliente.ToUpper().TrimEnd() == ControlText(txtNombre).ToUpper().TrimEnd()).ToList();
                }

                //Filtro para PRIMER APELLIDO
                if (ControlText(txtApellido1) != "")
                {
                    seguimientos = seguimientos.Where(w => w.Apellido_Paterno.ToUpper().TrimEnd() == ControlText(txtApellido1).ToUpper().TrimEnd()).ToList();
                }

                //Filtro para SEGUNDO APELLIDO
                if (ControlText(txtApelllido2) != "")
                {
                    seguimientos = seguimientos.Where(w => w.Apellido_Materno.ToUpper().TrimEnd() == ControlText(txtApelllido2).ToUpper().TrimEnd()).ToList();
                }

                //Filtro para CUENTA CLIENTE
                if (ControlText(txtCuenta) != "")
                {
                    seguimientos = seguimientos.Where(w => w.Cuenta_Cliente.ToUpper().TrimEnd() == ControlText(txtCuenta).ToUpper().TrimEnd()).ToList();
                }

                //Filtro por TIPO PERSONA
                if (rbTodas.Checked)
                {
                    seguimientos = seguimientos.Where(w => w.Tipo_Persona == 0 && w.Tipo_Persona == 1).ToList();
                }
                else if (rbFisica.Checked)
                {
                    seguimientos = seguimientos.Where(w => w.Tipo_Persona == 0).ToList();
                }
                else if (rbMoral.Checked)
                {
                    seguimientos = seguimientos.Where(w => w.Tipo_Persona == 1).ToList();
                }

                //Filtrop por RANGO DE FECHAS
                DateTime fecha_ini = ControlValue(dtpFecha1);
                DateTime fecha_fin = ControlValue(dtpFecha2).AddMinutes(1439).AddSeconds(59);


                if (ControlChecked(chkFechas))
                {
                    seguimientos = seguimientos.Where(w => w.Fecha_Captura >= fecha_ini && w.Fecha_Captura <= fecha_fin).ToList();
                }

                var tabla = seguimientos.Join(
                    bdbmtktp01.TIPO_STATUS,
                    s => s.Status,
                    ts => ts.Id_Status.ToString(),
                    (status, tipo_status) => new {
                        Num_Solicitud = status.Num_Solicitud,
                        Cuenta_Cliente = status.Cuenta_Cliente,
                        Fecha_Captura = status.Fecha_Captura,
                        Nombre_Cliente = status.Nombre_Cliente,
                        Apellido_Paterno = status.Apellido_Paterno,
                        Apellido_Materno = status.Apellido_Materno,
                        Descripcion_Status = tipo_status.Descripcion_Status
                    }).ToList();

                          
                dt = new DataTable();
               

                dt.Columns.Add("Numero Solicitud");
                dt.Columns.Add("Cuenta Cliente");
                dt.Columns.Add("Fecha Captura");
                dt.Columns.Add("Nombre Cliente");
                dt.Columns.Add("Apellido Paterno");
                dt.Columns.Add("Apellido Materno");
                dt.Columns.Add("Descripcion Status");


                foreach (var item in tabla)
                {
                    DataRow dr = dt.NewRow();

                    dr["Numero Solicitud"] = item.Num_Solicitud;
                    dr["Cuenta Cliente"] = item.Cuenta_Cliente;
                    dr["Fecha Captura"] = item.Fecha_Captura;
                    dr["Nombre Cliente"] = item.Nombre_Cliente;
                    dr["Apellido Paterno"] = item.Apellido_Paterno;
                    dr["Apellido Materno"] = item.Apellido_Materno;
                    dr["Descripcion Status"] = item.Descripcion_Status;

                    dt.Rows.Add(dr);

                }


                dataGridView1.Invoke(new MethodInvoker(delegate {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    dtgvw_cargado = true;
                }));

               
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

      

        private void bckWrkBusquedas_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cargando(false);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvw_cargado)
            {
                int seleccionado = dataGridView1.CurrentCell.RowIndex;
                string nombre = dataGridView1.Rows[seleccionado].Cells["Nombre Cliente"].Value.ToString();
                int num_solicitud = (int.TryParse(dataGridView1.Rows[seleccionado].Cells["Numero Solicitud"].Value.ToString(), out num_solicitud)) ? num_solicitud : -1;

                if (num_solicitud > 0)
                {
                    //dataGridView1.Enabled = false;
                    frml.solicitud_selec = bdbmtktp01.SEGUIMIENTO.Where(w => w.Num_Solicitud == num_solicitud).FirstOrDefault();

                    if (frml.solicitud_selec != null)
                    {
                        frml.consultor_selec = bdbmtktp01.CONSULTORES.OrderBy(o => o.Iniciales_ConsultorMac).Where(w => w.Id_ConsultorMac == frml.solicitud_selec.Id_ConsultorMac).ToList().FirstOrDefault();

                        if (frml.consultor_selec != null)
                        {
                            this.Hide();
                            PantallaCarga frm = new PantallaCarga(frml, frmp);
                            //frm.Height = this.Height - 33;
                            //frm.Width = this.Width;
                            frm.ShowDialog();
        
                            //dataGridView1.Enabled = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el estado CHECK de un control que se intenta llamar desde otro subproceso
        /// </summary>
        /// <param name="control">Control del form al que se desea acceder</param>
        /// <returns>Propiedad Checked</returns>
        private bool ControlChecked(CheckBox control)
        {
            bool checado = false;

            control.Invoke(new MethodInvoker(delegate {

                checado = control.Checked;

            }));

            return checado;
        }

        /// <summary>
        /// Devuelve el TEXT de un control que se intenta llamar desde otro subproceso
        /// </summary>
        /// <param name="control">Control del form al que se desea acceder</param>
        /// <returns>Propiedad TEXT</returns>
        private string ControlText(Control control)
        {
            string txtControl = "";

            control.Invoke(new MethodInvoker(delegate {

                txtControl = control.Text;

            }));

            return txtControl;

        }

        /// <summary>
        /// Devuelve el VALUE de un control que se intenta llamar desde otro subproceso
        /// </summary>
        /// <param name="control">Control del form al que se desea acceder</param>
        /// <returns>Propiedad VALUE</returns>
        private DateTime ControlValue(DateTimePicker control)
        {
            DateTime valueControl = DateTimePicker.MinimumDateTime;

            control.Invoke(new MethodInvoker(delegate {

                valueControl = control.Value;

            }));

            return valueControl;

        }

        /// <summary>
        /// Devuelve el VALUE de un control que se intenta llamar desde otro subproceso
        /// </summary>
        /// <param name="control">Control del form al que se desea acceder</param>
        /// <returns>Propiedad VALUE</returns>
        private object ControlValue(ComboBox control)
        {
            object valueControl = null;

            control.Invoke(new MethodInvoker(delegate {

                valueControl = control.SelectedValue;

            }));

            return valueControl;

        }
    }
}
