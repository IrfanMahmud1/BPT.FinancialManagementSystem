﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPT.FMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVoucherTableType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
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
            var sql = "DROP TYPE [dbo].[VoucherEntryTableType]";
            migrationBuilder.Sql(sql);
        }
    }
}
