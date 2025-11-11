namespace Laboratorio_4
{
    partial class frmMedicamento
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.nudCantD = new System.Windows.Forms.NumericUpDown();
            this.txtPrecioU = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnImagen = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantD)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(78, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(80, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Imagen";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(80, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Precio Unitario";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(63, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Cantidad disponible";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(201, 62);
            this.txtNombre.MaxLength = 100;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(100, 25);
            this.txtNombre.TabIndex = 4;
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(251, 105);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(50, 50);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 5;
            this.pbImage.TabStop = false;
            // 
            // nudCantD
            // 
            this.nudCantD.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCantD.Location = new System.Drawing.Point(201, 185);
            this.nudCantD.Name = "nudCantD";
            this.nudCantD.Size = new System.Drawing.Size(100, 25);
            this.nudCantD.TabIndex = 6;
            // 
            // txtPrecioU
            // 
            this.txtPrecioU.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrecioU.Location = new System.Drawing.Point(201, 229);
            this.txtPrecioU.MaxLength = 100;
            this.txtPrecioU.Name = "txtPrecioU";
            this.txtPrecioU.Size = new System.Drawing.Size(100, 25);
            this.txtPrecioU.TabIndex = 7;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(145, 14);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(92, 17);
            this.Title.TabIndex = 8;
            this.Title.Text = "Medicamento";
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.Location = new System.Drawing.Point(141, 282);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 27);
            this.btnAceptar.TabIndex = 9;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // btnImagen
            // 
            this.btnImagen.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImagen.Location = new System.Drawing.Point(161, 105);
            this.btnImagen.Name = "btnImagen";
            this.btnImagen.Size = new System.Drawing.Size(75, 28);
            this.btnImagen.TabIndex = 10;
            this.btnImagen.Text = "Cargar";
            this.btnImagen.UseVisualStyleBackColor = true;
            this.btnImagen.Click += new System.EventHandler(this.BtnCargarImagen_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Title);
            this.panel1.Controls.Add(this.btnImagen);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtPrecioU);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nudCantD);
            this.panel1.Controls.Add(this.txtNombre);
            this.panel1.Controls.Add(this.pbImage);
            this.panel1.Location = new System.Drawing.Point(236, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 338);
            this.panel1.TabIndex = 11;
            // 
            // frmMedicamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(820, 644);
            this.Controls.Add(this.panel1);
            this.Name = "frmMedicamento";
            this.Text = "Medicamento";
            this.Load += new System.EventHandler(this.frmMedicamento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantD)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.NumericUpDown nudCantD;
        private System.Windows.Forms.TextBox txtPrecioU;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnImagen;
        private System.Windows.Forms.Panel panel1;
    }
}