USE G_CuentasXCobrar;
GO

-- Agregar columnas faltantes a Cliente
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Cliente') AND name = 'correo')
    ALTER TABLE dbo.Cliente ADD correo VARCHAR(100) NULL;
GO

-- Agregar columnas faltantes a Factura
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Factura') AND name = 'IdCliente')
    ALTER TABLE dbo.Factura ADD IdCliente INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Factura') AND name = 'NumeroFactura')
    ALTER TABLE dbo.Factura ADD NumeroFactura VARCHAR(30) NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Factura') AND name = 'FechaVencimiento')
    ALTER TABLE dbo.Factura ADD FechaVencimiento DATE NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Factura') AND name = 'SaldoPendiente')
    ALTER TABLE dbo.Factura ADD SaldoPendiente DECIMAL(18,2) NULL;
GO

-- Reparar FK: quitar la mala (Factura.Id -> Cliente.Id) y crear la correcta (Factura.IdCliente -> Cliente.Id)
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Factura_Cliente')
    ALTER TABLE dbo.Factura DROP CONSTRAINT FK_Factura_Cliente;
GO

ALTER TABLE dbo.Factura
ADD CONSTRAINT FK_Factura_Cliente FOREIGN KEY (IdCliente) REFERENCES dbo.Cliente(Id);
GO

CREATE INDEX IX_Factura_IdCliente ON dbo.Factura(IdCliente);
CREATE INDEX IX_Factura_FechaVencimiento ON dbo.Factura(FechaVencimiento);
CREATE INDEX IX_Pago_IdFactura ON dbo.Pago(id_factura);
GO

SELECT 'Migracion lista' AS Resultado;
