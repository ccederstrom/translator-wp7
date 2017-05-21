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

namespace WatermarkedTextBoxControl
{
  // see: http://www.windowsphonegeek.com/articles/WP7-WatermarkedTextBox-custom-control
	public class WatermarkedTextBox : TextBox
	{
		ContentControl WatermarkContent;
		public static readonly DependencyProperty WatermarkProperty =
	  DependencyProperty.Register("Watermark", typeof(object), typeof(WatermarkedTextBox), new PropertyMetadata(OnWatermarkPropertyChanged));

		public static readonly DependencyProperty WatermarkStyleProperty =
	  DependencyProperty.Register("WatermarkStyle", typeof(Style), typeof(WatermarkedTextBox), null);

		public Style WatermarkStyle
		{
			get { return base.GetValue(WatermarkStyleProperty) as Style; }
			set { base.SetValue(WatermarkStyleProperty, value); }
		}

		public object Watermark
		{
			get { return base.GetValue(WatermarkProperty) as object; }
			set { base.SetValue(WatermarkProperty, value); }
		}

		public WatermarkedTextBox()
		{
			DefaultStyleKey = typeof(WatermarkedTextBox);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.WatermarkContent = this.GetTemplateChild("watermarkContent") as ContentControl;
			if(WatermarkContent != null)
			{
			  DetermineWatermarkContentVisibility();
			}
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (WatermarkContent != null && string.IsNullOrEmpty(this.Text))
			{
				this.WatermarkContent.Visibility = Visibility.Collapsed;
			}
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (WatermarkContent != null && string.IsNullOrEmpty(this.Text))
			{
				this.WatermarkContent.Visibility = Visibility.Visible;
			}
			base.OnLostFocus(e);
		}

		private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			WatermarkedTextBox watermarkTextBox = sender as WatermarkedTextBox;
			if(watermarkTextBox != null && watermarkTextBox.WatermarkContent !=null)
			{
			  watermarkTextBox.DetermineWatermarkContentVisibility();
			}
		}

		private void DetermineWatermarkContentVisibility()
		{
			if (string.IsNullOrEmpty(this.Text))
			{
				this.WatermarkContent.Visibility = Visibility.Visible;
			}
			else
			{
				this.WatermarkContent.Visibility = Visibility.Collapsed;
			}
		}
	}
}
