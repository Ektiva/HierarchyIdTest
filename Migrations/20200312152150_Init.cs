using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HierarchyIdTest1.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    DomainId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainName = table.Column<string>(maxLength: 256, nullable: false),
                    DomainTypeId = table.Column<int>(nullable: false),
                    Parentt = table.Column<string>(nullable: false),
                    HighLevel = table.Column<int>(nullable: false),
                    Level = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.DomainId);
                });

            migrationBuilder.CreateTable(
                name: "DomainTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewDomains",
                columns: table => new
                {
                    DomainId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainName = table.Column<string>(maxLength: 256, nullable: false),
                    DomainTypeId = table.Column<int>(nullable: false),
                    Parentt = table.Column<string>(nullable: false),
                    Node = table.Column<byte[]>(maxLength: 892, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewDomains", x => x.DomainId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropTable(
                name: "DomainTypes");

            migrationBuilder.DropTable(
                name: "NewDomains");
        }
    }
}
