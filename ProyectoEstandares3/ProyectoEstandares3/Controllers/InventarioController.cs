using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class InventarioController : Controller
    {
        InventarioAdmin usu = new InventarioAdmin();
        KardexAdmin kar = new KardexAdmin();
        AlmacenesAdmin alma = new AlmacenesAdmin();
        // GET: Inventario
        public ActionResult Index()
        {
            IEnumerable<Inventario> lista = usu.Consultar();
            return View(lista);
        }

        public ActionResult Guardar()
        {
            ViewBag.mensaje = "";
            return View();
        }

        //METODO CON VALIDACIONES
        public ActionResult Nuevo(Inventario modelo)
        {
            //se obtiene lo que el usuario envia desde el formulario
            string laDescripcion = modelo.Descripcion;
            //Estos campos ya estaban validados automáticamente al crear las vistas con el modelo de esta tabla
            //Por ello no fue necesario validar si están vacíos o no.
            int SMin = modelo.StockMinimo;
            int SMax = modelo.StockMaximo;
            int Exis = modelo.Existencia;
            
            //Si el campo de Descripcion está vacío
            if (laDescripcion != null)
            {
                //Si el stock mínimo es menor que el stock máximo
                if (SMin < SMax) {
                    //Si la cantidad de existencia del producto está entre los dos stocks
                    if (Exis >= SMin && Exis <= SMax) {
                        //Se guarda la info
                        usu.Guardar(modelo);
                        ViewBag.mensaje = "Producto guardado con éxito";
                        ViewBag.alerta = "";
                    }//Si no, muestra un mensaje
                    else {
                        ViewBag.mensaje = "";
                        ViewBag.alerta = "La cantidad del producto debe de estar entre los valores mínimo y máximo establecidos";
                    }

                }//Si no, muestra un mensaje
                else {
                    ViewBag.mensaje = "";
                    ViewBag.alerta = "El stock mínimo debe ser menor que el stock máximo";
                }
            }//Si no, muestra un mensaje
            else
            {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese el nombre / descripción del producto";
            }
            return View("Guardar", modelo);
        }

        public ActionResult Detalle(int id = 0)
        {
            Inventario modelo = usu.ConsultaDetalle(id);
            return View(modelo);
        }

        public ActionResult Modificar(int id = 0)
        {
            Inventario modelo = usu.ConsultaDetalle(id);
            ViewBag.mensaje = "";
            return View(modelo);
        }

        //METODO CON VALIDACIONES
        //Funciona con la misma lógica que el método "Nuevo", no hace falta documentarlo.
        public ActionResult Actualizar(Inventario modelo)
        {
            string laDescripcion = modelo.Descripcion;
            int SMin = modelo.StockMinimo;
            int SMax = modelo.StockMaximo;
            int Exis = modelo.Existencia;

            if (laDescripcion != null)
            {
                if (SMin < SMax)
                {

                    if (Exis >= SMin && Exis <= SMax)
                    {
                        usu.Modificar(modelo);
                        ViewBag.mensaje = "Producto modificado con éxito";
                        ViewBag.alerta = "";
                    }
                    else
                    {
                        ViewBag.mensaje = "";
                        ViewBag.alerta = "La cantidad del producto debe de estar entre los valores mínimo y máximo establecidos";
                    }

                }
                else
                {
                    ViewBag.mensaje = "";
                    ViewBag.alerta = "El stock mínimo debe ser menor que el stock máximo";
                }
            }
            else
            {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese el nombre / descripción del producto";
            }
            return View("Modificar", modelo);
        }

        public ActionResult Eliminar(int id = 0)
        {
            Inventario modelo = new Inventario()
            {
                Id = id
            };
            usu.Eliminar(modelo);
            IEnumerable<Inventario> lista = usu.Consultar();
            ViewBag.mensaje = "Producto eliminado con éxito";
            return View("Index", lista);
        }
        
        // GET: Kardex

        //Método para mostrar los detalles del Kardex del producto seleccionado.
        public ActionResult MostrarKardex(int id = 0)
        {
            IEnumerable<int> lista = kar.ConsultarIDS(id);
            ICollection<Kardex> listadoKar = new List<Kardex>();
            Inventario mInv = usu.ConsultaDetalle(id);
            int IdInv = mInv.Id;
            ViewBag.IDProd = IdInv;
            string nombreInv = mInv.Descripcion;
            ViewBag.Producto = nombreInv;
            int StoMin = mInv.StockMinimo;
            ViewBag.StockMin = StoMin;
            int StoMax = mInv.StockMaximo;
            ViewBag.StockMax = StoMax;
            int Cantid = mInv.Existencia;
            ViewBag.Existencia = Cantid;
            string nombreAl = "";
            string elTipo = "";
            foreach (int dato in lista) {
                Kardex mKar = kar.ConsultaDetalle(dato);

                int idEg = mKar.Id_hojaEgreso;
                int idComp = mKar.Id_compra;

                if (idEg == 0)
                {
                    elTipo = "ENTRADA (DOC-" + idComp + ")";
                }
                else {
                    elTipo = "SALIDA (Egreso #" + idEg + ")";
                }
                
                Almacenes mAl = alma.ConsultaDetalle(mKar.Id_almacen);
                nombreAl = mAl.Nombre;

                Kardex modeloKar = new Kardex()
                {
                    Id_kardex = mKar.Id_kardex,
                    Tipo = elTipo,
                    Almacen = nombreAl,
                    Cantidad = mKar.Cantidad,
                    Precio = mKar.Precio,
                    PrecioTotal = mKar.PrecioTotal,
                    Fecha = mKar.Fecha,
                    CantidadNueva = mKar.CantidadNueva,
                    PrecioNuevo = mKar.Precio,
                    PrecioTotalNuevo = mKar.CantidadNueva * mKar.Precio
                };
                listadoKar.Add(modeloKar);
            }
            return View(listadoKar);
        }

        //Similar al método "MostrarKardex", pero este va dirigido a la GENERACIÓN DEL REPORTE que luego
        //se guardará como PDF. Esta vista no será accesible por ningun usuario, sino que servirá de plantilla
        //para crear el archivo del reporte.
        public ActionResult ReporteKardex(int id = 0)
        {
            IEnumerable<int> lista = kar.ConsultarIDS(id);
            ICollection<Kardex> listadoKar = new List<Kardex>();
            Inventario mInv = usu.ConsultaDetalle(id);
            int IdInv = mInv.Id;
            ViewBag.IDProd = IdInv;
            string nombreInv = mInv.Descripcion;
            ViewBag.Producto = nombreInv;
            int StoMin = mInv.StockMinimo;
            ViewBag.StockMin = StoMin;
            int StoMax = mInv.StockMaximo;
            ViewBag.StockMax = StoMax;
            int Cantid = mInv.Existencia;
            ViewBag.Existencia = Cantid;
            string nombreAl = "";
            string elTipo = "";
            foreach (int dato in lista)
            {
                Kardex mKar = kar.ConsultaDetalle(dato);

                int idEg = mKar.Id_hojaEgreso;
                int idComp = mKar.Id_compra;

                if (idEg == 0)
                {
                    elTipo = "ENTRADA (DOC-" + idComp + ")";
                }
                else
                {
                    elTipo = "SALIDA (Egreso #" + idEg + ")";
                }

                Almacenes mAl = alma.ConsultaDetalle(mKar.Id_almacen);
                nombreAl = mAl.Nombre;

                Kardex modeloKar = new Kardex()
                {
                    Id_kardex = mKar.Id_kardex,
                    Tipo = elTipo,
                    Almacen = nombreAl,
                    Cantidad = mKar.Cantidad,
                    Precio = mKar.Precio,
                    PrecioTotal = mKar.PrecioTotal,
                    Fecha = mKar.Fecha,
                    CantidadNueva = mKar.CantidadNueva,
                    PrecioNuevo = mKar.Precio,
                    PrecioTotalNuevo = mKar.CantidadNueva * mKar.Precio
                };
                listadoKar.Add(modeloKar);
            }
            return View(listadoKar);
        }

        //Con este método se crea el reporte y se descarga el archivo PDF resultante con el nombre especificado en FileName
        public ActionResult ImprimirKardex(int elID = 0) {
            return new ActionAsPdf("ReporteKardex", new { id = elID }) {
                FileName = "KarReport-" + System.DateTime.Today
            };
        }
    }
}