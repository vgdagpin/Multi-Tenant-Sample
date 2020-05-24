using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiTenantSample.DbMigration.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantPersonnels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true, defaultValueSql: "CONVERT(INT, SESSION_CONTEXT(N'TenantId'))"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PrefixId = table.Column<int>(nullable: false),
                    GenderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantPersonnels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantPersonnels_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Female" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Male" });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Male" });

            migrationBuilder.InsertData(
                table: "TenantPersonnels",
                columns: new[] { "Id", "Active", "DOB", "FirstName", "GenderId", "LastName", "MiddleName", "PrefixId", "TenantId" },
                values: new object[] { 1, true, new DateTime(1955, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hazel", 1, "Ramos", "Peterson", 1, 1 });

            migrationBuilder.InsertData(
                table: "TenantPersonnels",
                columns: new[] { "Id", "Active", "DOB", "FirstName", "GenderId", "LastName", "MiddleName", "PrefixId", "TenantId" },
                values: new object[] { 2, true, new DateTime(1976, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dwight", 2, "Nguyen", null, 1, 1 });

            migrationBuilder.InsertData(
                table: "TenantPersonnels",
                columns: new[] { "Id", "Active", "DOB", "FirstName", "GenderId", "LastName", "MiddleName", "PrefixId", "TenantId" },
                values: new object[] { 3, true, new DateTime(1969, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kyle", 3, "Davis", null, 1, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_TenantPersonnels_GenderId",
                table: "TenantPersonnels",
                column: "GenderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantPersonnels");

            migrationBuilder.DropTable(
                name: "Genders");
        }
    }
}
