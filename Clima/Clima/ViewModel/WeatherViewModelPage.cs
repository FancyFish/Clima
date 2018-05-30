

namespace Clima.ViewModel
{
    using Clima.Model;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class WeatherViewModelPage:NotificableViewModel
    {
        #region atributos
        private string ubicacion;
        private string pais;
        private string resultTerm;
        private string region;
        private string ultimaActualizacion;
        private string clima;
        private string temperatura;
        private ImageSource imagen;
        #endregion
        #region Propiedades
        public string Ubicacion
        {
            get
            {
                return ubicacion;
            }
            set
            {
                SetValue(ref ubicacion, value);
            }
        }
        public string Pais
        {
            get
            {
                return pais;
            }
            set
            {
                SetValue(ref pais, value);
            }
        }
        public string ResultTerm
        {
            get
            {
                return resultTerm;
            }
            set
            {
                SetValue(ref resultTerm, value);
            }
        }
        public string Region
        {
            get
            {
                return region;
            }
            set
            {
                SetValue(ref region, value);
            }
        }
        public string UltimaActualizacion
        {
            get
            {
                return ultimaActualizacion;
            }
            set
            {
                SetValue(ref ultimaActualizacion, value);
            }
        }
        public string Clima
        {
            get
            {
                return clima;
            }
            set
            {
                SetValue(ref clima, value);
            }
        }
        public string Temperatura
        {
            get
            {
                return temperatura;
            }
            set
            {
                SetValue(ref temperatura, value);
            }
        }
        public ImageSource Image
        {
            get
            {
                return imagen;
            }
            set
            {
                SetValue(ref imagen, value);
            }
        }
        #endregion
        #region Comandos
        public ICommand BuscarCommand
        {
            get
            {
                return new RelayCommand(Buscar);
            }
        }

        private async void Buscar()
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri(obtenerUrl());
            var response = await cliente.GetAsync(cliente.BaseAddress);
            response.EnsureSuccessStatusCode();
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var weathermodel = Weather.FromJson(jsonResult);

            FijarValores(weathermodel);
        }

        private void FijarValores(Weather weathermodel)
        {
            Ubicacion = weathermodel.Query.Results.Channel.Location.City;
            Pais = weathermodel.Query.Results.Channel.Location.Country;
            Region= weathermodel.Query.Results.Channel.Location.Region;
            this.Temperatura= weathermodel.Query.Results.Channel.Units.Temperature;
            this.UltimaActualizacion = weathermodel.Query.Results.Channel.LastBuildDate;
            this.Clima = weathermodel.Query.Results.Channel.Description;
            var code= weathermodel.Query.Results.Channel.Item.Condition.Text;
            var imgLink = $"http://l.yimg.com/a/i/us/we/52/26{code}.gif";
            Image = ImageSource.FromUri(new Uri(imgLink));
        }

        public string obtenerUrl() {

            return $"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22{ResultTerm}%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
        }
        #endregion
        #region Constructores
        public WeatherViewModelPage()
        {

        }
        #endregion

    }
}
