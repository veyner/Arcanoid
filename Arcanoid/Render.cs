using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Arcanoid
{
    public class Render
    {
        private GameState _gameState;
        public List<Texture> TextureList = new List<Texture>();
        private PointF scale = new PointF(1, 1);
        public Bitmap BackBuffer;
        private MainForm _form;
        private string textureDir = "Textures";
        private string textureImage = "TexturePack.png";

        public Render(MainForm form, GameState gameState)
        {
            _form = form;
            _gameState = gameState;
        }

        /// <summary>
        /// Отрисовка блоков
        /// </summary>
        /// <param name="graph"></param>
        private void DrawBlockAsRectangles(Graphics graph)
        {
            foreach (Block block in _gameState.Blocks)
            {
                if (block.Visible)
                {
                    var rect = CreateBlockRectangle(block);
                    var textureBrush = TextureList[block.TextureNumber - 1].TextureBrush;
                    graph.FillRectangle(textureBrush, rect);
                }
            }
        }
        /// <summary>
        /// Создание блока для отрисовки по вычисленным позициям
        /// </summary>
        private Rectangle CreateBlockRectangle(Block block)
        {
            var x = Convert.ToInt32(block.BlockRectangle.X * scale.X);
            var y = Convert.ToInt32(block.BlockRectangle.Y * scale.Y);
            var height = Convert.ToInt32(block.Height * scale.Y);
            var width = Convert.ToInt32(block.Width * scale.X);
            var rect = new Rectangle(x, y, width, height);
            return rect;
        }

        /// <summary>
        /// Отрисовка шарика
        /// </summary>
        /// <param name="graph"></param>
        private void DrawBallAsRectangle(Graphics graph)
        {
            Pen blackPen1 = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            var ball = new Rectangle
            {
                X = Convert.ToInt32(_gameState.Ball.BallRectangle.X * scale.X),
                Y = Convert.ToInt32(_gameState.Ball.BallRectangle.Y * scale.Y),
                Height = Convert.ToInt32(_gameState.Ball.BallRectangle.Height * scale.Y),
                Width = Convert.ToInt32(_gameState.Ball.BallRectangle.Width * scale.X),
            };
            graph.DrawEllipse(blackPen1, ball);
            graph.FillEllipse(Brushes.Orange, ball);
        }

        /// <summary>
        /// Отрисовка платформы
        /// </summary>
        /// <param name="graph"></param>
        private void DrawPlatformAsRectangle(Graphics graph)
        {
            Pen blackPen2 = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
            var platform = new Rectangle()
            {
                X = Convert.ToInt32(_gameState.Platform.PlatformRectangle.X * scale.X),
                Y = Convert.ToInt32(_gameState.Platform.PlatformRectangle.Y * scale.Y),
                Height = Convert.ToInt32(_gameState.Platform.Height * scale.Y),
                Width = Convert.ToInt32(_gameState.Platform.Width * scale.X)
            };
            graph.DrawRectangle(blackPen2, platform);
            graph.FillRectangle(Brushes.Red, platform);
        }

        /// <summary>
        /// Отрисовка картинки буфера
        /// </summary>
        public void Draw()
        {
            if (BackBuffer != null)
            {
                using (var graph = Graphics.FromImage(BackBuffer))
                {
                    graph.Clear(Color.SkyBlue);
                    DrawBallAsRectangle(graph);
                    DrawPlatformAsRectangle(graph);
                    DrawBlockAsRectangles(graph);
                }
            }
        }

        /// <summary>
        /// Загрузка текстур для блоков из файла "TexturePack.png"
        /// </summary>
        public void AddBlockTextureList()
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
            TextureList.Add(texture1);
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
            TextureList.Add(texture2);
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
            TextureList.Add(texture3);
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
            TextureList.Add(texture4);
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
            TextureList.Add(texture5);
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
            TextureList.Add(texture6);
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
            TextureList.Add(texture7);
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
            TextureList.Add(texture8);
        }

        /// <summary>
        /// Обрезка текстуры блока
        /// </summary>
        /// <param name="src">файл с текстурами</param>
        /// <param name="textureRectangle">прямоугольник текстуры</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Загрузка каждой текстуры из файла
        /// </summary>
        /// <param name="textureRectangle">прямоугольник текстуры</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="textureImage">рисунок с текстурами</param>
        /// <returns></returns>
        private TextureBrush GetTextureBrush(Rectangle textureRectangle, int width, int height, string textureImage)
        {
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, textureDir, textureImage);
            Bitmap textures = (Bitmap)Image.FromFile(path);
            var cropBitmap = CropBitmap(textures, textureRectangle, width, height);
            TextureBrush textureBrush = new TextureBrush(cropBitmap);
            return textureBrush;
        }

        /// <summary>
        /// Скалирование виртуальный размеров шарика блоков и платформы к размерам для отрисовки в главное окно
        /// </summary>
        public void ScaleSize(int mainWindowHeight, int mainWindowWidth, int virtualHeight, int virtualWidth)
        {
            float height = mainWindowHeight;
            float width = mainWindowWidth;
            scale.Y = height / virtualHeight;
            scale.X = width / virtualWidth;
        }
    }
}
