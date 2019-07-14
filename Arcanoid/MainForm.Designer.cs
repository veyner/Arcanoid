namespace Arcanoid
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
            this.MainWindow = new System.Windows.Forms.PictureBox();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.StartGameButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.GameNameLabel = new System.Windows.Forms.Label();
            this.MainMenuGroupBox = new System.Windows.Forms.GroupBox();
            this.OptionsButton = new System.Windows.Forms.Button();
            this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.SaveChangesButton = new System.Windows.Forms.Button();
            this.MaxHasteLabel = new System.Windows.Forms.Label();
            this.MinHasteLabel = new System.Windows.Forms.Label();
            this.PlatformHasteTrackBar = new System.Windows.Forms.TrackBar();
            this.PlatformHasteLabel = new System.Windows.Forms.Label();
            this.DifficultyLabel = new System.Windows.Forms.Label();
            this.DifficultyComboBox = new System.Windows.Forms.ComboBox();
            this.GameGroupBox = new System.Windows.Forms.GroupBox();
            this.LifeLabel = new System.Windows.Forms.Label();
            this.Life2Label = new System.Windows.Forms.Label();
            this.lifeNumberLabel = new System.Windows.Forms.Label();
            this.ToMainMenuButton = new System.Windows.Forms.Button();
            this.BufferTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MainWindow)).BeginInit();
            this.MainMenuGroupBox.SuspendLayout();
            this.OptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlatformHasteTrackBar)).BeginInit();
            this.GameGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainWindow
            // 
            this.MainWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainWindow.Location = new System.Drawing.Point(6, 14);
            this.MainWindow.Name = "MainWindow";
            this.MainWindow.Size = new System.Drawing.Size(203, 300);
            this.MainWindow.TabIndex = 0;
            this.MainWindow.TabStop = false;
            this.MainWindow.SizeChanged += new System.EventHandler(this.MainWindow_SizeChanged);
            this.MainWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.MainWindow_Paint);
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Interval = 10;
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // StartGameButton
            // 
            this.StartGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartGameButton.Location = new System.Drawing.Point(80, 84);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(135, 39);
            this.StartGameButton.TabIndex = 1;
            this.StartGameButton.Text = "Новая игра";
            this.StartGameButton.UseVisualStyleBackColor = true;
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.Location = new System.Drawing.Point(80, 174);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(135, 39);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "Выход";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // GameNameLabel
            // 
            this.GameNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GameNameLabel.AutoSize = true;
            this.GameNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.GameNameLabel.Location = new System.Drawing.Point(104, 37);
            this.GameNameLabel.Name = "GameNameLabel";
            this.GameNameLabel.Size = new System.Drawing.Size(92, 24);
            this.GameNameLabel.TabIndex = 3;
            this.GameNameLabel.Text = "arcanoEd";
            // 
            // MainMenuGroupBox
            // 
            this.MainMenuGroupBox.Controls.Add(this.OptionsButton);
            this.MainMenuGroupBox.Controls.Add(this.GameNameLabel);
            this.MainMenuGroupBox.Controls.Add(this.ExitButton);
            this.MainMenuGroupBox.Controls.Add(this.StartGameButton);
            this.MainMenuGroupBox.Location = new System.Drawing.Point(3, 0);
            this.MainMenuGroupBox.Name = "MainMenuGroupBox";
            this.MainMenuGroupBox.Size = new System.Drawing.Size(300, 320);
            this.MainMenuGroupBox.TabIndex = 1;
            this.MainMenuGroupBox.TabStop = false;
            this.MainMenuGroupBox.Text = "Главное меню";
            // 
            // OptionsButton
            // 
            this.OptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsButton.Location = new System.Drawing.Point(80, 129);
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(135, 39);
            this.OptionsButton.TabIndex = 4;
            this.OptionsButton.Text = "Настройки";
            this.OptionsButton.UseVisualStyleBackColor = true;
            this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
            // 
            // OptionsGroupBox
            // 
            this.OptionsGroupBox.Controls.Add(this.ReturnButton);
            this.OptionsGroupBox.Controls.Add(this.SaveChangesButton);
            this.OptionsGroupBox.Controls.Add(this.MaxHasteLabel);
            this.OptionsGroupBox.Controls.Add(this.MinHasteLabel);
            this.OptionsGroupBox.Controls.Add(this.PlatformHasteTrackBar);
            this.OptionsGroupBox.Controls.Add(this.PlatformHasteLabel);
            this.OptionsGroupBox.Controls.Add(this.DifficultyLabel);
            this.OptionsGroupBox.Controls.Add(this.DifficultyComboBox);
            this.OptionsGroupBox.Location = new System.Drawing.Point(3, 0);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(300, 320);
            this.OptionsGroupBox.TabIndex = 5;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Настройки";
            // 
            // ReturnButton
            // 
            this.ReturnButton.Location = new System.Drawing.Point(12, 143);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(98, 23);
            this.ReturnButton.TabIndex = 7;
            this.ReturnButton.Text = "Назад";
            this.ReturnButton.UseVisualStyleBackColor = true;
            this.ReturnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // SaveChangesButton
            // 
            this.SaveChangesButton.Location = new System.Drawing.Point(12, 114);
            this.SaveChangesButton.Name = "SaveChangesButton";
            this.SaveChangesButton.Size = new System.Drawing.Size(98, 23);
            this.SaveChangesButton.TabIndex = 6;
            this.SaveChangesButton.Text = "Сохранить";
            this.SaveChangesButton.UseVisualStyleBackColor = true;
            this.SaveChangesButton.Click += new System.EventHandler(this.SaveChangesButton_Click);
            // 
            // MaxHasteLabel
            // 
            this.MaxHasteLabel.AutoSize = true;
            this.MaxHasteLabel.Location = new System.Drawing.Point(174, 95);
            this.MaxHasteLabel.Name = "MaxHasteLabel";
            this.MaxHasteLabel.Size = new System.Drawing.Size(19, 13);
            this.MaxHasteLabel.TabIndex = 5;
            this.MaxHasteLabel.Text = "30";
            // 
            // MinHasteLabel
            // 
            this.MinHasteLabel.AutoSize = true;
            this.MinHasteLabel.Location = new System.Drawing.Point(15, 95);
            this.MinHasteLabel.Name = "MinHasteLabel";
            this.MinHasteLabel.Size = new System.Drawing.Size(13, 13);
            this.MinHasteLabel.TabIndex = 4;
            this.MinHasteLabel.Text = "5";
            // 
            // PlatformHasteTrackBar
            // 
            this.PlatformHasteTrackBar.Location = new System.Drawing.Point(12, 63);
            this.PlatformHasteTrackBar.Maximum = 30;
            this.PlatformHasteTrackBar.Minimum = 5;
            this.PlatformHasteTrackBar.Name = "PlatformHasteTrackBar";
            this.PlatformHasteTrackBar.Size = new System.Drawing.Size(181, 45);
            this.PlatformHasteTrackBar.SmallChange = 5;
            this.PlatformHasteTrackBar.TabIndex = 3;
            this.PlatformHasteTrackBar.TickFrequency = 5;
            this.PlatformHasteTrackBar.Value = 10;
            // 
            // PlatformHasteLabel
            // 
            this.PlatformHasteLabel.AutoSize = true;
            this.PlatformHasteLabel.Location = new System.Drawing.Point(9, 47);
            this.PlatformHasteLabel.Name = "PlatformHasteLabel";
            this.PlatformHasteLabel.Size = new System.Drawing.Size(117, 13);
            this.PlatformHasteLabel.TabIndex = 2;
            this.PlatformHasteLabel.Text = "Скорость платформы";
            // 
            // DifficultyLabel
            // 
            this.DifficultyLabel.AutoSize = true;
            this.DifficultyLabel.Location = new System.Drawing.Point(9, 23);
            this.DifficultyLabel.Name = "DifficultyLabel";
            this.DifficultyLabel.Size = new System.Drawing.Size(63, 13);
            this.DifficultyLabel.TabIndex = 1;
            this.DifficultyLabel.Text = "Сложность";
            // 
            // DifficultyComboBox
            // 
            this.DifficultyComboBox.FormattingEnabled = true;
            this.DifficultyComboBox.Location = new System.Drawing.Point(75, 20);
            this.DifficultyComboBox.Name = "DifficultyComboBox";
            this.DifficultyComboBox.Size = new System.Drawing.Size(121, 21);
            this.DifficultyComboBox.TabIndex = 0;
            // 
            // GameGroupBox
            // 
            this.GameGroupBox.Controls.Add(this.LifeLabel);
            this.GameGroupBox.Controls.Add(this.Life2Label);
            this.GameGroupBox.Controls.Add(this.lifeNumberLabel);
            this.GameGroupBox.Controls.Add(this.ToMainMenuButton);
            this.GameGroupBox.Controls.Add(this.MainWindow);
            this.GameGroupBox.Enabled = false;
            this.GameGroupBox.Location = new System.Drawing.Point(3, 0);
            this.GameGroupBox.Name = "GameGroupBox";
            this.GameGroupBox.Size = new System.Drawing.Size(300, 320);
            this.GameGroupBox.TabIndex = 14;
            this.GameGroupBox.TabStop = false;
            this.GameGroupBox.Visible = false;
            // 
            // LifeLabel
            // 
            this.LifeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LifeLabel.AutoSize = true;
            this.LifeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LifeLabel.Location = new System.Drawing.Point(212, 152);
            this.LifeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.LifeLabel.Name = "LifeLabel";
            this.LifeLabel.Size = new System.Drawing.Size(83, 20);
            this.LifeLabel.TabIndex = 14;
            this.LifeLabel.Text = "Осталось";
            this.LifeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Life2Label
            // 
            this.Life2Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Life2Label.AutoSize = true;
            this.Life2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Life2Label.Location = new System.Drawing.Point(222, 172);
            this.Life2Label.Name = "Life2Label";
            this.Life2Label.Size = new System.Drawing.Size(64, 20);
            this.Life2Label.TabIndex = 17;
            this.Life2Label.Text = "жизней";
            // 
            // lifeNumberLabel
            // 
            this.lifeNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lifeNumberLabel.AutoSize = true;
            this.lifeNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.lifeNumberLabel.Location = new System.Drawing.Point(240, 192);
            this.lifeNumberLabel.Margin = new System.Windows.Forms.Padding(0);
            this.lifeNumberLabel.Name = "lifeNumberLabel";
            this.lifeNumberLabel.Size = new System.Drawing.Size(25, 37);
            this.lifeNumberLabel.TabIndex = 15;
            this.lifeNumberLabel.Text = "0";
            this.lifeNumberLabel.UseCompatibleTextRendering = true;
            // 
            // ToMainMenuButton
            // 
            this.ToMainMenuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToMainMenuButton.Location = new System.Drawing.Point(214, 256);
            this.ToMainMenuButton.Margin = new System.Windows.Forms.Padding(0);
            this.ToMainMenuButton.Name = "ToMainMenuButton";
            this.ToMainMenuButton.Size = new System.Drawing.Size(77, 39);
            this.ToMainMenuButton.TabIndex = 16;
            this.ToMainMenuButton.TabStop = false;
            this.ToMainMenuButton.Text = "В главное меню";
            this.ToMainMenuButton.UseVisualStyleBackColor = true;
            this.ToMainMenuButton.Click += new System.EventHandler(this.ToMainMenuButton_Click);
            // 
            // BufferTimer
            // 
            this.BufferTimer.Interval = 1;
            this.BufferTimer.Tick += new System.EventHandler(this.BufferTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 323);
            this.Controls.Add(this.MainMenuGroupBox);
            this.Controls.Add(this.OptionsGroupBox);
            this.Controls.Add(this.GameGroupBox);
            this.MaximumSize = new System.Drawing.Size(323, 362);
            this.MinimumSize = new System.Drawing.Size(323, 362);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.MainWindow)).EndInit();
            this.MainMenuGroupBox.ResumeLayout(false);
            this.MainMenuGroupBox.PerformLayout();
            this.OptionsGroupBox.ResumeLayout(false);
            this.OptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlatformHasteTrackBar)).EndInit();
            this.GameGroupBox.ResumeLayout(false);
            this.GameGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MainWindow;
        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.Button StartGameButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Label GameNameLabel;
        private System.Windows.Forms.GroupBox MainMenuGroupBox;
        private System.Windows.Forms.GroupBox OptionsGroupBox;
        private System.Windows.Forms.Button SaveChangesButton;
        private System.Windows.Forms.Label MaxHasteLabel;
        private System.Windows.Forms.Label MinHasteLabel;
        private System.Windows.Forms.TrackBar PlatformHasteTrackBar;
        private System.Windows.Forms.Label PlatformHasteLabel;
        private System.Windows.Forms.Label DifficultyLabel;
        private System.Windows.Forms.ComboBox DifficultyComboBox;
        private System.Windows.Forms.Button OptionsButton;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.GroupBox GameGroupBox;
        private System.Windows.Forms.Label LifeLabel;
        private System.Windows.Forms.Label Life2Label;
        private System.Windows.Forms.Label lifeNumberLabel;
        private System.Windows.Forms.Button ToMainMenuButton;
        private System.Windows.Forms.Timer BufferTimer;
    }
}

