SET IDENTITY_INSERT dbo.Fabricante ON
INSERT INTO dbo.Fabricante (Id, nombre) VALUES (1, 'Jaime');
INSERT INTO dbo.Fabricante (Id, nombre) VALUES (2, 'Daniel Balan');
INSERT INTO dbo.Fabricante (Id, nombre) VALUES (3, 'Daniel García');
INSERT INTO dbo.Fabricante (Id, nombre) VALUES (4, 'Jesús');
SET IDENTITY_INSERT dbo.Fabricante OFF

SET IDENTITY_INSERT dbo.Herramienta ON
INSERT INTO dbo.Herramienta (Id, material, nombre, precio, tiempoReparacion, FabricanteId) VALUES (1, 'Hierro', 'Martillo', 30, 22.5, 1);
INSERT INTO dbo.Herramienta (Id, material, nombre, precio, tiempoReparacion, FabricanteId) VALUES (2, 'Hierro', 'Destornillador', 25, 29, 2);
INSERT INTO dbo.Herramienta (Id, material, nombre, precio, tiempoReparacion, FabricanteId) VALUES (3, 'Madera', 'Metro', 10, 3, 3);
INSERT INTO dbo.Herramienta (Id, material, nombre, precio, tiempoReparacion, FabricanteId) VALUES (4, 'Plástico', 'Clavo', 3, 0, 4);
INSERT INTO dbo.Herramienta (Id, material, nombre, precio, tiempoReparacion, FabricanteId) VALUES (5, 'Madera', 'Sierra', 45, 30.5, 1);
INSERT INTO dbo.Herramienta (Id, material, nombre, precio, tiempoReparacion, FabricanteId) VALUES (6, 'Plástico', 'Alicates', 33, 16, 2);
SET IDENTITY_INSERT dbo.Herramienta OFF

DECLARE @UserId1 NVARCHAR(450);
DECLARE @UserId2 NVARCHAR(450);
DECLARE @UserId3 NVARCHAR(450);

SET IDENTITY_INSERT dbo.Alquiler ON
INSERT INTO dbo.Alquiler (id, direccionEnvio, fechaAlquiler, fechaFin, fechaInicio, precioTotal, métodoDePago, applicationUserId) 
	VALUES (1, 'Tobarra', '2025-10-18','2025-10-18','2025-10-26', 620,1,1);
SET IDENTITY_INSERT dbo.Alquiler OFF


INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (1,1,200,30);

