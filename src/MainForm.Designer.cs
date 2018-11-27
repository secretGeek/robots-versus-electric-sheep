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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.pnlPopulationGraph = new System.Windows.Forms.Panel();
            this.lblRobotCount = new System.Windows.Forms.Label();
            this.lblSheepCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMinGenerations = new System.Windows.Forms.Label();
            this.lblMaxGenerations = new System.Windows.Forms.Label();
            this.lblGeneration = new System.Windows.Forms.Label();
            this.lblStuff = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
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
            this.splitContainerMain.Panel1.Controls.Add(this.pnlPopulationGraph);
            this.splitContainerMain.Panel1.Controls.Add(this.lblRobotCount);
            this.splitContainerMain.Panel1.Controls.Add(this.lblSheepCount);
            this.splitContainerMain.Panel1.Controls.Add(this.label2);
            this.splitContainerMain.Panel1.Controls.Add(this.label1);
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
            // pnlPopulationGraph
            // 
            this.pnlPopulationGraph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPopulationGraph.Location = new System.Drawing.Point(183, 3);
            this.pnlPopulationGraph.Name = "pnlPopulationGraph";
            this.pnlPopulationGraph.Size = new System.Drawing.Size(700, 41);
            this.pnlPopulationGraph.TabIndex = 9;
            this.pnlPopulationGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPopulationGraph_Paint);
            // 
            // lblRobotCount
            // 
            this.lblRobotCount.AutoSize = true;
            this.lblRobotCount.ForeColor = System.Drawing.Color.Blue;
            this.lblRobotCount.Location = new System.Drawing.Point(153, 4);
            this.lblRobotCount.Name = "lblRobotCount";
            this.lblRobotCount.Size = new System.Drawing.Size(13, 13);
            this.lblRobotCount.TabIndex = 8;
            this.lblRobotCount.Text = "0";
            // 
            // lblSheepCount
            // 
            this.lblSheepCount.AutoSize = true;
            this.lblSheepCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblSheepCount.Location = new System.Drawing.Point(152, 30);
            this.lblSheepCount.Name = "lblSheepCount";
            this.lblSheepCount.Size = new System.Drawing.Size(13, 13);
            this.lblSheepCount.TabIndex = 7;
            this.lblSheepCount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(106, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sheep";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(106, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Robots";
            // 
            // lblMinGenerations
            // 
            this.lblMinGenerations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMinGenerations.AutoSize = true;
            this.lblMinGenerations.Location = new System.Drawing.Point(796, 17);
            this.lblMinGenerations.Name = "lblMinGenerations";
            this.lblMinGenerations.Size = new System.Drawing.Size(13, 13);
            this.lblMinGenerations.TabIndex = 4;
            this.lblMinGenerations.Text = "0";
            this.lblMinGenerations.Visible = false;
            // 
            // lblMaxGenerations
            // 
            this.lblMaxGenerations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxGenerations.AutoSize = true;
            this.lblMaxGenerations.Location = new System.Drawing.Point(768, 17);
            this.lblMaxGenerations.Name = "lblMaxGenerations";
            this.lblMaxGenerations.Size = new System.Drawing.Size(13, 13);
            this.lblMaxGenerations.TabIndex = 3;
            this.lblMaxGenerations.Text = "0";
            this.lblMaxGenerations.Visible = false;
            // 
            // lblGeneration
            // 
            this.lblGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneration.AutoSize = true;
            this.lblGeneration.Location = new System.Drawing.Point(829, 17);
            this.lblGeneration.Name = "lblGeneration";
            this.lblGeneration.Size = new System.Drawing.Size(13, 13);
            this.lblGeneration.TabIndex = 2;
            this.lblGeneration.Text = "0";
            this.lblGeneration.Visible = false;
            // 
            // lblStuff
            // 
            this.lblStuff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStuff.AutoSize = true;
            this.lblStuff.Location = new System.Drawing.Point(861, 17);
            this.lblStuff.Name = "lblStuff";
            this.lblStuff.Size = new System.Drawing.Size(13, 13);
            this.lblStuff.TabIndex = 1;
            this.lblStuff.Text = "0";
            this.lblStuff.Visible = false;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(12, 3);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 41);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 578);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Robots versus Electric Sheep";
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSheepCount;
        private System.Windows.Forms.Label lblRobotCount;
        private System.Windows.Forms.Panel pnlPopulationGraph;
    }
}

