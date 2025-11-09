using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
using System.Drawing;
using System.IO;
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

            // Configuración del FlowLayoutPanel
            ConfigurarFlowLayout();

            // Cargar items del carrito
            CargarCarrito();
        }

        private void ConfigurarFlowLayout()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Padding = new Padding(10);
        }

        private void CargarCarrito()
        {
            flowLayoutPanel1.Controls.Clear();

            var items = CarritoManager.Instancia.Items;

            foreach (var item in items)
            {
                flowLayoutPanel1.Controls.Add(CrearTarjetaCarrito(item));
            }

            // Panel de resumen (abajo)
            flowLayoutPanel1.Controls.Add(CrearPanelResumen());
        }

        private Panel CrearTarjetaCarrito(ItemCarrito item)
        {
            Panel panel = new Panel
            {
                Width = flowLayoutPanel1.ClientSize.Width - 40,
                Height = 110,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(8),
                Padding = new Padding(10)
            };

            int centroY = panel.Height / 2;

            // Imagen del medicamento
            PictureBox pb = new PictureBox
            {
                Width = 80,
                Height = 80,
                SizeMode = PictureBoxSizeMode.Zoom,
                Left = 10,
                Top = centroY - 40
            };
            string rutaImg = Path.Combine(Application.StartupPath, @"..\..\img\", item.Medicamento.Imagen ?? "");
            if (File.Exists(rutaImg))
            {
                using (var tempImg = Image.FromFile(rutaImg))
                    pb.Image = new Bitmap(tempImg);
            }

            // Nombre
            Label lblNombre = new Label
            {
                Text = item.Medicamento.Nombre,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Left = 110,
                Top = centroY - 25,
                AutoSize = true
            };

            // Precio unitario
            Label lblPrecio = new Label
            {
                Text = $"Precio: {item.Medicamento.PrecioUnitario:C2}",
                Font = new Font("Segoe UI", 10),
                Left = 110,
                Top = centroY + 5,
                AutoSize = true
            };

            // Botón eliminar
            Button btnEliminar = new Button
            {
                Text = "🗑️",
                Font = new Font("Segoe UI", 10),
                Width = 40,
                Height = 35,
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Top = centroY - (btnEliminar.Height / 2);
            btnEliminar.Click += (s, e) =>
            {
                CarritoManager.Instancia.Eliminar(item.Medicamento.IdMedicamento);
                CargarCarrito();
            };

            // Botón -
            Button btnMenos = new Button
            {
                Text = "-",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = 35,
                Height = 35,
                BackColor = Color.Gainsboro,
                FlatStyle = FlatStyle.Flat
            };
            btnMenos.FlatAppearance.BorderSize = 0;
            btnMenos.Top = centroY - (btnMenos.Height / 2);
            btnMenos.Click += (s, e) =>
            {
                if (item.Cantidad > 1)
                {
                    CarritoManager.Instancia.ModificarCantidad(item.Medicamento.IdMedicamento, item.Cantidad - 1);
                    CargarCarrito();
                }
            };

            // Cantidad
            Label lblCantidad = new Label
            {
                Text = item.Cantidad.ToString(),
                Font = new Font("Segoe UI", 11),
                Width = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Top = centroY - 15
            };

            // Botón +
            Button btnMas = new Button
            {
                Text = "+",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = 35,
                Height = 35,
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            btnMas.FlatAppearance.BorderSize = 0;
            btnMas.Top = centroY - (btnMas.Height / 2);
            btnMas.Click += (s, e) =>
            {
                if (item.Cantidad < item.Medicamento.CantidadDisponible)
                {
                    CarritoManager.Instancia.ModificarCantidad(item.Medicamento.IdMedicamento, item.Cantidad + 1);
                    CargarCarrito();
                }
            };

            // Subtotal
            Label lblSubtotal = new Label
            {
                Text = $"{item.Total:C2}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Width = 100,
                TextAlign = ContentAlignment.MiddleRight,
                Top = centroY - 15
            };

            // Agregar controles
            panel.Controls.Add(pb);
            panel.Controls.Add(lblNombre);
            panel.Controls.Add(lblPrecio);
            panel.Controls.Add(btnEliminar);
            panel.Controls.Add(btnMenos);
            panel.Controls.Add(lblCantidad);
            panel.Controls.Add(btnMas);
            panel.Controls.Add(lblSubtotal);

            // Ajuste dinámico al redimensionar
            panel.Resize += (s, e) =>
            {
                int baseRight = panel.Width - 10;
                btnEliminar.Left = baseRight - 300;
                btnMenos.Left = baseRight - 250;
                lblCantidad.Left = baseRight - 210;
                btnMas.Left = baseRight - 170;
                lblSubtotal.Left = baseRight - 90;

                int newCentroY = panel.Height / 2;
                pb.Top = newCentroY - 40;
                lblNombre.Top = newCentroY - 25;
                lblPrecio.Top = newCentroY + 5;
                btnEliminar.Top = newCentroY - (btnEliminar.Height / 2);
                btnMenos.Top = newCentroY - (btnMenos.Height / 2);
                lblCantidad.Top = newCentroY - 15;
                btnMas.Top = newCentroY - (btnMas.Height / 2);
                lblSubtotal.Top = newCentroY - 15;
            };

            return panel;
        }

        private Panel CrearPanelResumen()
        {
            Panel resumen = new Panel
            {
                Height = 80,
                Width = flowLayoutPanel1.ClientSize.Width - 40,
                Margin = new Padding(10),
                BackColor = Color.White,
                Dock = DockStyle.Top
            };

            decimal total = CarritoManager.Instancia.Items.Sum(i => i.Total);

            TableLayoutPanel tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };

            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            Label lblTotal = new Label
            {
                Text = "Total:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Anchor = AnchorStyles.Left,
                AutoSize = true
            };

            Label lblNum = new Label
            {
                Text = $"{total:C2}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.ForestGreen,
                Anchor = AnchorStyles.Left,
                AutoSize = true
            };

            Button btnFinalizar = new Button
            {
                Text = "Finalizar pedido",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Width = 180,
                Anchor = AnchorStyles.Right
            };
            btnFinalizar.FlatAppearance.BorderSize = 0;
            btnFinalizar.Click += BtnFinalizar_Click;

            tabla.Controls.Add(lblTotal, 0, 0);
            tabla.Controls.Add(lblNum, 1, 0);
            tabla.Controls.Add(btnFinalizar, 2, 0);

            resumen.Controls.Add(tabla);

            return resumen;
        }

        private void BtnFinalizar_Click(object sender, EventArgs e)
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

                    CarritoManager.Instancia.Vaciar();
                    CargarCarrito();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al registrar pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
