
namespace GOMAC.Views
{
    partial class Frm_NuevaSolicitud
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSolicitud = new System.Windows.Forms.TextBox();
            this.lblSolicitud = new System.Windows.Forms.Label();
            this.lblFechaCaptura = new System.Windows.Forms.Label();
            this.lblConsultorMac = new System.Windows.Forms.Label();
            this.dtpFechaCaptura = new System.Windows.Forms.DateTimePicker();
            this.cmbConsultorMac = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblPuntos = new System.Windows.Forms.Label();
            this.cmbTipoSolicitud = new System.Windows.Forms.ComboBox();
            this.grpCircuito = new System.Windows.Forms.GroupBox();
            this.rbCircuitoAuto = new System.Windows.Forms.RadioButton();
            this.rbCircuitoManual = new System.Windows.Forms.RadioButton();
            this.lblTipoTramite = new System.Windows.Forms.Label();
            this.cmbTipoTramite = new System.Windows.Forms.ComboBox();
            this.lblTipoSolicitud = new System.Windows.Forms.Label();
            this.txtPuntos = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbExisteCuentaSi = new System.Windows.Forms.RadioButton();
            this.rbExisteCuentaNo = new System.Windows.Forms.RadioButton();
            this.lblCuenta = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.txtCuenta = new System.Windows.Forms.TextBox();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.grpTipoPersona = new System.Windows.Forms.GroupBox();
            this.rbPersonaFisica = new System.Windows.Forms.RadioButton();
            this.rbPersonaMoral = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtApellidoP = new System.Windows.Forms.TextBox();
            this.txtApellidoM = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpCircuito.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpTipoPersona.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbConsultorMac);
            this.groupBox1.Controls.Add(this.dtpFechaCaptura);
            this.groupBox1.Controls.Add(this.lblConsultorMac);
            this.groupBox1.Controls.Add(this.lblFechaCaptura);
            this.groupBox1.Controls.Add(this.lblSolicitud);
            this.groupBox1.Controls.Add(this.txtSolicitud);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(793, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtSolicitud
            // 
            this.txtSolicitud.Location = new System.Drawing.Point(9, 32);
            this.txtSolicitud.Name = "txtSolicitud";
            this.txtSolicitud.Size = new System.Drawing.Size(192, 20);
            this.txtSolicitud.TabIndex = 0;
            // 
            // lblSolicitud
            // 
            this.lblSolicitud.AutoSize = true;
            this.lblSolicitud.Location = new System.Drawing.Point(6, 16);
            this.lblSolicitud.Name = "lblSolicitud";
            this.lblSolicitud.Size = new System.Drawing.Size(50, 13);
            this.lblSolicitud.TabIndex = 1;
            this.lblSolicitud.Text = "Solicitud:";
            // 
            // lblFechaCaptura
            // 
            this.lblFechaCaptura.AutoSize = true;
            this.lblFechaCaptura.Location = new System.Drawing.Point(234, 16);
            this.lblFechaCaptura.Name = "lblFechaCaptura";
            this.lblFechaCaptura.Size = new System.Drawing.Size(80, 13);
            this.lblFechaCaptura.TabIndex = 1;
            this.lblFechaCaptura.Text = "Fecha Captura:";
            // 
            // lblConsultorMac
            // 
            this.lblConsultorMac.AutoSize = true;
            this.lblConsultorMac.Location = new System.Drawing.Point(463, 18);
            this.lblConsultorMac.Name = "lblConsultorMac";
            this.lblConsultorMac.Size = new System.Drawing.Size(80, 13);
            this.lblConsultorMac.TabIndex = 1;
            this.lblConsultorMac.Text = "Consultor MAC:";
            // 
            // dtpFechaCaptura
            // 
            this.dtpFechaCaptura.Location = new System.Drawing.Point(237, 32);
            this.dtpFechaCaptura.Name = "dtpFechaCaptura";
            this.dtpFechaCaptura.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaCaptura.TabIndex = 1;
            // 
            // cmbConsultorMac
            // 
            this.cmbConsultorMac.FormattingEnabled = true;
            this.cmbConsultorMac.Location = new System.Drawing.Point(466, 32);
            this.cmbConsultorMac.Name = "cmbConsultorMac";
            this.cmbConsultorMac.Size = new System.Drawing.Size(307, 21);
            this.cmbConsultorMac.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grpTipoPersona);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.grpCircuito);
            this.groupBox2.Controls.Add(this.cmbTipoTramite);
            this.groupBox2.Controls.Add(this.cmbTipoSolicitud);
            this.groupBox2.Controls.Add(this.lblTipoSolicitud);
            this.groupBox2.Controls.Add(this.lblTipoTramite);
            this.groupBox2.Controls.Add(this.txtPuntos);
            this.groupBox2.Controls.Add(this.lblPuntos);
            this.groupBox2.Location = new System.Drawing.Point(12, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(793, 114);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // lblPuntos
            // 
            this.lblPuntos.AutoSize = true;
            this.lblPuntos.Location = new System.Drawing.Point(253, 75);
            this.lblPuntos.Name = "lblPuntos";
            this.lblPuntos.Size = new System.Drawing.Size(43, 13);
            this.lblPuntos.TabIndex = 1;
            this.lblPuntos.Text = "Puntos:";
            // 
            // cmbTipoSolicitud
            // 
            this.cmbTipoSolicitud.FormattingEnabled = true;
            this.cmbTipoSolicitud.Location = new System.Drawing.Point(96, 22);
            this.cmbTipoSolicitud.Name = "cmbTipoSolicitud";
            this.cmbTipoSolicitud.Size = new System.Drawing.Size(133, 21);
            this.cmbTipoSolicitud.TabIndex = 3;
            // 
            // grpCircuito
            // 
            this.grpCircuito.Controls.Add(this.rbCircuitoManual);
            this.grpCircuito.Controls.Add(this.rbCircuitoAuto);
            this.grpCircuito.Location = new System.Drawing.Point(240, 10);
            this.grpCircuito.Name = "grpCircuito";
            this.grpCircuito.Size = new System.Drawing.Size(138, 44);
            this.grpCircuito.TabIndex = 3;
            this.grpCircuito.TabStop = false;
            this.grpCircuito.Text = "Circuito";
            // 
            // rbCircuitoAuto
            // 
            this.rbCircuitoAuto.AutoSize = true;
            this.rbCircuitoAuto.Location = new System.Drawing.Point(6, 19);
            this.rbCircuitoAuto.Name = "rbCircuitoAuto";
            this.rbCircuitoAuto.Size = new System.Drawing.Size(47, 17);
            this.rbCircuitoAuto.TabIndex = 5;
            this.rbCircuitoAuto.TabStop = true;
            this.rbCircuitoAuto.Text = "Auto";
            this.rbCircuitoAuto.UseVisualStyleBackColor = true;
            // 
            // rbCircuitoManual
            // 
            this.rbCircuitoManual.AutoSize = true;
            this.rbCircuitoManual.Location = new System.Drawing.Point(70, 19);
            this.rbCircuitoManual.Name = "rbCircuitoManual";
            this.rbCircuitoManual.Size = new System.Drawing.Size(60, 17);
            this.rbCircuitoManual.TabIndex = 6;
            this.rbCircuitoManual.TabStop = true;
            this.rbCircuitoManual.Text = "Manual";
            this.rbCircuitoManual.UseVisualStyleBackColor = true;
            // 
            // lblTipoTramite
            // 
            this.lblTipoTramite.AutoSize = true;
            this.lblTipoTramite.Location = new System.Drawing.Point(6, 75);
            this.lblTipoTramite.Name = "lblTipoTramite";
            this.lblTipoTramite.Size = new System.Drawing.Size(84, 13);
            this.lblTipoTramite.TabIndex = 1;
            this.lblTipoTramite.Text = "Tipo de Tramite:";
            // 
            // cmbTipoTramite
            // 
            this.cmbTipoTramite.FormattingEnabled = true;
            this.cmbTipoTramite.Location = new System.Drawing.Point(96, 72);
            this.cmbTipoTramite.Name = "cmbTipoTramite";
            this.cmbTipoTramite.Size = new System.Drawing.Size(151, 21);
            this.cmbTipoTramite.TabIndex = 4;
            // 
            // lblTipoSolicitud
            // 
            this.lblTipoSolicitud.AutoSize = true;
            this.lblTipoSolicitud.Location = new System.Drawing.Point(6, 25);
            this.lblTipoSolicitud.Name = "lblTipoSolicitud";
            this.lblTipoSolicitud.Size = new System.Drawing.Size(89, 13);
            this.lblTipoSolicitud.TabIndex = 1;
            this.lblTipoSolicitud.Text = "Tipo de Solicitud:";
            // 
            // txtPuntos
            // 
            this.txtPuntos.Location = new System.Drawing.Point(302, 73);
            this.txtPuntos.Name = "txtPuntos";
            this.txtPuntos.Size = new System.Drawing.Size(76, 20);
            this.txtPuntos.TabIndex = 7;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbExisteCuentaNo);
            this.groupBox3.Controls.Add(this.rbExisteCuentaSi);
            this.groupBox3.Controls.Add(this.lblProducto);
            this.groupBox3.Controls.Add(this.cmbProducto);
            this.groupBox3.Controls.Add(this.lblCuenta);
            this.groupBox3.Controls.Add(this.txtCuenta);
            this.groupBox3.Location = new System.Drawing.Point(394, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(261, 98);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Existe Cuenta en Tocket?";
            // 
            // rbExisteCuentaSi
            // 
            this.rbExisteCuentaSi.AutoSize = true;
            this.rbExisteCuentaSi.Location = new System.Drawing.Point(9, 19);
            this.rbExisteCuentaSi.Name = "rbExisteCuentaSi";
            this.rbExisteCuentaSi.Size = new System.Drawing.Size(34, 17);
            this.rbExisteCuentaSi.TabIndex = 8;
            this.rbExisteCuentaSi.TabStop = true;
            this.rbExisteCuentaSi.Text = "Si";
            this.rbExisteCuentaSi.UseVisualStyleBackColor = true;
            // 
            // rbExisteCuentaNo
            // 
            this.rbExisteCuentaNo.AutoSize = true;
            this.rbExisteCuentaNo.Location = new System.Drawing.Point(9, 52);
            this.rbExisteCuentaNo.Name = "rbExisteCuentaNo";
            this.rbExisteCuentaNo.Size = new System.Drawing.Size(39, 17);
            this.rbExisteCuentaNo.TabIndex = 9;
            this.rbExisteCuentaNo.TabStop = true;
            this.rbExisteCuentaNo.Text = "No";
            this.rbExisteCuentaNo.UseVisualStyleBackColor = true;
            // 
            // lblCuenta
            // 
            this.lblCuenta.AutoSize = true;
            this.lblCuenta.Location = new System.Drawing.Point(69, 23);
            this.lblCuenta.Name = "lblCuenta";
            this.lblCuenta.Size = new System.Drawing.Size(44, 13);
            this.lblCuenta.TabIndex = 1;
            this.lblCuenta.Text = "Cuenta:";
            // 
            // lblProducto
            // 
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(60, 54);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(53, 13);
            this.lblProducto.TabIndex = 1;
            this.lblProducto.Text = "Producto:";
            // 
            // txtCuenta
            // 
            this.txtCuenta.Location = new System.Drawing.Point(119, 19);
            this.txtCuenta.Name = "txtCuenta";
            this.txtCuenta.Size = new System.Drawing.Size(132, 20);
            this.txtCuenta.TabIndex = 10;
            // 
            // cmbProducto
            // 
            this.cmbProducto.FormattingEnabled = true;
            this.cmbProducto.Location = new System.Drawing.Point(119, 48);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Size = new System.Drawing.Size(133, 21);
            this.cmbProducto.TabIndex = 13;
            // 
            // grpTipoPersona
            // 
            this.grpTipoPersona.Controls.Add(this.rbPersonaMoral);
            this.grpTipoPersona.Controls.Add(this.rbPersonaFisica);
            this.grpTipoPersona.Location = new System.Drawing.Point(662, 10);
            this.grpTipoPersona.Name = "grpTipoPersona";
            this.grpTipoPersona.Size = new System.Drawing.Size(125, 98);
            this.grpTipoPersona.TabIndex = 5;
            this.grpTipoPersona.TabStop = false;
            this.grpTipoPersona.Text = "Tipo de Persona?";
            // 
            // rbPersonaFisica
            // 
            this.rbPersonaFisica.AutoSize = true;
            this.rbPersonaFisica.Location = new System.Drawing.Point(20, 27);
            this.rbPersonaFisica.Name = "rbPersonaFisica";
            this.rbPersonaFisica.Size = new System.Drawing.Size(52, 17);
            this.rbPersonaFisica.TabIndex = 14;
            this.rbPersonaFisica.TabStop = true;
            this.rbPersonaFisica.Text = "Fisica";
            this.rbPersonaFisica.UseVisualStyleBackColor = true;
            // 
            // rbPersonaMoral
            // 
            this.rbPersonaMoral.AutoSize = true;
            this.rbPersonaMoral.Location = new System.Drawing.Point(20, 61);
            this.rbPersonaMoral.Name = "rbPersonaMoral";
            this.rbPersonaMoral.Size = new System.Drawing.Size(51, 17);
            this.rbPersonaMoral.TabIndex = 15;
            this.rbPersonaMoral.TabStop = true;
            this.rbPersonaMoral.Text = "Moral";
            this.rbPersonaMoral.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 279);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(793, 144);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(785, 118);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(341, 68);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtApellidoM);
            this.groupBox4.Controls.Add(this.txtApellidoP);
            this.groupBox4.Controls.Add(this.txtNombre);
            this.groupBox4.Location = new System.Drawing.Point(12, 213);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(793, 60);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Nombre del Cliente";
            // 
            // txtNombre
            // 
            this.txtNombre.ForeColor = System.Drawing.Color.LightGray;
            this.txtNombre.Location = new System.Drawing.Point(9, 28);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(230, 20);
            this.txtNombre.TabIndex = 16;
            // 
            // txtApellidoP
            // 
            this.txtApellidoP.ForeColor = System.Drawing.Color.LightGray;
            this.txtApellidoP.Location = new System.Drawing.Point(247, 28);
            this.txtApellidoP.Name = "txtApellidoP";
            this.txtApellidoP.Size = new System.Drawing.Size(148, 20);
            this.txtApellidoP.TabIndex = 17;
            // 
            // txtApellidoM
            // 
            this.txtApellidoM.ForeColor = System.Drawing.Color.LightGray;
            this.txtApellidoM.Location = new System.Drawing.Point(403, 28);
            this.txtApellidoM.Name = "txtApellidoM";
            this.txtApellidoM.Size = new System.Drawing.Size(148, 20);
            this.txtApellidoM.TabIndex = 18;
            // 
            // Frm_NuevaSolicitud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 450);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Frm_NuevaSolicitud";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_NuevaSolicitud";
            this.Load += new System.EventHandler(this.Frm_NuevaSolicitud_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpCircuito.ResumeLayout(false);
            this.grpCircuito.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpTipoPersona.ResumeLayout(false);
            this.grpTipoPersona.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbConsultorMac;
        private System.Windows.Forms.DateTimePicker dtpFechaCaptura;
        private System.Windows.Forms.Label lblConsultorMac;
        private System.Windows.Forms.Label lblFechaCaptura;
        private System.Windows.Forms.Label lblSolicitud;
        private System.Windows.Forms.TextBox txtSolicitud;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox grpCircuito;
        private System.Windows.Forms.RadioButton rbCircuitoManual;
        private System.Windows.Forms.RadioButton rbCircuitoAuto;
        private System.Windows.Forms.ComboBox cmbTipoTramite;
        private System.Windows.Forms.ComboBox cmbTipoSolicitud;
        private System.Windows.Forms.Label lblTipoSolicitud;
        private System.Windows.Forms.Label lblTipoTramite;
        private System.Windows.Forms.TextBox txtPuntos;
        private System.Windows.Forms.Label lblPuntos;
        private System.Windows.Forms.RadioButton rbExisteCuentaSi;
        private System.Windows.Forms.RadioButton rbExisteCuentaNo;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.Label lblCuenta;
        private System.Windows.Forms.TextBox txtCuenta;
        private System.Windows.Forms.GroupBox grpTipoPersona;
        private System.Windows.Forms.RadioButton rbPersonaMoral;
        private System.Windows.Forms.RadioButton rbPersonaFisica;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtApellidoM;
        private System.Windows.Forms.TextBox txtApellidoP;
        private System.Windows.Forms.TextBox txtNombre;
    }
}