using System;
using System.Collections;
using System.Linq;
using Terminal.Gui;

namespace wttop.Widgets.Common
{
    /// <summary>
    /// Grid component that draws a table.
    /// In order to parametrize it (data + header + footer + col width), you need also to implement the 
    /// interface IGridDataSourceBuilder
    /// </summary>
    public class Grid : Component<IList>
    {
        IGridDataSourceBuilder dataSourceBuilder;

        public Grid(IGridDataSourceBuilder DataSourceBuilder)
        {
            dataSourceBuilder = DataSourceBuilder;

            DrawFrame(this.Bounds, 0, false);
        }

        public override void Redraw(Rect region)
        {
            Driver.SetAttribute(dataSourceBuilder.HeaderStyle);
            
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

        /// <summary>
        /// Write the content and fill in the rest of the columns with blanks
        /// </summary>
        /// <param name="rowIndex">Index of the row to write to</param>
        /// <param name="colIndex">Index of the column to write to</param>
        /// <param name="width">Width of the col, as set in the IGridDataSourceBuilder implementation</param>
        /// <param name="text">Text to write</param>
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
           
            var mainColorAttributes = Terminal.Gui.Attribute.Make(Color.White, Color.Black);
            Driver.SetAttribute(mainColorAttributes);
            
            int x = 0;
            for(int i = 0; i < dataSourceBuilder.DataSource.Count; i ++)
            {
                x = i + 2;
                int y = 1;
                var row = dataSourceBuilder.GetRowData(i);
                
                for(int j = 0; j < row.Length; j++)
                {
                    int width = (int)Math.Floor(this.Bounds.Width*dataSourceBuilder.GetColumnsWidth()[j]);
                    WriteText(x, y, width, row[j]);
                    y += width;
                };
            }

            // Draw footer
            Driver.SetAttribute(dataSourceBuilder.FooterStyle);
            WriteText(x + 2, 1, 50, dataSourceBuilder.GetFooter());
        }
    }

    /// <summary>
    /// Interface to impkement to feed the grid with actual data
    /// </summary>
    public interface IGridDataSourceBuilder
    {
        /// <summary>
        /// Set the data source to display
        /// </summary>
        IList DataSource {get; set;}

        /// <summary>
        /// An array of string header columns
        /// </summary>
        /// <returns></returns>
        string[] GetHeader();

        /// <summary>
        /// Style of the header (background and forgrounc colors)
        /// </summary>
        Terminal.Gui.Attribute HeaderStyle
        {
            get;
        }

        /// <summary>
        /// An array of decimals for Columns widths
        /// </summary>
        /// <returns></returns>
        decimal[] GetColumnsWidth();

        /// <summary>
        /// For a given index, send back the array of data
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string[] GetRowData(int index);

        /// <summary>
        /// Footer to display
        /// </summary>
        /// <returns></returns>
        string GetFooter();

        /// <summary>
        /// Style of the footer (background and forgrounc colors)
        /// </summary>
        Terminal.Gui.Attribute FooterStyle
        {
            get;
        }
    }
}