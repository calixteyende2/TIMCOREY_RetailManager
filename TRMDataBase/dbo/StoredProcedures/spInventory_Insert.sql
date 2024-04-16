CREATE PROCEDURE [dbo].[spInventory_Insert]
	@ProductId int, 
	@Quantity int, 
	@PurchasePrice money, 
	@PurchaseDate datetime2
	
AS

Begin
	
	Set Nocount On;

	Insert Into dbo.Inventory(ProductId, Quantity, PurchasePrice, PurchaseDate)
	Values(@ProductId, @Quantity, @PurchasePrice, @PurchaseDate);

End
