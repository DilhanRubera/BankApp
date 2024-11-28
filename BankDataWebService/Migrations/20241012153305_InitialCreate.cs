using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankDataWebService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    ProfilePicdBytes = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdminUsername = table.Column<string>(type: "TEXT", nullable: true),
                    Action = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AffectedResource = table.Column<string>(type: "TEXT", nullable: true),
                    Details = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNo = table.Column<int>(type: "INTEGER", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    ProfilePicdBytes = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountName = table.Column<string>(type: "TEXT", nullable: true),
                    AccountBalance = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountHolderId = table.Column<int>(type: "INTEGER", nullable: false),
                    isActivated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionDescription = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderName = table.Column<string>(type: "TEXT", nullable: true),
                    SenderAccountNo = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiverName = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiverAccountNo = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiverId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Email", "Name", "Password", "PhoneNo", "ProfilePicdBytes" },
                values: new object[,]
                {
                    { 1, null, "jane.miller64@example.com", "Jane Miller", "123", 253275709, null },
                    { 2, null, "john.brown9@example.com", "John Brown", "123", 537849027, null },
                    { 3, null, "jane.jones45@example.com", "Jane Jones", "123", 273990287, null },
                    { 4, null, "michael.smith50@example.com", "Michael Smith", "123", 493115021, null },
                    { 5, null, "john.brown72@example.com", "John Brown", "123", 149130245, null },
                    { 6, null, "john.miller89@example.com", "John Miller", "123", 290765618, null }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "AccountBalance", "AccountHolderId", "AccountName", "AccountNumber", "isActivated" },
                values: new object[,]
                {
                    { 11, 22117, 1, "Checkings", 770065, true },
                    { 12, 65118, 1, "Deposits", 475083, true },
                    { 13, 79983, 1, "Savings", 880816, true },
                    { 21, 96946, 2, "Money Market ", 309277, true },
                    { 22, 44435, 2, "Money Market ", 751640, true },
                    { 23, 29259, 2, "Checkings", 733924, true },
                    { 31, 47306, 3, "Checkings", 897546, true },
                    { 32, 81590, 3, "Money Market ", 338677, true },
                    { 33, 93060, 3, "Deposits", 354018, true },
                    { 41, 79693, 4, "Money Market ", 837691, true },
                    { 42, 56154, 4, "Checkings", 210342, true },
                    { 43, 60854, 4, "Deposits", 348418, true },
                    { 51, 43148, 5, "Deposits", 470073, true },
                    { 52, 96130, 5, "Savings", 596000, true },
                    { 53, 1924, 5, "Checkings", 949319, true },
                    { 61, 42475, 6, "Checkings", 571118, true },
                    { 62, 74905, 6, "Deposits", 339286, true },
                    { 63, 10159, 6, "Money Market ", 786190, true }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "ReceiverAccountNo", "ReceiverId", "ReceiverName", "SenderAccountNo", "SenderId", "SenderName", "TransactionAmount", "TransactionDate", "TransactionDescription" },
                values: new object[,]
                {
                    { 111, 770065, 11, null, 770065, 11, null, 4174, new DateTime(2024, 7, 31, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7465), "Payment from 770065 to 770065" },
                    { 112, 770065, 11, null, 770065, 11, null, 3684, new DateTime(2024, 7, 23, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7484), "Payment from 770065 to 770065" },
                    { 113, 770065, 11, null, 770065, 11, null, 4190, new DateTime(2024, 8, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7486), "Payment from 770065 to 770065" },
                    { 114, 770065, 11, null, 770065, 11, null, 2040, new DateTime(2024, 9, 23, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7544), "Payment from 770065 to 770065" },
                    { 115, 770065, 11, null, 770065, 11, null, 2785, new DateTime(2024, 8, 16, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7547), "Payment from 770065 to 770065" },
                    { 121, 475083, 12, null, 475083, 12, null, 4341, new DateTime(2024, 7, 25, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7557), "Payment from 475083 to 475083" },
                    { 122, 770065, 11, null, 475083, 12, null, 624, new DateTime(2024, 7, 16, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7559), "Payment from 475083 to 770065" },
                    { 123, 770065, 11, null, 475083, 12, null, 3160, new DateTime(2024, 7, 14, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7561), "Payment from 475083 to 770065" },
                    { 124, 770065, 11, null, 475083, 12, null, 455, new DateTime(2024, 7, 14, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7562), "Payment from 475083 to 770065" },
                    { 125, 770065, 11, null, 475083, 12, null, 3689, new DateTime(2024, 8, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7565), "Payment from 475083 to 770065" },
                    { 131, 880816, 13, null, 880816, 13, null, 4047, new DateTime(2024, 9, 4, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7573), "Payment from 880816 to 880816" },
                    { 132, 880816, 13, null, 880816, 13, null, 1059, new DateTime(2024, 9, 1, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7574), "Payment from 880816 to 880816" },
                    { 133, 880816, 13, null, 880816, 13, null, 4273, new DateTime(2024, 9, 18, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7576), "Payment from 880816 to 880816" },
                    { 134, 880816, 13, null, 880816, 13, null, 3314, new DateTime(2024, 8, 24, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7579), "Payment from 880816 to 880816" },
                    { 135, 770065, 11, null, 880816, 13, null, 3492, new DateTime(2024, 10, 10, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7581), "Payment from 880816 to 770065" },
                    { 211, 880816, 13, null, 309277, 21, null, 1297, new DateTime(2024, 7, 9, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7590), "Payment from 309277 to 880816" },
                    { 212, 880816, 13, null, 309277, 21, null, 1096, new DateTime(2024, 9, 11, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7592), "Payment from 309277 to 880816" },
                    { 213, 309277, 21, null, 309277, 21, null, 3193, new DateTime(2024, 9, 14, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7595), "Payment from 309277 to 309277" },
                    { 214, 770065, 11, null, 309277, 21, null, 4031, new DateTime(2024, 7, 26, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7596), "Payment from 309277 to 770065" },
                    { 215, 770065, 11, null, 309277, 21, null, 2028, new DateTime(2024, 9, 23, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7599), "Payment from 309277 to 770065" },
                    { 221, 309277, 21, null, 751640, 22, null, 1412, new DateTime(2024, 7, 19, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7604), "Payment from 751640 to 309277" },
                    { 222, 751640, 22, null, 751640, 22, null, 4256, new DateTime(2024, 8, 28, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7605), "Payment from 751640 to 751640" },
                    { 223, 751640, 22, null, 751640, 22, null, 3746, new DateTime(2024, 9, 19, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7607), "Payment from 751640 to 751640" },
                    { 224, 770065, 11, null, 751640, 22, null, 576, new DateTime(2024, 8, 1, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7609), "Payment from 751640 to 770065" },
                    { 225, 880816, 13, null, 751640, 22, null, 4535, new DateTime(2024, 8, 1, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7611), "Payment from 751640 to 880816" },
                    { 231, 880816, 13, null, 733924, 23, null, 759, new DateTime(2024, 8, 28, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7615), "Payment from 733924 to 880816" },
                    { 232, 880816, 13, null, 733924, 23, null, 3205, new DateTime(2024, 8, 29, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7617), "Payment from 733924 to 880816" },
                    { 233, 770065, 11, null, 733924, 23, null, 1445, new DateTime(2024, 7, 29, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7618), "Payment from 733924 to 770065" },
                    { 234, 880816, 13, null, 733924, 23, null, 588, new DateTime(2024, 7, 30, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7620), "Payment from 733924 to 880816" },
                    { 235, 733924, 23, null, 733924, 23, null, 4010, new DateTime(2024, 9, 7, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7622), "Payment from 733924 to 733924" },
                    { 311, 770065, 11, null, 897546, 31, null, 1978, new DateTime(2024, 8, 2, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7630), "Payment from 897546 to 770065" },
                    { 312, 475083, 12, null, 897546, 31, null, 2960, new DateTime(2024, 8, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7631), "Payment from 897546 to 475083" },
                    { 313, 475083, 12, null, 897546, 31, null, 159, new DateTime(2024, 9, 25, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7633), "Payment from 897546 to 475083" },
                    { 314, 309277, 21, null, 897546, 31, null, 1402, new DateTime(2024, 8, 28, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7635), "Payment from 897546 to 309277" },
                    { 315, 880816, 13, null, 897546, 31, null, 2019, new DateTime(2024, 9, 7, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7637), "Payment from 897546 to 880816" },
                    { 321, 897546, 31, null, 338677, 32, null, 64, new DateTime(2024, 7, 13, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7680), "Payment from 338677 to 897546" },
                    { 322, 309277, 21, null, 338677, 32, null, 2545, new DateTime(2024, 7, 26, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7682), "Payment from 338677 to 309277" },
                    { 323, 309277, 21, null, 338677, 32, null, 4594, new DateTime(2024, 9, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7684), "Payment from 338677 to 309277" },
                    { 324, 751640, 22, null, 338677, 32, null, 533, new DateTime(2024, 10, 9, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7686), "Payment from 338677 to 751640" },
                    { 325, 770065, 11, null, 338677, 32, null, 703, new DateTime(2024, 8, 22, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7687), "Payment from 338677 to 770065" },
                    { 331, 733924, 23, null, 354018, 33, null, 2223, new DateTime(2024, 9, 9, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7692), "Payment from 354018 to 733924" },
                    { 332, 733924, 23, null, 354018, 33, null, 3284, new DateTime(2024, 9, 1, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7694), "Payment from 354018 to 733924" },
                    { 333, 309277, 21, null, 354018, 33, null, 4397, new DateTime(2024, 7, 14, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7696), "Payment from 354018 to 309277" },
                    { 334, 770065, 11, null, 354018, 33, null, 2101, new DateTime(2024, 8, 2, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7698), "Payment from 354018 to 770065" },
                    { 335, 733924, 23, null, 354018, 33, null, 1405, new DateTime(2024, 7, 5, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7700), "Payment from 354018 to 733924" },
                    { 411, 733924, 23, null, 837691, 41, null, 4746, new DateTime(2024, 7, 28, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7708), "Payment from 837691 to 733924" },
                    { 412, 770065, 11, null, 837691, 41, null, 1681, new DateTime(2024, 10, 9, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7709), "Payment from 837691 to 770065" },
                    { 413, 309277, 21, null, 837691, 41, null, 679, new DateTime(2024, 8, 7, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7711), "Payment from 837691 to 309277" },
                    { 414, 354018, 33, null, 837691, 41, null, 1275, new DateTime(2024, 9, 16, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7713), "Payment from 837691 to 354018" },
                    { 415, 338677, 32, null, 837691, 41, null, 3390, new DateTime(2024, 9, 25, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7715), "Payment from 837691 to 338677" },
                    { 421, 880816, 13, null, 210342, 42, null, 3245, new DateTime(2024, 9, 16, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7720), "Payment from 210342 to 880816" },
                    { 422, 837691, 41, null, 210342, 42, null, 1168, new DateTime(2024, 8, 16, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7721), "Payment from 210342 to 837691" },
                    { 423, 354018, 33, null, 210342, 42, null, 807, new DateTime(2024, 9, 29, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7723), "Payment from 210342 to 354018" },
                    { 424, 338677, 32, null, 210342, 42, null, 4234, new DateTime(2024, 7, 27, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7725), "Payment from 210342 to 338677" },
                    { 425, 354018, 33, null, 210342, 42, null, 3631, new DateTime(2024, 8, 7, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7727), "Payment from 210342 to 354018" },
                    { 431, 770065, 11, null, 348418, 43, null, 2219, new DateTime(2024, 7, 29, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7732), "Payment from 348418 to 770065" },
                    { 432, 733924, 23, null, 348418, 43, null, 3529, new DateTime(2024, 8, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7734), "Payment from 348418 to 733924" },
                    { 433, 897546, 31, null, 348418, 43, null, 2838, new DateTime(2024, 9, 14, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7735), "Payment from 348418 to 897546" },
                    { 434, 897546, 31, null, 348418, 43, null, 1913, new DateTime(2024, 8, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7737), "Payment from 348418 to 897546" },
                    { 435, 733924, 23, null, 348418, 43, null, 1143, new DateTime(2024, 8, 18, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7739), "Payment from 348418 to 733924" },
                    { 511, 354018, 33, null, 470073, 51, null, 1977, new DateTime(2024, 7, 23, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7748), "Payment from 470073 to 354018" },
                    { 512, 470073, 51, null, 470073, 51, null, 3188, new DateTime(2024, 8, 21, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7750), "Payment from 470073 to 470073" },
                    { 513, 733924, 23, null, 470073, 51, null, 1303, new DateTime(2024, 8, 28, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7751), "Payment from 470073 to 733924" },
                    { 514, 309277, 21, null, 470073, 51, null, 4902, new DateTime(2024, 7, 12, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7753), "Payment from 470073 to 309277" },
                    { 515, 733924, 23, null, 470073, 51, null, 448, new DateTime(2024, 9, 5, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7755), "Payment from 470073 to 733924" },
                    { 521, 475083, 12, null, 596000, 52, null, 2821, new DateTime(2024, 8, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7761), "Payment from 596000 to 475083" },
                    { 522, 470073, 51, null, 596000, 52, null, 4003, new DateTime(2024, 8, 13, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7763), "Payment from 596000 to 470073" },
                    { 523, 470073, 51, null, 596000, 52, null, 707, new DateTime(2024, 7, 25, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7804), "Payment from 596000 to 470073" },
                    { 524, 751640, 22, null, 596000, 52, null, 57, new DateTime(2024, 7, 31, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7806), "Payment from 596000 to 751640" },
                    { 525, 837691, 41, null, 596000, 52, null, 3586, new DateTime(2024, 7, 29, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7808), "Payment from 596000 to 837691" },
                    { 531, 470073, 51, null, 949319, 53, null, 4319, new DateTime(2024, 7, 7, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7813), "Payment from 949319 to 470073" },
                    { 532, 354018, 33, null, 949319, 53, null, 1825, new DateTime(2024, 9, 8, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7815), "Payment from 949319 to 354018" },
                    { 533, 733924, 23, null, 949319, 53, null, 4933, new DateTime(2024, 7, 29, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7817), "Payment from 949319 to 733924" },
                    { 534, 210342, 42, null, 949319, 53, null, 595, new DateTime(2024, 8, 11, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7819), "Payment from 949319 to 210342" },
                    { 535, 837691, 41, null, 949319, 53, null, 2396, new DateTime(2024, 8, 25, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7821), "Payment from 949319 to 837691" },
                    { 611, 470073, 51, null, 571118, 61, null, 156, new DateTime(2024, 8, 18, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7829), "Payment from 571118 to 470073" },
                    { 612, 880816, 13, null, 571118, 61, null, 1154, new DateTime(2024, 7, 30, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7831), "Payment from 571118 to 880816" },
                    { 613, 348418, 43, null, 571118, 61, null, 4319, new DateTime(2024, 8, 22, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7833), "Payment from 571118 to 348418" },
                    { 614, 770065, 11, null, 571118, 61, null, 2677, new DateTime(2024, 9, 17, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7835), "Payment from 571118 to 770065" },
                    { 615, 880816, 13, null, 571118, 61, null, 1721, new DateTime(2024, 9, 16, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7837), "Payment from 571118 to 880816" },
                    { 621, 837691, 41, null, 339286, 62, null, 1901, new DateTime(2024, 9, 7, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7843), "Payment from 339286 to 837691" },
                    { 622, 470073, 51, null, 339286, 62, null, 2767, new DateTime(2024, 8, 19, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7845), "Payment from 339286 to 470073" },
                    { 623, 770065, 11, null, 339286, 62, null, 4954, new DateTime(2024, 9, 25, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7847), "Payment from 339286 to 770065" },
                    { 624, 309277, 21, null, 339286, 62, null, 583, new DateTime(2024, 10, 10, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7849), "Payment from 339286 to 309277" },
                    { 625, 338677, 32, null, 339286, 62, null, 360, new DateTime(2024, 7, 23, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7851), "Payment from 339286 to 338677" },
                    { 631, 210342, 42, null, 786190, 63, null, 2381, new DateTime(2024, 7, 7, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7856), "Payment from 786190 to 210342" },
                    { 632, 309277, 21, null, 786190, 63, null, 1628, new DateTime(2024, 7, 23, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7858), "Payment from 786190 to 309277" },
                    { 633, 751640, 22, null, 786190, 63, null, 929, new DateTime(2024, 7, 21, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7860), "Payment from 786190 to 751640" },
                    { 634, 786190, 63, null, 786190, 63, null, 4472, new DateTime(2024, 8, 21, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7862), "Payment from 786190 to 786190" },
                    { 635, 571118, 61, null, 786190, 63, null, 3034, new DateTime(2024, 7, 23, 21, 3, 5, 127, DateTimeKind.Local).AddTicks(7864), "Payment from 786190 to 571118" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountHolderId",
                table: "Accounts",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderId",
                table: "Transactions",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
