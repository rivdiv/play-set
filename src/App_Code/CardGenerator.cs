using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GenerateCards
/// </summary>
public class GenerateCards
{
    public void GenerateCardImages()
    {
        int s = 10;
        int h = 100;
        int w = 300;
        foreach (var card in Card.GetCards())
        {
            var img = new Bitmap(w, w);
            var graphics = Graphics.FromImage(img);
            graphics.Clear(Color.White);

            Brush brush;
            if (card.Fill == Card.Fills.Solid.ToString())
            {
                brush = new SolidBrush(card.Color);
            }
            else if (card.Fill == Card.Fills.Striped.ToString())
            {
                brush = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.White, card.Color);
            }
            else
            {
                brush = new SolidBrush(Color.White);
            }

            // shape
            Rectangle[] rectangles = new Rectangle[card.Number];
            for (int i = 0; i < card.Number; i++)
            {
                int x = s;
                int width = w - (s * 2);
                int y = (i * h) + s;
                int height = h - (s * 2);
                var rectangle = new Rectangle(x, y, width, height);
                if (card.Shape == Card.Shapes.Rectangle.ToString())
                {
                    rectangles[i] = rectangle;

                    if (card.Fill == Card.Fills.Empty.ToString())
                    {
                        if (rectangles.Count() > 0)
                        {
                            graphics.DrawRectangle(new Pen(card.Color), rectangle);
                        }
                    }
                    else
                    {
                        graphics.FillRectangle(brush, rectangle);
                    }
                }
                else if (card.Shape == Card.Shapes.Oval.ToString())
                {
                    if (card.Fill == Card.Fills.Empty.ToString())
                    {
                        graphics.DrawEllipse(new Pen(card.Color), rectangle);
                    }
                    else
                    {
                        graphics.FillEllipse(brush, rectangle);
                    }
                }
                else if (card.Shape == Card.Shapes.Diamond.ToString())
                {
                    var sp = i == 0 ? s : s * 2;
                    var x1 = width;
                    var y1 = (height / 2) + (h * i);
                    var x2 = (width / 2);
                    var y2 = i * height;
                    var x3 = s / 2;
                    var y3 = (height / 2) + (h * i);
                    var x4 = width / 2;
                    // var y4 =(height * (i+1));
                    var y4 = y1 + (height / 2);
                    var points = new Point[]{
                           new Point(x1 + s, y1  - (s*i)+ sp - s),
                           new Point(x2 + s, y2+ (s* (i+1))+ sp-s),
                           new Point(x3, y3  - (s*i)+ sp-s ),
                           new Point(x4 + s, y4 - (s*i)+ sp-s)
                        };


                    if (card.Fill == Card.Fills.Empty.ToString())
                    {
                        graphics.DrawPolygon(new Pen(card.Color), points);
                    }
                    else
                    {
                        graphics.FillPolygon(brush, points);
                    }
                }
            }

            var path = string.Format("~/Images/Cards/SetImage-{0}.png", card.CardID);
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            img.Save(HttpContext.Current.Server.MapPath(path), ImageFormat.Png);
        }

    }
}