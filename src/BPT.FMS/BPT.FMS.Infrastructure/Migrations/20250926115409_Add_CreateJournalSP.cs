using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPT.FMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_CreateJournalSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                CREATE OR ALTER     PROCEDURE [dbo].[sp_CreateJournal]
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
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[sp_CreateJournal]";
            migrationBuilder.Sql(sql);
        }
    }
}
