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
        //private Rectangle platform = new Rectangle(90, 291, 20, 5);
        //private RectangleF ball = new RectangleF(97, 286, 4, 4);
        //public Point platformPosition;

        //public PointF ballPosition;

        private MainForm _form;

        //private int mainWindowWidth;
        //private int mainWindowHeight;
        public int virtualHeight = 300;

        public int virtualWidth = 200;

        public GameState gameState = new GameState();
        private PointF ballDirect = new PointF((float)-1, (float)-1);
        private int invisibleBlock = 0;

        public Logic(MainForm form)
        {
            _form = form;
            //mainWindowWidth = 300;
            //mainWindowHeight = 400;
            //MainWindowSize(height, width);
            //ballDirect = new PointF(-1, -1);
            //ScalingPositionsOfObjects(mainWindowHeight, mainWindowWidth);
        }

        //public void ScalingPositionsOfObjects(int height, int width)
        //{
        //    platformPosition = new PointF(ConvertFloatToInt((float)0.45, width), ConvertFloatToInt((float)0.97, height));
        //    ballPosition = new PointF(ConvertFloatToInt((float)0.485, width), ConvertFloatToInt((float)0.953, height));

        //    ScalingBlockPositions(height, width);
        //}

        //public void MainWindowSize(int height, int width)
        //{
        //    mainWindowHeight = height;
        //    mainWindowWidth = width;
        //}

        //public int ConvertFloatToInt(float i, int size)
        //{
        //    return Convert.ToInt32(i * size);
        //}

        /// <summary>
        /// Просчет положения шарика, проверка столкновений шарика и блоков
        /// </summary>
        public void GameLogic()
        {
            {
                var ball = new Rectangle
                {
                    X = Convert.ToInt32(gameState.Ball.Position.X),
                    Y = Convert.ToInt32(gameState.Ball.Position.Y),
                    Height = Convert.ToInt32(gameState.Ball.Diameter),
                    Width = Convert.ToInt32(gameState.Ball.Diameter)
                };

                if (_form.gameStarted && !_form.pause)
                {
                    gameState.Ball.Position.X += ballDirect.X;
                    gameState.Ball.Position.Y += ballDirect.Y;
                }
                //проверка пересечения рамок окна
                if (gameState.Ball.Position.X <= 0 || gameState.Ball.Position.X + 4 >= virtualWidth)
                {
                    ballDirect.X *= -1;
                }
                if (gameState.Ball.Position.Y <= 0 || gameState.Ball.Position.Y + 4 >= virtualHeight)
                {
                    ballDirect.Y *= -1;
                }
                //пересечение шарика и платформы
                //if (gameState.Platform.Position.X <= gameState.Ball.Position.X + gameState.Ball.Diameter / 2 && gameState.Ball.Position.X + gameState.Ball.Diameter / 2 <= gameState.Platform.Position.X + gameState.Platform.Width && gameState.Platform.Position.Y == gameState.Ball.Position.Y + gameState.Ball.Diameter / 2)
                //{
                //    ballDirect.Y *= -1;
                //}
                var platform = new Rectangle()
                {
                    X = gameState.Platform.Position.X,
                    Y = gameState.Platform.Position.Y,
                    Height = gameState.Platform.Height,
                    Width = gameState.Platform.Width
                };

                var rect1 = new Rectangle()
                {
                    X = platform.X,
                    Y = platform.Y,
                    Height = platform.Height,
                    Width = platform.Width / 5
                };
                var rect2 = new Rectangle()
                {
                    X = platform.X + 4,
                    Y = platform.Y,
                    Height = platform.Height,
                    Width = platform.Width / 5
                };
                var rect3 = new Rectangle()
                {
                    X = platform.X + 8,
                    Y = platform.Y,
                    Height = platform.Height,
                    Width = platform.Width / 5
                };
                var rect4 = new Rectangle()
                {
                    X = platform.X + 12,
                    Y = platform.Y,
                    Height = platform.Height,
                    Width = platform.Width / 5
                };
                var rect5 = new Rectangle()
                {
                    X = platform.X + 16,
                    Y = platform.Y,
                    Height = platform.Height,
                    Width = platform.Width / 5
                };

                if (ball.IntersectsWith(rect1))
                {
                    if (ballDirect.X < 0)
                    {
                        ballDirect.X = (float)-1.7;
                    }
                    else
                    {
                        ballDirect.X = (float)1.7;
                    }
                    ballDirect.Y = (float)-0.3;
                }
                if (ball.IntersectsWith(rect2))
                {
                    if (ballDirect.X < 0)
                    {
                        ballDirect.X = (float)-1.4;
                    }
                    else
                    {
                        ballDirect.X = (float)1.4;
                    }
                    ballDirect.Y = (float)-0.6;
                }
                if (ball.IntersectsWith(rect3))
                {
                    if (ballDirect.X < 0)
                    {
                        ballDirect.X = (float)-1;
                    }
                    else
                    {
                        ballDirect.X = (float)1;
                    }
                    ballDirect.Y = (float)-1;
                }
                if (ball.IntersectsWith(rect4))
                {
                    if (ballDirect.X < 0)
                    {
                        ballDirect.X = (float)-1.4;
                    }
                    else
                    {
                        ballDirect.X = (float)1.4;
                    }
                    ballDirect.Y = (float)-0.6;
                }
                if (ball.IntersectsWith(rect5))
                {
                    if (ballDirect.X < 0)
                    {
                        ballDirect.X = (float)-1.7;
                    }
                    else
                    {
                        ballDirect.X = (float)1.7;
                    }
                    ballDirect.Y = (float)-0.3;
                }

                if (gameState.Ball.Position.Y + gameState.Ball.Diameter >= virtualHeight)
                {
                    gameState.Life--;
                    if (gameState.Life == 0)
                    {
                        _form.EndGame();
                    }
                    _form.gameStarted = false;
                    gameState.StartPositions();
                    ballDirect.X = -1;
                    ballDirect.Y = -1;
                }

                _form.ChangeLifeLabel(gameState.Life.ToString());
                _form.CleanRectangleList();
                if (gameState.invisibleBlocksAtStart + invisibleBlock == gameState.Blocks.Length)
                {
                    _form.WinGame();
                    invisibleBlock = 0;
                    gameState.invisibleBlocksAtStart = 0;
                }
                //проверка позиции шарика относительно блоков
                foreach (Block block in gameState.Blocks)
                {
                    if (block.Visible)
                    {
                        var x = Convert.ToInt32(block.Position.X);
                        var y = Convert.ToInt32(block.Position.Y);
                        var height = Convert.ToInt32(block.Height);
                        var width = Convert.ToInt32(block.Width);
                        var rect = new Rectangle(x, y, width, height);

                        if (ball.IntersectsWith(rect))
                        {
                            var c = Math.Sqrt(Math.Pow(block.Height, 2) + Math.Pow(block.Width, 2));
                            var sin = (block.Height / c);
                            var angleA = Math.Asin(sin) * (180 / Math.PI);
                            var horizonAngle = angleA * 2;
                            var verticalAngle = 180 - horizonAngle;

                            var topleft = horizonAngle / 2;
                            var topright = topleft + verticalAngle;
                            var botRight = topright + horizonAngle;
                            var botLeft = botRight + verticalAngle;

                            float x1 = ball.X;
                            float y1 = ball.Y;
                            float x2 = block.Position.X + (block.Width / 2);
                            float y2 = block.Position.Y + (block.Height / 2);
                            float ballAngle = (float)(Math.Atan2(y1 - y2, x1 - x2) / Math.PI * 180);
                            ballAngle = (ballAngle < 0) ? ballAngle + 360 : ballAngle;
                            if (y1 > y2)
                            {
                                ballAngle += 180;
                            }
                            if (ballAngle == topleft || ballAngle == topright || ballAngle == botLeft || ballAngle == botRight)
                            {
                                ballDirect.X *= -1;
                                ballDirect.Y *= -1;
                                block.Visible = false;
                                invisibleBlock++;
                                break;
                            }
                            if (0 <= ballAngle && ballAngle < topleft || botLeft < ballAngle && ballAngle <= 360)
                            {
                                ballDirect.X *= -1;
                                block.Visible = false;
                                invisibleBlock++;
                                break;
                            }
                            if (topleft < ballAngle && ballAngle < topright)
                            {
                                ballDirect.Y *= -1;
                                block.Visible = false;
                                invisibleBlock++;
                                break;
                            }
                            if (topright < ballAngle && ballAngle < botRight)
                            {
                                ballDirect.X *= -1;
                                block.Visible = false;
                                invisibleBlock++;
                                break;
                            }
                            if (botRight < ballAngle && ballAngle < botLeft)
                            {
                                ballDirect.Y *= -1;
                                block.Visible = false;
                                invisibleBlock++;
                                break;
                            }
                            //if ((rect.Bottom - ball.Top) <= 1)
                            //{
                            //    ballDirect.Y *= -1;
                            //    block.Visible = false;
                            //    invisibleBlock++;
                            //    break;
                            //}
                            //if ((rect.Top - ball.Bottom) <= 1)
                            //{
                            //    ballDirect.Y *= -1;
                            //    block.Visible = false;
                            //    invisibleBlock++;
                            //    break;
                            //}
                            //if ((rect.Left - ball.Right) <=1)
                            //{
                            //    ballDirect.X *= -1;
                            //    block.Visible = false;
                            //    invisibleBlock++;
                            //    break;
                            //}
                            //if ((rect.Right - ball.Left) <= 1)
                            //{
                            //    ballDirect.X *= -1;
                            //    block.Visible = false;
                            //    invisibleBlock++;
                            //    break;
                            //}
                        }

                        //var point = block.Position;

                        //if (point.X <= gameState.Ball.Position.X + gameState.Ball.Diameter / 2 && gameState.Ball.Position.X + gameState.Ball.Diameter / 2 <= point.X + block.Width && point.Y == gameState.Ball.Position.Y + gameState.Ball.Diameter / 2)
                        //{
                        //    ballDirect.Y *= -1;
                        //    block.Visible = false;
                        //    invisibleBlock++;
                        //    break;
                        //}
                        //if (point.X <= gameState.Ball.Position.X + gameState.Ball.Diameter / 2 && gameState.Ball.Position.X + gameState.Ball.Diameter / 2 <= point.X + block.Width && gameState.Ball.Position.Y + gameState.Ball.Diameter / 2 == point.Y + block.Height)
                        //{
                        //    ballDirect.Y *= -1;
                        //    block.Visible = false;
                        //    invisibleBlock++;
                        //    break;
                        //}
                        //if (point.Y <= gameState.Ball.Position.Y + gameState.Ball.Diameter / 2 && gameState.Ball.Position.Y + gameState.Ball.Diameter / 2 <= point.Y + block.Height && gameState.Ball.Position.X + gameState.Ball.Diameter / 2 == point.X)
                        //{
                        //    ballDirect.X *= -1;
                        //    block.Visible = false;
                        //    invisibleBlock++;
                        //    break;
                        //}
                        //if (point.Y <= gameState.Ball.Position.Y + gameState.Ball.Diameter / 2 && gameState.Ball.Position.Y + gameState.Ball.Diameter / 2 <= point.Y + block.Height && gameState.Ball.Position.X + gameState.Ball.Diameter / 2 == point.X + block.Width)
                        //{
                        //    ballDirect.X *= -1;
                        //    block.Visible = false;
                        //    invisibleBlock++;
                        //    break;
                        //}
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

        //private void ScalingBlockPositions(int height, int width)
        //{
        //    if (blocks.Length != 0)
        //    {
        //        int x = blocks.GetUpperBound(0) + 1;
        //        int y = blocks.Length / x;
        //        for (int i = 0; i < x; i++)
        //        {
        //            for (int j = 0; j < y; j++)
        //            {
        //                blocks[i, j].Position = new PointF
        //                {
        //                    X = float.Parse(i.ToString()) * ConvertFloatToInt((float)0.1, width),
        //                    Y = float.Parse(j.ToString()) * ConvertFloatToInt((float)0.033, height)
        //                };
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// перемещение платформы влево
        /// в начале игры и после потери жизни вместе с платформой двигается шарик
        /// </summary>
        public void ChangePlatformPositionToLeft(int platformHaste)
        {
            if (gameState.Platform.Position.X > 0)
            {
                gameState.Platform.Position.X -= platformHaste;

                if (!_form.gameStarted)
                {
                    gameState.Ball.Position.X -= platformHaste;

                    _form.RefreshMainWindow();
                }
            }
        }

        /// <summary>
        /// перемещение платформы влево
        /// в начале игры и после потери жизни вместе с платформой двигается шарик
        /// </summary>
        public void ChangePlatformPositionToRight(int platformHaste)
        {
            if (gameState.Platform.Position.X < virtualWidth - gameState.Platform.Width)
            {
                gameState.Platform.Position.X += platformHaste;

                if (!_form.gameStarted)
                {
                    gameState.Ball.Position.X += platformHaste;

                    _form.RefreshMainWindow();
                }
            }
        }

        /*
         * сделать виртуальные координаты
         * с помощью них вычислять всю логику
         * поубирать конверты
         * формула скейла - размер реального окна делить на размер виртуальног окна и умножить на коеффициенты
         * вычислять перед отрисовкой
         *
         */
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