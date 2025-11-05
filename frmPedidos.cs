using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmPedidos : Form
    {
        private readonly ConexionBD _conexion;
        private readonly PedidosDAO _pedidosDAO;

        public frmPedidos()
        {
            InitializeComponent();

            _conexion = new ConexionBD();
            _pedidosDAO = new PedidosDAO(_conexion);

            ConfigurarDataGridView();

            CargarClientes();

            CargarPedidos();
        }

        private void ConfigurarDataGridView()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.Columns.Clear();

            // ID Pedido (oculto)
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdPedido",
                DataPropertyName = "IdPedido",
                HeaderText = "IdPedido",
                Visible = false
            });

            // Cliente
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cliente",
                HeaderText = "Cliente",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Total
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                HeaderText = "Total"
            });

            // Fecha
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha"
            });

            // Botón para detalles
            var colDetalles = new DataGridViewButtonColumn
            {
                HeaderText = "Detalles",
                Text = "+",
                UseColumnTextForButtonValue = true,
                Width = 60
            };
            dgvPedidos.Columns.Add(colDetalles);

            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.ReadOnly = true;
        }

        private void CargarClientes()
        {
            cboCliente.Items.Clear();
            var clientes = _pedidosDAO.ObtenerClientes();

            cboCliente.Items.Add(new ComboboxItem { Text = "Todos", Value = null });

            foreach (var c in clientes)
                cboCliente.Items.Add(c);

            cboCliente.SelectedIndex = 0; 
        }

        private void CargarPedidos()
        {
            dgvPedidos.Rows.Clear();

            string idCliente = null;
            if (cboCliente.SelectedItem is ComboboxItem selected && selected.Value != null)
                idCliente = selected.Value.ToString();

            var pedidos = _pedidosDAO.ObtenerPedidos(idCliente);

            foreach (var p in pedidos)
            {
                dgvPedidos.Rows.Add(p.IdPedido, p.Cliente, p.Total, p.Fecha);
            }
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPedidos();
        }

        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPedidos.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                int idPedido = Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells["IdPedido"].Value);

                // Abrir ventana de detalles
                frmDetallePedido detalle = new frmDetallePedido(_pedidosDAO, idPedido);
                detalle.ShowDialog();
            }
        }
    }
}
