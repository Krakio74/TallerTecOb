using ObligatorioTTec.Models;

namespace ObligatorioTTec.Views
{
    public partial class VerSucursales : ContentPage
    {
        private DBL _database;
        private Sucursal _sucursalSeleccionada;
        public List<Sucursal> SucursalesGuardadas { get; set; } = new List<Sucursal>();

        private Sucursal _selectedSucursal;

        public VerSucursales()
        {
            InitializeComponent();
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db3");
            _database = new DBL(databasePath);

            SucursalesCollectionView.BindingContext = this;

            CargarSucursales();
        }

        private async void CargarSucursales()
        {
            var sucursales = await _database.GetSucursales();
            SucursalesCollectionView.ItemsSource = sucursales;
        }

        public Command<Sucursal> SucursalTappedCommand => new Command<Sucursal>((sucursal) =>
        {
            _selectedSucursal = sucursal;
            EditButton.IsEnabled = _selectedSucursal != null;
            DeleteButton.IsEnabled = _selectedSucursal != null;
        });

        private void OnSucursalTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                _selectedSucursal = e.Item as Sucursal;
            }
        }

        private void OnSucursalSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSucursal = e.CurrentSelection.FirstOrDefault() as Sucursal;

            // Habilitar botones de Editar y Eliminar si hay una sucursal seleccionada
            EditButton.IsEnabled = _selectedSucursal != null;
            DeleteButton.IsEnabled = _selectedSucursal != null;
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (_selectedSucursal != null)
            {
                // Navega a la página de edición y pasa la sucursal seleccionada
                await Navigation.PushAsync(new EditarSucursalPage(_selectedSucursal));
            }
            else
            {
                await DisplayAlert("Error", "Selecciona una sucursal para editar.", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_selectedSucursal != null)
            {
                bool confirm = await DisplayAlert("Confirmar eliminación", "¿Está seguro de que desea eliminar esta sucursal?", "Sí", "No");

                if (confirm)
                {
                    await _database.BorrarSucursal(_selectedSucursal);
                    CargarSucursales(); // Recargar la lista después de eliminar
                    _selectedSucursal = null;
                }
            }
            else
            {
                await DisplayAlert("Error", "Selecciona una sucursal para eliminar.", "OK");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }


    }
}
