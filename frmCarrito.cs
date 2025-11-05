using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmCarrito : Form
    {
        private readonly string idCliente;

        public frmCarrito(string idCliente)
        {
            InitializeComponent();
            this.idCliente = idCliente;
            ConfigurarDataGridView();
            CargarCarrito();
        }

        private void btnPedir_Click(object sender, EventArgs e)
        {
            if (CarritoManager.Instancia.Items.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conexion = new ConexionBD())
            {
                var pedidosDAO = new PedidosDAO(conexion);

                int[] ids = CarritoManager.Instancia.Items
                    .Select(i => int.Parse(i.Medicamento.IdMedicamento))
                    .ToArray();

                int[] cantidades = CarritoManager.Instancia.Items
                    .Select(i => i.Cantidad)
                    .ToArray();

                try
                {
                    string resultado = pedidosDAO.RegistrarPedido(idCliente, ids, cantidades);
                    MessageBox.Show(resultado, "Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiar carrito
                    CarritoManager.Instancia.Vaciar();
                    CargarCarrito();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al registrar pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvCarrito.AutoGenerateColumns = false;
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrito.Columns.Clear();

            // ID oculto
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdMedicamento",
                Name = "IdMedicamento",
                HeaderText = "ID",
                Visible = false
            });

            // Nombre
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                Name = "Nombre",
                HeaderText = "Medicamento",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Precio
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                Name = "PrecioUnitario",
                HeaderText = "Precio",
                Width = 80
            });

            // Botón "-"
            dgvCarrito.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "",
                Name = "btnMenos",
                Text = "-",
                UseColumnTextForButtonValue = true,
                Width = 30
            });

            // Cantidad
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                Name = "Cantidad",
                HeaderText = "Cantidad",
                Width = 50,
                ReadOnly = true
            });

            // Botón "+"
            dgvCarrito.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "",
                Name = "btnMas",
                Text = "+",
                UseColumnTextForButtonValue = true,
                Width = 30
            });

            // Subtotal
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                Name = "Subtotal",
                HeaderText = "Subtotal",
                Width = 80,
                ReadOnly = true
            });

            // Botón eliminar
            dgvCarrito.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "",
                Name = "btnEliminar",
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                Width = 50
            });

            dgvCarrito.CellClick += DgvCarrito_CellClick;
        }

        private void CargarCarrito()
        {
            var items = CarritoManager.Instancia.Items
                .Select(i => new
                {
                    IdMedicamento = i.Medicamento.IdMedicamento,
                    Nombre = i.Medicamento.Nombre,
                    PrecioUnitario = i.Medicamento.PrecioUnitario,
                    Cantidad = i.Cantidad,
                    Subtotal = i.Total,
                    CantidadDisponible = i.Medicamento.CantidadDisponible
                })
                .ToList();

            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = items;

            // Actualizar total en lblNum
            decimal total = CarritoManager.Instancia.Items.Sum(i => i.Total);
            lblNum.Text = total.ToString("C2");
        }


        private void DgvCarrito_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var id = dgvCarrito.Rows[e.RowIndex].Cells["IdMedicamento"].Value.ToString();
            var item = CarritoManager.Instancia.Items.FirstOrDefault(i => i.Medicamento.IdMedicamento == id);
            if (item == null) return;

            // Botón "-"
            if (dgvCarrito.Columns[e.ColumnIndex].Name == "btnMenos")
            {
                if (item.Cantidad > 1)
                {
                    CarritoManager.Instancia.ModificarCantidad(id, item.Cantidad - 1);
                    CargarCarrito();
                }
            }

            // Botón "+"
            else if (dgvCarrito.Columns[e.ColumnIndex].Name == "btnMas")
            {
                if (item.Cantidad < item.Medicamento.CantidadDisponible)
                {
                    CarritoManager.Instancia.ModificarCantidad(id, item.Cantidad + 1);
                    CargarCarrito();
                }
            }

            // Botón eliminar
            else if (dgvCarrito.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                CarritoManager.Instancia.Eliminar(id);
                CargarCarrito();
            }
        }
    }
}
