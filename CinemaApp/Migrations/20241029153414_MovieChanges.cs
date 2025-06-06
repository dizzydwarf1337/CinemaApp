﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Migrations
{
    /// <inheritdoc />
    public partial class MovieChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "Duration",
                table: "Movies",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Movies");
        }
    }
}
