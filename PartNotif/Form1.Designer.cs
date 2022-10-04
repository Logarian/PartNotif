
namespace PartNotif
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.createDeclar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmDB = new System.Windows.Forms.ComboBox();
            this.cmWave = new System.Windows.Forms.ComboBox();
            this.cmGov = new System.Windows.Forms.ComboBox();
            this.cmSch = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cmExamDate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmExamType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // createDeclar
            // 
            this.createDeclar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.createDeclar.Location = new System.Drawing.Point(28, 429);
            this.createDeclar.Name = "createDeclar";
            this.createDeclar.Size = new System.Drawing.Size(251, 44);
            this.createDeclar.TabIndex = 0;
            this.createDeclar.Text = "Создать уведомления";
            this.createDeclar.UseVisualStyleBackColor = true;
            this.createDeclar.Click += new System.EventHandler(this.createDeclar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(25, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "БД:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(25, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Этап:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(25, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "МСУ:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(25, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "ОО:";
            // 
            // cmDB
            // 
            this.cmDB.FormattingEnabled = true;
            this.cmDB.Location = new System.Drawing.Point(28, 40);
            this.cmDB.Name = "cmDB";
            this.cmDB.Size = new System.Drawing.Size(251, 21);
            this.cmDB.TabIndex = 5;
            this.cmDB.SelectedIndexChanged += new System.EventHandler(this.cmDB_SelectedIndexChanged);
            // 
            // cmWave
            // 
            this.cmWave.FormattingEnabled = true;
            this.cmWave.Location = new System.Drawing.Point(28, 93);
            this.cmWave.Name = "cmWave";
            this.cmWave.Size = new System.Drawing.Size(251, 21);
            this.cmWave.TabIndex = 6;
            this.cmWave.SelectedIndexChanged += new System.EventHandler(this.cmWave_SelectedIndexChanged);
            // 
            // cmGov
            // 
            this.cmGov.FormattingEnabled = true;
            this.cmGov.Location = new System.Drawing.Point(28, 262);
            this.cmGov.Name = "cmGov";
            this.cmGov.Size = new System.Drawing.Size(251, 21);
            this.cmGov.TabIndex = 7;
            this.cmGov.SelectedIndexChanged += new System.EventHandler(this.cmGov_SelectedIndexChanged);
            // 
            // cmSch
            // 
            this.cmSch.FormattingEnabled = true;
            this.cmSch.Location = new System.Drawing.Point(28, 319);
            this.cmSch.Name = "cmSch";
            this.cmSch.Size = new System.Drawing.Size(251, 21);
            this.cmSch.TabIndex = 8;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(28, 370);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(251, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // cmExamDate
            // 
            this.cmExamDate.FormattingEnabled = true;
            this.cmExamDate.Location = new System.Drawing.Point(28, 147);
            this.cmExamDate.Name = "cmExamDate";
            this.cmExamDate.Size = new System.Drawing.Size(251, 21);
            this.cmExamDate.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(25, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "Дата:";
            // 
            // cmExamType
            // 
            this.cmExamType.FormattingEnabled = true;
            this.cmExamType.Location = new System.Drawing.Point(28, 204);
            this.cmExamType.Name = "cmExamType";
            this.cmExamType.Size = new System.Drawing.Size(251, 21);
            this.cmExamType.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(25, 185);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Дни (0 - осн; 1 - рез):";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 492);
            this.Controls.Add(this.cmExamType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmExamDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cmSch);
            this.Controls.Add(this.cmGov);
            this.Controls.Add(this.cmWave);
            this.Controls.Add(this.cmDB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.createDeclar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createDeclar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmDB;
        private System.Windows.Forms.ComboBox cmWave;
        private System.Windows.Forms.ComboBox cmGov;
        private System.Windows.Forms.ComboBox cmSch;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox cmExamDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmExamType;
        private System.Windows.Forms.Label label6;
    }
}

