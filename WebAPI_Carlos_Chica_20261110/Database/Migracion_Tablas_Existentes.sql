USE G_CuentasXCobrar;
GO

IF COL_LENGTH('dbo.Factura', 'IdCliente') IS NULL
    ALTER TABLE dbo.Factura ADD IdCliente int NULL;
IF COL_LENGTH('dbo.Factura', 'NumeroFactura') IS NULL
    ALTER TABLE dbo.Factura ADD NumeroFactura varchar(30) NULL;
IF COL_LENGTH('dbo.Factura', 'FechaVencimiento') IS NULL
    ALTER TABLE dbo.Factura ADD FechaVencimiento datetime NULL;
IF COL_LENGTH('dbo.Factura', 'SaldoPendiente') IS NULL
    ALTER TABLE dbo.Factura ADD SaldoPendiente decimal(12, 2) NULL;
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Pago_Factura')
    ALTER TABLE dbo.Pago DROP CONSTRAINT FK_Pago_Factura;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Factura_Cliente')
    ALTER TABLE dbo.Factura DROP CONSTRAINT FK_Factura_Cliente;
GO

IF OBJECT_ID('dbo.Factura_Nueva') IS NOT NULL
    DROP TABLE dbo.Factura_Nueva;
GO

CREATE TABLE dbo.Factura_Nueva (
    Id               int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    IdCliente        int NOT NULL,
    NumeroFactura    varchar(30) NOT NULL,
    fechaEmision     datetime NOT NULL CONSTRAINT DF_Factura_fechaEmision DEFAULT (getdate()),
    FechaVencimiento datetime NOT NULL,
    monto            decimal(12, 2) NOT NULL,
    SaldoPendiente   decimal(12, 2) NOT NULL,
    estadopago       varchar(20) NOT NULL
);
GO

SET IDENTITY_INSERT dbo.Factura_Nueva ON;

INSERT INTO dbo.Factura_Nueva (Id, IdCliente, NumeroFactura, fechaEmision, FechaVencimiento, monto, SaldoPendiente, estadopago)
SELECT f.Id,
       ISNULL(f.IdCliente, f.Id),
       ISNULL(f.NumeroFactura, 'FAC-' + RIGHT('000000' + CAST(f.Id AS varchar(10)), 6)),
       ISNULL(f.fechaEmision, GETDATE()),
       ISNULL(f.FechaVencimiento, DATEADD(DAY, 30, ISNULL(f.fechaEmision, GETDATE()))),
       f.monto,
       ISNULL(f.SaldoPendiente, f.monto - ISNULL((SELECT SUM(p.monto_abonado) FROM dbo.Pago p WHERE p.id_factura = f.Id), 0)),
       CASE WHEN f.estadopago = 'PAGADO' THEN 'PAGADA' ELSE f.estadopago END
FROM dbo.Factura f;

SET IDENTITY_INSERT dbo.Factura_Nueva OFF;
GO

DROP TABLE dbo.Factura;
GO

EXEC sp_rename 'dbo.Factura_Nueva', 'Factura';
GO

ALTER TABLE dbo.Factura ADD CONSTRAINT FK_Factura_Cliente
    FOREIGN KEY (IdCliente) REFERENCES dbo.Cliente (Id);
GO

ALTER TABLE dbo.Pago ADD CONSTRAINT FK_Pago_Factura
    FOREIGN KEY (id_factura) REFERENCES dbo.Factura (Id);
GO

UPDATE f
SET SaldoPendiente = f.monto - ISNULL(p.total, 0),
    estadopago = CASE
        WHEN f.monto - ISNULL(p.total, 0) <= 0 THEN 'PAGADA'
        WHEN ISNULL(p.total, 0) > 0 THEN 'PARCIAL'
        ELSE 'PENDIENTE'
    END
FROM dbo.Factura f
LEFT JOIN (
    SELECT id_factura, SUM(monto_abonado) AS total
    FROM dbo.Pago
    GROUP BY id_factura
) p ON p.id_factura = f.Id;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Factura_IdCliente' AND object_id = OBJECT_ID('dbo.Factura'))
    CREATE INDEX IX_Factura_IdCliente ON dbo.Factura (IdCliente);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Pago_id_factura' AND object_id = OBJECT_ID('dbo.Pago'))
    CREATE INDEX IX_Pago_id_factura ON dbo.Pago (id_factura);
GO

SELECT 'Clientes'  AS tabla, COUNT(*) AS filas FROM dbo.Cliente
UNION ALL
SELECT 'Facturas', COUNT(*) FROM dbo.Factura
UNION ALL
SELECT 'Pagos',    COUNT(*) FROM dbo.Pago;
GO
