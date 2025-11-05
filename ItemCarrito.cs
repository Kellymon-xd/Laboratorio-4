namespace Laboratorio_4.Modelos
{
    public class ItemCarrito
    {
        public Medicamento Medicamento { get; set; }
        public int Cantidad { get; set; }
        public decimal Total => Medicamento.PrecioUnitario * Cantidad;
    }
}
