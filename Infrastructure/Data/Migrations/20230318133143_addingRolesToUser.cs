using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationProj.Data.Migrations
{
    public partial class addingRolesToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AspNetUserRoles (UserId , RoleId) SELECT 'd8777f90-c2b4-448d-8399-99ee928145e4' ,ID FROM AspNetRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId = 'd8777f90-c2b4-448d-8399-99ee928145e4'");
        }
    }
}
