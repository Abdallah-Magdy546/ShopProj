using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationProj.Data.Migrations
{
    public partial class AddingAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[users] ([Id], [GENDER], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'087e3575-8a48-4e2f-bfee-9d532b511102', N'Male', N'Admin', N'ADMIN', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEHt47C2B2a+ZPaobHbzTojNgcPTNs2bCeFE/6dlTRXmziOzeDajHk74AucP6GSecHw==', N'HFQWPCDWCI6EH63MN2VCWRNQRSZW7ZP7', N'8979c64b-efa4-4318-879a-0c74018c4a1a', N'1212321213', 0, 0, NULL, 1, 0)\r\n");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[users] WHERE Id='087e3575-8a48-4e2f-bfee-9d532b511102' ");
        }
    }
}
