using Android.Content;
using ConsumerOne.Mobile.Controls;
using ConsumerOne.Mobile.Droid.Renderers;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]

namespace ConsumerOne.Mobile.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            Control.SetBackgroundColor(Color.Transparent);
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