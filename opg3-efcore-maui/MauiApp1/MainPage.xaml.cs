using MauiApp1.Viewmodels;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
     
        public MainPage(BookViewModel bookViewModel)
        {
            InitializeComponent();

            this.BindingContext = bookViewModel;

        }
    }
}