namespace Laboratorio_4
{
    partial class frmMedicamentos
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
            this.dgvMedicamentos = new System.Windows.Forms.DataGridView();
            this.lblMedicamentos = new System.Windows.Forms.Label();
            this.btnAgregar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicamentos)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMedicamentos
            // 
            this.dgvMedicamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMedicamentos.Location = new System.Drawing.Point(32, 86);
            this.dgvMedicamentos.Name = "dgvMedicamentos";
            this.dgvMedicamentos.RowHeadersWidth = 51;
            this.dgvMedicamentos.Size = new System.Drawing.Size(737, 341);
            this.dgvMedicamentos.TabIndex = 0;
            this.dgvMedicamentos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMedicamentos_CellContentClick);
            this.dgvMedicamentos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMedicamentos_CellFormatting);
            // 
            // lblMedicamentos
            // 
            this.lblMedicamentos.AutoSize = true;
            this.lblMedicamentos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicamentos.Location = new System.Drawing.Point(29, 26);
            this.lblMedicamentos.Name = "lblMedicamentos";
            this.lblMedicamentos.Size = new System.Drawing.Size(98, 17);
            this.lblMedicamentos.TabIndex = 1;
            this.lblMedicamentos.Text = "Medicamentos";
            // 
            // btnAgregar
            // 
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Location = new System.Drawing.Point(694, 20);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 29);
            this.btnAgregar.TabIndex = 2;
            this.btnAgregar.Text = "➕";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // frmMedicamentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.lblMedicamentos);
            this.Controls.Add(this.dgvMedicamentos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMedicamentos";
            this.Text = "Medicamentos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicamentos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMedicamentos;
        private System.Windows.Forms.Label lblMedicamentos;
        private System.Windows.Forms.Button btnAgregar;
    }
}