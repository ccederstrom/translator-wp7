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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using translator.Database.Model;
using translator.Database.ViewModel;
using Coding4Fun.Phone.Controls.Data;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using Microsoft.Phone.Marketplace;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace translator
{
    public partial class App : Application
    {
        #region Nuance 
        public static event CancelSpeechKitEventHandler CancelSpeechKit;


        #endregion


        IsolatedStorageSettings _IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;

        // used for releasing paid and ad versions
        private static bool _isPaidVersion = false;
        public static bool isPaidVersion
        {
            get
            {
                return _isPaidVersion;
            }
        }


        public static bool IsTranslateTextEnabled = false;


        #region TRIAL SOFTWARE
        private static LicenseInformation _licenseInfo = new LicenseInformation();
        private static bool _isTrial = true;
        public static bool IsTrial
        {
            get
            {
                return _isTrial;
            }
        }


        /// <summary>
        /// Check the current license information for this application
        /// </summary>
        private void CheckLicense()
        {
            // When debugging, we want to simulate a trial mode experience. The following conditional allows us to set the _isTrial 
            // property to simulate trial mode being on or off. 
#if DEBUG
            string message = "This sample demonstrates the implementation of a trial mode in an application." +
                               "Press 'OK' to simulate trial mode. Press 'Cancel' to run the application in normal mode.";
            if (MessageBox.Show(message, "Debug Trial",
                 MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _isTrial = true;
            }
            else
            {
                _isTrial = false;
            }
#else
            _isTrial = _licenseInfo.IsTrial();
#endif
        }
        
        #endregion


        // The static ViewModel, to be used across the application.
        private static TranslatorViewModel viewModel;
        public static TranslatorViewModel ViewModel
        {
            get { return viewModel; }
        }




        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }




            // Check version of app
            bool IsVersionChanged = false;
            if (_IsolatedStorageSettings.Contains("AppCurrentVersion"))
            {
                if (_IsolatedStorageSettings["AppCurrentVersion"].Equals(PhoneHelper.GetAppAttribute("Version")))
                {
                    Debug.WriteLine("Version hasn't changed"); // do nothing
                }
                else
                {
                    // App Version has changed
                    _IsolatedStorageSettings.Remove("AppCurrentVersion");
                    _IsolatedStorageSettings.Add("AppCurrentVersion", PhoneHelper.GetAppAttribute("Version"));
                    Debug.WriteLine("New Version");
                    IsVersionChanged = true;
                    Debug.WriteLine("App Current Version = " + _IsolatedStorageSettings["AppCurrentVersion"]);
                }
            }
            else
            {
                // add the Version setting
                _IsolatedStorageSettings.Add("AppCurrentVersion", PhoneHelper.GetAppAttribute("Version"));
                Debug.WriteLine("App Current Version = " + _IsolatedStorageSettings["AppCurrentVersion"]);
            }
            _IsolatedStorageSettings.Save(); 




            // Specify the local database connection string.
            string DBConnectionString = "Data Source=isostore:/Translator.sdf";

            // Create the database if it does not exist.
            using (TranslatorDataContext db = new TranslatorDataContext(DBConnectionString))
            {
#if(DEBUG)
       //         Debug.WriteLine("Deleting database db");
         //       db.DeleteDatabase();
#endif
                if (db.DatabaseExists() == false)
                {
                    // Create the local database.
                    Debug.WriteLine("Creating database");
                    db.CreateDatabase();
                    // Save categories to the database.
                    db.SubmitChanges();
                }
                else if (db.DatabaseExists() == true && IsVersionChanged == true)
                {
                    // rebuild the database for new version of app
                    Debug.WriteLine("Rebuilding database ");

                    // dont need to rebuild for version 1.4
                  //  db.DeleteDatabase();
                  //  db.CreateDatabase();
                }
            }

            // Create the ViewModel object.
            viewModel = new TranslatorViewModel(DBConnectionString);

            // Query the local database and load observable collections.
            viewModel.LoadCollectionsFromDatabase();

            #region Nuance setup
            this.ApplicationLifetimeObjects.Add(new XNAAsyncDispatcher(TimeSpan.FromMilliseconds(50)));
            #endregion
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            MediaPlayer.Pause(); // Nuance
            CheckLicense();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            MediaPlayer.Pause(); // Nuance
            CheckLicense();
            Debug.WriteLine("IsTranslateTextEnabled = true");
            IsTranslateTextEnabled = true;
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            CancelSpeechKit(); // Nuance
            MediaPlayer.Resume(); // Nuance
            
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            MediaPlayer.Resume(); // Nuance
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }


    #region Nuance setup
    public class XNAAsyncDispatcher : IApplicationService
    {
        DispatcherTimer frameworkDispatcherTimer;

        public XNAAsyncDispatcher(TimeSpan dispatchInterval)
        {
            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.Tick += new EventHandler(frameworkDispatcherTimer_Tick);
            this.frameworkDispatcherTimer.Interval = dispatchInterval;
            FrameworkDispatcher.Update();
        }

        void IApplicationService.StartService(ApplicationServiceContext context) { this.frameworkDispatcherTimer.Start(); }
        void IApplicationService.StopService() { this.frameworkDispatcherTimer.Stop(); }
        void frameworkDispatcherTimer_Tick(object sender, EventArgs e) { FrameworkDispatcher.Update(); }
    }
    #endregion
}