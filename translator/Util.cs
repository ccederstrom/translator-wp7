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

namespace translator
{
  public static class Util
  {
    /// <summary>
    /// Shows the element, playing a storyboard if one is present
    /// </summary>
    /// <param name="element"></param>
    public static void Show(this FrameworkElement element, Action completedAction)
    {
      string animationName = element.Name + "ShowAnim";

      // check for presence of a show animation
      Storyboard showAnim = element.Resources[animationName] as Storyboard;
      if (showAnim != null)
      {
        showAnim.Begin();
        showAnim.Completed += (s, e) => completedAction();
      }
      else
      {
        element.Visibility = Visibility.Visible;
      }
    }

    /// <summary>
    /// Hides the element, playing a storyboard if one is present
    /// </summary>
    /// <param name="element"></param>
    public static void Hide(this FrameworkElement element)
    {
      string animationName = element.Name + "HideAnim";

      // check for presence of a hide animation
      Storyboard showAnim = element.Resources[animationName] as Storyboard;
      if (showAnim != null)
      {
        showAnim.Begin();
      }
      else
      {
        element.Visibility = Visibility.Collapsed;
      }
    }
  }
}
