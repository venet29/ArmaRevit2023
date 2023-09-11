namespace ArmaduraLosaRevit.Model.UTILES.FormFallasDialogo
{
    partial class UtilitarioFallasDialogosForm
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
            this.button_aceptar = new System.Windows.Forms.Button();
            this.radioButton_normal = new System.Windows.Forms.RadioButton();
            this.radioButton_volver = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBox_msaje = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_aceptar
            // 
            this.button_aceptar.Location = new System.Drawing.Point(22, 103);
            this.button_aceptar.Name = "button_aceptar";
            this.button_aceptar.Size = new System.Drawing.Size(75, 23);
            this.button_aceptar.TabIndex = 0;
            this.button_aceptar.Text = "Aceptar";
            this.button_aceptar.UseVisualStyleBackColor = true;
            this.button_aceptar.Click += new System.EventHandler(this.button_aceptar_Click);
            // 
            // radioButton_normal
            // 
            this.radioButton_normal.AutoSize = true;
            this.radioButton_normal.Checked = true;
            this.radioButton_normal.Location = new System.Drawing.Point(26, 23);
            this.radioButton_normal.Name = "radioButton_normal";
            this.radioButton_normal.Size = new System.Drawing.Size(140, 17);
            this.radioButton_normal.TabIndex = 2;
            this.radioButton_normal.TabStop = true;
            this.radioButton_normal.Text = "Resolucion normal Revit";
            this.toolTip1.SetToolTip(this.radioButton_normal, "Continua con el mensaje de error que envia Revit\r\n");
            this.radioButton_normal.UseVisualStyleBackColor = true;
            // 
            // radioButton_volver
            // 
            this.radioButton_volver.AutoSize = true;
            this.radioButton_volver.Location = new System.Drawing.Point(26, 59);
            this.radioButton_volver.Name = "radioButton_volver";
            this.radioButton_volver.Size = new System.Drawing.Size(128, 17);
            this.radioButton_volver.TabIndex = 3;
            this.radioButton_volver.Text = "Volver atras comando";
            this.toolTip1.SetToolTip(this.radioButton_volver, "Vuelve atras el comando. Se deshacen los cambios.\r\n");
            this.radioButton_volver.UseVisualStyleBackColor = true;
            // 
            // textBox_msaje
            // 
            this.textBox_msaje.Location = new System.Drawing.Point(228, 26);
            this.textBox_msaje.Multiline = true;
            this.textBox_msaje.Name = "textBox_msaje";
            this.textBox_msaje.Size = new System.Drawing.Size(291, 100);
            this.textBox_msaje.TabIndex = 4;
            // 
            // UtilitarioFallasDialogosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 149);
            this.Controls.Add(this.textBox_msaje);
            this.Controls.Add(this.radioButton_volver);
            this.Controls.Add(this.radioButton_normal);
            this.Controls.Add(this.button_aceptar);
            this.Name = "UtilitarioFallasDialogosForm";
            this.Text = "Fallas Dialogos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_aceptar;
        private System.Windows.Forms.RadioButton radioButton_normal;
        private System.Windows.Forms.RadioButton radioButton_volver;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBox_msaje;
    }
}