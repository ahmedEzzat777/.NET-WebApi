using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpgWebApi.Migrations
{
    public partial class dataseeding2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "UserName" },
                values: new object[] { 1, new byte[] { 3, 148, 139, 75, 8, 48, 156, 239, 231, 63, 100, 138, 118, 228, 7, 113, 6, 158, 198, 125, 234, 129, 63, 247, 227, 112, 202, 61, 124, 29, 2, 210, 54, 229, 80, 178, 85, 151, 194, 2, 90, 52, 242, 113, 193, 178, 143, 26, 252, 205, 195, 151, 46, 133, 238, 179, 179, 183, 104, 132, 218, 217, 174, 3 }, new byte[] { 55, 173, 211, 24, 248, 68, 156, 62, 109, 148, 219, 222, 78, 120, 167, 245, 43, 150, 235, 70, 72, 159, 172, 179, 225, 239, 137, 88, 90, 55, 226, 183, 181, 193, 101, 59, 1, 192, 244, 25, 15, 15, 30, 201, 47, 131, 92, 172, 94, 134, 88, 249, 203, 82, 240, 165, 101, 255, 177, 228, 81, 219, 172, 74, 146, 167, 189, 154, 213, 13, 204, 41, 238, 255, 168, 102, 14, 53, 236, 243, 84, 109, 100, 230, 200, 113, 19, 241, 75, 66, 202, 86, 183, 98, 99, 33, 160, 130, 1, 180, 10, 128, 127, 227, 116, 233, 25, 130, 72, 52, 62, 191, 126, 207, 106, 95, 75, 61, 64, 169, 60, 171, 117, 222, 136, 217, 146, 9 }, "User1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "UserName" },
                values: new object[] { 2, new byte[] { 3, 148, 139, 75, 8, 48, 156, 239, 231, 63, 100, 138, 118, 228, 7, 113, 6, 158, 198, 125, 234, 129, 63, 247, 227, 112, 202, 61, 124, 29, 2, 210, 54, 229, 80, 178, 85, 151, 194, 2, 90, 52, 242, 113, 193, 178, 143, 26, 252, 205, 195, 151, 46, 133, 238, 179, 179, 183, 104, 132, 218, 217, 174, 3 }, new byte[] { 55, 173, 211, 24, 248, 68, 156, 62, 109, 148, 219, 222, 78, 120, 167, 245, 43, 150, 235, 70, 72, 159, 172, 179, 225, 239, 137, 88, 90, 55, 226, 183, 181, 193, 101, 59, 1, 192, 244, 25, 15, 15, 30, 201, 47, 131, 92, 172, 94, 134, 88, 249, 203, 82, 240, 165, 101, 255, 177, 228, 81, 219, 172, 74, 146, 167, 189, 154, 213, 13, 204, 41, 238, 255, 168, 102, 14, 53, 236, 243, 84, 109, 100, 230, 200, 113, 19, 241, 75, 66, 202, 86, 183, 98, 99, 33, 160, 130, 1, 180, 10, 128, 127, 227, 116, 233, 25, 130, 72, 52, 62, 191, 126, 207, 106, 95, 75, 61, 64, 169, 60, 171, 117, 222, 136, 217, 146, 9 }, "User1" });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Class", "Defeats", "Defense", "Fights", "HitPoints", "Intelligence", "Name", "Strength", "UserId", "Victories" },
                values: new object[] { 1, 1, 0, 10, 0, 100, 10, "Frodo", 10, 1, 0 });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Class", "Defeats", "Defense", "Fights", "HitPoints", "Intelligence", "Name", "Strength", "UserId", "Victories" },
                values: new object[] { 2, 1, 0, 10, 0, 100, 10, "Frodo", 10, 2, 0 });

            migrationBuilder.InsertData(
                table: "CharacterSkills",
                columns: new[] { "CharacterId", "SkillId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "CharacterSkills",
                columns: new[] { "CharacterId", "SkillId" },
                values: new object[] { 2, 3 });

            migrationBuilder.InsertData(
                table: "CharacterSkills",
                columns: new[] { "CharacterId", "SkillId" },
                values: new object[] { 2, 2 });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "CharacterId", "Damage", "Name" },
                values: new object[] { 1, 1, 30, "sword" });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "CharacterId", "Damage", "Name" },
                values: new object[] { 2, 2, 40, "short sword" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CharacterSkills",
                keyColumns: new[] { "CharacterId", "SkillId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "CharacterSkills",
                keyColumns: new[] { "CharacterId", "SkillId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "CharacterSkills",
                keyColumns: new[] { "CharacterId", "SkillId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "Weapons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Weapons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
