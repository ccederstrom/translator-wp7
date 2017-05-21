using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace translator.Speech
{
    public class SpeechTTS
    {
        #region Variables & EventHandlers
        private List<string> lstSpeakLanguages = new List<string>();
        private List<string> lstTranslateLanguages = new List<string>();
        private string appId, translatedvalue;
        private bool ResumeMusic = false, isAudioPlaying = false;
        private int countDown = 0;
        private System.Windows.Threading.DispatcherTimer myDispatchTimer = new System.Windows.Threading.DispatcherTimer();
        private const string SpokenLanguages = "http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForSpeak?appId={0}";
        private const string speakUrl = "http://api.microsofttranslator.com/V2/Http.svc/Speak?appId={0}&text={1}&language={2}";
        private const string translateLanguages = "http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForTranslate?appId={0}";
        private const string translateUrl = "http://api.microsofttranslator.com/V2/Http.svc/Translate?appId={0}&from={1}&to={2}&text={3}";

        public event EventHandler SpeakingStatusChanged;
        public event EventHandler TextTranslated;
        public event EventHandler SpokenLanguagesReady;
        public event EventHandler TranslatableLanguagesReady;
        public bool EnableZuneInterruption { get; set; }
        public string SpeakLanguage { get; set; }

        public bool AudioIsPlaying
        {
            get
            {
                return isAudioPlaying;
            }
        }
        public string TranslatedText
        {
            get
            {
                return translatedvalue;
            }
        }
        public List<string> SpeakLanguages
        {
            get
            {
                return lstSpeakLanguages;
            }
        }
        public List<string> TranslatableLanguages
        {
            get
            {
                return lstTranslateLanguages;
            }
        }
        #endregion
        //Constructor
        public SpeechTTS(string AppID)
        {
            appId = AppID;
            EnableZuneInterruption = true;
            SpeakLanguage = "en";
            myDispatchTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            myDispatchTimer.Tick += new EventHandler(AudioCountDown);
        }
        //Just makes sure that the APPID is not empty
        private void CheckappId()
        {
            if (String.IsNullOrEmpty(appId.Trim()))
            {
                MessageBox.Show("No API key provided for speech or translation.\nGo here for key: http://www.bing.com/developers/appids.aspx");
                return;
            }
        }
        #region Loads language codes into lists
        public void LoadLanguageCodes()
        {
            CheckappId();
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetSpeakLanguages);
            wc.DownloadStringAsync(new Uri(string.Format(SpokenLanguages, appId)));
            WebClient wc1 = new WebClient();
            wc1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetTranslatableLanguages);
            wc1.DownloadStringAsync(new Uri(string.Format(translateLanguages, appId)));
        }
        private void GetSpeakLanguages(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                    return;
                XElement tDocument = XElement.Parse(e.Result);
                IEnumerable<XElement> elements = tDocument.Elements();
                foreach (XElement tElement in elements)
                {
                    lstSpeakLanguages.Add(tElement.Value);
                }
                SpokenLanguagesReady(sender, e);
            }
            catch (Exception ex)
            {
            }
        }
        private void GetTranslatableLanguages(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                    return;
                XElement tDocument = XElement.Parse(e.Result);
                IEnumerable<XElement> elements = tDocument.Elements();
                foreach (XElement tElement in elements)
                {
                    lstTranslateLanguages.Add(tElement.Value);
                }
                TranslatableLanguagesReady(sender, e);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region Translation features
        public void Translate(string text, string from, string to)
        {
            CheckappId();
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return;
            else
            {

                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Translation_Completed);
                wc.DownloadStringAsync(new Uri(string.Format(translateUrl, appId, from, to, text)));
            }
        }
        private void Translation_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                    return;
                XElement tDocument = XElement.Parse(e.Result);
                translatedvalue = tDocument.Value;
                TextTranslated(translatedvalue, e);

            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region Text To Speech
        //Text to speech method
        public void SpeakText(string speechstring)
        {
            CheckappId();
            if (string.IsNullOrEmpty(speechstring))
                return;
            else
            {

                WebClient wc = new WebClient();
                wc.OpenReadCompleted += new OpenReadCompletedEventHandler(speak_Completed);
                wc.OpenReadAsync(new Uri(string.Format(speakUrl, appId, speechstring, SpeakLanguage)));
            }
        }
        void speak_Completed(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                PlayAudio(e.Result, e);
            }
            catch (Exception ex)
            {
            }
        }
        private void PlayAudio(Stream input, OpenReadCompletedEventArgs e)
        {
            try
            {
                //Make sure audio is not playing otherwise don't play at all.
                if (isAudioPlaying == false)
                {
                    if (Microsoft.Xna.Framework.Media.MediaPlayer.State == MediaState.Playing && EnableZuneInterruption)
                    {
                        ResumeMusic = true;
                        Microsoft.Xna.Framework.Media.MediaPlayer.Pause();
                    }
                    FrameworkDispatcher.Update();
                    SoundEffect tEffect = SoundEffect.FromStream(input);
                    tEffect.Play();
                    countDown = tEffect.Duration.Seconds;
                    isAudioPlaying = true;
                    myDispatchTimer.Start();

                    SpeakingStatusChanged(input, e);
                }
            }
            catch (Exception ex)
            {
            }
        }
        //Dispatch timer counts down for X seconds until audio is done playing;
        private void AudioCountDown(object sender, EventArgs e)
        {
            countDown--;
            if (countDown <= 0)
            {
                countDown = 0;
                isAudioPlaying = false;
                if (ResumeMusic == true && EnableZuneInterruption)
                {
                    ResumeMusic = false;
                    Microsoft.Xna.Framework.Media.MediaPlayer.Resume();
                }

                myDispatchTimer.Stop();
                AudioFinished(sender, e);
            }
        }
        private void AudioFinished(object sender, EventArgs e)
        {
            if (SpeakingStatusChanged != null)
                SpeakingStatusChanged(sender, e);
        }
        #endregion

    }
}
