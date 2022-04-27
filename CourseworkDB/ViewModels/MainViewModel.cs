namespace CourseworkDB.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly MainContext mainContext = new();
        private BaseViewModel currentViewModel;

        private readonly SearchViewModel searchViewModel;

        public BaseViewModel CurrentViewModel 
        { 
            get => currentViewModel; 
            set { currentViewModel = value; OnPropertyChanged(); }
        }

        public RelayCommand NavigateToSearch { get; private set; }

        public MainViewModel()
        {
            searchViewModel = new SearchViewModel(this, mainContext);
            currentViewModel = searchViewModel;

            NavigateToSearch = new RelayCommand(x => CurrentViewModel = searchViewModel);
        }
    }
}

