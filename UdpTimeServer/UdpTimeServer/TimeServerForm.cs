using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UdpTimeServer
{
    public partial class TimeServerForm : Form
    {
        private readonly Size currentSize;
        private readonly TextBox delayInput = new TextBox();
        private readonly HashSet<int> lockedPorts = new HashSet<int>();
        private int NextLineY;
        private readonly TextBox portInput = new TextBox();

        public TimeServerForm()
        {
            currentSize = new Size(ClientSize.Width, 30);
            var portLabel = new Label();
            portLabel.Location = new Point(0, 0);
            portLabel.Size = currentSize;
            portLabel.Text = "Enter listening port";

            portInput.Location = new Point(portLabel.Right, 0);
            portInput.Size = currentSize;

            var delayLabel = new Label();
            delayLabel.Location = new Point(0, portLabel.Bottom);
            delayLabel.Size = currentSize;
            delayLabel.Text = "Enter time delay";

            delayInput.Location = new Point(portLabel.Right, portLabel.Bottom);
            delayInput.Size = currentSize;

            var createServer = new Button();
            createServer.Location = new Point(portLabel.Right, delayInput.Bottom);
            createServer.Size = currentSize;
            createServer.Text = "Start server";
            createServer.Click += AddTimeServer;

            NextLineY = createServer.Bottom;
            Controls.Add(portLabel);
            Controls.Add(portInput);
            Controls.Add(delayInput);
            Controls.Add(delayLabel);
            Controls.Add(createServer);


            InitializeComponent();
        }

        private async void AddTimeServer(object sender, EventArgs args)
        {
            if (int.TryParse(portInput.Text, out var port))
            {
                if (!lockedPorts.Contains(port))
                {
                    if (int.TryParse(delayInput.Text, out var delay))
                    {
                        lockedPorts.Add(port);
                        var server = new TimeServerButtons(NextLineY, currentSize);
                        NextLineY = server.GetNextLineY();
                        Controls.AddRange(server.GetControls().ToArray());
                        server.StartTimeServer(port, delay);
                        server.stopServer.Click += (s, a) =>
                        {
                            Controls.Remove(server.serverLabel);
                            Controls.Remove(server.stopServer);
                            lockedPorts.Remove(port);
                        };
                    }
                }
            }
        }

      
    }
}