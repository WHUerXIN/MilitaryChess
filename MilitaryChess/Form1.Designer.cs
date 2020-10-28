namespace WarChess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.RePlayBtn = new System.Windows.Forms.Button();
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.TalkTextBox = new System.Windows.Forms.TextBox();
            this.SendTextBox = new System.Windows.Forms.TextBox();
            this.MyServerBtn = new System.Windows.Forms.Button();
            this.IPAddressTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MyClientBtn = new System.Windows.Forms.Button();
            this.SendMessageBtn = new System.Windows.Forms.Button();
            this.AboutBtn = new System.Windows.Forms.Button();
            this.WebPlayBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.IPOfMineBox = new System.Windows.Forms.TextBox();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SaveMapBtn = new System.Windows.Forms.Button();
            this.WeChat = new System.Windows.Forms.Label();
            this.ReadMapBtn = new System.Windows.Forms.Button();
            this.LocalPlayBtn = new System.Windows.Forms.Button();
            this.AIPlayBtn = new System.Windows.Forms.Button();
            this.SettingBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            this.SuspendLayout();
            // 
            // RePlayBtn
            // 
            this.RePlayBtn.Enabled = false;
            this.RePlayBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RePlayBtn.Location = new System.Drawing.Point(785, 751);
            this.RePlayBtn.Name = "RePlayBtn";
            this.RePlayBtn.Size = new System.Drawing.Size(275, 32);
            this.RePlayBtn.TabIndex = 1;
            this.RePlayBtn.Text = "重  新  开  始";
            this.RePlayBtn.UseVisualStyleBackColor = true;
            this.RePlayBtn.Click += new System.EventHandler(this.RePlayBtn_Click);
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxMap.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMap.Image")));
            this.pictureBoxMap.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(769, 1070);
            this.pictureBoxMap.TabIndex = 0;
            this.pictureBoxMap.TabStop = false;
            // 
            // TalkTextBox
            // 
            this.TalkTextBox.Enabled = false;
            this.TalkTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TalkTextBox.Location = new System.Drawing.Point(785, 41);
            this.TalkTextBox.Multiline = true;
            this.TalkTextBox.Name = "TalkTextBox";
            this.TalkTextBox.ReadOnly = true;
            this.TalkTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TalkTextBox.Size = new System.Drawing.Size(285, 292);
            this.TalkTextBox.TabIndex = 2;
            // 
            // SendTextBox
            // 
            this.SendTextBox.Enabled = false;
            this.SendTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SendTextBox.Location = new System.Drawing.Point(785, 340);
            this.SendTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.SendTextBox.Multiline = true;
            this.SendTextBox.Name = "SendTextBox";
            this.SendTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SendTextBox.Size = new System.Drawing.Size(285, 64);
            this.SendTextBox.TabIndex = 8;
            // 
            // MyServerBtn
            // 
            this.MyServerBtn.Enabled = false;
            this.MyServerBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MyServerBtn.Location = new System.Drawing.Point(785, 532);
            this.MyServerBtn.Name = "MyServerBtn";
            this.MyServerBtn.Size = new System.Drawing.Size(130, 32);
            this.MyServerBtn.TabIndex = 9;
            this.MyServerBtn.Text = "监    听";
            this.MyServerBtn.UseVisualStyleBackColor = true;
            this.MyServerBtn.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // IPAddressTextBox
            // 
            this.IPAddressTextBox.Enabled = false;
            this.IPAddressTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IPAddressTextBox.Location = new System.Drawing.Point(841, 501);
            this.IPAddressTextBox.Name = "IPAddressTextBox";
            this.IPAddressTextBox.Size = new System.Drawing.Size(219, 27);
            this.IPAddressTextBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(782, 504);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "对方IP";
            // 
            // MyClientBtn
            // 
            this.MyClientBtn.Enabled = false;
            this.MyClientBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MyClientBtn.Location = new System.Drawing.Point(930, 532);
            this.MyClientBtn.Name = "MyClientBtn";
            this.MyClientBtn.Size = new System.Drawing.Size(130, 32);
            this.MyClientBtn.TabIndex = 12;
            this.MyClientBtn.Text = "连    接";
            this.MyClientBtn.UseVisualStyleBackColor = true;
            this.MyClientBtn.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // SendMessageBtn
            // 
            this.SendMessageBtn.Enabled = false;
            this.SendMessageBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SendMessageBtn.Location = new System.Drawing.Point(785, 420);
            this.SendMessageBtn.Name = "SendMessageBtn";
            this.SendMessageBtn.Size = new System.Drawing.Size(285, 30);
            this.SendMessageBtn.TabIndex = 13;
            this.SendMessageBtn.Text = "发        送";
            this.SendMessageBtn.UseVisualStyleBackColor = true;
            this.SendMessageBtn.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // AboutBtn
            // 
            this.AboutBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AboutBtn.Location = new System.Drawing.Point(785, 977);
            this.AboutBtn.Name = "AboutBtn";
            this.AboutBtn.Size = new System.Drawing.Size(275, 34);
            this.AboutBtn.TabIndex = 14;
            this.AboutBtn.Text = "关于";
            this.AboutBtn.UseVisualStyleBackColor = true;
            this.AboutBtn.Click += new System.EventHandler(this.AboutBtn_Click);
            // 
            // WebPlayBtn
            // 
            this.WebPlayBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WebPlayBtn.Location = new System.Drawing.Point(785, 677);
            this.WebPlayBtn.Name = "WebPlayBtn";
            this.WebPlayBtn.Size = new System.Drawing.Size(275, 34);
            this.WebPlayBtn.TabIndex = 15;
            this.WebPlayBtn.Text = "网  络  对  战";
            this.WebPlayBtn.UseVisualStyleBackColor = true;
            this.WebPlayBtn.Click += new System.EventHandler(this.WebPlayBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(782, 474);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "本地IP";
            // 
            // IPOfMineBox
            // 
            this.IPOfMineBox.Enabled = false;
            this.IPOfMineBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IPOfMineBox.Location = new System.Drawing.Point(841, 470);
            this.IPOfMineBox.Name = "IPOfMineBox";
            this.IPOfMineBox.ReadOnly = true;
            this.IPOfMineBox.Size = new System.Drawing.Size(219, 27);
            this.IPOfMineBox.TabIndex = 17;
            // 
            // HelpBtn
            // 
            this.HelpBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HelpBtn.Location = new System.Drawing.Point(785, 937);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(275, 34);
            this.HelpBtn.TabIndex = 18;
            this.HelpBtn.Text = "帮助";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // SaveMapBtn
            // 
            this.SaveMapBtn.Enabled = false;
            this.SaveMapBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveMapBtn.Location = new System.Drawing.Point(785, 800);
            this.SaveMapBtn.Name = "SaveMapBtn";
            this.SaveMapBtn.Size = new System.Drawing.Size(130, 32);
            this.SaveMapBtn.TabIndex = 19;
            this.SaveMapBtn.Text = "保 存 残 局";
            this.SaveMapBtn.UseVisualStyleBackColor = true;
            this.SaveMapBtn.Click += new System.EventHandler(this.SaveMapBtn_Click);
            // 
            // WeChat
            // 
            this.WeChat.AutoSize = true;
            this.WeChat.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WeChat.Location = new System.Drawing.Point(883, 9);
            this.WeChat.Name = "WeChat";
            this.WeChat.Size = new System.Drawing.Size(72, 27);
            this.WeChat.TabIndex = 20;
            this.WeChat.Text = "聊天区";
            // 
            // ReadMapBtn
            // 
            this.ReadMapBtn.Enabled = false;
            this.ReadMapBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ReadMapBtn.Location = new System.Drawing.Point(930, 800);
            this.ReadMapBtn.Name = "ReadMapBtn";
            this.ReadMapBtn.Size = new System.Drawing.Size(130, 32);
            this.ReadMapBtn.TabIndex = 21;
            this.ReadMapBtn.Text = "读 取 残 局";
            this.ReadMapBtn.UseVisualStyleBackColor = true;
            this.ReadMapBtn.Click += new System.EventHandler(this.ReadMapBtn_Click);
            // 
            // LocalPlayBtn
            // 
            this.LocalPlayBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LocalPlayBtn.Location = new System.Drawing.Point(785, 594);
            this.LocalPlayBtn.Name = "LocalPlayBtn";
            this.LocalPlayBtn.Size = new System.Drawing.Size(275, 34);
            this.LocalPlayBtn.TabIndex = 22;
            this.LocalPlayBtn.Text = "本  地  对  战";
            this.LocalPlayBtn.UseVisualStyleBackColor = true;
            this.LocalPlayBtn.Click += new System.EventHandler(this.LocalPlayBtn_Click);
            // 
            // AIPlayBtn
            // 
            this.AIPlayBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AIPlayBtn.Location = new System.Drawing.Point(785, 637);
            this.AIPlayBtn.Name = "AIPlayBtn";
            this.AIPlayBtn.Size = new System.Drawing.Size(275, 34);
            this.AIPlayBtn.TabIndex = 23;
            this.AIPlayBtn.Text = "人  机  对  战";
            this.AIPlayBtn.UseVisualStyleBackColor = true;
            this.AIPlayBtn.Click += new System.EventHandler(this.AIPlayBtn_Click);
            // 
            // SettingBtn
            // 
            this.SettingBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SettingBtn.Location = new System.Drawing.Point(785, 899);
            this.SettingBtn.Name = "SettingBtn";
            this.SettingBtn.Size = new System.Drawing.Size(275, 32);
            this.SettingBtn.TabIndex = 25;
            this.SettingBtn.Text = "设置";
            this.SettingBtn.UseVisualStyleBackColor = true;
            this.SettingBtn.Click += new System.EventHandler(this.SettingBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 1023);
            this.Controls.Add(this.SettingBtn);
            this.Controls.Add(this.AIPlayBtn);
            this.Controls.Add(this.LocalPlayBtn);
            this.Controls.Add(this.ReadMapBtn);
            this.Controls.Add(this.WeChat);
            this.Controls.Add(this.SaveMapBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.IPOfMineBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WebPlayBtn);
            this.Controls.Add(this.AboutBtn);
            this.Controls.Add(this.SendMessageBtn);
            this.Controls.Add(this.MyClientBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IPAddressTextBox);
            this.Controls.Add(this.MyServerBtn);
            this.Controls.Add(this.SendTextBox);
            this.Controls.Add(this.TalkTextBox);
            this.Controls.Add(this.RePlayBtn);
            this.Controls.Add(this.pictureBoxMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "WarChess";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button RePlayBtn;
        private System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.TextBox TalkTextBox;
        private System.Windows.Forms.TextBox SendTextBox;
        private System.Windows.Forms.Button MyServerBtn;
        private System.Windows.Forms.TextBox IPAddressTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MyClientBtn;
        private System.Windows.Forms.Button SendMessageBtn;
        private System.Windows.Forms.Button AboutBtn;
        private System.Windows.Forms.Button WebPlayBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IPOfMineBox;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button SaveMapBtn;
        private System.Windows.Forms.Label WeChat;
        private System.Windows.Forms.Button ReadMapBtn;
        private System.Windows.Forms.Button LocalPlayBtn;
        private System.Windows.Forms.Button AIPlayBtn;
        private System.Windows.Forms.Button SettingBtn;
    }
}

