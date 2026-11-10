USE G_CuentasXCobrar;
GO

IF OBJECT_ID('dbo.FK_Pago_Factura', 'F') IS NOT NULL
    ALTER TABLE dbo.Pago DROP CONSTRAINT FK_Pago_Factura;
GO

IF OBJECT_ID('dbo.FK_Factura_Cliente', 'F') IS NOT NULL
    ALTER TABLE dbo.Factura DROP CONSTRAINT FK_Factura_Cliente;
GO

IF OBJECT_ID('dbo.Pago', 'U') IS NOT NULL DROP TABLE dbo.Pago;
GO
IF OBJECT_ID('dbo.Factura', 'U') IS NOT NULL DROP TABLE dbo.Factura;
GO
IF OBJECT_ID('dbo.Cliente', 'U') IS NOT NULL DROP TABLE dbo.Cliente;
GO

CREATE TABLE dbo.Cliente
(
    Id          INT IDENTITY(1,1) NOT NULL,
    RazonSocial VARCHAR(150)      NOT NULL,
    Ruc         VARCHAR(13)       NOT NULL,
    Correo      VARCHAR(100)      NULL,
    Telefono    VARCHAR(20)       NULL,
    CONSTRAINT PK_Cliente PRIMARY KEY (Id),
    CONSTRAINT UQ_Cliente_Ruc UNIQUE (Ruc)
);
GO

CREATE TABLE dbo.Factura
(
    Id               INT IDENTITY(1,1) NOT NULL,
    IdCliente        INT               NOT NULL,
    NumeroFactura    VARCHAR(30)       NOT NULL,
    FechaEmision     DATE              NOT NULL,
    FechaVencimiento DATE              NOT NULL,
    Monto            DECIMAL(18,2)     NOT NULL,
    SaldoPendiente   DECIMAL(18,2)     NOT NULL,
    EstadoPago       VARCHAR(20)       NOT NULL CONSTRAINT DF_Factura_EstadoPago DEFAULT ('PENDIENTE'),
    CONSTRAINT PK_Factura PRIMARY KEY (Id),
    CONSTRAINT FK_Factura_Cliente FOREIGN KEY (IdCliente) REFERENCES dbo.Cliente (Id),
    CONSTRAINT UQ_Factura_Numero UNIQUE (NumeroFactura),
    CONSTRAINT CK_Factura_Monto CHECK (Monto > 0),
    CONSTRAINT CK_Factura_Saldo CHECK (SaldoPendiente >= 0),
    CONSTRAINT CK_Factura_Fechas CHECK (FechaVencimiento >= FechaEmision),
    CONSTRAINT CK_Factura_Estado CHECK (EstadoPago IN ('PENDIENTE', 'PARCIAL', 'PAGADA'))
);
GO

CREATE TABLE dbo.Pago
(
    Id           INT IDENTITY(1,1) NOT NULL,
    IdFactura    INT               NOT NULL,
    FechaPago    DATE              NOT NULL CONSTRAINT DF_Pago_FechaPago DEFAULT (CAST(GETDATE() AS DATE)),
    MontoAbonado DECIMAL(18,2)     NOT NULL,
    MetodoPago   VARCHAR(50)       NOT NULL,
    Referencia   VARCHAR(100)      NULL,
    CONSTRAINT PK_Pago PRIMARY KEY (Id),
    CONSTRAINT FK_Pago_Factura FOREIGN KEY (IdFactura) REFERENCES dbo.Factura (Id),
    CONSTRAINT CK_Pago_Monto CHECK (MontoAbonado > 0)
);
GO
