using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CRMSystemZhukov.Data;
using CRMSystemZhukov.Models;

namespace CRMSystemZhukov.ViewModels
{
    public class AutomationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BirthdayReminder> Birthdays { get; set; } = new ObservableCollection<BirthdayReminder>();
        public ObservableCollection<OverdueTaskReminder> OverdueTasks { get; set; } = new ObservableCollection<OverdueTaskReminder>();

        public AutomationViewModel()
        {
            using (var db = new AppDbContext())
            {
                // Напоминания о днях рождения на ближайшие 7 дней
                var today = DateTime.Today;
                var week = today.AddDays(7);
                var birthdays = db.Clients
                    .Where(c => c.Birthdate != null)
                    .ToList()
                    .Where(c =>
                        c.Birthdate.Value.Month == today.Month && c.Birthdate.Value.Day >= today.Day && c.Birthdate.Value.Day <= week.Day
                        || (c.Birthdate.Value.Month == week.Month && c.Birthdate.Value.Day <= week.Day))
                    .Select(c => new BirthdayReminder { Name = c.Name, Birthdate = c.Birthdate.Value })
                    .OrderBy(c => c.Birthdate)
                    .ToList();
                Birthdays.Clear();
                foreach (var b in birthdays)
                    Birthdays.Add(b);

                // Просроченные задачи
                var overdue = db.Tasks
                    .Where(t => t.DueDate < today && t.Status != "Выполнена")
                    .Select(t => new OverdueTaskReminder { Description = t.Description, DueDate = t.DueDate, Status = t.Status })
                    .ToList();
                OverdueTasks.Clear();
                foreach (var t in overdue)
                    OverdueTasks.Add(t);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class BirthdayReminder
    {
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
    }
    public class OverdueTaskReminder
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }
} 