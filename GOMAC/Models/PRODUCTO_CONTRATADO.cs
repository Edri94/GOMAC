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
    
    public partial class PRODUCTO_CONTRATADO
    {
        public int producto_contratado1 { get; set; }
        public short producto { get; set; }
        public string cuenta_cliente { get; set; }
        public string clave_producto_contratado { get; set; }
        public System.DateTime fecha_contratacion { get; set; }
        public Nullable<System.DateTime> fecha_vencimiento { get; set; }
        public Nullable<short> status_producto { get; set; }
        public Nullable<byte> agencia { get; set; }
    
        public virtual CUENTA_EJE CUENTA_EJE { get; set; }
    }
}