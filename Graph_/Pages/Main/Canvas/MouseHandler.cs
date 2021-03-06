﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph_
{
    public partial class WFCanvas
    {
        Point prev = new Point(0, 0);
        bool mouseMoved=false;

        private void onFieldMouseMove(object sender, MouseEventArgs e)
        {
            if (!scalable) return;
            mouseMoved = true;
            if (isMousePressed && !childControleMouse)
            {
                Point cure = e.Location;
                if (!Object.Equals(prev, cure))
                {
                    Point delta = new Point(prev.X - cure.X, prev.Y - cure.Y);
                    center = new PointF(center.X - delta.X, center.Y - delta.Y);
                    prev = cure;
                }

            }
            else
            {
                prev = e.Location;
            }

            if (isMousePressed)
                render();
            onMouseMove?.Invoke(sender, e);
        }

        private void onFieldMouseUp(object sender, MouseEventArgs e)
        {
            if (!scalable) return;
            isMousePressed = false;
            restrintChildControleMouse = false;
            if (!mouseMoved)
            {
                onClick?.Invoke(sender, e);
            }
        }

        private void onFieldMouseDown(object sender, MouseEventArgs e)
        {
            if (!scalable) return;
            isMousePressed = true;
            if (!childControleMouse)
                restrintChildControleMouse = true;
            mouseMoved = false;
            //prev = new Point(0, 0);
        }

        private void onFieldWheel(object sender, MouseEventArgs e)
        {
            if (!scalable) return;
            //Console.WriteLine($"center: {center}");
            //Console.WriteLine($"location: {e.Location}");

            //
            //center = new PointF(baseCenter.X+baseCenter.X-e.X, baseCenter.Y+baseCenter.Y-e.Y);
            //center = e.Location;
            //center = new PointF(e.X/scale, e.Y/scale);
            if (e.Delta > 0)
            {
                PointF delta = new PointF((center.X - e.X) / scale, (center.Y - e.Y) / scale);
                center = new PointF(center.X + delta.X, center.Y + delta.Y);
                setScale(scale + 1);

            }
            else
            {
                PointF delta = new PointF((center.X - e.X) / scale, (center.Y - e.Y) / scale);
                center = new PointF(center.X - delta.X, center.Y - delta.Y);
                setScale(scale - 1);
            }  

        }
        private void onFieldHover(object sender, EventArgs e)
        {
            if (!scalable) return;
            field.Cursor = Cursors.Hand;
        }
    }
}
