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

