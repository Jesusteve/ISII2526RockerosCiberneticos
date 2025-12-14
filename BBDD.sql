SET IDENTITY_INSERT [dbo].[Fabricante] ON
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

INSERT INTO [dbo].[AspNetUsers] ([Id],[nombreCliente],[apellidoCliente],[direccionEnvío],[correoElectonico],[teléfono],[UserName],[NormalizedUserName],[Email],
[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],
[LockoutEnd],[LockoutEnabled],[AccessFailedCount]) 
VALUES 
	('1','Homer','Simpson','Avenida Siempreviva 742','homer@springfield.com',555123456,'homer.simpson','HOMER.SIMPSON','homer@springfield.com',
	'HOMER@SPRINGFIELD.COM',0,'donas123','GUID1','GUID1','555123456',0,0,NULL,0,0),

	('2','Sheldon','Cooper','Calle Principal 2311','sheldon@caltech.edu',555987654,'sheldon.cooper','SHELDON.COOPER','sheldon@caltech.edu',
	'SHELDON@CALTECH.EDU',0,'bazinga123','GUID2','GUID2','555987654',0,0,NULL,0,0),

	('3','David','Broncano','Gran Vía 28','david@latemotiv.com',555456789,'david.broncano','DAVID.BRONCANO','david@latemotiv.com','
	DAVID@LATEMOTIV.COM',0,'late123','GUID3','GUID3','555456789',0,0,NULL,0,0);

SET IDENTITY_INSERT dbo.Alquiler ON
INSERT INTO dbo.Alquiler (id, direccionEnvio, fechaAlquiler, fechaFin, fechaInicio, precioTotal, metodoDePago, applicationUserId) 
	VALUES (1, 'Tobarra', '2025-10-18','2025-10-18','2025-10-26', 620,1,1);
INSERT INTO dbo.Alquiler (id, direccionEnvio, fechaAlquiler, fechaFin, fechaInicio, precioTotal, metodoDePago, applicationUserId) 
VALUES (2, 'Madrid', '2025-09-15', '2025-09-15', '2025-09-20', 450, 2, 2);

INSERT INTO dbo.Alquiler (id, direccionEnvio, fechaAlquiler, fechaFin, fechaInicio, precioTotal, metodoDePago, applicationUserId) 
VALUES (3, 'Barcelona', '2025-08-10', '2025-08-10', '2025-08-17', 320, 1, 3);

INSERT INTO dbo.Alquiler (id, direccionEnvio, fechaAlquiler, fechaFin, fechaInicio, precioTotal, metodoDePago, applicationUserId) 
VALUES (4, 'Valencia', '2025-11-05', '2025-11-05', '2025-11-12', 780, 3, 4);

INSERT INTO dbo.Alquiler (id, direccionEnvio, fechaAlquiler, fechaFin, fechaInicio, precioTotal, metodoDePago, applicationUserId) 
VALUES (5, 'Sevilla', '2025-07-22', '2025-07-22', '2025-07-29', 210, 2, 1);

INSERT INTO dbo.Alquiler (id, direccionEnvio, fechaAlquiler, fechaFin, fechaInicio, precioTotal, metodoDePago, applicationUserId) 
VALUES (6, 'Bilbao', '2025-12-01', '2025-12-01', '2025-12-10', 920, 1, 2);

SET IDENTITY_INSERT dbo.Alquiler OFF


INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (1,1,200,30);

INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (2, 2, 180, 2);
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (3, 2, 270, 3);

INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (1, 3, 120, 1);
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (4, 3, 200, 4);

INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (5, 4, 350, 2);
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (6, 4, 280, 3);
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (2, 4, 150, 1);

INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (3, 5, 90, 1);
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (4, 5, 120, 2);

INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (1, 6, 300, 2);
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (5, 6, 450, 3);
INSERT INTO dbo.AlquilarItem (herramientaId, alquilerId, precio, cantidad) VALUES (6, 6, 170, 1);

SET IDENTITY_INSERT dbo.Oferta ON;

-- Oferta 1: Socios (dirigidaA = 0)
INSERT INTO dbo.Oferta (Id, fechaInicio, fechaFinal, fechaOferta, dirigidaA, metodopago, usuarioId) 
VALUES (1, '2023-11-01', '2023-11-30', '2023-10-25', 0, 0, 1);

-- Oferta 2: Clientes (dirigidaA = 1)
INSERT INTO dbo.Oferta (Id, fechaInicio, fechaFinal, fechaOferta, dirigidaA, metodopago, usuarioId) 
VALUES (2, '2023-12-01', '2023-12-15', '2023-11-28', 1, 1, 2);

-- Oferta 3: Clientes (dirigidaA = 1)
INSERT INTO dbo.Oferta (Id, fechaInicio, fechaFinal, fechaOferta, dirigidaA, metodopago, usuarioId) 
VALUES (3, '2024-01-01', '2024-01-31', '2023-12-20', 1, 2, 1);

SET IDENTITY_INSERT dbo.Oferta OFF;

-- ITEMS DE LA OFERTA 1 (Descuentos moderados en herramientas caras)
-- Martillo (Id 1): Precio original 30. Descuento 10%. Final = 27
INSERT INTO dbo.OfertaItem (HerramientaId, OfertaId, porcentaje, precioFinal) 
VALUES (1, 1, 10, 27);

-- Sierra (Id 5): Precio original 45. Descuento 20%. Final = 36
INSERT INTO dbo.OfertaItem (HerramientaId, OfertaId, porcentaje, precioFinal) 
VALUES (5, 1, 20, 36);


-- ITEMS DE LA OFERTA 2 (Descuentos agresivos en herramientas medianas)
-- Destornillador (Id 2): Precio original 25. Descuento 50%. Final = 12.5
INSERT INTO dbo.OfertaItem (HerramientaId, OfertaId, porcentaje, precioFinal) 
VALUES (2, 2, 50, 12.5);

-- Alicates (Id 6): Precio original 33. Descuento 15%. Final = 28.05
INSERT INTO dbo.OfertaItem (HerramientaId, OfertaId, porcentaje, precioFinal) 
VALUES (6, 2, 15, 28.05);


-- ITEMS DE LA OFERTA 3 (Liquidación de herramientas baratas)
-- Metro (Id 3): Precio original 10. Descuento 30%. Final = 7
INSERT INTO dbo.OfertaItem (HerramientaId, OfertaId, porcentaje, precioFinal) 
VALUES (3, 3, 30, 7);

-- Clavo (Id 4): Precio original 3. Descuento 5%. Final = 2.85
INSERT INTO dbo.OfertaItem (HerramientaId, OfertaId, porcentaje, precioFinal) 
VALUES (4, 3, 5, 2.85);