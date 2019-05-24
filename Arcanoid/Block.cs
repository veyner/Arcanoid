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
        public Point Position { get; set; }
        public bool Visible { get; set; }

        public Block()
        {
            Visible = true;
        }
    }
}