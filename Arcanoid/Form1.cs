using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcanoid
{
    public partial class Form1 : Form
    {
        private Point rectanglePos = new Point(85, 295);
        private Point ballDirect = new Point(-1, -1);
        private Rectangle platform = new Rectangle(90, 291, 20, 5);
        private Rectangle ball = new Rectangle(97, 286, 4, 4);
        private bool gameStarted = false;

        private string difficulty = "Легко";
        private bool difficultyChanged = false;
        private int platformHaste;

        private Point _direct = Point.Empty;
        private Block[,] blocks = new Block[0, 0];
        private int life;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();

            platformHaste = PlatformHasteTrackBar.Value;

            DifficultyList();
            OptionsGroupBox.Visible = false;
            OptionsGroupBox.Enabled = false;

            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = e.Graphics;
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);

            if (gameStarted)
            {
                ball.X += ballDirect.X;
                ball.Y += ballDirect.Y;
            }

            if (ball.X <= 0 || ball.X >= 200)
            {
                ballDirect.X *= -1;
            }
            if (ball.Y <= 0 || ball.Y >= 300)
            {
                ballDirect.Y *= -1;
            }

            if (platform.X <= ball.X + 2 && ball.X + 2 <= platform.X + 20 && platform.Y == ball.Y + 2)
            {
                ballDirect.Y *= -1;
            }

            if (ball.Y >= 300)
            {
                life--;
                if (life == 0)
                {
                    EndGame();
                }
                gameStarted = false;
                ball.X = 97;
                ball.Y = 286;

                platform.X = 90;
                platform.Y = 291;
            }

            graph.DrawEllipse(blackPen, ball);
            graph.DrawRectangle(blackPen, platform);

            if (difficultyChanged)
            {
                difficultyChanged = false;
                switch (difficulty)
                {
                    case "Легко":
                        blocks = CreateBlocks(10, 6, 40);
                        life = 5;
                        break;

                    case "Среднe":
                        blocks = CreateBlocks(10, 8, 60);
                        life = 3;
                        break;

                    case "Сложно":
                        blocks = CreateBlocks(10, 12, 70);
                        life = 2;
                        break;

                    case "Ад":
                        blocks = CreateBlocks(10, 18, 95);
                        life = 1;
                        break;
                }
            }
            lifeNumberLabel.Text = life.ToString();
            foreach (Block block in blocks)
            {
                if (block.Visible)
                {
                    var point = block.Position;
                    var rect = new Rectangle(point.X, point.Y, 20, 10);

                    graph.DrawRectangle(blackPen, rect);

                    if (point.X <= ball.X + 2 && ball.X + 2 <= point.X + 20 && point.Y == ball.Y + 2)
                    {
                        ballDirect.Y *= -1;
                        block.Visible = false;
                        break;
                    }
                    if (point.X <= ball.X + 2 && ball.X + 2 <= point.X + 20 && ball.Y + 2 == point.Y + 10)
                    {
                        ballDirect.Y *= -1;
                        block.Visible = false;
                        break;
                    }
                    if (point.Y <= ball.Y + 2 && ball.Y + 2 <= point.Y + 10 && ball.X + 2 == point.X)
                    {
                        ballDirect.X *= -1;
                        block.Visible = false;
                        break;
                    }
                    if (point.Y <= ball.Y + 2 && ball.Y + 2 <= point.Y + 10 && ball.X + 2 == point.X + 20)
                    {
                        ballDirect.X *= -1;
                        block.Visible = false;
                        break;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (platform.X > 0)
                {
                    platform.X -= platformHaste;
                    if (!gameStarted)
                    {
                        ball.X -= platformHaste;
                        Refresh();
                    }
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                if (platform.X < 180)
                {
                    platform.X += platformHaste;
                    if (!gameStarted)
                    {
                        ball.X += platformHaste;
                        Refresh();
                    }
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                RefreshTimer.Enabled = true;
                gameStarted = true;
            }
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            MainMenuGroupBox.Visible = false;
            MainMenuGroupBox.Enabled = false;

            GameGroupBox.Visible = true;
            GameGroupBox.Enabled = true;

            RefreshTimer.Enabled = true;
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

        private void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            difficultyChanged = true;
        }

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

        private Block[,] CreateBlocks(int x, int y, int difficulty)
        {
            Block[,] blocks = new Block[x, y];
            var random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    blocks[i, j] = new Block
                    {
                        Position = new Point
                        {
                            X = i * 20,
                            Y = j * 10
                        }
                    };
                    var randomNumber = random.Next(1, 100);
                    if (randomNumber >= 1 && randomNumber <= difficulty)
                    {
                        blocks[i, j].Visible = true;
                    }
                    else
                    {
                        blocks[i, j].Visible = false;
                    }
                }
            }

            return blocks;
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            OptionsGroupBox.Visible = false;
            OptionsGroupBox.Enabled = false;

            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;

            difficultyChanged = false;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            OptionsGroupBox.Visible = true;
            OptionsGroupBox.Enabled = true;

            MainMenuGroupBox.Visible = false;
            MainMenuGroupBox.Enabled = false;
        }

        private void EndGame()
        {
            RefreshTimer.Enabled = false;
            var result = MessageBox.Show("Потрачено!");
            if (result == DialogResult.OK)
            {
                MainMenuGroupBox.Visible = true;
                MainMenuGroupBox.Enabled = true;

                GameGroupBox.Visible = false;
                GameGroupBox.Enabled = false;
            }
        }

        private void ToMainMenuButton_Click(object sender, EventArgs e)
        {
            RefreshTimer.Enabled = false;
            gameStarted = false;
            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;

            GameGroupBox.Visible = false;
            GameGroupBox.Enabled = false;
        }
    }
}