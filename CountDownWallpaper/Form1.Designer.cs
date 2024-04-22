
namespace CountDownWallpaper
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.setTargetTimeBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.hourDomainUpDown = new System.Windows.Forms.DomainUpDown();
            this.minuteDomainUpDown = new System.Windows.Forms.DomainUpDown();
            this.secondDomainUpDown = new System.Windows.Forms.DomainUpDown();
            this.SuspendLayout();
            // 
            // setTargetTimeBtn
            // 
            this.setTargetTimeBtn.Location = new System.Drawing.Point(171, 56);
            this.setTargetTimeBtn.Name = "setTargetTimeBtn";
            this.setTargetTimeBtn.Size = new System.Drawing.Size(91, 26);
            this.setTargetTimeBtn.TabIndex = 0;
            this.setTargetTimeBtn.Text = "SetTargetTime";
            this.setTargetTimeBtn.UseVisualStyleBackColor = true;
            this.setTargetTimeBtn.Click += new System.EventHandler(this.setTargetTimeBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dateDateTimePicker
            // 
            this.dateDateTimePicker.CustomFormat = "";
            this.dateDateTimePicker.Location = new System.Drawing.Point(31, 29);
            this.dateDateTimePicker.Name = "dateDateTimePicker";
            this.dateDateTimePicker.Size = new System.Drawing.Size(120, 21);
            this.dateDateTimePicker.TabIndex = 6;
            this.dateDateTimePicker.Enter += new System.EventHandler(this.dateDateTimePicker_Enter);
            // 
            // hourDomainUpDown
            // 
            this.hourDomainUpDown.Location = new System.Drawing.Point(31, 61);
            this.hourDomainUpDown.Name = "hourDomainUpDown";
            this.hourDomainUpDown.Size = new System.Drawing.Size(36, 21);
            this.hourDomainUpDown.TabIndex = 7;
            this.hourDomainUpDown.Text = "00";
            this.hourDomainUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hourDomainUpDown.Enter += new System.EventHandler(this.hourDomainUpDown_Enter);
            this.hourDomainUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.hourDomainUpDown_KeyPress);
            // 
            // minuteDomainUpDown
            // 
            this.minuteDomainUpDown.Location = new System.Drawing.Point(73, 61);
            this.minuteDomainUpDown.Name = "minuteDomainUpDown";
            this.minuteDomainUpDown.Size = new System.Drawing.Size(36, 21);
            this.minuteDomainUpDown.TabIndex = 8;
            this.minuteDomainUpDown.Text = "00";
            this.minuteDomainUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.minuteDomainUpDown.Enter += new System.EventHandler(this.minuteDomainUpDown_Enter);
            this.minuteDomainUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.minuteDomainUpDown_KeyPress);
            // 
            // secondDomainUpDown
            // 
            this.secondDomainUpDown.Location = new System.Drawing.Point(115, 61);
            this.secondDomainUpDown.Name = "secondDomainUpDown";
            this.secondDomainUpDown.Size = new System.Drawing.Size(36, 21);
            this.secondDomainUpDown.TabIndex = 9;
            this.secondDomainUpDown.Text = "00";
            this.secondDomainUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.secondDomainUpDown.Enter += new System.EventHandler(this.secondDomainUpDown_Enter);
            this.secondDomainUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.secondDomainUpDown_KeyPress);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 145);
            this.Controls.Add(this.secondDomainUpDown);
            this.Controls.Add(this.minuteDomainUpDown);
            this.Controls.Add(this.hourDomainUpDown);
            this.Controls.Add(this.dateDateTimePicker);
            this.Controls.Add(this.setTargetTimeBtn);
            this.Name = "Form1";
            this.Text = "Countdown Wallpaper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button setTargetTimeBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DateTimePicker dateDateTimePicker;
        private System.Windows.Forms.DomainUpDown hourDomainUpDown;
        private System.Windows.Forms.DomainUpDown minuteDomainUpDown;
        private System.Windows.Forms.DomainUpDown secondDomainUpDown;
    }
}

