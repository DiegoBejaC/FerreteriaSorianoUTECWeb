using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class AlmacenInventarioAdmin
    {
        /// <summary>
        /// Consulta todos los datos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Almacen_Inventario> Consultar()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Almacen_Inventario.AsNoTracking().ToList();
            }
        }
        /// <summary>
        /// Guardar un registro en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(Almacen_Inventario modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Almacen_Inventario.Add(modelo);
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Para consultar un único registro de la tabla
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Almacen_Inventario ConsultaDetalle(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Almacen_Inventario.FirstOrDefault(p => p.Id_almacen == ID);
            }
        }
        /// <summary>
        /// Modifica los datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Modificar(Almacen_Inventario modelo)
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
        public void Eliminar(Almacen_Inventario modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Deleted;
                contexto.SaveChanges();
            }
        }

        //Método para obtener un unico campo de la tabla en donde el Id_almacen y el Id_inventario son iguales
        //a los que se pasarán por parámetro
        public Almacen_Inventario ConsultaEspecifica(int ID, int IdInv)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Almacen_Inventario.FirstOrDefault(p => p.Id_almacen == ID && p.Id_inventario == IdInv);
            }
        }

        //Método para obtener las existencias de aquellos valores de la tabla en donde el ID del inventario sea igual 
        public IEnumerable<int?> ConsultaExistencias(int id)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Almacen_Inventario.Where(c => c.Id_inventario == id).Select(c => c.Existencia).ToList();
            }
        }
    }
}