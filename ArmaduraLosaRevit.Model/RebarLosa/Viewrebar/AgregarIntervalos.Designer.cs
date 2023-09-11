namespace ArmaduraLosaRevit.Model.RebarLosa.Viewrebar
{
    partial class AgregarIntervalos
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_listaIntervalos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_recorrido = new System.Windows.Forms.Label();
            this.label_largoBarra = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_diam = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 177);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(167, 177);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancelar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Intervalos en [mt]";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // textBox_listaIntervalos
            // 
            this.textBox_listaIntervalos.Location = new System.Drawing.Point(125, 124);
            this.textBox_listaIntervalos.Name = "textBox_listaIntervalos";
            this.textBox_listaIntervalos.Size = new System.Drawing.Size(117, 20);
            this.textBox_listaIntervalos.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Formato ejemplo:  6,12,6";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ancho Recorrido [mt]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Largo barra [mt]";
            this.label4.Click += new System.EventHandler(this.Label4_Click);
            // 
            // label_recorrido
            // 
            this.label_recorrido.AutoSize = true;
            this.label_recorrido.Location = new System.Drawing.Point(144, 39);
            this.label_recorrido.Name = "label_recorrido";
            this.label_recorrido.Size = new System.Drawing.Size(10, 13);
            this.label_recorrido.TabIndex = 7;
            this.label_recorrido.Text = ":";
            // 
            // label_largoBarra
            // 
            this.label_largoBarra.AutoSize = true;
            this.label_largoBarra.Location = new System.Drawing.Point(144, 63);
            this.label_largoBarra.Name = "label_largoBarra";
            this.label_largoBarra.Size = new System.Drawing.Size(10, 13);
            this.label_largoBarra.TabIndex = 8;
            this.label_largoBarra.Text = ":";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_diam);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label_largoBarra);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label_recorrido);
            this.groupBox1.Location = new System.Drawing.Point(24, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 100);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos";
            // 
            // label_diam
            // 
            this.label_diam.AutoSize = true;
            this.label_diam.Location = new System.Drawing.Point(144, 14);
            this.label_diam.Name = "label_diam";
            this.label_diam.Size = new System.Drawing.Size(10, 13);
            this.label_diam.TabIndex = 10;
            this.label_diam.Text = ":";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Diametro [mm]";
            // 
            // AgregarIntervalos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(281, 224);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_listaIntervalos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "AgregarIntervalos";
            this.ShowIcon = false;
            this.Text = "AgregarIntervalos";
            this.Load += new System.EventHandler(this.AgregarIntervalos_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_listaIntervalos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_recorrido;
        private System.Windows.Forms.Label label_largoBarra;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_diam;
        private System.Windows.Forms.Label label5;
    }
}