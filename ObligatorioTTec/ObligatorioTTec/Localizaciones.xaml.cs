using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class Localizaciones : ContentPage
{
    public List<Sucursal> SucursalesGuardadas { get; set; } = new List<Sucursal>();
    public Localizaciones()
    {
        InitializeComponent();
        Navigation.PushAsync(new Views.Mapa(SucursalesGuardadas));
    }

}