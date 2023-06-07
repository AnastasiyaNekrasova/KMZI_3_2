
namespace mp5crypt
{
    partial class Cripta_Lab11
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textToEncrypt = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            encryptText = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            encrypt = new System.Windows.Forms.Button();
            TimeLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // textToEncrypt
            // 
            textToEncrypt.Location = new System.Drawing.Point(10, 41);
            textToEncrypt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            textToEncrypt.Name = "textToEncrypt";
            textToEncrypt.Size = new System.Drawing.Size(532, 27);
            textToEncrypt.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(10, 17);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(173, 20);
            label1.TabIndex = 1;
            label1.Text = "Текст для хеширования";
            // 
            // encryptText
            // 
            encryptText.Location = new System.Drawing.Point(10, 106);
            encryptText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            encryptText.Name = "encryptText";
            encryptText.Size = new System.Drawing.Size(532, 27);
            encryptText.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(10, 82);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 20);
            label2.TabIndex = 3;
            label2.Text = "Хеш";
            // 
            // encrypt
            // 
            encrypt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            encrypt.Location = new System.Drawing.Point(174, 194);
            encrypt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            encrypt.Name = "encrypt";
            encrypt.Size = new System.Drawing.Size(177, 38);
            encrypt.TabIndex = 6;
            encrypt.Text = "Получить хеш";
            encrypt.UseVisualStyleBackColor = false;
            encrypt.Click += encrypt_Click;
            // 
            // TimeLabel
            // 
            TimeLabel.AutoSize = true;
            TimeLabel.Location = new System.Drawing.Point(10, 151);
            TimeLabel.Name = "TimeLabel";
            TimeLabel.Size = new System.Drawing.Size(161, 20);
            TimeLabel.TabIndex = 7;
            TimeLabel.Text = "Время выполнения:   ";
            // 
            // Cripta_Lab11
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLight;
            ClientSize = new System.Drawing.Size(550, 256);
            Controls.Add(TimeLabel);
            Controls.Add(encrypt);
            Controls.Add(label2);
            Controls.Add(encryptText);
            Controls.Add(label1);
            Controls.Add(textToEncrypt);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "Cripta_Lab11";
            Text = "Cripta_Lab11";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textToEncrypt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox encryptText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button encrypt;
        private System.Windows.Forms.Label TimeLabel;
    }
}

