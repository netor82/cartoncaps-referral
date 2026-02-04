using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartonCaps.Referrals.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReferralIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ReferredUserId",
                table: "Referrals",
                column: "ReferredUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_ReferrerUserId",
                table: "Referrals",
                column: "ReferrerUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Referrals_ReferredUserId",
                table: "Referrals");

            migrationBuilder.DropIndex(
                name: "IX_Referrals_ReferrerUserId",
                table: "Referrals");
        }
    }
}
