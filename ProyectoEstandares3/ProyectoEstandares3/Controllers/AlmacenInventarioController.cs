using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class AlmacenInventarioController : Controller
    {
        AlmacenInventarioAdmin usu = new AlmacenInventarioAdmin();
        AlmacenesAdmin alma = new AlmacenesAdmin();
        InventarioAdmin invent = new InventarioAdmin();
        // GET: Almacen_Inventario

        //Index para mostrar todos los datos de la tabla con los nombres de los registros de Almacenes e Inventario
        //en lugar de mostrar los IDs por defecto
        public ActionResult Index()
        {
            IEnumerable<Almacen_Inventario> lista = usu.Consultar();
            ICollection<Almacen_Inventario> listadoAlma = new List<Almacen_Inventario>();
            using (var sequenceEnum = lista.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    Almacen_Inventario modelo1 = sequenceEnum.Current;
                    Inventario inv = invent.ConsultaDetalle(modelo1.Id_inventario);
                    string nombreInv = inv.Descripcion;
                    Almacenes alm = alma.ConsultaDetalle(modelo1.Id_almacen);
                    string nombreAl = alm.Nombre;
                    Almacen_Inventario modeloNuevo = new Almacen_Inventario() { 
                        Id_almacen = modelo1.Id_almacen,
                        Almacen = nombreAl,
                        Id_inventario = modelo1.Id_inventario,
                        Inventario = nombreInv,
                        Existencia = modelo1.Existencia
                    };
                    listadoAlma.Add(modeloNuevo);
                }
            }
            return View(listadoAlma);
        }

        public ActionResult Guardar()
        {
            //Muestra un mensaje en general
            ViewBag.mensaje = "";
            return View();
        }

        //METODO CON VALIDACIONES
        public ActionResult Nuevo(Almacen_Inventario modelo)
        {
            //Se obtienen los datos del formulario enviado
            string elAlmacen = "" + modelo.Id_almacen;
            string elInventario = "" + modelo.Id_inventario;
            string laExistencia = "" + modelo.Existencia;
            int elIDInv = modelo.Id_inventario;

            //Si los campos no están vacíos
            if (!elAlmacen.Equals("") && !elInventario.Equals("") && !laExistencia.Equals(""))
            {
                //Creo un modelo de la clase de la tabla Inventario
                Inventario modeloInv = invent.ConsultaDetalle(elIDInv);
                //Obtengo el dato de la cantidad de existencias totales del producto que trato de ingresar
                int ExistenciaOriginal = modeloInv.Existencia;
                //Una variable auxiliar para todas las cantidades del producto previamente ingresadas en otros almacenes
                int ExistenciasRecientes = 0;
                //Consulto todas las existencias del producto desde otros almacenes por medio de este método
                //Como lo que obtengo de vuelta es una lista de las cantidades por separado, debo de sumar todo
                IEnumerable<int?> cantidades = usu.ConsultaExistencias(elIDInv);
                //Para eso ocupo un foreach
                foreach (int canti in cantidades)
                {
                    ExistenciasRecientes = ExistenciasRecientes + canti;
                }
                //Obtengo la existencia que yo ingresé desde el formulario
                int miExistencia = (int)modelo.Existencia;
                //Hago la suma de las cantidades existentes en otros almacenes y la cantidad a punto de ingresarse
                int NuevasExistencias = ExistenciasRecientes + miExistencia;
                //Se obtiene la cantidad que se permite ingresar según la cantidad total de existencias del producto
                //(Esto último para procurar que el usuario no "almacene" más unidades de las que realmente se cuentan del producto)
                int ExisPermitida = ExistenciaOriginal - ExistenciasRecientes;
                //Se compara si las nuevas existencias NO sobrepasan la cantidad total del producto
                if (NuevasExistencias <= ExistenciaOriginal)
                {
                    usu.Guardar(modelo);
                    ViewBag.mensaje = "Registro guardado con éxito";
                    ViewBag.alerta = "";
                }//Si no, entonces muestra un mensaje de error
                else
                {
                    ViewBag.mensaje = "";
                    ViewBag.alerta = "La cantidad de existencias del inventario especificado no es suficiente para ser guardado en este almacen. " + "Para el inventario que usted intenta ingresar al almacen, hay " + ExisPermitida + " unidades disponibles";
                }
            }//Si alguno de los campos del formulario está vacío, entonces se muestra un mensaje.
            else
            {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese todos los datos";
            }
            return View("Guardar", modelo);
        }

        public ActionResult Detalle(int id = 0)
        {
            Almacen_Inventario modelo = usu.ConsultaDetalle(id);
            return View(modelo);
        }
        
        public ActionResult Modificar(int id = 0)
        {
            Almacen_Inventario modelo = usu.ConsultaDetalle(id);
            Almacen_Inventario modelo2 = new Almacen_Inventario()
            {
                Id_almacen = modelo.Id_almacen,
                Id_inventario = modelo.Id_inventario,
                Existencia = 0
            };
            int? ExisAnterior = modelo.Existencia;
            ViewBag.ExisAnterior = ExisAnterior;
            ViewBag.mensaje = "";
            return View(modelo2);
        }

        //METODO CON VALIDACIONES
        public ActionResult Actualizar(Almacen_Inventario modelo, int Exis = 0)
        {
            // Envio a la vista la cantidad de existencias reciente de ese campo que se desea modificar
            int ExisAgregada;
            ViewBag.ExisAnterior = Exis;
            //Creo un nuevo modelo que terminaré mandando al método de modificación correspondiente
            Almacen_Inventario nuevoModelo;

            //Se obtienen los datos del form
            string laExistencia = "" + modelo.Existencia;
            int elIDInv = modelo.Id_inventario;

            //Si el campo del form no está vacío
            if (!laExistencia.Equals(""))
            {
                //Se obtienen todos los datos del producto en la tabla Inventario
                Inventario modeloInv = invent.ConsultaDetalle(elIDInv);
                //Se obtiene la existencia total de unidades de ese producto. 
                int ExistenciaOriginal = modeloInv.Existencia;
                //Una variable auxiliar para todas las cantidades del producto previamente ingresadas en otros almacenes
                int ExistenciasRecientes = 0;
                //Consulto todas las existencias del producto desde otros almacenes por medio de este método
                //Como lo que obtengo de vuelta es una lista de las cantidades por separado, debo de sumar todo
                IEnumerable<int?> cantidades = usu.ConsultaExistencias(elIDInv);
                //Para eso ocupo un foreach
                foreach (int canti in cantidades)
                {
                    ExistenciasRecientes = ExistenciasRecientes + canti;
                }

                //Obtengo la existencia que yo ingresé desde el formulario
                ExisAgregada = (int)modelo.Existencia;
                //Se suma la cantidad de existencias reciente mas la cantidad a la que se trata de modificar.
                int ExisPosible = Exis + ExisAgregada;
                //Hago la suma de las cantidades existentes en otros almacenes y la cantidad a punto de ingresarse
                int NuevasExistencias = ExistenciasRecientes + ExisAgregada;
                //Se obtiene la cantidad que se permite agregar según la cantidad total de existencias del producto
                int ExisPermitida = ExistenciaOriginal - ExistenciasRecientes;
                //Se verifica si se puede agregar o no
                if (NuevasExistencias <= ExistenciaOriginal)
                {
                    //Nos aseguramos que la cantidad nueva de productos en el almacen no es menor a cero
                    if (ExisPosible >= 0)
                    {
                        nuevoModelo = new Almacen_Inventario()
                        {
                            Id_almacen = modelo.Id_almacen,
                            Id_inventario = modelo.Id_inventario,
                            Existencia = Exis + ExisAgregada
                        };
                        usu.Modificar(nuevoModelo);
                        ViewBag.ExisAnterior = Exis + ExisAgregada;
                        ViewBag.mensaje = "Registro modificado con éxito";
                        ViewBag.alerta = "";
                    }
                    else
                    {
                        ViewBag.mensaje = "";
                        ViewBag.alerta = "No se puede restar más de la cantidad asignada al almacén.";
                    }
                }
                else
                {
                    ViewBag.mensaje = "";
                    ViewBag.alerta = "La cantidad por la que usted intenta modificar el registro excederá la cantidad de unidades existentes en total del producto. " + "Únicamente puede añadir hasta " + ExisPermitida + " unidades más al almacén.";
                }
            }
            else
            {
                ViewBag.mensaje = "";
                ViewBag.alerta = "Ingrese todos los datos";
            }
            return View("Modificar", modelo);
        }

        //ESTE MÉTODO AL PARECER NO SERÁ NECESARIO

        /*public ActionResult Eliminar(int id = 0)
        {
            Almacen_Inventario modelo = new Almacen_Inventario()
            {
                Id_almacen = id
            };
            usu.Eliminar(modelo);
            IEnumerable<Almacen_Inventario> lista = usu.Consultar();
            ViewBag.mensaje = "Registro eliminado con éxito";
            return View("Index", lista);
        }*/
    }
}