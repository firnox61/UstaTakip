using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UstaTakip.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentRateToInsurancePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentRate",
                table: "InsurancePayments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentRate",
                table: "InsurancePayments");
        }
    }
}
