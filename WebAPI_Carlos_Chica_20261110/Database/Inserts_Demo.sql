USE G_CuentasXCobrar;
GO

-- 1. Clientes
INSERT INTO dbo.Cliente (RazonSocial, Ruc, Correo, Telefono) VALUES
('Constructora Andina S.A.', '1792345678001', 'cobros@andina.com', '022345678'),
('Distribuidora Pacifico Cia Ltda.', '1798765432001', 'pagos@pacifico.com', '042345678'),
('Servicios Industriales Quito S.A.', '1791234567001', 'cartera@siq.com', '022987654');
GO

-- 2. Facturas (IdCliente = 1, 2, 3)
INSERT INTO dbo.Factura (IdCliente, NumeroFactura, FechaEmision, FechaVencimiento, Monto, SaldoPendiente, EstadoPago) VALUES
(1, 'FAC-001-001', DATEADD(DAY, -40, CAST(GETDATE() AS DATE)), DATEADD(DAY, -10, CAST(GETDATE() AS DATE)), 1500.00, 1500.00, 'PENDIENTE'),
(1, 'FAC-001-002', DATEADD(DAY, -20, CAST(GETDATE() AS DATE)), DATEADD(DAY,   3, CAST(GETDATE() AS DATE)),  800.00,  800.00, 'PENDIENTE'),
(2, 'FAC-002-001', DATEADD(DAY, -35, CAST(GETDATE() AS DATE)), DATEADD(DAY,  -5, CAST(GETDATE() AS DATE)), 2400.00, 2400.00, 'PENDIENTE'),
(2, 'FAC-002-002', DATEADD(DAY, -50, CAST(GETDATE() AS DATE)), DATEADD(DAY, -15, CAST(GETDATE() AS DATE)),  900.00,  900.00, 'PENDIENTE'),
(3, 'FAC-003-001', DATEADD(DAY, -15, CAST(GETDATE() AS DATE)), DATEADD(DAY,   6, CAST(GETDATE() AS DATE)), 1800.00, 1800.00, 'PENDIENTE'),
(3, 'FAC-003-002', DATEADD(DAY,  -5, CAST(GETDATE() AS DATE)), DATEADD(DAY,  45, CAST(GETDATE() AS DATE)), 3200.00, 3200.00, 'PENDIENTE');
GO

-- 3. Pagos
INSERT INTO dbo.Pago (IdFactura, FechaPago, MontoAbonado, MetodoPago, Referencia) VALUES
(1, DATEADD(DAY, -5, CAST(GETDATE() AS DATE)), 500.00, 'Transferencia', 'TRF-001'),
(3, DATEADD(DAY, -3, CAST(GETDATE() AS DATE)), 800.00, 'Cheque',        'CHQ-002');
GO

-- 4. Actualizar saldos y estados
UPDATE f
SET f.SaldoPendiente = f.Monto - ISNULL(p.TotalPagado, 0),
    f.EstadoPago = CASE
        WHEN f.Monto - ISNULL(p.TotalPagado, 0) <= 0 THEN 'PAGADA'
        WHEN ISNULL(p.TotalPagado, 0) > 0 THEN 'PARCIAL'
        ELSE 'PENDIENTE'
    END
FROM dbo.Factura f
LEFT JOIN (
    SELECT IdFactura, SUM(MontoAbonado) AS TotalPagado
    FROM dbo.Pago
    GROUP BY IdFactura
) p ON p.IdFactura = f.Id;
GO

SELECT 'Listo: ' + CAST(COUNT(*) AS VARCHAR) + ' clientes' FROM dbo.Cliente;
SELECT 'Listo: ' + CAST(COUNT(*) AS VARCHAR) + ' facturas' FROM dbo.Factura;
SELECT 'Listo: ' + CAST(COUNT(*) AS VARCHAR) + ' pagos' FROM dbo.Pago;
