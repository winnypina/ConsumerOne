using System;
using System.ComponentModel;
using Android.Content;
using Android.Widget;
using ConsumerOne.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZXing.Net.Mobile.Forms;


[assembly: ExportRenderer(typeof(ZXingBarcodeImageView), typeof(CustomBarcodeRenderer))]
namespace ConsumerOne.Mobile.Droid.Renderers
{
	public class CustomBarcodeRenderer : ViewRenderer<ZXingBarcodeImageView, ImageView>
	{

        public CustomBarcodeRenderer(Context context) : base(context)
        {
            this.context = context;
        }

		public static void Init()
		{
			var temp = DateTime.Now;
		}

		ZXingBarcodeImageView formsView;
		ImageView imageView;
        private readonly Context context;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			regenerate();

			base.OnElementPropertyChanged(sender, e);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ZXingBarcodeImageView> e)
		{
			formsView = Element;

			if (imageView == null)
			{

				imageView = new ImageView(context);

				base.SetNativeControl(imageView);
			}

			regenerate();

			base.OnElementChanged(e);
		}

		void regenerate()
		{
            if (formsView != null && !string.IsNullOrWhiteSpace(formsView.BarcodeValue))
            {
                var writer = new ZXing.Mobile.BarcodeWriter();

				if (formsView != null && formsView.BarcodeOptions != null)
					writer.Options = formsView.BarcodeOptions;
				if (formsView != null)
					writer.Format = formsView.BarcodeFormat;

				var value = formsView != null ? formsView.BarcodeValue : string.Empty;

				Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
					var image = writer.Write(value);

					imageView.SetImageBitmap(image);
				});
			}
		}
	}
}
