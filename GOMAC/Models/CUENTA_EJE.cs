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
    
    public partial class CUENTA_EJE
    {
        public int producto_contratado { get; set; }
        public byte tipo_cuenta_eje { get; set; }
        public Nullable<byte> tipo_cuenta_anterior { get; set; }
    
        public virtual PRODUCTO_CONTRATADO PRODUCTO_CONTRATADO1 { get; set; }
        public virtual TIPO_CUENTA_EJE TIPO_CUENTA_EJE1 { get; set; }
    }
}
