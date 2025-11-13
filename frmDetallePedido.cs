using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmDetallePedido : Form
    {
        private readonly PedidosDAO _pedidosDAO;
        private readonly int _idPedido;

        private Color colorPrimario = Color.FromArgb(46, 125, 50);
        private Color colorSecundario = Color.FromArgb(27, 94, 32);
        private Color colorFondo = Color.FromArgb(245, 248, 250);
        private Color colorTexto = Color.FromArgb(33, 33, 33);

        private Panel panelHeader;

        public frmDetallePedido(PedidosDAO pedidosDAO, int idPedido)
        {
            InitializeComponent();
            _pedidosDAO = pedidosDAO;
            _idPedido = idPedido;
            PersonalizarFormulario();
            ConfigurarDataGridView();
            CargarDetalles();
        }

        private void PersonalizarFormulario()
        {
            this.Text = $"Detalle del Pedido #{_idPedido}";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = colorFondo;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl != dgvDetalle)
                {
                    ctrl.Visible = false;
                }
            }

            panelHeader = new Panel
            {
                Size = new Size(700, 120),
                Location = new Point(0, 0),
                BackColor = colorPrimario
            };
            panelHeader.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    panelHeader.ClientRectangle,
                    colorPrimario,
                    colorSecundario,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, panelHeader.ClientRectangle);
                }
            };

            Panel iconoDocumento = new Panel
            {
                Size = new Size(60, 60),
                Location = new Point(50, 30),
                BackColor = Color.White
            };
            iconoDocumento.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(Brushes.White, 0, 0, 60, 60);
                using (SolidBrush brushDoc = new SolidBrush(colorPrimario))
                {
                    e.Graphics.FillRectangle(brushDoc, 15, 12, 30, 36);
                    e.Graphics.FillRectangle(Brushes.White, 20, 20, 20, 2);
                    e.Graphics.FillRectangle(Brushes.White, 20, 26, 20, 2);
                    e.Graphics.FillRectangle(Brushes.White, 20, 32, 15, 2);
                }
            };
            panelHeader.Controls.Add(iconoDocumento);

            Label lblTituloPrincipal = new Label
            {
                Text = "Detalle del Pedido",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(130, 35),
                BackColor = Color.Transparent
            };
            panelHeader.Controls.Add(lblTituloPrincipal);

            Label lblSubtitulo = new Label
            {
                Text = $"Pedido #{_idPedido}",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 255, 200),
                AutoSize = true,
                Location = new Point(130, 68),
                BackColor = Color.Transparent
            };
            panelHeader.Controls.Add(lblSubtitulo);

            Panel panelContenido = new Panel
            {
                Size = new Size(640, 380),
                Location = new Point(30, 140),
                BackColor = Color.White
            };
            panelContenido.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 1, 1, panelContenido.Width - 2, panelContenido.Height - 2);
                }
            };

            Label lblProductos = new Label
            {
                Text = "Productos del Pedido",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = colorTexto,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panelContenido.Controls.Add(lblProductos);

            dgvDetalle.Size = new Size(600, 280);
            dgvDetalle.Location = new Point(20, 60);
            dgvDetalle.Visible = true;
            dgvDetalle.BringToFront();
            panelContenido.Controls.Add(dgvDetalle);

            Label lblTotalTexto = new Label
            {
                Text = "Total del Pedido:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = colorTexto,
                AutoSize = true,
                Location = new Point(410, 350)
            };
            panelContenido.Controls.Add(lblTotalTexto);

            Label lblTotalValor = new Label
            {
                Name = "lblTotalValor",
                Text = "$0.00",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = colorPrimario,
                AutoSize = true,
                Location = new Point(550, 348)
            };
            panelContenido.Controls.Add(lblTotalValor);

            this.Controls.Add(panelHeader);
            this.Controls.Add(panelContenido);

            panelHeader.BringToFront();
            panelContenido.BringToFront();
        }

        private void ConfigurarDataGridView()
        {
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.Columns.Clear();

            dgvDetalle.BackgroundColor = Color.White;
            dgvDetalle.BorderStyle = BorderStyle.None;
            dgvDetalle.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDetalle.GridColor = Color.FromArgb(230, 230, 230);
            dgvDetalle.RowHeadersVisible = false;
            dgvDetalle.AllowUserToAddRows = false;
            dgvDetalle.AllowUserToDeleteRows = false;
            dgvDetalle.AllowUserToResizeRows = false;
            dgvDetalle.RowTemplate.Height = 40;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalle.MultiSelect = false;

            dgvDetalle.EnableHeadersVisualStyles = false;
            dgvDetalle.ColumnHeadersDefaultCellStyle.BackColor = colorPrimario;
            dgvDetalle.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetalle.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDetalle.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvDetalle.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvDetalle.ColumnHeadersHeight = 45;

            dgvDetalle.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvDetalle.DefaultCellStyle.ForeColor = colorTexto;
            dgvDetalle.DefaultCellStyle.BackColor = Color.White;
            dgvDetalle.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 201);
            dgvDetalle.DefaultCellStyle.SelectionForeColor = colorTexto;
            dgvDetalle.DefaultCellStyle.Padding = new Padding(10, 5, 5, 5);

            dgvDetalle.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);


            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Medicamento",
                HeaderText = "Medicamento",
                Width = 300,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cantidad",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvDetalle.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                HeaderText = "Subtotal",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "C2",
                    ForeColor = colorPrimario
                }
            });

            dgvDetalle.ReadOnly = true;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void CargarDetalles()
        {
            dgvDetalle.Rows.Clear();
            var detalles = _pedidosDAO.ObtenerDetallePedido(_idPedido);
            decimal total = 0;

            if (detalles == null || !detalles.Any())
            {
                return;
            }

            foreach (var d in detalles)
            {
                dgvDetalle.Rows.Add(d.Medicamento, d.Cantidad, d.Subtotal);
                total += d.Subtotal;
            }

            Label lblTotal = this.Controls.Find("lblTotalValor", true).FirstOrDefault() as Label;
            if (lblTotal != null)
            {
                lblTotal.Text = total.ToString("C2");
            }
        }
    }

    public class DetallePedido
    {
        public string Medicamento { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}