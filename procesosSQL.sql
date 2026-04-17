USE [master]
GO

CREATE DATABASE [PracticaS13]
GO

USE [PracticaS13]
GO

CREATE TABLE [dbo].[Principal](
  [Id_Compra] [bigint] IDENTITY(1,1) NOT NULL,
  [Precio] [decimal](18, 5) NOT NULL,
  [Saldo] [decimal](18, 5) NOT NULL,
  [Descripcion] [varchar](500) NOT NULL,
  [Estado] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Principal] PRIMARY KEY CLUSTERED ([Id_Compra] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Abonos](
  [Id_Compra] [bigint] NOT NULL,
  [Id_Abono] [bigint] IDENTITY(1,1) NOT NULL,
  [Monto] [decimal](18, 2) NOT NULL,
  [Fecha] [datetime] NOT NULL,
 CONSTRAINT [PK_Abonos] PRIMARY KEY CLUSTERED ([Id_Abono] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Abonos] WITH CHECK ADD CONSTRAINT [FK_Abonos_Principal]
  FOREIGN KEY([Id_Compra]) REFERENCES [dbo].[Principal] ([Id_Compra])
GO
ALTER TABLE [dbo].[Abonos] CHECK CONSTRAINT [FK_Abonos_Principal]
GO

SET IDENTITY_INSERT [dbo].[Principal] ON
GO
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (1, CAST(50000.00000 AS Decimal(18, 5)), CAST(50000.00000 AS Decimal(18, 5)), N'Producto 1', N'Pendiente')
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (2, CAST(13500.00000 AS Decimal(18, 5)), CAST(13500.00000 AS Decimal(18, 5)), N'Producto 2', N'Pendiente')
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (3, CAST(83600.00000 AS Decimal(18, 5)), CAST(83600.00000 AS Decimal(18, 5)), N'Producto 3', N'Pendiente')
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (4, CAST(1220.00000 AS Decimal(18, 5)), CAST(1220.00000 AS Decimal(18, 5)), N'Producto 4', N'Pendiente')
INSERT [dbo].[Principal] ([Id_Compra], [Precio], [Saldo], [Descripcion], [Estado]) VALUES (5, CAST(480.00000 AS Decimal(18, 5)), CAST(480.00000 AS Decimal(18, 5)), N'Producto 5', N'Pendiente')
GO
SET IDENTITY_INSERT [dbo].[Principal] OFF
GO

CREATE PROCEDURE ConsultarCompras
AS
BEGIN
    SELECT
        Id_Compra,
        Descripcion,
        Precio,
        Saldo,
        Estado
    FROM Principal
    ORDER BY 
        CASE WHEN Estado = 'Pendiente' THEN 0 ELSE 1 END,
        Id_Compra
END
GO

CREATE PROCEDURE ConsultarComprasPendientes
AS
BEGIN
    SELECT 
        Id_Compra,
        Descripcion
    FROM Principal
    WHERE Estado = 'Pendiente'
END
GO

CREATE PROCEDURE ConsultarSaldoCompra
    @Id_Compra BIGINT
AS
BEGIN
    SELECT 
        Id_Compra,
        Saldo
    FROM Principal
    WHERE Id_Compra = @Id_Compra
END
GO

CREATE PROCEDURE RegistrarAbono
    @Id_Compra BIGINT,
    @Monto DECIMAL(18,2)
AS
BEGIN
    DECLARE @SaldoActual DECIMAL(18,5)

    SELECT @SaldoActual = Saldo
    FROM Principal
    WHERE Id_Compra = @Id_Compra

    IF @Monto > @SaldoActual
    BEGIN
        RAISERROR('El abono no puede ser mayor al saldo anterior.', 16, 1)
        RETURN
    END

    INSERT INTO Abonos (Id_Compra, Monto, Fecha)
    VALUES (@Id_Compra, @Monto, GETDATE())

    UPDATE Principal
    SET 
        Saldo = Saldo - @Monto,
        Estado = CASE 
                    WHEN (Saldo - @Monto) = 0 THEN 'Cancelado'
                    ELSE Estado
                 END
    WHERE Id_Compra = @Id_Compra
END
GO

