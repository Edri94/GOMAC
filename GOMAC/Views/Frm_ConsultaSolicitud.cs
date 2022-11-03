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
    public partial class Frm_ConsultaSolicitud : Form
    {
        Frm_PantallaPrincipal frmp;
        private bmtktp01Entities bdbmtktp01;

        public Frm_ConsultaSolicitud(Frm_PantallaPrincipal frmp)
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

        }

        private void LlenaComboConsultor()
        {
            try
            {
                List<ver_consultores> consultores =
                    (from c in bdbmtktp01.ver_consultores orderby c.Id_ConsultorMac ascending select c).ToList();

                if (consultores != null)
                {
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
                
                //Anadiendo default
                statuses.Insert(0, new TIPO_STATUS { Descripcion_Status = "..." });

                cmbStatus.DataSource = statuses;
                cmbStatus.ValueMember = "Id_Status";
                cmbStatus.DisplayMember = "Descripcion_Status";

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

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }
    }
}
