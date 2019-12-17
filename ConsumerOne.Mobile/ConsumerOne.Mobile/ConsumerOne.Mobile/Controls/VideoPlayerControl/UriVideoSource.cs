using Xamarin.Forms;

namespace ConsumerOne.Mobile.Controls.VideoPlayerControl
{
    public class UriVideoSource : VideoSource
    {
        public static readonly BindableProperty UriProperty =
            BindableProperty.Create(nameof(Uri), typeof(string), typeof(UriVideoSource));

        public string Uri
        {
            set => SetValue(UriProperty, value);
            get => (string)GetValue(UriProperty);
        }
    }
}
