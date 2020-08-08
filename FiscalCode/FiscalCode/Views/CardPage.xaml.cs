using System;
using System.IO;
using System.Threading.Tasks;

using FiscalCode.Utilities;

using FiscalCodeCalculator;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPage : ContentPage
    {
        SKImage image;
        readonly Person person;


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
            using (var canvas = new SKCanvas(bitmap))
            {
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
            }

            image = SKImage.FromBitmap(bitmap);

            imageControl.Source = (SKImageImageSource)image;

            bitmap.Dispose();
            stream.Dispose();
        }

        async void SaveToolbarItemClicked(object sender, EventArgs e)
        {
            if (await CheckIfWriteExternalStoragePermissionIsGrantedAsync())
            {
                var dateTime = DateTime.Now;
                var fileName = $"{dateTime.Year}{dateTime.Month}{dateTime.Day}_{dateTime.Hour}{dateTime.Minute}{dateTime.Second}.jpg";
                var skData = image.Encode(SKEncodedImageFormat.Jpeg, 100);

                if (await DependencyService.Get<IPhotoLibrary>().SavePhotoAsync(skData.ToArray(), string.Empty, fileName))
                    DependencyService.Get<IMessage>().ShortAlert(Localization.Locale.Localize("SaveMessageDescription"));
            }
        }

        async void ShareToolBarItemClicked(object sender, EventArgs e)
        {
            var dateTime = DateTime.Now;
            var fileName = $"{dateTime:yyyyMMdd_HHmmss}.jpg";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            var skData = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            var file = File.Create(filePath);
            skData.SaveTo(file);

            await Share.RequestAsync(new ShareFileRequest(new ShareFile(filePath)));
        }

        async Task<bool> CheckIfWriteExternalStoragePermissionIsGrantedAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return true;

            return false;
        }
    }
}
