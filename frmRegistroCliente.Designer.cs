namespace Laboratorio_4
{
    partial class frmRegistroCliente
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
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtContraseña = new System.Windows.Forms.TextBox();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.lblContraseña = new System.Windows.Forms.Label();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtApellido
            // 
            this.txtApellido.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApellido.Location = new System.Drawing.Point(152, 109);
            this.txtApellido.Margin = new System.Windows.Forms.Padding(4);
            this.txtApellido.MaxLength = 50;
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(148, 25);
            this.txtApellido.TabIndex = 11;
            this.txtApellido.TextChanged += new System.EventHandler(this.txtApellido_TextChanged);
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(152, 60);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(4);
            this.txtNombre.MaxLength = 50;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(148, 25);
            this.txtNombre.TabIndex = 10;
            this.txtNombre.TextChanged += new System.EventHandler(this.txtNombre_TextChanged);
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApellido.Location = new System.Drawing.Point(49, 112);
            this.lblApellido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(60, 17);
            this.lblApellido.TabIndex = 9;
            this.lblApellido.Text = "Apellido";
            this.lblApellido.Click += new System.EventHandler(this.lblApellido_Click);
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(49, 60);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(58, 17);
            this.lblNombre.TabIndex = 8;
            this.lblNombre.Text = "Nombre";
            this.lblNombre.Click += new System.EventHandler(this.lblNombre_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(132, 9);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(76, 17);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "Registrarse";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // txtContraseña
            // 
            this.txtContraseña.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContraseña.Location = new System.Drawing.Point(152, 200);
            this.txtContraseña.Margin = new System.Windows.Forms.Padding(4);
            this.txtContraseña.MaxLength = 128;
            this.txtContraseña.Name = "txtContraseña";
            this.txtContraseña.PasswordChar = '*';
            this.txtContraseña.Size = new System.Drawing.Size(148, 25);
            this.txtContraseña.TabIndex = 17;
            this.txtContraseña.TextChanged += new System.EventHandler(this.txtContraseña_TextChanged);
            // 
            // txtCorreo
            // 
            this.txtCorreo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCorreo.Location = new System.Drawing.Point(152, 151);
            this.txtCorreo.Margin = new System.Windows.Forms.Padding(4);
            this.txtCorreo.MaxLength = 100;
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(148, 25);
            this.txtCorreo.TabIndex = 16;
            this.txtCorreo.TextChanged += new System.EventHandler(this.txtCorreo_TextChanged);
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContraseña.Location = new System.Drawing.Point(49, 203);
            this.lblContraseña.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(77, 17);
            this.lblContraseña.TabIndex = 15;
            this.lblContraseña.Text = "Contraseña";
            this.lblContraseña.Click += new System.EventHandler(this.lblContraseña_Click);
            // 
            // lblCorreo
            // 
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorreo.Location = new System.Drawing.Point(49, 151);
            this.lblCorreo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(49, 17);
            this.lblCorreo.TabIndex = 14;
            this.lblCorreo.Text = "Correo";
            this.lblCorreo.Click += new System.EventHandler(this.lblCorreo_Click);
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ForeColor = System.Drawing.Color.White;
            this.btnIngresar.Location = new System.Drawing.Point(125, 256);
            this.btnIngresar.Margin = new System.Windows.Forms.Padding(4);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(112, 32);
            this.btnIngresar.TabIndex = 18;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = false;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(132, 306);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(90, 17);
            this.linkLabel1.TabIndex = 19;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Iniciar Sesión";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.lblNombre);
            this.panel1.Controls.Add(this.btnIngresar);
            this.panel1.Controls.Add(this.lblApellido);
            this.panel1.Controls.Add(this.txtContraseña);
            this.panel1.Controls.Add(this.txtNombre);
            this.panel1.Controls.Add(this.txtCorreo);
            this.panel1.Controls.Add(this.txtApellido);
            this.panel1.Controls.Add(this.lblContraseña);
            this.panel1.Controls.Add(this.lblCorreo);
            this.panel1.Location = new System.Drawing.Point(242, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 364);
            this.panel1.TabIndex = 20;
            // 
            // frmRegistroCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(865, 699);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRegistroCliente";
            this.Text = "Registro de Cliente";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtContraseña;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label lblContraseña;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panel1;
    }
}