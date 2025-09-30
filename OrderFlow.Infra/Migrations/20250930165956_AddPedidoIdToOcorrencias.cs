using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderFlow.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddPedidoIdToOcorrencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PedidoIdPedido",
                table: "Ocorrencias",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ocorrencias_PedidoIdPedido",
                table: "Ocorrencias",
                column: "PedidoIdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Ocorrencias_Pedidos_PedidoIdPedido",
                table: "Ocorrencias",
                column: "PedidoIdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ocorrencias_Pedidos_PedidoIdPedido",
                table: "Ocorrencias");

            migrationBuilder.DropIndex(
                name: "IX_Ocorrencias_PedidoIdPedido",
                table: "Ocorrencias");

            migrationBuilder.DropColumn(
                name: "PedidoIdPedido",
                table: "Ocorrencias");
        }
    }
}
