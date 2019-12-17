using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConsumerOne.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomNavigationPage
	{
	    public CustomNavigationPage() 
	    {
	        InitializeComponent();
            BarBackgroundColor = Color.Transparent;
            BarTextColor = Color.White;
	    }

	    public CustomNavigationPage(Page root) : base(root)
	    {
	        InitializeComponent();
            BarBackgroundColor = Color.Transparent;
            BarTextColor = Color.White;
        }

	    public bool IgnoreLayoutChange { get; set; } = false;

	    protected override void OnSizeAllocated(double width, double height)
	    {
	        if (!IgnoreLayoutChange)
	            base.OnSizeAllocated(width, height);
	    }
    }
}