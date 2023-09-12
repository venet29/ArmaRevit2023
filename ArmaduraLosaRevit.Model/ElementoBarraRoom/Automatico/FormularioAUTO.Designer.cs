namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Automatico
{
    partial class FormularioAUTO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormularioAUTO));
            this.comboBox_F1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonaceptar = new System.Windows.Forms.Button();
            this.button_cerrar = new System.Windows.Forms.Button();
            this.textBox_F3 = new System.Windows.Forms.TextBox();
            this.textBox_F4 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_largoRecorrido = new System.Windows.Forms.TextBox();
            this.textBox_largoBarra = new System.Windows.Forms.TextBox();
            this.checkBoxIsAhorro = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_verificarEspesor = new System.Windows.Forms.CheckBox();
            this.comboBox_tipoF1 = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_solocopiardatos = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_F1
            // 
            this.comboBox_F1.Enabled = false;
            this.comboBox_F1.FormattingEnabled = true;
            this.comboBox_F1.Items.AddRange(new object[] {
            "F20",
            "F21"});
            this.comboBox_F1.Location = new System.Drawing.Point(77, 19);
            this.comboBox_F1.Name = "comboBox_F1";
            this.comboBox_F1.Size = new System.Drawing.Size(47, 21);
            this.comboBox_F1.TabIndex = 0;
            this.comboBox_F1.Text = "F20";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Largo Barra [cm]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "Largo \r\nRecorrido [cm]";
            // 
            // buttonaceptar
            // 
            this.buttonaceptar.Location = new System.Drawing.Point(82, 268);
            this.buttonaceptar.Name = "buttonaceptar";
            this.buttonaceptar.Size = new System.Drawing.Size(75, 23);
            this.buttonaceptar.TabIndex = 4;
            this.buttonaceptar.Text = "Aceptar";
            this.buttonaceptar.UseVisualStyleBackColor = true;
            this.buttonaceptar.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_cerrar
            // 
            this.button_cerrar.Location = new System.Drawing.Point(212, 268);
            this.button_cerrar.Name = "button_cerrar";
            this.button_cerrar.Size = new System.Drawing.Size(75, 23);
            this.button_cerrar.TabIndex = 5;
            this.button_cerrar.Text = "Cerrar";
            this.button_cerrar.UseVisualStyleBackColor = true;
            this.button_cerrar.Click += new System.EventHandler(this.button_cerrar_Click);
            // 
            // textBox_F3
            // 
            this.textBox_F3.Enabled = false;
            this.textBox_F3.Location = new System.Drawing.Point(77, 49);
            this.textBox_F3.Name = "textBox_F3";
            this.textBox_F3.ReadOnly = true;
            this.textBox_F3.Size = new System.Drawing.Size(47, 20);
            this.textBox_F3.TabIndex = 6;
            this.textBox_F3.Text = "F16";
            // 
            // textBox_F4
            // 
            this.textBox_F4.Enabled = false;
            this.textBox_F4.Location = new System.Drawing.Point(77, 78);
            this.textBox_F4.Name = "textBox_F4";
            this.textBox_F4.ReadOnly = true;
            this.textBox_F4.Size = new System.Drawing.Size(47, 20);
            this.textBox_F4.TabIndex = 7;
            this.textBox_F4.Text = "F19";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox_F1);
            this.groupBox1.Controls.Add(this.textBox_F4);
            this.groupBox1.Controls.Add(this.textBox_F3);
            this.groupBox1.Location = new System.Drawing.Point(182, 142);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 110);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipos";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "F4 por";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "F3 por";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "F1  por";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_largoRecorrido);
            this.groupBox2.Controls.Add(this.textBox_largoBarra);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(30, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(146, 110);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dimensiones";
            // 
            // textBox_largoRecorrido
            // 
            this.textBox_largoRecorrido.Enabled = false;
            this.textBox_largoRecorrido.Location = new System.Drawing.Point(100, 73);
            this.textBox_largoRecorrido.Name = "textBox_largoRecorrido";
            this.textBox_largoRecorrido.Size = new System.Drawing.Size(40, 20);
            this.textBox_largoRecorrido.TabIndex = 5;
            this.textBox_largoRecorrido.Text = "200";
            this.textBox_largoRecorrido.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_largoRecorrido_KeyPress);
            // 
            // textBox_largoBarra
            // 
            this.textBox_largoBarra.Enabled = false;
            this.textBox_largoBarra.Location = new System.Drawing.Point(100, 33);
            this.textBox_largoBarra.Name = "textBox_largoBarra";
            this.textBox_largoBarra.Size = new System.Drawing.Size(40, 20);
            this.textBox_largoBarra.TabIndex = 4;
            this.textBox_largoBarra.Text = "400";
            this.textBox_largoBarra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_largoBarra_KeyPress);
            // 
            // checkBoxIsAhorro
            // 
            this.checkBoxIsAhorro.AutoSize = true;
            this.checkBoxIsAhorro.Location = new System.Drawing.Point(30, 114);
            this.checkBoxIsAhorro.Name = "checkBoxIsAhorro";
            this.checkBoxIsAhorro.Size = new System.Drawing.Size(109, 17);
            this.checkBoxIsAhorro.TabIndex = 10;
            this.checkBoxIsAhorro.Text = "Considerar ahorro";
            this.checkBoxIsAhorro.UseVisualStyleBackColor = true;
            this.checkBoxIsAhorro.CheckedChanged += new System.EventHandler(this.checkBoxIsAhorro_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.checkBox_verificarEspesor);
            this.groupBox3.Controls.Add(this.comboBox_tipoF1);
            this.groupBox3.Location = new System.Drawing.Point(29, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(309, 70);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Datos Diseño";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Tipo Pata";
            // 
            // checkBox_verificarEspesor
            // 
            this.checkBox_verificarEspesor.AutoSize = true;
            this.checkBox_verificarEspesor.Location = new System.Drawing.Point(165, 25);
            this.checkBox_verificarEspesor.Name = "checkBox_verificarEspesor";
            this.checkBox_verificarEspesor.Size = new System.Drawing.Size(117, 30);
            this.checkBox_verificarEspesor.TabIndex = 1;
            this.checkBox_verificarEspesor.Text = "Verificar si espesor \r\nmenor 15cm";
            this.toolTip1.SetToolTip(this.checkBox_verificarEspesor, "Verificar si espesor es menor 15 cm(ACI 8.7.4.1.3).\r\nSi es menor se utlizan barra" +
        "s F11 o F11A\r\n\r\n");
            this.checkBox_verificarEspesor.UseVisualStyleBackColor = true;
            // 
            // comboBox_tipoF1
            // 
            this.comboBox_tipoF1.FormattingEnabled = true;
            this.comboBox_tipoF1.Items.AddRange(new object[] {
            "F1",
            "F3"});
            this.comboBox_tipoF1.Location = new System.Drawing.Point(80, 29);
            this.comboBox_tipoF1.Name = "comboBox_tipoF1";
            this.comboBox_tipoF1.Size = new System.Drawing.Size(40, 21);
            this.comboBox_tipoF1.TabIndex = 0;
            this.comboBox_tipoF1.Text = "F1";
            this.comboBox_tipoF1.SelectedIndexChanged += new System.EventHandler(this.ComboBox_tipoF1_SelectedIndexChanged);
            // 
            // checkBox_solocopiardatos
            // 
            this.checkBox_solocopiardatos.AutoSize = true;
            this.checkBox_solocopiardatos.Location = new System.Drawing.Point(197, 114);
            this.checkBox_solocopiardatos.Name = "checkBox_solocopiardatos";
            this.checkBox_solocopiardatos.Size = new System.Drawing.Size(117, 17);
            this.checkBox_solocopiardatos.TabIndex = 13;
            this.checkBox_solocopiardatos.Text = "Solo Copiar Datos?";
            this.toolTip1.SetToolTip(this.checkBox_solocopiardatos, "Solo copia los datos de cuantias horizontal vertical  y direcciones principales.\r" +
        "\nNO DIBUJA BARRAS.\r\n");
            this.checkBox_solocopiardatos.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(470, 296);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(168, 139);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // FormularioAUTO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(367, 322);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox_solocopiardatos);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.checkBoxIsAhorro);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cerrar);
            this.Controls.Add(this.buttonaceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormularioAUTO";
            this.Text = "Armadura Inferior";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_F1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonaceptar;
        private System.Windows.Forms.Button button_cerrar;
        private System.Windows.Forms.TextBox textBox_F3;
        private System.Windows.Forms.TextBox textBox_F4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_largoRecorrido;
        private System.Windows.Forms.TextBox textBox_largoBarra;
        private System.Windows.Forms.CheckBox checkBoxIsAhorro;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_verificarEspesor;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBox_tipoF1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox_solocopiardatos;
    }
}