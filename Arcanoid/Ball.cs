using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Arcanoid
{
    public class Ball
    {
        //public PointF Position { get; set; }
        public int Diameter { get; set; }
        public int Texture { get; set; }

        public RectangleF BallRectangle { get; set; }
        
    }
}