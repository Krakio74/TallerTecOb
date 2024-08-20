using ObligatorioTTec.Models;

namespace ObligatorioTTec.Views
{
    public partial class PinDetailsPage : ContentPage
    {
        private Location _location;
        private DBL _db;

        public PinDetailsPage(Location location)
        {
            InitializeComponent();
            _location = location;
            // Configura la base de datos (reemplaza con tu ruta de base de datos)
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "sucursales.db3");
            _db = new DBL(dbPath);
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            var nombre = NombreEntry.Text;
            var direccion = DireccionEntry.Text;
            var telefono = TelefonoEntry.Text;

            // Aseg�rate de que el stack de navegaci�n tenga m�s de 1 p�gina
            if (Navigation.NavigationStack.Count > 1)
            {
                var mapaPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as Mapa;
                if (mapaPage != null)
                {
                    var sucursal = new Sucursal
                    {
                        Nombre = nombre,
                        Direccion = direccion,
                        Telefono = telefono,
                        Ubicacion = $"{_location.Latitude}, {_location.Longitude}"
                    };

                    bool result = await _db.SetSucursal(sucursal);

                    if (result)
                    {
                        await DisplayAlert("�xito", "La sucursal se guard� correctamente.", "OK");
                        await Navigation.PopModalAsync(); // Volver a la p�gina anterior
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo guardar la sucursal.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo encontrar la p�gina de mapa.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No hay una p�gina de mapa en la pila de navegaci�n.", "OK");
            }
        }
    }
}


