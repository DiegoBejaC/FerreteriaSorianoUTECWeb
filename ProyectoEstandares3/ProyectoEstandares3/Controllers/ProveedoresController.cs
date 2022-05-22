using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class ProveedoresController : Controller
    {
        ProveedoresAdmin usu = new ProveedoresAdmin();
        // GET: Proveedores
        public ActionResult Index()
        {
            IEnumerable<Proveedores> lista = usu.Consultar();
            return View(lista);
        }

        public ActionResult Guardar()
        {
            ViewBag.mensaje = "";
            return View();
        }
        
        //METODO CON VALIDACIONES
        public ActionResult Nuevo(Proveedores modelo)
        {
            //Se obtienen los datos ingresados desde el formulario
            string elNombre = modelo.Nombre;
            string elTelefono = modelo.Telefono;
            string laDireccion = modelo.Direccion;

            //Se verifica si están vacíos o no
            if (elNombre != null && elTelefono != null && laDireccion != null)
            {
                //Si no están vacíos, entonces se verifica si el teléfono contiene 8 caracteres
                if (elTelefono.Length == 8)
                {
                    //Si es así, se verifica si los caracteres son SÓLO NUMEROS, y NO letras.
                    Boolean noNumero = false; //Booleano que ayudará a determinar lo anterior
                    char[] digitos = elTelefono.ToCharArray(); //Se convierte la cadena del teléfono a un arreglo de caracteres

                    //Se recorre cada caracter para ver si es una letra o un número
                    for (int i = 0; i < 8; i++) {
                        //Si es un número, el método devolverá FALSE
                        noNumero = Char.IsLetter(digitos[i]);
                        if (noNumero) {//Si encuentra una letra, este IF se ejecuta y se termina el bucle FOR
                            i = 8;
                        }
                    }

                    //Al final, se verifica si "noNumero" es falso, si es falso, entonces quiere decir que el teléfono sólo tiene números
                    if (!noNumero) {
                        //Si es así, se guarda el registro en la BD
                        usu.Guardar(modelo);
                        ViewBag.mensaje = "Proveedor guardado con éxito";
                        ViewBag.alerta = "";
                    }//Si no, no.
                    else {
                        ViewBag.mensaje = "";
                        ViewBag.alerta = "El campo Teléfono debe contener SÓLO números";
                    }
                }
                else {
                    ViewBag.mensaje = "";
                    ViewBag.alerta = "El teléfono debe contener 8 caracteres numéricos";
                }
            }
            else
            {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese todos los datos";
            }
            return View("Guardar", modelo);
        }

        public ActionResult Detalle(int id = 0)
        {
            Proveedores modelo = usu.ConsultaDetalle(id);
            return View(modelo);
        }

        public ActionResult Modificar(int id = 0)
        {
            Proveedores modelo = usu.ConsultaDetalle(id);
            ViewBag.mensaje = "";
            return View(modelo);
        }

        //METODO CON VALIDACIONES:
        //Sigue la misma lógica que el método "Nuevo".
        public ActionResult Actualizar(Proveedores modelo)
        {
            string elNombre = modelo.Nombre;
            string elTelefono = modelo.Telefono;
            string laDireccion = modelo.Direccion;

            if (elNombre != null && elTelefono != null && laDireccion != null)
            {
                if (elTelefono.Length == 8)
                {
                    Boolean noNumero = false;
                    char[] digitos = elTelefono.ToCharArray();

                    for (int i = 0; i < 8; i++)
                    {
                        noNumero = Char.IsLetter(digitos[i]);
                        if (noNumero)
                        {
                            i = 8;
                        }
                    }

                    if (!noNumero)
                    {
                        usu.Modificar(modelo);
                        ViewBag.mensaje = "Proveedor modificado con éxito";
                        ViewBag.alerta = "";
                    }
                    else
                    {
                        ViewBag.mensaje = "";
                        ViewBag.alerta = "El campo Teléfono debe contener SÓLO números";
                    }
                }
                else
                {
                    ViewBag.mensaje = "";
                    ViewBag.alerta = "El teléfono debe contener 8 caracteres numéricos";
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
            Proveedores modelo = new Proveedores()
            {
                Id = id
            };
            usu.Eliminar(modelo);
            IEnumerable<Proveedores> lista = usu.Consultar();
            ViewBag.mensaje = "Proveedor eliminado con éxito";
            return View("Index", lista);
        }
    }
}