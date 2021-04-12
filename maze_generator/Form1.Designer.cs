namespace maze_generator
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnConstant = new System.Windows.Forms.Button();
            this.btnAnimacja = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.labelPredkosc = new System.Windows.Forms.Label();
            this.timerDijkstra = new System.Windows.Forms.Timer(this.components);
            this.timerDFS = new System.Windows.Forms.Timer(this.components);
            this.timerBFS = new System.Windows.Forms.Timer(this.components);
            this.labelAlgorytm = new System.Windows.Forms.Label();
            this.trackBarMaze = new System.Windows.Forms.TrackBar();
            this.labelRozmiar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaze)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox1.Location = new System.Drawing.Point(34, 154);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 800);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(337, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 2;
            // 
            // btnConstant
            // 
            this.btnConstant.Location = new System.Drawing.Point(163, 91);
            this.btnConstant.Name = "btnConstant";
            this.btnConstant.Size = new System.Drawing.Size(111, 32);
            this.btnConstant.TabIndex = 3;
            this.btnConstant.Text = "Stała generacja";
            this.btnConstant.UseVisualStyleBackColor = true;
            this.btnConstant.Click += new System.EventHandler(this.btnConstant_Click);
            // 
            // btnAnimacja
            // 
            this.btnAnimacja.Location = new System.Drawing.Point(34, 91);
            this.btnAnimacja.Name = "btnAnimacja";
            this.btnAnimacja.Size = new System.Drawing.Size(111, 32);
            this.btnAnimacja.TabIndex = 4;
            this.btnAnimacja.Text = "Animacja";
            this.btnAnimacja.UseVisualStyleBackColor = true;
            this.btnAnimacja.Click += new System.EventHandler(this.btnAnimacja_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(723, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 32);
            this.button1.TabIndex = 5;
            this.button1.Text = "Pokaż ścieżkę";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Dijkstra",
            "A*",
            "DFS",
            "BFS"});
            this.comboBox1.Location = new System.Drawing.Point(547, 99);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(147, 24);
            this.comboBox1.TabIndex = 7;
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 100;
            this.trackBar1.Location = new System.Drawing.Point(304, 91);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBar1.Size = new System.Drawing.Size(216, 45);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 8;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.Value = 500;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // labelPredkosc
            // 
            this.labelPredkosc.AutoSize = true;
            this.labelPredkosc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPredkosc.Location = new System.Drawing.Point(310, 68);
            this.labelPredkosc.Name = "labelPredkosc";
            this.labelPredkosc.Size = new System.Drawing.Size(198, 20);
            this.labelPredkosc.TabIndex = 9;
            this.labelPredkosc.Text = "Szybkość animacji: 500 ms";
            // 
            // timerDijkstra
            // 
            this.timerDijkstra.Tick += new System.EventHandler(this.timerDijkstra_Tick);
            // 
            // timerDFS
            // 
            this.timerDFS.Tick += new System.EventHandler(this.timerDFS_Tick);
            // 
            // timerBFS
            // 
            this.timerBFS.Tick += new System.EventHandler(this.timerBFS_Tick);
            // 
            // labelAlgorytm
            // 
            this.labelAlgorytm.AutoSize = true;
            this.labelAlgorytm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAlgorytm.Location = new System.Drawing.Point(612, 68);
            this.labelAlgorytm.Name = "labelAlgorytm";
            this.labelAlgorytm.Size = new System.Drawing.Size(127, 20);
            this.labelAlgorytm.TabIndex = 10;
            this.labelAlgorytm.Text = "Wybór algorytmu";
            // 
            // trackBarMaze
            // 
            this.trackBarMaze.LargeChange = 4;
            this.trackBarMaze.Location = new System.Drawing.Point(35, 43);
            this.trackBarMaze.Maximum = 200;
            this.trackBarMaze.Minimum = 2;
            this.trackBarMaze.Name = "trackBarMaze";
            this.trackBarMaze.Size = new System.Drawing.Size(239, 45);
            this.trackBarMaze.SmallChange = 2;
            this.trackBarMaze.TabIndex = 11;
            this.trackBarMaze.TickFrequency = 10;
            this.trackBarMaze.Value = 80;
            this.trackBarMaze.Scroll += new System.EventHandler(this.trackBarMaze_Scroll);
            // 
            // labelRozmiar
            // 
            this.labelRozmiar.AutoSize = true;
            this.labelRozmiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelRozmiar.Location = new System.Drawing.Point(40, 20);
            this.labelRozmiar.Name = "labelRozmiar";
            this.labelRozmiar.Size = new System.Drawing.Size(135, 20);
            this.labelRozmiar.TabIndex = 12;
            this.labelRozmiar.Text = "Rozmiar labiryntu:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 966);
            this.Controls.Add(this.labelRozmiar);
            this.Controls.Add(this.trackBarMaze);
            this.Controls.Add(this.labelAlgorytm);
            this.Controls.Add(this.labelPredkosc);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAnimacja);
            this.Controls.Add(this.btnConstant);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaze)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConstant;
        private System.Windows.Forms.Button btnAnimacja;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label labelPredkosc;
        private System.Windows.Forms.Timer timerDijkstra;
        private System.Windows.Forms.Timer timerDFS;
        private System.Windows.Forms.Timer timerBFS;
        private System.Windows.Forms.Label labelAlgorytm;
        private System.Windows.Forms.TrackBar trackBarMaze;
        private System.Windows.Forms.Label labelRozmiar;
    }
}

