using ConsumerOne.Mobile.ViewModels;

namespace ConsumerOne.Mobile.Popups
{
    public partial class PostOptionsPopup 
    {
        public PostOptionsPopup(IPostPopupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
