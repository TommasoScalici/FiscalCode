using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Utilities
{
    [ContentProperty(nameof(Bindings))]
    public class MultiBinding : IMarkupExtension<Binding>
    {
        readonly InternalValue internalValue = new InternalValue();
        readonly IList<BindableProperty> properties = new List<BindableProperty>();
        BindableObject target;


        public IList<Binding> Bindings { get; } = new List<Binding>();
        public string StringFormat { get; set; }


        public Binding ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(StringFormat))
                throw new InvalidOperationException($"{nameof(MultiBinding)} requires a {nameof(StringFormat)}");

            var provideValueTarget = (IProvideValueTarget)serviceProvider?.GetService(typeof(IProvideValueTarget));
            target = provideValueTarget?.TargetObject as BindableObject;

            if (target == null)
                return null;

            foreach (var b in Bindings)
            {
                var property = BindableProperty.Create($"Property-{Guid.NewGuid().ToString("N")}", typeof(object),
                    typeof(MultiBinding), default, propertyChanged: (_, o, n) => SetValue());

                properties.Add(property);
                target.SetBinding(property, b);
            }

            SetValue();

            var binding = new Binding
            {
                Path = nameof(InternalValue.Value),
                Source = internalValue
            };

            return binding;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

        void SetValue()
        {
            if (target == null)
                return;

            var values = properties.Select(target.GetValue).ToArray();

            if (!string.IsNullOrWhiteSpace(StringFormat))
            {
                internalValue.Value = string.Format(StringFormat, values);
                return;
            }

            internalValue.Value = values;
        }


        sealed class InternalValue : INotifyPropertyChanged
        {
            object value;

            public event PropertyChangedEventHandler PropertyChanged;

            public object Value
            {
                get => value;
                set
                {
                    if (!Equals(this.value, value))
                    {
                        this.value = value;
                        OnPropertyChanged();
                    }
                }
            }

            void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
