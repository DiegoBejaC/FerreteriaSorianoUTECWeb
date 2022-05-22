using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class AlmacenesAdmin
    {
        /// <summary>
        /// Consulta todos los datos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Almacenes> Consultar()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Almacenes.AsNoTracking().ToList();
            }
        }
        //Para obtener SOLO los IDS
        public IEnumerable<int> ConsultarIDS()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Almacenes.Select(c => c.Id).ToList();
            }
        }
        /// <summary>
        /// Guardar un registro en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(Almacenes modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Almacenes.Add(modelo);
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Para consultar un único registro de la tabla
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Almacenes ConsultaDetalle(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Almacenes.FirstOrDefault(p => p.Id == ID);
            }
        }
        /// <summary>
        /// Modifica los datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Modificar(Almacenes modelo)
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
        public void Eliminar(Almacenes modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Deleted;
                contexto.SaveChanges();
            }
        }
    }
}