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
  
  public enum MessageSide
  {
    Me,
    You
  }

  /// <summary>
  /// An SMS message
  /// </summary>
  public class Message
  {
    private static DateTime _now = DateTime.Now;
    private static Random _rand = new Random();

    public Message()
    {
      Timestamp = _now;
      _now = _now.AddMinutes(_rand.Next(100));
    }

    public string Text { get; set; }

    public DateTime Timestamp { get; set; }

    public MessageSide Side { get; set; }
  }
}
