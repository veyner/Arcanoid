using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arcanoid
{
    public partial class Form1 : Form
    {
        private Point rectanglePos = new Point(85, 295);
        private Point ellipseDirect = new Point(-10, -10);
        private Rectangle rectangle = new Rectangle(90, 295, 20, 5);
        private Rectangle ellipse = new Rectangle(97, 285, 5, 5);
        private Point _direct = Point.Empty;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = e.Graphics;
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 5);

            ellipse.X += ellipseDirect.X;
            ellipse.Y += ellipseDirect.Y;

            if (ellipse.X < 0 || ellipse.X > 200)
            {
                ellipseDirect.X *= -1;
            }
            if (ellipse.Y < 0 || ellipse.Y > 300)
            {
                ellipseDirect.Y *= -1;
            }
            graph.DrawEllipse(blackPen, ellipse);
            graph.DrawRectangle(blackPen, rectangle);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (rectangle.X > 0)
                {
                    rectangle.X -= 15;
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                if (rectangle.X < 180)
                {
                    rectangle.X += 15;
                }
            }
        }
    }
}