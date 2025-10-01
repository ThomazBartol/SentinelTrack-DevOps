using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeCap.Migrations
{
    /// <inheritdoc />
    public partial class MakeSensorDataNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Temperature",
                table: "SC_SensorReadings",
                type: "BINARY_FLOAT",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "BINARY_FLOAT");

            migrationBuilder.AlterColumn<float>(
                name: "Light",
                table: "SC_SensorReadings",
                type: "BINARY_FLOAT",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "BINARY_FLOAT");

            migrationBuilder.AlterColumn<float>(
                name: "Humidity",
                table: "SC_SensorReadings",
                type: "BINARY_FLOAT",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "BINARY_FLOAT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Temperature",
                table: "SC_SensorReadings",
                type: "BINARY_FLOAT",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "BINARY_FLOAT",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Light",
                table: "SC_SensorReadings",
                type: "BINARY_FLOAT",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "BINARY_FLOAT",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Humidity",
                table: "SC_SensorReadings",
                type: "BINARY_FLOAT",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "BINARY_FLOAT",
                oldNullable: true);
        }
    }
}
