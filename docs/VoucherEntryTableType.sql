USE [BPT.FMS]
GO

/****** Object:  UserDefinedTableType [dbo].[VoucherEntryTableType]    Script Date: 9/25/2025 2:44:46 PM ******/
CREATE TYPE [dbo].[VoucherEntryTableType] AS TABLE(
	[EntryId] [uniqueidentifier] NULL,
	[VoucherId] [uniqueidentifier] NULL,
	[ChartOfAccountId] [uniqueidentifier] NULL,
	[Debit] [decimal](18, 2) NULL,
	[Credit] [decimal](18, 2) NULL
)
GO


