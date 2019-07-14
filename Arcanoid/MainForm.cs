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
        private Rectangle platform;
        private RectangleF ball;
        public bool gameStarted;
        public string difficulty;
        private Bitmap backBuffer;
        private Logic logic;
        private List<Rectangle> rectList = new List<Rectangle>();
        private int platformHaste;

        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();

            DifficultyList();
            OptionsGroupBox.Visible = false;
            OptionsGroupBox.Enabled = false;

            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;

            this.ResizeEnd += Form1_CreateBackBuffer;
            this.Load += Form1_CreateBackBuffer;
            difficulty = (string)DifficultyComboBox.SelectedItem;
            platformHaste = PlatformHasteTrackBar.Value;

            logic = new Logic(platformHaste, this);

            ball = new RectangleF(0, 0, 4, 4);
            platform = new Rectangle(0, 0, 20, 5);
            logic.MainWindowSize(MainWindow.Height, MainWindow.Width);
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
        /// <param name="position">позиция блока</param>
        public void AddBlocksAsRectangles(PointF position)
        {
            var x = Convert.ToInt32(position.X);
            var y = Convert.ToInt32(position.Y);
            var rect = new Rectangle(x, y, 20, 10);
            rectList.Add(rect);
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
                    var ballPosition = logic.ballPosition;
                    ball.X = ballPosition.X;
                    ball.Y = ballPosition.Y;
                    graph.DrawEllipse(blackPen, ball);
                    var platformPosition = logic.platformPosition;
                    platform.X = Convert.ToInt32(platformPosition.X);
                    platform.Y = Convert.ToInt32(platformPosition.Y);
                    graph.DrawRectangle(blackPen, platform);
                    foreach (Rectangle rect in rectList)
                    {
                        graph.DrawRectangle(blackPen, rect);
                    }
                    Invalidate();
                }
            }
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            logic.StartGame(difficulty);
            MainMenuGroupBox.Visible = false;
            MainMenuGroupBox.Enabled = false;

            GameGroupBox.Visible = true;
            GameGroupBox.Enabled = true;

            RefreshTimer.Enabled = true;
            BufferTimer.Enabled = true;
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
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
            if (e.KeyCode == Keys.Left)
            {
                logic.ChangePlatformPositionToLeft();
            }
            if (e.KeyCode == Keys.Right)
            {
                logic.ChangePlatformPositionToRight();
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RefreshTimer.Enabled = true;
                BufferTimer.Enabled = true;
                gameStarted = true;
            }
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            logic.MainWindowSize(MainWindow.Height, MainWindow.Width);
        }
    }
}