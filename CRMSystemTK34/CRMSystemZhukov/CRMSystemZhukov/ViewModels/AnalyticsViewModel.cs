using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CRMSystemZhukov.Data;
using CRMSystemZhukov.Models;

namespace CRMSystemZhukov.ViewModels
{
    public class AnalyticsViewModel : INotifyPropertyChanged
    {
        public int ClientsCount { get; set; }
        public int DealsCount { get; set; }
        public int TasksCount { get; set; }
        public int CampaignsCount { get; set; }

        public ObservableCollection<SalesByMonth> SalesByMonths { get; set; } = new ObservableCollection<SalesByMonth>();
        public ObservableCollection<StatusCount> TasksByStatus { get; set; } = new ObservableCollection<StatusCount>();
        public ObservableCollection<SegmentCount> ClientsBySegment { get; set; } = new ObservableCollection<SegmentCount>();

        public AnalyticsViewModel()
        {
            using (var db = new AppDbContext())
            {
                ClientsCount = db.Clients.Count();
                DealsCount = db.Deals.Count();
                TasksCount = db.Tasks.Count();
                CampaignsCount = db.Campaigns.Count();

                var sales = db.Deals
                    .Where(d => d.CreatedAt != null)
                    .GroupBy(d => new { d.CreatedAt.Year, d.CreatedAt.Month })
                    .Select(g => new SalesByMonth
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Total = g.Sum(x => x.Amount)
                    }).ToList();
                SalesByMonths.Clear();
                foreach (var s in sales.OrderBy(x => x.Year).ThenBy(x => x.Month))
                    SalesByMonths.Add(s);

                var tasks = db.Tasks
                    .GroupBy(t => t.Status)
                    .Select(g => new StatusCount { Status = g.Key, Count = g.Count() })
                    .ToList();
                TasksByStatus.Clear();
                foreach (var t in tasks)
                    TasksByStatus.Add(t);

                var segments = db.Clients
                    .GroupBy(c => c.Segment)
                    .Select(g => new SegmentCount { Segment = g.Key, Count = g.Count() })
                    .ToList();
                ClientsBySegment.Clear();
                foreach (var s in segments)
                    ClientsBySegment.Add(s);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class SalesByMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Total { get; set; }
    }
    public class StatusCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
    public class SegmentCount
    {
        public string Segment { get; set; }
        public int Count { get; set; }
    }
} 