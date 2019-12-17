using System;
using System.ComponentModel;
using System.IO;
using Android.Content;
using Android.Media;
using Android.Widget;
using ConsumerOne.Mobile.Controls.VideoPlayerControl;
using ConsumerOne.Mobile.Droid.Renderers;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ARelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportRenderer(typeof(VideoPlayer),
                          typeof(VideoPlayerRenderer))]

namespace ConsumerOne.Mobile.Droid.Renderers
{

    public class VideoCompletedListener : Java.Lang.Object, MediaPlayer.IOnCompletionListener
    {
        private readonly VideoView _videoView;

        public VideoCompletedListener(VideoView videoView)
        {
            _videoView = videoView;
        }

        public void OnCompletion(MediaPlayer mp)
        {
            //mp.Looping = true;
            mp.SeekTo(1);
            _videoView.Start();
            _videoView.SetOnCompletionListener(new VideoCompletedListener(_videoView));

        }
    }

    public class VideoPreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
    {
        public void OnPrepared(MediaPlayer mp)
        {
            mp.Looping = true;
        }
    }

    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, ARelativeLayout>
    {
        VideoView videoView;
        MediaController mediaController;    // Used to display transport controls
        bool isPrepared;

        public VideoPlayerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    // Save the VideoView for future reference
                    videoView = new VideoView(Context);

                    // Put the VideoView in a RelativeLayout
                    ARelativeLayout relativeLayout = new ARelativeLayout(Context);
                    relativeLayout.AddView(videoView);

                    // Center the VideoView in the RelativeLayout
                    ARelativeLayout.LayoutParams layoutParams =
                        new ARelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                    layoutParams.AddRule(LayoutRules.AlignParentBottom);
                    layoutParams.AddRule(LayoutRules.AlignParentTop);
                    layoutParams.AddRule(LayoutRules.AlignParentLeft);
                    layoutParams.AddRule(LayoutRules.AlignParentRight);
                    videoView.LayoutParameters = layoutParams;
                    videoView.SetOnPreparedListener(new VideoPreparedListener());
                    videoView.SetOnCompletionListener(new VideoCompletedListener(videoView));
                    // Handle a VideoView event
                    videoView.Prepared += OnVideoViewPrepared;

                    SetNativeControl(relativeLayout);
                }

                SetAreTransportControlsEnabled();
                SetSource();

                args.NewElement.UpdateStatus += OnUpdateStatus;
                args.NewElement.PlayRequested += OnPlayRequested;
                args.NewElement.PauseRequested += OnPauseRequested;
                args.NewElement.StopRequested += OnStopRequested;
            }

            if (args.OldElement != null)
            {
                args.OldElement.UpdateStatus -= OnUpdateStatus;
                args.OldElement.PlayRequested -= OnPlayRequested;
                args.OldElement.PauseRequested -= OnPauseRequested;
                args.OldElement.StopRequested -= OnStopRequested;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null && videoView != null)
            {
                videoView.Prepared -= OnVideoViewPrepared;
            }
            if (Element != null)
            {
                Element.UpdateStatus -= OnUpdateStatus;
            }

            base.Dispose(disposing);
        }

        void OnVideoViewPrepared(object sender, EventArgs args)
        {
            isPrepared = true;
            ((IVideoPlayerController)Element).Duration = TimeSpan.FromMilliseconds(videoView.Duration);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == VideoPlayer.AreTransportControlsEnabledProperty.PropertyName)
            {
                SetAreTransportControlsEnabled();
            }
            else if (args.PropertyName == VideoPlayer.SourceProperty.PropertyName)
            {
                SetSource();
            }
            else if (args.PropertyName == VideoPlayer.PositionProperty.PropertyName)
            {
                if (Math.Abs(videoView.CurrentPosition - Element.Position.TotalMilliseconds) > 1000)
                {
                    videoView.SeekTo((int)Element.Position.TotalMilliseconds);
                }
            }
        }

        void SetAreTransportControlsEnabled()
        {
            if (Element.AreTransportControlsEnabled)
            {
                mediaController = new MediaController(Context);
                mediaController.SetMediaPlayer(videoView);
                videoView.SetMediaController(mediaController);
            }
            else
            {
                videoView.SetMediaController(null);

                if (mediaController != null)
                {
                    mediaController.SetMediaPlayer(null);
                    mediaController = null;
                }
            }
        }

        public static byte[] ReadFully(System.IO.Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        void SetSource()
        {
            isPrepared = false;
            bool hasSetSource = false;

            if (Element.Source is UriVideoSource)
            {
                string uri = (Element.Source as UriVideoSource).Uri;

                if (!String.IsNullOrWhiteSpace(uri))
                {
                    videoView.SetVideoURI(Android.Net.Uri.Parse(uri));
                    hasSetSource = true;
                }
            }
            else if (Element.Source is FileVideoSource)
            {
                string filename = (Element.Source as FileVideoSource).File;

                if (!String.IsNullOrWhiteSpace(filename))
                {
                    videoView.SetVideoPath(filename);
                    hasSetSource = true;
                }
            }
            if (Element.Source is StreamVideoSource)
            {
                var stream = ((StreamVideoSource) Element.Source).Stream;
                if (stream != null)
                {
                    var videoByteArray = ReadFully(stream);
                    Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                    Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/Temp");
                    dir.Mkdirs();
                    Java.IO.File file = new Java.IO.File(dir, RandomFilenameGenerator() + ".mp4");
                    if (!file.Exists())
                    {
                        file.CreateNewFile();
                        file.Mkdir();
                        System.IO.File.WriteAllBytes(file.Path, videoByteArray);
                    }
                    videoView.SetVideoPath(file.Path);
                    hasSetSource = true;
                }
            }
            else if (Element.Source is ResourceVideoSource)
            {
                string package = Context.PackageName;
                string path = (Element.Source as ResourceVideoSource).Path;

                if (!String.IsNullOrWhiteSpace(path))
                {
                    string filename = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
                    string uri = "android.resource://" + package + "/raw/" + filename;
                    videoView.SetVideoURI(Android.Net.Uri.Parse(uri));
                    hasSetSource = true;
                }
            }
              
            if (hasSetSource && Element.AutoPlay)
            {
                videoView.Start();
            }
        }

        private Random rnd = new Random();

        private string RandomFilenameGenerator()
        {
            return rnd.Next(0, 1000000).ToString();
        }

        // Event handler to update status
        void OnUpdateStatus(object sender, EventArgs args)
        {
            VideoStatus status = VideoStatus.NotReady;

            if (isPrepared)
            {
                status = videoView.IsPlaying ? VideoStatus.Playing : VideoStatus.Paused;
            }

            ((IVideoPlayerController)Element).Status = status;

            // Set Position property
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(videoView.CurrentPosition);
            ((IElementController)Element).SetValueFromRenderer(VideoPlayer.PositionProperty, timeSpan);
        }

        // Event handlers to implement methods
        void OnPlayRequested(object sender, EventArgs args)
        {
            videoView.Start();
        }

        void OnPauseRequested(object sender, EventArgs args)
        {
            videoView.Pause();
        }

        void OnStopRequested(object sender, EventArgs args)
        {
            videoView.StopPlayback();
        }
    }
}