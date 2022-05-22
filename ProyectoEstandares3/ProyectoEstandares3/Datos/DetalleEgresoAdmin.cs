using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class DetalleEgresoAdmin
    {
        /// <summary>
        /// Consulta sólo aquellos registros donde el ID del documento sea igual al de una compra específica.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DetalleEgreso> ConsultaDetalles(int id)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.DetalleEgreso.Where(c => c.Id_hojaEgreso == id).ToList();
            }
        }

        /// <summary>
        /// Guardar un registro en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(DetalleEgreso modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.DetalleEgreso.Add(modelo);
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Eliminar el registro seleccionado
        /// </summary>
        /// <param name="modelo"></param>
        /*public void Eliminar(ComprasDetalle modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Deleted;
                contexto.SaveChanges();
            }
        }*/

        //Obtener un listado de todos los IDS de registro de todas las filas
        //de la tabla ComprasDetalle donde el Id_documento sea igual al de la compra que en ese momento
        //Se estña realizando.
        //Similar al método ConsultaDetalles pero obteniendo únicamente los IDs de las filas.
        public IEnumerable<int> ConsultaIDS(int id)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.DetalleEgreso.Where(c => c.Id_hojaEgreso == id).Select(c => c.Id_detalle).ToList();
            }
        }
        //Método para obtener el modelo de un único registro de la tabla ComprasDetalle
        public DetalleEgreso ConsultaUnicoRegistro(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.DetalleEgreso.FirstOrDefault(p => p.Id_detalle == ID);
            }
        }
    }
}