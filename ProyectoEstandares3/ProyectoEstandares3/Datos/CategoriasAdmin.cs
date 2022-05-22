using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class CategoriasAdmin
    {
        /// <summary>
        /// Consulta todos los datos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Categorias> Consultar()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Categorias.AsNoTracking().ToList();
            }
        }
        /// <summary>
        /// Guardar un registro en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(Categorias modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Categorias.Add(modelo);
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Para consultar un único registro de la tabla
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Categorias ConsultaDetalle(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Categorias.FirstOrDefault(p => p.Id == ID);
            }
        }
        /// <summary>
        /// Modifica los datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Modificar(Categorias modelo)
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
        public void Eliminar(Categorias modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Deleted;
                contexto.SaveChanges();
            }
        }
    }
}