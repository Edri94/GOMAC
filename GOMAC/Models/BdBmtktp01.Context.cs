﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GOMAC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bmtktp01Entities : DbContext
    {
        public bmtktp01Entities()
            : base("name=bmtktp01Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CONECT> CONECT { get; set; }
        public virtual DbSet<CONSULTORES> CONSULTORES { get; set; }
        public virtual DbSet<dtproperties> dtproperties { get; set; }
        public virtual DbSet<HISTORIALPWD> HISTORIALPWD { get; set; }
        public virtual DbSet<MOVIMIENTOS_PERFIL> MOVIMIENTOS_PERFIL { get; set; }
        public virtual DbSet<MOVIMIENTOS_USUARIO> MOVIMIENTOS_USUARIO { get; set; }
        public virtual DbSet<OPERACION_PIU> OPERACION_PIU { get; set; }
        public virtual DbSet<PERFIL> PERFIL { get; set; }
        public virtual DbSet<SECTORES> SECTORES { get; set; }
        public virtual DbSet<SEGUIMIENTO> SEGUIMIENTO { get; set; }
        public virtual DbSet<SEGUIMIENTO_DOCTOS> SEGUIMIENTO_DOCTOS { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TIPO_MOV_PERFIL> TIPO_MOV_PERFIL { get; set; }
        public virtual DbSet<TIPO_MOV_USUARIO> TIPO_MOV_USUARIO { get; set; }
        public virtual DbSet<TIPO_SOLICITUD> TIPO_SOLICITUD { get; set; }
        public virtual DbSet<TIPO_STATUS> TIPO_STATUS { get; set; }
        public virtual DbSet<TIPO_TRAMITE> TIPO_TRAMITE { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<OBSERVACIONES> OBSERVACIONES { get; set; }
        public virtual DbSet<PERFIL_SECTOR> PERFIL_SECTOR { get; set; }
        public virtual DbSet<log_conect> log_conect { get; set; }
        public virtual DbSet<ver_conect> ver_conect { get; set; }
        public virtual DbSet<ver_conect2> ver_conect2 { get; set; }
        public virtual DbSet<ver_consultores> ver_consultores { get; set; }
        public virtual DbSet<VER_FUNCIONARIOS> VER_FUNCIONARIOS { get; set; }
        public virtual DbSet<ver_mov_perfil> ver_mov_perfil { get; set; }
        public virtual DbSet<ver_mov_solicitudes> ver_mov_solicitudes { get; set; }
        public virtual DbSet<ver_mov_usuario> ver_mov_usuario { get; set; }
        public virtual DbSet<ver_perfil_sector> ver_perfil_sector { get; set; }
        public virtual DbSet<ver_perfiles> ver_perfiles { get; set; }
        public virtual DbSet<ver_sectores> ver_sectores { get; set; }
        public virtual DbSet<ver_Tipo_Solicitud> ver_Tipo_Solicitud { get; set; }
        public virtual DbSet<ver_Tipo_Tramite> ver_Tipo_Tramite { get; set; }
        public virtual DbSet<ver_usuarios> ver_usuarios { get; set; }
        public virtual DbSet<ver_usuarios2> ver_usuarios2 { get; set; }
    }
}