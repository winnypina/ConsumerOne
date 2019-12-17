using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.Controls.VideoPlayerControl
{
    public class StreamVideoSource : VideoSource
    {
        public static readonly BindableProperty StreamProperty =
            BindableProperty.Create(nameof(Stream), typeof(Stream), typeof(StreamVideoSource));

        public Stream Stream
        {
            set => SetValue(StreamProperty, value);
            get => (Stream)GetValue(StreamProperty);
        }
    }
}
