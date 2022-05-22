using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class UsuariosController : Controller
    {
        UsuariosAdmin usu = new UsuariosAdmin();
        // GET: Usuarios
        public ActionResult Index()
        {
            IEnumerable<Usuarios> lista = usu.Consultar();
            return View(lista);
        }

        public ActionResult Guardar()
        {
            ViewBag.mensaje = "";
            return View();
        }

        //METODO CON VALIDACIONES
        public ActionResult Nuevo(Usuarios modelo)
        {
            //Obteniendo los valores ingresados desde el formulario
            string elUsuario = modelo.Usuario;
            string laClave = modelo.Clave;
            string elNombre = modelo.Nombre;
            string elNivel = "" + modelo.Nivel;

            //Se verifica si ninguno está vacío
            if (elUsuario != null && laClave != null && elNombre != null && !elNivel.Equals(""))
            {
                //Si no están vacíos, se verifica si la clave contiene 5 o más caracteres Y si el nivel está entre 1 y 3
                if (laClave.Length >= 5 && (elNivel.Equals("1") || elNivel.Equals("2") || elNivel.Equals("3")))
                {
                    //Si esto se logra, se guarda el registro en la base de datos
                    usu.Guardar(modelo);
                    ViewBag.mensaje = "Usuario guardado con éxito";
                    ViewBag.alerta = "";
                }
                else {//Si no, se determina cuál mensaje de error se devolverá
                    if (laClave.Length < 5)
                    {
                        if (!(elNivel.Equals("1") || elNivel.Equals("2") || elNivel.Equals("3")))
                        {
                            ViewBag.mensaje = "";
                            ViewBag.alerta = "La clave debe tener un mínimo de 5 caracteres y el nivel debe ser 1, 2 o 3";
                        }
                        else {
                            ViewBag.mensaje = "";
                            ViewBag.alerta = "La clave debe tener un mínimo de 5 caracteres";
                        }
                    }
                    else if (!(elNivel.Equals("1") || elNivel.Equals("2") || elNivel.Equals("3")))
                    {
                        ViewBag.mensaje = "";
                        ViewBag.alerta = "El nivel debe ser 1, 2 o 3";
                    }           
                }
            }
            else {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese todos los datos";
            }
            return View("Guardar", modelo);
        }

        public ActionResult Detalle(int id = 0)
        {
            Usuarios modelo = usu.ConsultaDetalle(id);
            return View(modelo);
        }

        public ActionResult Modificar(int id=0)
        {
            Usuarios modelo = usu.ConsultaDetalle(id);
            ViewBag.mensaje = "";
            return View(modelo);
        }

        //METODO CON VALIDACIONES
        public ActionResult Actualizar(Usuarios modelo)
        {
            //Obteniendo los valores ingresados desde el formulario
            string elUsuario = modelo.Usuario;
            string laClave = modelo.Clave;
            string elNombre = modelo.Nombre;
            string elNivel = "" + modelo.Nivel;

            //Se verifica si ninguno está vacío
            if (elUsuario != null && laClave != null && elNombre != null && !elNivel.Equals(""))
            {
                //Si no están vacíos, se verifica si la clave contiene 5 o más caracteres Y si el nivel está entre 1 y 3
                if (laClave.Length >= 5 && (elNivel.Equals("1") || elNivel.Equals("2") || elNivel.Equals("3")))
                {
                    //Si esto se logra, se modifica el registro en la base de datos
                    usu.Modificar(modelo);
                    ViewBag.mensaje = "Usuario modificado con éxito";
                }
                else
                {//Si no, se determina cuál mensaje de error se devolverá
                    if (laClave.Length < 5)
                    {
                        if (!(elNivel.Equals("1") || elNivel.Equals("2") || elNivel.Equals("3")))
                        {
                            ViewBag.mensaje = "";
                            ViewBag.alerta = "La clave debe tener un mínimo de 5 caracteres y el nivel debe ser 1, 2 o 3";
                        }
                        else
                        {
                            ViewBag.mensaje = "";
                            ViewBag.alerta = "La clave debe tener un mínimo de 5 caracteres";
                        }
                    }
                    else if (!(elNivel.Equals("1") || elNivel.Equals("2") || elNivel.Equals("3")))
                    {
                        ViewBag.mensaje = "";
                        ViewBag.alerta = "El nivel debe ser 1, 2 o 3";
                    }
                }
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
            Usuarios modelo = new Usuarios()
            {
                Id = id
            };
            usu.Eliminar(modelo);
            IEnumerable<Usuarios> lista = usu.Consultar();
            ViewBag.mensaje = "Usuario eliminado con éxito";
            return View("Index", lista);
        }
    }
}