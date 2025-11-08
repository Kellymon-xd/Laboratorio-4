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
        private int mode; // 1 = agregar, 2 = editar, 3=eliminar
        private readonly string carpetaImg = Path.Combine(Application.StartupPath, @"..\..\img\");

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
                string ruta = Path.Combine(carpetaImg, imagen);
                if (File.Exists(ruta))
                {
                    using (var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
                    {
                        pbImage.Image = Image.FromStream(fs);
                    }
                }
            }
        }

        private void BtnCargarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        pbImage.Image = Image.FromStream(fs);
                    }
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

            string nombreImagen = null;

            if (!string.IsNullOrEmpty(imagen))
            {
                string extension = ".jpg";
                nombreImagen = nombre.ToLower() + extension;
                string rutaDestino = Path.Combine(carpetaImg, nombreImagen);

                try
                {

                    if (File.Exists(imagen) && !imagen.StartsWith(carpetaImg, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(imagen, rutaDestino, true);
                    }
                    else if (File.Exists(Path.Combine(carpetaImg, imagen)))
                    {
                        string rutaActual = Path.Combine(carpetaImg, imagen);
                        if (!rutaActual.Equals(rutaDestino, StringComparison.OrdinalIgnoreCase))
                        {
                            File.Copy(rutaActual, rutaDestino, true);
                            File.Delete(rutaActual);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar la imagen: " + ex.Message);
                }
            }

            using (var conexion = new ConexionBD())
            {
                var dao = new MedicamentosDAO(conexion);

                string resultado="";


                if (mode == 1) // Agregar
                    resultado = dao.AgregarMedicamento(nombre, nombreImagen, cantidad, precio);
                else if (mode == 2) // Editar
                    resultado = dao.ModificarMedicamento(Id_medicamento, nombre, nombreImagen, cantidad, precio);
                else if (mode == 3) // Eliminar
                {
                    var confirm = MessageBox.Show($"¿Seguro que desea eliminar '{nombre}'?",
                                                  "Confirmar eliminación",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);
                    if (confirm == DialogResult.No)
                        return;

                    resultado = dao.EliminarMedicamento(Id_medicamento);
                }

            }

           
            var frm = Application.OpenForms["frmMedicamentos"] as frmMedicamentos;
            frm?.CargarMedicamentos();


            this.Close();
        }

    



        private void frmMedicamento_Load(object sender, EventArgs e)
        {

        }
    }
}
