using System;
using System.Drawing;
using System.Windows.Forms;
using NPSMLib;
using System.Diagnostics;
using CSDeskBand;

namespace Muzyka
{
    public partial class UserControl1: UserControl
    {
        static NowPlayingSessionManager manager;
        static MediaPlaybackDataSource src;
        public UserControl1(CSDeskBand.CSDeskBandWin w)
        {
            InitializeComponent();
            this.MouseEnter += label2_MouseEnter;
            label1.MouseEnter += label2_MouseEnter;
            pictureBox1.MouseEnter += label2_MouseEnter;
            button1.MouseLeave += UserControl1_MouseLeave;
            button2.MouseLeave += UserControl1_MouseLeave;
            button3.MouseLeave += UserControl1_MouseLeave;
            //textBox1.GotFocus += (o, e) => w.UpdateFocus(true);
            manager = new NowPlayingSessionManager();
            manager.SessionListChanged += Manager_SessionListChanged;
            var sessions = manager.GetSessions();
            for (int i = 0; i < sessions.Length; i++)
            {
                var session = sessions[i];
                var processes = Process.GetProcessesByName("spotify");
                for (int j = 0; j < processes.Length; j++)
                {
                    var process = processes[j];
                    if (session.PID == process.Id)
                    {
                        src = session.ActivateMediaPlaybackDataSource();
                        src.MediaPlaybackDataChanged += Src_MediaPlaybackDataChanged;
                        label2.Text = src.GetMediaObjectInfo().Title;
                        label1.Text = src.GetMediaObjectInfo().AlbumArtist;
                        var origimg = new Bitmap(Image.FromStream(src.GetThumbnailStream()));
                        var cropimg = origimg.Clone(new Rectangle(0, 0, origimg.Width, origimg.Height - 50), System.Drawing.Imaging.PixelFormat.DontCare);
                        pictureBox1.Image = cropimg;
                        if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Playing)
                        {
                            button1.Text = "⏸️";
                        }
                        else if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Paused)
                        {
                            button1.Text = "▶";
                        }
                    }
                }
            }
        }

        private void Src_MediaPlaybackDataChanged(object sender, MediaPlaybackDataChangedArgs e)
        {
            src = manager.CurrentSession.ActivateMediaPlaybackDataSource();
            label2.Text = src.GetMediaObjectInfo().Title;
            label1.Text = src.GetMediaObjectInfo().AlbumArtist;
            var origimg = new Bitmap(Image.FromStream(src.GetThumbnailStream()));
            var cropimg = origimg.Clone(new Rectangle(0, 0, origimg.Width, origimg.Height - 50), System.Drawing.Imaging.PixelFormat.DontCare);
            pictureBox1.Image = cropimg;
            if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Playing)
            {
                button1.Text = "⏸️";
            }
            else if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Paused)
            {
                button1.Text = "▶";
            }
        }

        private void Manager_SessionListChanged(object sender, NowPlayingSessionManagerEventArgs e)
        {
            var sessions = manager.GetSessions();
            for (int i = 0; i < sessions.Length; i++)
            {
                var session = sessions[i];
                var processes = Process.GetProcessesByName("spotify");
                for (int j = 0; j < processes.Length; j++)
                {
                    var process = processes[j];
                    if (session.PID == process.Id)
                    {
                        src = session.ActivateMediaPlaybackDataSource();
                        src.MediaPlaybackDataChanged += Src_MediaPlaybackDataChanged;
                        label2.Text = src.GetMediaObjectInfo().Title;
                        label1.Text = src.GetMediaObjectInfo().AlbumArtist;
                        var origimg = new Bitmap(Image.FromStream(src.GetThumbnailStream()));
                        var cropimg = origimg.Clone(new Rectangle(0, 0, origimg.Width, origimg.Height - 50), System.Drawing.Imaging.PixelFormat.DontCare);
                        pictureBox1.Image = cropimg;
                        if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Playing)
                        {
                            button1.Text = "⏸️";
                        }
                        else if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Paused)
                        {
                            button1.Text = "▶";
                        }
                    }
                }
            }
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if ((Keys)m.WParam == Keys.Tab)
            {
                var selected = SelectNextControl(ActiveControl, true, true, true, true);
                return true;
            }

            return base.ProcessKeyPreview(ref m);
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            button1.Visible = button2.Visible = button3.Visible = true;
        }

        private void UserControl1_MouseLeave(object sender, EventArgs e)
        {
            label1.Visible = true;
            label2.Visible = true;
            button1.Visible = button2.Visible = button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Playing)
            {
                src.SendMediaPlaybackCommand(MediaPlaybackCommands.Pause);
                button1.Text = "▶";
            }
            else if (src.GetMediaPlaybackInfo().PlaybackState == MediaPlaybackState.Paused)
            {
                src.SendMediaPlaybackCommand(MediaPlaybackCommands.Play);
                button1.Text = "⏸";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            src.SendMediaPlaybackCommand(MediaPlaybackCommands.Previous);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            src.SendMediaPlaybackCommand(MediaPlaybackCommands.Next);
        }
    }
}
