﻿using ChatProgram.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        public Dictionary<uint, Label> MessageLabels = new Dictionary<uint, Label>();

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
                Client.MessageDeleted += Client_MessageDeleted;
                Client.NewImageUploaded += Client_NewImageUploaded;
                Client.UploadStatus += Client_UploadStatus;
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

        private void Client_UploadStatus(object sender, UploadStatusEvent e)
        {
            pbProgressBar.Maximum = e.Maximum;
            pbProgressBar.Value = e.Current;
            double percentage = (e.Current / e.Maximum) * 100;
            pbProgressBar.Tag = $"Uploading image; {e.Current}/{e.Maximum} {Math.Round(percentage, 0)}%";
        }

        PictureBox getPictureBoxFor(Classes.Image image, ref int y)
        {
            int y_offset = y - gbMessages.VerticalScroll.Value;
            var pb = new PictureBox();
            var loadedImage = System.Drawing.Image.FromStream(image.GetStream());
            pb.Image = loadedImage;
            pb.Location = new Point(5, y_offset);
            pb.Tag = image;
            pb.MaximumSize = new Size(gbMessages.Size.Width - 50, 0);
            pb.Size = new Size(
                Math.Min(gbMessages.Size.Width - 50, loadedImage.Width),
                Math.Min(gbMessages.Size.Width - 50, loadedImage.Height) // maximum square
                );
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.BackColor = Color.Gray;
            y += 5;
            return pb;
        }

        Label getLabelFor(Classes.Image e, ref int y)
        {
            int y_offset = y - gbMessages.VerticalScroll.Value;
            var lbl = new Label();
            lbl.Tag = e;
            lbl.Text = $"{e.UploadedBy.DisplayName} uploaded {e.Name}:";
            lbl.MaximumSize = new Size(gbMessages.Width - 15, 0);
            lbl.AutoSize = true;
            lbl.Location = new Point(5, y_offset);
            y += 5;
            return lbl;
        }

        private void Client_NewImageUploaded(object sender, Classes.Image e)
        {
            if(CurrentlyUploadingSlices > 0)
            {
                Client_UploadStatus(this, new UploadStatusEvent(e, e.MaximumSlices));
                CurrentlyUploadingSlices = 0;
            }
            CurrentlyUploadingSlices = 0;
            var lbl = getLabelFor(e, ref MESSAGE_Y);
            this.gbMessages.Controls.Add(lbl);
            MESSAGE_Y += lbl.Height;
            var pb = getPictureBoxFor(e, ref MESSAGE_Y);
            this.gbMessages.Controls.Add(pb);
            MESSAGE_Y += pb.Height;
        }

        private void Client_MessageDeleted(object sender, uint e)
        {
            lock(MessageLabels)
            {
                if(MessageLabels.TryGetValue(e, out var lbl))
                {
                    string newText = "";
                    foreach(var chr in lbl.Text)
                    {
                        if(chr != ' ' && chr != '(' && chr != ')' && chr != ':')
                            newText += '█';
                        else
                            newText += chr;
                    }
                    lbl.Text = newText;
                    lbl.ContextMenu = new ContextMenu();
                    lbl.ContextMenu.MenuItems.Add($"Message Deleted");
                    lbl.Tag = null;
                }
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
            lbl.MaximumSize = new Size(gbMessages.Size.Width - 45, 0);
            lbl.Location = new Point(5, y_offset);
            lbl.ContextMenu = new ContextMenu();
            lbl.ContextMenu.Tag = message;
            lbl.ContextMenu.MenuItems.Add($"Id: {message.Id}", putTextToClipboard);
            lbl.ContextMenu.MenuItems.Add($"Author: {message.Author.UserName} ({message.Author.Id})", putTextToClipboard);
            if(message.Id > 0)
            {
                lbl.ContextMenu.MenuItems.Add($"[Delete]", deleteMessage);
                lbl.ContextMenu.MenuItems.Add($"[Edit]", editMessage);
            }
            y += 5;
            return lbl;
        }

        void deleteMessage(object sender, EventArgs e)
        {
            if(sender is MenuItem mItem && mItem.Parent.Tag is Classes.Message message)
            {
                var obj = new Newtonsoft.Json.Linq.JObject();
                obj["id"] = message.Id;
                var packet = new Packet(PacketId.RequestDeleteMessage, obj);
                Client.Send(packet.ToString());
            }
        }

        void editMessage(object sender, EventArgs e)
        {
            if(sender is MenuItem mItem && mItem.Parent.Tag is Classes.Message message)
            {
                MessageBox.Show("Feature yet to be added");
            }
        }

        void putTextToClipboard(object sender, EventArgs e)
        {
            if(sender is MenuItem mItem && mItem.Parent.Tag is Classes.Message message)
            {
                if (mItem.Text.StartsWith("Id"))
                    Clipboard.SetText(message.Id.ToString());
                else
                    Clipboard.SetText(message.Author.Id.ToString());
            }
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

		private NotificationForm notificationForm;
        private void Client_NewMessage(object sender, Classes.Message e)
        {
            Label lbl;
            lock(MessageLabels)
            {
                lbl = getLabelFor(e, ref MESSAGE_Y);
                lbl.Click += Lbl_Click;
                if (e.Id > 0)
                    MessageLabels[e.Id] = lbl;
            }
            lbl.ForeColor = e.Colour;
            int height = lbl.Height;
            if (Form.ActiveForm == this)
            {
                LAST_SEEN_MESSAGE = e.Id;
            } else
            {
                lbl.BackColor = Color.LightCoral;
                if(e.Author.Id != Client.CurrentUser.Id)
                {
                    notificationForm = notificationForm ?? new NotificationForm(this);
                    notificationForm.Show(e);
                }
            }
            this.gbMessages.Controls.Add(lbl);
            height = lbl.Height;
            MESSAGE_Y += height;
            gbMessages.HorizontalScroll.Enabled = false;
            if(!UserHasSetScroll)
            {
                gbMessages.ScrollControlIntoView(lbl);
            }
        }

		private void Lbl_Click(object sender, EventArgs e)
        {
            if(e is MouseEventArgs me)
            {
                if (me.Button.HasFlag(MouseButtons.Right))
                    return;
            }
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


        static uint _imageInProgressId = 2;

        public static int CurrentlyUploadingSlices = 0;

        [STAThread]
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (CurrentlyUploadingSlices > 0 && e != null)
                return;
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.Title = "Select Image to Upload";
            dialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowDialog(this);
            if (string.IsNullOrWhiteSpace(dialog.FileName))
                return;
            var path = System.IO.Path.GetFileName(dialog.FileName);
            var image = new Classes.Image(path, Client.CurrentUser);
            image.Id = _imageInProgressId++;
            if(!Directory.Exists("Images"))
                Directory.CreateDirectory("Images");

            if (!Directory.Exists($"Images/{image.UploadedBy.Id}"))
                Directory.CreateDirectory($"Images/{image.UploadedBy.Id}");

            if (!Directory.Exists($"Images/{image.UploadedBy.Id}/{image.Id}"))
                Directory.CreateDirectory($"Images/{image.UploadedBy.Id}/{image.Id}");

            File.Copy(dialog.FileName, image.Path, true);
            image.LoadImageIntoString();
            CurrentlyUploadingSlices = image.Slices.Count;
            var packet = new Packet(PacketId.RequestUploadImage, image.ToJson(true));
            Client.Send(packet.ToString());
            Common.AddImage(image);
        }

        public bool UserHasSetScroll = false;

        private void gbMessages_Scroll(object sender, ScrollEventArgs e)
        {
            UserHasSetScroll = true;
            this.Text = $"{e.NewValue} {e.OldValue}";
            int total = gbMessages.VerticalScroll.Value + gbMessages.VerticalScroll.LargeChange;
            if(e.NewValue > e.OldValue)
            {
                UserHasSetScroll = total >= gbMessages.VerticalScroll.Maximum;
            } else
            {
                UserHasSetScroll = true;
            }
            this.Text = $"{UserHasSetScroll}";
        }

        private void gbMessages_MouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.None)
            {
                int total = gbMessages.VerticalScroll.Value + gbMessages.VerticalScroll.LargeChange;
                if(total >= gbMessages.VerticalScroll.Maximum)
                {
                    if(e.Delta < 0)
                    {
                        UserHasSetScroll = false;
                    } else
                    {
                        UserHasSetScroll = true;
                    }
                } else
                {
                    UserHasSetScroll = true;
                }
            }
        }

        private void pbProgressBar_MouseHover(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((string)pbProgressBar.Tag))
                pbProgressBar.Tag = $"Not currently downloading/uploading anything";
            toolTip.ToolTipTitle = (string)pbProgressBar.Tag;
            toolTip.Show((string)pbProgressBar.Tag, this, pbProgressBar.Location, 5000);
        }
    }
}
