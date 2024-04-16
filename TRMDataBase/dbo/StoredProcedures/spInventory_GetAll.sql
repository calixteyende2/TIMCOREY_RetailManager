CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
Begin
	Set NoCount On;
	Select [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
	From dbo.Inventory;
End
