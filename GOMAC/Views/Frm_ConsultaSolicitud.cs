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
        Frm_Login frmp;
        private bmtktp01Entities bdbmtktp01;
        private DataTable dt;
        private bool dtgvw_cargado = false;

        public Frm_ConsultaSolicitud(Frm_Login frmp)
        {
            InitializeComponent();

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
                    consultores.Insert(0, new ver_consultores { Iniciales_ConsultorMac = "..." });

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
                    statuses.Insert(0, new TIPO_STATUS { Descripcion_Status = "..." });

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
                    funcionarios.Insert(0, "...");
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
            try
            {
                List<SEGUIMIENTO> seguimientos = (
                    from s in bdbmtktp01.SEGUIMIENTO
                    join ts in bdbmtktp01.TIPO_STATUS on s.Status equals ts.Id_Status.ToString()
                    select s
                ).ToList();


                if(cmbConsultor.Text == "..." && txtCuenta.Text == "" && txtFolio.Text == "" &&  chkFechas.Checked == false)
                {
                    MessageBox.Show("Debe introducir una cuenta de cliente o seleccionar un consultor o introducir un numero de folio de solicitud o un rango de fechas para realizar la busqueda.", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(cmbConsultor.Text == "..."  && txtCuenta.Text == "" && chkFechas.Checked == true)
                {
                    if(dtpFecha1.Value == DateTime.MinValue || dtpFecha2.Value == DateTime.MinValue)
                    {
                        MessageBox.Show("Debe introducir un rango de fehcas para poder realizar la busqueda.", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }


                //Filtro para FOLIO
                if(txtFolio.Text != "")
                {
                    seguimientos = seguimientos.Where(w => w.Num_Solicitud == Int32.Parse(txtFolio.Text)).ToList();
                }

                //Filtro para CONSULTOR
                if (cmbConsultor.Text != "...")
                {
                    seguimientos = seguimientos.Where(w => w.Id_ConsultorMac == Int32.Parse(cmbConsultor.SelectedValue.ToString())).ToList();
                }

                //Filtro para BANCA
                if (cmbBanca.Text != "...")
                {
                    VER_FUNCIONARIOS funcionario = bdbmtktp01.VER_FUNCIONARIOS.Where(w => w.BANCA == cmbBanca.SelectedValue.ToString()).FirstOrDefault();
                    seguimientos = seguimientos.Where(w => w.Banca == funcionario.BANCA).ToList();
                }

                //Filtro para STATUS
                if(cmbStatus.Text != "...")
                {
                    seguimientos = seguimientos.Where(w => w.Status == cmbStatus.SelectedValue.ToString()).ToList();
                }

                //Filtro para NOMBRE CLIENTE
                if(txtNombre.Text != "")
                {
                    seguimientos = seguimientos.Where(w => w.Nombre_Cliente.ToUpper().TrimEnd() == txtNombre.Text.ToUpper().TrimEnd()).ToList();
                }

                //Filtro para PRIMER APELLIDO
                if (txtApellido1.Text != "")
                {
                    seguimientos = seguimientos.Where(w => w.Apellido_Paterno.ToUpper().TrimEnd() == txtApellido1.Text.ToUpper().TrimEnd()).ToList();
                }

                //Filtro para SEGUNDO APELLIDO
                if (txtAplllido2.Text != "")
                {
                    seguimientos = seguimientos.Where(w => w.Apellido_Materno.ToUpper().TrimEnd() == txtAplllido2.Text.ToUpper().TrimEnd()).ToList();
                }

                //Filtro para CUENTA CLIENTE
                if (txtCuenta.Text != "")
                {
                    seguimientos = seguimientos.Where(w => w.Cuenta_Cliente.ToUpper().TrimEnd() == txtCuenta.Text.ToUpper().TrimEnd()).ToList();
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
                DateTime fecha_ini = dtpFecha1.Value;
                DateTime fecha_fin = dtpFecha2.Value.AddMinutes(1439).AddSeconds(59);


                if (chkFechas.Checked)
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
                dt.Clear();
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


                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                dtgvw_cargado = true;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
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

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvw_cargado)
            {
                int seleccionado = dataGridView1.CurrentCell.RowIndex;
                string nombre = dataGridView1.Rows[seleccionado].Cells["Nombre Cliente"].Value.ToString();
                int num_solicitud = (int.TryParse(dataGridView1.Rows[seleccionado].Cells["Numero Solicitud"].Value.ToString(), out num_solicitud)) ? num_solicitud : -1;

                if(num_solicitud > 0)
                {
                    dataGridView1.Enabled = false;
                    frmp.solicitud_selec = bdbmtktp01.SEGUIMIENTO.Where(w => w.Num_Solicitud == num_solicitud).FirstOrDefault();

                    if(frmp.solicitud_selec != null)
                    {
                         frmp.consultor_selec = bdbmtktp01.CONSULTORES.OrderBy(o => o.Iniciales_ConsultorMac).Where(w => w.Id_ConsultorMac == frmp.solicitud_selec.Id_ConsultorMac).ToList().FirstOrDefault();

                        if(frmp.consultor_selec != null)
                        {
                            FrmNueva_Solicitud frm = new FrmNueva_Solicitud(frmp);
                            frm.ShowDialog();
                        }
                    }
                }

                MessageBox.Show($"{num_solicitud}consultar el cliente: {nombre}");
            }
        }
    }
}
