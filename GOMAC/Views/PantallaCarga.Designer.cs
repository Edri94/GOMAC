
namespace GOMAC.Views
{
    partial class PantallaCarga
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
            this.components = new System.ComponentModel.Container();
            this.loading = new System.Windows.Forms.PictureBox();
            this.tmrTraerDatos = new System.Windows.Forms.Timer(this.components);
            this.bckWrkConsultas = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.loading)).BeginInit();
            this.SuspendLayout();
            // 
            // loading
            // 
            this.loading.BackColor = System.Drawing.Color.Transparent;
            this.loading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.loading.Image = global::GOMAC.Properties.Resources.loading;
            this.loading.Location = new System.Drawing.Point(309, 34);
            this.loading.Name = "loading";
            this.loading.Size = new System.Drawing.Size(607, 559);
            this.loading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loading.TabIndex = 0;
            this.loading.TabStop = false;
            // 
            // tmrTraerDatos
            // 
            this.tmrTraerDatos.Tick += new System.EventHandler(this.tmrTraerDatos_Tick);
            // 
            // bckWrkConsultas
            // 
            this.bckWrkConsultas.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckWrkConsultas_DoWork);
            this.bckWrkConsultas.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckWrkConsultas_RunWorkerCompleted);
            // 
            // PantallaCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1222, 635);
            this.Controls.Add(this.loading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PantallaCarga";
            this.Opacity = 0.1D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PantallaCarga";
            this.Load += new System.EventHandler(this.PantallaCarga_Load);
            this.Shown += new System.EventHandler(this.PantallaCarga_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.loading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox loading;
        private System.Windows.Forms.Timer tmrTraerDatos;
        private System.ComponentModel.BackgroundWorker bckWrkConsultas;
    }
}