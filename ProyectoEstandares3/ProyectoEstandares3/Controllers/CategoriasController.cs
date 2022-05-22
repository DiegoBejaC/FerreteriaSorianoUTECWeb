using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class CategoriasController : Controller
    {
        CategoriasAdmin usu = new CategoriasAdmin();
        // GET: Categorias
        public ActionResult Index()
        {
            IEnumerable<Categorias> lista = usu.Consultar();
            return View(lista);
        }

        public ActionResult Guardar()
        {
            ViewBag.mensaje = "";
            return View();
        }
        
        //METODO CON VALIDACIONES
        public ActionResult Nuevo(Categorias modelo)
        {
            //Se obtiene el valor del form
            string laDescripcion = modelo.Descripcion;

            //Si no está vacío
            if (laDescripcion != null)
            {
                //Se guarda la info
                usu.Guardar(modelo);
                ViewBag.mensaje = "Categoría guardada con éxito";
                ViewBag.alerta = "";
            }//Si no, muestra un mensaje de alerta
            else
            {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese todos los datos";
            }
            return View("Guardar", modelo);
        }

        public ActionResult Detalle(int id = 0)
        {
            Categorias modelo = usu.ConsultaDetalle(id);
            return View(modelo);
        }

        public ActionResult Modificar(int id = 0)
        {
            Categorias modelo = usu.ConsultaDetalle(id);
            ViewBag.mensaje = "";
            return View(modelo);
        }

        //METODO CON VALIDACIONES
        //Funciona con la misma lógica que el método "Nuevo", no hace falta documentarlo
        public ActionResult Actualizar(Categorias modelo)
        {
            string laDescripcion = modelo.Descripcion;

            if (laDescripcion != null)
            {
                usu.Modificar(modelo);
                ViewBag.mensaje = "Categoria modificada con éxito";
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
            Categorias modelo = new Categorias()
            {
                Id = id
            };
            usu.Eliminar(modelo);
            IEnumerable<Categorias> lista = usu.Consultar();
            ViewBag.mensaje = "Categoria eliminada con éxito";
            return View("Index", lista);
        }
    }
}