using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blog_api.Migrations
{
    public partial class CommunityData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Community",
                columns: new[] { "Id", "CreateTime", "Name", "Description", "IsClosed", "SubscribersCount" },
                values: new object[,]
                {
                    { Guid.Parse("21db62c6-a964-45c1-17e0-08dbea521a96"), DateTime.Parse("2023-11-21T12:24:15.8106622").ToUniversalTime(), "Масонская ложа", "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами", true, 49 },
                    { Guid.Parse("c5639aab-3a25-4efc-17e1-08dbea521a96"), DateTime.Parse("2023-11-21T12:24:15.8106695").ToUniversalTime(), "Следствие вели с Л. Каневским", "Без длинных предисловий: мужчина умер", false, 36 },
                    { Guid.Parse("b9851a35-b836-47f8-17e2-08dbea521a96"), DateTime.Parse("2023-11-21T12:24:15.8106705").ToUniversalTime(), "IT <3", "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления", false, 34 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Community",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    Guid.Parse("21db62c6-a964-45c1-17e0-08dbea521a96"),
                    Guid.Parse("c5639aab-3a25-4efc-17e1-08dbea521a96"),
                    Guid.Parse("b9851a35-b836-47f8-17e2-08dbea521a96")
                });
        }
    }
}