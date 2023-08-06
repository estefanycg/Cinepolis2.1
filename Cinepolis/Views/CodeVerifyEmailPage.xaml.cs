using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CodeVerifyEmailPage : ContentPage
	{
		public CodeVerifyEmailPage ()
		{
			InitializeComponent ();
		}

        private async void VerificarButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string codigo = cod.Text;

                var data = new
                {
                    codigo
                };

                string jsonData = JsonConvert.SerializeObject(data);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "auth/verificacion");
                    request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string respuesta = await response.Content.ReadAsStringAsync();
                        dynamic responseObject = JsonConvert.DeserializeObject(respuesta);
                        string token = responseObject?.data?.token;
                        if (!string.IsNullOrEmpty(token))
                        {
                            Application.Current.Properties["token"] = token;
                            await Navigation.PushAsync(new Location());
                        }

                    }
                    else
                    {
                        await DisplayAlert("Proveer todos los datos", "Intente De Nuevo", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}