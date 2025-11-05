using Laboratorio_4.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace Laboratorio_4.Servicios
{
    public class CarritoManager
    {
        private static CarritoManager _instancia;
        public static CarritoManager Instancia => _instancia ?? (_instancia = new CarritoManager());

        private readonly List<ItemCarrito> _items = new List<ItemCarrito>();
        public List<ItemCarrito> Items => _items;

        public void Agregar(Medicamento medicamento)
        {
            var existente = _items.FirstOrDefault(i => i.Medicamento.IdMedicamento == medicamento.IdMedicamento);
            if (existente != null)
                existente.Cantidad++;
            else
                _items.Add(new ItemCarrito { Medicamento = medicamento, Cantidad = 1 });
        }

        public void Eliminar(string idMedicamento)
        {
            var item = _items.FirstOrDefault(i => i.Medicamento.IdMedicamento == idMedicamento);
            if (item != null)
                _items.Remove(item);
        }

        public void ModificarCantidad(string idMedicamento, int nuevaCantidad)
        {
            var item = _items.FirstOrDefault(i => i.Medicamento.IdMedicamento == idMedicamento);
            if (item != null)
                item.Cantidad = nuevaCantidad;
        }

        public void Vaciar() => _items.Clear();
    }
}
