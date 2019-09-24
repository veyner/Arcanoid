using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Arcanoid
{
    public class GameState
    {
        public Block[,] Blocks;
        public Platform Platform;
        public Ball Ball;
        public int Life;
        public int InvisibleBlocksAtStart = 0;
        public bool Pause;
        public bool GameStarted;
        public bool WinGame;
        public bool LoseGame;
        public bool ChangeLife;

        public GameState()
        {
            Ball = new Ball()
            {
                Position = new LocalPoint(),
                Diameter = 8,
                Texture = 9,
            };
            Platform = new Platform()
            {
                Position = new LocalPoint(),
                Height = 10,
                Width = 40,
                Texture = 10,
            };
            Blocks = new Block[0, 0];
        }

        /// <summary>
        /// Установка шарика и платформы в стартовую позицию
        /// </summary>
        public void SetStartPositions()
        {
            Ball.Position.X = 194;
            Ball.Position.Y = 572;

            Platform.Position.X = 180;
            Platform.Position.Y = 582;
        }

        /// <summary>
        /// Уставка сложности в зависимости от выбранной в настройках
        /// </summary>
        /// <param name="currentDifficulty"></param>
        public void SwitchDifficulty(string currentDifficulty)
        {
            switch (currentDifficulty)
            {
                case "Легко":
                    Blocks = CreateBlocks(10, 6, 40);
                    Life = 5;
                    break;

                case "Среднe":
                    Blocks = CreateBlocks(10, 8, 60);
                    Life = 3;
                    break;

                case "Сложно":
                    Blocks = CreateBlocks(10, 12, 70);
                    Life = 2;
                    break;

                case "Ад":
                    Blocks = CreateBlocks(10, 18, 95);
                    Life = 1;
                    break;
            }
        }

        /// <summary>
        /// Создание массива блоков
        /// </summary>
        /// <param name="x">координата X блока</param>
        /// <param name="y">координата Y блока</param>
        /// <param name="difficulty">текущая сложность</param>
        /// <returns></returns>
        private Block[,] CreateBlocks(int x, int y, int difficulty)
        {
            Block[,] blocks = new Block[x, y];
            var blockHeight = 20;
            var blockWidth = 40;
            var random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var randomTextureNumber = random.Next(1, 8);
                    blocks[i, j] = new Block
                    {
                        Height = blockHeight,
                        Width = blockWidth,
                        Position = new LocalPoint
                        {
                            X = i * blockWidth,
                            Y = j * blockHeight
                        },
                        TextureNumber = randomTextureNumber,
                    };

                    var randomNumber = random.Next(1, 100);
                    if (randomNumber >= 1 && randomNumber <= difficulty)
                    {
                        blocks[i, j].Visible = true;
                    }
                    else
                    {
                        blocks[i, j].Visible = false;
                        InvisibleBlocksAtStart++;
                    }
                }
            }
            return blocks;
        }
    }
}