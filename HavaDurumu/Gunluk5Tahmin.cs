using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HavaDurumu
{
    public class Gunluk5Tahmin
    {
        Form1 ana;
        public Gunluk5Tahmin(Form1 frm)
        {
            ana = frm;
        }
        string gun1dur;
        string gun2dur;
        string gun3dur;
        string gun4dur;
        string gun5dur;

        string gun1temp;
        string gun2temp;
        string gun3temp;
        string gun4temp;
        string gun5temp;
        public void testCon()
        {
            try
            {
                System.Net.Sockets.TcpClient kontrol_client = new System.Net.Sockets.TcpClient("www.google.com.tr", 80);
                kontrol_client.Close();
            }
            catch (Exception)
            {
                DialogResult soru = MessageBox.Show("******************* İNTERNET BAĞLANTISI YOK *******************", "HATA", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (soru == DialogResult.Retry)
                {
                    testCon();
                }
            }
        }
        public void tahmin(string konum, string api)
        {

            try
            {
                string con = "https://api.openweathermap.org/data/2.5/forecast?q=" + konum + "&mode=xml&lang=tr&units=metric&&appid=" + api;
                XDocument gunluk = XDocument.Load(con);

                //İkonlar
                var gun1ikon = gunluk.Descendants("time").ElementAt(6).Descendants("symbol").ElementAt(0).Attribute("var").Value;
                var gun2ikon = gunluk.Descendants("time").ElementAt(14).Descendants("symbol").ElementAt(0).Attribute("var").Value;
                var gun3ikon = gunluk.Descendants("time").ElementAt(22).Descendants("symbol").ElementAt(0).Attribute("var").Value;
                var gun4ikon = gunluk.Descendants("time").ElementAt(30).Descendants("symbol").ElementAt(0).Attribute("var").Value;
                var gun5ikon = gunluk.Descendants("time").ElementAt(38).Descendants("symbol").ElementAt(0).Attribute("var").Value;

                //Tarihler
                var gun2tarih = gunluk.Descendants("time").ElementAt(14).Attribute("from").Value;
                var gun3tarih = gunluk.Descendants("time").ElementAt(22).Attribute("from").Value;
                var gun4tarih = gunluk.Descendants("time").ElementAt(30).Attribute("from").Value;
                var gun5tarih = gunluk.Descendants("time").ElementAt(38).Attribute("from").Value;

                //Sıcaklıklar
                gun1temp = gunluk.Descendants("time").ElementAt(6).Descendants("temperature").ElementAt(0).Attribute("value").Value;
                gun2temp = gunluk.Descendants("time").ElementAt(14).Descendants("temperature").ElementAt(0).Attribute("value").Value;
                gun3temp = gunluk.Descendants("time").ElementAt(22).Descendants("temperature").ElementAt(0).Attribute("value").Value;
                gun4temp = gunluk.Descendants("time").ElementAt(30).Descendants("temperature").ElementAt(0).Attribute("value").Value;
                gun5temp = gunluk.Descendants("time").ElementAt(38).Descendants("temperature").ElementAt(0).Attribute("value").Value;

                //Durumlar 
                gun1dur = gunluk.Descendants("time").ElementAt(6).Descendants("symbol").ElementAt(0).Attribute("name").Value;
                gun2dur = gunluk.Descendants("time").ElementAt(14).Descendants("symbol").ElementAt(0).Attribute("name").Value;
                gun3dur = gunluk.Descendants("time").ElementAt(22).Descendants("symbol").ElementAt(0).Attribute("name").Value;
                gun4dur = gunluk.Descendants("time").ElementAt(30).Descendants("symbol").ElementAt(0).Attribute("name").Value;
                gun5dur = gunluk.Descendants("time").ElementAt(38).Descendants("symbol").ElementAt(0).Attribute("name").Value;

                //Tarihler
                string[] zaman2 = gun2tarih.Split('-', 'T');
                ana.lbl2.Text = zaman2[1] + "." + zaman2[2];
                string[] zaman3 = gun3tarih.Split('-', 'T');
                ana.lbl3.Text = zaman3[1] + "." + zaman3[2];
                string[] zaman4 = gun4tarih.Split('-', 'T');
                ana.lbl4.Text = zaman3[1] + "." + zaman4[2];
                string[] zaman5 = gun5tarih.Split('-', 'T');
                ana.lbl5.Text = zaman5[1] + "." + zaman5[2];

                //Resimler
                string[] Gun1ResimAdi = gun1ikon.Split('n', 'd');
                ana.pctYarin.Image = GetResourceImage(Gun1ResimAdi[0] + "d");
                string[] Gun2ResimAdi = gun2ikon.Split('n', 'd');
                ana.pct2.Image = GetResourceImage(Gun2ResimAdi[0] + "d");
                string[] Gun3ResimAdi = gun3ikon.Split('n', 'd');
                ana.pct3.Image = GetResourceImage(Gun3ResimAdi[0] + "d");
                string[] Gun4ResimAdi = gun4ikon.Split('n', 'd');
                ana.pct4.Image = GetResourceImage(Gun4ResimAdi[0] + "d");
                string[] Gun5ResimAdi = gun5ikon.Split('n', 'd');
                ana.pct5.Image = GetResourceImage(Gun5ResimAdi[0] + "d");
            }
            catch (WebException ex)
            {
                MessageBox.Show("******************* GEÇERSİZ KONUM *******************\n\n" + ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ana.textBox1.Clear();
                ana.textBox1.Focus();
            }
        }
        public void DurumGoster()
        {
            ana.lbl1Durum.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(gun1dur);
            ana.lbl2Durum.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(gun2dur);
            ana.lbl3Durum.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(gun3dur);
            ana.lbl4Durum.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(gun4dur);
            ana.lbl5Durum.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(gun5dur);
        }
        public void SicaklikGoster()
        {
            ana.lbl1Durum.Text = gun1temp + "°C";
            ana.lbl2Durum.Text = gun2temp + "°C";
            ana.lbl3Durum.Text = gun3temp + "°C";
            ana.lbl4Durum.Text = gun4temp + "°C";
            ana.lbl5Durum.Text = gun5temp + "°C";
        }

        public static Image GetResourceImage(string imageName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetName().Name + ".Properties.Resources";
            var resourceManager = new ResourceManager(resourceName, assembly);
            return (Bitmap)resourceManager.GetObject(imageName);
        }
    }
}



