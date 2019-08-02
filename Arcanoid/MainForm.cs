using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcanoid
{
    public partial class MainForm : Form
    {
        public bool gameStarted;
        public string difficulty;
        private Bitmap backBuffer;
        private Logic logic;
        private List<Rectangle> rectList = new List<Rectangle>();
        private int platformHaste;
        public bool pause;
        private PointF scale = new PointF(1, 1);

        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
            FormStandartSize();
            GroupBoxLocation();

            DifficultyList();
            OptionsGroupBox.Visible = false;
            OptionsGroupBox.Enabled = false;

            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;

            this.ResizeEnd += Form1_CreateBackBuffer;
            this.Load += Form1_CreateBackBuffer;
            difficulty = (string)DifficultyComboBox.SelectedItem;
            platformHaste = PlatformHasteTrackBar.Value;
            MainWindow.Size = new Size() { Height = 300, Width = 203 };

            logic = new Logic(this);

            //logic.MainWindowSize(MainWindow.Height, MainWindow.Width);
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            //отрисовка картинки из буфера
            if (backBuffer != null)
            {
                e.Graphics.DrawImageUnscaled(backBuffer, Point.Empty);
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            //отрисовка в буфер
            Draw();
            MainWindow.Refresh();
        }

        private void BufferTimer_Tick(object sender, EventArgs e)
        {
            // логика (пересчет позиций и столкновений)
            logic.GameLogic();
        }

        private void Form1_CreateBackBuffer(object sender, EventArgs e)
        {
            if (backBuffer != null)
            {
                backBuffer.Dispose();
            }

            backBuffer = new Bitmap(MainWindow.Width, MainWindow.Height);
        }

        /// <summary>
        /// создание блока для отрисовки по вычисленным позициям
        /// </summary>

        public void AddBlocksAsRectangles()
        {
            foreach (Block block in logic.gameState.Blocks)
            {
                if (block.Visible)
                {
                    var x = Convert.ToInt32(block.Position.X * scale.X);
                    var y = Convert.ToInt32(block.Position.Y * scale.Y);
                    var height = Convert.ToInt32(block.Height * scale.Y);
                    var width = Convert.ToInt32(block.Width * scale.X);
                    var rect = new Rectangle(x, y, width, height);
                    rectList.Add(rect);
                }
            }
        }

        /// <summary>
        /// очистка листа блоков для отрисовки
        /// </summary>
        public void CleanRectangleList()
        {
            rectList.Clear();
        }

        /// <summary>
        /// отрисовка шарика, платформы и блоков
        /// </summary>
        private void Draw()
        {
            if (backBuffer != null)
            {
                using (var graph = Graphics.FromImage(backBuffer))
                {
                    graph.Clear(Color.White);
                    Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
                    var ball = new RectangleF
                    {
                        X = Convert.ToInt32(logic.gameState.Ball.Position.X * scale.X),
                        Y = Convert.ToInt32(logic.gameState.Ball.Position.Y * scale.Y),
                        Height = Convert.ToInt32(logic.gameState.Ball.Diameter * scale.Y),
                        Width = Convert.ToInt32(logic.gameState.Ball.Diameter * scale.X)
                    };
                    graph.DrawEllipse(blackPen, ball);
                    var platform = new Rectangle()
                    {
                        X = Convert.ToInt32(logic.gameState.Platform.Position.X * scale.X),
                        Y = Convert.ToInt32(logic.gameState.Platform.Position.Y * scale.Y),
                        Height = Convert.ToInt32(logic.gameState.Platform.Height * scale.Y),
                        Width = Convert.ToInt32(logic.gameState.Platform.Width * scale.X)
                    };
                    graph.DrawRectangle(blackPen, platform);
                    AddBlocksAsRectangles();
                    foreach (Rectangle rect in rectList)
                    {
                        graph.DrawRectangle(blackPen, rect);
                    }
                    Invalidate();
                }
            }
        }

        private void ScaleSize()
        {
            float height = MainWindow.Height;
            float width = MainWindow.Width;
            scale.Y = height / logic.virtualHeight;
            scale.X = width / logic.virtualWidth;
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            MaximumSize = new Size()
            { Height = 0, Width = 0 };

            GameGroupBox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            StartGame();
            MainMenuGroupBox.Visible = false;
            MainMenuGroupBox.Enabled = false;

            GameGroupBox.Visible = true;
            GameGroupBox.Enabled = true;

            RefreshTimer.Enabled = true;
            BufferTimer.Enabled = true;
        }

        private void StartGame()
        {
            gameStarted = false;
            logic.gameState.StartPositions();
            logic.gameState.SwitchDifficulty(difficulty);
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            FormStandartSize();
            platformHaste = PlatformHasteTrackBar.Value;
            difficulty = (string)DifficultyComboBox.SelectedItem;

            OptionsGroupBox.Visible = false;
            OptionsGroupBox.Enabled = false;

            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;
        }

        /// <summary>
        /// лист сложностей для комбобокса
        /// </summary>
        private void DifficultyList()
        {
            var diffList = new List<string>
            {
                "Легко",
                "Среднe",
                "Сложно",
                "Ад"
            };
            DifficultyComboBox.DataSource = diffList;
            DifficultyComboBox.DisplayMember = nameof(diffList);
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            FormStandartSize();
            OptionsGroupBox.Visible = false;
            OptionsGroupBox.Enabled = false;

            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            FormStandartSize();
            DifficultyComboBox.SelectedItem = difficulty;
            PlatformHasteTrackBar.Value = platformHaste;

            OptionsGroupBox.Visible = true;
            OptionsGroupBox.Enabled = true;

            MainMenuGroupBox.Visible = false;
            MainMenuGroupBox.Enabled = false;
        }

        /// <summary>
        /// остановка игры если хп = 0
        /// </summary>
        public void EndGame()
        {
            RefreshTimer.Enabled = false;
            BufferTimer.Enabled = false;
            var result = MessageBox.Show("Потрачено!");
            if (result == DialogResult.OK)
            {
                FormStandartSize();
                MainMenuGroupBox.Visible = true;
                MainMenuGroupBox.Enabled = true;

                GameGroupBox.Visible = false;
                GameGroupBox.Enabled = false;
            }
        }

        public void WinGame()
        {
            RefreshTimer.Enabled = false;
            BufferTimer.Enabled = false;
            var result = MessageBox.Show("Выиграно!");
            if (result == DialogResult.OK)
            {
                FormStandartSize();
                MainMenuGroupBox.Visible = true;
                MainMenuGroupBox.Enabled = true;

                GameGroupBox.Visible = false;
                GameGroupBox.Enabled = false;
            }
        }

        public void ChangeLifeLabel(string life)
        {
            lifeNumberLabel.Text = life;
        }

        private void ToMainMenuButton_Click(object sender, EventArgs e)
        {
            FormStandartSize();
            RefreshTimer.Enabled = false;
            BufferTimer.Enabled = false;
            gameStarted = false;
            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;

            GameGroupBox.Visible = false;
            GameGroupBox.Enabled = false;
        }

        public void RefreshMainWindow()
        {
            MainWindow.Refresh();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Left)
            //{
            //}
            //if (e.KeyCode == Keys.Right)
            //{
            //}
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //    }
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            if (MainWindow.Visible)
            {
                ScaleSize();
                //logic.MainWindowSize(MainWindow.Height, MainWindow.Width);
                //ball.Height = logic.ConvertFloatToInt((float)0.013, MainWindow.Height);
                //ball.Width = logic.ConvertFloatToInt((float)0.02, MainWindow.Width);
                //platform.Height = logic.ConvertFloatToInt((float)0.0166, MainWindow.Height);
                //platform.Width = logic.ConvertFloatToInt((float)0.1, MainWindow.Width);

                //logic.ScalingPositionsOfObjects(MainWindow.Height, MainWindow.Width);
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            pause = true;
            PauseButton.Enabled = false;
            PauseButton.Visible = false;
            ContinueButton.Visible = true;
            ContinueButton.Enabled = true;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            pause = false;
            PauseButton.Enabled = true;
            PauseButton.Visible = true;
            ContinueButton.Visible = false;
            ContinueButton.Enabled = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left: // left arrow key
                    if (!pause)
                    {
                        logic.ChangePlatformPositionToLeft(platformHaste);
                    }
                    return true;

                case Keys.Right: // right arrow key
                    if (!pause)
                    {
                        logic.ChangePlatformPositionToRight(platformHaste);
                    }
                    return true;

                case Keys.Enter:
                    RefreshTimer.Enabled = true;
                    BufferTimer.Enabled = true;
                    gameStarted = true;
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormStandartSize()
        {
            Height = 362;
            Width = 323;
            var size = new Size();
            size.Height = 362;
            size.Width = 323;
            MinimumSize = size;
            MaximumSize = size;
        }

        private void GroupBoxLocation()
        {
            var location = new Point()
            {
                X = 1,
                Y = 1
            };

            MainMenuGroupBox.Location = location;
            OptionsGroupBox.Location = location;
            GameGroupBox.Location = location;
        }
    }
}