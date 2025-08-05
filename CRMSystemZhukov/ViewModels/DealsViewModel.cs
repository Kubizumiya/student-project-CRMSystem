using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CRMSystemZhukov.Models;
using CRMSystemZhukov.Data;

namespace CRMSystemZhukov.ViewModels
{
    public class DealsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Deal> Deals { get; set; } = new ObservableCollection<Deal>();
        private Deal _selectedDeal;
        public Deal SelectedDeal
        {
            get => _selectedDeal;
            set { _selectedDeal = value; OnPropertyChanged(nameof(SelectedDeal)); }
        }

        public ICommand AddDealCommand { get; }
        public ICommand EditDealCommand { get; }
        public ICommand DeleteDealCommand { get; }

        public DealsViewModel()
        {
            LoadDeals();
            AddDealCommand = new RelayCommand(AddDeal);
            EditDealCommand = new RelayCommand(EditDeal, () => SelectedDeal != null);
            DeleteDealCommand = new RelayCommand(DeleteDeal, () => SelectedDeal != null);
        }

        private void LoadDeals()
        {
            using (var db = new AppDbContext())
            {
                Deals.Clear();
                foreach (var d in db.Deals.ToList())
                    Deals.Add(d);
            }
        }

        private void AddDeal()
        {
            var newDeal = new Deal { Name = "", Stage = "Лид", Status = "Открыта", CreatedAt = DateTime.Now };
            var window = new Views.DealEditWindow(newDeal);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Deals.Add(newDeal);
                    db.SaveChanges();
                }
                LoadDeals();
            }
        }

        private void EditDeal()
        {
            if (SelectedDeal == null) return;
            var editable = new Deal
            {
                Id = SelectedDeal.Id,
                ClientId = SelectedDeal.ClientId,
                Name = SelectedDeal.Name,
                Stage = SelectedDeal.Stage,
                Amount = SelectedDeal.Amount,
                Status = SelectedDeal.Status,
                CreatedAt = SelectedDeal.CreatedAt,
                ClosedAt = SelectedDeal.ClosedAt
            };
            var window = new Views.DealEditWindow(editable);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    var d = db.Deals.Find(editable.Id);
                    if (d != null)
                    {
                        d.ClientId = editable.ClientId;
                        d.Name = editable.Name;
                        d.Stage = editable.Stage;
                        d.Amount = editable.Amount;
                        d.Status = editable.Status;
                        d.ClosedAt = editable.ClosedAt;
                        db.SaveChanges();
                    }
                }
                LoadDeals();
            }
        }

        private void DeleteDeal()
        {
            if (SelectedDeal == null) return;
            using (var db = new AppDbContext())
            {
                var d = db.Deals.Find(SelectedDeal.Id);
                if (d != null)
                {
                    db.Deals.Remove(d);
                    db.SaveChanges();
                }
            }
            LoadDeals();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 