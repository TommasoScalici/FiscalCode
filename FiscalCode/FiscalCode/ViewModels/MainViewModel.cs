using System.Collections.Generic;

using FiscalCode.ViewModels;
using FiscalCodeCalculator;
using Xamarin.Forms;

[assembly: Dependency(typeof(MainViewModel))]

namespace FiscalCode.ViewModels
{
    public class MainViewModel
    {
        public IList<Person> People { get; } = new List<Person>();
    }
}
