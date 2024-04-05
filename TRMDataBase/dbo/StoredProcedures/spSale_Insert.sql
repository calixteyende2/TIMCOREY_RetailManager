CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id int output,
	@CashierId nvarchar(128),
	@SaleDate datetime2,
	@SubTotal money,
	@Tax money,
	@Total money
AS
Begin
	Set NoCount On;

	Insert Into dbo.Sale(CashierId, SaleDate, SubTotal, Tax, Total)
	Values(@CashierId, @SaleDate, @SubTotal, @Tax, @Total);

	Select @Id = @@IDENTITY;
End
