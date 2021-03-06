USE [Seerys]
GO
/****** Object:  StoredProcedure [dbo].[tcd_updCustomer]    Script Date: 11/30/2010 10:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[tcd_updCustomer]
	@CustomerId int,
	@CustomerName nchar(255),
	@Address ntext,
	@DiscountPercent float,
	@IsVatExempt bit,
	@Route nchar(50),
	@PaymentType nchar(50) = ' '
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

update tcd_tblCustomers set CustomerName = @CustomerName, Address = @Address, DiscountPercent = @DiscountPercent,
IsVatExempt = @IsVatExempt, Route = @Route, PaymentType = @PaymentType where CustomerID = @CustomerId

END
