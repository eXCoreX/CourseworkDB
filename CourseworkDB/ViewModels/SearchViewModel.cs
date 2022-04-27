using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CourseworkDB.ViewModels
{
    internal class SearchViewModel: BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly MainContext _mainContext;

        public int Value { get; set; } = 100;

        public TrulyObservableCollection<Selection<Body>> Bodies { get; set; }
        public TrulyObservableCollection<Selection<FuelType>> FuelTypes { get; private set; }
        public TrulyObservableCollection<Selection<Transmission>> Transmissions { get; private set; }
        public TrulyObservableCollection<Selection<Color>> Colors { get; private set; }

        private IQueryable<Advert> adverts = Enumerable.Empty<Advert>().AsQueryable();
        public IQueryable<Advert> Adverts
        {
            get => adverts;
            set { adverts = value; OnPropertyChanged(); OnPropertyChanged(nameof(AdvertsCount)); }
        }

        public int AdvertsCount
        {
            get => adverts.Count();
        }

        public RelayCommand ShowCommand { get; private set; }
        public RelayCommand AddAdvertCommand { get; private set; }

        public SearchViewModel(MainViewModel mainViewModel, MainContext mainContext)
        {
            _mainViewModel = mainViewModel;
            _mainContext = mainContext;

            Bodies = new TrulyObservableCollection<Selection<Body>>(mainContext.Bodies.Select(x => new Selection<Body>(x, false)));
            FuelTypes = new TrulyObservableCollection<Selection<FuelType>>(mainContext.FuelTypes.Select(x => new Selection<FuelType>(x, false)));
            Transmissions = new TrulyObservableCollection<Selection<Transmission>>(mainContext.Transmissions.Select(x => new Selection<Transmission>(x, false)));
            Colors = new TrulyObservableCollection<Selection<Color>>(mainContext.Colors.Select(x => new Selection<Color>(x, false)));

            ShowCommand = new RelayCommand(x => ShowAdvertList());
            AddAdvertCommand = new RelayCommand(x => ShowAddAdvert());

            PropertyChanged += UpdateAdverts;

            Bodies.CollectionChanged += BodiesChanged;
            FuelTypes.CollectionChanged += BodiesChanged;
            Transmissions.CollectionChanged += BodiesChanged;
            Colors.CollectionChanged += BodiesChanged;

            UpdateAdverts(null, new PropertyChangedEventArgs(nameof(Bodies)));
        }

        private void ShowAddAdvert()
        {
            _mainViewModel.CurrentViewModel = new AddAdvertViewModel(_mainViewModel, _mainContext);
        }

        private void ShowAdvertList()
        {
            var adverts = from advert in Adverts
                          where advert != null && advert.Vehicle != null
                          select new
                          {
                              Color = advert.Vehicle.Color.Name,
                              Year = advert.Vehicle.ProductionYear,
                              Brand = advert.Vehicle.Model.Brand.Name,
                              Model = advert.Vehicle.Model.Name,
                              Price = advert.Price
                          };

            foreach (var advert in adverts)
            {
                Debug.WriteLine($"{advert.Color} {advert.Year} {advert.Brand} {advert.Model} for ${advert.Price}");
            }

            var observable = new ObservableCollection<Advert>(Adverts);
            if (observable != null)
                _mainViewModel.CurrentViewModel = new AdvertListViewModel(_mainViewModel, observable);
        }

        private void BodiesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateAdverts(sender, new PropertyChangedEventArgs(nameof(Bodies)));
        }

        private void UpdateAdverts(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Adverts) ||
                e.PropertyName == nameof(AdvertsCount))
            {
                return;
            }

            var selectedBodies = from body in Bodies
                                 where body.IsSelected
                                 select body.Item;

            var selectedFuelTypes = from fuelType in FuelTypes
                                    where fuelType.IsSelected
                                    select fuelType.Item;

            var selectedTransmissions = from transmission in Transmissions
                                        where transmission.IsSelected
                                        select transmission.Item;

            var selectedColors = from color in Colors
                                 where color.IsSelected
                                 select color.Item;

            _mainContext.Adverts
                .Include(advert => advert.User)
                .Include(advert => advert.Vehicle.Transmission)
                .Include(advert => advert.Vehicle.Color)
                .Include(advert => advert.Vehicle.FuelType)
                .Include(advert => advert.Vehicle.Body)
                .Include(advert => advert.Vehicle.Model.Brand)
                .Load();

            Adverts = (from advert in _mainContext.Adverts.Local
                      where !selectedBodies.Any() || selectedBodies.Contains(advert.Vehicle.Body)
                      where !selectedFuelTypes.Any() || selectedFuelTypes.Contains(advert.Vehicle.FuelType)
                      where !selectedTransmissions.Any() || selectedTransmissions.Contains(advert.Vehicle.Transmission)
                      where !selectedColors.Any() || selectedColors.Contains(advert.Vehicle.Color)
                      select advert).AsQueryable();
        }
    }
}
