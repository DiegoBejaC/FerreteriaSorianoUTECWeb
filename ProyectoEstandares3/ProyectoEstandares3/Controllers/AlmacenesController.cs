using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class AlmacenesController : Controller
    {
        AlmacenesAdmin usu = new AlmacenesAdmin();
        // GET: Almacenes
        public ActionResult Index()
        {
            IEnumerable<Almacenes> lista = usu.Consultar();
            return View(lista);
        }

        public ActionResult Guardar()
        {
            ViewBag.mensaje = "";
            return View();
        }

        //METODO CON VALIDACIONES
        public ActionResult Nuevo(Almacenes modelo)
        {
            //Se obtiene el valor del form
            string elNombre = modelo.Nombre;

            //Si no está vacío
            if (elNombre != null)
            {
                //Se guarda la info
                usu.Guardar(modelo);
                ViewBag.mensaje = "Almacen guardado con éxito";
                ViewBag.alerta = "";
            }
            else
            {//Si no, muestra un mensaje de alerta
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese todos los datos";
            }
            return View("Guardar", modelo);
        }

        public ActionResult Detalle(int id = 0)
        {
            Almacenes modelo = usu.ConsultaDetalle(id);
            return View(modelo);
        }

        public ActionResult Modificar(int id = 0)
        {
            Almacenes modelo = usu.ConsultaDetalle(id);
            ViewBag.mensaje = "";
            return View(modelo);
        }

        //METODO CON VALIDACIONES
        //Funciona con la misma lógica que el método "Nuevo", no hace falta documentarlo
        public ActionResult Actualizar(Almacenes modelo)
        {
            string elNombre = modelo.Nombre;

            if (elNombre != null)
            {
                usu.Modificar(modelo);
                ViewBag.mensaje = "Almacén modificado con éxito";
                ViewBag.alerta = "";
            }
            else
            {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese todos los datos";
            }
            return View("Modificar", modelo);
        }

        public ActionResult Eliminar(int id = 0)
        {
            Almacenes modelo = new Almacenes()
            {
                Id = id
            };
            usu.Eliminar(modelo);
            IEnumerable<Almacenes> lista = usu.Consultar();
            ViewBag.mensaje = "Almacén eliminado con éxito";
            return View("Index", lista);
        }
    }
}