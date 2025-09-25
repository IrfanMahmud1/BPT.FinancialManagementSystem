using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPT.FMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JournalTableType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                USE [BPT.FMS]
                GO

                /****** Object:  UserDefinedTableType [dbo].[JournalEntryTableType]    Script Date: 9/25/2025 6:01:39 AM ******/
                CREATE TYPE [dbo].[JournalEntryTableType] AS TABLE(
                	[EntryId] [uniqueidentifier] NULL,
                	[JournalId] [uniqueidentifier] NULL,
                	[ChartOfAccountId] [uniqueidentifier] NULL,
                	[Debit] [decimal](18, 2) NULL,
                	[Credit] [decimal](18, 2) NULL
                )
                GO
                
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                USE [BPT.FMS]
                GO
                DROP TYPE [dbo].[JournalEntryTableType]
                GO
                """";
            migrationBuilder.Sql(sql);
        }
    }
}
