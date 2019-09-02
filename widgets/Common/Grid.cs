using System;
using System.Collections;
using System.Linq;
using Terminal.Gui;
using NStack;

namespace wttop.Widgets.Common
{
    public class Grid : Component<IList>
    {
        GridDataSourceBuilder dataSourceBuilder;

        int y = 1;

        public Grid(GridDataSourceBuilder DataSourceBuilder)
        {
            dataSourceBuilder = DataSourceBuilder;

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
            for(int i = 0; i < dataSourceBuilder.GetHeader().Length; i++)
            {
                int width = (int)Math.Floor(this.Bounds.Width*dataSourceBuilder.GetColumnsWidth()[i]);
                WriteText(x, y, width, dataSourceBuilder.GetHeader()[i]);
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

        protected override void UpdateAction(IList newListItems)
        {
            dataSourceBuilder.DataSource = newListItems;

            // Ensure color schema is correct
            for(int i = 0; i < dataSourceBuilder.DataSource.Count; i ++)
            {
                var row = dataSourceBuilder.GetRowData(i);
                int x = i + 2;
                int y = 1;
                
                for(int j = 0; j < row.Length; j++)
                {
                    int width = (int)Math.Floor(this.Bounds.Width*dataSourceBuilder.GetColumnsWidth()[j]);
                    WriteText(x, y, width, row[j]);
                    y += width;
                };
            }
        }
    }

    public interface GridDataSourceBuilder
    {
        IList DataSource {get; set;}

        string[] GetHeader();

        decimal[] GetColumnsWidth();

        string[] GetRowData(int index);
    }
}