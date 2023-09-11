namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    partial class FormCub
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.button_creaExcel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(135, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 81);
            this.button1.TabIndex = 1;
            this.button1.Text = " Crear Schedules\r\n\'Cubicacion Barras\'\r\n";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button_creaExcel
            // 
            this.button_creaExcel.BackColor = System.Drawing.Color.White;
            this.button_creaExcel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_creaExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_creaExcel.Location = new System.Drawing.Point(14, 13);
            this.button_creaExcel.Name = "button_creaExcel";
            this.button_creaExcel.Size = new System.Drawing.Size(107, 81);
            this.button_creaExcel.TabIndex = 2;
            this.button_creaExcel.Text = "Crear Excel\r\nCubicaciones";
            this.toolTip1.SetToolTip(this.button_creaExcel, "La cubicacion se realice  en la vista 3D \'{3D}\'.\r\n\r\n");
            this.button_creaExcel.UseVisualStyleBackColor = false;
            this.button_creaExcel.Click += new System.EventHandler(this.Button_creaExcel_Click);
            // 
            // FormCub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(265, 112);
            this.Controls.Add(this.button_creaExcel);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCub";
            this.Text = "Cubicaciones";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_creaExcel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}