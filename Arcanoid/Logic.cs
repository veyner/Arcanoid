using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Arcanoid
{
    public class Logic
    {
        public Timer LogicTimer;

        public int VirtualHeight = 300;

        public int VirtualWidth = 200;

        private GameState _gameState;
        private PointF ballDirect;
        private PointF startAndCentralPlatDirect = new PointF((float)-1, (float)-1); // направление движения шарика - начальное и при отбитии от центральной части платформы 
        private PointF secondAndFourthPlatDirect = new PointF((float)-1.4, (float)-0.6); // направление движения шарика при отбитии от второй и четвертой части платформы
        private PointF firstAndFifthPlatDirect = new PointF((float)-1.7, (float)-0.3); // направление движения шарика при отбитии от первой и пятой части платформы
        private int invisibleBlock = 0;
        private List<Block> intersectedBlockList = new List<Block>();

        private bool ballOutOfPlatform = true; // отбился ли шарик от платформы

        public Logic(GameState gameState)
        {
            _gameState = gameState;
            LogicTimer = new Timer();
            LogicTimer.Interval = 10;
            LogicTimer.Elapsed += LogicTimer_Elapsed;
        }

        private void LogicTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckGameStatus();
        }

        public void CleanInvisibleBlockList()
        {
            invisibleBlock = 0;
            _gameState.InvisibleBlocksAtStart = 0;
        }
        /// <summary>
        /// остаток блоков - если все блоки выбиты - игра завершается
        /// </summary>
        private void CheckCurrentBlocksNumber()
        {
            if (_gameState.InvisibleBlocksAtStart + invisibleBlock == _gameState.Blocks.Length)
            {
                _gameState.WinGame = true;
            }
        }
        /// <summary>
        /// Проверка шарика отностительно нижней части игрового поля
        /// </summary>
        private void CheckBallOutOfGameWindow(RectangleF ball)
        {
            if (ball.Y + _gameState.Ball.Diameter >= VirtualHeight)
            {
                _gameState.Life--;
                if (_gameState.Life == 0)
                {
                    _gameState.LoseGame = true;
                }
                _gameState.GameStarted = false;
                _gameState.StartPositions();
                StartDirection();
                ballOutOfPlatform = true;
            }

            if (ball.X <= 0 || ball.X + _gameState.Ball.Diameter >= VirtualWidth)
            {
                ballDirect.X *= -1;
            }
            if (ball.Y <= 0 || ball.Y + _gameState.Ball.Diameter >= VirtualHeight)
            {
                ballDirect.Y *= -1;
                ballOutOfPlatform = false;
            }
        }
        /// <summary>
        /// Удаление блоков в зависимости от того, сколько блоков выбил шарик
        /// </summary>
        /// <param name="ball"></param>
        private void RemoveBlocks(RectangleF ball)
        {
            if (intersectedBlockList.Count == 1)
            {
                RemoveSingleBlock(intersectedBlockList[0], ball);
                ballOutOfPlatform = false;
            }
            else if (intersectedBlockList.Count == 2)
            {
                var block1 = intersectedBlockList[0];
                var block2 = intersectedBlockList[1];

                if (block1.Position.X == block2.Position.X)
                {
                    ballDirect.X *= -1;
                    block1.Visible = false;
                    block2.Visible = false;
                    invisibleBlock += 2;
                }
                else if (block1.Position.Y == block2.Position.Y)
                {
                    ballDirect.Y *= -1;
                    block1.Visible = false;
                    block2.Visible = false;
                    invisibleBlock += 2;
                }
                else
                {
                    ballDirect.Y *= -1;
                    ballDirect.X *= -1;
                    block1.Visible = false;
                    block2.Visible = false;
                    invisibleBlock += 2;
                }
                ballOutOfPlatform = false;
            }
            intersectedBlockList.Clear();
            }

        /// <summary>
        /// Проверка столкновений шарика и блоков, просчет положения шарика 
        /// </summary>
        public void CheckGameStatus()
        {
            {
                var ball = new RectangleF
                {
                    X = _gameState.Ball.Position.X,
                    Y = _gameState.Ball.Position.Y,
                    Height = _gameState.Ball.Diameter,
                    Width = _gameState.Ball.Diameter
                };
                CheckCurrentBlocksNumber();
                CheckBallOutOfGameWindow(ball);
                
                BallAndPlatformIntersection(ball);
                _gameState.ChangeLife = true;

                //проверка - сколько блоков зацепил шарик
                CheckBlock(ball);
                RemoveBlocks(ball);
                if (_gameState.GameStarted && !_gameState.Pause)
                {
                    ball.X += ballDirect.X;
                    ball.Y += ballDirect.Y;
                    _gameState.Ball.Position.X = ball.X;
                    _gameState.Ball.Position.Y = ball.Y;
                }
            }
        }

        /// <summary>
        /// Вычисление угла между верхней стороной прямоугольника и его диагональю
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private float CountRectangleAngle(float height, float width)
        {
            var c = (float)(Math.Sqrt(Math.Pow(height, 2) + Math.Pow(width, 2))); //длина диагонали прямоугольника
            var sin = height / c; //синус угла между верхней стороной прямоугольника и диагональю
            return (float)(Math.Asin(sin) * (180 / Math.PI)); //градус угла между верхней стороной прямоугольника и диагональю
        }

        /// <summary>
        /// Вычисление угла прилета шарика к платформе
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        private float CountBallAngleToPlatform(RectangleF ball, RectangleF platform)
        {
            float x1 = ball.X + ball.Width / 2;
            float y1 = ball.Y + ball.Height / 2;
            float x2 = platform.X + (platform.Width / 2);
            float y2 = platform.Y + (platform.Height / 2);

            float ballAngle= (float)(Math.Atan2(y1 - y2, x1 - x2) / Math.PI * 180);
            ballAngle = (ballAngle < 0) ? ballAngle + 360 : ballAngle;
            if (y2 > y1)
            {
                ballAngle -= 180;
            }
            else
            {
                ballAngle += 180;
            }
            return ballAngle;
        }

        /// <summary>
        /// Проверка пересечения шарика и платформы
        /// </summary>
        /// <param name="ball"></param>
        private void BallAndPlatformIntersection(RectangleF ball)
        {
            //пересечение шарика и платформы
            var platform = new RectangleF()
            {
                X = _gameState.Platform.Position.X,
                Y = _gameState.Platform.Position.Y,
                Height = _gameState.Platform.Height,
                Width = _gameState.Platform.Width
            };
            if (!ballOutOfPlatform)
            {
                if (ball.IntersectsWith(platform))
                {
                    ballOutOfPlatform = true;
                    //вычисление внутренних углов платформы образованные диагоналями
                    var horizonAngle = CountRectangleAngle(platform.Height, platform.Width) * 2;
                    var verticalAngle = 180 - horizonAngle;

                    //градус каждого угла платформы
                    var topleft = horizonAngle / 2;
                    var topright = topleft + verticalAngle;
                    var botRight = topright + horizonAngle;
                    var botLeft = botRight + verticalAngle;

                    //вычисление угла под которым шарик прилетает в платформу
                    float ballAngle = CountBallAngleToPlatform(ball,platform);
                    var platformPartAngle = verticalAngle / 5;
                    //разделение платформы на 5 частей
                    var platformAngle1 = topleft + platformPartAngle;
                    var platformAngle2 = platformAngle1 + platformPartAngle;
                    var platformAngle3 = platformAngle2 + platformPartAngle;
                    var platformAngle4 = platformAngle3 + platformPartAngle;
                    var intersectRect = RectangleF.Intersect(ball, platform);
                    //у каждой части свой угол отбивания шарика
                    if (intersectRect.Width >= intersectRect.Height && ballAngle > topleft && ballAngle<topright)
                    {
                        if (topleft<=ballAngle && ballAngle<=platformAngle1 || platformAngle4<=ballAngle && ballAngle<=topright)
                        {
                            if (ballDirect.X< 0)
                            {
                                ballDirect.X = firstAndFifthPlatDirect.X;
                            }
                            else
                            {
                                ballDirect.X = firstAndFifthPlatDirect.X *(-1);
                            }
                            ballDirect.Y = firstAndFifthPlatDirect.Y;
                        }
                        else if (platformAngle1<ballAngle && ballAngle<platformAngle2 || platformAngle3<ballAngle && ballAngle<platformAngle4)
                        {
                            if (ballDirect.X< 0)
                            {
                                ballDirect.X = secondAndFourthPlatDirect.X;
                            }
                            else
                            {
                                ballDirect.X = secondAndFourthPlatDirect.X *(-1);
                            }
                            ballDirect.Y = secondAndFourthPlatDirect.Y;
                        }
                        else if (platformAngle2<=ballAngle && ballAngle<=platformAngle3)
                        {
                            if (ballDirect.X< 0)
                            {
                                ballDirect.X = startAndCentralPlatDirect.X;
                            }
                            else
                            {
                                ballDirect.X = startAndCentralPlatDirect.X *(-1);
                            }
                            ballDirect.Y = startAndCentralPlatDirect.Y;
                        }
                    }
                    //если шарик прилетает в боковую сторону платформы, он меняет направление на обратное
                    else if (ballDirect.X< 0)
                    {
                        if (0 <= ballAngle && ballAngle<topleft || ballAngle <= 360 && botLeft<=ballAngle)
                        {
                            ballDirect.Y *= -1;
                        }
                        else if (topright<=ballAngle && ballAngle<=botRight)
                        {
                            ballDirect.X *= -1;
                            ballDirect.Y *= -1;
                        }
                    }
                    else if (ballDirect.X > 0)
                    {
                        if (0 <= ballAngle && ballAngle<=topleft || ballAngle <= 360 && botLeft<=ballAngle)
                        {
                            ballDirect.X *= -1;
                            ballDirect.Y *= -1;
                        }
                        else if (topright<ballAngle && ballAngle<=botRight)
                        {
                            ballDirect.Y *= -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверка позиции шарика относительно блоков
        /// </summary>
        /// <param name="ball"></param>
        private void CheckBlock(RectangleF ball)
        {
            foreach (Block block in _gameState.Blocks)
            {
                if (block.Visible)
                {
                    var x = block.Position.X;
                    var y = block.Position.Y;
                    var height = block.Height;
                    var width = block.Width;
                    var rect = new RectangleF(x, y, width, height);
                    if (ball.IntersectsWith(rect))
                    {
                        intersectedBlockList.Add(block);
                    }
                }
            }
        }

        /// <summary>
        /// Вычисление под каким углом шарик прилетел в блок
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        private float CountBallAngleToBlock(RectangleF ball, Block block)
        {
            float x1 = ball.X + _gameState.Ball.Diameter / 2;
            float y1 = ball.Y + _gameState.Ball.Diameter / 2;
            float x2 = block.Position.X + (block.Width / 2);
            float y2 = block.Position.Y + (block.Height / 2);

            var tanY = y1 - y2;
            var tanX = x1 - x2;
            float ballAngle = (float)(Math.Atan2(tanY, tanX) / Math.PI * 180);
            ballAngle = (ballAngle < 0) ? ballAngle + 180 : ballAngle;
            if (y2 <= y1)
            {
                ballAngle += 180;
            }
            return ballAngle;
        }
        
        /// <summary>
        /// Проверка позиции шарика относительно блоков и удаление блока если коснулся шарик
        /// </summary>
        /// <param name="block"></param>
        /// <param name="ball"></param>
        private void RemoveSingleBlock(Block block, RectangleF ball)
        {
            var horizonAngle = CountRectangleAngle(block.Height,block.Width) * 2;
            var verticalAngle = 180 - horizonAngle;

            var topleft = horizonAngle / 2;
            var topright = topleft + verticalAngle;
            var botRight = topright + horizonAngle;
            var botLeft = botRight + verticalAngle;

            var ballAngle = CountBallAngleToBlock(ball, block);
           
            if (ballDirect.X<0 && ballDirect.Y<0)
            {
                if (botRight < ballAngle && ballAngle <= botLeft)
                {
                ballDirect.Y *= -1;
                block.Visible = false;
                invisibleBlock++;
                }
                else if (topright <= ballAngle && ballAngle < botRight)
                {
                ballDirect.X *= -1;
                block.Visible = false;
                invisibleBlock++;
                }
            }
            else if(ballDirect.X>0 && ballDirect.Y>0)
            {
                if (0 <= ballAngle && ballAngle < topleft || botLeft <= ballAngle && ballAngle <= 360)
                {
                ballDirect.X *= -1;
                block.Visible = false;
                invisibleBlock++;
                }
                else if (topleft < ballAngle && ballAngle <= topright)
                {
                ballDirect.Y *= -1;
                block.Visible = false;
                invisibleBlock++;
                }

            }
            else if(ballDirect.X<0 && ballDirect.Y>0)
            {
                if (topleft <= ballAngle && ballAngle < topright)
                {
                ballDirect.Y *= -1;
                block.Visible = false;
                invisibleBlock++;
                }
                else if (topright < ballAngle && ballAngle <= botRight)
                {
                ballDirect.X *= -1;
                block.Visible = false;
                invisibleBlock++;
                }
            }
            else if(ballDirect.X>0 && ballDirect.Y<0)
            {
                if (botRight <= ballAngle && ballAngle < botLeft)
                {
                ballDirect.Y *= -1;
                block.Visible = false;
                invisibleBlock++;
                }
                else if (0 <= ballAngle && ballAngle <= topleft || botLeft < ballAngle && ballAngle <= 360)
                {
                ballDirect.X *= -1;
                block.Visible = false;
                invisibleBlock++;
                }
            }
        }

        /// <summary>
        /// Первоначальное направление шарика
        /// </summary>
        public void StartDirection()
        {
            ballDirect = startAndCentralPlatDirect;
        }

        /// <summary>
        /// Выставление всех первоначальных значений (положение шарика и платформы, направление шарика)
        /// Установка сложности
        /// </summary>
        public void StartGame(String difficulty)
        {
            _gameState.GameStarted = false;
            CleanInvisibleBlockList();
            _gameState.StartPositions();
            _gameState.SwitchDifficulty(difficulty);
            StartDirection();
        }
    }
}