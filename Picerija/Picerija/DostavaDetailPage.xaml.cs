using Picerija.Persistance;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Picerija
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DostavaDetailPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;

        public event EventHandler<Dostava> DostavaAdded;
        public event EventHandler<Dostava> DostavaUpdated;

        public DostavaDetailPage(Dostava dostava)
        {
            if (dostava == null)
            {
                throw new ArgumentNullException(nameof(dostava));
            }
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();

            BindingContext = new Dostava
            {
                Id = dostava.Id,
                Address = dostava.Address,
                Sum = dostava.Sum
            };
        }

        protected override void OnAppearing()
        {
            if (cenaDetail.Text == "0" && baksisDetail.Text == "0")
            {
                cenaDetail.Text = null;
                baksisDetail.Text = null;
            }
            base.OnAppearing();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var dostava = BindingContext as Dostava;

            if (string.IsNullOrWhiteSpace(dostava.DostavaDetail))
            {
                await DisplayAlert("Greska", "Unesite podatke za dostavu", "OK");
                return;
            }

            if (dostava.Id == 0)
            {
                await _connection.InsertAsync(dostava);
                DostavaAdded?.Invoke(this, dostava);
            }
            else
            {
                await _connection.UpdateAsync(dostava);
                DostavaUpdated?.Invoke(this, dostava);
            }

            await Navigation.PopAsync();
        }
    }
}