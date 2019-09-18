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
        public string difficulty;
        private Logic _logic;
        private int platformHaste; //расстояние перемещения шарика
        private GameState _gameState = new GameState();
        private Render _render;

        private bool rightArrow;
        private bool leftArrow;


        public MainForm()
        {
            _render = new Render(this,_gameState);
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            UpdateStyles();
            Height = 362;
            Width = 323;
            GroupBoxLocation();

            DifficultyList();
            SizeList();
            OptionsGroupBox.Visible = false;

            MainMenuGroupBox.Visible = true;

            this.ResizeEnd += MainForm_CreateBackBuffer;
            this.Load += MainForm_CreateBackBuffer;
            difficulty = (string)DifficultyComboBox.SelectedItem;
            platformHaste = PlatformHasteTrackBar.Value;
            _logic = new Logic(_gameState);
        }
        //отрисовка из буфера в окно
        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            if (_render.BackBuffer != null)
            {
                e.Graphics.DrawImageUnscaled(_render.BackBuffer, Point.Empty);
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            //отрисовка в буфер
            _render.Draw();
            MainWindow.Refresh();
            if(_gameState.WinGame)
            {
                WinGame();
            }
            if(_gameState.LoseGame)
            {
                EndGame();
            }
            if(_gameState.ChangeLife)
            {
                _gameState.ChangeLife = false;
                lifeNumberLabel.Text = _gameState.Life.ToString();
            }
        }

        private void MainForm_CreateBackBuffer(object sender, EventArgs e)
        {
            if (_render.BackBuffer != null)
            {
                _render.BackBuffer.Dispose();
            }

            _render.BackBuffer = new Bitmap(MainWindow.Width, MainWindow.Height);
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            GameGroupBox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            SelectSize();
            _render.ScaleSize(MainWindow.Height, MainWindow.Width, _logic.VirtualHeight, _logic.VirtualWidth);
            _logic.StartGame(difficulty);
            _render.TextureList.Clear();
            _render.AddBlockTextureList();
            
            PauseButton.Text = "Пауза"; 

            MainMenuGroupBox.Visible = false;

            GameGroupBox.Visible = true;

            RefreshTimer.Start();
            _logic.LogicTimer.Start();
            //BufferTimer.Enabled = true;
        }
        
        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            platformHaste = PlatformHasteTrackBar.Value;
            difficulty = (string)DifficultyComboBox.SelectedItem;

            OptionsGroupBox.Visible = false;

            MainMenuGroupBox.Visible = true;
        }

        /// <summary>
        /// Лист сложностей для настроек
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
        /// <summary>
        /// Лист размеров окна для настроек
        /// </summary>
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
            OptionsGroupBox.Visible = false;

            MainMenuGroupBox.Visible = true;
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

            MainMenuGroupBox.Visible = false;
        }

        /// <summary>
        /// Остановка игры если хп = 0
        /// </summary>
        public void EndGame()
        {
            RefreshTimer.Stop();
            _logic.LogicTimer.Stop();
            PlatformTimer.Stop();
            _gameState.LoseGame = false;
            
            var result = MessageBox.Show("Потрачено!");
            if (result == DialogResult.OK)
            {
                FormStandartSize();
                MainMenuGroupBox.Visible = true;
                _gameState.GameStarted = false;
                GameGroupBox.Visible = false;
            }
        }
        /// <summary>
        /// Выигрыш игры если выбиты все блоки
        /// </summary>
        public void WinGame()
        {
            RefreshTimer.Stop();
            _logic.LogicTimer.Stop();
            PlatformTimer.Stop();
            _gameState.WinGame = false;
            
            var result = MessageBox.Show("Выиграно!");
            if (result == DialogResult.OK)
            {
                FormStandartSize();
                MainMenuGroupBox.Visible = true;
                _gameState.GameStarted = false;
                GameGroupBox.Visible = false;
            }
        }

        private void ToMainMenuButton_Click(object sender, EventArgs e)
        {
            FormStandartSize();
            RefreshTimer.Stop();
            _logic.LogicTimer.Stop();
            PlatformTimer.Stop();
            
            _gameState.GameStarted = false;
            MainMenuGroupBox.Visible = true;
            _gameState.Pause = false;
            GameGroupBox.Visible = false;
        }

        public void RefreshMainWindow()
        {
            MainWindow.Refresh();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (!_gameState.Pause)
            {
                _gameState.Pause = true;
                PauseButton.Text = "Продолжить";
            }
            else
            {
                _gameState.Pause = false;
                PauseButton.Text = "Пауза";
            }
        }

        private const int WM_KEYUP = 0x101;

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYUP)
            {
                if (leftArrow)
                {
                    PlatformTimer.Stop();
                    leftArrow = false;
                }
                else if (rightArrow)
                {
                    PlatformTimer.Stop();
                    rightArrow = false;
                }
            }
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left: // left arrow key
                    if (!_gameState.Pause)
                    {
                        PlatformTimer.Start();
                        leftArrow = true;
                    }
                    return true;

                case Keys.Right: // right arrow key
                    if (!_gameState.Pause)
                    {
                        PlatformTimer.Start();
                        rightArrow = true;
                    }
                    return true;

                case Keys.Enter:
                    if(GameGroupBox.Visible)
                    {
                        RefreshTimer.Start();
                        _logic.LogicTimer.Start();
                        
                        _gameState.GameStarted = true;
                    }
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// Установленные размеры главного меню и настроек
        /// </summary>
        private void FormStandartSize()
        {
            Height = 362;
            Width = 323;
            var size = new Size();
            size.Height = 362;
            size.Width = 323;
        }
        /// <summary>
        /// Установка всех groupbox по заданным координатам
        /// </summary>
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
        /// <summary>
        /// Размеры главного игрового окна
        /// </summary>
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
            _render.BackBuffer = new Bitmap(MainWindow.Width, MainWindow.Height);
        }
        
        private void PlatformTimer_Tick(object sender, EventArgs e)
        {
            // в начале игры и после потери жизни вместе с платформой двигается шарик
            if (leftArrow) //перемещение платформы влево
            {
                if (!_gameState.Pause)
                {
                    if (_gameState.Platform.Position.X - platformHaste >= 0)
                    {
                        _gameState.Platform.Position.X -= platformHaste;

                        if (!_gameState.GameStarted)
                        {
                            _gameState.Ball.Position.X -= platformHaste;

                            MainWindow.Refresh();
                        }
                    }
                }
            }
            if (rightArrow) //перемещение платформы вправо
            {
                if (!_gameState.Pause)
                {
                    if (_gameState.Platform.Position.X + platformHaste <= _logic.VirtualWidth - _gameState.Platform.Width)
                    {
                        _gameState.Platform.Position.X += platformHaste;

                        if (!_gameState.GameStarted)
                        {
                            _gameState.Ball.Position.X += platformHaste;

                            MainWindow.Refresh();
                        }
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var exitResult = MessageBox.Show("Вы хотите закрыть приложение?","Выход",MessageBoxButtons.YesNo);
            if (exitResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}