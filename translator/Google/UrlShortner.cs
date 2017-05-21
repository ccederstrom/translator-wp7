using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;

namespace translator.Google
{
    public class UrlShortner
    {
        public UrlShortner()
        {
            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.Headers["Content-Type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            client.UploadStringAsync(new Uri("https://www.googleapis.com/urlshortener/v1/url"), "POST", "{\"longUrl\": \"http://www.google.com/\"}");
        }

        public UrlShortner(string url)
        {
            WebClient client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
            client.Headers["Content-Type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            client.UploadStringAsync(new Uri("https://www.googleapis.com/urlshortener/v1/url"), "POST", "{\"longUrl\": \""+ url + "\"}");
        }


        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            MessageBox.Show(e.Result);
        } 
    }
}
