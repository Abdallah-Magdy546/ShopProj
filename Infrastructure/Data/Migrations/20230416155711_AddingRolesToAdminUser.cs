using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationProj.Data.Migrations
{
    public partial class AddingRolesToAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [ShoppingProjDb].[dbo].[AspNetUserRoles] (UserId , RoleId) SELECT '087e3575-8a48-4e2f-bfee-9d532b511102' ,ID FROM [ShoppingProjDb].[dbo].[AspNetRoles]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [ShoppingProjDb].[dbo].[AspNetUserRoles] WHERE UserId = '087e3575-8a48-4e2f-bfee-9d532b511102'");
        }
    }
}
