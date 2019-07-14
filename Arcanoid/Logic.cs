using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Arcanoid
{
    public class Logic
    {
        private PointF ballDirect;

        //private Rectangle platform = new Rectangle(90, 291, 20, 5);
        //private RectangleF ball = new RectangleF(97, 286, 4, 4);
        public PointF platformPosition;

        public PointF ballPosition;
        private int _platformHaste;

        private Block[,] blocks = new Block[0, 0];
        private int life;

        private MainForm _form;

        private int mainWindowWidth;
        private int mainWindowHeight;

        public Logic(int platformHaste, MainForm form)
        {
            _platformHaste = platformHaste;
            _form = form;
            //mainWindowWight = 200;
            //mainWindowHeight = 300;

            platformPosition = new PointF((float)0.45 * mainWindowWidth, (float)0.97 * mainWindowHeight);
            ballPosition = new PointF((float)0.485 * mainWindowWidth, (float)0.953 * mainWindowHeight);
            ballDirect = new PointF((float)-0.0033 * mainWindowWidth, (float)-0.0033 * mainWindowHeight);
        }

        public void MainWindowSize(int height, int width)
        {
            mainWindowHeight = height;
            mainWindowWidth = width;
        }

        private int ConvertFloatToInt(float i, int size)
        {
            return Convert.ToInt32(i * size);
        }

        /// <summary>
        /// Просчет положения шарика, проверка столкновений шарика и блоков
        /// </summary>
        public void GameLogic()
        {
            {
                if (_form.gameStarted)
                {
                    ballPosition.X += ballDirect.X;
                    ballPosition.Y += ballDirect.Y;
                }
                //проверка пересечения рамок окна
                if (ballPosition.X <= 0 || ballPosition.X >= mainWindowWidth)
                {
                    ballDirect.X *= -1;
                }
                if (ballPosition.Y <= 0 || ballPosition.Y >= mainWindowHeight)
                {
                    ballDirect.Y *= -1;
                }
                //пересечение шарика и платформы
                if (platformPosition.X <= ballPosition.X + (0.01 * mainWindowWidth) && ballPosition.X + (0.01 * mainWindowWidth) <= platformPosition.X + (0.1 * mainWindowWidth) && platformPosition.Y == ballPosition.Y + (0.01 * mainWindowHeight))
                {
                    ballDirect.Y *= -1;
                }

                if (ballPosition.Y >= mainWindowHeight)
                {
                    life--;
                    if (life == 0)
                    {
                        _form.EndGame();
                    }
                    StartPlatformAndBallPosition();
                }

                _form.ChangeLifeLabel(life.ToString());
                _form.CleanRectangleList();
                //проверка позиции шарика относительно блоков
                foreach (Block block in blocks)
                {
                    if (block.Visible)
                    {
                        var point = block.Position;
                        _form.AddBlocksAsRectangles(point);

                        if (point.X <= ballPosition.X + (0.01 * mainWindowWidth) && ballPosition.X + (0.01 * mainWindowWidth) <= point.X + (0.1 * mainWindowWidth) && point.Y == ballPosition.Y + (0.01 * mainWindowHeight))
                        {
                            ballDirect.Y *= -1;
                            block.Visible = false;
                            break;
                        }
                        if (point.X <= ballPosition.X + (0.01 * mainWindowWidth) && ballPosition.X + (0.01 * mainWindowWidth) <= point.X + (0.1 * mainWindowWidth) && ballPosition.Y + (0.01 * mainWindowHeight) == point.Y + (0.05 * mainWindowHeight))
                        {
                            ballDirect.Y *= -1;
                            block.Visible = false;
                            break;
                        }
                        if (point.Y <= ballPosition.Y + (0.01 * mainWindowHeight) && ballPosition.Y + (0.01 * mainWindowHeight) <= point.Y + (0.05 * mainWindowHeight) && ballPosition.X + (0.01 * mainWindowWidth) == point.X)
                        {
                            ballDirect.X *= -1;
                            block.Visible = false;
                            break;
                        }
                        if (point.Y <= ballPosition.Y + (0.01 * mainWindowHeight) && ballPosition.Y + (0.01 * mainWindowHeight) <= point.Y + (0.05 * mainWindowHeight) && ballPosition.X + (0.01 * mainWindowWidth) == point.X + (0.1 * mainWindowWidth))
                        {
                            ballDirect.X *= -1;
                            block.Visible = false;
                            break;
                        }
                        //if (rect.Contains(ball))
                        //{
                        //    var ballPointList = new List<Point>();
                        //    var ballPointCenter = new Point(ball.X + 2, ball.Y + 2);
                        //    var ballPointUp = new Point(ball.X + 2, ball.Y);
                        //    var ballPointLeft = new Point(ball.X, ball.Y + 2);
                        //    var ballPointRight = new Point(ball.X + 4, ball.Y + 2);
                        //    var ballPointDown = new Point(ball.X + 2, ball.Y + 4);

                        //    if (ballDirect.X < 0 && ballDirect.Y < 0)
                        //    {
                        //        var ballPointDirect = new Point(ball.X + 1, ball.Y + 1);
                        //        ballPointList.Add(ballPointDirect);
                        //        ballPointList.Add(ballPointLeft);
                        //        ballPointList.Add(ballPointUp);
                        //    }
                        //    if (ballDirect.X > 0 && ballDirect.Y < 0)
                        //    {
                        //        var ballPointDirect = new Point(ball.X + 3, ball.Y + 1);
                        //        ballPointList.Add(ballPointDirect);
                        //        ballPointList.Add(ballPointRight);
                        //        ballPointList.Add(ballPointUp);
                        //    }
                        //    if (ballDirect.X > 0 && ballDirect.Y > 0)
                        //    {
                        //        var ballPointDirect = new Point(ball.X + 3, ball.Y + 3);
                        //        ballPointList.Add(ballPointDirect);
                        //        ballPointList.Add(ballPointRight);
                        //        ballPointList.Add(ballPointDown);
                        //    }
                        //    if (ballDirect.X < 0 && ballDirect.Y > 0)
                        //    {
                        //        var ballPointDirect = new Point(ball.X + 1, ball.Y + 3);
                        //        ballPointList.Add(ballPointDirect);
                        //        ballPointList.Add(ballPointDown);
                        //        ballPointList.Add(ballPointLeft);
                        //    }

                        //    var dot = new Point(ballPointCenter.X - (2 * ballDirect.X), ballPointCenter.Y - (2 * ballDirect.Y));
                        //    var rectDot1 = new Point(point.X, point.Y);
                        //    var rectDot2 = new Point(point.X + 20, point.Y);
                        //    var rectDot3 = new Point(point.X, point.Y + 10);
                        //    var rectDot4 = new Point(point.X + 20, point.Y + 10);

                        //    if (Intersection(rectDot3, rectDot4, ballPointCenter, dot))
                        //    {
                        //        ballDirect.Y *= -1;
                        //        block.Visible = false;
                        //        break;
                        //    }
                        //    if (Intersection(rectDot1, rectDot2, ballPointCenter, dot))
                        //    {
                        //        ballDirect.Y *= -1;
                        //        block.Visible = false;
                        //        break;
                        //    }
                        //    if (Intersection(rectDot1, rectDot3, ballPointCenter, dot))
                        //    {
                        //        ballDirect.X *= -1;
                        //        block.Visible = false;
                        //        break;
                        //    }
                        //    if (Intersection(rectDot2, rectDot4, ballPointCenter, dot))
                        //    {
                        //        ballDirect.X *= -1;
                        //        block.Visible = false;
                        //        break;
                        //    }
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// загрузка стартового положения шарика, платформы и установленной сложности
        /// </summary>
        /// <param name="currentDifficulty">текущая сложность</param>
        public void StartGame(string currentDifficulty)
        {
            StartPlatformAndBallPosition();
            switch (currentDifficulty)
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

        /// <summary>
        /// стартовая позиция платформы и шарика
        /// (и в начале игры и после потери жизни)
        /// </summary>
        private void StartPlatformAndBallPosition()
        {
            _form.gameStarted = false;
            ballPosition.X = (float)0.485 * mainWindowWidth;
            ballPosition.Y = (float)0.953 * mainWindowHeight;
            platformPosition.X = (float)0.45 * mainWindowWidth;
            platformPosition.Y = (float)0.97 * mainWindowHeight;
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
            var random = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    blocks[i, j] = new Block
                    {
                        Position = new PointF
                        {
                            X = float.Parse(i.ToString()) * ((float)0.1 * mainWindowWidth),
                            Y = float.Parse(j.ToString()) * ((float)0.05 * mainWindowHeight)
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

        /// <summary>
        /// перемещение платформы влево
        /// в начале игры и после потери жизни вместе с платформой двигается шарик
        /// </summary>
        public void ChangePlatformPositionToLeft()
        {
            if (platformPosition.X > 0)
            {
                platformPosition.X -= _platformHaste;
                if (!_form.gameStarted)
                {
                    ballPosition.X -= _platformHaste;
                    _form.RefreshMainWindow();
                }
            }
        }

        /// <summary>
        /// перемещение платформы влево
        /// в начале игры и после потери жизни вместе с платформой двигается шарик
        /// </summary>
        public void ChangePlatformPositionToRight()
        {
            if (platformPosition.X < 0.9 * mainWindowWidth)
            {
                platformPosition.X += _platformHaste;
                if (!_form.gameStarted)
                {
                    ballPosition.X += _platformHaste;
                    _form.RefreshMainWindow();
                }
            }
        }

        //private bool Intersection(Point firstLinePoint1, Point firstLinePoint2, Point SecondLinePoint1, Point SecondLinePoint2)
        //{
        //    //int v1 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y);
        //    //int v2 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y);
        //    //int v3 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y);
        //    //int v4 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y);
        //    //int v1 = VectorMult(SecondLinePoint2.X - SecondLinePoint1.X, SecondLinePoint2.Y - SecondLinePoint1.Y, firstLinePoint1.X - SecondLinePoint1.X, firstLinePoint1.Y - SecondLinePoint1.Y);
        //    //int v2 = VectorMult(SecondLinePoint2.X - SecondLinePoint1.X, SecondLinePoint2.Y - SecondLinePoint1.Y, firstLinePoint2.X - SecondLinePoint1.X, firstLinePoint2.Y - SecondLinePoint1.Y);
        //    //int v3 = VectorMult(firstLinePoint2.X - firstLinePoint1.X, firstLinePoint2.Y - firstLinePoint1.Y, SecondLinePoint1.X - firstLinePoint1.X, SecondLinePoint1.Y - firstLinePoint1.Y);
        //    //int v4 = VectorMult(firstLinePoint2.X - firstLinePoint1.X, firstLinePoint2.Y - firstLinePoint1.Y, SecondLinePoint2.X - firstLinePoint1.X, SecondLinePoint2.Y - firstLinePoint1.Y);
        //    //if ((v1 * v2) < 0 && (v3 * v4) < 0)
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}

        //    if (firstLinePoint2.X < firstLinePoint1.X)
        //    {
        //        Point tmp = firstLinePoint1;
        //        firstLinePoint1 = firstLinePoint2;
        //        firstLinePoint2 = tmp;
        //    }

        //    if (SecondLinePoint2.X < SecondLinePoint1.X)
        //    {
        //        Point tmp = SecondLinePoint1;
        //        SecondLinePoint1 = SecondLinePoint2;
        //        SecondLinePoint2 = tmp;
        //    }
        //    if (firstLinePoint1.X - firstLinePoint2.X == 0)
        //    {
        //        //найдём Xa, Ya - точки пересечения двух прямых
        //        double Xa = firstLinePoint1.X;
        //        double a2 = (SecondLinePoint1.Y - SecondLinePoint2.Y) / (SecondLinePoint1.X - SecondLinePoint2.X);
        //        double b2 = SecondLinePoint1.Y - a2 * SecondLinePoint1.X;
        //        double Ya = a2 * Xa + b2;

        //        return SecondLinePoint1.X <= Xa && SecondLinePoint2.X >= Xa && Math.Min(firstLinePoint1.Y, firstLinePoint2.Y) <= Ya &&
        //                Math.Max(firstLinePoint1.Y, firstLinePoint2.Y) >= Ya;
        //    }
        //    else
        //    {
        //        double A1 = firstLinePoint2.Y - firstLinePoint1.Y;
        //        double B1 = firstLinePoint2.X - firstLinePoint1.X;
        //        double A2 = SecondLinePoint2.Y - SecondLinePoint1.Y;
        //        double B2 = SecondLinePoint2.X - SecondLinePoint1.X;
        //        double C1 = A1 * firstLinePoint1.X + B1 * firstLinePoint1.Y;
        //        double C2 = A2 * SecondLinePoint1.X + B2 * SecondLinePoint1.Y;
        //        double delta = A1 * B2 - A2 * B1;

        //        if (delta == 0)
        //            throw new ArgumentException("Lines are parallel");

        //        var x = ((B2 * C1) - (B1 * C2)) / delta;
        //        var y = ((A1 * C2) - (A2 * C1)) / delta;

        //        return x >= firstLinePoint1.X && x <= firstLinePoint2.X && y >= firstLinePoint1.Y && y <= firstLinePoint2.Y;
        //    }
        //}

        //private int VectorMult(int ax, int ay, int bx, int by)
        //{
        //    return ax * by - bx * ay;
        //}
    }
}