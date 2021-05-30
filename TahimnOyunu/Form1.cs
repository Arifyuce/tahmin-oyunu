using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace TahimnOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int _clickedPanelCount = 0;
        List<Card> _cardList = new List<Card>();
        Card _openedCard;
        bool isOnClick = false;
        String ServerUrl = "C:\\Users\\Genius\\Desktop\\TahimnOyunu\\";
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        int point = 0;
        public int zaman = 180;


        private void Form1_Load(object sender, EventArgs e)
        {
            lblPuan.Text = "0";
            
        }

        public async void panelClick(object sender, EventArgs args)
        {
            String soundLocation = ServerUrl + "sounds\\";
            if (!isOnClick)
            {
                Panel panel = sender as Panel;
                panel.Visible = false;

                if (_clickedPanelCount == 1)
                {
                    isOnClick = true;
                    await Task.Delay(1000);
                    Card chosenCard = _cardList.Where(x => x.panelName == panel.Name).FirstOrDefault();
                    if (_openedCard.path != chosenCard.path)
                    {
                        player.SoundLocation = soundLocation + "mixkit-game-show-buzz-in-3090.wav";
                        player.Play();
                        var opened_panel = Controls.Find(_openedCard.panelName, true);
                        opened_panel[0].Visible = true;
                        panel.Visible = true;
                        await Task.Delay(400);
                    }
                    else
                    {
                        player.SoundLocation = soundLocation + "mixkit-space-coin-win-notification-271.wav";
                        player.Play();
                        point += 10;
                        lblPuan.Text = (point).ToString();
                        if (point == 50)
                        {
                            timer1.Enabled = false;
                            PictureBox congrats = new PictureBox();
                            congrats.Location = new Point(this.ClientSize.Width / 4 - congrats.Size.Width / 2,
                            10);
                            congrats.Image = new Bitmap(ServerUrl + "icons\\congrats.jpg");
                            congrats.ClientSize = new Size(500, 400);
                            Controls.Add(congrats);
                            congrats.BringToFront();
                            player.SoundLocation = soundLocation + "mixkit-cartoon-monkey-applause-103.wav";
                            player.Play();

                        }
                    }
                    _clickedPanelCount = 0;
                    isOnClick = false;
                }
                else if (_clickedPanelCount == 0)
                {
                    _openedCard = _cardList.Where(x => x.panelName == panel.Name).FirstOrDefault();
                    _clickedPanelCount++;
                }
            }
        }
        
        public void start()
        {
            timer1.Enabled = true;
            lblPuan.Text = "0";
            timer.Text = "180";
            String imagePath = ServerUrl + "images\\";
            string[] image_arr =  Directory.GetFiles(imagePath);
            var pbxCollection = new List<PictureBox>();
            int x = 50, y = 50;
            List<int> randomNumbers = new List<int>();

            int count = 0;
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < image_arr.Length; i++)
                {
                    count++;
                    if (j == 1 && i == 0) { y += 140; x = 50; randomNumbers.Clear(); }
                    int rand = new Random().Next(0, image_arr.Length);
                    while (randomNumbers.Contains(rand))
                    {
                        rand = new Random().Next(0, image_arr.Length);
                    }
                    randomNumbers.Add(rand);
                    Image img = new Bitmap(image_arr[rand]);

                    Panel _panel = new Panel();
                    _panel.BackColor = System.Drawing.ColorTranslator.FromHtml("#6171ff");
                    _panel.ClientSize = new Size(128, 128);
                    _panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    _panel.Location = new Point(x, y);
                    _panel.Name = "_panel_" + count;
                    _panel.Click += panelClick;
                    PictureBox pb = new PictureBox();
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.ClientSize = new Size(128, 128);
                    pb.Location = new Point(x, y);
                    pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    pb.Image = (Image)img;
                    this.Controls.Add(pb);
                    this.Controls.Add(_panel);
                    _panel.BringToFront();
                    x += 140;
                    Card card = new Card();
                    card.id = rand;
                    card.path = image_arr[rand];
                    card.panelName = _panel.Name;
                    _cardList.Add(card);
                }
            }
            
            
        }

        public void closeAll()
        {
            point = 0;
            lblPuan.Text = "";
            start();
        }

        private void baslat_Click(object sender, EventArgs e)
        {
            closeAll();
            baslat.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer.Text = zaman.ToString();
            zaman--;
            if (zaman == 0)
            {
                timer.Text = "zaman doldu";
            }
        }
    }
}
