
namespace GOMAC.Views
{
    partial class Frm_ConsultaSolicitud
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.grpRangoFechas = new System.Windows.Forms.GroupBox();
            this.dtpFecha2 = new System.Windows.Forms.DateTimePicker();
            this.dtpFecha1 = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.grpTipoPersona = new System.Windows.Forms.GroupBox();
            this.rbMoral = new System.Windows.Forms.RadioButton();
            this.rbFisica = new System.Windows.Forms.RadioButton();
            this.rbTodas = new System.Windows.Forms.RadioButton();
            this.txtAplllido2 = new System.Windows.Forms.TextBox();
            this.txtApellido1 = new System.Windows.Forms.TextBox();
            this.txtFolio = new System.Windows.Forms.TextBox();
            this.txtCuenta = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBanca = new System.Windows.Forms.ComboBox();
            this.cmbConsultor = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.chkFechas = new System.Windows.Forms.CheckBox();
            this.grpFiltros.SuspendLayout();
            this.grpRangoFechas.SuspendLayout();
            this.grpTipoPersona.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpFiltros
            // 
            this.grpFiltros.Controls.Add(this.button1);
            this.grpFiltros.Controls.Add(this.grpRangoFechas);
            this.grpFiltros.Controls.Add(this.grpTipoPersona);
            this.grpFiltros.Controls.Add(this.txtAplllido2);
            this.grpFiltros.Controls.Add(this.txtApellido1);
            this.grpFiltros.Controls.Add(this.txtFolio);
            this.grpFiltros.Controls.Add(this.txtCuenta);
            this.grpFiltros.Controls.Add(this.txtNombre);
            this.grpFiltros.Controls.Add(this.label5);
            this.grpFiltros.Controls.Add(this.label9);
            this.grpFiltros.Controls.Add(this.label4);
            this.grpFiltros.Controls.Add(this.label6);
            this.grpFiltros.Controls.Add(this.label3);
            this.grpFiltros.Controls.Add(this.label8);
            this.grpFiltros.Controls.Add(this.label7);
            this.grpFiltros.Controls.Add(this.cmbStatus);
            this.grpFiltros.Controls.Add(this.label2);
            this.grpFiltros.Controls.Add(this.label1);
            this.grpFiltros.Controls.Add(this.cmbBanca);
            this.grpFiltros.Controls.Add(this.cmbConsultor);
            this.grpFiltros.Location = new System.Drawing.Point(8, 8);
            this.grpFiltros.Margin = new System.Windows.Forms.Padding(2);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Padding = new System.Windows.Forms.Padding(2);
            this.grpFiltros.Size = new System.Drawing.Size(783, 226);
            this.grpFiltros.TabIndex = 0;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "Seleccione los filtros de busqueda";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(657, 188);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 31);
            this.button1.TabIndex = 15;
            this.button1.Text = "Buscar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grpRangoFechas
            // 
            this.grpRangoFechas.Controls.Add(this.chkFechas);
            this.grpRangoFechas.Controls.Add(this.dtpFecha2);
            this.grpRangoFechas.Controls.Add(this.dtpFecha1);
            this.grpRangoFechas.Controls.Add(this.label11);
            this.grpRangoFechas.Controls.Add(this.label10);
            this.grpRangoFechas.Location = new System.Drawing.Point(311, 118);
            this.grpRangoFechas.Margin = new System.Windows.Forms.Padding(2);
            this.grpRangoFechas.Name = "grpRangoFechas";
            this.grpRangoFechas.Padding = new System.Windows.Forms.Padding(2);
            this.grpRangoFechas.Size = new System.Drawing.Size(457, 66);
            this.grpRangoFechas.TabIndex = 4;
            this.grpRangoFechas.TabStop = false;
            this.grpRangoFechas.Text = "Rango de Fechas";
            // 
            // dtpFecha2
            // 
            this.dtpFecha2.Location = new System.Drawing.Point(313, 32);
            this.dtpFecha2.Margin = new System.Windows.Forms.Padding(2);
            this.dtpFecha2.Name = "dtpFecha2";
            this.dtpFecha2.Size = new System.Drawing.Size(135, 20);
            this.dtpFecha2.TabIndex = 14;
            // 
            // dtpFecha1
            // 
            this.dtpFecha1.Location = new System.Drawing.Point(106, 32);
            this.dtpFecha1.Margin = new System.Windows.Forms.Padding(2);
            this.dtpFecha1.Name = "dtpFecha1";
            this.dtpFecha1.Size = new System.Drawing.Size(135, 20);
            this.dtpFecha1.TabIndex = 13;
            this.dtpFecha1.ValueChanged += new System.EventHandler(this.dtpFecha1_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(247, 36);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Fecha Final";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 36);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Fecha Inicial";
            // 
            // grpTipoPersona
            // 
            this.grpTipoPersona.Controls.Add(this.rbMoral);
            this.grpTipoPersona.Controls.Add(this.rbFisica);
            this.grpTipoPersona.Controls.Add(this.rbTodas);
            this.grpTipoPersona.Location = new System.Drawing.Point(470, 17);
            this.grpTipoPersona.Margin = new System.Windows.Forms.Padding(2);
            this.grpTipoPersona.Name = "grpTipoPersona";
            this.grpTipoPersona.Padding = new System.Windows.Forms.Padding(2);
            this.grpTipoPersona.Size = new System.Drawing.Size(304, 44);
            this.grpTipoPersona.TabIndex = 3;
            this.grpTipoPersona.TabStop = false;
            this.grpTipoPersona.Text = "Tipo Persona";
            // 
            // rbMoral
            // 
            this.rbMoral.AutoSize = true;
            this.rbMoral.Location = new System.Drawing.Point(249, 16);
            this.rbMoral.Margin = new System.Windows.Forms.Padding(2);
            this.rbMoral.Name = "rbMoral";
            this.rbMoral.Size = new System.Drawing.Size(51, 17);
            this.rbMoral.TabIndex = 5;
            this.rbMoral.TabStop = true;
            this.rbMoral.Text = "Moral";
            this.rbMoral.UseVisualStyleBackColor = true;
            // 
            // rbFisica
            // 
            this.rbFisica.AutoSize = true;
            this.rbFisica.Location = new System.Drawing.Point(166, 16);
            this.rbFisica.Margin = new System.Windows.Forms.Padding(2);
            this.rbFisica.Name = "rbFisica";
            this.rbFisica.Size = new System.Drawing.Size(52, 17);
            this.rbFisica.TabIndex = 4;
            this.rbFisica.TabStop = true;
            this.rbFisica.Text = "Fisica";
            this.rbFisica.UseVisualStyleBackColor = true;
            // 
            // rbTodas
            // 
            this.rbTodas.AutoSize = true;
            this.rbTodas.Location = new System.Drawing.Point(81, 16);
            this.rbTodas.Margin = new System.Windows.Forms.Padding(2);
            this.rbTodas.Name = "rbTodas";
            this.rbTodas.Size = new System.Drawing.Size(55, 17);
            this.rbTodas.TabIndex = 3;
            this.rbTodas.TabStop = true;
            this.rbTodas.Text = "Todas";
            this.rbTodas.UseVisualStyleBackColor = true;
            // 
            // txtAplllido2
            // 
            this.txtAplllido2.Location = new System.Drawing.Point(568, 88);
            this.txtAplllido2.Margin = new System.Windows.Forms.Padding(2);
            this.txtAplllido2.Name = "txtAplllido2";
            this.txtAplllido2.Size = new System.Drawing.Size(207, 20);
            this.txtAplllido2.TabIndex = 8;
            // 
            // txtApellido1
            // 
            this.txtApellido1.Location = new System.Drawing.Point(351, 88);
            this.txtApellido1.Margin = new System.Windows.Forms.Padding(2);
            this.txtApellido1.Name = "txtApellido1";
            this.txtApellido1.Size = new System.Drawing.Size(207, 20);
            this.txtApellido1.TabIndex = 7;
            // 
            // txtFolio
            // 
            this.txtFolio.Location = new System.Drawing.Point(59, 167);
            this.txtFolio.Margin = new System.Windows.Forms.Padding(2);
            this.txtFolio.Name = "txtFolio";
            this.txtFolio.Size = new System.Drawing.Size(149, 20);
            this.txtFolio.TabIndex = 11;
            // 
            // txtCuenta
            // 
            this.txtCuenta.Location = new System.Drawing.Point(59, 118);
            this.txtCuenta.Margin = new System.Windows.Forms.Padding(2);
            this.txtCuenta.Name = "txtCuenta";
            this.txtCuenta.Size = new System.Drawing.Size(149, 20);
            this.txtCuenta.TabIndex = 9;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(17, 88);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(325, 20);
            this.txtNombre.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(565, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Segundo Apellido";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 169);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Folio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(349, 73);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Primer Apellido";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 120);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Cuenta";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 73);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Cliente Nombre(s)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 144);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Status";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 49);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Banca";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(59, 142);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(2);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(197, 21);
            this.cmbStatus.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Banca";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Consultor";
            // 
            // cmbBanca
            // 
            this.cmbBanca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBanca.FormattingEnabled = true;
            this.cmbBanca.Location = new System.Drawing.Point(70, 46);
            this.cmbBanca.Margin = new System.Windows.Forms.Padding(2);
            this.cmbBanca.Name = "cmbBanca";
            this.cmbBanca.Size = new System.Drawing.Size(186, 21);
            this.cmbBanca.TabIndex = 2;
            // 
            // cmbConsultor
            // 
            this.cmbConsultor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConsultor.FormattingEnabled = true;
            this.cmbConsultor.Location = new System.Drawing.Point(70, 19);
            this.cmbConsultor.Margin = new System.Windows.Forms.Padding(2);
            this.cmbConsultor.Name = "cmbConsultor";
            this.cmbConsultor.Size = new System.Drawing.Size(186, 21);
            this.cmbConsultor.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(8, 244);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(783, 213);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resultado de la Busqueda";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 16);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(770, 187);
            this.dataGridView1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(679, 467);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 28);
            this.button2.TabIndex = 3;
            this.button2.Text = "Salir";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(553, 467);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 28);
            this.button3.TabIndex = 3;
            this.button3.Text = "Exporta a Excel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 467);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(529, 28);
            this.progressBar1.TabIndex = 4;
            // 
            // chkFechas
            // 
            this.chkFechas.AutoSize = true;
            this.chkFechas.Location = new System.Drawing.Point(16, 35);
            this.chkFechas.Name = "chkFechas";
            this.chkFechas.Size = new System.Drawing.Size(15, 14);
            this.chkFechas.TabIndex = 12;
            this.chkFechas.UseVisualStyleBackColor = true;
            this.chkFechas.CheckedChanged += new System.EventHandler(this.chkFechas_CheckedChanged);
            // 
            // Frm_ConsultaSolicitud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 504);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpFiltros);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Frm_ConsultaSolicitud";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta de Solicitudes";
            this.Load += new System.EventHandler(this.Frm_ConsultaSolicitud_Load);
            this.grpFiltros.ResumeLayout(false);
            this.grpFiltros.PerformLayout();
            this.grpRangoFechas.ResumeLayout(false);
            this.grpRangoFechas.PerformLayout();
            this.grpTipoPersona.ResumeLayout(false);
            this.grpTipoPersona.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFiltros;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox grpTipoPersona;
        private System.Windows.Forms.RadioButton rbMoral;
        private System.Windows.Forms.RadioButton rbFisica;
        private System.Windows.Forms.RadioButton rbTodas;
        private System.Windows.Forms.TextBox txtAplllido2;
        private System.Windows.Forms.TextBox txtApellido1;
        private System.Windows.Forms.TextBox txtFolio;
        private System.Windows.Forms.TextBox txtCuenta;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBanca;
        private System.Windows.Forms.ComboBox cmbConsultor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox grpRangoFechas;
        private System.Windows.Forms.DateTimePicker dtpFecha2;
        private System.Windows.Forms.DateTimePicker dtpFecha1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox chkFechas;
    }
}