using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoEstandares3.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult Indicacion() {
            return RedirectToAction("Index", new { message = "Iniciar sesión para continuar" });
        }

        [HttpPost]
        public ActionResult Login(string usuario, string clave)
        {
            //Se verifica si los campos de usuario y clave están vacíos.
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(clave))
            {
                //Si están llenos, se verifica si la información es correcta.
                
                //Aquí se crea el objeto de la clase de entidad, que contiene la conexión con la base de datos
                FerreSoriano17Entities db = new FerreSoriano17Entities();
                
                //Se crea la consulta para obtener el registro donde el usuario Y la clave sean iguales a los ingresados en el form.
                var user = db.Usuarios.FirstOrDefault(u => u.Usuario == usuario && u.Clave == clave);
                // Si esta consulta NO devuelve un valor nulo
                if (user != null)
                {
                    //Aquí se encontró al usuario con sus datos
                    //Se configura una cookie con la información del usuario, esta cookie se destruirá hasta que el usuario decida cerrar sesión
                    FormsAuthentication.SetAuthCookie(user.Usuario, true);
                    return RedirectToAction("Index", "Home");
                }
                else //Si no se encuentra el usuario en la base, muestra un mensaje de error
                {
                    return RedirectToAction("Index", new { message = "Usuario no registrado" });
                }
            }
            else //Si alguno de los campos está vacío, muestra un mensaje de error
            {
                return RedirectToAction("Index", new { message = "Por favor llene todos los campos" });
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut(); //Cuando se presione en "Cerrar sesión", se destruirá la cookie creada.
            return RedirectToAction("Index", "Home");
        }

    }
}