//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ver_usuarios
    {
        public int id_usuario { get; set; }
        public int id_perfil { get; set; }
        public string login { get; set; }
        public string pwd { get; set; }
        public bool fechaexpira { get; set; }
        public Nullable<System.DateTime> fecha_de_expiracion { get; set; }
        public bool habilitado { get; set; }
        public bool bloqueado { get; set; }
        public System.DateTime fechacreacion { get; set; }
        public bool conectado { get; set; }
        public Nullable<bool> nuevoinicio { get; set; }
        public Nullable<bool> resetpwd { get; set; }
    }
}