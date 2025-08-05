using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CRMSystemZhukov.Models;
using CRMSystemZhukov.Data;
using System.Data.Entity;
using CRMSystemZhukov.Views;
using System.Windows;
using System.Collections.Generic;

namespace CRMSystemZhukov.ViewModels
{
    public class ClientsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(nameof(SelectedClient)); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                UpdateFilter();
            }
        }

        public ObservableCollection<Client> FilteredClients { get; set; } = new ObservableCollection<Client>();

        public ICommand AddClientCommand { get; }
        public ICommand EditClientCommand { get; }
        public ICommand DeleteClientCommand { get; }

        public ClientsViewModel()
        {
            LoadClients();
            AddClientCommand = new RelayCommand(AddClient);
            EditClientCommand = new RelayCommand(EditClient, () => SelectedClient != null);
            DeleteClientCommand = new RelayCommand(DeleteClient, () => SelectedClient != null);
        }

        private void LoadClients()
        {
            using (var db = new AppDbContext())
            {
                Clients.Clear();
                foreach (var client in db.Clients.ToList())
                    Clients.Add(client);
            }
            UpdateFilter();
        }

        private void AddClient()
        {
            var newClient = new Client { Name = "", CreatedAt = DateTime.Now };
            var window = new ClientEditWindow(newClient);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Clients.Add(newClient);
                    db.SaveChanges();
                }
                LoadClients();
            }
        }

        private void EditClient()
        {
            if (SelectedClient == null) return;
            // Копируем данные, чтобы не менять их до сохранения
            var editable = new Client
            {
                Id = SelectedClient.Id,
                Name = SelectedClient.Name,
                Email = SelectedClient.Email,
                Phone = SelectedClient.Phone,
                Birthdate = SelectedClient.Birthdate,
                Gender = SelectedClient.Gender,
                Address = SelectedClient.Address,
                Segment = SelectedClient.Segment,
                CreatedAt = SelectedClient.CreatedAt,
                UpdatedAt = SelectedClient.UpdatedAt
            };
            var window = new ClientEditWindow(editable);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    var client = db.Clients.Find(editable.Id);
                    if (client != null)
                    {
                        client.Name = editable.Name;
                        client.Email = editable.Email;
                        client.Phone = editable.Phone;
                        client.Birthdate = editable.Birthdate;
                        client.Gender = editable.Gender;
                        client.Address = editable.Address;
                        client.Segment = editable.Segment;
                        client.UpdatedAt = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                LoadClients();
            }
        }

        private void DeleteClient()
        {
            if (SelectedClient == null) return;
            using (var db = new AppDbContext())
            {
                db.Entry(SelectedClient).State = EntityState.Deleted;
                db.SaveChanges();
            }
            LoadClients();
        }

        private void UpdateFilter()
        {
            FilteredClients.Clear();
            IEnumerable<Client> filtered = Clients;
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lower = SearchText.ToLower();
                filtered = filtered.Where(c =>
                    (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(lower)) ||
                    (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(lower)) ||
                    (!string.IsNullOrEmpty(c.Phone) && c.Phone.ToLower().Contains(lower))
                );
            }
            foreach (var c in filtered)
                FilteredClients.Add(c);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // RelayCommand реализация для команд
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
    }
} 