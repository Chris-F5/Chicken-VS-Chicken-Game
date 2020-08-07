namespace GameServer.GUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BackgroundPannel = new System.Windows.Forms.Panel();
            this.MinimizeBtn = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.TitleBar = new System.Windows.Forms.Panel();
            this.FormTitle = new System.Windows.Forms.Label();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.OutputBox = new System.Windows.Forms.RichTextBox();
            this.BackgroundPannel.SuspendLayout();
            this.TitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // BackgroundPannel
            // 
            this.BackgroundPannel.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundPannel.BackgroundImage = global::GameServer.Properties.Resources.WindowImmage;
            this.BackgroundPannel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BackgroundPannel.Controls.Add(this.MinimizeBtn);
            this.BackgroundPannel.Controls.Add(this.CloseButton);
            this.BackgroundPannel.Controls.Add(this.TitleBar);
            this.BackgroundPannel.Controls.Add(this.SubmitButton);
            this.BackgroundPannel.Controls.Add(this.InputBox);
            this.BackgroundPannel.Controls.Add(this.OutputBox);
            this.BackgroundPannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackgroundPannel.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackgroundPannel.Location = new System.Drawing.Point(0, 0);
            this.BackgroundPannel.Name = "BackgroundPannel";
            this.BackgroundPannel.Size = new System.Drawing.Size(800, 596);
            this.BackgroundPannel.TabIndex = 0;
            // 
            // MinimizeBtn
            // 
            this.MinimizeBtn.BackgroundImage = global::GameServer.Properties.Resources.MinimizeWindow;
            this.MinimizeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MinimizeBtn.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MinimizeBtn.FlatAppearance.BorderSize = 0;
            this.MinimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeBtn.Location = new System.Drawing.Point(746, 4);
            this.MinimizeBtn.Name = "MinimizeBtn";
            this.MinimizeBtn.Size = new System.Drawing.Size(20, 20);
            this.MinimizeBtn.TabIndex = 2;
            this.MinimizeBtn.UseVisualStyleBackColor = true;
            this.MinimizeBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MinimizeBtn_MouseDown);
            this.MinimizeBtn.MouseEnter += new System.EventHandler(this.MinimizeBtn_MouseEnter);
            this.MinimizeBtn.MouseLeave += new System.EventHandler(this.MinimizeBtn_MouseLeave);
            // 
            // CloseButton
            // 
            this.CloseButton.BackgroundImage = global::GameServer.Properties.Resources.CloseWindowCross;
            this.CloseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CloseButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Location = new System.Drawing.Point(772, 4);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(20, 20);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CloseButton_MouseDown);
            this.CloseButton.MouseEnter += new System.EventHandler(this.CloseButton_MouseEnter);
            this.CloseButton.MouseLeave += new System.EventHandler(this.CloseButton_MouseLeave);
            // 
            // TitleBar
            // 
            this.TitleBar.BackColor = System.Drawing.Color.Transparent;
            this.TitleBar.Controls.Add(this.FormTitle);
            this.TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleBar.Location = new System.Drawing.Point(0, 0);
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.Size = new System.Drawing.Size(800, 30);
            this.TitleBar.TabIndex = 1;
            this.TitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            this.TitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseMove);
            this.TitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseUp);
            // 
            // FormTitle
            // 
            this.FormTitle.AutoSize = true;
            this.FormTitle.BackColor = System.Drawing.Color.Transparent;
            this.FormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.FormTitle.Location = new System.Drawing.Point(14, 4);
            this.FormTitle.Name = "FormTitle";
            this.FormTitle.Size = new System.Drawing.Size(237, 22);
            this.FormTitle.TabIndex = 2;
            this.FormTitle.Text = "Chicken VS Chicken Game Server";
            this.FormTitle.UseCompatibleTextRendering = true;
            this.FormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            this.FormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseMove);
            this.FormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseUp);
            // 
            // SubmitButton
            // 
            this.SubmitButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SubmitButton.BackgroundImage")));
            this.SubmitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SubmitButton.FlatAppearance.BorderSize = 0;
            this.SubmitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SubmitButton.Location = new System.Drawing.Point(736, 560);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(52, 28);
            this.SubmitButton.TabIndex = 4;
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SubmitButton_MouseDown);
            this.SubmitButton.MouseEnter += new System.EventHandler(this.SubmitButton_MouseEnter);
            this.SubmitButton.MouseLeave += new System.EventHandler(this.SubmitButton_MouseLeave);
            // 
            // InputBox
            // 
            this.InputBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.InputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputBox.ForeColor = System.Drawing.Color.White;
            this.InputBox.Location = new System.Drawing.Point(14, 564);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(716, 19);
            this.InputBox.TabIndex = 3;
            this.InputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBox_KeyDown);
            // 
            // OutputBox
            // 
            this.OutputBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.OutputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OutputBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.OutputBox.DetectUrls = false;
            this.OutputBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputBox.ForeColor = System.Drawing.Color.White;
            this.OutputBox.Location = new System.Drawing.Point(12, 27);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedHorizontal;
            this.OutputBox.Size = new System.Drawing.Size(776, 528);
            this.OutputBox.TabIndex = 2;
            this.OutputBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LimeGreen;
            this.ClientSize = new System.Drawing.Size(800, 596);
            this.Controls.Add(this.BackgroundPannel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.BackgroundPannel.ResumeLayout(false);
            this.BackgroundPannel.PerformLayout();
            this.TitleBar.ResumeLayout(false);
            this.TitleBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel BackgroundPannel;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Panel TitleBar;
        private System.Windows.Forms.Button MinimizeBtn;
        private System.Windows.Forms.Label FormTitle;
        private System.Windows.Forms.RichTextBox OutputBox;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.TextBox InputBox;
    }
}

