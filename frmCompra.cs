using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmCompra : Form
    {
        public frmCompra()
        {
            InitializeComponent();
            ConfigurarDataGridView();
            cargarInventario();
        }

        private void cargarInventario()
        {
            using (var conexion = new ConexionBD())
            {
                var dao = new MedicamentosDAO(conexion);
                var lista = dao.ObtenerTodos();
                dgvCatalogo.DataSource = lista;
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvCatalogo.AutoGenerateColumns = false;

            dgvCatalogo.Columns.Clear();

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdMedicamento",
                HeaderText = "ID",
                Visible = false
            });


            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Medicamento",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            var imgCol = new DataGridViewImageColumn
            {
                DataPropertyName = "Imagen",
                HeaderText = "Imagen",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            dgvCatalogo.Columns.Add(imgCol);


            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CantidadDisponible",
                HeaderText = "Disponible"
            });

            dgvCatalogo.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio"
            });

            // 🔹 Columna con botón "+"
            var colAgregar = new DataGridViewButtonColumn
            {
                HeaderText = "Agregar",
                Text = "+",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dgvCatalogo.Columns.Add(colAgregar);

            dgvCatalogo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCatalogo.ReadOnly = true;
        }

        private void dgvCatalogo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCatalogo.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                var medicamento = (Medicamento)dgvCatalogo.Rows[e.RowIndex].DataBoundItem;
                CarritoManager.Instancia.Agregar(medicamento);
                MessageBox.Show($"{medicamento.Nombre} agregado al carrito.");
            }
        }

        private void dgvCatalogo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCatalogo.Columns[e.ColumnIndex].HeaderText == "Imagen" && e.Value != null)
            {
                string rutaImg = Path.Combine(Application.StartupPath, @"..\..\img\");

                string ruta = Path.Combine(rutaImg, e.Value.ToString());
                if (File.Exists(ruta))
                {
                    e.Value = Image.FromFile(ruta);
                }
            }
        }

    }
}
