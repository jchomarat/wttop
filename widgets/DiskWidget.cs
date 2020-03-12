using System;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Widgets.Common;

namespace wttop.Widgets
{

    /// <summary>
    /// Widget that will display the disk graph
    /// </summary>
    public class DiskWidget : WidgetFrame
    {
        private int _cycle = 0;
        private Label _write;
        private Label _read;
        private long _valueWrite = 0;
        private long _valueRead = 0;
        private View _disksView;
        private Label[] _disksName = null;
        private Bar[] _disksUsage = null;

        /// <summary>
        /// Main constructor dfor the Disk widget
        /// </summary>
        /// <param name="serviceProvider">A service provider</param>
        public DiskWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            this.Title = settings.DiskWidgetTitle;

            // Disk activity
            Label titleWrite = new Label("Write (kB/sec): ")
            {
                X = 1,
                Y = 1
            };

            Add(titleWrite);

            _write = new Label(string.Empty)
            {
                X = Pos.Right(titleWrite),
                Y = 1
            };

            _write.TextColor = Terminal.Gui.Attribute.Make(settings.DiskWidgetWriteTextColor, settings.MainBackgroundColor);

            Add(_write);

            Label titleRead = new Label("Read (kB/sec) : ")
            {
                X = 1,
                Y = Pos.Bottom(titleWrite)
            };

            Add(titleRead);

            _read = new Label(string.Empty)
            {
                X = Pos.Right(_write),
                Y = Pos.Bottom(_write)
            };

            _read.TextColor = Terminal.Gui.Attribute.Make(settings.DiskWidgetReadTextColor, settings.MainBackgroundColor);

            Add(_read);

            // Disk usage
            var diskCount = systemInfo.DiskCount;
            _disksView = new View()
            {
                X = 1,
                Y = 4,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            Add(_disksView);

            _disksName = new Label[diskCount];
            _disksUsage = new Bar[diskCount];
            int offset = 1;

            for (var i = 0; i < diskCount; i++)
            {
                _disksUsage[i] =
                    new Bar(settings.DiskWidgetUsageBarColor, settings.MainBackgroundColor)
                    {
                        X = 1,
                        Y = offset,
                        Width = Dim.Percent(40),
                        Height = Dim.Sized(1)
                    };

                _disksName[i] = new Label(string.Empty)
                {
                    X = Pos.Right(_disksUsage[i]),
                    Y = offset
                };

                offset++;
            }

            _disksView.Add(_disksName);
            _disksView.Add(_disksUsage);
        }

        protected override async Task Update(MainLoop MainLoop)
        {
            var disks = await systemInfo.GetDiskActivity();
            if (_valueWrite == 0)
            {
                // First round, don't do anything, we need a first value for calculation
                _valueWrite = disks.TotalBytesWritten;
                _valueRead = disks.TotalBytesRead;
            }
            else
            {
                var i = ((disks.TotalBytesWritten - _valueWrite) / 100);
                _write.Text = $"{i}              ";
                _valueWrite = disks.TotalBytesWritten;

                var j = ((disks.TotalBytesRead - _valueRead) / 100);
                _read.Text = $"{j}               ";
                _valueRead = disks.TotalBytesRead;
            }

            // Disks usage refresh does not need to be refreshed at the same pace, use cycle
            if (_cycle == 0 || _cycle == 59)
            {
                // First iteration, or one minute
                var storageInfo = await systemInfo.GetDiskStorageInfo();
                for (var i = 0; i < _disksUsage.Length; i++)
                {
                    _disksName[i].Text = string.Format("used on '{0} ({1})'", storageInfo.ElementAt(i).VolumeCaption, storageInfo.ElementAt(i).VolumeName);
                    _disksUsage[i].Update(MainLoop, storageInfo.ElementAt(i).PercentageUsed);
                };

                _cycle = _cycle == 59 ? 0 : _cycle++;
            }
        }
    }
}