using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace CSGitCack
{
    public class Conversions
    {
        public static Image convertByteToImage(Object pictureByte)
        {
            byte[] imageByte = new byte[0];
            imageByte = (byte[]) pictureByte;
            MemoryStream memStream = new MemoryStream(imageByte);
            Image returnImage = Image.FromStream(memStream);

            return returnImage;
        }

        public static List<string> RecombineQuotedStrings(string[] tokens)
        {
            var ret = new List<string>();
            char quoteChar = '"';
            int state = 0;
            string newtok = "";
            foreach (var t in tokens)
            {
                switch (state)
                {
                    case 0: // looking for first to combine, e.g. _"task_ or _'task_
                        if (t[0] == '"' || t[0] == '\'')
                        {
                            quoteChar = t[0];
                        }

                        if (t[0] == quoteChar && t[t.Length - 1] != quoteChar)
                        {
                            state = 1;
                            newtok = t;
                        }
                        else
                        {
                            ret.Add(t);
                        }

                        break;

                    case 1: // recombining; looking for last to combine, e.g. _task"_ or _task'_
                        newtok += "," + t;
                        if (t[t.Length - 1] == quoteChar)
                        {
                            state = 0;
                            ret.Add(newtok);
                            newtok = "";
                        }

                        break;
                }
            }

            if (state == 1) // then we've got to the end of the list without a closing quote, e.g. [task1,\"task2a,b,c]
            {
                // It's anyone's guess what the user actually meant here, so let's just bung a quote at the end and leave them to figure out what's wrong
                newtok += quoteChar;
                ret.Add(newtok);
            }

            return ret;
        }
        // test25: What does GetTextGeometryAndFormatting do if the text is empty?
        // Ans: Bounds.Width=-Inf.  Bounds.Width < 20 returns TRUE.
        //public static void GetTextGeometryAndFormatting(string Text, string Font, double FontSize, bool Italics, bool Bold,
        //    System.Windows.Point Location, System.Windows.Media.Color Colour, out Geometry geom, out FormattedText ft,
        //    out System.Windows.Point whitespace)
        //{
        //    var tf = new Typeface(new System.Windows.Media.FontFamily(Font),
        //        Italics ? FontStyles.Italic : FontStyles.Normal,
        //        Bold ? FontWeights.Bold : FontWeights.Normal,
        //        FontStretches.Normal);
        //    var br1 = new SolidColorBrush();
        //    try
        //    {
        //        br1.Color = Colour;
        //    }
        //    catch
        //    {
        //        br1.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);
        //    }
        //
        //    string measureString = Text;
        //
        //    // Next line throws:
        //    // 1>C:\_Dave\TBS\Dev\CSGitCack\Conversions.cs(94,18,95,77): warning CS0618: 'FormattedText.FormattedText(string, CultureInfo, FlowDirection, Typeface, double, Brush)' is /obsolete: /'Use the PixelsPerDip override'
        //    ft = new FormattedText(measureString, CultureInfo.InvariantCulture,
        //        System.Windows.FlowDirection.LeftToRight, tf, FontSize, br1);
        //
        //    // Problem is: we don't know what the space dimensions are.  So if we draw it at 0,0, then the bounding rectangle will start at x,y which indicates the space dimension.
        //    var geom1 = ft.BuildGeometry(new System.Windows.Point(0, 0));
        //    // geom1.Bounds.TopLeft is now the width and height of the whitespace
        //
        //    // Rebuild the geometry to discard the whitespace and offset by this.(x,y), because for general use elsewhere we want geom.Bounds to indicate the drawn text area
        //    whitespace = geom1.Bounds.TopLeft; // draw however still needs to be able to take this into account
        //    System.Windows.Point offset =
        //        new System.Windows.Point(whitespace.X * -1 + Location.X, whitespace.Y * -1 + Location.Y);
        //    geom = ft.BuildGeometry(offset);
        //}
    }
}
