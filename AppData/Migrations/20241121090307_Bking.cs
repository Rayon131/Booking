using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    /// <inheritdoc />
    public partial class Bking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DichVus_LoaiPhongs_LoaiPhongId",
                table: "DichVus");

            migrationBuilder.AddForeignKey(
                name: "FK_DichVus_LoaiPhongs_LoaiPhongId",
                table: "DichVus",
                column: "LoaiPhongId",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DichVus_LoaiPhongs_LoaiPhongId",
                table: "DichVus");

            migrationBuilder.AddForeignKey(
                name: "FK_DichVus_LoaiPhongs_LoaiPhongId",
                table: "DichVus",
                column: "LoaiPhongId",
                principalTable: "LoaiPhongs",
                principalColumn: "MaLoaiPhong");
        }
    }
}
