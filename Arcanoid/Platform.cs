using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Arcanoid
{
    public class Platform
    {
        public Point Position;
        public int Height { get; set; }
        public int Width { get; set; }
        public int Texture { get; set; }
    }
}