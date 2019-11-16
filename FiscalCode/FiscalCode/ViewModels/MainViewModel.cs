using System;
using System.Collections.Generic;
using System.IO;

using FiscalCode.Commands;
using FiscalCodeCalculator;

using Newtonsoft.Json;

using Plugin.Clipboard;
using Xamarin.Essentials;

namespace FiscalCode.ViewModels
{
    public class DeletionRequestedEventArgs : EventArgs
    {
        public bool DeletionConfirmed { get; set; }
    }


    public class MainViewModel
    {
        const string dataFileName = "data.json";
        static readonly string localAppDataPath = FileSystem.AppDataDirectory;


        public MainViewModel()
        {
            AddCommand = new RelayCommand(() => EditorViewModel = new EditorViewModel(this));
            CopyCommand = new RelayCommand(person => CrossClipboard.Current.SetText((person as Person)?.FiscalCode));
            DeleteCommand = new RelayCommand(() => DeletionRequested?.Invoke(this, EventArgs.Empty), () => SelectedItems.Count > 0);
            EditCommand = new RelayCommand(person => EditorViewModel = new EditorViewModel(this, person as Person));
            ShowCardCommand = new RelayCommand(person => EditorViewModel = new EditorViewModel(this, person as Person));
            LoadData();
        }


        public event EventHandler DeletionRequested;


        public RelayCommand AddCommand { get; }
        public RelayCommand CopyCommand { get; }
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
