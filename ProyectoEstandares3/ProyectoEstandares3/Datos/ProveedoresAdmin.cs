using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class ProveedoresAdmin
    {
        /// <summary>
        /// Consulta todos los datos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Proveedores> Consultar()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Proveedores.AsNoTracking().ToList();
            }
        }
        /// <summary>
        /// Guardar un registro en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(Proveedores modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Proveedores.Add(modelo);
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Para consultar un único registro de la tabla
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Proveedores ConsultaDetalle(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Proveedores.FirstOrDefault(p => p.Id == ID);
            }
        }
        /// <summary>
        /// Modifica los datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Modificar(Proveedores modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
                contexto.SaveChanges();
            }
        }
        /// <summary>
        /// Eliminar el registro seleccionado
        /// </summary>
        /// <param name="modelo"></param>
        public void Eliminar(Proveedores modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Deleted;
                contexto.SaveChanges();
            }
        }
    }
}