using Laboratorio_4.Modelos;
using Laboratorio_4.Servicios;
using System;
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
            ConfigurarFormulario();
            ConfigurarFlowLayout();
            CargarCatalogo();
        }

        private void ConfigurarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.MinimumSize = new Size(900, 600);
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Padding = new Padding(0);
            this.Margin = new Padding(0);
        }

        private void ConfigurarFlowLayout()
        {
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.BackColor = Color.FromArgb(240, 240, 240);
            flowLayoutPanel1.Padding = new Padding(20);
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
            var verdePrincipal = Color.FromArgb(46, 125, 50);

            var card = new Panel
            {
                Width = 250,
                Height = 300,
                BackColor = Color.White,
                Margin = new Padding(15),
                BorderStyle = BorderStyle.None
            };

            var pb = new PictureBox
            {
                Width = 220,
                Height = 130,
                Top = 10,
                Left = 15,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = CargarImagen(medicamento.Imagen),
                BackColor = Color.White
            };
            card.Controls.Add(pb);

            
            var lineaSeparadora = new Panel
            {
                Width = 220,
                Height = 1,
                Left = 15,
                Top = pb.Bottom + 5,
                BackColor = Color.FromArgb(220, 220, 220)
            };
            card.Controls.Add(lineaSeparadora);

            
            var lblNombre = new Label
            {
                Text = medicamento.Nombre,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 220,
                Height = 40,
                Top = lineaSeparadora.Bottom + 5,
                Left = 15,
                BackColor = Color.White
            };
            card.Controls.Add(lblNombre);

            
            var lblPrecio = new Label
            {
                Text = $"Precio: ${medicamento.PrecioUnitario:F2}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 220,
                Height = 25,
                Top = lblNombre.Bottom + 5,
                Left = 15,
                BackColor = Color.White
            };
            card.Controls.Add(lblPrecio);

           
            var btnAgregar = new Button
            {
                Text = "Agregar 🛒",
                Width = 220,
                Height = 40,
                Top = lblPrecio.Bottom + 10, 
                Left = 15,
                BackColor = verdePrincipal,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAgregar.FlatAppearance.BorderSize = 0;

            btnAgregar.Click += (s, e) =>
            {
                CarritoManager.Instancia.Agregar(medicamento);
                MessageBox.Show($"{medicamento.Nombre} agregado al carrito.", "Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnAgregar.MouseEnter += (s, e) => btnAgregar.BackColor = Color.FromArgb(56, 142, 60);
            btnAgregar.MouseLeave += (s, e) => btnAgregar.BackColor = verdePrincipal;

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
            catch { }
            return null;
        }
    }
}
