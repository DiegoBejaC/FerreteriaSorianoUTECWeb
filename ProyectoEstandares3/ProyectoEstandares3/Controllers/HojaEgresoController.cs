using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class HojaEgresoController : Controller
    {
        //Un objeto del tipo de entidad, no lo he borrado por si acaso.
        FerreSoriano17Entities db = new FerreSoriano17Entities();

        //Objetos de las clases de la capa de datos
        HojaEgresoAdmin hoja = new HojaEgresoAdmin();
        DetalleEgresoAdmin detHoja = new DetalleEgresoAdmin();
        InventarioAdmin inven = new InventarioAdmin();
        AlmacenesAdmin alll = new AlmacenesAdmin();
        AlmacenInventarioAdmin alma = new AlmacenInventarioAdmin();
        KardexAdmin kar = new KardexAdmin();

        // GET: HojaEgreso

        //Index para mostrar la tabla de datos "HojaEgreso"
        public ActionResult Index(String mensaje = "")
        {
            ViewBag.confirmacion = mensaje;
            IEnumerable<HojaEgreso> lista = hoja.Consultar();
            return View(lista);
        }

        //public ActionResult ObtenerUltimoID(HojaEgreso modelo) {
        //    return RedirectToAction("Guardar", new { id = modelo.Id });
        //}


        //Método que abre la vista "Guardar", con un formulario para guardar los
        //de la compra.
        public ActionResult Guardar()
        {
            //HojaEgreso modelo = new HojaEgreso() { 
            //    Documento = "DOC-" + (id+1)
            //};
            return View();
        }

        //Al enviar los datos de la compra nueva, se guardan en la tabla de la BD
        //Y se llama a la vista "RegistrosEgreso", la cual muestra los datos de 
        //HojaEgresoDetalle ligados a la compra recíen ingresada por el usuario
        //Por ello se envia un argumento del ID de la compra ingresada.
        public ActionResult NuevoEgreso(HojaEgreso modelo)
        {
            string laFecha = "" + modelo.Fecha;
            if (!laFecha.Equals(""))
            {
                hoja.Guardar(modelo);
                ViewBag.alerta = "";
                return RedirectToAction("RegistrosEgreso", new { id = modelo.Id, total = 0 });
            }
            else
            {
                ViewBag.alerta = "Debe ingresar una fecha";
                return View("Guardar");
            }
        }

        //Método para guardar los registros de los detalles de la compra recién ingresada
        //Esta vista muestra los datos de la compra a realizar. Y abajo muestra una tabla
        //Que permitirá al usuario ver los registros de aquellos productos que formarán
        //Parte de la compra, con sus cantidades y precio unitario. Además del almacén en donde
        //Estos productos serán guardados.

        //Aquí se usa ViewBag para poder enviar como propiedades los detalles de HojaEgreso
        //Y se envía un modelo de una colección de datos para llenar la tabla para HojaEgresoDETALLE
        public ActionResult RegistrosEgreso(int id = 0, double total = 0)
        {
            //ESTAS SÓLO FUERON PRUEBAS, ESTAS LÍNEAS NO FUNCIONAN
            //ViewBag.CompraDocumento = "DOC00001";
            //ViewBag.CompraProveedor = "Empresaxyz";
            //-------------------
            //ViewBag.CompraDocumento = comp.ConsultaDocumento(id);
            //ViewBag.CompraProveedor = comp.ConsultaProveedor(id).ToString();

            //ESTAS LÍNEAS SI FUNCIONAN PERFECTAMENTE

            //Se crea un modelo de la clase HojaEgreso que almacena los datos obtenidos por el método "ObtenerDetalles"
            HojaEgreso modelo = hoja.ObtenerDetalles(id);
            //Se almacenan algunas propiedades del modelo en variables del mismo tipo de dato
            //para luego ser almacenados en variables ViewBag
            int comID = (int)modelo.Id;
            ViewBag.EgresoID = comID;
            DateTime ComF = (DateTime)modelo.Fecha;
            String FechaNueva = ComF.Date.ToShortDateString();
            ViewBag.EgresoFecha = FechaNueva;
            ViewBag.FechaNoVisible = ComF;

            ViewBag.EgresoPrecioTotal = total;

            //Colección de datos de HojaEgresoDetalle
            IEnumerable<DetalleEgreso> lista = detHoja.ConsultaDetalles(id);
            ICollection<DetalleEgreso> listadoDetalles = new List<DetalleEgreso>();
            using (var sequenceEnum = lista.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    DetalleEgreso modelo1 = sequenceEnum.Current;
                    Inventario inv = inven.ConsultaDetalle(modelo1.Id_inventario);
                    string nombreInv = inv.Descripcion;
                    Almacenes alm = alll.ConsultaDetalle(modelo1.Id_almacen);
                    string nombreAl = alm.Nombre;
                    DetalleEgreso modeloNuevo = new DetalleEgreso()
                    {
                        Id_detalle = modelo1.Id_detalle,
                        Id_hojaEgreso = modelo1.Id_hojaEgreso,
                        Id_almacen = modelo1.Id_almacen,
                        Almacen = nombreAl,
                        Id_inventario = modelo1.Id_inventario,
                        Inventario2 = nombreInv,
                        Cantidad = modelo1.Cantidad,
                        Precio = modelo1.Precio,
                        PrecioTotal = modelo1.PrecioTotal
                    };
                    listadoDetalles.Add(modeloNuevo);
                }
            }
            return View(listadoDetalles);
        }

        //Método que permite agregar el registro de un producto con su cantidad, precio y almacén a una compra específica.
        public ActionResult AgregarRegistrosEgreso(int id = 0, int idinventario = 0, int idalmacen = 0, int cantid = 0, int? prec = 0, double total = 0, string mensaje = "")
        {
            DetalleEgreso modelo = new DetalleEgreso()
            {
                Id_hojaEgreso = id,
                Id_inventario = idinventario,
                Id_almacen = idalmacen,
                Cantidad = cantid,
                Precio = prec
            };
            ViewBag.EgresoPrecioTotal = total;
            ViewBag.alerta = mensaje;
            //HojaEgreso modelo = comp.ObtenerDetalles(id);
            //int comID = (int)modelo.Id;
            //ViewBag.CompraID = comID;
            return View(modelo);
        }

        //Este método se ejecuta cuando se manda el formulario de la vista "AgregarRegistrosEgreso", se guarda
        //el nuevo registro en la tabla "HojaEgresoDetalle" y se redirecciona a la vista "RegistrosEgreso"
        //porque en ese instante todavía se sigue rellenando los detalles del egreso.
        public ActionResult NuevoRegistrosEgreso(DetalleEgreso modelo, double total = 0)
        {
            //Se obtienen los detalles del formulario
            string laCantidad = "" + modelo.Cantidad;
            int? CantidadDefecto = 0 + modelo.Cantidad;
            string elPrecio = "" + modelo.Precio;
            decimal? PrecioDefecto = 0 + modelo.Precio;
            int IdInv = modelo.Id_inventario;
            int IdAl = modelo.Id_almacen;
            double todoElPrecio;

            //Se chequea si los campos no están vacíos
            if (!laCantidad.Equals("") && !elPrecio.Equals(""))
            {
                //Se chequea si los campos no contienen cero
                if (CantidadDefecto != 0 && PrecioDefecto != 0)
                {
                    Inventario modeloInv = inven.ConsultaDetalle(IdInv);
                    int stockMini = modeloInv.StockMinimo;
                    int Existencias = modeloInv.Existencia;
                    int nuevaExis = (int)modelo.Cantidad;
                    int posibleExis = Existencias - nuevaExis;

                    Almacen_Inventario modeloAlma = alma.ConsultaEspecifica(IdAl, IdInv);
                    int? cantAlmacen = modeloAlma.Existencia;

                    int? nuevaCantAlmacen = cantAlmacen - nuevaExis;

                    int Disponible = Existencias - stockMini;

                    if (cantAlmacen >= nuevaExis)
                    {
                        if (posibleExis >= stockMini)
                        {
                            double Cant2 = (double)modelo.Cantidad;
                            double Prec2 = (double)modelo.Precio;
                            double PrecioT2 = Cant2 * Prec2;
                            DetalleEgreso modelo3 = new DetalleEgreso()
                            {
                                Id_hojaEgreso = modelo.Id_hojaEgreso,
                                Id_inventario = modelo.Id_inventario,
                                Id_almacen = modelo.Id_almacen,
                                Cantidad = modelo.Cantidad,
                                Precio = modelo.Precio,
                                PrecioTotal = (decimal?)PrecioT2
                            };
                            //Se guarda el registro en la tabla DetalleEgreso
                            detHoja.Guardar(modelo3);

                            //Aquí actualizaremos los datos del registro de la tabla Inventario en donde el ID_inventario sea igual al que se ha registrado en este detalle del egreso
                            var resultado = (from p in db.Inventario
                                             where p.Id == IdInv
                                             select p).SingleOrDefault();
                            resultado.Existencia = posibleExis;
                            db.SaveChanges();


                            //Aquí actualizaremos los datos del registro de la tabla Almacen_Inventario
                            var resultado2 = (from p in db.Almacen_Inventario
                                              where p.Id_almacen == IdAl && p.Id_inventario == IdInv
                                              select p).SingleOrDefault();
                            resultado2.Existencia = nuevaCantAlmacen;
                            db.SaveChanges();

                            //Aquí mandamos los datos a la tabla "Kardex"
                            HojaEgreso modelolast = hoja.ObtenerDetalles(modelo.Id_hojaEgreso);
                            DateTime ComF = (DateTime)modelolast.Fecha;

                            Kardex modeloKar = new Kardex()
                            {
                                Id_hojaEgreso = modelo.Id_hojaEgreso,
                                Id_inventario = modelo.Id_inventario,
                                Id_compra = 0,
                                Id_almacen = modelo.Id_almacen,
                                Cantidad = modelo.Cantidad,
                                Precio = modelo.Precio,
                                PrecioTotal = (decimal?)PrecioT2,
                                Fecha = ComF,
                                CantidadNueva = posibleExis
                            };
                            kar.Guardar(modeloKar);

                            todoElPrecio = total + PrecioT2;
                            ViewBag.alerta = "";
                            return RedirectToAction("RegistrosEgreso", new { id = modelo.Id_hojaEgreso, total = todoElPrecio });
                        }
                        else
                        {
                            ViewBag.alerta = "No puede exceder el stock mínimo para este producto. Sólo puede egresar " + Disponible + " unidades.";
                            return RedirectToAction("AgregarRegistrosEgreso", new { id = modelo.Id_hojaEgreso, idinventario = modelo.Id_inventario, idalmacen = modelo.Id_almacen, cantid = modelo.Cantidad, prec = modelo.Precio, total = total, mensaje = ViewBag.alerta });
                        }
                    }
                    else {
                        ViewBag.alerta = "No puede egresar más unidades de la que ya existen en este almacen. Solo puede extraer " + cantAlmacen + " unidades de este almacen.";
                        return RedirectToAction("AgregarRegistrosEgreso", new { id = modelo.Id_hojaEgreso, idinventario = modelo.Id_inventario, idalmacen = modelo.Id_almacen, cantid = modelo.Cantidad, prec = modelo.Precio, total = total, mensaje = ViewBag.alerta });
                    }  
                }
                else
                {
                    ViewBag.alerta = "La cantidad y el precio no pueden ser cero";
                    return RedirectToAction("AgregarRegistrosEgreso", new { id = modelo.Id_hojaEgreso, idinventario = modelo.Id_inventario, idalmacen = modelo.Id_almacen, cantid = modelo.Cantidad, prec = modelo.Precio, total = total, mensaje = ViewBag.alerta });
                }
            }
            else
            {
                ViewBag.alerta = "Ingrese por favor la cantidad y precio unitario del producto";
                return RedirectToAction("AgregarRegistrosEgreso", new { id = modelo.Id_hojaEgreso, idinventario = modelo.Id_inventario, idalmacen = modelo.Id_almacen, cantid = modelo.Cantidad, prec = modelo.Precio, total = total, mensaje = ViewBag.alerta });
            }
        }

        //Metodo que se ejecuta cuando se le da fin al registro total de la compra con todos sus detalles.
        //Se redirecciona a la vista Index con un mensaje de confirmación.
        public ActionResult EgresoFinalizado(DateTime fecha, int id = 0, double total = 0)
        {
            //Se manda el dato de la suma total al egreso realizado
            HojaEgreso modelo = new HojaEgreso()
            {
                Id = id,
                Fecha = fecha,
                Suma = (decimal?)total
            };
            hoja.Modificar(modelo);

            //ESTAS LÍNEAS ESTÁN OBSOLETAS -------------------------------------------
            /*
            //Aquí se modificarán las existencias en las tablas Inventario y Almacen_inventario para el producto correspondiente
            IEnumerable<int> listaIDS = detHoja.ConsultaIDS(id); //Para obtener los IDs de los registros de DetalleEgreso
                                                                 //adjuntos a la compra en curso
            int CantReciente = 0; //La cantidad ingresada en el registro del detalle del egreso
            int IdInvReciente = 0; //El ID de inventario ingresado en el registro del detalle del egreso
            int CantOriginal = 0; //La cantidad original de existencias del producto en la tabla Inventario
            int IdAlReciente = 0; //El ID del almacen ingresado en el registro del detalle del egreso
            int NuevaCantidad = 0; //La nueva cantidad a actualizarse en la existencia del registro en la tabla Inventario
            int CantidadAlmacen = 0; //La cantidad original de existencias del producto en la tabla Almacen_inventario
            int NuevaCantidadAlmacen = 0; //La nueva cantidad a actualizarse en la existencia del registro en la tabla Almacen_inventario
            DetalleEgreso modeloNuevo; //El modelo para poder obtener algunos datos del detalle del egreso
            foreach (int identi in listaIDS) //Iterador para operar sobre cada uno de los registros de detalles de la compra
            {
                //Obteniendo los datos del ID de regitro, cantidad y ID de inventario de una única fila
                //de la tabla ComprasDetalle correspondiente al egreso en curso
                modeloNuevo = detHoja.ConsultaUnicoRegistro(identi);
                CantReciente = (int)modeloNuevo.Cantidad;
                IdInvReciente = modeloNuevo.Id_inventario;
                IdAlReciente = modeloNuevo.Id_almacen;

                //Obteniendo la fila de la tabla Inventario correspondiente al producto encontrado
                //en la fila del detalle del egreso
                Inventario modeloInv = inven.ConsultaDetalle(IdInvReciente);
                CantOriginal = (int)modeloInv.Existencia;

                //Restando las cantidades obtenidas (Por ser egresos, se restan)
                NuevaCantidad = CantOriginal - CantReciente;

                //Pasar nuevo valor de existencias a la fila de la tabla inventario que contiene el ID de inventario obtenido

                var resultado = (from p in db.Inventario
                                 where p.Id == IdInvReciente
                                 select p).SingleOrDefault();
                resultado.Existencia = NuevaCantidad;
                db.SaveChanges();

                //Obteniendo la fila de la tabla AlmacenInventario correspondiente al producto y su almacen encontrados
                //en la fila del detalle del egreso
                Almacen_Inventario modeloAlma = alma.ConsultaEspecifica(IdAlReciente, IdInvReciente);
                CantidadAlmacen = (int)modeloAlma.Existencia;

                //Restando las cantidades para la tabla de Almacen_inventario
                NuevaCantidadAlmacen = CantidadAlmacen - CantReciente;

                //Pasar nuevo valor de existencias a la fila de la tabla Almacen_Inventario

                var resultado2 = (from p in db.Almacen_Inventario
                                  where p.Id_almacen == IdAlReciente && p.Id_inventario == IdInvReciente
                                  select p).SingleOrDefault();
                resultado2.Existencia = NuevaCantidadAlmacen;
                db.SaveChanges();
            }
            */
            //-----------------------------------------------------------------------------------------------

            return RedirectToAction("Index", new { mensaje = "¡Egreso finalizado con éxito!" });
        }

        //Método que abre la vista para únicamente visualizar los detalles de una compra, sin
        //permitir algún tipo de modificación
        //La misma lógica del método RegistrosEgreso pero enviando más datos.
        public ActionResult TodoElEgreso(int id = 0)
        {
            HojaEgreso modelo = hoja.ObtenerDetalles(id);
            int comID = (int)modelo.Id;
            ViewBag.EgresoID = comID;
            double comSuma = (double)modelo.Suma;
            ViewBag.EgresoSuma = comSuma;
            DateTime ComF = (DateTime)modelo.Fecha;
            String FechaNueva = ComF.Date.ToShortDateString();
            ViewBag.CompraFecha = FechaNueva;

            //Colección de datos de HojaEgresoDetalle
            IEnumerable<DetalleEgreso> lista = detHoja.ConsultaDetalles(id);
            ICollection<DetalleEgreso> listadoDetalles = new List<DetalleEgreso>();
            using (var sequenceEnum = lista.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    DetalleEgreso modelo1 = sequenceEnum.Current;
                    Inventario inv = inven.ConsultaDetalle(modelo1.Id_inventario);
                    string nombreInv = inv.Descripcion;
                    Almacenes alm = alll.ConsultaDetalle(modelo1.Id_almacen);
                    string nombreAl = alm.Nombre;
                    DetalleEgreso modeloNuevo = new DetalleEgreso()
                    {
                        Id_detalle = modelo1.Id_detalle,
                        Id_hojaEgreso = modelo1.Id_hojaEgreso,
                        Id_almacen = modelo1.Id_almacen,
                        Almacen = nombreAl,
                        Id_inventario = modelo1.Id_inventario,
                        Inventario2 = nombreInv,
                        Cantidad = modelo1.Cantidad,
                        Precio = modelo1.Precio,
                        PrecioTotal = modelo1.PrecioTotal
                    };
                    listadoDetalles.Add(modeloNuevo);
                }
            }
            return View(listadoDetalles);
        }

        //Método para eliminar el registro de alguno de los detalles de la Compra en curso. 
        public ActionResult Eliminar(int id = 0, int id_doc = 0, double todoElPrecio = 0)
        {
            //Obtener el precio de ese elemento que se desea eliminar, para poderlo restar de la cantidad total
            DetalleEgreso modelo2 = detHoja.ConsultaUnicoRegistro(id);
            int elAlmacen = (int)modelo2.Id_almacen;
            int elInventario = (int)modelo2.Id_inventario;
            int laCantidad = (int)modelo2.Cantidad;
            double miPrecio = (double)modelo2.PrecioTotal;
            double nuevoPrecio = todoElPrecio - miPrecio;

            //Antes de borrar el registro, hay que obtener la cantidad de unidades ingresada y sumarla de vuelta a los
            //Correspondientes registros de las tablas Inventario y Almacen_inventario

            //Primero, sumamos en Inventario
            Inventario modeloInv = inven.ConsultaDetalle(elInventario);
            int CantTablaInv = modeloInv.Existencia;
            int CantNueva = CantTablaInv + laCantidad;

            var resultado = (from p in db.Inventario
                             where p.Id == elInventario
                             select p).SingleOrDefault();
            resultado.Existencia = CantNueva;
            db.SaveChanges();

            //Luego sumamos de vuelta en Almacen_inventario
            Almacen_Inventario modeloAlma = alma.ConsultaEspecifica(elAlmacen, elInventario);
            int CantTablaAlma = (int)modeloAlma.Existencia;
            CantNueva = CantTablaAlma + laCantidad;

            var resultado2 = (from p in db.Almacen_Inventario
                              where p.Id_almacen == elAlmacen && p.Id_inventario == elInventario
                              select p).SingleOrDefault();
            resultado2.Existencia = CantNueva;
            db.SaveChanges();

            //Luego eliminamos el registro ingresado en el Kardex

            Kardex modeloKar = kar.ConsultaEspecifica(id_doc, elInventario, 0, elAlmacen, laCantidad);
            int idKar = modeloKar.Id_kardex;
            int idCompras = 0;
            Kardex modeloKar2 = db.Kardex.Find(idKar, id_doc, elInventario, idCompras, elAlmacen);
            db.Kardex.Remove(modeloKar2);
            db.SaveChanges();

            //Borrando el registro de la base por completo
            DetalleEgreso modelo = db.DetalleEgreso.Find(id, elInventario, elAlmacen);
            db.DetalleEgreso.Remove(modelo);
            db.SaveChanges();

            return RedirectToAction("RegistrosEgreso", new { id = id_doc, total = nuevoPrecio });
        }
    }
}