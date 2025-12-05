using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UstaTakip.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RepairJobInsurancePayment_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InsurancePayments_RepairJobId",
                table: "InsurancePayments");

            migrationBuilder.DropColumn(
                name: "CoverageAmount",
                table: "InsurancePolicies");

            migrationBuilder.AddColumn<int>(
                name: "InsurancePaymentRate",
                table: "RepairJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "InsurancePolicyId",
                table: "RepairJobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgencyCode",
                table: "InsurancePolicies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RepairJobs_InsurancePolicyId",
                table: "RepairJobs",
                column: "InsurancePolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePayments_RepairJobId",
                table: "InsurancePayments",
                column: "RepairJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairJobs_InsurancePolicies_InsurancePolicyId",
                table: "RepairJobs",
                column: "InsurancePolicyId",
                principalTable: "InsurancePolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairJobs_InsurancePolicies_InsurancePolicyId",
                table: "RepairJobs");

            migrationBuilder.DropIndex(
                name: "IX_RepairJobs_InsurancePolicyId",
                table: "RepairJobs");

            migrationBuilder.DropIndex(
                name: "IX_InsurancePayments_RepairJobId",
                table: "InsurancePayments");

            migrationBuilder.DropColumn(
                name: "InsurancePaymentRate",
                table: "RepairJobs");

            migrationBuilder.DropColumn(
                name: "InsurancePolicyId",
                table: "RepairJobs");

            migrationBuilder.DropColumn(
                name: "AgencyCode",
                table: "InsurancePolicies");

            migrationBuilder.AddColumn<decimal>(
                name: "CoverageAmount",
                table: "InsurancePolicies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePayments_RepairJobId",
                table: "InsurancePayments",
                column: "RepairJobId",
                unique: true);
        }
    }
}
