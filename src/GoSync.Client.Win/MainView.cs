using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoSync.Client.Win
{
    public class MainView : IDisposable
    {

        private readonly NotifyIcon _notifyIcon;

        protected NotifyIcon NotifyIcon {  get { return _notifyIcon; } }

        public MainView()
        {
            _notifyIcon = new NotifyIcon();
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
            NotifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            NotifyIcon.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("&Sync", (s, e) => OnSync()),
                new MenuItem("E&xit", (s, e) => OnExit())
            });

            NotifyIcon.Visible = true;
        }

        protected virtual void OnExit()
        {
            Application.Exit();
        }

        protected virtual void OnSync()
        {
            var events = SyncProvider.Sync();

            if(events.Count() > 0)
            {
                var builder = new StringBuilder();

                foreach (var calendarEvent in events)
                {
                    builder.AppendLine(calendarEvent.Title);
                }

                NotifyIcon.ShowBalloonTip(5000, "New Events", builder.ToString(), ToolTipIcon.Info);
            }
            else
            {
                NotifyIcon.ShowBalloonTip(5000, "No New Events", "Calendar is up to date.", ToolTipIcon.Info);
            }
        }

        public void Dispose()
        {
            NotifyIcon.Visible = false;
            NotifyIcon.Dispose();
        }
    }
}
