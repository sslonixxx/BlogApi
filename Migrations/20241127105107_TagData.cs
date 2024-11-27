using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blog_api.Migrations
{
    public partial class TagData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[,]
                {
                    { Guid.Parse("7dd0e51a-606f-4dea-e6e3-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106519").ToUniversalTime(), "история" },
                    { Guid.Parse("d774dd11-2600-46ab-e6e4-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106539").ToUniversalTime(), "еда" },
                    { Guid.Parse("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106543").ToUniversalTime(), "18+" },
                    { Guid.Parse("553f5361-428a-4122-e6e6-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106553").ToUniversalTime(), "приколы" },
                    { Guid.Parse("86acb301-05ff-4822-e6e7-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106563").ToUniversalTime(), "it" },
                    { Guid.Parse("e587312f-4df7-4879-e6e8-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106573").ToUniversalTime(), "интернет" },
                    { Guid.Parse("47aa0a33-2b86-4039-e6e9-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106583").ToUniversalTime(), "теория_заговора" },
                    { Guid.Parse("e463050a-d659-433d-e6ea-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106592").ToUniversalTime(), "соцсети" },
                    { Guid.Parse("0c64569f-5675-484b-e6eb-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106602").ToUniversalTime(), "косплей" },
                    { Guid.Parse("fb1f2ce1-6943-420f-e6ec-08dbea521a91"), DateTime.Parse("2023-11-21T12:24:15.8106612").ToUniversalTime(), "преступление" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    Guid.Parse("7dd0e51a-606f-4dea-e6e3-08dbea521a91"),
                    Guid.Parse("d774dd11-2600-46ab-e6e4-08dbea521a91"),
                    Guid.Parse("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"),
                    Guid.Parse("553f5361-428a-4122-e6e6-08dbea521a91"),
                    Guid.Parse("86acb301-05ff-4822-e6e7-08dbea521a91"),
                    Guid.Parse("e587312f-4df7-4879-e6e8-08dbea521a91"),
                    Guid.Parse("47aa0a33-2b86-4039-e6e9-08dbea521a91"),
                    Guid.Parse("e463050a-d659-433d-e6ea-08dbea521a91"),
                    Guid.Parse("0c64569f-5675-484b-e6eb-08dbea521a91"),
                    Guid.Parse("fb1f2ce1-6943-420f-e6ec-08dbea521a91")
                });
        }
    }
}