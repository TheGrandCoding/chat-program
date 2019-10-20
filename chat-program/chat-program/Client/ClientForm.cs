using ChatProgram.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram.Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        { 
            InitializeComponent();
            Client = new ClientConnection(this, Disconnected);
            Logger.LogMsg("Client started");
        }

        public bool AllowInteract = true;

        public ClientConnection Client;

        Task Disconnected(Connection conn, Exception ex)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        Disconnected(conn, ex);
                    }));
                } catch { }
                return Task.CompletedTask;
            }

            this.txtMessage.Enabled = false;
            AllowInteract = false;
            this.Text += " | Connection Closed";
            MessageBox.Show((ex?.Message ?? "Client disconnected from server connection for an unknown reason"), "Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return Task.CompletedTask;
        }

        public bool Connect(IPAddress ip)
        {
            Logger.LogMsg($"Connecting {ip}");
            try
            {
                Client.Client.Connect(ip, Program.Port);
            }
            catch (Exception ex)
            {
                Logger.LogMsg(ex.ToString(), LogSeverity.Warning);
            }
            if(Client.Client.Connected)
            {
                Logger.LogMsg("Connected");
                Client.NewMessage += Client_NewMessage;
                Client.NewUser += Client_UserListChange;
                Client.IdentityKnown += Client_UserListChange;
                Client.UserUpdate += Client_UserListChange;
                Client.UserDisconnected += Client_UserDisconnected;
                Client.SetMonitorState += Client_SetMonitorState;
                Client.Send(Environment.UserName);
                Logger.LogMsg("Sent username, opened listener");
                Client.Listen();
                Common.Users[999] = new User() { Id = 999, UserName = "Server" };
                this.Activated += ClientForm_Activated;
                return true;
            } else
            {
                MessageBox.Show($"Failed to connect to IP");
                this.Close();
                return false;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        private int SC_MONITORPOWER = 0xF170;
        private uint WM_SYSCOMMAND = 0x0112;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        private void Client_SetMonitorState(object sender, bool e)
        {
            if(e)
            { // ?
            } else
            { // lock computer
                if(ChatProgram.Menu.Server == null)
                {
                    // only lock for non-local clients.
                    try
                    {
                        SendMessage(this.Handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)2);
                        LockWorkStation();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                } else
                {
                    Client_NewMessage(this, new Classes.Message()
                    {
                        Author = new User()
                        {
                            UserName = "(Client)",
                            Id = 0
                        },
                        Colour = Color.Red,
                        Content = $"Would lockdown, but local client",
                        Id = 0
                    });
                }
            }
        }

        Label createLabelFor(User u, ref int y)
        {
            int x = 5;
            var label = new Label();
            label.Tag = u;
            label.Text = $"#{u.Id} {u.UserName} {u.NickName}";
            label.Location = new Point(x, y);
            y += 30;
            return label;
        }
        private void Client_UserDisconnected(object sender, User e)
        {
            Common.Users.Remove(e.Id);
            Client_UserListChange(this, null);
        }

        public void Client_UserListChange(object sender, Classes.User e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    Client_UserListChange(sender, e);
                }));
                return;
            }
            if(e != null)
                Common.Users[e.Id] = e;
            gbUsers.Controls.Clear();
            var users = Common.Users.OrderBy(x => x.Key);
            int y = 15;
            foreach (var user in users)
            {
                var lbl = createLabelFor(user.Value, ref y);
                lbl.AutoSize = true;
                lbl.MaximumSize = new Size(gbUsers.Size.Width - 5, 15);
                lbl.TextAlign = ContentAlignment.TopCenter;
                gbUsers.Controls.Add(lbl);
                lbl.Click += user_click;
            }
        }

        private void user_click(object sender, EventArgs e)
        {
            if (sender is Label lbl && e is MouseEventArgs me)
            {
                if (me.Button == MouseButtons.Right)
                    lbl.BackColor = Color.LightBlue;
            }
        }

        Label getLabelFor(Classes.Message message, ref int y)
        {
            int y_offset = y - gbMessages.VerticalScroll.Value;
            var lbl = new FormatLabel(message, gbMessages);
            lbl.Text = $"{message.Author.DisplayName}: {message.Content}";
            lbl.Tag = message;
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(gbMessages.Size.Width - 15, 0);
            lbl.Location = new Point(5, y_offset);
            y += 5;
            return lbl;
        }
        public void ClientForm_Activated(object sender, EventArgs e)
        {
            uint latestMax = LAST_SEEN_MESSAGE;
            foreach(var control in gbMessages.Controls)
            {
                if(control is Label lbl)
                {
                    if(lbl.Tag is Classes.Message msg)
                    {
                        lbl.BackColor = Color.FromKnownColor(KnownColor.Control);
                        if(msg.Id > latestMax) // since no guarante of order
                            latestMax = msg.Id;
                    }
                }
            }
            if(LAST_SEEN_MESSAGE < latestMax) // hasnt changed in mean time
                LAST_SEEN_MESSAGE = latestMax;
        }
        int MESSAGE_Y = 5;
        public uint LAST_SEEN_MESSAGE = 0;
        private void Client_NewMessage(object sender, Classes.Message e)
        {
            var lbl = getLabelFor(e, ref MESSAGE_Y);
            int width = lbl.Height;
            lbl.ForeColor = e.Colour;
            lbl.Click += Lbl_Click;
            if(Form.ActiveForm == this)
            {
                LAST_SEEN_MESSAGE = e.Id;
            } else
            {
                lbl.BackColor = Color.LightCoral;
                if(e.Author.Id != Client.CurrentUser.Id)
                {
                    var notForm = new NotificationForm(this);
                    notForm.Show(e);
                }
            }
            this.gbMessages.Controls.Add(lbl);
            width = lbl.Height;
            MESSAGE_Y += width;
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            if(sender is Label lbl && lbl.Tag is Classes.Message msg)
            {
                MessageBox.Show(msg.Content, $"#{msg.Id} from {msg.Author.UserName} ({msg.Author.Id}) {msg.Author.NickName}");
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                if (string.IsNullOrWhiteSpace(txtMessage.Text))
                    return;
                var msg = new Classes.Message() { Author = Client.CurrentUser, Content = txtMessage.Text };
                txtMessage.Text = "";
                var pcket = new Packet(PacketId.SendMessage, msg.ToJson());
                Client.Send(pcket.ToString());
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var packet = new Packet(PacketId.Disconnect, new Newtonsoft.Json.Linq.JObject());
                Client.Send(packet.ToString());
            } catch { }
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Client.Client.Close();
            }
            catch { }
            ChatProgram.Menu.Client = null;
            ChatProgram.Menu.INSTANCE.Show();
            Client = null;
        }
    }
}
