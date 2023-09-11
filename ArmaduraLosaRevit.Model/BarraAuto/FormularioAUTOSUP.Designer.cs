namespace ArmaduraLosaRevit.Model
{
    partial class FormularioAUTOSUP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormularioAUTOSUP));
            this.comboBox_SX = new System.Windows.Forms.ComboBox();
            this.buttonaceptar = new System.Windows.Forms.Button();
            this.button_cerrar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_SX
            // 
            this.comboBox_SX.FormattingEnabled = true;
            this.comboBox_SX.Items.AddRange(new object[] {
            "S1",
            "S2"});
            this.comboBox_SX.Location = new System.Drawing.Point(81, 43);
            this.comboBox_SX.Name = "comboBox_SX";
            this.comboBox_SX.Size = new System.Drawing.Size(47, 21);
            this.comboBox_SX.TabIndex = 0;
            this.comboBox_SX.Text = "S1";
  
            // 
            // buttonaceptar
            // 
            this.buttonaceptar.Location = new System.Drawing.Point(23, 128);
            this.buttonaceptar.Name = "buttonaceptar";
            this.buttonaceptar.Size = new System.Drawing.Size(75, 23);
            this.buttonaceptar.TabIndex = 4;
            this.buttonaceptar.Text = "Aceptar";
            this.buttonaceptar.UseVisualStyleBackColor = true;
            this.buttonaceptar.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_cerrar
            // 
            this.button_cerrar.Location = new System.Drawing.Point(104, 128);
            this.button_cerrar.Name = "button_cerrar";
            this.button_cerrar.Size = new System.Drawing.Size(75, 23);
            this.button_cerrar.TabIndex = 5;
            this.button_cerrar.Text = "Cerrar";
            this.button_cerrar.UseVisualStyleBackColor = true;
            this.button_cerrar.Click += new System.EventHandler(this.button_cerrar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox_SX);
            this.groupBox1.Location = new System.Drawing.Point(23, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 110);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Suple";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // FormularioAUTOSUP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(206, 166);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cerrar);
            this.Controls.Add(this.buttonaceptar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormularioAUTOSUP";
            this.Text = "Armadura Inferior";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_SX;
        private System.Windows.Forms.Button buttonaceptar;
        private System.Windows.Forms.Button button_cerrar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
    }
}