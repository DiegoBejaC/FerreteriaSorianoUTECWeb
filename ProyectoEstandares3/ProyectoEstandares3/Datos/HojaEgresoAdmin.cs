using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class HojaEgresoAdmin
    {
        /// <summary>
        /// Consulta todos los datos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HojaEgreso> Consultar()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.HojaEgreso.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Guardar un registro en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(HojaEgreso modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.HojaEgreso.Add(modelo);
                contexto.SaveChanges();
            }
        }
        //En este método se obtienen todos los campos de un único registro de la tabla Compras
        //Con tal de poderlos mostrar durante el proceso de agregar los detalles de toda la compra.
        public HojaEgreso ObtenerDetalles(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                //return (Compras)contexto.Compras.Where(p => p.Id == ID).Select(c => c.Documento);
                return contexto.HojaEgreso.FirstOrDefault(p => p.Id == ID);
            }
        }

        //Método para modificar todos los campos de un sólo registro de la tabla.
        public void Modificar(HojaEgreso modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
                contexto.SaveChanges();
            }
        }
    }
}