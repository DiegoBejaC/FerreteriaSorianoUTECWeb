using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class ComprasAdmin
    {
        /// <summary>
        /// Consulta todos los datos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Compras> Consultar()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Compras.AsNoTracking().ToList();
            }
        }
        //Método para obtener sólo los IDS de todos los registros de la tabla de Compras.
        public IEnumerable<int> ConsultarIDS()
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Compras.Select(c => c.Id).ToList();
            }
        }
        /// <summary>
        /// Guardar un registro en la base de datos
        /// </summary>
        /// <param name="modelo"></param>
        public void Guardar(Compras modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Compras.Add(modelo);
                contexto.SaveChanges();
            }
        }
        //En este método se obtienen todos los campos de un único registro de la tabla Compras
        //Con tal de poderlos mostrar durante el proceso de agregar los detalles de toda la compra.
        public Compras ObtenerDetalles(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                //return (Compras)contexto.Compras.Where(p => p.Id == ID).Select(c => c.Documento);
                return contexto.Compras.FirstOrDefault(p => p.Id == ID);
            }
        }

        //Metodo para obtener el ID del último registro ingresado en la tabla.
        //Esto para poder generar la etiqueta de Documento para la siguiente compra.
        public Compras ObtenerUltimo() {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                //return (Compras)contexto.Compras.Where(p => p.Id == ID).Select(c => c.Documento);
                return (Compras)contexto.Compras.OrderByDescending(x => x.Id).FirstOrDefault();
            }
        }

        //Para modificar un único registro de la tabla.
        public void Modificar(Compras modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
                contexto.SaveChanges();
            }
        }

    }
}