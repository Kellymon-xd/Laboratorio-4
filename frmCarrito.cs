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

        private Color colorPrimario = Color.FromArgb(46, 125, 50);
        private Color colorFondo = Color.FromArgb(245, 248, 250);
        private Color colorTexto = Color.FromArgb(33, 33, 33);
        private Color colorBotonCantidad = Color.FromArgb(220, 220, 220);
        private Color colorBotonEliminar = Color.FromArgb(220, 53, 69);

        public frmCarrito(string idCliente)
        {
            InitializeComponent();
            this.idCliente = idCliente;
            this.BackColor = colorFondo;

            ConfigurarFlowLayout();

            CargarCarrito();
        }

        private void ConfigurarFlowLayout()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Padding = new Padding(10);
            flowLayoutPanel1.BackColor = colorFondo;
        }

        private void CargarCarrito()
        {
            flowLayoutPanel1.Controls.Clear();

            var items = CarritoManager.Instancia.Items;

            if (items.Count == 0)
            {
                Label lblVacio = new Label
                {
                    Text = "🛒 El carrito está vacío.",
                    Font = new Font("Segoe UI", 14, FontStyle.Italic),
                    ForeColor = colorPrimario,
                    AutoSize = true,
                    Anchor = AnchorStyles.None,
                    Margin = new Padding(50)
                };
                flowLayoutPanel1.Controls.Add(lblVacio);
                lblVacio.Left = (flowLayoutPanel1.Width - lblVacio.Width) / 2;
            }
            else
            {
                foreach (var item in items)
                {
                    flowLayoutPanel1.Controls.Add(CrearTarjetaCarrito(item));
                }

                flowLayoutPanel1.Controls.Add(CrearPanelResumen());
            }
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
                Padding = new Padding(10),
                Cursor = Cursors.Default
            };
            panel.Tag = item.Medicamento.IdMedicamento;

            panel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, panel.ClientRectangle, Color.LightGray, ButtonBorderStyle.Solid);
            };

            int centroY = panel.Height / 2;

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
                try
                {
                    using (var fs = new FileStream(rutaImg, FileMode.Open, FileAccess.Read))
                    {
                        pb.Image = Image.FromStream(fs);
                    }
                }
                catch (Exception)
                {
                }
            }

            Label lblNombre = new Label
            {
                Text = item.Medicamento.Nombre,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = colorTexto,
                Left = 110,
                Top = centroY - 25,
                AutoSize = true
            };

            Label lblPrecio = new Label
            {
                Text = $"Precio: {item.Medicamento.PrecioUnitario:C2}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Left = 110,
                Top = centroY + 5,
                AutoSize = true
            };

            Button btnEliminar = new Button
            {
                Text = "🗑️ Eliminar",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Width = 90,
                Height = 35,
                BackColor = colorBotonEliminar,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Top = centroY - (btnEliminar.Height / 2);
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.Click += (s, e) =>
            {
                CarritoManager.Instancia.Eliminar(item.Medicamento.IdMedicamento);
                CargarCarrito();
            };

            Button btnMenos = new Button
            {
                Text = "—",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Width = 35,
                Height = 35,
                BackColor = colorBotonCantidad,
                FlatStyle = FlatStyle.Flat,
                Enabled = item.Cantidad > 1
            };
            btnMenos.FlatAppearance.BorderSize = 0;
            btnMenos.Top = centroY - (btnMenos.Height / 2);
            btnMenos.Cursor = Cursors.Hand;
            btnMenos.Click += (s, e) =>
            {
                if (item.Cantidad > 1)
                {
                    CarritoManager.Instancia.ModificarCantidad(item.Medicamento.IdMedicamento, item.Cantidad - 1);
                    CargarCarrito();
                }
            };

            Label lblCantidad = new Label
            {
                Text = item.Cantidad.ToString(),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = 35,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Top = centroY - 15,
                BackColor = Color.LightGray,
                Padding = new Padding(0, 5, 0, 5)
            };
            lblCantidad.Top = centroY - (lblCantidad.Height / 2);


            Button btnMas = new Button
            {
                Text = "+",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = 35,
                Height = 35,
                BackColor = colorBotonCantidad,
                FlatStyle = FlatStyle.Flat,
                Enabled = item.Cantidad < item.Medicamento.CantidadDisponible
            };
            btnMas.FlatAppearance.BorderSize = 0;
            btnMas.Top = centroY - (btnMas.Height / 2);
            btnMas.Cursor = Cursors.Hand;
            btnMas.Click += (s, e) =>
            {
                if (item.Cantidad < item.Medicamento.CantidadDisponible)
                {
                    CarritoManager.Instancia.ModificarCantidad(item.Medicamento.IdMedicamento, item.Cantidad + 1);
                    CargarCarrito();
                }
                else
                {
                    MessageBox.Show("Stock máximo alcanzado.", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            Label lblSubtotal = new Label
            {
                Text = $"{item.Total:C2}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = colorPrimario,
                Width = 120,
                TextAlign = ContentAlignment.MiddleRight,
                Top = centroY - 15
            };

            panel.Controls.Add(pb);
            panel.Controls.Add(lblNombre);
            panel.Controls.Add(lblPrecio);
            panel.Controls.Add(btnEliminar);
            panel.Controls.Add(btnMenos);
            panel.Controls.Add(lblCantidad);
            panel.Controls.Add(btnMas);
            panel.Controls.Add(lblSubtotal);

            panel.Resize += (s, e) =>
            {
                int baseRight = panel.Width - 10;

                lblSubtotal.Left = baseRight - lblSubtotal.Width;

                btnMas.Left = lblSubtotal.Left - btnMas.Width - 15;

                lblCantidad.Left = btnMas.Left - lblCantidad.Width - 10;

                btnMenos.Left = lblCantidad.Left - btnMenos.Width - 10;

                btnEliminar.Left = btnMenos.Left - btnEliminar.Width - 25;

                int newCentroY = panel.Height / 2;
                pb.Top = newCentroY - 40;
                lblNombre.Top = newCentroY - 25;
                lblPrecio.Top = newCentroY + 5;

                btnEliminar.Top = newCentroY - (btnEliminar.Height / 2);
                btnMenos.Top = newCentroY - (btnMenos.Height / 2);
                lblCantidad.Top = newCentroY - (lblCantidad.Height / 2);
                btnMas.Top = newCentroY - (btnMas.Height / 2);
                lblSubtotal.Top = newCentroY - 15;
            };
            panel.PerformLayout();

            return panel;
        }

        private Panel CrearPanelResumen()
        {
            Panel resumen = new Panel
            {
                Height = 48,
                Width = flowLayoutPanel1.ClientSize.Width - 40,
                Margin = new Padding(10),
                BackColor = colorFondo,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Top
            };

            decimal total = CarritoManager.Instancia.Items.Sum(i => i.Total);

            TableLayoutPanel tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(20, 2, 20, 2),
                BackColor = Color.Transparent
            };

            tabla.RowStyles.Clear();
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            Label lblTotal = new Label
            {
                Text = "Total:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = colorTexto,
                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                AutoSize = true
            };

            lblTotal.Margin = new Padding(0, 0, 5, 0);

            Label lblNum = new Label
            {
                Text = $"{total:C2}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = colorPrimario,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                AutoSize = true
            };

            lblNum.Margin = new Padding(0, 0, 10, 0);

            Button btnFinalizar = new Button
            {
                Text = "Finalizar Pedido",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = colorPrimario,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 36,
                Width = 150,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Right
            };
            btnFinalizar.FlatAppearance.BorderSize = 0;
            btnFinalizar.Cursor = Cursors.Hand;
            btnFinalizar.Click += BtnFinalizar_Click;

            Panel panelPusher = new Panel() { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            tabla.Controls.Add(panelPusher, 0, 0);
            tabla.Controls.Add(lblTotal, 1, 0);
            tabla.Controls.Add(lblNum, 2, 0);
            tabla.Controls.Add(btnFinalizar, 3, 0);

            btnFinalizar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;


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