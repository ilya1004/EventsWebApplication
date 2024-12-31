using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventsWebApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Category_Name", "Category_NormalizedName", "Description", "EventDateTime", "Image", "ParticipantsMaxCount", "Title", "Place_Name", "Place_NormalizedName" },
                values: new object[,]
                {
                    { 1, "Technology", "TECHNOLOGY", "Join us for a day of insightful talks and networking with industry leaders.", new DateTime(2025, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, 1000, "Tech Conference 2025", "Convention Center", "CONVENTION CENTER" },
                    { 2, "Art", "ART", "A beginner-friendly workshop to learn the fundamentals of painting.", new DateTime(2025, 6, 15, 14, 0, 0, 0, DateTimeKind.Utc), null, 850, "Art Workshop: Painting Basics", "Art Studio", "ART STUDIO" },
                    { 3, "Business", "BUSINESS", "Local startups pitch their ideas to a panel of investors.", new DateTime(2025, 7, 20, 18, 30, 0, 0, DateTimeKind.Utc), null, 320, "Startup Pitch Night", "Startup Hub", "STARTUP HUB" },
                    { 4, "Sports", "SPORTS", "Get ready for the upcoming marathon with expert-led training.", new DateTime(2025, 8, 10, 7, 0, 0, 0, DateTimeKind.Utc), null, 500, "Marathon Training Session", "City Park", "CITY PARK" },
                    { 5, "Science", "SCIENCE", "An evening of stargazing and learning about the cosmos.", new DateTime(2025, 10, 18, 20, 0, 0, 0, DateTimeKind.Utc), null, 10, "Astronomy Night", "Observatory", "OBSERVATORY" }
                });

            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "Id", "Email", "EventId", "EventRegistrationDate", "Person_BirthdayDate", "Person_Name", "Person_Surname" },
                values: new object[,]
                {
                    { 1, "ilya@gmail.com", 1, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2004, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Ilya", "Rabets" },
                    { 2, "ilya@gmail.com", 2, new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2004, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Ilya", "Rabets" },
                    { 3, "ilya@gmail.com", 3, new DateTime(2024, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2004, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Ilya", "Rabets" },
                    { 4, "ilya@gmail.com", 4, new DateTime(2024, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2004, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Ilya", "Rabets" },
                    { 5, "ilya@gmail.com", 5, new DateTime(2024, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2004, 9, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Ilya", "Rabets" },
                    { 6, "anna@gmail.com", 1, new DateTime(2024, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1995, 4, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Anna", "Petrova" },
                    { 7, "anna@gmail.com", 3, new DateTime(2024, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1995, 4, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Anna", "Petrova" },
                    { 8, "dmitry@gmail.com", 2, new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1988, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Dmitry", "Ivanov" },
                    { 9, "dmitry@gmail.com", 4, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1988, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Dmitry", "Ivanov" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
