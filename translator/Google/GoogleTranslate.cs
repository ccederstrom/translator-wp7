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
using Newtonsoft.Json;
using translator.Google.Model;
using System.Collections.Generic;
using System.Diagnostics;

namespace translator.Google
{
    public class GoogleTranslate
    {
        TextBox txtBox;

        public string Result;

        public void GetResult(TextBox reference)
        {
            txtBox = reference;
        }

        public void Translate(string sourceLanguage, string query, string targetLanguage )
        {
            Debug.WriteLine("source language: " + sourceLanguage + "\ntarget language: " + targetLanguage + "\nquery: " + query);
            string translateUrl = @"https://www.googleapis.com/language/translate/v2?key=" + GoogleInfo.key + "&source=" + sourceLanguage + "&target=" + targetLanguage + "&q=" + query;
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(translateUrl));
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            
            Debug.WriteLine(e.Result);

            //Dictionary<string, Translations> translations = JsonConvert.DeserializeObject<Dictionary<string, Translations>>(e.Result);
            //foreach (KeyValuePair<string, Translations> entry in translations){
            //    Debug.WriteLine(entry.Value.translatedText);
            //    Debug.WriteLine("---");
            //}
            
            
            Dictionary<string, Data> data = JsonConvert.DeserializeObject<Dictionary<string, Data>>(e.Result);
            //Debug.WriteLine("t:" + translations.TranslatedText);
            foreach (KeyValuePair<string, Data> entry in data)
            {
                Debug.WriteLine(entry.Key);
                foreach (Translations t in entry.Value.translations )
                {
                    Debug.WriteLine(t.translatedText);
                    Result = t.translatedText;
                    if(txtBox != null)
                        txtBox.Text = t.translatedText;
                }
            }

        }
    }
}
