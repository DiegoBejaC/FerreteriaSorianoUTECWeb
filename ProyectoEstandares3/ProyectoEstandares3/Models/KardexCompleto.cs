namespace ProyectoEstandares3.Models
{
    using System;
    using System.Collections.Generic;
    public partial class KardexCompleto
    {
        public int Id_kardex { get; set; }
        public int Id_hojaEgreso { get; set; }
        public string Inventario { get; set; }
        public int Id_compra { get; set; }
        public string Almacen { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<decimal> PrecioTotal { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
    }
}