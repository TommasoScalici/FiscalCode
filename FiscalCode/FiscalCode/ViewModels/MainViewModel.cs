using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FiscalCode.Commands;
using FiscalCodeCalculator;
using Newtonsoft.Json;

namespace FiscalCode.ViewModels
{
    public class DeletionRequestedEventArgs : EventArgs
    {
        public bool DeletionConfirmed { get; set; }
    }


    public class MainViewModel
    {
        static readonly string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static readonly string dataFileName = "data.json";


        public MainViewModel()
        {
            AddCommand = new RelayCommand(() => EditorViewModel = new EditorViewModel(this));
            DeleteCommand = new RelayCommand(() => DeletionRequested?.Invoke(this, EventArgs.Empty), () => SelectedItems.Count > 0);
            EditCommand = new RelayCommand(() => EditorViewModel = new EditorViewModel(this, SelectedItems.Single()),
                                           () => SelectedItems.Count == 1);
            ShowCardCommand = new RelayCommand(() => EditorViewModel = new EditorViewModel(this, SelectedItems.Single()),
                                               () => SelectedItems.Count == 1);
            LoadData();
        }


        public event EventHandler DeletionRequested;


        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand ShowCardCommand { get; }
        public EditorViewModel EditorViewModel { get; private set; }
        public IList<Person> People { get; private set; } = new List<Person>();
        public IList<Person> SelectedItems { get; set; } = new List<Person>();


        public void LoadData()
        {
            if (File.Exists(Path.Combine(localAppDataPath, dataFileName)))
            {
                var serializedData = File.ReadAllText(Path.Combine(localAppDataPath, dataFileName));
                People = JsonConvert.DeserializeObject<List<Person>>(serializedData);
            }
        }

        public void SaveData()
        {
            var serializedData = JsonConvert.SerializeObject(People);
            File.WriteAllText(Path.Combine(localAppDataPath, dataFileName), serializedData);
        }

        public void DeleteSelectedItems()
        {
            foreach (var item in SelectedItems)
                People.Remove(item);

            SaveData();
        }
    }
}
