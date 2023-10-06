using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class fixFkAccountRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_m_account_roles_account_guid",
                table: "tb_m_account_roles");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_account_roles_account_guid",
                table: "tb_m_account_roles",
                column: "account_guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_m_account_roles_account_guid",
                table: "tb_m_account_roles");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_account_roles_account_guid",
                table: "tb_m_account_roles",
                column: "account_guid",
                unique: true);
        }
    }
}
