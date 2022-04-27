using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseworkDB.ViewModels
{
    internal class Selection<T> : BaseViewModel
    {
        private T item;
        public T Item
        {
            get => item;
            set { item = value; OnPropertyChanged(); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set { isSelected = value; OnPropertyChanged(); }
        }

        public Selection(T item, bool isSelected = false)
        {
            this.item = item;
            this.isSelected = isSelected;
        }
    }
}
