using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class addEducation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "gpa",
                table: "tb_m_educations",
                type: "real",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "gpa",
                table: "tb_m_educations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
