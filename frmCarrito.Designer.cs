namespace Laboratorio_4
{
    partial class frmCarrito
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
            this.dgvCarrito = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnPedir = new System.Windows.Forms.Button();
            this.lblNum = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCarrito
            // 
            this.dgvCarrito.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvCarrito.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCarrito.Location = new System.Drawing.Point(35, 35);
            this.dgvCarrito.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvCarrito.Name = "dgvCarrito";
            this.dgvCarrito.Size = new System.Drawing.Size(988, 296);
            this.dgvCarrito.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(51, 379);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(47, 17);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total: ";
            // 
            // btnPedir
            // 
            this.btnPedir.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnPedir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPedir.ForeColor = System.Drawing.Color.White;
            this.btnPedir.Location = new System.Drawing.Point(55, 434);
            this.btnPedir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPedir.Name = "btnPedir";
            this.btnPedir.Size = new System.Drawing.Size(144, 30);
            this.btnPedir.TabIndex = 2;
            this.btnPedir.Text = "Realizar Pedido";
            this.btnPedir.UseVisualStyleBackColor = false;
            this.btnPedir.Click += new System.EventHandler(this.btnPedir_Click);
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum.Location = new System.Drawing.Point(149, 379);
            this.lblNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(33, 17);
            this.lblNum.TabIndex = 3;
            this.lblNum.Text = "0.00";
            // 
            // frmCarrito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 588);
            this.Controls.Add(this.lblNum);
            this.Controls.Add(this.btnPedir);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dgvCarrito);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCarrito";
            this.Text = "Carrito";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCarrito;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnPedir;
        private System.Windows.Forms.Label lblNum;
    }
}