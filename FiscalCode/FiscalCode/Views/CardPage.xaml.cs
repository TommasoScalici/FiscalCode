using System;
using System.Threading.Tasks;
using FiscalCode.Localization;
using FiscalCodeCalculator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Toasts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPage : ContentPage
    {
        SKImage image;
        Person person;


        public CardPage() => InitializeComponent();

        public CardPage(Person person)
            : this()
        {
            this.person = person;
            DrawTextOnCard();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "Orientation.ForceLandScape");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "Orientation.Unspecified");
        }

        void DrawTextOnCard()
        {
            var assembly = GetType().Assembly;
            var stream = assembly.GetManifestResourceStream("FiscalCode.Resources.TesseraSanitaria.jpg");

            if (stream == null)
                return;

            var bitmap = SKBitmap.Decode(stream);
            var canvas = new SKCanvas(bitmap);
            var font = SKTypeface.FromFamilyName("Roboto Mono");
            var brush = new SKPaint
            {
                Color = SKColors.Black,
                FakeBoldText = true,
                IsAntialias = true,
                Typeface = font,
                TextSize = Convert.ToInt64(Device.GetNamedSize(NamedSize.Default, typeof(Label))),
            };

            canvas.DrawText(person.FiscalCode, 90, 130, brush);
            canvas.DrawText(person.Surname, 90, 160, brush);
            canvas.DrawText(person.Name, 90, 190, brush);
            canvas.DrawText(person.BirthDistrict.Name, 90, 230, brush);
            canvas.DrawText(person.BirthDistrict.ProvinceAbbreviation, 90, 260, brush);
            canvas.DrawText(person.Birthdate.ToString("d"), 90, 290, brush);
            canvas.DrawText(person.Sex, 480, 190, brush);

            image = SKImage.FromBitmap(bitmap);

            imageControl.Source = (SKImageImageSource)image;

            bitmap.Dispose();
            stream.Dispose();
        }

        async void SaveToolbarItemClicked(object sender, EventArgs e)
        {
            if (await CheckIfWriteExternalStoragePermissionIsGranted())
            {
                MessagingCenter.Send(this, "SaveToGallery", image);

                var notificationOptions = new NotificationOptions
                {
                    Title = Locale.Localize("SaveMessageTitle"),
                    Description = Locale.Localize("SaveMessageDescription"),
                    IsClickable = false,
                };

                var notification = DependencyService.Get<IToastNotificator>();
                var result = await notification.Notify(notificationOptions);
            }
        }

        void ShareToolBarItemClicked(object sender, EventArgs e) => MessagingCenter.Send(this, "Share", image);

        async Task<bool> CheckIfWriteExternalStoragePermissionIsGranted()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    await DisplayAlert(Locale.Localize("PermissionTitle"), Locale.Localize("PermissionMessage"),
                                       Locale.Localize("Allow"), Locale.Localize("Deny"));

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
            }

            if (status == PermissionStatus.Granted)
                return true;

            return false;
        }
    }
}
