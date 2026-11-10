USE [G_CuentasXCobrar]
GO

-- 1. Insertar Clientes (Roles requeridos por el enunciado: Administrador o Ejecutivo)
INSERT INTO [dbo].[Cliente] ([Id], [nombre], [apellido], [Cedula], [Rol], [correo]) 
VALUES 
(1, 'Carlos', 'Perez', '0912345678', 'Administrador', 'cperez@banco.com'),
(2, 'Maria', 'Gomez', '0987654321', 'Ejecutivo', 'mgomez@banco.com'),
(3, 'Juan', 'Zambrano', '0911223344', 'Ejecutivo', 'jzambrano@banco.com');
GO

-- 2. Insertar Facturas
INSERT INTO [dbo].[Factura] ([Id], [fechaEmision], [monto], [estadopago]) 
VALUES 
(1, '2026-10-01 10:30:00', 1500.50, 'PENDIENTE'),
(2, '2026-10-05 14:15:00', 3200.00, 'PENDIENTE'),
(3, '2026-10-08 09:00:00', 850.00, 'PAGADO');
GO

-- 3. Insertar Pagos
INSERT INTO [dbo].[Pago] ([id_factura], [fecha_pago], [monto_abonado], [metodo_pago], [referencia]) 
VALUES 
(1, '2026-10-10', 500.00, 'Transferencia', 'TRF-998877'),
(3, '2026-10-09', 850.00, 'Cheque', 'CHQ-112233');
GO