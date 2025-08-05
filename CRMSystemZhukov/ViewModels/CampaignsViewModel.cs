using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CRMSystemZhukov.Models;
using CRMSystemZhukov.Data;

namespace CRMSystemZhukov.ViewModels
{
    public class CampaignsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Campaign> Campaigns { get; set; } = new ObservableCollection<Campaign>();
        private Campaign _selectedCampaign;
        public Campaign SelectedCampaign
        {
            get => _selectedCampaign;
            set { _selectedCampaign = value; OnPropertyChanged(nameof(SelectedCampaign)); }
        }

        public ICommand AddCampaignCommand { get; }
        public ICommand EditCampaignCommand { get; }
        public ICommand DeleteCampaignCommand { get; }

        public CampaignsViewModel()
        {
            LoadCampaigns();
            AddCampaignCommand = new RelayCommand(AddCampaign);
            EditCampaignCommand = new RelayCommand(EditCampaign, () => SelectedCampaign != null);
            DeleteCampaignCommand = new RelayCommand(DeleteCampaign, () => SelectedCampaign != null);
        }

        private void LoadCampaigns()
        {
            using (var db = new AppDbContext())
            {
                Campaigns.Clear();
                foreach (var c in db.Campaigns.ToList())
                    Campaigns.Add(c);
            }
        }

        private void AddCampaign()
        {
            var newCampaign = new Campaign { Name = "", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7), Status = "Черновик" };
            var window = new Views.CampaignEditWindow(newCampaign);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Campaigns.Add(newCampaign);
                    db.SaveChanges();
                }
                LoadCampaigns();
            }
        }

        private void EditCampaign()
        {
            if (SelectedCampaign == null) return;
            var editable = new Campaign
            {
                Id = SelectedCampaign.Id,
                Name = SelectedCampaign.Name,
                Type = SelectedCampaign.Type,
                StartDate = SelectedCampaign.StartDate,
                EndDate = SelectedCampaign.EndDate,
                Description = SelectedCampaign.Description,
                Status = SelectedCampaign.Status
            };
            var window = new Views.CampaignEditWindow(editable);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    var c = db.Campaigns.Find(editable.Id);
                    if (c != null)
                    {
                        c.Name = editable.Name;
                        c.Type = editable.Type;
                        c.StartDate = editable.StartDate;
                        c.EndDate = editable.EndDate;
                        c.Description = editable.Description;
                        c.Status = editable.Status;
                        db.SaveChanges();
                    }
                }
                LoadCampaigns();
            }
        }

        private void DeleteCampaign()
        {
            if (SelectedCampaign == null) return;
            using (var db = new AppDbContext())
            {
                var c = db.Campaigns.Find(SelectedCampaign.Id);
                if (c != null)
                {
                    db.Campaigns.Remove(c);
                    db.SaveChanges();
                }
            }
            LoadCampaigns();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 