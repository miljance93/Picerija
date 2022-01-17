using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Picerija
{
    public class Dostava : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string address;

        [MaxLength(255)]
        public string Address
        {
            get { return address; }
            set
            {
                if (address == value)
                {
                    return;
                }
                address = value;

                OnPropertyChanged();
            }
        }

        private double sum;

        public double Sum
        {
            get { return sum; }
            set
            {
                if (sum == value)
                {
                    return;
                }
                sum = value;

                OnPropertyChanged();
            }
        }

        public string DostavaDetail
        {
            get
            {
                return $"Adresa: {Address} Iznos: {Sum} RSD";                
            }
        }

        private double tip;

        public double Tip
        {
            get
            {
                return tip;
            }
            set
            {
                if (tip == value)
                {
                    return;
                }
                tip = value;

                OnPropertyChanged();
            }
        }

        public string TipDetail
        {
            get
            {
                return $"Baksis: {Tip}";
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
