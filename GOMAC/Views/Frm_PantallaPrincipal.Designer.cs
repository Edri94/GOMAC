﻿
namespace GOMAC.Views
{
    partial class Frm_PantallaPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_PantallaPrincipal));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sistemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perfilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevaSolicitudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultaSolicitudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualizarSolicitudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualizarSolicitudToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.administradorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.altaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenimientoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perfilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.conexionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inact = new System.Windows.Forms.Timer(this.components);
            this.tmtHora = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sistemaToolStripMenuItem,
            this.nuevaSolicitudToolStripMenuItem,
            this.consultaSolicitudToolStripMenuItem,
            this.actualizarSolicitudToolStripMenuItem,
            this.administradorToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sistemaToolStripMenuItem
            // 
            this.sistemaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.perfilToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.sistemaToolStripMenuItem.Name = "sistemaToolStripMenuItem";
            this.sistemaToolStripMenuItem.Size = new System.Drawing.Size(90, 29);
            this.sistemaToolStripMenuItem.Text = "Sistema";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.loginToolStripMenuItem.Text = "Login";
            // 
            // perfilToolStripMenuItem
            // 
            this.perfilToolStripMenuItem.Name = "perfilToolStripMenuItem";
            this.perfilToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.perfilToolStripMenuItem.Text = "Perfil";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.salirToolStripMenuItem.Text = "Salir";
            // 
            // nuevaSolicitudToolStripMenuItem
            // 
            this.nuevaSolicitudToolStripMenuItem.Name = "nuevaSolicitudToolStripMenuItem";
            this.nuevaSolicitudToolStripMenuItem.Size = new System.Drawing.Size(151, 29);
            this.nuevaSolicitudToolStripMenuItem.Text = "Nueva Solicitud";
            this.nuevaSolicitudToolStripMenuItem.Click += new System.EventHandler(this.nuevaSolicitudToolStripMenuItem_Click);
            // 
            // consultaSolicitudToolStripMenuItem
            // 
            this.consultaSolicitudToolStripMenuItem.Name = "consultaSolicitudToolStripMenuItem";
            this.consultaSolicitudToolStripMenuItem.Size = new System.Drawing.Size(170, 29);
            this.consultaSolicitudToolStripMenuItem.Text = "Consulta Solicitud";
            this.consultaSolicitudToolStripMenuItem.Click += new System.EventHandler(this.consultaSolicitudToolStripMenuItem_Click);
            // 
            // actualizarSolicitudToolStripMenuItem
            // 
            this.actualizarSolicitudToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actualizarSolicitudToolStripMenuItem1});
            this.actualizarSolicitudToolStripMenuItem.Name = "actualizarSolicitudToolStripMenuItem";
            this.actualizarSolicitudToolStripMenuItem.Size = new System.Drawing.Size(133, 29);
            this.actualizarSolicitudToolStripMenuItem.Text = "Herramientas";
            // 
            // actualizarSolicitudToolStripMenuItem1
            // 
            this.actualizarSolicitudToolStripMenuItem1.Name = "actualizarSolicitudToolStripMenuItem1";
            this.actualizarSolicitudToolStripMenuItem1.Size = new System.Drawing.Size(263, 34);
            this.actualizarSolicitudToolStripMenuItem1.Text = "Actualizar Solicitud";
            this.actualizarSolicitudToolStripMenuItem1.Click += new System.EventHandler(this.actualizarSolicitudToolStripMenuItem1_Click);
            // 
            // administradorToolStripMenuItem
            // 
            this.administradorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usuarioToolStripMenuItem,
            this.perfilesToolStripMenuItem,
            this.reporteToolStripMenuItem});
            this.administradorToolStripMenuItem.Name = "administradorToolStripMenuItem";
            this.administradorToolStripMenuItem.Size = new System.Drawing.Size(142, 29);
            this.administradorToolStripMenuItem.Text = "Administrador";
            // 
            // usuarioToolStripMenuItem
            // 
            this.usuarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.altaToolStripMenuItem,
            this.mantenimientoToolStripMenuItem});
            this.usuarioToolStripMenuItem.Name = "usuarioToolStripMenuItem";
            this.usuarioToolStripMenuItem.Size = new System.Drawing.Size(176, 34);
            this.usuarioToolStripMenuItem.Text = "Usuario";
            // 
            // altaToolStripMenuItem
            // 
            this.altaToolStripMenuItem.Name = "altaToolStripMenuItem";
            this.altaToolStripMenuItem.Size = new System.Drawing.Size(234, 34);
            this.altaToolStripMenuItem.Text = "Alta";
            // 
            // mantenimientoToolStripMenuItem
            // 
            this.mantenimientoToolStripMenuItem.Name = "mantenimientoToolStripMenuItem";
            this.mantenimientoToolStripMenuItem.Size = new System.Drawing.Size(234, 34);
            this.mantenimientoToolStripMenuItem.Text = "Mantenimiento";
            // 
            // perfilesToolStripMenuItem
            // 
            this.perfilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.altaToolStripMenuItem1,
            this.mantenimientoToolStripMenuItem1});
            this.perfilesToolStripMenuItem.Name = "perfilesToolStripMenuItem";
            this.perfilesToolStripMenuItem.Size = new System.Drawing.Size(176, 34);
            this.perfilesToolStripMenuItem.Text = "Perfiles";
            // 
            // altaToolStripMenuItem1
            // 
            this.altaToolStripMenuItem1.Name = "altaToolStripMenuItem1";
            this.altaToolStripMenuItem1.Size = new System.Drawing.Size(234, 34);
            this.altaToolStripMenuItem1.Text = "Alta";
            // 
            // mantenimientoToolStripMenuItem1
            // 
            this.mantenimientoToolStripMenuItem1.Name = "mantenimientoToolStripMenuItem1";
            this.mantenimientoToolStripMenuItem1.Size = new System.Drawing.Size(234, 34);
            this.mantenimientoToolStripMenuItem1.Text = "Mantenimiento";
            // 
            // reporteToolStripMenuItem
            // 
            this.reporteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usuariosToolStripMenuItem,
            this.perfilesToolStripMenuItem1,
            this.conexionesToolStripMenuItem});
            this.reporteToolStripMenuItem.Name = "reporteToolStripMenuItem";
            this.reporteToolStripMenuItem.Size = new System.Drawing.Size(176, 34);
            this.reporteToolStripMenuItem.Text = "Reporte";
            // 
            // usuariosToolStripMenuItem
            // 
            this.usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            this.usuariosToolStripMenuItem.Size = new System.Drawing.Size(205, 34);
            this.usuariosToolStripMenuItem.Text = "Usuarios";
            // 
            // perfilesToolStripMenuItem1
            // 
            this.perfilesToolStripMenuItem1.Name = "perfilesToolStripMenuItem1";
            this.perfilesToolStripMenuItem1.Size = new System.Drawing.Size(205, 34);
            this.perfilesToolStripMenuItem1.Text = "Perfiles";
            // 
            // conexionesToolStripMenuItem
            // 
            this.conexionesToolStripMenuItem.Name = "conexionesToolStripMenuItem";
            this.conexionesToolStripMenuItem.Size = new System.Drawing.Size(205, 34);
            this.conexionesToolStripMenuItem.Text = "Conexiones";
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(79, 29);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // inact
            // 
            this.inact.Tick += new System.EventHandler(this.inact_Tick);
            // 
            // tmtHora
            // 
            this.tmtHora.Tick += new System.EventHandler(this.tmtHora_Tick);
            // 
            // Frm_PantallaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.BackgroundImage = global::GOMAC.Properties.Resources.snippet_open_innovation;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Frm_PantallaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GOMAC";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PantallaPrincipal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sistemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perfilToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevaSolicitudToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultaSolicitudToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualizarSolicitudToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administradorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usuarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem altaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mantenimientoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem altaToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mantenimientoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem reporteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perfilesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem conexionesToolStripMenuItem;
        private System.Windows.Forms.Timer inact;
        private System.Windows.Forms.Timer tmtHora;
        private System.Windows.Forms.ToolStripMenuItem actualizarSolicitudToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
    }
}