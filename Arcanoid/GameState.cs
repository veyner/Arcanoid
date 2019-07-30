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

        public GameState()
        {
            Ball = new Ball()
            {
                Position = new PointF(97, 286),
                Diameter = 4
            };
            Platform = new Platform()
            {
                Position = new PointF(90, 291),
                Height = 5,
                Width = 20
            };
            Blocks = new Block[0, 0];
        }

        public void StartPositions()
        {
            Ball.Position = new PointF(97, 286);
            Platform.Position = new PointF(90, 291);
        }

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
        /// Создание массива с блоками
        /// </summary>
        /// <param name="x">координата X блока</param>
        /// <param name="y">координата Y блока</param>
        /// <param name="difficulty">текущая сложность</param>
        /// <returns></returns>
        private Block[,] CreateBlocks(int x, int y, int difficulty)
        {
            Block[,] blocks = new Block[x, y];
            var blockHeight = 10;
            var blockWidth = 20;
            var random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    blocks[i, j] = new Block
                    {
                        Height = 10,
                        Width = 20,
                        Position = new Point
                        {
                            X = i * blockWidth,
                            Y = j * blockHeight
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
    }
}