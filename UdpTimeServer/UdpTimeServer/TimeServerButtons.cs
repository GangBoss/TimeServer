using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UdpTimeServer
{
    public class TimeServerButtons
    {
        private TimeServer server;
        public TimeServerButtons(int nextLine, Size currentSize)
        {
            serverLabel = new Label();
            stopServer = new Button();
            serverLabel.Location = new Point(0, nextLine);
            serverLabel.Size = currentSize;
            serverLabel.Text = "Server";
            stopServer.Location = new Point(serverLabel.Right, nextLine);
            stopServer.Size = currentSize;
            stopServer.Text = "Remove server";
            stopServer.Click += RemoveTimeServer;
        }

        public Label serverLabel { get; }
        public Button stopServer { get; }

        protected void RemoveTimeServer(object sender, EventArgs e)
        {
            server.Stop();
        }

        public void StartTimeServer(int port, int delay)
        {
            serverLabel.Text = $"Server working on {port} with delay {delay}";
            server = new TimeServer(port, delay);
            server.StartAsync();
        }

        public IEnumerable<Control> GetControls()
        {
            yield return serverLabel;
            yield return stopServer;
        }

        public int GetNextLineY()
        {
            return stopServer.Bottom;
        }
    }
}