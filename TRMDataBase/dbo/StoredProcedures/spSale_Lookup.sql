CREATE PROCEDURE [dbo].[spSale_Lookup]
	@CashierId nvarchar(128),
	@SaleDate datetime2
AS
Begin
	Set NoCount On;
	Select Id
	From dbo.Sale
	Where CashierId = @CashierId And SaleDate = @SaleDate;
End
