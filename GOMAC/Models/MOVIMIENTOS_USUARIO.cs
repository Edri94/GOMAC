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
    
    public partial class MOVIMIENTOS_USUARIO
    {
        public int id_movus { get; set; }
        public int id_usuario { get; set; }
        public int id_usuariomodificado { get; set; }
        public int id_mov { get; set; }
        public string datoanterior { get; set; }
        public string datosctual { get; set; }
        public Nullable<System.DateTime> fechamov { get; set; }
        public Nullable<System.TimeSpan> horamov { get; set; }
    
        public virtual TIPO_MOV_USUARIO TIPO_MOV_USUARIO { get; set; }
        public virtual USUARIO USUARIO { get; set; }
        public virtual USUARIO USUARIO1 { get; set; }
    }
}