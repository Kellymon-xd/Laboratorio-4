using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmPedidos : Form
    {
        private readonly ConexionBD _conexion;
        private readonly PedidosDAO _pedidosDAO;

        // Colores del tema farmacia
        private Color colorPrimario = Color.FromArgb(46, 125, 50);
        private Color colorSecundario = Color.FromArgb(27, 94, 32);
        private Color colorFondo = Color.FromArgb(245, 248, 250);
        private Color colorTexto = Color.FromArgb(33, 33, 33);

        public frmPedidos()
        {
            InitializeComponent();
            _conexion = new ConexionBD();
            _pedidosDAO = new PedidosDAO(_conexion);
            PersonalizarFormulario();
            ConfigurarDataGridView();
            CargarClientes();
            CargarPedidos();
        }

        private void PersonalizarFormulario()
        {
            // Configuración básica del formulario PRIMERO
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Size = new Size(950, 700);
            this.Text = "Gestión de Pedidos";
            this.BackColor = colorFondo;

            // Ocultar controles del diseñador excepto DataGridView y ComboBox
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl != dgvPedidos && ctrl != cboCliente)
                {
                    ctrl.Visible = false;
                }
            }

            // Panel Header
            Panel panelHeader = new Panel
            {
                Size = new Size(this.Width, 120),
                Location = new Point(0, 0),
                BackColor = colorPrimario,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
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
            this.Controls.Add(panelHeader);

            // Icono de pedidos
            Panel iconoPedidos = new Panel
            {
                Size = new Size(50, 50),
                Location = new Point(30, 35),
                BackColor = Color.White
            };
            iconoPedidos.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(Brushes.White, 0, 0, 50, 50);
                using (SolidBrush brush = new SolidBrush(colorPrimario))
                {
                    // Icono de documento con lista
                    e.Graphics.FillRectangle(brush, 12, 10, 26, 30);
                    e.Graphics.FillRectangle(Brushes.White, 16, 16, 18, 2);
                    e.Graphics.FillRectangle(Brushes.White, 16, 22, 18, 2);
                    e.Graphics.FillRectangle(Brushes.White, 16, 28, 12, 2);
                }
            };
            panelHeader.Controls.Add(iconoPedidos);

            // Título
            Label lblTitulo = new Label
            {
                Text = "Gestión de Pedidos",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(100, 35),
                BackColor = Color.Transparent
            };
            panelHeader.Controls.Add(lblTitulo);

            // Subtítulo
            Label lblSubtitulo = new Label
            {
                Text = "Administra y consulta los pedidos de clientes",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 255, 200),
                AutoSize = true,
                Location = new Point(100, 68),
                BackColor = Color.Transparent
            };
            panelHeader.Controls.Add(lblSubtitulo);

            // Panel de filtros
            Panel panelFiltros = new Panel
            {
                Size = new Size(this.Width - 40, 80),
                Location = new Point(20, 140),
                BackColor = Color.White,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            panelFiltros.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panelFiltros.Width - 1, panelFiltros.Height - 1);
                }
            };
            this.Controls.Add(panelFiltros);

            // Label "Filtrar por Cliente"
            Label lblFiltro = new Label
            {
                Text = "Filtrar por Cliente:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = colorTexto,
                AutoSize = true,
                Location = new Point(20, 28)
            };
            panelFiltros.Controls.Add(lblFiltro);

            // ComboBox Cliente estilizado
            cboCliente.Size = new Size(300, 30);
            cboCliente.Location = new Point(170, 25);
            cboCliente.Font = new Font("Segoe UI", 10);
            cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCliente.FlatStyle = FlatStyle.Flat;
            cboCliente.BackColor = Color.FromArgb(250, 250, 250);
            cboCliente.Visible = true;
            panelFiltros.Controls.Add(cboCliente);
            cboCliente.BringToFront();

            // Botón Actualizar
            Button btnActualizar = new Button
            {
                Text = "🔄 Actualizar",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(140, 40),
                Location = new Point(500, 20),
                BackColor = colorPrimario,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.MouseEnter += (s, e) => btnActualizar.BackColor = colorSecundario;
            btnActualizar.MouseLeave += (s, e) => btnActualizar.BackColor = colorPrimario;
            btnActualizar.Click += (s, e) => CargarPedidos();
            panelFiltros.Controls.Add(btnActualizar);

            // Panel contenedor del DataGridView
            Panel panelContenido = new Panel
            {
                Location = new Point(20, 240),
                BackColor = Color.White,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            };
            panelContenido.Size = new Size(this.Width - 40, this.Height - 280);
            panelContenido.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panelContenido.Width - 1, panelContenido.Height - 1);
                }
            };
            this.Controls.Add(panelContenido);

            // Label "Lista de Pedidos"
            Label lblListaPedidos = new Label
            {
                Text = "Lista de Pedidos",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = colorTexto,
                AutoSize = true,
                Location = new Point(15, 15)
            };
            panelContenido.Controls.Add(lblListaPedidos);

            // Configurar DataGridView
            dgvPedidos.Location = new Point(15, 50);
            dgvPedidos.Size = new Size(panelContenido.Width - 30, panelContenido.Height - 65);
            dgvPedidos.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            dgvPedidos.Visible = true;
            panelContenido.Controls.Add(dgvPedidos);
            dgvPedidos.BringToFront();

            // Traer todo al frente
            panelHeader.BringToFront();
            panelFiltros.BringToFront();
            panelContenido.BringToFront();
        }

        private void ConfigurarDataGridView()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.Columns.Clear();
            dgvPedidos.RowTemplate.Height = 50;

            dgvPedidos.BackgroundColor = Color.White;
            dgvPedidos.BorderStyle = BorderStyle.None;
            dgvPedidos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPedidos.GridColor = Color.FromArgb(230, 230, 230);
            dgvPedidos.RowHeadersVisible = false;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToDeleteRows = false;
            dgvPedidos.AllowUserToResizeRows = false;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.MultiSelect = false;
            dgvPedidos.ReadOnly = true;

            // Estilos del header
            dgvPedidos.EnableHeadersVisualStyles = false;
            dgvPedidos.ColumnHeadersDefaultCellStyle.BackColor = colorPrimario;
            dgvPedidos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPedidos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPedidos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPedidos.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            dgvPedidos.ColumnHeadersHeight = 45;

            // Estilos de las celdas
            dgvPedidos.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvPedidos.DefaultCellStyle.ForeColor = colorTexto;
            dgvPedidos.DefaultCellStyle.BackColor = Color.White;
            dgvPedidos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 201);
            dgvPedidos.DefaultCellStyle.SelectionForeColor = colorTexto;
            dgvPedidos.DefaultCellStyle.Padding = new Padding(5, 5, 5, 5);
            dgvPedidos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // ID Pedido (oculto)
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdPedido",
                DataPropertyName = "IdPedido",
                HeaderText = "IdPedido",
                Visible = false
            });

            // Número de Pedido
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdPedido",
                HeaderText = "N° Pedido",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            });

            // Cliente
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cliente",
                HeaderText = "Cliente",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 200,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                }
            });

            // Fecha
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            // Total
            dgvPedidos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                HeaderText = "Total",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "C2",
                    ForeColor = colorPrimario,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold)
                }
            });

            // Botón para detalles
            var colDetalles = new DataGridViewButtonColumn
            {
                HeaderText = "Ver Detalles",
                Text = "👁️ Ver",
                UseColumnTextForButtonValue = true,
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            dgvPedidos.Columns.Add(colDetalles);
        }

        private void CargarClientes()
        {
            // Desuscribir temporalmente el evento para evitar errores durante la carga
            cboCliente.SelectedIndexChanged -= cboCliente_SelectedIndexChanged;

            cboCliente.Items.Clear();
            var clientes = _pedidosDAO.ObtenerClientes();
            cboCliente.Items.Add(new ComboboxItem { Text = "Todos", Value = null });
            foreach (var c in clientes)
                cboCliente.Items.Add(c);
            cboCliente.SelectedIndex = 0;

            // Volver a suscribir el evento
            cboCliente.SelectedIndexChanged += cboCliente_SelectedIndexChanged;
        }

        private void CargarPedidos()
        {
            try
            {
                dgvPedidos.Rows.Clear();
                string idCliente = null;

                if (cboCliente.SelectedItem != null && cboCliente.SelectedItem is ComboboxItem selected)
                {
                    idCliente = selected.Value?.ToString();
                }

                var pedidos = _pedidosDAO.ObtenerPedidos(idCliente);
                foreach (var p in pedidos)
                {
                    dgvPedidos.Rows.Add(p.IdPedido, p.IdPedido, p.Cliente, p.Fecha, p.Total);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pedidos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCliente.SelectedItem != null)
            {
                CargarPedidos();
            }
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