using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UstaTakip.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverageAmountInsurancePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CoverageAmount",
                table: "InsurancePolicies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverageAmount",
                table: "InsurancePolicies");
        }
    }
}
