using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;

namespace Arcanoid
{
    public class Logic
    {
        public Timer LogicTimer; // Таймер просчет логики игры

        //Виртуальные координаты, относительно которых считается вся логика игры
        public int VirtualHeight = 300; 
        public int VirtualWidth = 200;

        private GameState _gameState;
        private PointF ballMoveStepAndDirect; // Направление и шаг перемещения шарика
        private PointF startAndCentralPlatDirect = new PointF((float)-1, (float)-1); // Направление движения шарика - начальное и при отбитии от центральной части платформы 
        private PointF secondAndFourthPlatDirect = new PointF((float)-1.4, (float)-0.6); // Направление движения шарика при отбитии от второй и четвертой части платформы
        private PointF firstAndFifthPlatDirect = new PointF((float)-1.7, (float)-0.3); // Направление движения шарика при отбитии от первой и пятой части платформы
        private int invisibleBlock = 0;

        // Переменная для проверки отбился ли шарик от платформы, позволяя избежать многократного вычисления. 
        // Изменяется на false при отбитии шарика от блока или верхней стороны игрового поля
        private bool ballOutOfPlatform = true; 


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

        /// <summary>
        /// Очистка количества невидимых блоков
        /// </summary>
        private void CleanInvisibleBlock()
        {
            invisibleBlock = 0;
            _gameState.InvisibleBlocksAtStart = 0;
        }

        /// <summary>
        /// Удаление блока/ов с игрового поля (блок становится невидимым)
        /// </summary>
        /// <param name="block"></param>
        /// <param name="blockQuantity">количество блоков для удаления</param>
        private void RemoveBlock(Block block, int blockQuantity)
        {
            block.Visible = false;
            invisibleBlock += blockQuantity;
        }

        /// <summary>
        /// Проверка количества невидимых блоков
        /// </summary>
        private void CheckInvisibleBlocksNumber()
        {
            if (_gameState.InvisibleBlocksAtStart + invisibleBlock == _gameState.Blocks.Length)
            {
                _gameState.WinGame = true;
            }
        }

        /// <summary>
        /// Проверка позиции шарика отностительно сторон игрового поля
        /// </summary>
        private void CheckBallOutOfGameWindow(RectangleF ball)
        {
            //Eсли шарик вылетел за нижний предел игрового окна - отнимается жизнь. 
            //Шарик и платформы возвращаются в стартовую позицию, задается первоначальное направление шарика
            //Eсли число жизней = 0 - игра завершается
            if (ball.Y + _gameState.Ball.Diameter >= VirtualHeight)
            {
                _gameState.Life--;
                _gameState.ChangeLife = true;
                if (_gameState.Life == 0)
                {
                    _gameState.LoseGame = true;
                }
                _gameState.GameStarted = false;
                _gameState.SetStartPositions();
                SetStartDirection();
                ballOutOfPlatform = true;
            }

            if (ball.X <= 0 || ball.X + ball.Width >= VirtualWidth)
            {
                ballMoveStepAndDirect.X *= -1;
            }
            if (ball.Y <= 0)
            {
                ballMoveStepAndDirect.Y *= -1;
                ballOutOfPlatform = false;
            }
        }

        /// <summary>
        /// Изменить направление шарика при столкновении с блоком или блоками
        /// </summary>
        /// <param name="ball"></param>
        private void ChangeBallDirectionAtCollisionWithBlocks(RectangleF ball, List<Block> intersectedBlockList)
        {
            if (intersectedBlockList.Count == 1)
            {
                if (CheckBallCollisionWithSingleBlock(intersectedBlockList[0], ball))
                {
                    RemoveBlock(intersectedBlockList[0], intersectedBlockList.Count);
                }
                ballOutOfPlatform = false;
            }
            else if (intersectedBlockList.Count == 2)
            {
                if (ChangeBallDirectionAtCollisionWithTwoBlocks(intersectedBlockList))
                {
                    foreach (Block block in intersectedBlockList)
                    {
                        RemoveBlock(block, intersectedBlockList.Count);
                    }
                }
                ballOutOfPlatform = false;
            }
        }

        /// <summary>
        /// Изменение направления шарика при столкновении с двумя блоками
        /// </summary>
        /// <param name="intersectedBlockList"></param>
        /// <returns>true - позволяет удалить блоки с игрового поля</returns>
        private bool ChangeBallDirectionAtCollisionWithTwoBlocks(List<Block> intersectedBlockList)
        {
            var block1 = intersectedBlockList[0];
            var block2 = intersectedBlockList[1];

            if (block1.BlockRectangle.X == block2.BlockRectangle.X)
            {
                ballMoveStepAndDirect.X *= -1;
                return true;
            }
            else if (block1.BlockRectangle.Y == block2.BlockRectangle.Y)
            {
                ballMoveStepAndDirect.Y *= -1;
                return true;
            }
            else
            {
                ballMoveStepAndDirect.Y *= -1;
                ballMoveStepAndDirect.X *= -1;
                return true;
            }
        }

        /// <summary>
        /// Проверка состояния игры
        /// Изменение направления движения шарика
        /// Перемещение шарика
        /// </summary>
        public void CheckGameStatus()
        {
            var ballRectangle = _gameState.Ball.BallRectangle;
            CheckInvisibleBlocksNumber();
            CheckBallOutOfGameWindow(ballRectangle);
            CheckBallAndPlatformCollision(ballRectangle);
            var intersectedBlocks = CheckBallCollisionWithBlocks(ballRectangle);//проверка - сколько блоков зацепил шарик
            ChangeBallDirectionAtCollisionWithBlocks(ballRectangle, intersectedBlocks);
            MoveBall(ballRectangle);
        }

        /// <summary>
        /// Перемещение шарика на заданный шаг
        /// </summary>
        /// <param name="ballRectangle"></param>
        private void MoveBall(RectangleF ballRectangle)
        {
            if (_gameState.GameStarted && !_gameState.Pause)
            {
                ballRectangle.X += ballMoveStepAndDirect.X;
                ballRectangle.Y += ballMoveStepAndDirect.Y;
                _gameState.Ball.BallRectangle = ballRectangle;
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
            var c = (float)(Math.Sqrt(Math.Pow(height, 2) + Math.Pow(width, 2))); //Длина диагонали прямоугольника
            var sin = height / c; //Синус угла между верхней стороной прямоугольника и диагональю
            return (float)(Math.Asin(sin) * (180 / Math.PI)); //Градус угла между верхней стороной прямоугольника и диагональю
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
        /// Проверка столкновения шарика и платформы
        /// </summary>
        /// <param name="ballRect"></param>
        private void CheckBallAndPlatformCollision(RectangleF ballRect)
        {
            var platRect = _gameState.Platform.PlatformRectangle;
            if (!ballOutOfPlatform)
            {
                if (ballRect.IntersectsWith(platRect))
                {
                    ballOutOfPlatform = true; //пересечение шарика и платформы
                    ChangeBallDirectionAtPlatformCollision(ballRect, platRect);
                }
            }
        }

        /// <summary>
        /// Изменение направления шарика при столкновении с платформой
        /// </summary>
        /// <param name="ballRect"></param>
        private void ChangeBallDirectionAtPlatformCollision(RectangleF ballRect, RectangleF platRect)
        {

            //Вычисление внутренних углов платформы образованные диагоналями
            var horizonAngle = CountRectangleAngle(platRect.Height, platRect.Width) * 2;
            var verticalAngle = 180 - horizonAngle;
            /*С помощью внутренних углов (образованных диагоналями прямоугольника) можно вычислить с какой стороны шарик столкнулся с платформой.
             * От центра до каждого угла прямоугольника лежит отрезок диагонали под определенным углом относительно центра платформы.
             * topleft - угол отрезка до верхнего левого угла
             * topright - угол отрезка до верхнего правого угла
             * borright - угол отрезка до нижнего правого угла
             * botLeft - угол отрезка до нижнего левого угла
             * Нулевая отметка находится на середине левой стороны прямоугольника
             * Так получается диапазон градусов каждой стороны прямоугольника:
             * левая сторона прямоугольника - от botLeft до 360 градусов и от 0 до topLeft
             * верхняя сторона  - от topLeft до topRight
             * правая сторона - от topRight до botRight
             * нижняя сторона - от botLeft до botRight
             * Вычислив угол, под которым шарик столкнулся с платформой относительно центра платформы,
             * можно понять в какую сторону прилетел шарик и куда его нужно отбить
             */
            var topLeft = horizonAngle / 2;
            var topright = topLeft + verticalAngle;
            var botRight = topright + horizonAngle;
            var botLeft = botRight + verticalAngle;

            //Вычисление угла под которым шарик прилетает в платформу
            float ballAngle = CountBallAngleToPlatform(ballRect, platRect);
            var platformPartAngle = verticalAngle / 5;
            //Разделение платформы на 5 частей
            var platformAngle1 = topLeft + platformPartAngle;
            var platformAngle2 = platformAngle1 + platformPartAngle;
            var platformAngle3 = platformAngle2 + platformPartAngle;
            var platformAngle4 = platformAngle3 + platformPartAngle;
            var intersectRect = RectangleF.Intersect(ballRect, platRect);
            //Проверка в какую часть платформы прилетел шарик
            //У каждой платформы свой угол отбития шарика
            if (intersectRect.Width >= intersectRect.Height && ballAngle > topLeft && ballAngle < topright)
            {
                if (topLeft <= ballAngle && ballAngle <= platformAngle1 || platformAngle4 <= ballAngle && ballAngle <= topright)
                {
                    if (ballMoveStepAndDirect.X < 0)
                    {
                        ballMoveStepAndDirect.X = firstAndFifthPlatDirect.X;
                    }
                    else
                    {
                        ballMoveStepAndDirect.X = firstAndFifthPlatDirect.X * (-1);
                    }
                    ballMoveStepAndDirect.Y = firstAndFifthPlatDirect.Y;
                }
                else if (platformAngle1 < ballAngle && ballAngle < platformAngle2 || platformAngle3 < ballAngle && ballAngle < platformAngle4)
                {
                    if (ballMoveStepAndDirect.X < 0)
                    {
                        ballMoveStepAndDirect.X = secondAndFourthPlatDirect.X;
                    }
                    else
                    {
                        ballMoveStepAndDirect.X = secondAndFourthPlatDirect.X * (-1);
                    }
                    ballMoveStepAndDirect.Y = secondAndFourthPlatDirect.Y;
                }
                else if (platformAngle2 <= ballAngle && ballAngle <= platformAngle3)
                {
                    if (ballMoveStepAndDirect.X < 0)
                    {
                        ballMoveStepAndDirect.X = startAndCentralPlatDirect.X;
                    }
                    else
                    {
                        ballMoveStepAndDirect.X = startAndCentralPlatDirect.X * (-1);
                    }
                    ballMoveStepAndDirect.Y = startAndCentralPlatDirect.Y;
                }
            }
            //Если шарик прилетает в боковую сторону платформы
            //В зависимости от направления его движения:
            //Направление шарика меняется по Y или по X и Y
            else if (ballMoveStepAndDirect.X < 0)
            {
                if (0 <= ballAngle && ballAngle < topLeft || ballAngle <= 360 && botLeft <= ballAngle)
                {
                    ballMoveStepAndDirect.Y *= -1;
                }
                else if (topright <= ballAngle && ballAngle <= botRight)
                {
                    ballMoveStepAndDirect.X *= -1;
                    ballMoveStepAndDirect.Y *= -1;
                }
            }
            else if (ballMoveStepAndDirect.X > 0)
            {
                if (0 <= ballAngle && ballAngle <= topLeft || ballAngle <= 360 && botLeft <= ballAngle)
                {
                    ballMoveStepAndDirect.X *= -1;
                    ballMoveStepAndDirect.Y *= -1;
                }
                else if (topright < ballAngle && ballAngle <= botRight)
                {
                    ballMoveStepAndDirect.Y *= -1;
                }
            }
        }

        /// <summary>
        /// Подсчет сколько блоков шарик задел при столкновении
        /// </summary>
        /// <param name="ball"></param>
        private List<Block> CheckBallCollisionWithBlocks(RectangleF ball)
        {
            List<Block> intersectedBlockList = new List<Block>();
            foreach (Block block in _gameState.Blocks)
            {
                if (block.Visible)
                {
                    if (ball.IntersectsWith(block.BlockRectangle))
                    {
                        intersectedBlockList.Add(block);
                    }
                }
            }
            return intersectedBlockList;
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
            float x2 = block.BlockRectangle.X + (block.Width / 2);
            float y2 = block.BlockRectangle.Y + (block.Height / 2);

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
        /// Изменение направления движения шарика при столкновении с единичным блоком
        /// </summary>
        /// <param name="block"></param>
        /// <param name="ball"></param>
        /// <returns>true - позволяет удалить блок, с которым столкнулся шарик</returns>
        private bool CheckBallCollisionWithSingleBlock(Block block, RectangleF ball)
        {
            var horizonAngle = CountRectangleAngle(block.Height,block.Width) * 2;
            var verticalAngle = 180 - horizonAngle;

            var topleft = horizonAngle / 2;
            var topright = topleft + verticalAngle;
            var botRight = topright + horizonAngle;
            var botLeft = botRight + verticalAngle;

            var ballAngle = CountBallAngleToBlock(ball, block); //Вычисление угла, под которым шарик прилетел в блок относительно центра блока
           
            if (ballMoveStepAndDirect.X<0 && ballMoveStepAndDirect.Y<0)
            {
                if (botRight < ballAngle && ballAngle <= botLeft)
                {
                ballMoveStepAndDirect.Y *= -1;
                    return true;
                }
                else if (topright <= ballAngle && ballAngle < botRight)
                {
                ballMoveStepAndDirect.X *= -1;
                    return true;
                }
            }
            else if(ballMoveStepAndDirect.X>0 && ballMoveStepAndDirect.Y>0)
            {
                if (0 <= ballAngle && ballAngle < topleft || botLeft <= ballAngle && ballAngle <= 360)
                {
                ballMoveStepAndDirect.X *= -1;
                    return true;
                }
                else if (topleft < ballAngle && ballAngle <= topright)
                {
                ballMoveStepAndDirect.Y *= -1;
                    return true;
                }
            }
            else if(ballMoveStepAndDirect.X<0 && ballMoveStepAndDirect.Y>0)
            {
                if (topleft <= ballAngle && ballAngle < topright)
                {
                ballMoveStepAndDirect.Y *= -1;
                    return true;
                }
                else if (topright < ballAngle && ballAngle <= botRight)
                {
                ballMoveStepAndDirect.X *= -1;
                    return true;
                }
            }
            else if(ballMoveStepAndDirect.X>0 && ballMoveStepAndDirect.Y<0)
            {
                if (botRight <= ballAngle && ballAngle < botLeft)
                {
                ballMoveStepAndDirect.Y *= -1;
                    return true;
                }
                else if (0 <= ballAngle && ballAngle <= topleft || botLeft < ballAngle && ballAngle <= 360)
                {
                ballMoveStepAndDirect.X *= -1;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Первоначальное направление шарика
        /// </summary>
        public void SetStartDirection()
        {
            ballMoveStepAndDirect = startAndCentralPlatDirect;
        }

        /// <summary>
        /// Выставление всех первоначальных значений (положение шарика и платформы, направление шарика)
        /// Установка сложности
        /// </summary>
        public void StartGame(String difficulty)
        {
            _gameState.GameStarted = false;
            CleanInvisibleBlock();
            _gameState.SetStartPositions();
            _gameState.SwitchDifficulty(difficulty);
            SetStartDirection();
        }
    }
}