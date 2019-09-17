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
        private MainForm _form;

        public int virtualHeight = 300;

        public int virtualWidth = 200;

        public GameState gameState = new GameState();
        private PointF ballDirect;
        private PointF startAndCentralPlatDirect = new PointF((float)-1, (float)-1); // направление движения шарика - начальное и при отбитии от центральной части платформы 
        private PointF secondAndFourthPlatDirect = new PointF((float)-1.4, (float)-0.6); // направление движения шарика при отбитии от второй и четвертой части платформы
        private PointF firstAndFifthPlatDirect = new PointF((float)-1.7, (float)-0.3); // направление движения шарика при отбитии от первой и пятой части платформы
        private int invisibleBlock = 0;
        private List<Block> intersectedBlockList = new List<Block>();

        private bool ballOutOfPlatform = true; // отбился ли шарик от платформы

        public Logic(MainForm form)
        {
            _form = form;
        }

        public void CleanInvisibleBlockList()
        {
            invisibleBlock = 0;
            gameState.invisibleBlocksAtStart = 0;
        }

        /// <summary>
        /// Просчет положения шарика, проверка столкновений шарика и блоков
        /// </summary>
        public void GameLogic()
        {
            {
                // остаток блоков - если все блоки выбиты - игра завершается
                if (gameState.invisibleBlocksAtStart + invisibleBlock == gameState.Blocks.Length)
                {
                    _form.WinGame();
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
                    StartDirection();
                    ballOutOfPlatform = true;
                }

                var ball = new Rectangle
                {
                    X = Convert.ToInt32(gameState.Ball.Position.X)-1,
                    Y = Convert.ToInt32(gameState.Ball.Position.Y)-1,
                    Height = Convert.ToInt32(gameState.Ball.Diameter)+1,
                    Width = Convert.ToInt32(gameState.Ball.Diameter)+1
                };

                if (_form.gameStarted && !_form.pause)
                {
                    gameState.Ball.Position.X += ballDirect.X;
                    gameState.Ball.Position.Y += ballDirect.Y;
                }
                //проверка пересечения рамок окна
                if (gameState.Ball.Position.X <= 0 || gameState.Ball.Position.X + gameState.Ball.Diameter >= virtualWidth)
                {
                    ballDirect.X *= -1;
                }
                if (gameState.Ball.Position.Y <= 0 || gameState.Ball.Position.Y + gameState.Ball.Diameter >= virtualHeight)
                {
                    ballDirect.Y *= -1;
                    ballOutOfPlatform = false;
                }

                BallAndPlatformIntersection(ball);

                _form.ChangeLifeLabel(gameState.Life.ToString());
                _form.CleanRectangleList();

                //проверка - сколько блоков зацепил шарик
                CheckBlock(ball);

                if (intersectedBlockList.Count == 1)
                {
                    DeleteBlock(intersectedBlockList[0], ball);
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
                else if (intersectedBlockList.Count == 3)
                {
                    foreach (Block block in intersectedBlockList)
                    {
                        block.Visible = false;
                    }
                    ballDirect.Y *= -1;
                    ballDirect.X *= -1;
                    invisibleBlock += 3;
                    ballOutOfPlatform = false;
                }
                intersectedBlockList.Clear();
            }
        }

        private void BallAndPlatformIntersection(Rectangle ball)
        {
            //пересечение шарика и платформы
            var platform = new Rectangle()
            {
                X = gameState.Platform.Position.X - 1,
                Y = gameState.Platform.Position.Y - 1,
                Height = gameState.Platform.Height + 1,
                Width = gameState.Platform.Width + 1
            };
            if (!ballOutOfPlatform)
            {
                if (ball.IntersectsWith(platform))
                {
                    ballOutOfPlatform = true;
                    //вычисление внутренних углов платформы образованные диагоналями
                    var c = Math.Sqrt(Math.Pow(platform.Height, 2) + Math.Pow(platform.Width, 2));
                    var sin = (platform.Height / c);
                    var angleA = Math.Asin(sin) * (180 / Math.PI);
                    var horizonAngle = angleA * 2;
                    var verticalAngle = 180 - horizonAngle;

                    //градус каждого угла платформы
                    var topleft = horizonAngle / 2;
                    var topright = topleft + verticalAngle;
                    var botRight = topright + horizonAngle;
                    var botLeft = botRight + verticalAngle;

                    float x1 = ball.X + ball.Width / 2;
                    float y1 = ball.Y + ball.Height / 2;
                    float x2 = platform.X + (platform.Width / 2);
                    float y2 = platform.Y + (platform.Height / 2);
                    //вычисление угла под которым шарик прилетает в платформу
                    float ballAngle = (float)(Math.Atan2(y1 - y2, x1 - x2) / Math.PI * 180);
                    ballAngle = (ballAngle< 0) ? ballAngle + 360 : ballAngle;
                    if (y2 > y1)
                    {
                        ballAngle -= 180;
                    }
                    else
                    {
                        ballAngle += 180;
                    }
                    var platformPartAngle = verticalAngle / 5;
                    //разделение платформы на 5 частей
                    var platformAngle1 = topleft + platformPartAngle;
                    var platformAngle2 = platformAngle1 + platformPartAngle;
                    var platformAngle3 = platformAngle2 + platformPartAngle;
                    var platformAngle4 = platformAngle3 + platformPartAngle;
                    var intersectRect = Rectangle.Intersect(ball, platform);
                    //у каждой части свой угол отбивания шарика
                    if (intersectRect.Width >= intersectRect.Height && ballAngle > topleft && ballAngle<topright)
                    {
                        if (topleft<ballAngle && ballAngle<platformAngle1 || platformAngle4<ballAngle && ballAngle<topright)
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
                        else if (platformAngle2<ballAngle && ballAngle<platformAngle3)
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
                        if (0 <= ballAngle && ballAngle<topleft || ballAngle <= 360 && botLeft<ballAngle)
                        {
                            ballDirect.Y *= -1;
                        }
                        else if (topright<=ballAngle && ballAngle<botRight)
                        {
                            ballDirect.X *= -1;
                            ballDirect.Y *= -1;
                        }
                    }
                    else if (ballDirect.X > 0)
                    {
                        if (0 <= ballAngle && ballAngle<=topleft || ballAngle <= 360 && botLeft<ballAngle)
                        {
                            ballDirect.X *= -1;
                            ballDirect.Y *= -1;
                        }
                        else if (topright<ballAngle && ballAngle<botRight)
                        {
                            ballDirect.Y *= -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// проверка позиции шарика относительно блоков
        /// </summary>
        /// <param name="ball"></param>
        private void CheckBlock(Rectangle ball)
        {
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
                        intersectedBlockList.Add(block);
                    }
                }
            }
        }
        
        /// <summary>
        /// проверка позиции шарика относительно блоков и удаление блока если коснулся шарик
        /// </summary>
        /// <param name="block"></param>
        /// <param name="ball"></param>
        private void DeleteBlock(Block block, Rectangle ball)
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

            float x1 = ball.X + gameState.Ball.Diameter / 2;
            float y1 = ball.Y + gameState.Ball.Diameter / 2;
            float x2 = block.Position.X + (block.Width / 2);
            float y2 = block.Position.Y + (block.Height / 2);

            var tanY = y1 - y2;
            var tanX = x1 - x2;
            float ballAngle = (float)(Math.Atan2(tanY, tanX) / Math.PI * 180);
            ballAngle = (ballAngle < 0) ? ballAngle + 180 : ballAngle;
            if (ballDirect.Y < 0)
            {
                if(y2<=y1)
                {
                    ballAngle += 180;
                }
            }
            else
            {
                if(y2<=y1)
                {
                    ballAngle += 180;
                }
            }
            
            if(ballDirect.X<0 && ballDirect.Y<0)
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
        /// первоначальное направление шарика
        /// </summary>
        public void StartDirection()
        {
            ballDirect = startAndCentralPlatDirect;
        }

        /// <summary>
        /// перемещение платформы влево
        /// в начале игры и после потери жизни вместе с платформой двигается шарик
        /// </summary>
        public void ChangePlatformPositionToLeft(int platformHaste)
        {
            if (gameState.Platform.Position.X - platformHaste >= 0)
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
            if (gameState.Platform.Position.X+platformHaste <= virtualWidth - gameState.Platform.Width)
            {
                gameState.Platform.Position.X += platformHaste;

                if (!_form.gameStarted)
                {
                    gameState.Ball.Position.X += platformHaste;

                    _form.RefreshMainWindow();
                }
            }
        }
    }
}