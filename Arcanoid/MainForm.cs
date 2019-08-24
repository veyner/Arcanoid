﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

        //private bool rightArrow;
        //private bool leftArrow;
        private string textureDir = "Textures";

        private string textureImage = "TexturePack.png";
        private string ballTextureImage = "BallTexture.png";
        private List<Texture> textureList = new List<Texture>();

        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            UpdateStyles();
            //FormStandartSize();
            Height = 362;
            Width = 323;
            GroupBoxLocation();

            DifficultyList();
            SizeList();
            OptionsGroupBox.Visible = false;
            OptionsGroupBox.Enabled = false;

            MainMenuGroupBox.Visible = true;
            MainMenuGroupBox.Enabled = true;

            this.ResizeEnd += Form1_CreateBackBuffer;
            this.Load += Form1_CreateBackBuffer;
            difficulty = (string)DifficultyComboBox.SelectedItem;
            platformHaste = PlatformHasteTrackBar.Value;
            //MainWindow.Size = new Size() { Height = 300, Width = 203 };
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

        public void DrawBlockAsRectangles(Graphics graph)
        {
            foreach (Block block in logic.gameState.Blocks)
            {
                if (block.Visible)
                {
                    var rect = CreateBlockRectangle(block);
                    var textureBrush = textureList[block.TextureNumber - 1].TextureBrush;
                    graph.FillRectangle(textureBrush, rect);
                }
            }
        }

        private Rectangle CreateBlockRectangle(Block block)
        {
            var x = Convert.ToInt32(block.Position.X * scale.X);
            var y = Convert.ToInt32(block.Position.Y * scale.Y);
            var height = Convert.ToInt32(block.Height * scale.Y);
            var width = Convert.ToInt32(block.Width * scale.X);
            var rect = new Rectangle(x, y, width, height);
            return rect;
        }

        /// <summary>
        /// очистка листа блоков для отрисовки
        /// </summary>
        public void CleanRectangleList()
        {
            rectList.Clear();
        }

        // 152,80 128х24
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
                    Pen blackPen1 = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
                    Pen blackPen2 = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
                    var ball = new Rectangle
                    {
                        X = Convert.ToInt32(logic.gameState.Ball.Position.X * scale.X),
                        Y = Convert.ToInt32(logic.gameState.Ball.Position.Y * scale.Y),
                        Height = Convert.ToInt32(logic.gameState.Ball.Diameter * scale.Y),
                        Width = Convert.ToInt32(logic.gameState.Ball.Diameter * scale.X),
                    };
                    graph.DrawEllipse(blackPen1, ball);
                    graph.FillEllipse(Brushes.Orange, ball);
                    //var ballTexture = logic.gameState.Ball.Texture;
                    //graph.FillRectangle(textureList[ballTexture - 1].TextureBrush, ball);
                    var platform = new Rectangle()
                    {
                        X = Convert.ToInt32(logic.gameState.Platform.Position.X * scale.X),
                        Y = Convert.ToInt32(logic.gameState.Platform.Position.Y * scale.Y),
                        Height = Convert.ToInt32(logic.gameState.Platform.Height * scale.Y),
                        Width = Convert.ToInt32(logic.gameState.Platform.Width * scale.X)
                    };
                    graph.DrawRectangle(blackPen2, platform);
                    graph.FillRectangle(Brushes.Red, platform);
                    //var platformTexture = logic.gameState.Platform.Texture;
                    //graph.FillRectangle(textureList[platformTexture - 1].TextureBrush, platform);
                    DrawBlockAsRectangles(graph);
                    Invalidate();
                }
            }
        }

        private void AddBlockTextureList()
        {
            var blockHeight = Convert.ToInt32(10 * scale.Y);
            var blockWidth = Convert.ToInt32(20 * scale.X);
            var rect = new Rectangle(0, 0, blockWidth, blockHeight);
            var texture1 = new Texture()
            {
                X = 0,
                Y = 0,
                Width = 65,
                Height = 33,
                Number = 1
            };
            var textureRect1 = new Rectangle(texture1.X, texture1.Y, texture1.Width, texture1.Height);
            texture1.TextureBrush = GetTextureBrush(textureRect1, rect.Width, rect.Height, textureImage);
            textureList.Add(texture1);
            var texture2 = new Texture()
            {
                X = 72,
                Y = 0,
                Width = 65,
                Height = 33,
                Number = 2
            };
            var textureRect2 = new Rectangle(texture2.X, texture2.Y, texture2.Width, texture2.Height);
            texture2.TextureBrush = GetTextureBrush(textureRect2, rect.Width, rect.Height, textureImage);
            textureList.Add(texture2);
            var texture3 = new Texture()
            {
                X = 144,
                Y = 0,
                Width = 65,
                Height = 33,
                Number = 3
            };
            var textureRect3 = new Rectangle(texture3.X, texture3.Y, texture3.Width, texture3.Height);
            texture3.TextureBrush = GetTextureBrush(textureRect3, rect.Width, rect.Height, textureImage);
            textureList.Add(texture3);
            var texture4 = new Texture()
            {
                X = 216,
                Y = 0,
                Width = 65,
                Height = 33,
                Number = 4
            };
            var textureRect4 = new Rectangle(texture4.X, texture4.Y, texture4.Width, texture4.Height);
            texture4.TextureBrush = GetTextureBrush(textureRect4, rect.Width, rect.Height, textureImage);
            textureList.Add(texture4);
            var texture5 = new Texture()
            {
                X = 0,
                Y = 40,
                Width = 65,
                Height = 33,
                Number = 5
            };
            var textureRect5 = new Rectangle(texture5.X, texture5.Y, texture5.Width, texture5.Height);
            texture5.TextureBrush = GetTextureBrush(textureRect5, rect.Width, rect.Height, textureImage);
            textureList.Add(texture5);
            var texture6 = new Texture()
            {
                X = 72,
                Y = 40,
                Width = 65,
                Height = 33,
                Number = 6
            };
            var textureRect6 = new Rectangle(texture6.X, texture6.Y, texture6.Width, texture6.Height);
            texture6.TextureBrush = GetTextureBrush(textureRect6, rect.Width, rect.Height, textureImage);
            textureList.Add(texture6);
            var texture7 = new Texture()
            {
                X = 144,
                Y = 40,
                Width = 65,
                Height = 33,
                Number = 7
            };
            var textureRect7 = new Rectangle(texture7.X, texture7.Y, texture7.Width, texture7.Height);
            texture7.TextureBrush = GetTextureBrush(textureRect7, rect.Width, rect.Height, textureImage);
            textureList.Add(texture7);
            var texture8 = new Texture()
            {
                X = 216,
                Y = 40,
                Width = 65,
                Height = 33,
                Number = 8
            };
            var textureRect8 = new Rectangle(texture8.X, texture8.Y, texture8.Width, texture8.Height);
            texture8.TextureBrush = GetTextureBrush(textureRect8, rect.Width, rect.Height, textureImage);
            textureList.Add(texture8);
            //var ballHeight = Convert.ToInt32(logic.gameState.Ball.Diameter * scale.Y);
            //var ballWidth = Convert.ToInt32(logic.gameState.Ball.Diameter * scale.X);
            //var ballTexture = new Texture()
            //{
            //    X = 0,
            //    Y = 0,
            //    Width = 24,
            //    Height = 24,
            //    Number = 9
            //};
            //var ballTextureRect = new Rectangle(ballTexture.X, ballTexture.Y, ballTexture.Width, ballTexture.Height);
            //ballTexture.TextureBrush = GetTextureBrush(ballTextureRect, ballWidth, ballHeight, ballTextureImage);
            //textureList.Add(ballTexture);
            //var platformHeight = Convert.ToInt32(logic.gameState.Platform.Height * scale.Y);
            //var platformWidth = Convert.ToInt32(logic.gameState.Platform.Width * scale.X);
            //var platformTexture = new Texture()
            //{
            //    X = 152,
            //    Y = 80,
            //    Width = 128,
            //    Height = 24,
            //    Number = 10
            //};
            //var platformTextureRect = new Rectangle(platformTexture.X, platformTexture.Y, platformTexture.Width, platformTexture.Height);
            //platformTexture.TextureBrush = GetTextureBrush(platformTextureRect, platformWidth, platformHeight, textureImage);
            //textureList.Add(platformTexture);
        }

        private Bitmap CropBitmap(Bitmap src, Rectangle textureRectangle, int width, int height)
        {
            Bitmap target = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                 textureRectangle,
                                 GraphicsUnit.Pixel);
            }
            return target;
        }

        private TextureBrush GetTextureBrush(Rectangle textureRectangle, int width, int height, string textureImage)
        {
            var path = Path.Combine(Environment.CurrentDirectory, textureDir, textureImage);
            Bitmap textures = (Bitmap)Image.FromFile(path);
            var cropBitmap = CropBitmap(textures, textureRectangle, width, height);
            TextureBrush textureBrush = new TextureBrush(cropBitmap);
            return textureBrush;
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
            //MaximumSize = new Size()
            //{ Height = 0, Width = 0 };

            GameGroupBox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            SelectSize();
            ScaleSize();
            StartGame();
            textureList.Clear();
            AddBlockTextureList();

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
            //FormStandartSize();
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

        private void SizeList()
        {
            var sizeList = new List<string>
            {
                "Маленький",
                "Cредний",
                "Большой"
            };
            SizeComboBox.DataSource = sizeList;
            DifficultyComboBox.DisplayMember = nameof(sizeList);
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            //FormStandartSize();
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
            //FormStandartSize();
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
                //FormStandartSize();
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
                //FormStandartSize();
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
            //PlatformTimer.Enabled = true;

            //if (e.KeyCode == Keys.Left)
            //{
            //    leftArrow = true;
            //}
            //if (e.KeyCode == Keys.Right)
            //{
            //    rightArrow = true;
            //}
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            //PlatformTimer.Enabled = false;
            //if (e.KeyCode == Keys.Left)
            //{
            //    leftArrow = false;
            //}
            //if (e.KeyCode == Keys.Right)
            //{
            //    rightArrow = false;
            //}
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //    }
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            if (MainWindow.Visible)
            {
                //ScaleSize();
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
            //MinimumSize = size;
            //MaximumSize = size;
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

        private void SelectSize()
        {
            switch (SizeComboBox.Text)
            {
                case "Маленький":
                    Height = 362;
                    Width = 323;
                    MainWindow.Size = new Size(200, 300);
                    break;

                case "Cредний":
                    Height = 662;
                    Width = 523;
                    MainWindow.Size = new Size(400, 600);
                    break;

                case "Большой":
                    Height = 962;
                    Width = 723;
                    MainWindow.Size = new Size(600, 900);
                    break;
            }
            backBuffer = new Bitmap(MainWindow.Width, MainWindow.Height);
        }

        private void SizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void PlatformTimer_Tick(object sender, EventArgs e)
        {
            //if (leftArrow)
            //{
            //    if (!pause)
            //    {
            //        logic.ChangePlatformPositionToLeft(platformHaste);
            //    }
            //}
            //if (rightArrow)
            //{
            //    if (!pause)
            //    {
            //        logic.ChangePlatformPositionToRight(platformHaste);
            //    }
            //}
        }
    }
}