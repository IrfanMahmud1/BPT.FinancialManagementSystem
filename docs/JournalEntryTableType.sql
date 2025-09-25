USE [BPT.FMS]
GO

/****** Object:  UserDefinedTableType [dbo].[JournalEntryTableType]    Script Date: 9/25/2025 2:44:11 PM ******/
CREATE TYPE [dbo].[JournalEntryTableType] AS TABLE(
	[EntryId] [uniqueidentifier] NULL,
	[JournalId] [uniqueidentifier] NULL,
	[ChartOfAccountId] [uniqueidentifier] NULL,
	[Debit] [decimal](18, 2) NULL,
	[Credit] [decimal](18, 2) NULL
)
GO


