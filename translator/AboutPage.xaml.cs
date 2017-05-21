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
using Microsoft.Phone.Tasks;
using Coding4Fun.Phone.Controls.Data;
using System.Windows.Media.Imaging;

namespace translator
{
    public partial class InfoPage : PhoneApplicationPage
    {
        MarketplaceReviewTask _marketplaceReview = new MarketplaceReviewTask();
        MarketplaceSearchTask _marketplaceSearch = new MarketplaceSearchTask();
        EmailComposeTask _emailComposeTask = new EmailComposeTask();

        public InfoPage()
        {
            InitializeComponent();
            #region Paid version Check
            if (App.isPaidVersion == true)
            {
                // disables ads
                adControl.Visibility = System.Windows.Visibility.Collapsed;
                //adControl.IsAutoRefreshEnabled = false;
                adControl.IsEnabled = false;
                adControl.IsAutoCollapseEnabled = false;
                //direction.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                // enable ads
                adControl.Visibility = System.Windows.Visibility.Visible;
                //adControl.IsAutoRefreshEnabled = true;
                adControl.IsEnabled = true;
                adControl.IsAutoCollapseEnabled = false;
            }
            #endregion

            #region Trial check
            //if (App.IsTrial == false)
            //{
            //    AdControlPanel.Visibility = System.Windows.Visibility.Collapsed;
            //}
            #endregion

            // http://www.windowsphonegeek.com/articles/Getting-data-out-of-WP7-WMAppManifest-is-easy-with-Coding4Fun-PhoneHelper
           
            txtAppName.Text = PhoneHelper.GetAppAttribute("Title") + " by " + PhoneHelper.GetAppAttribute("Author");
            txtVersion.Text = "version " + PhoneHelper.GetAppAttribute("Version");
            txtDescription.Text = PhoneHelper.GetAppAttribute("Description");



            if (IsDarkTheme())
            {
                NuanceLogo.Source = new BitmapImage(new Uri("/Images/Nuance/Nuancelogo_dark_144x72.png", UriKind.Relative));
            }
            else
            {
                NuanceLogo.Source = new BitmapImage(new Uri("/Images/Nuance/Nuancelogo_light_144x72.png", UriKind.Relative));
            }

        }

        private void btnMarketplace_Click(object sender, RoutedEventArgs e)
        {
            _marketplaceSearch.SearchTerms = "PNGC WP7";
            _marketplaceSearch.Show();
        }

        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            _marketplaceReview.Show();
        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            _emailComposeTask.To = "pngc.wp7@hotmail.com";
            _emailComposeTask.Subject = "Translator Feedback";
            _emailComposeTask.Show();
        }

        private void AdControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AdControlPanel.Visibility = Visibility.Collapsed;
           // mTimer.Start();
        }

        public bool IsDarkTheme()
        {
            bool isDarkTheme;
            isDarkTheme = (Visibility.Visible == (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"]);
            return isDarkTheme;
        }
    }
}