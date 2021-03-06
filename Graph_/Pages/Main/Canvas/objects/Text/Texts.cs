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
    public class Texts
    {
        WFCanvas.WFCanvasContext context;
        private List<Tuple<Color, Tuple<string, float, float>>> texts;
        List<Text> properTexts;

        public Texts(WFCanvas.WFCanvasContext context)
        {
            this.context = context;
            this.texts = new List<Tuple<Color, Tuple<string, float, float>>>();
            properTexts = new List<Text>();
        }

        public int addText(string text, float x = 0, float y = 0, Color? _color=null)
        {
            Color color = _color ?? Color.Black;
            texts.Add(new Tuple<Color, Tuple<string, float, float>>(color, 
                      new Tuple<string, float, float>(text, x, y)));

            return texts.Count() - 1;
        }

        public int addText(Text text)
        {
            properTexts.Add(text);

            return properTexts.Count() - 1;
        }

        public void draw()
        {
            PointF center = context.center;
            int scale = context.scale;
            foreach (var textProps in texts)
            {
                int fontSize = 1 * scale;

                Color color = textProps.Item1;
                var _text = textProps.Item2;

                string text = _text.Item1;
                float x = _text.Item2 * scale + center.X;
                float y = context.center.Y - _text.Item3 * scale;

                Rectangle textArea = new Rectangle((int)x - fontSize * text.Length / 2,
                                                   (int)y - fontSize / 2, fontSize * text.Length, fontSize);

                StringFormat alignCenter = new StringFormat();
                alignCenter.Alignment = StringAlignment.Center;
                alignCenter.LineAlignment = StringAlignment.Center;

                Font font = new Font("arial", fontSize <= 0 ? 0.000001f : fontSize, GraphicsUnit.Pixel);

                context.graph.DrawString(text, font, new SolidBrush(color), textArea, alignCenter);

                //context.graph.DrawRectangle(new Pen(Color.Red), textArea); //DEBUG
            }

            foreach (Text text in properTexts)
            {
                RectangleF textArea = text.getAsTranslatedRectangle(scale, center);
                Font font = text.getCalcedFont(context.scale);
                //Font font = text.Font;
                Color color = text.Color;
                String _text = text.String;

                StringFormat alignCenter = new StringFormat();
                alignCenter.Alignment = StringAlignment.Center;
                alignCenter.LineAlignment = StringAlignment.Center;

                context.graph.DrawString(_text, font, new SolidBrush(color), textArea, alignCenter);

                //context.graph.DrawRectangle(new Pen(Color.Red), textArea); //DEBUG
            }
        }

        public void clear()
        {
            texts.Clear();
            properTexts.Clear();
        }
    }
}
