Create database FerreSoriano17
go
use FerreSoriano17
go
create table Almacenes(
	Id int not null identity (1,1) primary key,
	Nombre varchar(50)
)
go
create table Proveedores(
	Id int not null identity (1,1) primary key,
	Nombre varchar(150) not null,
	Telefono varchar(12) null,
	Direccion varchar(250) null
)

go
create table Categorias(
	Id int not null identity (1,1) primary key,
	Descripcion varchar(50)
)
go
create table Inventario(
	Id int not null identity (1,1) primary key,
	Descripcion varchar(150) not null,
	StockMinimo int not null default(0),
	StockMaximo int not null default(0),
	Existencia int not null default(0)
)
go
create table Categorias_Inventario(
	Id_categoria int,
	Id_inventario int
	primary key(Id_categoria,Id_inventario)
	foreign key(Id_inventario) references Inventario(Id),
	foreign key(Id_categoria) references Categorias(Id)
)
go
create table Almacen_Inventario(
	Id_almacen int not null,
	Id_inventario int not null foreign key references Almacenes(Id),
	Existencia int
	primary key(
		Id_almacen,Id_inventario
	)
)
go
create table Compras(
	Id int not null identity (1,1) primary key,
	Documento varchar(20),
	Id_proveedor int foreign key references Proveedores(Id),
	Fecha date not null default getdate(),
	Sumas decimal(18,2),
	Iva decimal(18,2),
)
go

create table ComprasDetalle(
	Id int not null identity (1,1),
	Id_documento int not null,
	Id_almacen int,
	Id_inventario int,
	Cantidad int,
	Precio decimal(18,2),
	PrecioTotal decimal(18,2)
	primary key(Id,Id_almacen,Id_inventario),
	foreign key(Id_documento) references Compras(Id),
	FOREIGN KEY([Id_almacen], [Id_inventario])
	REFERENCES [dbo].[Almacen_Inventario] ([Id_almacen], [Id_inventario])
)
go
create table HojaEgreso(
	Id int not null identity (1,1) primary key,
	Fecha date,
	Suma decimal(18,2)
)
go

create table DetalleEgreso(
	Id_detalle int not null identity (1,1),
	Id_hojaEgreso int not null,
	Id_inventario int not null,
	Id_almacen int,
	Cantidad int not null default(0),
	Precio decimal(18,2),
	PrecioTotal decimal(18,2)
	primary key(Id_detalle,Id_inventario,Id_almacen),
	foreign key(Id_hojaEgreso) references HojaEgreso(Id),
	foreign key(Id_inventario) references Inventario(Id),
	FOREIGN KEY([Id_almacen], [Id_inventario])
	REFERENCES [dbo].[Almacen_Inventario] ([Id_almacen], [Id_inventario])
)
go
create table Kardex(
	Id_kardex int not null identity (1,1),
	Id_hojaEgreso int not null,
	Id_inventario int not null,
	Id_compra int not null,
	Id_almacen int not null,
	Cantidad int,
	Precio decimal(18,2),
	PrecioTotal decimal(18,2),
	Fecha date,
	CantidadNueva int
	primary key(Id_kardex,Id_hojaEgreso,Id_inventario,Id_compra,Id_almacen)
)
go

create table Usuarios(
	Id int not null identity (1,1),
	Usuario varchar(10),
	Clave varchar(15),
	Nombre varchar(80),
	Nivel int,
	primary key(Id)
)
use FerreSoriano17

select * from DetalleEgreso