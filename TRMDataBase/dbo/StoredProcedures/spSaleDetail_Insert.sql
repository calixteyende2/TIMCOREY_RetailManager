CREATE PROCEDURE [dbo].[spSaleDetail_Insert]
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@Tax money

	As
	Begin
		Set NoCount On;

		Insert Into dbo.SaleDetail(SaleId, ProductId, Quantity, PurchasePrice, Tax)
		Values(@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax)
	End
