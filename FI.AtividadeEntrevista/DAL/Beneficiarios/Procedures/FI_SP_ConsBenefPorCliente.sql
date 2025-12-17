CREATE PROCEDURE FI_SP_ConsBenefPorCliente
    @IdCliente BIGINT
AS
BEGIN
    SELECT Id, IdCliente, CPF, Nome
    FROM BENEFICIARIOS
    WHERE IdCliente = @IdCliente
    ORDER BY Nome
END