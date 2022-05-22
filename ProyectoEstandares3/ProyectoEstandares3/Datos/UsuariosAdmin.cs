using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class UsuariosAdmin
    {
        /// <summary>
        /// Consulta todos los usuarios
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usuarios> Consultar()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Usuarios.AsNoTracking().ToList();
            }
        }
        /// <summary>
        /// Guardar un usuarios en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(Usuarios modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Usuarios.Add(modelo);
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Para consultar un único registro de la tabla
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Usuarios ConsultaDetalle(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Usuarios.FirstOrDefault(u => u.Id == ID);
            }
        }
        /// <summary>
        /// Modifica los datos de los usuarios
        /// </summary>
        /// <param name="modelo"></param>
        public void Modificar(Usuarios modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
                contexto.SaveChanges();
            }
        }
        /// <summary>
        /// Eliminar el usuarios seleccionado
        /// </summary>
        /// <param name="modelo"></param>
        public void Eliminar(Usuarios modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Deleted;
                contexto.SaveChanges();
            }
        }
    }
}