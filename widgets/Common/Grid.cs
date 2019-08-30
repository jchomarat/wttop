using System;
using System.Collections;
using System.Linq;
using Terminal.Gui;
using NStack;

namespace wttop.Widgets.Common
{
    public class Grid : Component<GridDataSource>
    {
        string[] header;

        decimal[] columnsRatio;

        int y = 1;

        public Grid(string[] Header, decimal[] ColumnsRatio)
        {
            columnsRatio = ColumnsRatio;
            header = Header;
            DrawFrame(this.Bounds, 0, false);

            var mainColorScheme = new ColorScheme();
            var mainColorAttributes = Terminal.Gui.Attribute.Make(Color.Black, Color.BrightGreen);
            mainColorScheme.Focus = mainColorAttributes;
            mainColorScheme.HotFocus = mainColorAttributes;
            mainColorScheme.HotNormal = mainColorAttributes;
            mainColorScheme.Normal = mainColorAttributes;

            this.ColorScheme = mainColorScheme;   
        }

        public override void Redraw(Rect region)
        {
            //Generate header
            int y = 1;
            int x = 1;
            for(int i = 0; i < header.Length; i++)
            {
                int width = (int)Math.Floor(this.Bounds.Width*columnsRatio[i]);
                WriteText(x, y, width, header[i]);
                y += width;                
            };
        }

        void WriteText(int rowIndex, int colIndex, int width, string text)
        {
            int offset = 0;
            text.ToCharArray().ToList().ForEach(c => {
                this.AddRune(colIndex + offset, rowIndex, new Rune(c));
                offset ++;
            });

            // Complete the 'width' with 'space'
            if (offset < width) {
                for(int i = offset; i < width; i++)
                {
                    this.AddRune(colIndex + i, rowIndex, new Rune(' '));
                }
            }
        }

        protected override void UpdateAction(GridDataSource newValue)
        {
            // Ensure color schema is correct
            for(int i = 0; i < newValue.DataSource.Count; i ++)
            {
                var row = newValue.GetRowFromObject(i);
                int x = i + 2;
                int y = 1;
                
                for(int j = 0; j < row.Length; j++)
                {
                    int width = (int)Math.Floor(this.Bounds.Width*columnsRatio[j]);
                    WriteText(x, y, width, row[j]);
                    y += width;
                };
            }
        }
    }

    public interface GridDataSource
    {
        IList DataSource {get; set;}

        string[] GetRowFromObject(int index);
    }
}