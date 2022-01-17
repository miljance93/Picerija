using Picerija.Persistance;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Picerija
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Dostava> _dostave;
        private SQLiteAsyncConnection _connection;
        private List<Dostava> dostava;

        public MainPage()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected async override void OnAppearing()
        {
            await LoadData();
            base.OnAppearing();
        }

        private async Task LoadData()
        {
            try
            {
                await _connection.CreateTableAsync<Dostava>();

                dostava = await _connection.Table<Dostava>().ToListAsync();

                _dostave = new ObservableCollection<Dostava>(dostava);
                listView.ItemsSource = _dostave;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {            
            var dostavaDetail = new DostavaDetailPage(new Dostava());

            dostavaDetail.DostavaAdded += (source, dostava) =>
            {
                _dostave.Add(dostava);
            };

            await Navigation.PushAsync(dostavaDetail);
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var dostava = (sender as MenuItem).CommandParameter as Dostava;

            if (await DisplayAlert("Upozorenje", $"Da li ste sigurni da zelite da obrisete { dostava.DostavaDetail }?", "Da", "Ne"))
            {
                _dostave.Remove(dostava);

                await _connection.DeleteAsync(dostava);
            }

        }

        private async void Delete_all_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Upozorenje", "Da li ste sigurni da zelite da obrisete celu listu?", "Da", "Ne"))
            {
                _dostave.Clear();
            }
            foreach (var item in dostava)
            {
                await _connection.DeleteAsync(item);
            }
        }

        private async void Sum_Clicked(object sender, EventArgs e)
        {
            double sum = 0;
            foreach (var item in _dostave)
            {
                sum += item.Sum;
            }
            await DisplayAlert("Ukupno", sum.ToString() + " RSD", "OK");
        }

        private int GetDostava
        {
            get
            {
                return _dostave.Count;
            }
        }

        private async void CountDostava_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Broj dostava", $"{ GetDostava }", "Ok");
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem == null)
            {
                return;
            }

            var selectedDostava = e.SelectedItem as Dostava;

            listView.SelectedItem = null;

            var dostavaPage = new DostavaDetailPage(selectedDostava);
            dostavaPage.DostavaUpdated += (source, dostava) =>
            {
                selectedDostava.Address = dostava.Address;
                selectedDostava.Id = dostava.Id;
                selectedDostava.Sum = dostava.Sum;
            };

            await Navigation.PushAsync(dostavaPage);
        }

        private async void Baksis_Clicked(object sender, EventArgs e)
        {
            double sum = 0;
            foreach (var item in _dostave)
            {
                sum += item.Tip;
            }

            await DisplayAlert("Baksis", sum.ToString() + " RSD", "OK");
        }
    }
}
