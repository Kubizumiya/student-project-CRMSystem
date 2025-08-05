using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CRMSystemZhukov.Models;
using CRMSystemZhukov.Data;

namespace CRMSystemZhukov.ViewModels
{
    public class TasksViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
        private Task _selectedTask;
        public Task SelectedTask
        {
            get => _selectedTask;
            set { _selectedTask = value; OnPropertyChanged(nameof(SelectedTask)); }
        }

        public int DealId { get; set; }

        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public TasksViewModel(int dealId)
        {
            DealId = dealId;
            LoadTasks();
            AddTaskCommand = new RelayCommand(AddTask);
            EditTaskCommand = new RelayCommand(EditTask, () => SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(DeleteTask, () => SelectedTask != null);
        }

        private void LoadTasks()
        {
            using (var db = new AppDbContext())
            {
                Tasks.Clear();
                foreach (var t in db.Tasks.Where(x => x.DealId == DealId).ToList())
                    Tasks.Add(t);
            }
        }

        private void AddTask()
        {
            var newTask = new Task { DealId = DealId, Description = "", DueDate = DateTime.Now, Status = "Открыта" };
            var window = new Views.TaskEditWindow(newTask);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    db.Tasks.Add(newTask);
                    db.SaveChanges();
                }
                LoadTasks();
            }
        }

        private void EditTask()
        {
            if (SelectedTask == null) return;
            var editable = new Task
            {
                Id = SelectedTask.Id,
                DealId = SelectedTask.DealId,
                Description = SelectedTask.Description,
                DueDate = SelectedTask.DueDate,
                Status = SelectedTask.Status
            };
            var window = new Views.TaskEditWindow(editable);
            if (window.ShowDialog() == true)
            {
                using (var db = new AppDbContext())
                {
                    var t = db.Tasks.Find(editable.Id);
                    if (t != null)
                    {
                        t.Description = editable.Description;
                        t.DueDate = editable.DueDate;
                        t.Status = editable.Status;
                        db.SaveChanges();
                    }
                }
                LoadTasks();
            }
        }

        private void DeleteTask()
        {
            if (SelectedTask == null) return;
            using (var db = new AppDbContext())
            {
                var t = db.Tasks.Find(SelectedTask.Id);
                if (t != null)
                {
                    db.Tasks.Remove(t);
                    db.SaveChanges();
                }
            }
            LoadTasks();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 