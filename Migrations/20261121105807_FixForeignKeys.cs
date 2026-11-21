using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrack.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId1",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Students_StudentId1",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_StudentId1",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Students_CourseId1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "Submissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId1",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId1",
                table: "Submissions",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseId1",
                table: "Students",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId1",
                table: "Students",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Students_StudentId1",
                table: "Submissions",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
