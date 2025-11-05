using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmMedicamento : Form
    {
        private string Id_medicamento;
        private string imagen;
        private int mode; // 1 = agregar, 2 = editar
        private readonly string carpetaImg = Path.Combine(Application.StartupPath, "img");

        public frmMedicamento(int mode)
        {
            InitializeComponent();
            this.mode = mode;

            if (!Directory.Exists(carpetaImg))
                Directory.CreateDirectory(carpetaImg);
        }

        public void setDatos(Medicamento m)
        {
            Id_medicamento = m.IdMedicamento;
            txtNombre.Text = m.Nombre;
            txtPrecioU.Text = m.PrecioUnitario.ToString("0.00");
            imagen = m.Imagen;

            nudCantD.Minimum = 0;
            nudCantD.Maximum = 1000000;
            nudCantD.Value = m.CantidadDisponible;

            if (!string.IsNullOrEmpty(imagen))
            {
                string rutaImagen = Path.Combine(carpetaImg, imagen);
                if (File.Exists(rutaImagen))
                    pbImage.Image = Image.FromFile(rutaImagen);
            }
        }

        private void BtnCargarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbImage.Image = Image.FromFile(ofd.FileName);
                    imagen = ofd.FileName;
                }
            }
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            decimal precio;
            int cantidad = (int)nudCantD.Value;

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingrese un nombre válido.");
                return;
            }

            if (!decimal.TryParse(txtPrecioU.Text, out precio))
            {
                MessageBox.Show("Ingrese un precio válido.");
                return;
            }

            string extension = Path.GetExtension(imagen) ?? ".jpg";
            string nombreImagen = nombre + extension;
            string rutaDestino = Path.Combine(carpetaImg, nombreImagen);

            try
            {
                if (!string.IsNullOrEmpty(imagen) && File.Exists(imagen))
                {
                    File.Copy(imagen, rutaDestino, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la imagen: " + ex.Message);
            }

            if (mode == 1) // Agregar
            {
                using (var conexion = new ConexionBD())
                {
                    var dao = new MedicamentosDAO(conexion);
                    string resultado = dao.AgregarMedicamento(nombre, nombreImagen, cantidad, precio);
                    MessageBox.Show(resultado);
                }
            }
            else if (mode == 2) // Editar
            {
                using (var conexion = new ConexionBD())
                {
                    var dao = new MedicamentosDAO(conexion);
                    string resultado = dao.ModificarMedicamento(Id_medicamento, nombre, nombreImagen, cantidad, precio);
                    MessageBox.Show(resultado);
                }
            }

            this.Close();
        }
    }
}
