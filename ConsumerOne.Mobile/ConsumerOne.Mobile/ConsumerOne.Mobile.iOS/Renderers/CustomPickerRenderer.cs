using System;
using System.ComponentModel;
using ConsumerOne.Mobile.iOS.Renderers.SuaveControls.MaterialForms.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace ConsumerOne.Mobile.iOS.Renderers
{
    
namespace SuaveControls.MaterialForms.iOS.Renderers
	{
		public class CustomPickerRenderer : PickerRenderer
		{
			public static void Init() { }
			protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				base.OnElementPropertyChanged(sender, e);
                if(Control!=null)
                {
                    Control.Layer.BorderWidth = 0;
                    Control.BorderStyle = UITextBorderStyle.None;
                }
				
			}
		}
	}
}
