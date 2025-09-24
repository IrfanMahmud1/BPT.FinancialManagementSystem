using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPT.FMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sqltype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                USE [BPT.FMS]
                GO

                /****** Object:  UserDefinedTableType [dbo].[VoucherEntryTableType]    Script Date: 9/24/2025 5:40:25 PM ******/
                CREATE TYPE [dbo].[VoucherEntryTableType] AS TABLE(
                	[EntryId] [uniqueidentifier] NULL,
                	[VoucherId] [uniqueidentifier] NULL,
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
            var sql = "DROP TYPE [dbo].[VoucherEntryTableType];\r\n";
                migrationBuilder.Sql(sql);
        }
    }
}
