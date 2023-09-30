using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VulnerableAPI.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "LastName", "PasswordHash", "PasswordSalt" },
                values: new object[,]
                {
                    { new Guid("133e3479-43b7-484a-8449-0ce31bc82f4c"), "ida@hillswisozk.biz", "Brianne", false, "Bartell", "hash", "salt" },
                    { new Guid("15a61231-a186-4fc9-ae24-a83d582eaa4d"), "carmen@marvinhills.name", "Rico", false, "Runolfsdottir", "hash", "salt" },
                    { new Guid("82f6754f-5d67-4b59-a2c1-936e1d72396d"), "eugene_damore@monahan.uk", "Trent", false, "Will", "hash", "salt" },
                    { new Guid("842b911c-7000-4714-b615-52a8849d569f"), "rupert_mayert@terryhirthe.co.uk", "Mariane", false, "Harber", "hash", "salt" },
                    { new Guid("8adb7e81-d14a-45d7-9b5d-dfc222643029"), "shyann@bergnaum.com", "Sid", false, "Considine", "hash", "salt" },
                    { new Guid("92b23f71-e891-4616-8e62-f28c761feb25"), "fletcher_ryan@price.co.uk", "Kristofer", false, "Schroeder", "hash", "salt" },
                    { new Guid("a28af4ba-d62c-4a7a-8109-9ff9fac19423"), "hilda.kirlin@gleason.ca", "Claudie", false, "Franecki", "hash", "salt" },
                    { new Guid("d3ece0e4-6cb7-4af6-a98b-7bb4169c8b8d"), "bonita@gislasonrosenbaum.ca", "Carolina", false, "Murray", "hash", "salt" },
                    { new Guid("ed807edb-f6bb-4dcf-b293-72d5e4262157"), "curt@powlowski.info", "Andreane", false, "Gleason", "hash", "salt" },
                    { new Guid("f373061d-5a04-4725-9140-243236afd26a"), "marquise@schuster.info", "Robb", false, "Hermiston", "hash", "salt" },
                    { new Guid("fc66e5ef-d1e8-4f7d-a845-903f93a90387"), "deontae@pouros.ca", "Brooklyn", false, "Goyette", "hash", "salt" },
                    { new Guid("fcffdaad-83bb-4ba1-83e9-90caeecf147d"), "lauryn.hilll@harveysmith.co.uk", "Manuel", false, "Leuschke", "hash", "salt" },
                    { new Guid("fd4bd871-01e0-46fc-bbbd-8b0311659f57"), "murray_jast@bogan.biz", "Jalen", false, "Erdman", "hash", "salt" },
                    { new Guid("ff9763ee-b247-48a3-84bd-7099a54a2074"), "phoebe_abernathy@barrows.us", "Ruben", false, "Yost", "hash", "salt" }
                });

            migrationBuilder.InsertData(
                table: "Ledgers",
                columns: new[] { "Id", "Balance", "Currency", "Iban", "UserId" },
                values: new object[,]
                {
                    { new Guid("0065e6ad-b62b-42b9-aa46-ebd403d70f66"), 606.0, 0, "LT320065400000000904", new Guid("92b23f71-e891-4616-8e62-f28c761feb25") },
                    { new Guid("1cbdc2b6-bf6b-45bc-b10a-ca37791293ea"), 313.0, 0, "LT390065400000000196", new Guid("a28af4ba-d62c-4a7a-8109-9ff9fac19423") },
                    { new Guid("646f7b61-7d2f-4b04-a26b-7c44a5dd84a2"), 158.0, 0, "LT850065400000000338", new Guid("fcffdaad-83bb-4ba1-83e9-90caeecf147d") },
                    { new Guid("74d40f86-6b6a-4fa1-8e46-b274255618e9"), 664.0, 0, "LT100065400000000912", new Guid("ed807edb-f6bb-4dcf-b293-72d5e4262157") },
                    { new Guid("813185ff-1002-45e5-8892-ade558884846"), 100.0, 0, "LT810065400000000216", new Guid("f373061d-5a04-4725-9140-243236afd26a") },
                    { new Guid("86e5521e-665f-4ca7-ae1c-846d1d35dfe8"), 695.0, 0, "LT120065400000000391", new Guid("ff9763ee-b247-48a3-84bd-7099a54a2074") },
                    { new Guid("aa4f643e-59d5-4f22-b7d4-893c3855f010"), 624.0, 0, "LT530065400000000623", new Guid("82f6754f-5d67-4b59-a2c1-936e1d72396d") },
                    { new Guid("ac17bc08-8501-4165-bb97-5e77b1d5bbaa"), 469.0, 0, "LT620065400000000267", new Guid("8adb7e81-d14a-45d7-9b5d-dfc222643029") },
                    { new Guid("aff85785-faeb-40cf-9169-1c796819cea3"), 643.0, 0, "LT650065400000000892", new Guid("fc66e5ef-d1e8-4f7d-a845-903f93a90387") },
                    { new Guid("b46fe692-341d-4775-bcc5-242826552655"), 673.0, 0, "LT020065400000000571", new Guid("fd4bd871-01e0-46fc-bbbd-8b0311659f57") },
                    { new Guid("c4dbca8d-7aa3-475f-bde9-aa81bd7d458f"), 309.0, 0, "LT360065400000000153", new Guid("133e3479-43b7-484a-8449-0ce31bc82f4c") },
                    { new Guid("cbda0a4c-6420-4379-a6c9-cb31cd2a8f8a"), 540.0, 0, "LT360065400000000444", new Guid("d3ece0e4-6cb7-4af6-a98b-7bb4169c8b8d") },
                    { new Guid("dc63e02a-8375-46c8-811d-0cb2249cbecc"), 103.0, 0, "LT800065400000000428", new Guid("15a61231-a186-4fc9-ae24-a83d582eaa4d") },
                    { new Guid("f4352f51-5ccb-479e-a6c8-2d5f79b6d718"), 140.0, 0, "LT260065400000000915", new Guid("842b911c-7000-4714-b615-52a8849d569f") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("0065e6ad-b62b-42b9-aa46-ebd403d70f66"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("1cbdc2b6-bf6b-45bc-b10a-ca37791293ea"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("646f7b61-7d2f-4b04-a26b-7c44a5dd84a2"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("74d40f86-6b6a-4fa1-8e46-b274255618e9"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("813185ff-1002-45e5-8892-ade558884846"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("86e5521e-665f-4ca7-ae1c-846d1d35dfe8"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("aa4f643e-59d5-4f22-b7d4-893c3855f010"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("ac17bc08-8501-4165-bb97-5e77b1d5bbaa"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("aff85785-faeb-40cf-9169-1c796819cea3"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("b46fe692-341d-4775-bcc5-242826552655"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("c4dbca8d-7aa3-475f-bde9-aa81bd7d458f"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("cbda0a4c-6420-4379-a6c9-cb31cd2a8f8a"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("dc63e02a-8375-46c8-811d-0cb2249cbecc"));

            migrationBuilder.DeleteData(
                table: "Ledgers",
                keyColumn: "Id",
                keyValue: new Guid("f4352f51-5ccb-479e-a6c8-2d5f79b6d718"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("133e3479-43b7-484a-8449-0ce31bc82f4c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("15a61231-a186-4fc9-ae24-a83d582eaa4d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("82f6754f-5d67-4b59-a2c1-936e1d72396d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("842b911c-7000-4714-b615-52a8849d569f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8adb7e81-d14a-45d7-9b5d-dfc222643029"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("92b23f71-e891-4616-8e62-f28c761feb25"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a28af4ba-d62c-4a7a-8109-9ff9fac19423"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d3ece0e4-6cb7-4af6-a98b-7bb4169c8b8d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ed807edb-f6bb-4dcf-b293-72d5e4262157"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f373061d-5a04-4725-9140-243236afd26a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fc66e5ef-d1e8-4f7d-a845-903f93a90387"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fcffdaad-83bb-4ba1-83e9-90caeecf147d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fd4bd871-01e0-46fc-bbbd-8b0311659f57"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ff9763ee-b247-48a3-84bd-7099a54a2074"));
        }
    }
}
