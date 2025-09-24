using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPT.FMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class storedprocedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """"
                                USE [BPT.FMS]
                GO
                /****** Object:  StoredProcedure [dbo].[sp_CreateVoucher]    Script Date: 9/24/2025 5:39:56 PM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                	CREATE OR ALTER PROCEDURE [dbo].[sp_CreateVoucher]
                    @VoucherId UNIQUEIDENTIFIER,
                    @Date DATE,
                    @ReferenceNo NVARCHAR(50),
                    @Type NVARCHAR(10),
                    @Entries dbo.VoucherEntryTableType READONLY
                AS
                BEGIN
                    BEGIN TRY
                        BEGIN TRANSACTION;

                        -- Insert into Vouchers table
                        INSERT INTO Vouchers (Id, Date, ReferenceNo, Type)
                        VALUES (@VoucherId, @Date, @ReferenceNo, @Type);

                        -- Insert into VoucherEntries table
                        INSERT INTO VoucherEntries (Id, VoucherId, ChartOfAccountId, Debit, Credit)
                        SELECT EntryId, VoucherId, ChartOfAccountId, Debit, Credit
                        FROM @Entries;

                        COMMIT;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK;
                        THROW;
                    END CATCH
                END

                
                """";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[sp_CreateVoucher]";
            migrationBuilder.Sql(sql);
        }
    }
}
