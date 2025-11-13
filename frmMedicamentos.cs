using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmMedicamentos : Form
    {
        private Color colorPrimario = Color.FromArgb(46, 125, 50);
        private Color colorSecundario = Color.FromArgb(27, 94, 32);
        private Color colorFondo = Color.FromArgb(245, 248, 250);
        private Color colorTexto = Color.FromArgb(33, 33, 33);

        public frmMedicamentos()
        {
            InitializeComponent();
            PersonalizarFormulario();
            ConfigurarDataGridView();
            CargarMedicamentos();
        }

        private void PersonalizarFormulario()
        {
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gestión de Inventario de Medicamentos";
            this.BackColor = colorFondo;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl != dgvMedicamentos)
                {
                    ctrl.Visible = false;
                }
            }

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

            // Icono de inventario
            Panel iconoInventario = new Panel
            {
                Size = new Size(50, 50),
                Location = new Point(30, 35),
                BackColor = Color.White
            };
            iconoInventario.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(Brushes.White, 0, 0, 50, 50);
                using (SolidBrush brush = new SolidBrush(colorPrimario))
                {
                    // Caja de inventario
                    e.Graphics.FillRectangle(brush, 12, 15, 26, 20);
                    e.Graphics.FillRectangle(Brushes.White, 17, 20, 16, 2);
                    e.Graphics.FillRectangle(Brushes.White, 17, 25, 16, 2);
                }
            };
            panelHeader.Controls.Add(iconoInventario);

            Label lblTituloPrincipal = new Label
            {
                Text = "Inventario",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(100, 35),
                BackColor = Color.Transparent
            };
            panelHeader.Controls.Add(lblTituloPrincipal);

            Label lblSubtitulo = new Label
            {
                Text = "Gestión de existencias y precios",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(200, 255, 200),
                AutoSize = true,
                Location = new Point(100, 68),
                BackColor = Color.Transparent
            };
            panelHeader.Controls.Add(lblSubtitulo);

            Button btnAgregar = new Button
            {
                Text = "➕ Agregar Nuevo",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(160, 45),
                BackColor = Color.White,
                ForeColor = colorPrimario,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(this.Width - 190, 37),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Cursor = Cursors.Hand
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.MouseEnter += (s, e) => btnAgregar.BackColor = Color.FromArgb(240, 248, 240);
            btnAgregar.MouseLeave += (s, e) => btnAgregar.BackColor = Color.White;
            btnAgregar.Click += new EventHandler(btnAgregar_Click);
            panelHeader.Controls.Add(btnAgregar);

            Panel panelContenido = new Panel
            {
                Location = new Point(20, 140),
                BackColor = Color.White,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            };

            panelContenido.Size = new Size(this.Width - 40, this.Height - 180);

            panelContenido.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panelContenido.Width - 1, panelContenido.Height - 1);
                }
            };
            this.Controls.Add(panelContenido);

            dgvMedicamentos.Location = new Point(10, 10);
            dgvMedicamentos.Size = new Size(panelContenido.Width - 20, panelContenido.Height - 20);

            dgvMedicamentos.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            dgvMedicamentos.Visible = true;
            panelContenido.Controls.Add(dgvMedicamentos);

            panelHeader.BringToFront();
            panelContenido.BringToFront();
        }

        private void CargarInventario()
        {
            CargarMedicamentos();
        }

        private void ConfigurarDataGridView()
        {
            dgvMedicamentos.AutoGenerateColumns = false;
            dgvMedicamentos.Columns.Clear();
            dgvMedicamentos.RowTemplate.Height = 70;

            dgvMedicamentos.AllowUserToResizeColumns = true;
            dgvMedicamentos.AllowUserToResizeRows = false;

            dgvMedicamentos.BackgroundColor = Color.White;
            dgvMedicamentos.BorderStyle = BorderStyle.None;
            dgvMedicamentos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMedicamentos.GridColor = Color.FromArgb(230, 230, 230);
            dgvMedicamentos.RowHeadersVisible = false;
            dgvMedicamentos.AllowUserToAddRows = false;
            dgvMedicamentos.AllowUserToDeleteRows = false;
            dgvMedicamentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMedicamentos.MultiSelect = false;
            dgvMedicamentos.ReadOnly = true;

            dgvMedicamentos.EnableHeadersVisualStyles = false;
            dgvMedicamentos.ColumnHeadersDefaultCellStyle.BackColor = colorPrimario;
            dgvMedicamentos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMedicamentos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvMedicamentos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMedicamentos.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            dgvMedicamentos.ColumnHeadersHeight = 50;

            dgvMedicamentos.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvMedicamentos.DefaultCellStyle.ForeColor = colorTexto;
            dgvMedicamentos.DefaultCellStyle.BackColor = Color.White;
            dgvMedicamentos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 201);
            dgvMedicamentos.DefaultCellStyle.SelectionForeColor = colorTexto;
            dgvMedicamentos.DefaultCellStyle.Padding = new Padding(5, 8, 5, 8);
            dgvMedicamentos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvMedicamentos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Columna de Imagen
            var imgCol = new DataGridViewImageColumn
            {
                DataPropertyName = "Imagen",
                HeaderText = "Imagen",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 90,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvMedicamentos.Columns.Add(imgCol);

            // Columna Medicamento (con Fill para ocupar el espacio restante)
            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Medicamento",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 200,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            });

            // Columna Stock
            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CantidadDisponible",
                HeaderText = "Stock",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            });

            // Columna Precio
            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio",
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

            // Columna Editar
            var colEditar = new DataGridViewButtonColumn
            {
                HeaderText = "Editar",
                Text = "✏️",
                UseColumnTextForButtonValue = true,
                Width = 85,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            dgvMedicamentos.Columns.Add(colEditar);

            // Columna Eliminar
            var colEliminar = new DataGridViewButtonColumn
            {
                HeaderText = "Eliminar",
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                Width = 85,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            dgvMedicamentos.Columns.Add(colEliminar);

            // Columna ID (oculta)
            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdMedicamento",
                HeaderText = "ID",
                Visible = false
            });

            dgvMedicamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvMedicamentos.ScrollBars = ScrollBars.Both;
        }

        public void CargarMedicamentos()
        {
            using (var conexion = new ConexionBD())
            {
                var dao = new MedicamentosDAO(conexion);
                var lista = dao.ObtenerTodos();
                dgvMedicamentos.DataSource = lista;
            }
        }

        private void dgvMedicamentos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvMedicamentos.Columns[e.ColumnIndex].HeaderText == "Eliminar")
            {
                var medicamento = (Medicamento)dgvMedicamentos.Rows[e.RowIndex].DataBoundItem;

                var confirm = MessageBox.Show(
                    $"¿Seguro que desea eliminar el medicamento '{medicamento.Nombre}'?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.No)
                    return;

                using (var conexion = new ConexionBD())
                {
                    var dao = new MedicamentosDAO(conexion);
                    string resultado = dao.EliminarMedicamento(medicamento.IdMedicamento);

                    try
                    {
                        string carpetaImg = Path.Combine(Application.StartupPath, @"..\..\img\");

                        if (!string.IsNullOrEmpty(medicamento.Imagen))
                        {
                            string rutaImg = Path.Combine(carpetaImg, medicamento.Imagen);

                            if (File.Exists(rutaImg))
                                File.Delete(rutaImg);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la imagen: " + ex.Message);
                    }

                    MessageBox.Show(resultado);
                }

                CargarMedicamentos();
            }

            else if (dgvMedicamentos.Columns[e.ColumnIndex].HeaderText == "Editar")
            {
                var medicamento = (Medicamento)dgvMedicamentos.Rows[e.RowIndex].DataBoundItem;
                frmMedicamento frm = new frmMedicamento(2);
                frm.setDatos(medicamento);
                frm.Show();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmMedicamento frm = new frmMedicamento(1);
            frm.Show();
        }

        private void dgvMedicamentos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvMedicamentos.Columns[e.ColumnIndex].HeaderText == "Imagen" && e.Value != null)
            {
                try
                {
                    string rutaImg = Path.Combine(Application.StartupPath, @"..\..\img\");
                    string ruta = Path.Combine(rutaImg, e.Value.ToString());

                    if (File.Exists(ruta))
                    {
                        using (var tempImage = Image.FromFile(ruta))
                        {
                            e.Value = new Bitmap(tempImage);
                        }
                    }
                    else
                    {
                        e.Value = null;
                    }
                }
                catch
                {
                    e.Value = null;
                }
            }
        }
    }
}