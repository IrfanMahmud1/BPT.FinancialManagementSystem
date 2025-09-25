using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPT.FMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateJournalStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                USE [BPT.FMS]
                GO
                /****** Object:  StoredProcedure [dbo].[CreateJournal]    Script Date: 9/25/2025 12:11:15 AM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE OR ALTER PROCEDURE [dbo].[sp_CreateJournal]
                    @JournalId UNIQUEIDENTIFIER,
                    @Date DATETIME,
                    @ReferenceNo NVARCHAR(50),
                    @Type NVARCHAR(20),
                    @Entries dbo.VoucherEntryTableType READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO Journals (Id, Date, ReferenceNo, Type)
                    VALUES (@JournalId, @Date, @ReferenceNo, @Type);
                    INSERT INTO JournalEntries (Id, JournalId, ChartOfAccountId, Debit, Credit)
                    SELECT EntryId, @JournalId, ChartOfAccountId, Debit, Credit
                    FROM @Entries;
                END
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                USE [BPT.FMS]
                GO
                DROP PROCEDURE [dbo].[CreateJournal]
                GO
                """;
            migrationBuilder.Sql(sql);
        }
    }
}
