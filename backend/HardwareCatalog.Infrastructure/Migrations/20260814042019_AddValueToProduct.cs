using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValueToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 1);

            // Backfill the numeric magnitude of each product's existing UnitOfMeasure.
            migrationBuilder.Sql(@"
                UPDATE [Products] SET [Value] = 8 WHERE [Name] = N'Kingston 8 GB DDR5';
                UPDATE [Products] SET [Value] = 16 WHERE [Name] = N'Kingston 16 GB DDR5';
                UPDATE [Products] SET [Value] = 32 WHERE [Name] = N'Kingston 32 GB DDR5';
                UPDATE [Products] SET [Value] = 512 WHERE [Name] = N'Kingston 512 MB';
                UPDATE [Products] SET [Value] = 1 WHERE [Name] = N'Western Digital 1TB SSD';
                UPDATE [Products] SET [Value] = 2 WHERE [Name] = N'Western Digital 2TB SSD';
                UPDATE [Products] SET [Value] = 2 WHERE [Name] = N'Seagate 2TB HDD Barracuda';
                UPDATE [Products] SET [Value] = 3 WHERE [Name] = N'Seagate 3TB HDD Barracuda';
                UPDATE [Products] SET [Value] = 4 WHERE [Name] = N'Seagate 4TB HDD Barracuda';
                UPDATE [Products] SET [Value] = 512 WHERE [Name] = N'Seagate 512GB SDD';
                UPDATE [Products] SET [Value] = 750 WHERE [Name] = N'Seagate 750GB SDD';
                UPDATE [Products] SET [Value] = 256 WHERE [Name] = N'Seagate 256GB SDD';
                UPDATE [Products] SET [Value] = 500 WHERE [Name] = N'Corsair 500W PSU';
                UPDATE [Products] SET [Value] = 508 WHERE [Name] = N'Corsair 508 W PSU';
                UPDATE [Products] SET [Value] = 1000 WHERE [Name] = N'MSI 1000 W PSU';
                UPDATE [Products] SET [Value] = 450 WHERE [Name] = N'MSI 450 W PSU';
                UPDATE [Products] SET [Value] = 750 WHERE [Name] = N'MSI 750 W PSU';
                UPDATE [Products] SET [Value] = 1 WHERE [UnitOfMeasure] = 3;
            ");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Value_Positive",
                table: "Products",
                sql: "[Value] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Value_Positive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Products");
        }
    }
}
