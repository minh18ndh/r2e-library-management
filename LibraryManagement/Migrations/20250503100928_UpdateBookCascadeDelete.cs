﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_BookBorrowingRequestDetails_Books_BookId",
                table: "BookBorrowingRequestDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
