using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_api.Migrations
{

    public partial class UpdatedDataContext : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleEntity",
                table: "VehicleEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEntity",
                table: "UserEntity");

            migrationBuilder.RenameTable(
                name: "VehicleEntity",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "UserEntity",
                newName: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Vehicles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "VehicleEntity");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserEntity");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "VehicleEntity",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserEntity",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleEntity",
                table: "VehicleEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEntity",
                table: "UserEntity",
                column: "Id");
        }
    }
}
