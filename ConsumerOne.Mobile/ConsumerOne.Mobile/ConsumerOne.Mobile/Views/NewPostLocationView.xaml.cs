using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsumerOne.Mobile.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace ConsumerOne.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false, HostViewModelType = typeof(NewPostViewModel))]
    public partial class NewPostLocationView
	{
        

		public NewPostLocationView ()
		{
			InitializeComponent ();
		    map.MyLocationEnabled = true;
		    map.UiSettings.MyLocationButtonEnabled = true;
            Appearing += NewPostLocationView_Appearing;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void NewPostLocationView_Appearing(object sender, EventArgs e)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            if (ViewModel.CurrentLocation != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(ViewModel.CurrentLocation.Target, Distance.FromMeters(100)), true);
            }
            
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentLocation")
            {
                Device.BeginInvokeOnMainThread(() => {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(ViewModel.CurrentLocation.Target,
          Distance.FromMeters(100)), true);
                });
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            using (var paint = new SKPaint())
            {
                // Create 300-pixel square centered rectangle
                var x = (info.Width - 300) / 2;
                var y = (info.Height - 300) / 2;
                var rect = new SKRect(0, 0, info.Width, info.Height);

                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, info.Height),
                    new SKPoint(info.Width, info.Height),
                    new[] { SKColor.Parse("#75c6ff"), SKColor.Parse("#718bf5") },
                    new float[] { 0, 1 },
                    SKShaderTileMode.Repeat);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
        }

        private void OnCanvasViewPaintSurface2(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            using (var paint = new SKPaint())
            {
                // Create 300-pixel square centered rectangle
                var x = (info.Width - 300) / 2;
                var y = (info.Height - 300) / 2;
                var rect = new SKRect(0, 0, info.Width, info.Height);
                var roundRect = new SKRoundRect(rect, 20, 20);
                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, info.Height),
                    new SKPoint(info.Width, info.Height),
                    new SKColor[] { SKColor.Parse("#718bf5"), SKColor.Parse("#75c6ff") },
                    new float[] { 0, 1 },
                    SKShaderTileMode.Repeat);

                // Draw the gradient on the rectangle
                canvas.DrawRoundRect(roundRect, paint);
            }
        }
    }
}