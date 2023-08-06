using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TerminosPage : ContentPage
	{
		public TerminosPage ()
		{
			InitializeComponent ();
		}

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}