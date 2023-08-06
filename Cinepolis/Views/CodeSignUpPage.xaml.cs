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
    public partial class CodeSignUpPage : ContentPage
    {
        public CodeSignUpPage()
        {
            InitializeComponent();
        }

        //Código de verificación de identidad, enviado al correo para completar el registro
        private async void AceptClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());

        }
    }
}