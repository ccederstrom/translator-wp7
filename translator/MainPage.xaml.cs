// Reference: http://wptts.codeplex.com/documentation
// List of available languages can be found at: http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForSpeak?appId=00EFD818BED4355748D0D1AF0F19932E119F1E80  
// List of available languages for speech synthesis (populated when LoadLanguageCodes method is called): http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForSpeak?appId=00EFD818BED4355748D0D1AF0F19932E119F1E80 
// List of available languages for text translation (populated when LoadLanguageCodes method is called): http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForTranslate?appId=00EFD818BED4355748D0D1AF0F19932E119F1E80  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using System.Diagnostics;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using System.IO;
using System.Windows.Threading;

using translator.Database.Model;
using System.Windows.Media.Imaging;
using com.nuance.nmdp.speechkit;
using com.nuance.nmdp.speechkit.oem;
using System.Windows.Controls.Primitives;
using System.Threading;
using com.nuance.nmdp.speechkit.util;
using translator.Bing;
using translator.Database;
using translator.Speech;
using translator.AzureDataMarket;

namespace translator
{
    public delegate void CancelSpeechKitEventHandler();

    public partial class MainPage : PhoneApplicationPage, RecognizerListener, VocalizerListener
    {
        #region Nuance setup
        SpeechKit _speechKit = null;
        Recognizer _recognizer = null;
        Vocalizer _vocalizer = null;
        Prompt _beep = null;
        OemConfig _oemconfig = new OemConfig();
        object _handler = null;
        string _ttsText = string.Empty;
        string _ttsVoice = string.Empty;
        Popup _popup = new Popup();
        string serverIp = Nuance.AppInfo.SpeechKitServer;
        string serverPort = Nuance.AppInfo.SpeechKitPort.ToString();
        #endregion

        IsolatedStorageSettings isolatedStorage;
        SpeechTTS tts;
        bool IsSelectionChangedEnabled = false;
        DispatcherTimer mTimer = new System.Windows.Threading.DispatcherTimer();

        TranslatorData bingTranslator;

        #region Initialization

        public MainPage()
        {
            InitializeComponent();
            IsSelectionChangedEnabled = false;
            // Set the page DataContext property to the ViewModel. http://msdn.microsoft.com/en-us/library/hh286405(v=VS.92).aspx
            this.DataContext = App.ViewModel;

            bingTranslator = new TranslatorData();

            // All history
            historyListBox.ItemsSource = App.ViewModel.AllTranslatorHistory;
            // Starred history
            favoriteListBox.ItemsSource = App.ViewModel.FavoriteTranslatorHistory;

            #region Nuance setup
            //listBoxTtsVoice.Items.Add("Tom");
            //listBoxTtsVoice.Items.Add("Samantha");
            //listBoxTtsVoice.SelectedIndex = 0;
            speechkitInitialize();

            App.CancelSpeechKit += new CancelSpeechKitEventHandler(App_CancelSpeechKit);
            #endregion

            #region Trial
            if (App.IsTrial == true)
            {
                //Ads Timer
                mTimer.Interval = new TimeSpan(0, 0, 0, 0, 300000); // 5 mins
                mTimer.Tick += new EventHandler(mTimer_Tick);
            }
            else
            {
                AdControlPanel.Visibility = System.Windows.Visibility.Collapsed;
                pivotControl.Margin = new Thickness(0);
            }
            #endregion

            #region Paid version Check
            //if (App.isPaidVersion == true)
            //{
            //    // disables ads
            //    adControl.Visibility = System.Windows.Visibility.Collapsed;
            //    //adControl.IsAutoRefreshEnabled = false;
            //    adControl.IsEnabled = false;
            //    adControl.IsAutoCollapseEnabled = false;
            //    pivotControl.Margin = new Thickness(0, 0, 0, 0);
            //    // direction.Margin = new Thickness(0, 0, 0, 0);
            //}
            //else
            //{
            //    // enable ads
            //    adControl.Visibility = System.Windows.Visibility.Visible;
            //    //adControl.IsAutoRefreshEnabled = true;
            //    adControl.IsEnabled = true;
            //    adControl.IsAutoCollapseEnabled = false;
            //    //Ads Timer
            //    mTimer.Interval = new TimeSpan(0, 0, 0, 0, 300000); // 5 mins
            //    mTimer.Tick += new EventHandler(mTimer_Tick);
            //}
            #endregion

            #region Translator initalization
            tts = new SpeechTTS(BingInfo.AppID);
            tts.LoadLanguageCodes();
            tts.EnableZuneInterruption = false;
            Debug.WriteLine("HELLO TRANSLATOR\n-----------------");
            tts.SpeakLanguage = "ja";
            #endregion

            #region Initialize Languages
            List<Languages> mFrom_Languages = new List<Languages>();
            #region Languages for Text translation
            // Google languages http://code.google.com/apis/language/translate/v2/using_rest.html
            // languages for text translation
            //      - <ArrayOfstring xmlns="http://schemas.microsoft.com/2003/10/Serialization/Arrays" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            mFrom_Languages.Add(new Languages() { Full = "Afrikaans *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Afrikaans.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Albanian *", Short = "sq", Speech = false, FlagImagePath = "/Images/Flags/200px-Albanian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Arabic *", Short = "ar", Speech = false, FlagImagePath = "/Images/Flags/200px-Arabic.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Azerbaijani *", Short = "az", Speech = false, FlagImagePath = "/Images/Flags/200px-Azerbaijani.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Basque *", Short = "eu", Speech = false, FlagImagePath = "/Images/Flags/200px-Basque.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Bengali *", Short = "bn", Speech = false, FlagImagePath = "/Images/Flags/200px-Bengali.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Belarusian *", Short = "be", Speech = false, FlagImagePath = "/Images/Flags/200px-Belarusian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Bulgarian *", Short = "bg", Speech = false, FlagImagePath = "/Images/Flags/200px-Bulgarian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Catalan", Short = "ca", Speech = true, FlagImagePath = "/Images/Flags/200px-Catalan.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Chinese Simpified", Short = "zh-CHS", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mFrom_Languages.Add(new Languages() { Full = "Chinese Traditional", Short = "zh-CHT", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese_Traditional.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mFrom_Languages.Add(new Languages() { Full = "Croatian *", Short = "hr", Speech = false, FlagImagePath = "/Images/Flags/200px-Croatian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Czech *", Short = "cs", Speech = false, FlagImagePath = "/Images/Flags/200px-Czech.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Danish", Short = "da", Speech = true, FlagImagePath = "/Images/Flags/200px-Danish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Dutch", Short = "nl", Speech = true, FlagImagePath = "/Images/Flags/200px-Dutch.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "English", Short = "en", Speech = true, FlagImagePath = "/Images/Flags/200px-English.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Esperanto *", Short = "eo", Speech = false, FlagImagePath = "/Images/Flags/200px-Esperanto.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Estonian *", Short = "et", Speech = false, FlagImagePath = "/Images/Flags/200px-Estonian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Filipino *", Short = "tl", Speech = false, FlagImagePath = "/Images/Flags/200px-Filipino.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Finnish", Short = "fi", Speech = true, FlagImagePath = "/Images/Flags/200px-Finnish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "French", Short = "fr", Speech = true, FlagImagePath = "/Images/Flags/200px-French.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Galician *", Short = "gl", Speech = false, FlagImagePath = "/Images/Flags/200px-Galician.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Georgian *", Short = "ka", Speech = false, FlagImagePath = "/Images/Flags/200px-Georgian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "German", Short = "de", Speech = true, FlagImagePath = "/Images/Flags/200px-German.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Greek *", Short = "el", Speech = false, FlagImagePath = "/Images/Flags/200px-Greek.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Gujarati *", Short = "gu", Speech = false, FlagImagePath = "/Images/Flags/200px-Gujarati.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Haitian Creole *", Short = "ht", Speech = false, FlagImagePath = "/Images/Flags/200px-Haitian_Creole.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Hebrew *", Short = "he", Speech = false, FlagImagePath = "/Images/Flags/200px-Hebrew.png", isBing = true, isBothAvailable = false }); // google code different
            mFrom_Languages.Add(new Languages() { Full = "Hindi *", Short = "hi", Speech = false, FlagImagePath = "/Images/Flags/200px-Hindi.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Hungarian *", Short = "hu", Speech = false, FlagImagePath = "/Images/Flags/200px-Hungarian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Icelandic *", Short = "is", Speech = false, FlagImagePath = "/Images/Flags/200px-Icelandic.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Indonesian *", Short = "id", Speech = false, FlagImagePath = "/Images/Flags/200px-Indonesian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Irish *", Short = "ga", Speech = false, FlagImagePath = "/Images/Flags/200px-Irish.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Italian", Short = "it", Speech = true, FlagImagePath = "/Images/Flags/200px-Italian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Japanese", Short = "ja", Speech = true, FlagImagePath = "/Images/Flags/200px-Japanese.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Kannada *", Short = "kn", Speech = false, FlagImagePath = "/Images/Flags/200px-Kannada.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Korean", Short = "ko", Speech = true, FlagImagePath = "/Images/Flags/200px-Korean.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Latin *", Short = "la", Speech = false, FlagImagePath = "/Images/Flags/200px-Latin.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Latvian *", Short = "lv", Speech = false, FlagImagePath = "/Images/Flags/200px-Latvian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Lithuanian", Short = "lt", Speech = true, FlagImagePath = "/Images/Flags/200px-Lithuanian.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Macedonian *", Short = "mk", Speech = false, FlagImagePath = "/Images/Flags/200px-Macedonian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Malay *", Short = "ms", Speech = false, FlagImagePath = "/Images/Flags/200px-Malay.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Maltese *", Short = "mt", Speech = false, FlagImagePath = "/Images/Flags/200px-Maltese.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Norwegian", Short = "no", Speech = true, FlagImagePath = "/Images/Flags/200px-Norwegian.png", isBing = true, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Persian *", Short = "fa", Speech = false, FlagImagePath = "/Images/Flags/200px-Persian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Polish", Short = "pl", Speech = true, FlagImagePath = "/Images/Flags/200px-Polish.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Portuguese", Short = "pt", Speech = true, FlagImagePath = "/Images/Flags/200px-Portuguese.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Romanian *", Short = "ro", Speech = false, FlagImagePath = "/Images/Flags/200px-Romanian.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Russian", Short = "ru", Speech = true, FlagImagePath = "/Images/Flags/200px-Russian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Serbian *", Short = "sr", Speech = false, FlagImagePath = "/Images/Flags/200px-Serbian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Slovak *", Short = "sk", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovak.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Slovenian *", Short = "sl", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovenian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Spanish", Short = "es", Speech = true, FlagImagePath = "/Images/Flags/200px-Spanish.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Swahili *", Short = "sw", Speech = false, FlagImagePath = "/Images/Flags/200px-Swahili.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Swedish", Short = "sv", Speech = true, FlagImagePath = "/Images/Flags/200px-Swedish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Tamil *", Short = "ta", Speech = false, FlagImagePath = "/Images/Flags/200px-Tamil.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Telugu *", Short = "te", Speech = false, FlagImagePath = "/Images/Flags/200px-Telugu.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Thai *", Short = "th", Speech = false, FlagImagePath = "/Images/Flags/200px-Thai.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Turkish *", Short = "tr", Speech = false, FlagImagePath = "/Images/Flags/200px-Turkish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Ukrainian *", Short = "uk", Speech = false, FlagImagePath = "/Images/Flags/200px-Ukrainian.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Urdu *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Urdu.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Vietnamese *", Short = "vi", Speech = false, FlagImagePath = "/Images/Flags/200px-Vietnamese.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Welsh *", Short = "cy", Speech = false, FlagImagePath = "/Images/Flags/200px-Welsh.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Yiddish *", Short = "yi", Speech = false, FlagImagePath = "/Images/Flags/200px-Yiddish.png", isBing = false, isBothAvailable = false });

            //</ArrayOfstring>

            #endregion

            List<Languages> mTo_Languages = new List<Languages>();
            #region Languages for Text translation
            // languages for text translation
            //      - <ArrayOfstring xmlns="http://schemas.microsoft.com/2003/10/Serialization/Arrays" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            mTo_Languages.Add(new Languages() { Full = "Afrikaans *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Afrikaans.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Albanian *", Short = "sq", Speech = false, FlagImagePath = "/Images/Flags/200px-Albanian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Arabic *", Short = "ar", Speech = false, FlagImagePath = "/Images/Flags/200px-Arabic.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Azerbaijani *", Short = "az", Speech = false, FlagImagePath = "/Images/Flags/200px-Azerbaijani.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Basque *", Short = "eu", Speech = false, FlagImagePath = "/Images/Flags/200px-Basque.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Bengali *", Short = "bn", Speech = false, FlagImagePath = "/Images/Flags/200px-Bengali.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Belarusian *", Short = "be", Speech = false, FlagImagePath = "/Images/Flags/200px-Belarusian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Bulgarian *", Short = "bg", Speech = false, FlagImagePath = "/Images/Flags/200px-Bulgarian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Catalan", Short = "ca", Speech = true, FlagImagePath = "/Images/Flags/200px-Catalan.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Chinese Simpified", Short = "zh-CHS", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mTo_Languages.Add(new Languages() { Full = "Chinese Traditional", Short = "zh-CHT", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese_Traditional.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mTo_Languages.Add(new Languages() { Full = "Croatian *", Short = "hr", Speech = false, FlagImagePath = "/Images/Flags/200px-Croatian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Czech *", Short = "cs", Speech = false, FlagImagePath = "/Images/Flags/200px-Czech.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Danish", Short = "da", Speech = true, FlagImagePath = "/Images/Flags/200px-Danish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Dutch", Short = "nl", Speech = true, FlagImagePath = "/Images/Flags/200px-Dutch.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "English", Short = "en", Speech = true, FlagImagePath = "/Images/Flags/200px-English.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Esperanto *", Short = "eo", Speech = false, FlagImagePath = "/Images/Flags/200px-Esperanto.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Estonian *", Short = "et", Speech = false, FlagImagePath = "/Images/Flags/200px-Estonian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Filipino *", Short = "tl", Speech = false, FlagImagePath = "/Images/Flags/200px-Filipino.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Finnish", Short = "fi", Speech = true, FlagImagePath = "/Images/Flags/200px-Finnish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "French", Short = "fr", Speech = true, FlagImagePath = "/Images/Flags/200px-French.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Galician *", Short = "gl", Speech = false, FlagImagePath = "/Images/Flags/200px-Galician.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Georgian *", Short = "ka", Speech = false, FlagImagePath = "/Images/Flags/200px-Georgian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "German", Short = "de", Speech = true, FlagImagePath = "/Images/Flags/200px-German.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Greek *", Short = "el", Speech = false, FlagImagePath = "/Images/Flags/200px-Greek.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Gujarati *", Short = "gu", Speech = false, FlagImagePath = "/Images/Flags/200px-Gujarati.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Haitian Creole *", Short = "ht", Speech = false, FlagImagePath = "/Images/Flags/200px-Haitian_Creole.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Hebrew *", Short = "he", Speech = false, FlagImagePath = "/Images/Flags/200px-Hebrew.png", isBing = true, isBothAvailable = false }); // google code different
            mTo_Languages.Add(new Languages() { Full = "Hindi *", Short = "hi", Speech = false, FlagImagePath = "/Images/Flags/200px-Hindi.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Hungarian *", Short = "hu", Speech = false, FlagImagePath = "/Images/Flags/200px-Hungarian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Icelandic *", Short = "is", Speech = false, FlagImagePath = "/Images/Flags/200px-Icelandic.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Indonesian *", Short = "id", Speech = false, FlagImagePath = "/Images/Flags/200px-Indonesian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Irish *", Short = "ga", Speech = false, FlagImagePath = "/Images/Flags/200px-Irish.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Italian", Short = "it", Speech = true, FlagImagePath = "/Images/Flags/200px-Italian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Japanese", Short = "ja", Speech = true, FlagImagePath = "/Images/Flags/200px-Japanese.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Kannada *", Short = "kn", Speech = false, FlagImagePath = "/Images/Flags/200px-Kannada.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Korean", Short = "ko", Speech = true, FlagImagePath = "/Images/Flags/200px-Korean.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Latin *", Short = "la", Speech = false, FlagImagePath = "/Images/Flags/200px-Latin.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Latvian *", Short = "lv", Speech = false, FlagImagePath = "/Images/Flags/200px-Latvian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Lithuanian", Short = "lt", Speech = true, FlagImagePath = "/Images/Flags/200px-Lithuanian.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Macedonian *", Short = "mk", Speech = false, FlagImagePath = "/Images/Flags/200px-Macedonian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Malay *", Short = "ms", Speech = false, FlagImagePath = "/Images/Flags/200px-Malay.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Maltese *", Short = "mt", Speech = false, FlagImagePath = "/Images/Flags/200px-Maltese.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Norwegian", Short = "no", Speech = true, FlagImagePath = "/Images/Flags/200px-Norwegian.png", isBing = true, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Persian *", Short = "fa", Speech = false, FlagImagePath = "/Images/Flags/200px-Persian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Polish", Short = "pl", Speech = true, FlagImagePath = "/Images/Flags/200px-Polish.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Portuguese", Short = "pt", Speech = true, FlagImagePath = "/Images/Flags/200px-Portuguese.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Romanian *", Short = "ro", Speech = false, FlagImagePath = "/Images/Flags/200px-Romanian.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Russian", Short = "ru", Speech = true, FlagImagePath = "/Images/Flags/200px-Russian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Serbian *", Short = "sr", Speech = false, FlagImagePath = "/Images/Flags/200px-Serbian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Slovak *", Short = "sk", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovak.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Slovenian *", Short = "sl", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovenian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Spanish", Short = "es", Speech = true, FlagImagePath = "/Images/Flags/200px-Spanish.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Swahili *", Short = "sw", Speech = false, FlagImagePath = "/Images/Flags/200px-Swahili.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Swedish", Short = "sv", Speech = true, FlagImagePath = "/Images/Flags/200px-Swedish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Tamil *", Short = "ta", Speech = false, FlagImagePath = "/Images/Flags/200px-Tamil.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Telugu *", Short = "te", Speech = false, FlagImagePath = "/Images/Flags/200px-Telugu.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Thai *", Short = "th", Speech = false, FlagImagePath = "/Images/Flags/200px-Thai.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Turkish *", Short = "tr", Speech = false, FlagImagePath = "/Images/Flags/200px-Turkish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Ukrainian *", Short = "uk", Speech = false, FlagImagePath = "/Images/Flags/200px-Ukrainian.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Urdu *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Urdu.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Vietnamese *", Short = "vi", Speech = false, FlagImagePath = "/Images/Flags/200px-Vietnamese.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Welsh *", Short = "cy", Speech = false, FlagImagePath = "/Images/Flags/200px-Welsh.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Yiddish *", Short = "yi", Speech = false, FlagImagePath = "/Images/Flags/200px-Yiddish.png", isBing = false, isBothAvailable = false });
            //</ArrayOfstring>

            #endregion
            #endregion

            #region UI Initalization
            App.IsTranslateTextEnabled = false;
            Debug.WriteLine("IsTranslateTextEnbaled = false");
            IsSelectionChangedEnabled = false; // work around for saving selected item
            From_Lang.ItemsSource = mFrom_Languages; // load dictionary to listpicker
            To_Lang.ItemsSource = mTo_Languages; // load dictionary to listpicker
            From_Lang.SelectedIndex = 15;
            To_Lang.SelectedIndex = 34;
            IsSelectionChangedEnabled = true; // work around for saving selected item
            SettingsRestore();

            #endregion

        }
        #endregion

    //    bool IsTranslateTextEnabled = false;

        #region UI

        //static method to check if the phone is in darktheme mode
        public bool IsDarkTheme()
        {
            bool isDarkTheme;
            isDarkTheme = (Visibility.Visible == (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"]);
            return isDarkTheme;
        }

        #endregion

        #region Nuance setup

        ~MainPage()
        {
            //_speechKit.release();

            App.CancelSpeechKit -= new CancelSpeechKitEventHandler(App_CancelSpeechKit);
        }

        private bool speechkitInitialize()
        {
            try
            {
                //    Nuance.AppInfo.SpeechKitServer = textBoxServerIp.Text;
                //    Nuance.AppInfo.SpeechKitPort = Convert.ToInt32(textBoxServerPort.Text);
            }
            catch (FormatException e)
            {
                MessageBox.Show("Port must be a number");

                return false;
            }

            try
            {
                _speechKit = SpeechKit.initialize(Nuance.AppInfo.SpeechKitAppId, Nuance.AppInfo.SpeechKitServer, Nuance.AppInfo.SpeechKitPort, Nuance.AppInfo.SpeechKitSsl, Nuance.AppInfo.SpeechKitApplicationKey);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                return false;
            }

            _beep = _speechKit.defineAudioPrompt("beep.wav");
            _speechKit.setDefaultRecognizerPrompts(_beep, null, null, null);
            _speechKit.connect();
            Thread.Sleep(10); // to guarantee the time to load prompt resource

            return true;
        }

        void App_CancelSpeechKit()
        {
            Logger.info(this, "App_CancelSpeechKit()");
            if (_speechKit != null)
            {
                _speechKit.cancelCurrent();
                hidePopup();
            }
        }

        private void hidePopup()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //(((_popup.Child as Border).Child as Panel).Children[1] as PerformanceProgressBar).IsIndeterminate = false;
                performanceProgressBar1.IsIndeterminate = false;
                _popup.IsOpen = false;
                enableScreen(true);
                //LayoutRoot.IsHitTestVisible = true;
            });
        }

        private void enableScreen(bool enable)
        {
            // bug fix
            if (enable == null)
                enable = true;

            System.Windows.Media.Color color;

            if (enable == true)
            {
                color = Colors.White;
            }
            else
            {
                color = Colors.Gray;
            }

            this.IsEnabled = enable;
            adControl.IsEnabled = enable;
            // menu bar
            if (ApplicationBar.Buttons.Count > 0)
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = enable;
                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = enable;
                (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = enable;
                (ApplicationBar.Buttons[3] as ApplicationBarIconButton).IsEnabled = enable;
            }
            this.ApplicationBar.IsMenuEnabled = enable;
            this.pivotControl.IsEnabled = enable;
            //           (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).IsEnabled = false;

            //ApplicationTitle.Foreground = new SolidColorBrush(color);
            //PageTitle.Foreground = new SolidColorBrush(color);
            //textBlockTtsVoice.Foreground = new SolidColorBrush(color);
            //textBlockServerIp.Foreground = new SolidColorBrush(color);
            //textBlockServerPort.Foreground = new SolidColorBrush(color);
            //listBoxTtsVoice.Background = new SolidColorBrush(color);
            //listBoxTtsVoice.BorderBrush = new SolidColorBrush(color);

        }


        private void buttonDictate_Click(object sender, RoutedEventArgs e)
        {
            Logger.info(this, "buttonDictate_Click()");
            //if (AppInfo.SpeechKitServer != textBoxServerIp.Text || AppInfo.SpeechKitPort != Convert.ToInt32(textBoxServerPort.Text))
            {
                // AppInfo.SpeechKitServer = textBoxServerIp.Text;
                //_speechKit.release();
                if (speechkitInitialize() == false)
                {
                    return;
                }
            }
            txtbFrom.Text = string.Empty;
            dictationStart(RecognizerRecognizerType.Dictation);
        }

        private void dictationStart(string type)
        {
            Logger.info(this, "dictationStart() type: " + type);
            Thread thread = new Thread(() =>
            {
                //speechkitInitialize();
                _recognizer = _speechKit.createRecognizer(type, RecognizerEndOfSpeechDetection.Long, _oemconfig.defaultLanguage(), this, _handler);
                _recognizer.start();
                showPopup("Please wait");
            });
            thread.Start();
        }


        private void showPopup(string text)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Create some content to show in the popup. Typically you would 
                // create a user control.
                double width = Application.Current.RootVisual.RenderSize.Width;
                double height = Application.Current.RootVisual.RenderSize.Height;

                enableScreen(false);

                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.White);
                border.BorderThickness = new Thickness(3);
                border.Width = width - 150;

                StackPanel panel = new StackPanel();
                panel.Background = new SolidColorBrush(Colors.Black);

                TextBlock textblock = new TextBlock();
                textblock.Text = text;
                textblock.Foreground = new SolidColorBrush(Colors.White);
                textblock.Margin = new Thickness(3);
                textblock.FontSize = 20;
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                panel.Children.Add(textblock);

                //PerformanceProgressBar progressbar = new PerformanceProgressBar();
                performanceProgressBar1.IsIndeterminate = true;
                //panel.Children.Add(progressbar);

                Button button = new Button();
                button.Foreground = new SolidColorBrush(Colors.White);
                button.BorderBrush = new SolidColorBrush(Colors.White);

                Button button1 = new Button();
                button1.Foreground = new SolidColorBrush(Colors.White);
                button1.BorderBrush = new SolidColorBrush(Colors.White);

                Grid grid = new Grid();
                grid.Background = new SolidColorBrush(Colors.Black);

                switch (text)
                {
                    case "Please wait":
                    case "Processing Dictation":
                        button.Content = "Cancel";
                        button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        button.Width = 160;
                        button.Click += new RoutedEventHandler(dictationStop);
                        panel.Children.Add(button);
                        break;
                    case "Processing TTS":
                    case "Speaking":
                        button.Content = "Cancel";
                        button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        button.Width = 160;
                        button.Click += new RoutedEventHandler(speechStop);
                        panel.Children.Add(button);
                        break;
                    case "Listening":
                        button.Content = "Stop";
                        button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        button.Width = 160;
                        button.Click += new RoutedEventHandler(dictationStop);
                        button1.Content = "Cancel";
                        button1.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        button1.Width = 160;
                        button1.Click += new RoutedEventHandler(dictationStop);
                        grid.Children.Add(button);
                        grid.Children.Add(button1);
                        panel.Children.Add(grid);
                        break;
                    default:
                        break;
                }

                border.Child = panel;

                // Set the Child property of Popup to the border 
                // which contains a stackpanel, textblock and button.
                _popup.Child = border;

                // Set where the popup will show up on the screen.
                _popup.VerticalOffset = 500;// (height - border.Height) / 2;
                _popup.HorizontalOffset = (width - border.Width) / 2;

                // Open the popup.
                //LayoutRoot.IsHitTestVisible = false;
                _popup.IsOpen = true;
            });
        }

        void speechStop(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                if (_vocalizer != null)
                {
                    _vocalizer.cancel();
                    //_speechKit.release();
                }
                hidePopup();
            });
            thread.Start();
        }


        void dictationStop(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content as string;

            Thread thread = new Thread(() =>
            {
                switch (content)
                {
                    case "Stop":
                        _recognizer.stopRecording();
                        showPopup("Processing Dictation");
                        break;
                    case "Cancel":
                        if (_recognizer != null)
                        {
                            _recognizer.cancel();
                        }
                        hidePopup();
                        break;
                    default:
                        break;
                }
            });
            thread.Start();
        }


        public void onRecordingBegin(Recognizer recognizer)
        {
            Logger.info(this, "onRecordingBegin()");
            showPopup("Listening");
        }

        public void onRecordingDone(Recognizer recognizer)
        {
            Logger.info(this, "onRecordingDone()");
            //_recognizer.stopRecording();
            showPopup("Processing Dictation");
        }

        public void onResults(Recognizer recognizer, Recognition results)
        {
            Logger.info(this, "onResults()");

            // for debugging purpose: logging the speechkit session id
            Logger.info(this, "session id: [" + _speechKit.getSessionId() + "]");

            hidePopup();
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                txtbFrom.Text = results.getResult(0).getText();
            });
            _recognizer = null;
            _recognizer.cancel();
            //_speechKit.release();
        }

        public void onError(Recognizer recognizer, SpeechError error)
        {
            Logger.info(this, "onError()");

            // for debugging purpose: logging the speechkit session id
            Logger.info(this, "session id: [" + _speechKit.getSessionId() + "]");

            if (recognizer != _recognizer)
            {
                return;
            }
            hidePopup();
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(error.getErrorDetail());
            });
            _recognizer = null;
            _recognizer.cancel();
            //_speechKit.release();
        }

        public void onSpeakingBegin(Vocalizer vocalizer, string text, object context)
        {
            Logger.info(this, "onSpeakingBegin()");

            showPopup("Speaking");
        }

        public void onSpeakingDone(Vocalizer vocalizer, string text, SpeechError error, object context)
        {
            Logger.info(this, "onSpeakingDone()");

            // for debugging purpose: logging the speechkit session id
            Logger.info(this, "session id: [" + _speechKit.getSessionId() + "]");

            hidePopup();
            if (error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(error.getErrorDetail());
                });
            }
            _vocalizer.cancel();
            //_speechKit.release();
        }


        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logger.info(this, "PhoneApplicationPage_BackKeyPress()");
            if (_popup.IsOpen)
            {
                App_CancelSpeechKit();
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }


        #endregion


        #region Navigation
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
        //    Debug.WriteLine("IsTranslateTextEnabled = false");
       //     IsTranslateTextEnabled = false;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            SettingsRestore();
            bool isLightTheme = (Visibility.Visible == (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"]);
            if (isLightTheme)
            {
                Debug.WriteLine("WHITE");
                Uri uri = new Uri("/Icons/light/arrow_for_white.png", UriKind.RelativeOrAbsolute);
                BitmapImage bmp = new BitmapImage(uri);
                swap_icon.Source = bmp;
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            SettingsSave();
            base.OnNavigatedTo(e);
        }
        #endregion


        private void SettingsRestore()
        {
            try
            {
                // Get the settings for this application.
                isolatedStorage = IsolatedStorageSettings.ApplicationSettings;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }


            if (isolatedStorage.Contains("FromLanguageIndex"))
            {
                From_Lang.SelectedIndex = (int)isolatedStorage["FromLanguageIndex"];
            }

            if (isolatedStorage.Contains("ToLanguageIndex"))
            {
                To_Lang.SelectedIndex = (int)isolatedStorage["ToLanguageIndex"];
            }


            if (isolatedStorage.Contains("FromLanguageText"))
            {
                txtbFrom.Text = (string)isolatedStorage["FromLanguageText"];
            }

            if (isolatedStorage.Contains("ToLanguageText"))
            {
                txtbTo.Text = (string)isolatedStorage["ToLanguageText"];
            }
        }

        private void SettingsSave()
        {
            // Save changes to the database.
            App.ViewModel.SaveChangesToDB();

            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
            isolatedStorage["FromLanguageIndex"] = From_Lang.SelectedIndex;
            isolatedStorage["FromLanguageText"] = txtbFrom.Text;
            isolatedStorage["ToLanguageIndex"] = To_Lang.SelectedIndex;
            isolatedStorage["ToLanguageText"] = txtbTo.Text;
            isolatedStorage.Save();
        }




        private void btnSpeak_Click(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Debug.WriteLine("From language (full)=" + ((Languages)From_Lang.SelectedItem).Full);
            Debug.WriteLine("From language (short)=" + ((Languages)From_Lang.SelectedItem).Short);
            Debug.WriteLine("From language (speech)=" + ((Languages)From_Lang.SelectedItem).Speech.ToString());

            Debug.WriteLine("To language (full)=" + ((Languages)To_Lang.SelectedItem).Full);
            Debug.WriteLine("To language (short)=" + ((Languages)To_Lang.SelectedItem).Short);
            Debug.WriteLine("To language (speech)=" + ((Languages)To_Lang.SelectedItem).Speech.ToString());

            tts.SpeakLanguage = ((Languages)To_Lang.SelectedItem).Short;
            // NOTE Speak disable because of Azure marketplace
            //           tts.SpeakText(txtbTo.Text); // speak
        }



        private void Translate_Click(object sender, EventArgs e)
        {
            this.Focus();
            TranslateText();
        }



        #region Fix Translator Framework bug
        // translator framework bug work around
        private int translatePressCount = 0;
        private int frameworkTranslateCount = 0;
        #endregion



        #region Languages for Speech
        // Available languages for Speech
        //      - <ArrayOfstring xmlns="http://schemas.microsoft.com/2003/10/Serialization/Arrays" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ca" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ca-es" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "da" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "da-dk" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "de" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "de-de" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "en" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "en-au" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "en-ca" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "en-gb" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "en-in" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "en-us" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "es" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "es-es" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "es-mx" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "fi" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "fi-fi" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "fr" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "fr-ca" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "fr-fr" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "it" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "it-it" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ja" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ja-jp" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ko" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ko-kr" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "nb-no" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "nl" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "nl-nl" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "no" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "pl" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "pl-pl" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "pt" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "pt-br" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "pt-pt" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ru" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "ru-ru" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "sv" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "sv-se" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "zh-chs" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "zh-cht" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "zh-cn" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "zh-hk" });
        //mLanguages.Add(new Languages() { Full = "millimeters", Short = "zh-tw" });
        //</ArrayOfstring>
        #endregion


        private void ClearAll_Click(object sender, EventArgs e)
        {
            //  txtbFrom.Focus();
            txtbFrom.Text = "";
            txtbTo.Text = "";
        }

        private void Swap_Click(object sender, EventArgs e)
        {
            //   txtbFrom.Focus();
            int temp = From_Lang.SelectedIndex;
            From_Lang.SelectedIndex = To_Lang.SelectedIndex;
            To_Lang.SelectedIndex = temp;

            string temp_text = "";
            temp_text = txtbFrom.Text;
            txtbFrom.Text = txtbTo.Text;
            txtbTo.Text = temp_text;
        }


        private void Speak_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("From language (full)=" + ((Languages)From_Lang.SelectedItem).Full);
            Debug.WriteLine("From language (short)=" + ((Languages)From_Lang.SelectedItem).Short);
            Debug.WriteLine("From language (speech)=" + ((Languages)From_Lang.SelectedItem).Speech.ToString());

            Debug.WriteLine("To language (full)=" + ((Languages)To_Lang.SelectedItem).Full);
            Debug.WriteLine("To language (short)=" + ((Languages)To_Lang.SelectedItem).Short);
            Debug.WriteLine("To language (speech)=" + ((Languages)To_Lang.SelectedItem).Speech.ToString());

            tts.SpeakLanguage = ((Languages)To_Lang.SelectedItem).Short;
            // NOTE speak disabled because of Azure data
            //         tts.SpeakText(txtbTo.Text); // speak
        }


        private void SMS_Click(object sender, EventArgs e)
        {
            SmsComposeTask sms = new SmsComposeTask();
            sms.Body = txtbTo.Text;
            sms.Show();
        }

        private void Email_Click(object sender, EventArgs e)
        {
            EmailComposeTask email = new EmailComposeTask();
            email.Subject = "Translator";
            email.Body = txtbTo.Text;
            email.Show();
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.RelativeOrAbsolute));
        }



        private void Testing_Click(object sender, EventArgs e)
        {
            // historyListBox.Items.Add(new object());
            // DatabaseAddHistory();
            Debug.WriteLine("db count = " + App.ViewModel.AllTranslatorHistory.Count);
            //App.ViewModel.AllTranslatorHistory.First
        }

        private void HistoryPlayButton_Click(object sender, RoutedEventArgs e)
        {
            object item = ((Image)sender).DataContext;
            TranslatorHistory translatorHistory = (TranslatorHistory)item;
            historyListBox.SelectedItem = translatorHistory; // highlight the item
            Debug.WriteLine(translatorHistory.HistoryId);

            // NOTE SpeakText disable because of Azure Marketplace
            tts.SpeakLanguage = translatorHistory.ToLanguageCode;
        //    tts.SpeakText(translatorHistory.ToText); // speak
        }

        private void HistoryFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            var content = (Image)sender;
            object item = ((Image)sender).DataContext;
            TranslatorHistory translatorHistory = (TranslatorHistory)item;
            historyListBox.SelectedItem = translatorHistory; // highlight the item
            DatabaseController.DatabaseUpdateHistoryFavorite(translatorHistory);
            if (translatorHistory.IsFavorite == true)
            {
                if (IsDarkTheme())
                {
                    content.Source = new BitmapImage(new Uri("/Icons/dark/appbar.favs.rest.png", UriKind.Relative));
                }
                else
                {
                    content.Source = new BitmapImage(new Uri("/Icons/light/appbar.favs.rest.png", UriKind.Relative));
                }
            }
            else
            {
                if (IsDarkTheme())
                {
                    content.Source = new BitmapImage(new Uri("/Icons/dark/appbar.favs.add.rest.png", UriKind.Relative));
                }
                else
                {
                    content.Source = new BitmapImage(new Uri("/Icons/light/appbar.favs.add.rest.png", UriKind.Relative));
                }
            }

            historyListBox.ItemsSource = null;
            historyListBox.ItemsSource = App.ViewModel.AllTranslatorHistory;
        }


        private void HistoryDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Image;
            TranslatorHistory translatorHistoryForDelete = button.DataContext as TranslatorHistory;
            historyListBox.SelectedItem = translatorHistoryForDelete; // highlight the item

            MessageBoxResult result = MessageBox.Show("Do you want to delete this history?", "Confirm deletion", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                DatabaseController.DatabaseDeleteHistory(translatorHistoryForDelete);
            }
        }



        private void HistoryEditButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var tap = sender as Image;
            TranslatorHistory translatorHistoryForEdit = tap.DataContext as TranslatorHistory;
            historyListBox.SelectedItem = translatorHistoryForEdit; // highlight the item
            From_Lang.SelectedIndex = translatorHistoryForEdit.FromLanguageIndex;
            txtbFrom.Text = translatorHistoryForEdit.FromText;

            To_Lang.SelectedIndex = translatorHistoryForEdit.ToLanguageIndex;
            txtbTo.Text = translatorHistoryForEdit.ToText;
            pivotControl.SelectedIndex = 0;
            pivotTranslate.Focus();
        }


        private void HistoryFavoriteImage_Loaded(object sender, RoutedEventArgs e)
        {
            var image = sender as Image;
            TranslatorHistory translatorHistoryForImage = image.DataContext as TranslatorHistory;
            // change the favorite button image
            if (translatorHistoryForImage.IsFavorite == true)
            {
                if (IsDarkTheme())
                {
                    image.Source = new BitmapImage(new Uri("/Icons/dark/appbar.favs.rest.png", UriKind.Relative));
                }
                else
                {
                    image.Source = new BitmapImage(new Uri("/Icons/light/appbar.favs.rest.png", UriKind.Relative));
                }
            }
            else
            {
                if (IsDarkTheme())
                {
                    image.Source = new BitmapImage(new Uri("/Icons/dark/appbar.favs.add.rest.png", UriKind.Relative));
                }
                else
                {
                    image.Source = new BitmapImage(new Uri("/Icons/light/appbar.favs.add.rest.png", UriKind.Relative));
                }
            }
        }

        private void ClearHistory_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to remove all history not marked as favorite?", "Confirm deletion", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                for (int i = (historyListBox.Items.Count - 1); i >= 0; i--)
                {
                    if (((TranslatorHistory)historyListBox.Items[i]).IsFavorite == false)
                    {
                        // delete all history that is not a favorite
                        DatabaseController.DatabaseDeleteHistory((TranslatorHistory)historyListBox.Items[i]);
                    }
                }
            }
        }



        private void txtbFrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                TranslateText();
            }
        }

        private void txtbTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }


        #region Advertisement
        private void AdControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AdControlPanel.Visibility = Visibility.Collapsed;
            pivotControl.Margin = new Thickness(0);
            mTimer.Start();
        }

        void mTimer_Tick(object sender, EventArgs e)
        {
            AdControlPanel.Visibility = Visibility.Visible;
            pivotControl.Margin = new Thickness(0, 59, 0, 0);
        }
        #endregion


        private void StatusButton_Click(object sender, EventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = txtbTo.Text;
            shareStatusTask.Show();
        }

        private void txtbTo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbTo.SelectAll();
        }

        private void txtbFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbFrom.SelectAll();
        }




        Google.GoogleTranslate googleTranslate = new Google.GoogleTranslate();

        private void TranslateText()
        {
            Debug.WriteLine("TRANSLATE TEXT\nIsTranslateTextEnabled = " + App.IsTranslateTextEnabled);
            if (App.IsTranslateTextEnabled == true)
            {
                Languages mFrom = (Languages)From_Lang.SelectedItem;
                Languages mTo = (Languages)To_Lang.SelectedItem;
                if (txtbFrom != null && mFrom != null && mTo != null)
                {

//                     Uncomment once Azure Data translator is working.
                    if (mFrom.isBing == true && mTo.isBing == true)
                    {
                        //if (tts != null)
                        //{
                        //    translatePressCount++; // translator framework bug work around
                        //    tts.Translate(txtbFrom.Text, mFrom.Short, mTo.Short);
                        //    tts.TextTranslated += new EventHandler(setTranslatedToText);
                        //}
                        if (bingTranslator != null)
                        {
                            bingTranslator.Translate(txtbFrom.Text, mTo.Short, mFrom.Short);
                            bingTranslator.TranslationChanged += new EventHandler(setTranslatedToText);
                        }
                    } else{
                        googleTranslate.GetResult(txtbTo);
                        googleTranslate.Translate(mFrom.Short, txtbFrom.Text, mTo.Short);
                        InsertHistory();

                    }
                    //    // for now have an else case for each one
                    //else if (mFrom.isBing == false && mTo.isBing == false)
                    //{
                    //    googleTranslate.GetResult(txtbTo);
                    //    googleTranslate.Translate(mFrom.Short, txtbFrom.Text, mTo.Short);
                    //    InsertHistory();
                    //    //txtbTo.Text = translate.Result;
                    //}
                    //else if (mFrom.isBing == false && mTo.isBing == true && mTo.isBothAvailable == true)
                    //{
                    //    //Google.GoogleTranslate translate = new Google.GoogleTranslate();
                    //    googleTranslate.GetResult(txtbTo);
                    //    googleTranslate.Translate(mFrom.Short, txtbFrom.Text, mTo.Short);
                    //    InsertHistory();
                    //    //      txtbTo.Text = translate.Result;

                    //}
                    //else if (mFrom.isBing == true && mTo.isBing == false && mFrom.isBothAvailable == true)
                    //{
                    //    // Google.GoogleTranslate translate = new Google.GoogleTranslate();
                    //    googleTranslate.GetResult(txtbTo);
                    //    googleTranslate.Translate(mFrom.Short, txtbFrom.Text, mTo.Short);
                    //    InsertHistory();
                    //    //       txtbTo.Text = translate.Result;
                    //}
                }
            }
        }

        void setTranslatedToText(object sender, EventArgs e)
        {
            //     frameworkTranslateCount++; // translator framework bug work around
            //txtbTo.Text = tts.TranslatedText;
            Dispatcher.BeginInvoke( ()=> {           
                txtbTo.Text = bingTranslator.TranslatedText; // cross-thread a
            });

            string translation =  bingTranslator.TranslatedText;
            //txtbTo.Text = translation;
            //Debug.WriteLine("translatePressCount = " + translatePressCount);
            //Debug.WriteLine("frameworkTranslateCount = " + frameworkTranslateCount);
            // STRANGE BUG!
            // maybe good place to set adding to database
            //if (frameworkTranslateCount == translatePressCount) // translator framework bug work around
            //{
            //    DatabaseController.DatabaseAddHistory(From_Lang.SelectedIndex, ((Languages)From_Lang.SelectedItem).Full, ((Languages)From_Lang.SelectedItem).Short, txtbFrom.Text, To_Lang.SelectedIndex, ((Languages)To_Lang.SelectedItem).Full, ((Languages)To_Lang.SelectedItem).Short, txtbTo.Text);
            //    frameworkTranslateCount = 0;
            //}
            Dispatcher.BeginInvoke(() =>
            {
                InsertHistory();
            });
        }

        void  InsertHistory(){
            DatabaseController.DatabaseAddHistory(From_Lang.SelectedIndex, ((Languages)From_Lang.SelectedItem).Full, ((Languages)From_Lang.SelectedItem).Short, txtbFrom.Text, To_Lang.SelectedIndex, ((Languages)To_Lang.SelectedItem).Full, ((Languages)To_Lang.SelectedItem).Short, txtbTo.Text);
        }


        #region Language List Picker Events
        private void From_Lang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isolatedStorage = IsolatedStorageSettings.ApplicationSettings;
            if (IsSelectionChangedEnabled == true)
            {
                isolatedStorage["FromLanguageIndex"] = (sender as ListPicker).SelectedIndex;
                isolatedStorage.Save();
            }

            // check if Voice to text will be enabled
            Languages language = ((Languages)(sender as ListPicker).SelectedItem);
            if (language.Short == "en")
            {
                Debug.WriteLine("english selected enable voice to text");
                if (ApplicationBar.Buttons.Count > 0)
                  (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;

            }
            else
            {
                // disable Voice to text for all other languages
                if (ApplicationBar.Buttons.Count > 0)
                     (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
            }
            //TranslatePriorFromLanguageText();
            //StorePriorSelecteLanguage((Languages)From_Lang.SelectedItem);
            // use the old language when translating
            //       TranslatePriorFromLanguageText(); // Translate text automatically when user changes language
        }

        private void To_Lang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isolatedStorage = IsolatedStorageSettings.ApplicationSettings;
            if (IsSelectionChangedEnabled == true)
            {
                isolatedStorage["ToLanguageIndex"] = (sender as ListPicker).SelectedIndex;
                isolatedStorage.Save();
            }

            // check if text to speech will be enabled
            Languages language = ((Languages)(sender as ListPicker).SelectedItem);
            if (language.Speech == true)
            {
                Debug.WriteLine("Text to speech button enabled");
                if (ApplicationBar.Buttons.Count > 0)
                   (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = false; // NOTE Azure data doesnt have speech, set to true once fixed.

            }
            else
            {
                // disable Voice to text for all other languages
                if (ApplicationBar.Buttons.Count > 0)
                 (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = false;
            }


            TranslateText(); // Translate text automatically when user changes language

        }
        #endregion
        bool IsPriorLanguageStored = false;

        private void dicateAppBar_Click(object sender, EventArgs e)
        {
            Logger.info(this, "dicateAppBar_Click()");
            //if (AppInfo.SpeechKitServer != textBoxServerIp.Text || AppInfo.SpeechKitPort != Convert.ToInt32(textBoxServerPort.Text))
            // {
            // AppInfo.SpeechKitServer = textBoxServerIp.Text;
            //_speechKit.release();
            if (speechkitInitialize() == false)
            {
                return;
            }
            //}
            txtbFrom.Text = string.Empty;
            dictationStart(RecognizerRecognizerType.Dictation);
        }


        #region UI initalization
        private void HistoryPlayButton_Loaded(object sender, RoutedEventArgs e)
        {
            var image = sender as Image;
            if (IsDarkTheme())
            {
                // do nothing setup for dark
                // image.Source = new BitmapImage(new Uri("/Icons/dark/appbar.sound.3.png", UriKind.Relative));
            }
            else
            {
                image.Source = new BitmapImage(new Uri("/Icons/light/appbar.sound.3.png", UriKind.Relative));
            }
        }

        private void HistoryEditButton_Loaded(object sender, RoutedEventArgs e)
        {
            var image = sender as Image;
            if (IsDarkTheme())
            {
                // do nothing setup for dark
                // image.Source = new BitmapImage(new Uri("/Icons/dark/appbar.edit.rest.png", UriKind.Relative));
            }
            else
            {
                image.Source = new BitmapImage(new Uri("/Icons/light/appbar.edit.rest.png", UriKind.Relative));
            }
        }

        private void HistoryDeleteButton_Loaded(object sender, RoutedEventArgs e)
        {
            var image = sender as Image;
            if (IsDarkTheme())
            {
                // do nothing setup for dark
                // image.Source = new BitmapImage(new Uri("/Icons/dark/appbar.delete.rest.png", UriKind.Relative));
            }
            else
            {
                image.Source = new BitmapImage(new Uri("/Icons/light/appbar.delete.rest.png", UriKind.Relative));
            }
        }
        #endregion


        /// <summary>
        /// Used to help with diction
        /// </summary>
        private void DisableUI()
        {
            this.pivotControl.IsEnabled = false;
        }

        private void pivotControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                //translate
                case 0:
                    ApplicationBar = ((ApplicationBar)this.Resources["TRANSLATE"]);
                    Debug.WriteLine("pivot 0");
                    break;
                // history
                case 1:
                    ApplicationBar = ((ApplicationBar)this.Resources["HISTORY"]);
                    Debug.WriteLine("pivot 1");
                    break;
                //starred
                case 2:
                    ApplicationBar = ((ApplicationBar)this.Resources["STARRED"]);
                    Debug.WriteLine("pivot 2");
                    break;
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Google.GoogleTranslate translate = new Google.GoogleTranslate();
            translate.Translate("en", "Hello world", "es");
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Used to workaround saving a prior translation. 
            Debug.WriteLine("PAGE LOADED");
            App.IsTranslateTextEnabled = true;
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs args)
        {
           Debug.WriteLine("ORIENTATION: "+args.Orientation.ToString());
           if (args.Orientation == PageOrientation.LandscapeLeft || args.Orientation == PageOrientation.LandscapeRight)
           {
               From_Lang.Width = 250;
               To_Lang.Width = 250;
           }
           else
           {
               From_Lang.Width = 175;
               To_Lang.Width = 175;
           }
            base.OnOrientationChanged(args);
        }

    } // End of Class
} // End of Namespace