CREATE PROCEDURE [dbo].[spSale_SaleReport]
AS
Begin
	Set NoCount On;
	Select [S].[SaleDate], [S].[SubTotal], [S].[Tax], [S].[Total], U.FirstName, U.LastName, U.EmailAddress
	From dbo.Sale S
	Inner join dbo.[User] U On S.CashierId = U.Id;
End
