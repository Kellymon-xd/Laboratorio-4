using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmDetallePedido : Form
    {
        private readonly PedidosDAO _pedidosDAO;
        private readonly int _idPedido;

        public frmDetallePedido(PedidosDAO pedidosDAO, int idPedido)
        {
            InitializeComponent();
            _pedidosDAO = pedidosDAO;
            _idPedido = idPedido;

            ConfigurarDataGridView();
            CargarDetalles();
        }

        private void ConfigurarDataGridView()
        {
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.Columns.Clear();

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Medicamento",
                HeaderText = "Medicamento",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cantidad"
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                HeaderText = "Subtotal"
            });

            dgvDetalle.ReadOnly = true;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void CargarDetalles()
        {
            dgvDetalle.Rows.Clear();

            var detalles = _pedidosDAO.ObtenerDetallePedido(_idPedido);

            foreach (var d in detalles)
            {
                dgvDetalle.Rows.Add(d.Medicamento, d.Cantidad, d.Subtotal);
            }
        }
    }

    // Clase para mapear los detalles del pedido
    public class DetallePedido
    {
        public string Medicamento { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
