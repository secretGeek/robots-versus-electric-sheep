namespace nimble_life
{
    partial class MainForm
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.lblGeneration = new System.Windows.Forms.Label();
            this.lblStuff = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblMaxGenerations = new System.Windows.Forms.Label();
            this.lblMinGenerations = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.lblMinGenerations);
            this.splitContainerMain.Panel1.Controls.Add(this.lblMaxGenerations);
            this.splitContainerMain.Panel1.Controls.Add(this.lblGeneration);
            this.splitContainerMain.Panel1.Controls.Add(this.lblStuff);
            this.splitContainerMain.Panel1.Controls.Add(this.btnGo);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainerMain.Size = new System.Drawing.Size(886, 578);
            this.splitContainerMain.SplitterDistance = 47;
            this.splitContainerMain.TabIndex = 0;
            // 
            // lblGeneration
            // 
            this.lblGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneration.AutoSize = true;
            this.lblGeneration.Location = new System.Drawing.Point(767, 9);
            this.lblGeneration.Name = "lblGeneration";
            this.lblGeneration.Size = new System.Drawing.Size(13, 13);
            this.lblGeneration.TabIndex = 2;
            this.lblGeneration.Text = "0";
            // 
            // lblStuff
            // 
            this.lblStuff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStuff.AutoSize = true;
            this.lblStuff.Location = new System.Drawing.Point(861, 9);
            this.lblStuff.Name = "lblStuff";
            this.lblStuff.Size = new System.Drawing.Size(13, 13);
            this.lblStuff.TabIndex = 1;
            this.lblStuff.Text = "0";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(12, 12);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblMaxGenerations
            // 
            this.lblMaxGenerations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxGenerations.AutoSize = true;
            this.lblMaxGenerations.Location = new System.Drawing.Point(93, 17);
            this.lblMaxGenerations.Name = "lblMaxGenerations";
            this.lblMaxGenerations.Size = new System.Drawing.Size(13, 13);
            this.lblMaxGenerations.TabIndex = 3;
            this.lblMaxGenerations.Text = "0";
            // 
            // lblMinGenerations
            // 
            this.lblMinGenerations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMinGenerations.AutoSize = true;
            this.lblMinGenerations.Location = new System.Drawing.Point(162, 17);
            this.lblMinGenerations.Name = "lblMinGenerations";
            this.lblMinGenerations.Size = new System.Drawing.Size(13, 13);
            this.lblMinGenerations.TabIndex = 4;
            this.lblMinGenerations.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 578);
            this.Controls.Add(this.splitContainerMain);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Animal Simulator";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblStuff;
        private System.Windows.Forms.Label lblGeneration;
        private System.Windows.Forms.Label lblMaxGenerations;
        private System.Windows.Forms.Label lblMinGenerations;
    }
}

