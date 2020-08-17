namespace Архивация_BMP
{
    partial class Frm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBMP = new System.Windows.Forms.TextBox();
            this.OD = new System.Windows.Forms.OpenFileDialog();
            this.Pn = new System.Windows.Forms.Panel();
            this.cbBWT = new System.Windows.Forms.CheckBox();
            this.lbRes = new System.Windows.Forms.Label();
            this.btBMP = new System.Windows.Forms.Button();
            this.btCode = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.PB = new System.Windows.Forms.PictureBox();
            this.Pn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBMP
            // 
            this.txtBMP.Location = new System.Drawing.Point(2, 2);
            this.txtBMP.Name = "txtBMP";
            this.txtBMP.Size = new System.Drawing.Size(339, 20);
            this.txtBMP.TabIndex = 0;
            this.txtBMP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBMP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_KeyDown);
            // 
            // OD
            // 
            this.OD.Filter = "Файлы BMP|*.bmp";
            // 
            // Pn
            // 
            this.Pn.BackColor = System.Drawing.Color.Silver;
            this.Pn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pn.Controls.Add(this.cbBWT);
            this.Pn.Controls.Add(this.lbRes);
            this.Pn.Controls.Add(this.btBMP);
            this.Pn.Controls.Add(this.btCode);
            this.Pn.Controls.Add(this.txtCode);
            this.Pn.Dock = System.Windows.Forms.DockStyle.Top;
            this.Pn.Location = new System.Drawing.Point(0, 0);
            this.Pn.Name = "Pn";
            this.Pn.Size = new System.Drawing.Size(402, 62);
            this.Pn.TabIndex = 2;
            // 
            // cbBWT
            // 
            this.cbBWT.AutoSize = true;
            this.cbBWT.Location = new System.Drawing.Point(1, 42);
            this.cbBWT.Name = "cbBWT";
            this.cbBWT.Size = new System.Drawing.Size(51, 17);
            this.cbBWT.TabIndex = 6;
            this.cbBWT.Text = "BWT";
            this.cbBWT.UseVisualStyleBackColor = true;
            // 
            // lbRes
            // 
            this.lbRes.AutoSize = true;
            this.lbRes.Location = new System.Drawing.Point(61, 44);
            this.lbRes.Name = "lbRes";
            this.lbRes.Size = new System.Drawing.Size(0, 13);
            this.lbRes.TabIndex = 5;
            // 
            // btBMP
            // 
            this.btBMP.Location = new System.Drawing.Point(340, -1);
            this.btBMP.Name = "btBMP";
            this.btBMP.Size = new System.Drawing.Size(60, 23);
            this.btBMP.TabIndex = 4;
            this.btBMP.Text = "BMP";
            this.btBMP.UseVisualStyleBackColor = true;
            this.btBMP.Click += new System.EventHandler(this.btBMP_Click);
            this.btBMP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_KeyDown);
            // 
            // btCode
            // 
            this.btCode.Location = new System.Drawing.Point(340, 21);
            this.btCode.Name = "btCode";
            this.btCode.Size = new System.Drawing.Size(60, 23);
            this.btCode.TabIndex = 3;
            this.btCode.Text = "CODE";
            this.btCode.UseVisualStyleBackColor = true;
            this.btCode.Click += new System.EventHandler(this.btCode_Click);
            this.btCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_KeyDown);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(1, 22);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(339, 20);
            this.txtCode.TabIndex = 2;
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_KeyDown);
            // 
            // PB
            // 
            this.PB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB.Location = new System.Drawing.Point(0, 62);
            this.PB.Name = "PB";
            this.PB.Size = new System.Drawing.Size(402, 411);
            this.PB.TabIndex = 3;
            this.PB.TabStop = false;
            // 
            // Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 473);
            this.Controls.Add(this.PB);
            this.Controls.Add(this.txtBMP);
            this.Controls.Add(this.Pn);
            this.Name = "Frm";
            this.Text = "Архивация BMP";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_KeyDown);
            this.Pn.ResumeLayout(false);
            this.Pn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBMP;
        private System.Windows.Forms.OpenFileDialog OD;
        private System.Windows.Forms.Panel Pn;
        private System.Windows.Forms.Button btCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btBMP;
        private System.Windows.Forms.PictureBox PB;
        private System.Windows.Forms.Label lbRes;
        private System.Windows.Forms.CheckBox cbBWT;
    }
}

