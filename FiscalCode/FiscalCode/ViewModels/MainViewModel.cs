using System.Collections.Generic;

using FiscalCode.DataModels;
using FiscalCode.ViewModels;
using Xamarin.Forms;

[assembly: Dependency(typeof(MainViewModel))]

namespace FiscalCode.ViewModels
{
    public class MainViewModel
    {
        public IList<Person> People { get; } = new List<Person>();
    }
}
