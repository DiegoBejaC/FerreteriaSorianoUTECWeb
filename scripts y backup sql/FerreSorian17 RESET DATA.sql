use FerreSoriano17

delete from Almacen_Inventario
delete from Almacenes
delete from Categorias
delete from Categorias_Inventario
delete from Compras
delete from ComprasDetalle
delete from DetalleEgreso
delete from HojaEgreso
delete from Inventario
delete from Kardex
delete from Proveedores
delete from Usuarios

select * from Almacen_Inventario
select * from Almacenes
select * from Categorias
select * from Categorias_Inventario
select * from Compras
select * from ComprasDetalle
select * from DetalleEgreso
select * from HojaEgreso
select * from Inventario
select * from Kardex
select * from Proveedores
select * from Usuarios


--Consultas para reestablecer el conteo de los IDs autoincrementables de las tablas

DBCC CHECKIDENT('dbo.Almacen_Inventario', RESEED, 0)
DBCC CHECKIDENT('dbo.Almacenes', RESEED, 0)
DBCC CHECKIDENT('dbo.Categorias', RESEED, 0)
DBCC CHECKIDENT('dbo.Categorias_Inventario', RESEED, 0)
DBCC CHECKIDENT('dbo.Compras', RESEED, 0)
DBCC CHECKIDENT('dbo.ComprasDetalle', RESEED, 0)
DBCC CHECKIDENT('dbo.DetalleEgreso', RESEED, 0)
DBCC CHECKIDENT('dbo.HojaEgreso', RESEED, 0)
DBCC CHECKIDENT('dbo.Inventario', RESEED, 0)
DBCC CHECKIDENT('dbo.Kardex', RESEED, 0)
DBCC CHECKIDENT('dbo.Proveedores', RESEED, 0)
DBCC CHECKIDENT('dbo.Usuarios', RESEED, 0)


