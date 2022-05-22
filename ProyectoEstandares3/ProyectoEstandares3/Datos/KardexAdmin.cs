using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoEstandares3.Datos
{
    public class KardexAdmin
    {
        /// Guardar un registro en la base de datos
        public void Guardar(Kardex modelo)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                contexto.Kardex.Add(modelo);
                contexto.SaveChanges();
            }
        }
        //Método para buscar un único registro de la tabla.
        public Kardex ConsultaEspecifica(int idHoja, int idInv, int idComp, int idAl, int Cantidad)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Kardex.FirstOrDefault(p => p.Id_hojaEgreso == idHoja && p.Id_inventario == idInv && p.Id_compra == idComp && p.Id_almacen == idAl && p.Cantidad == Cantidad);
            }
        }

        //Metodo para consultar todos los datos

        public IEnumerable<Kardex> Consultar(int id)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Kardex.Where(c => c.Id_inventario == id).ToList();
            }
        }

        //Metodo para consultar todos los datos

        public IEnumerable<int> ConsultarIDS(int id)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Kardex.Where(c => c.Id_inventario == id).Select(c => c.Id_kardex).ToList();
            }
        }

        //Método para buscar un registro sólo con el ID
        public Kardex ConsultaDetalle(int ID)
        {
            using (FerreSoriano17Entities contexto = new FerreSoriano17Entities())
            {
                return contexto.Kardex.FirstOrDefault(p => p.Id_kardex == ID);
            }
        }

    }
}