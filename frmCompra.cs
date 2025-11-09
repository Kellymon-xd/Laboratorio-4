using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmCompra : Form
    {
        public frmCompra()
        {
            InitializeComponent();
            ConfigurarFlowLayout();
            CargarCatalogo();
        }

        private void ConfigurarFlowLayout()
        {
            // Configura el flowLayoutPanel1 desde código por si algo se resetea en el diseñador
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.BackColor = Color.LightGray;
        }

        private void CargarCatalogo()
        {
            flowLayoutPanel1.Controls.Clear();

            using (var conexion = new ConexionBD())
            {
                var dao = new MedicamentosDAO(conexion);
                var lista = dao.ObtenerTodos();

                foreach (var medicamento in lista)
                {
                    var tarjeta = CrearTarjetaMedicamento(medicamento);
                    flowLayoutPanel1.Controls.Add(tarjeta);
                }
            }
        }

        private Panel CrearTarjetaMedicamento(Medicamento medicamento)
        {
            // Panel principal de la tarjeta
            var card = new Panel
            {
                Width = 220,
                Height = 320,
                BackColor = Color.White,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Imagen
            var pb = new PictureBox
            {
                Width = 200,
                Height = 150,
                Top = 10,
                Left = 10,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = CargarImagen(medicamento.Imagen)
            };

            // Nombre del medicamento
            var lblNombre = new Label
            {
                Text = medicamento.Nombre,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 200,
                Height = 40,
                Top = pb.Bottom + 5,
                Left = 10,
                BackColor = Color.Transparent
            };

            // Precio
            var lblPrecio = new Label
            {
                Text = $"Precio: ${medicamento.PrecioUnitario:F2}",
                Font = new Font("Segoe UI", 9),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 200,
                Height = 25,
                Top = lblNombre.Bottom + 5,
                Left = 10,
                BackColor = Color.Transparent
            };

            // Botón de agregar al carrito
            var btnAgregar = new Button
            {
                Text = "Agregar 🛒",
                Width = 200,
                Height = 35,
                Top = lblPrecio.Bottom + 5,
                Left = 10,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAgregar.FlatAppearance.BorderSize = 0;

            btnAgregar.Click += (s, e) =>
            {
                CarritoManager.Instancia.Agregar(medicamento);
                MessageBox.Show($"{medicamento.Nombre} agregado al carrito.");
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = Color.White;
                card.BorderStyle = BorderStyle.FixedSingle;
            };

            // Agregar controles dentro del panel
            card.Controls.Add(pb);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblPrecio);
            card.Controls.Add(btnAgregar);

            return card;
        }

        private Image CargarImagen(string nombreArchivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreArchivo))
                    return null;

                string rutaImg = Path.Combine(Application.StartupPath, @"..\..\img\", nombreArchivo);
                if (File.Exists(rutaImg))
                    return Image.FromFile(rutaImg);
            }
            catch
            {
                // Ignorar errores de imagen
            }
            return null;
        }
    }
}
