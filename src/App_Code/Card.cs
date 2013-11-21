using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Card
/// </summary>

[Serializable]
public   class Card
{
    public Color Color { get; set; }
    public string Fill { get; set; }
    public int Number { get; set; }
    public string Shape { get; set; }
    public int CardID { get; set; }

    public static  List<Color> Colors = new List<Color>() { Color.Red, Color.Green, Color.Purple };
    public   enum Fills { Solid, Striped, Empty };
    public   enum Shapes { Oval, Diamond, Rectangle };
    public  static int[] Numbers = new int[] { 1, 2, 3 };

    public static List<Card> GetCards()
    {
        var cards = new List<Card>();
        int i = 1;
        foreach (var number in Numbers)
        {
            foreach (var color in Colors)
            {
                foreach (var fill in Enum.GetValues(typeof(Fills)))
                {
                    foreach (var shape in Enum.GetValues(typeof(Shapes)))
                    {
                        cards.Add(new Card
                        {
                            Color = color,
                            Shape = shape.ToString(),
                            Fill = fill.ToString(),
                            Number = number,
                            CardID = i
                        });
                        i += 1;
                    }
                }
            }
        }

        return cards;
    }

}