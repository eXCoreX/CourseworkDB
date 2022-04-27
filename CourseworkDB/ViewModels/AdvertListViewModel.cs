using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseworkDB.ViewModels
{
    internal class AdvertListViewModel: BaseViewModel
    {
        enum OrderParameterEnum
        {
            price,
            date
        }

        private ObservableCollection<Advert> adverts;

        public IEnumerable<Advert> Adverts
        {
            get
            {
                if (sortAscending)
                {
                    return orderParameter switch
                    {
                        OrderParameterEnum.price => adverts.OrderBy(a => a.Price),
                        _ => adverts.OrderBy(a => a.CreationDate),
                    };
                }
                else
                {
                    return orderParameter switch
                    {
                        OrderParameterEnum.price => adverts.OrderByDescending(a => a.Price),
                        _ => adverts.OrderByDescending(a => a.CreationDate),
                    };
                }
            }
        }

        public RelayCommand BackCommand { get; private set; }

        private OrderParameterEnum orderParameter = OrderParameterEnum.date;
        public string OrderParameter
        {
            get
            {
                return orderParameter switch
                {
                    OrderParameterEnum.price => "Price",
                    OrderParameterEnum.date => "Date",
                    _ => "Unknown",
                };
            }
            set
            {
                switch (value)
                {
                    case "Price":
                        orderParameter = OrderParameterEnum.price;
                        break;
                    case "Date":
                        orderParameter = OrderParameterEnum.date;
                        break;
                    default:
                        break;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(Adverts));
            }
        }

        private bool sortAscending = true;
        public bool SortAscending
        {
            get => sortAscending;
            set { sortAscending = value; OnPropertyChanged(); OnPropertyChanged(nameof(Adverts)); }
        }

        public AdvertListViewModel(MainViewModel mainViewModel, ObservableCollection<Advert> adverts)
        {
            this.adverts = adverts;

            BackCommand = mainViewModel.NavigateToSearch;
        }
    }
}
