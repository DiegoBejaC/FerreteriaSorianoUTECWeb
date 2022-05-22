using ProyectoEstandares3.Datos;
using ProyectoEstandares3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoEstandares3.Controllers
{
    public class ComprasController : Controller
    {
        //Un objeto del tipo de entidad, no lo he borrado por si acaso.
        FerreSoriano17Entities db = new FerreSoriano17Entities();
        
        //Objetos de las clases de la capa de datos
        ComprasAdmin comp = new ComprasAdmin();
        ComprasDetalleAdmin detComp = new ComprasDetalleAdmin();
        InventarioAdmin inven = new InventarioAdmin();
        AlmacenesAdmin alll = new AlmacenesAdmin();
        AlmacenInventarioAdmin alma = new AlmacenInventarioAdmin();
        KardexAdmin kar = new KardexAdmin();
        ProveedoresAdmin prov = new ProveedoresAdmin();

        // GET: Compras
        
        //Index para mostrar la tabla de datos "Compras"
        public ActionResult Index(String mensaje = "")
        {
            IEnumerable<int> listaComp = comp.ConsultarIDS();
            ICollection<Compras> listadoCompras = new List<Compras>();
            string Provee = "";
            foreach (int dato in listaComp)
            {
                Compras mComp = comp.ObtenerDetalles(dato);
                Proveedores prv = prov.ConsultaDetalle((int)mComp.Id_proveedor);
                Provee = prv.Nombre;
                Compras modeloComp = new Compras()
                {
                    Id = mComp.Id,
                    Documento = mComp.Documento,
                    Proveedor = Provee,
                    Fecha = mComp.Fecha,
                    Sumas = mComp.Sumas,
                    Iva = mComp.Iva
                };
                listadoCompras.Add(modeloComp);
            }
                ViewBag.confirmacion = mensaje;
            return View(listadoCompras);
        }

        //public ActionResult ObtenerUltimoID(Compras modelo) {
        //    return RedirectToAction("Guardar", new { id = modelo.Id });
        //}


        //Método que abre la vista "Guardar", con un formulario para guardar los
        //de la compra.
        public ActionResult Guardar()
        {
            Compras modelo = comp.ObtenerUltimo();
            int ultimoID = 0;
            if (modelo != null) {
                ultimoID += modelo.Id;
            }
            ultimoID++;
            Compras modelo2 = new Compras()
            {
                Documento = "DOC-" + ultimoID
            };
            return View(modelo2);
        }

        //Al enviar los datos de la compra nueva, se guardan en la tabla de la BD
        //Y se llama a la vista "DetalleCompra", la cual muestra los datos de 
        //ComprasDetalle ligados a la compra recíen ingresada por el usuario
        //Por ello se envia un argumento del ID de la compra ingresada.
        public ActionResult NuevaCompra(Compras modelo) {
            comp.Guardar(modelo);
            return RedirectToAction("DetalleCompra", new { id = modelo.Id, total = 0 });
        }

        //Método para guardar los registros de los detalles de la compra recién ingresada
        //Esta vista muestra los datos de la compra a realizar. Y abajo muestra una tabla
        //Que permitirá al usuario ver los registros de aquellos productos que formarán
        //Parte de la compra, con sus cantidades y precio unitario. Además del almacén en donde
        //Estos productos serán guardados.

        //Aquí se usa ViewBag para poder enviar como propiedades los detalles de COMPRAS
        //Y se envía un modelo de una colección de datos para llenar la tabla para COMPRASDETALLE
        public ActionResult DetalleCompra(int id = 0, double total = 0)
        {
            //ESTAS SÓLO FUERON PRUEBAS, ESTAS LÍNEAS NO FUNCIONAN
            //ViewBag.CompraDocumento = "DOC00001";
            //ViewBag.CompraProveedor = "Empresaxyz";
            //-------------------
            //ViewBag.CompraDocumento = comp.ConsultaDocumento(id);
            //ViewBag.CompraProveedor = comp.ConsultaProveedor(id).ToString();

            //ESTAS LÍNEAS SI FUNCIONAN PERFECTAMENTE
                
            //Se crea un modelo de la clase Compras que almacena los datos obtenidos por el método "ObtenerDetalles"
            Compras modelo = comp.ObtenerDetalles(id);
            //Se almacenan algunas propiedades del modelo en variables del mismo tipo de dato
            //para luego ser almacenados en variables ViewBag
            int comID = (int)modelo.Id;
            ViewBag.CompraID = comID;
            String comD = modelo.Documento;
            ViewBag.CompraDocumento = comD;
            int comP = (int)modelo.Id_proveedor;
            Proveedores provvv = prov.ConsultaDetalle(comP);
            string nombreProv = provvv.Nombre;
            ViewBag.CompraProveedor = comP;
            ViewBag.CompraProveedorNombre = nombreProv;
            DateTime ComF = modelo.Fecha;
            String FechaNueva = ComF.Date.ToShortDateString();
            ViewBag.CompraFecha = FechaNueva;
            ViewBag.FechaNoVisible = ComF;

            ViewBag.CompraPrecioTotal = total;

            //Colección de datos de ComprasDetalle
            IEnumerable<ComprasDetalle> lista = detComp.ConsultaDetalles(id);
            ICollection<ComprasDetalle> listadoDetalles = new List<ComprasDetalle>();
            using (var sequenceEnum = lista.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    ComprasDetalle modelo1 = sequenceEnum.Current;
                    Inventario inv = inven.ConsultaDetalle(modelo1.Id_inventario);
                    string nombreInv = inv.Descripcion;
                    Almacenes alm = alll.ConsultaDetalle(modelo1.Id_almacen);
                    string nombreAl = alm.Nombre;
                    ComprasDetalle modeloNuevo = new ComprasDetalle()
                    {
                        Id = modelo1.Id,
                        Id_documento = modelo1.Id_documento,
                        Id_almacen = modelo1.Id_almacen,
                        Almacen = nombreAl,
                        Id_inventario = modelo1.Id_inventario,
                        Inventario = nombreInv,
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
        public ActionResult AgregarDetallesCompra(int id = 0, int idinventario = 0, int idalmacen = 0, int? cantid = 0, int? prec = 0, double total = 0, string mensaje = "") {
            ComprasDetalle modelo = new ComprasDetalle()
            {
                Id_documento = id,
                Id_inventario = idinventario,
                Id_almacen = idalmacen,
                Cantidad = cantid,
                Precio = prec
            };
            ViewBag.CompraPrecioTotal = total;
            ViewBag.alerta = mensaje;
            //Compras modelo = comp.ObtenerDetalles(id);
            //int comID = (int)modelo.Id;
            //ViewBag.CompraID = comID;
            return View(modelo);
        }

        //Este método se ejecuta cuando se manda el formulario de la vista "AgregarDetallesCompra", se guarda
        //el nuevo registro en la tabla "ComprasDetalle" y se redirecciona a la vista "DetalleCompra"
        //porque en ese instante todavía se sigue rellenando los detalles de la compra.

        //En el método se modifican también los registros de existencias de las tablas "Inventario" y "Almacen_inventario"

        //Y también se crea el método para mandar el registro del detalle de la compra a la tabla "Kardex"
        public ActionResult NuevoDetalleCompra(ComprasDetalle modelo, double total = 0)
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
                    int stockMaxi = modeloInv.StockMaximo;
                    int Existencias = modeloInv.Existencia;
                    int nuevaExis = (int)modelo.Cantidad;
                    int posibleExis = nuevaExis + Existencias;

                    Almacen_Inventario modeloAlma = alma.ConsultaEspecifica(IdAl, IdInv);
                    int? cantAlmacen = modeloAlma.Existencia;

                    int? nuevaCantAlmacen = cantAlmacen + nuevaExis;

                    int Disponible = stockMaxi - Existencias;

                    if (posibleExis <= stockMaxi)
                    {
                        //Aqui mandaremos el dato del detalle de la compra hacia la tabla correspondiente.
                        double Cant = (double)modelo.Cantidad;
                        double Prec = (double)modelo.Precio;
                        double PrecioT = Cant * Prec;
                        ComprasDetalle modelo2 = new ComprasDetalle()
                        {
                            Id_documento = modelo.Id_documento,
                            Id_inventario = modelo.Id_inventario,
                            Id_almacen = modelo.Id_almacen,
                            Cantidad = modelo.Cantidad,
                            Precio = modelo.Precio,
                            PrecioTotal = (decimal?)PrecioT
                        };

                        detComp.Guardar(modelo2);

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
                        Compras modelolast = comp.ObtenerDetalles(modelo.Id_documento);
                        DateTime ComF = modelolast.Fecha;

                        Kardex modeloKar = new Kardex() {
                            Id_hojaEgreso = 0,
                            Id_inventario = modelo.Id_inventario,
                            Id_compra = modelo.Id_documento,
                            Id_almacen = modelo.Id_almacen,
                            Cantidad = modelo.Cantidad,
                            Precio = modelo.Precio,
                            PrecioTotal = (decimal?)PrecioT,
                            Fecha = ComF,
                            CantidadNueva = posibleExis
                        };
                        kar.Guardar(modeloKar);

                        // Aquí mandamos todo el dato a la vista "DetalleCompra" para continuar la transacción
                        todoElPrecio = total + PrecioT;
                        ViewBag.alerta = "";
                        return RedirectToAction("DetalleCompra", new { id = modelo.Id_documento, total = todoElPrecio });
                    }
                    else
                    {
                        ViewBag.alerta = "No puede exceder el stock máximo para este producto. Sólo puede comprar " + Disponible + " unidades.";
                        return RedirectToAction("AgregarDetallesCompra", new { id = modelo.Id_documento, idinventario = modelo.Id_inventario, idalmacen = modelo.Id_almacen, cantid = modelo.Cantidad, prec = modelo.Precio, total = total, mensaje = ViewBag.alerta });
                    }
                }
                else {
                    ViewBag.alerta = "La cantidad y el precio no pueden ser cero";
                    return RedirectToAction("AgregarDetallesCompra", new { id = modelo.Id_documento, idinventario = modelo.Id_inventario, idalmacen = modelo.Id_almacen, cantid = modelo.Cantidad, prec = modelo.Precio, total = total, mensaje = ViewBag.alerta });
                }
            }
            else {
                ViewBag.alerta = "Ingrese por favor la cantidad y precio unitario del producto";
                return RedirectToAction("AgregarDetallesCompra", new { id = modelo.Id_documento, idinventario = modelo.Id_inventario, idalmacen = modelo.Id_almacen, cantid = modelo.Cantidad, prec = modelo.Precio, total = total, mensaje = ViewBag.alerta });
            }
        }

        //Metodo que se ejecuta cuando se le da fin al registro total de la compra con todos sus detalles.
        //Se redirecciona a la vista Index con un mensaje de confirmación.
        public ActionResult CompraFinalizada(DateTime fecha, int id = 0, String idDoc = "", int idProv = 0, double total = 0)
        {
            //Se manda el dato de la suma total y el IVA a la compra realizada
            double CompraIva = total * 0.21;
            Compras modelo = new Compras()
            {
                Id = id,
                Documento = idDoc,
                Id_proveedor = idProv,
                Fecha = fecha,
                Sumas = (decimal?)total,
                Iva = (decimal?)CompraIva
            };
            comp.Modificar(modelo);

            //ESTAS LÍNEAS ESTÁN OBSOLETAS -------------------------------------------
            /*

            //Aquí se modificarán las existencias en las tablas Inventario y Almacen_inventario para el producto correspondiente
            IEnumerable<int> listaIDS = detComp.ConsultaIDS(id); //Para obtener los IDs de los registros de ComprasDetalle
                                                                 //adjuntos a la compra en curso
            int CantReciente = 0; //La cantidad ingresada en el registro del detalle de la compra
            int IdInvReciente = 0; //El ID de inventario ingresado en el registro del detalle de la compra
            int CantOriginal = 0; //La cantidad original de existencias del producto en la tabla Inventario
            int IdAlReciente = 0; //El ID del almacen ingresado en el registro del detalle de la compra
            int NuevaCantidad = 0; //La nueva cantidad a actualizarse en la existencia del registro en la tabla Inventario
            int CantidadAlmacen = 0; //La cantidad original de existencias del producto en la tabla Almacen_inventario
            int NuevaCantidadAlmacen = 0; //La nueva cantidad a actualizarse en la existencia del registro en la tabla Almacen_inventario
            ComprasDetalle modeloNuevo; //El modelo para poder obtener algunos datos del detalle de la compra
            foreach (int identi in listaIDS) //Iterador para operar sobre cada uno de los registros de detalles de la compra
            {
                //Obteniendo los datos del ID de regitro, cantidad y ID de inventario de una única fila
                //de la tabla ComprasDetalle correspondiente a la compra en curso
                modeloNuevo = detComp.ConsultaUnicoRegistro(identi);
                CantReciente = (int)modeloNuevo.Cantidad;
                IdInvReciente = modeloNuevo.Id_inventario;
                IdAlReciente = modeloNuevo.Id_almacen;

                //Obteniendo la fila de la tabla Inventario correspondiente al producto encontrado
                //en la fila del detalle de la compra
                Inventario modeloInv = inven.ConsultaDetalle(IdInvReciente);
                CantOriginal = (int)modeloInv.Existencia;

                //Sumando las cantidades obtenidas (Por ser una compra, se sumarán)
                NuevaCantidad = CantReciente + CantOriginal;

                //Pasar nuevo valor de existencias a la fila de la tabla inventario que contiene el ID de inventario obtenido

                var resultado = (from p in db.Inventario
                                 where p.Id == IdInvReciente
                                 select p).SingleOrDefault();
                resultado.Existencia = NuevaCantidad;
                db.SaveChanges();

                //Obteniendo la fila de la tabla AlmacenInventario correspondiente al producto y su almacen encontrados
                //en la fila del detalle de la compra
                Almacen_Inventario modeloAlma = alma.ConsultaEspecifica(IdAlReciente, IdInvReciente);
                CantidadAlmacen = (int)modeloAlma.Existencia;

                //Sumando las cantidades para la tabla de Almacen_inventario
                NuevaCantidadAlmacen = CantReciente + CantidadAlmacen;

                //Pasar nuevo valor de existencias a la fila de la tabla Almacen_Inventario

                var resultado2 = (from p in db.Almacen_Inventario
                                 where p.Id_almacen == IdAlReciente && p.Id_inventario == IdInvReciente
                                 select p).SingleOrDefault();
                resultado2.Existencia = NuevaCantidadAlmacen;
                db.SaveChanges();
            }

            */
            //-----------------------------------------------------------------------------------------------

            return RedirectToAction("Index", new { mensaje = "¡Compra finalizada con éxito!" });
        }

        //public void ActualizarExistenciasInv(int IdReg = 0, int cant = 0) {
        //    ComprasDetalle modelo = detComp.ConsultaUnicoRegistro(IdReg);
        //    int Cantidad = (int)modelo.Cantidad;
        //    ViewBag.Cantidad = Cantidad;
        //}

        //Método que abre la vista para únicamente visualizar los detalles de una compra, sin
        //permitir algún tipo de modificación
        //La misma lógica del método DetalleCompra pero enviando más datos.
        public ActionResult TodaLaCompra(int id = 0) {
            Compras modelo = comp.ObtenerDetalles(id);
            int comID = (int)modelo.Id;
            ViewBag.CompraID = comID;
            String comD = modelo.Documento;
            ViewBag.CompraDocumento = comD;
            int comP = (int)modelo.Id_proveedor;
            Proveedores provvv = prov.ConsultaDetalle(comP);
            string nombreProv = provvv.Nombre;
            ViewBag.CompraProveedor = nombreProv;
            double Suma = (double)modelo.Sumas;
            ViewBag.CompraSumas = Suma;
            double IVA = (double)modelo.Iva;
            ViewBag.CompraIva = IVA;
            DateTime ComF = modelo.Fecha;
            String FechaNueva = ComF.Date.ToShortDateString();
            ViewBag.CompraFecha = FechaNueva;



            IEnumerable<ComprasDetalle> lista = detComp.ConsultaDetalles(id);
            ICollection<ComprasDetalle> listadoDetalles = new List<ComprasDetalle>();
            using (var sequenceEnum = lista.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    ComprasDetalle modelo1 = sequenceEnum.Current;
                    Inventario inv = inven.ConsultaDetalle(modelo1.Id_inventario);
                    string nombreInv = inv.Descripcion;
                    Almacenes alm = alll.ConsultaDetalle(modelo1.Id_almacen);
                    string nombreAl = alm.Nombre;
                    ComprasDetalle modeloNuevo = new ComprasDetalle()
                    {
                        Id = modelo1.Id,
                        Id_documento = modelo1.Id_documento,
                        Id_almacen = modelo1.Id_almacen,
                        Almacen = nombreAl,
                        Id_inventario = modelo1.Id_inventario,
                        Inventario = nombreInv,
                        Cantidad = modelo1.Cantidad,
                        Precio = modelo1.Precio,
                        PrecioTotal = modelo1.PrecioTotal
                    };
                    listadoDetalles.Add(modeloNuevo);
                }
            }
            return View(listadoDetalles);
        }
        //Este método NO BORRA la COMPRA, SOLAMENTE borra el DETALLE de la compra que el usuario haya seleccionado
        public ActionResult Eliminar(int id = 0, int id_doc = 0, double todoElPrecio = 0)
        {
            //Obtener el precio de ese elemento que se desea eliminar, para poderlo restar de la cantidad total
            ComprasDetalle modelo2 = detComp.ConsultaUnicoRegistro(id);
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
            int CantNueva = CantTablaInv - laCantidad;

            var resultado = (from p in db.Inventario
                             where p.Id == elInventario
                             select p).SingleOrDefault();
            resultado.Existencia = CantNueva;
            db.SaveChanges();

            //Luego sumamos de vuelta en Almacen_inventario
            Almacen_Inventario modeloAlma = alma.ConsultaEspecifica(elAlmacen, elInventario);
            int CantTablaAlma = (int)modeloAlma.Existencia;
            CantNueva = CantTablaAlma - laCantidad;

            var resultado2 = (from p in db.Almacen_Inventario
                              where p.Id_almacen == elAlmacen && p.Id_inventario == elInventario
                              select p).SingleOrDefault();
            resultado2.Existencia = CantNueva;
            db.SaveChanges();

            //Luego eliminamos el registro ingresado en el Kardex

            Kardex modeloKar = kar.ConsultaEspecifica(0, elInventario, id_doc, elAlmacen, laCantidad);
            int idKar = modeloKar.Id_kardex;
            int idHojas = 0;
            Kardex modeloKar2 = db.Kardex.Find(idKar, idHojas, elInventario, id_doc, elAlmacen);
            db.Kardex.Remove(modeloKar2);
            db.SaveChanges();

            //Borrando el registro de la base por completo
            ComprasDetalle modelo = db.ComprasDetalle.Find(id, elAlmacen, elInventario);
            db.ComprasDetalle.Remove(modelo);
            db.SaveChanges();

            return RedirectToAction("DetalleCompra", new { id = id_doc, total = nuevoPrecio });
        }

    }
}