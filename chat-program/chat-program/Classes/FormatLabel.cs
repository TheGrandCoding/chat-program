using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram.Classes
{
    public class FormatLabel : Label
    {
        public Classes.Message Message { get; }
        public string NoFormatingText { get; private set; }
        public Panel ParentPanel { get; private set; }
        public FormatLabel(Classes.Message message, Panel container) : base()
        {
            ParentPanel = container;
            Message = message;
        }

        Font getFont(Font normal, int bold, int italic)
        {
            var style = normal.Style;
            if (bold > 0)
                style = style | FontStyle.Bold;
            if (italic > 0)
                style = style | FontStyle.Italic;
            return new Font(normal, style);
        }

        string getNextFormatting(int index)
        {
            if (Text[index] != '<')
                return null;
            string remainder = Text.Substring(index);
            int first = remainder.IndexOf("<");
            int last = remainder.IndexOf(">");
            if(first >= 0 && last >= 0)
            {
                return remainder.Substring(first, (last - first) + 1);
            }
            return null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Point drawPoint = new Point(0,0);
            Graphics g = e.Graphics;

            int numBolds = 0;
            int numItalics = 0;
            Color curColor = Message.Colour;

            Font normalFont = this.Font;
            Brush brush = new SolidBrush(curColor);
            SizeF size = g.MeasureString("A", normalFont);
            float lineSpacing = size.Height;

            NoFormatingText = "";
            float posX = 0.0f;
            float posY = 0.0f;
            for(int i = 0; i < Text.Length; i++)
            {
                var nextFormat = getNextFormatting(i);
                if(nextFormat != null)
                {
                    switch (nextFormat)
                    {
                        case "<b>":
                            numBolds++;
                            break;
                        case "</b>":
                            numBolds--;
                            break;
                        case "<i>":
                            numItalics++;
                            break;
                        case "</i>":
                            numItalics--;
                            posX += 0.5f; // since it is slanted a wee bit, give some space
                            break;
                        case "</r>":
                            numBolds = 0;
                            if (numItalics > 0)
                                posX += 0.75f; // as above
                            numItalics = 0;
                            break;
                    }
                    i += nextFormat.Length - 1; // we dont print special formatting options
                    // length -1, since we i++ in the for loop anyway
                    continue;
                }
                var font = getFont(normalFont, numBolds, numItalics);
                string charToDraw = new string(Text[i], 1);
                g.DrawString(charToDraw, font, brush, posX, posY);
                SizeF sizeChar = g.MeasureString(charToDraw, font);
                posX += sizeChar.Width * 0.6f;
                NoFormatingText += charToDraw;
                if(posX >= (this.MaximumSize.Width - 1))
                {
                    posX = 0;
                    posY += lineSpacing;
                }
            }
            this.AutoSize = false;
            Bounds = new Rectangle(Location, new Size(ParentPanel.Width - 5, (int)posY + 13));
            //this.Size = new Size(Container.Width - 10, (int)posY + 13);
            //base.OnPaint(e);
        }
    }
}
