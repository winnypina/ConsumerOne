using System;

namespace ConsumerOne.Mobile.Controls.VideoPlayerControl
{ 
    public interface IVideoPlayerController
    {
        VideoStatus Status { set; get; }

        TimeSpan Duration { set; get; }
    }
}
