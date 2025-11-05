using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmMedicamentos : Form
    {
        public frmMedicamentos()
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
                dgvMedicamentos.DataSource = lista;
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvMedicamentos.AutoGenerateColumns = false;

            dgvMedicamentos.Columns.Clear();

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdMedicamento",
                HeaderText = "ID",
                Visible = false
            });


            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Medicamento",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CantidadDisponible",
                HeaderText = "Disponible"
            });

            dgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio"
            });

            var colAgregar = new DataGridViewButtonColumn
            {
                HeaderText = "Editar",
                Text = "✏️",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dgvMedicamentos.Columns.Add(colAgregar);

            dgvMedicamentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMedicamentos.ReadOnly = true;
        }

        private void dgvMedicamentos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMedicamentos.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
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
    }
}