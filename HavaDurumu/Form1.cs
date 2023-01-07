using System;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Drawing;
using System.Globalization;
using Microsoft.Win32;
using System.Net;
using System.Net.NetworkInformation;
using HavaDurumu.Properties;
using System.Resources;
using System.Reflection;

namespace HavaDurumu
{
    public partial class Form1 : Form 
    {
        public Form1()
        {
            InitializeComponent();
            tahmin = new Gunluk5Tahmin(this);
        }
        string konum;
        string api = "b4ba52b9fbe0ee5d523da67c230749bb";
        XDocument hava;
        Gunluk5Tahmin tahmin;

        void havaDurum(string konum, string api)
        {

            tahmin.testCon();
            try
            {
                string con = "https://api.openweathermap.org/data/2.5/weather?q=" + konum + "&mode=xml&lang=tr&units=metric&appid=" + api;
                XDocument hava = XDocument.Load(con);
                var sicaklik = hava.Descendants("temperature").ElementAt(0).Attribute("value").Value;
                var hsicaklik = hava.Descendants("feels_like").ElementAt(0).Attribute("value").Value;
                var bolge = hava.Descendants("country").ElementAt(0).Value;
                var bulut = hava.Descendants("clouds").ElementAt(0).Attribute("value").Value;
                var ruzgar = hava.Descendants("speed").ElementAt(0).Attribute("value").Value;
                var nem = hava.Descendants("humidity").ElementAt(0).Attribute("value").Value;
                var durum = hava.Descendants("weather").ElementAt(0).Attribute("value").Value;
                var durumIkon = hava.Descendants("weather").ElementAt(0).Attribute("icon").Value;
                var sonUpdate = hava.Descendants("lastupdate").ElementAt(0).Attribute("value").Value;
                float sicak = Convert.ToSingle(sicaklik.ToString());
                string[] zaman = sonUpdate.Split('-', 'T');
                label2.Text = zaman[0] + "." + zaman[1] + "." + zaman[2] + " Saat: " + zaman[3];
                label1.Text = konum.ToUpper() + ", " + bolge;
                label4.Text = sicaklik + "°C";
                label5.Text = "%" + bulut;
                label7.Text = ruzgar + " m/s";
                label8.Text = "%" + nem;
                label12.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(durum);
                label13.Text = hsicaklik + "°C";
                label10.Text = "%0";
                string[] resimAdi = durumIkon.Split('n','d');
                string resim = resimAdi[0] + "d";
                label4.ForeColor = Color.White;
                pictureBox1.Image = GetResourceImage(resim);
                if (durum == "açık" && sicak > 3400f)
                {
                    pictureBox1.ImageLocation = Application.StartupPath + @"/durumlar/SCK.png";
                    label4.ForeColor = Color.FromArgb(189, 40, 0);
                }
                else if (durum == "hafif yağmur")
                {
                    var yagis = hava.Descendants("precipitation").ElementAt(0).Attribute("value").Value;
                    label10.Text = "%" + (Convert.ToSingle(yagis + ""));
                }
                else if (durum == "şiddetli yağmur")
                {
                    pictureBox1.Image = Resources._11d;
                    var yagis = hava.Descendants("precipitation").ElementAt(0).Attribute("value").Value;
                    label10.Text = "%" + (Convert.ToSingle(yagis + ""));

                }
                else if (durum == "orta şiddetli yağmur")
                {
                    pictureBox1.Image = Resources._11d;
                    var yagis = hava.Descendants("precipitation").ElementAt(0).Attribute("value").Value;
                    label10.Text = "%" + (Convert.ToSingle(yagis + ""));
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("******************* GEÇERSİZ KONUM *******************\n\n" + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox1.Focus();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            konum = textBox1.Text;
            havaDurum(konum, api);
            tahmin.tahmin(konum, api);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Registry.CurrentUser.OpenSubKey("hava").GetValue("havaKey") == null)
            {
                konum = "istanbul";
            }
            else
            {
                konum = Registry.CurrentUser.OpenSubKey("hava").GetValue("havaKey").ToString();
            }
            havaDurum(konum, api);
            tahmin.tahmin(konum, api);
            tahmin.DurumGoster();
            timer1.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey("hava").SetValue("havaKey", textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("****************** Konumunuzu Giriniz ******************\n\n " + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox1.Focus();

            }
        }
        public static Image GetResourceImage(string imageName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetName().Name + ".Properties.Resources";
            var resourceManager = new ResourceManager(resourceName, assembly);
            return (Bitmap)resourceManager.GetObject(imageName);
        }
        int sayac;

        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;
            if (sayac ==3)
            {
                tahmin.SicaklikGoster();
            }
            else if(sayac ==5)
            {
                tahmin.DurumGoster();
                sayac = 0;
            }
           
        }
    }
}
