namespace ArmaduraLosaRevit.Model.BarraV.Copiar.wpf
{
    partial class CopiarRebarWpf
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
            this.listView_allLevel = new System.Windows.Forms.ListView();
            this.button_aceptar = new System.Windows.Forms.Button();
            this.button_cerrar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_visualizar = new System.Windows.Forms.Button();
            this.button_ocultar = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_allLevel
            // 
            this.listView_allLevel.CheckBoxes = true;
            this.listView_allLevel.HideSelection = false;
            this.listView_allLevel.Location = new System.Drawing.Point(36, 120);
            this.listView_allLevel.Name = "listView_allLevel";
            this.listView_allLevel.Size = new System.Drawing.Size(285, 456);
            this.listView_allLevel.TabIndex = 0;
            this.listView_allLevel.UseCompatibleStateImageBehavior = false;
            this.listView_allLevel.View = System.Windows.Forms.View.Details;
            // 
            // button_aceptar
            // 
            this.button_aceptar.Location = new System.Drawing.Point(70, 611);
            this.button_aceptar.Name = "button_aceptar";
            this.button_aceptar.Size = new System.Drawing.Size(75, 23);
            this.button_aceptar.TabIndex = 1;
            this.button_aceptar.Text = "Copiar";
            this.toolTip1.SetToolTip(this.button_aceptar, "Copia las vigas idem seleccionadas\r\n");
            this.button_aceptar.UseVisualStyleBackColor = true;
            this.button_aceptar.Click += new System.EventHandler(this.button_aceptar_Click);
            // 
            // button_cerrar
            // 
            this.button_cerrar.Location = new System.Drawing.Point(207, 611);
            this.button_cerrar.Name = "button_cerrar";
            this.button_cerrar.Size = new System.Drawing.Size(75, 23);
            this.button_cerrar.TabIndex = 2;
            this.button_cerrar.Text = "Cerrar";
            this.button_cerrar.UseVisualStyleBackColor = true;
            this.button_cerrar.Click += new System.EventHandler(this.button_cerrar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_ocultar);
            this.groupBox1.Controls.Add(this.button_visualizar);
            this.groupBox1.Location = new System.Drawing.Point(36, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 69);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Accion";
            // 
            // button_visualizar
            // 
            this.button_visualizar.Location = new System.Drawing.Point(50, 28);
            this.button_visualizar.Name = "button_visualizar";
            this.button_visualizar.Size = new System.Drawing.Size(75, 23);
            this.button_visualizar.TabIndex = 0;
            this.button_visualizar.Text = "DesOcultar";
            this.toolTip1.SetToolTip(this.button_visualizar, "\r\n");
            this.button_visualizar.UseVisualStyleBackColor = true;
            this.button_visualizar.Click += new System.EventHandler(this.button_visualizar_Click);
            // 
            // button_ocultar
            // 
            this.button_ocultar.Location = new System.Drawing.Point(171, 28);
            this.button_ocultar.Name = "button_ocultar";
            this.button_ocultar.Size = new System.Drawing.Size(75, 23);
            this.button_ocultar.TabIndex = 1;
            this.button_ocultar.Text = "Ocultar";
            this.toolTip1.SetToolTip(this.button_ocultar, "Ocultar Barras copiadas de vigas idem");
            this.button_ocultar.UseVisualStyleBackColor = true;
            this.button_ocultar.Click += new System.EventHandler(this.button_ocultar_Click);
            // 
            // CopiarRebarWpf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 661);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cerrar);
            this.Controls.Add(this.button_aceptar);
            this.Controls.Add(this.listView_allLevel);
            this.Name = "CopiarRebarWpf";
            this.Text = "CopiarRebarWpf";
            this.Load += new System.EventHandler(this.CopiarRebarWpf_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_allLevel;
        private System.Windows.Forms.Button button_aceptar;
        private System.Windows.Forms.Button button_cerrar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_ocultar;
        private System.Windows.Forms.Button button_visualizar;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}