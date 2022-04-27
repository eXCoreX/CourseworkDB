using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseworkDB.ViewModels
{
    internal class AddAdvertViewModel : BaseViewModel
    {
        private MainViewModel _mainViewModel;
        private MainContext _mainContext;

        #region Props
        private User? user;

        public User? User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }

        private Brand? brand;

        public Brand? Brand
        {
            get { return brand; }
            set
            {
                brand = value;
                OnPropertyChanged();
                Model = null;
                if (value == null)
                {
                    return;
                }
                AvailableModels = value.Models.ToList();
            }
        }

        private Model? model;

        public Model? Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged(); }
        }

        private Body? body;

        public Body? Body
        {
            get { return body; }
            set { body = value; OnPropertyChanged(); }
        }

        private FuelType? fuelType;

        public FuelType? FuelType
        {
            get { return fuelType; }
            set { fuelType = value; OnPropertyChanged(); }
        }

        private Transmission? transmission;

        public Transmission? Transmission
        {
            get { return transmission; }
            set { transmission = value; OnPropertyChanged(); }
        }

        private Color? color;

        public Color? Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(); }
        }

        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged(); }
        }

        private int mileage;

        public int Mileage
        {
            get { return mileage; }
            set { mileage = value; OnPropertyChanged(); }
        }

        private int productionYear;

        public int ProductionYear
        {
            get { return productionYear; }
            set { productionYear = value; OnPropertyChanged(); }
        }

        private float power;

        public float Power
        {
            get { return power; }
            set { power = value; OnPropertyChanged(); }
        }

        private float displacement;

        public float Displacement
        {
            get { return displacement; }
            set { displacement = value; OnPropertyChanged(); }
        }

        private string? description;
        private IList<Model> availableModels;

        public string? Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        #endregion

        public IList<User> AvailableUsers { get; private set; }
        public IList<Brand> AvailableBrands { get; private set; }
        public IList<Model> AvailableModels 
        { 
            get => availableModels; 
            private set { availableModels = value; OnPropertyChanged(); }
        }
        public IList<Body> AvailableBodies { get; private set; }
        public IList<FuelType> AvailableFuelTypes { get; private set; }
        public IList<Transmission> AvailableTransmissions { get; private set; }
        public IList<Color> AvailableColors { get; private set; }

        public RelayCommand AddAdvertCommand { get; private set; }
        public RelayCommand BackCommand { get; private set; }

        public AddAdvertViewModel(MainViewModel mainViewModel, MainContext mainContext)
        {
            _mainViewModel = mainViewModel;
            _mainContext = mainContext;

            AvailableUsers = mainContext.Users.ToList();
            mainContext.Brands.Include(b => b.Models).Load();
            AvailableBrands = mainContext.Brands.ToList();

            availableModels = new List<Model>();

            AvailableBodies = mainContext.Bodies.ToList();
            AvailableFuelTypes = mainContext.FuelTypes.ToList();
            AvailableTransmissions = mainContext.Transmissions.ToList();
            AvailableColors = mainContext.Colors.ToList();

            AddAdvertCommand = new RelayCommand(AddAdvert, CanAddAdvert);
            BackCommand = mainViewModel.NavigateToSearch;
        }

        private void AddAdvert(object? obj)
        {
            var advertToAdd = new Advert();
            var vehicleToAdd = new Vehicle();

            if (!CanAddAdvert(null))
            {
                return;
            }

            vehicleToAdd.Model = Model!;
            vehicleToAdd.Body = Body!;
            vehicleToAdd.FuelType = FuelType!;
            vehicleToAdd.Transmission = Transmission!;
            vehicleToAdd.Color = Color!;
            vehicleToAdd.ProductionYear = ProductionYear;
            vehicleToAdd.Displacement = Displacement;
            vehicleToAdd.Mileage = Mileage;
            vehicleToAdd.Power = Power;
            vehicleToAdd.Advert = advertToAdd;

            advertToAdd.User = User!;
            advertToAdd.Price = Price;
            advertToAdd.Description = Description;

            _mainContext.Add(advertToAdd);
            _mainContext.Add(vehicleToAdd);
            _mainContext.SaveChangesAsync();

            _mainViewModel.NavigateToSearch.Execute(null);
        }

        private bool CanAddAdvert(object? obj)
        {
            if (User == null ||
                Brand == null ||
                Model == null ||
                Body == null ||
                FuelType == null ||
                Transmission == null ||
                Color == null)
            {
                return false;
            }

            if (Model.Brand == null ||
                Model.Brand != Brand)
            {
                return false;
            }

            if (Displacement < 0 || Displacement > 20)
            {
                return false;
            }

            if (Power < 0)
            {
                return false;
            }

            if (Mileage < 0)
            {
                return false;
            }

            if (Price < 0)
            {
                return false;
            }

            if (ProductionYear < 1900 || ProductionYear > DateTime.Now.Year)
            {
                return false;
            }


            return true;
        }
    }
}
