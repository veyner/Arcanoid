using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Arcanoid
{
    public class Block
    {
        //public Point Position;
        public bool Visible { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int TextureNumber { get; set; }
        public RectangleF BlockRectangle { get; set; }

        public Block()
        {
            Visible = true;
        }
    }
}