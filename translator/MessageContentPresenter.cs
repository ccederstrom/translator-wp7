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
  /// <summary>
  /// Content control that selects a template based on the Message.Side property
  /// </summary>
  public class MessageContentPresenter : ContentControl
  {
    /// <summary>
    /// The DataTemplate to use when Message.Side == Side.Me
    /// </summary>
    public DataTemplate MeTemplate { get; set; }

    /// <summary>
    /// The DataTemplate to use when Message.Side == Side.You
    /// </summary>
    public DataTemplate YouTemplate { get; set; }


    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);

      // apply the required template
      Message message = newContent as Message;
      if (message.Side == MessageSide.Me)
      {
        ContentTemplate = MeTemplate;
      }
      else if (message.Side == MessageSide.You)
      {
        ContentTemplate = YouTemplate;
      }
    }
  }
}
