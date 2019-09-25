using System;
using System.Collections.Generic;
using System.Timers;

namespace Arcanoid
{
    public class Logic
    {
        public Timer LogicTimer; // Таймер просчет логики игры

        //Виртуальные координаты, относительно которых считается вся логика игры
        public int VirtualHeight = 600;
        public int VirtualWidth = 400;

        private GameState _gameState;
        private LocalPoint _ballVector = new LocalPoint() { X = (float)(-1.5), Y = (float)(-1.5) }; // Направление и шаг перемещения шарика
        private LocalPoint _startAndCentralPlatDirect = new LocalPoint(){ X = (float)(-1.5) , Y = (float)(-1.5) }; // Направление движения шарика - начальное и при отбитии от центральной части платформы 
        private LocalPoint _secondAndFourthPlatDirect = new LocalPoint(){ X= (float)(-1.9), Y = (float)(-1.1) }; // Направление движения шарика при отбитии от второй и четвертой части платформы
        private LocalPoint _firstAndFifthPlatDirect = new LocalPoint(){ X=(float)(-2.2), Y=(float)(-0.8) }; // Направление движения шарика при отбитии от первой и пятой части платформы
        private int _invisibleBlock = 0;

        // Переменная для проверки отбился ли шарик от платформы, позволяя избежать многократного вычисления. 
        // Изменяется на false при отбитии шарика от блока или верхней стороны игрового поля
        private bool _ballOutOfPlatform = true; 


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
            _invisibleBlock = 0;
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
            _invisibleBlock += blockQuantity;
        }

        /// <summary>
        /// Проверка количества невидимых блоков
        /// </summary>
        private void CheckInvisibleBlocksNumber()
        {
            if (_gameState.InvisibleBlocksAtStart + _invisibleBlock == _gameState.Blocks.Length)
            {
                _gameState.WinGame = true;
            }
        }

        /// <summary>
        /// Проверка позиции шарика отностительно сторон игрового поля
        /// </summary>
        private void CheckBallOutOfGameWindow(Ball ball)
        {
            //Eсли шарик вылетел за нижний предел игрового окна - отнимается жизнь. 
            //Шарик и платформы возвращаются в стартовую позицию, задается первоначальное направление шарика
            //Eсли число жизней = 0 - игра завершается
            if (ball.Position.Y + _gameState.Ball.Diameter >= VirtualHeight)
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
                _ballOutOfPlatform = true;
            }

            //При отбитии от боковых сторон и верхней стороны игрового поля - меняется направление движения шарика
            if (ball.Position.X <= 0 || ball.Position.X + ball.Diameter >= VirtualWidth)
            {
                _ballVector.X *= -1;
            }
            if (ball.Position.Y <= 0)
            {
                _ballVector.Y *= -1;
                _ballOutOfPlatform = false;
            }
        }

        /// <summary>
        /// Изменить направление шарика при столкновении с блоком или блоками
        /// </summary>
        /// <param name="ball"></param>
        private void ChangeBallDirectionAtCollisionWithBlocks(Ball ball, List<Block> intersectedBlockList)
        {
            if (intersectedBlockList.Count == 1)
            {
                var rectSizeNumber = CheckBallCollisionWithSingleBlock(intersectedBlockList[0], ball);
                if(rectSizeNumber != 0)
                {
                    if (rectSizeNumber == 1 || rectSizeNumber == 3)
                    {
                        _ballVector.X *= -1;
                    }
                    else if (rectSizeNumber == 4 || rectSizeNumber == 2)
                    {
                        _ballVector.Y *= -1;
                    }
                    RemoveBlock(intersectedBlockList[0], intersectedBlockList.Count);
                    _ballOutOfPlatform = false;
                }
                
            }
            else if (intersectedBlockList.Count == 2)
            {
                ChangeBallDirectionAtCollisionWithTwoBlocks(intersectedBlockList);
                foreach (Block block in intersectedBlockList)
                {
                    RemoveBlock(block, intersectedBlockList.Count);
                }
                _ballOutOfPlatform = false;
            }
        }

        /// <summary>
        /// Изменение направления шарика при столкновении с двумя блоками
        /// </summary>
        /// <param name="intersectedBlockList"></param>
        /// <returns>true - позволяет удалить блоки с игрового поля</returns>
        private void ChangeBallDirectionAtCollisionWithTwoBlocks(List<Block> intersectedBlockList)
        {
            var block1 = intersectedBlockList[0];
            var block2 = intersectedBlockList[1];

            if (block1.Position.X == block2.Position.X)
            {
                _ballVector.X *= -1;
            }
            else if (block1.Position.Y == block2.Position.Y)
            {
                _ballVector.Y *= -1;
            }
            else
            {
                _ballVector.Y *= -1;
                _ballVector.X *= -1;
            }
        }

        /// <summary>
        /// Проверка состояния игры
        /// Изменение направления движения шарика
        /// Перемещение шарика
        /// </summary>
        private void CheckGameStatus()
        {
            CheckInvisibleBlocksNumber();
            CheckBallOutOfGameWindow(_gameState.Ball);
            CheckBallAndPlatformCollision(_gameState.Ball);
            var intersectedBlocks = CheckBallCollisionWithBlocks(_gameState.Ball);//проверка - сколько блоков зацепил шарик
            ChangeBallDirectionAtCollisionWithBlocks(_gameState.Ball, intersectedBlocks);
            MoveBall(_gameState.Ball);
        }

        /// <summary>
        /// Перемещение шарика на заданный шаг
        /// </summary>
        /// <param name="ball"></param>
        private void MoveBall(Ball ball)
        {
            if (_gameState.GameStarted && !_gameState.Pause)
            {
                ball.Position.X += _ballVector.X;
                ball.Position.Y += _ballVector.Y;
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
        private float CountBallAngleToPlatform(Ball ball, Platform platform)
        {
            float x1 = ball.Position.X + ball.Diameter / 2;
            float y1 = ball.Position.Y + ball.Diameter / 2;
            float x2 = platform.Position.X + (platform.Width / 2);
            float y2 = platform.Position.Y + (platform.Height / 2);

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
        /// Проверка столкновения шарика и платформы. 
        /// Если произошло столкновение - вычисление части платформы, куда попал шарик и отбитие шарика с заданным направлением
        /// </summary>
        /// <param name="ballRect"></param>
        private void CheckBallAndPlatformCollision(Ball ball)
        {
            var platform = _gameState.Platform;
            if (!_ballOutOfPlatform)
            {
                if (Intersects(ball,platform.Position.X,platform.Position.Y,platform.Width,platform.Height))
                {
                    _ballOutOfPlatform = true; //пересечение шарика и платформы
                    var platformPart = FindPlatformPartOnCollisionWithBall(ball, platform);
                    ChangeBallDirectionAtCollisionWithPlatform(platformPart);
                }
            }
        }

        /// <summary>
        /// Номер части платформы
        /// </summary>
        enum PlatformPart : int {First=1, Second, Third, Forth, Fifth, Left, Right};

        /// <summary>
        /// Расчет, в какую часть платформы прилетел шарик
        /// </summary>
        /// <param name="ball"></param>
        private int FindPlatformPartOnCollisionWithBall(Ball ball, Platform platform)
        {
            //Вычисление внутренних углов платформы образованные диагоналями
            var horizonAngle = CountRectangleAngle(platform.Height, platform.Width) * 2;
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
            float ballAngle = CountBallAngleToPlatform(ball, platform);
            var platformPartAngle = verticalAngle / 5;
            //Разделение платформы на 5 частей
            var platformAngle1 = topLeft + platformPartAngle;
            var platformAngle2 = platformAngle1 + platformPartAngle;
            var platformAngle3 = platformAngle2 + platformPartAngle;
            var platformAngle4 = platformAngle3 + platformPartAngle;

            var platformPartNumber = 0;
            //Проверка в какую часть платформы прилетел шарик
            
            if (ballAngle > topLeft && ballAngle < topright)
            {
                if (topLeft <= ballAngle && ballAngle <= platformAngle1)
                {
                    platformPartNumber = (int)PlatformPart.Fifth;
                }
                else if(platformAngle4 <= ballAngle && ballAngle <= topright)
                {
                    platformPartNumber = (int)PlatformPart.Fifth;
                }
                else if(platformAngle3 < ballAngle && ballAngle < platformAngle4)
                {
                    platformPartNumber = (int)PlatformPart.Forth;
                }
                else if (platformAngle1 < ballAngle && ballAngle < platformAngle2)
                {
                    
                    platformPartNumber = (int)PlatformPart.Second;
                }
                else if (platformAngle2 <= ballAngle && ballAngle <= platformAngle3)
                {
                    
                    platformPartNumber = (int)PlatformPart.Third;
                }
            }
            
            else if (_ballVector.X < 0)
            {
                if (0 <= ballAngle && ballAngle < topLeft || ballAngle <= 360 && botLeft <= ballAngle)
                {
                    platformPartNumber = (int)PlatformPart.Left;
                }
                else if (topright <= ballAngle && ballAngle <= botRight)
                {
                    platformPartNumber = (int)PlatformPart.Right;
                }
                
            }
            else if (_ballVector.X > 0)
            {
                if (0 <= ballAngle && ballAngle <= topLeft || ballAngle <= 360 && botLeft <= ballAngle)
                {
                    
                    platformPartNumber = (int)PlatformPart.Left;
                }
                else if (topright < ballAngle && ballAngle <= botRight)
                {
                    platformPartNumber = (int)PlatformPart.Right;
                }
            }
            return platformPartNumber;
        }

        /// <summary>
        /// Изменение направления шарика в зависимости от части платформы, куда он попал
        /// </summary>
        /// <param name="platformPart"></param>
        private void ChangeBallDirectionAtCollisionWithPlatform(int platformPart)
        {
            if(platformPart != 0)
            {
                //У каждой части платформы свой угол отбития шарика
                if (platformPart == 1 || platformPart == 5)
                {
                    if (_ballVector.X < 0)
                    {
                        _ballVector.X = _firstAndFifthPlatDirect.X;
                    }
                    else
                    {
                        _ballVector.X = _firstAndFifthPlatDirect.X * (-1);
                    }
                    _ballVector.Y = _firstAndFifthPlatDirect.Y;
                }
                else if (platformPart == 2 || platformPart == 4)
                {
                    if (_ballVector.X < 0)
                    {
                        _ballVector.X = _secondAndFourthPlatDirect.X;
                    }
                    else
                    {
                        _ballVector.X = _secondAndFourthPlatDirect.X * (-1);
                    }
                    _ballVector.Y = _secondAndFourthPlatDirect.Y;
                }
                else if (platformPart == 3)
                {
                    if (_ballVector.X < 0)
                    {
                        _ballVector.X = _startAndCentralPlatDirect.X;
                    }
                    else
                    {
                        _ballVector.X = _startAndCentralPlatDirect.X * (-1);
                    }
                    _ballVector.Y = _startAndCentralPlatDirect.Y;
                }
                //Если шарик прилетает в боковую сторону платформы
                //В зависимости от направления его движения:
                //Направление шарика меняется по Y или по X и Y
                else if (platformPart == 6)
                {
                    if (_ballVector.X > 0)
                    {
                        _ballVector.X *= -1;
                        _ballVector.Y *= -1;
                    }
                    else
                    {
                        _ballVector.Y *= -1;

                    }
                }
                else if (platformPart == 7)
                {
                    if (_ballVector.X > 0)
                    {
                        _ballVector.Y *= -1;

                    }
                    else
                    {
                        _ballVector.X *= -1;
                        _ballVector.Y *= -1;
                    }
                }
            }
        }

        /// <summary>
        /// Подсчет сколько блоков шарик задел при столкновении
        /// </summary>
        /// <param name="ball"></param>
        private List<Block> CheckBallCollisionWithBlocks(Ball ball)
        {
            List<Block> intersectedBlockList = new List<Block>();
            foreach (Block block in _gameState.Blocks)
            {
                if (block.Visible)
                {
                    if (Intersects(ball, block.Position.X,block.Position.Y, block.Width,block.Height))
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
        private float CountBallAngleToBlock(Ball ball, Block block)
        {
            float x1 = ball.Position.X + _gameState.Ball.Diameter / 2;
            float y1 = ball.Position.Y + _gameState.Ball.Diameter / 2;
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

        enum BlockPart : int
        {
            Left = 1, Top, Right,Bottom
        }
        
        /// <summary>
        /// Изменение направления движения шарика при столкновении с единичным блоком
        /// </summary>
        /// <param name="block"></param>
        /// <param name="ball"></param>
        /// <returns>true - позволяет удалить блок, с которым столкнулся шарик</returns>
        private int CheckBallCollisionWithSingleBlock(Block block, Ball ball)
        {
            var horizonAngle = CountRectangleAngle(block.Height,block.Width) * 2;
            var verticalAngle = 180 - horizonAngle;

            var topleft = horizonAngle / 2;
            var topright = topleft + verticalAngle;
            var botRight = topright + horizonAngle;
            var botLeft = botRight + verticalAngle;

            var ballAngle = CountBallAngleToBlock(ball, block); //Вычисление угла, под которым шарик прилетел в блок относительно центра блока

            var blockSide = 0;
            if (_ballVector.X < 0 && _ballVector.Y < 0)
            {
                if (botRight < ballAngle && ballAngle <= botLeft)
                {
                    blockSide = (int)BlockPart.Bottom;
                }
                else if (topright <= ballAngle && ballAngle < botRight)
                {
                    blockSide = (int)BlockPart.Right;
                }
            }
            else if (_ballVector.X > 0 && _ballVector.Y > 0)
            {
                if (0 <= ballAngle && ballAngle < topleft || botLeft <= ballAngle && ballAngle <= 360)
                {
                    blockSide = (int)BlockPart.Left;
                }
                else if (topleft < ballAngle && ballAngle <= topright)
                {
                    blockSide = (int)BlockPart.Top;
                }
            }
            else if (_ballVector.X < 0 && _ballVector.Y > 0)
            {
                if (topleft <= ballAngle && ballAngle < topright)
                {
                    blockSide = (int)BlockPart.Top;
                }
                else if (topright < ballAngle && ballAngle <= botRight)
                {
                    blockSide = (int)BlockPart.Right;
                }
            }
            else if (_ballVector.X > 0 && _ballVector.Y < 0)
            {
                if (botRight <= ballAngle && ballAngle < botLeft)
                {
                    blockSide = (int)BlockPart.Bottom;
                }
                else if (0 <= ballAngle && ballAngle <= topleft || botLeft < ballAngle && ballAngle <= 360)
                {
                    blockSide = (int)BlockPart.Left;
                }
            }
            return blockSide;
        }

        /// <summary>
        /// Первоначальное направление шарика
        /// </summary>
        public void SetStartDirection()
        {
            _ballVector.X = _startAndCentralPlatDirect.X;
            _ballVector.Y = _startAndCentralPlatDirect.Y;
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

        /// <summary>
        /// Проверка пересечения окружности (шарика) и прямоугольника (платформы или блока)
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private bool Intersects(Ball ball, float rectX, float rectY, int rectWidth, int rectHeight)
        {
            var circleX = ball.Position.X + ball.Diameter / 2;
            var circleY = ball.Position.Y + ball.Diameter / 2;
            var circleDistanceX = Math.Abs(circleX - (rectX+rectWidth/2));
            var circleDistanceY = Math.Abs(circleY - (rectY + rectHeight / 2));

            if (circleDistanceX > (rectWidth / 2 + ball.Diameter/2))
            {
                return false;
            }
            if (circleDistanceY > (rectHeight / 2 + ball.Diameter / 2))
            {
                return false;
            }

            if (circleDistanceX <= (rectWidth / 2))
            {
                return true;
            }
            if (circleDistanceY <= (rectHeight / 2))
            {
                return true;
            }

            var cornerDistance_sq = Math.Pow((circleDistanceX - rectWidth / 2), 2) + Math.Pow((circleDistanceY - rectHeight / 2), 2);
            return (cornerDistance_sq <= Math.Pow((ball.Diameter/2), 2));
        }

    }
}