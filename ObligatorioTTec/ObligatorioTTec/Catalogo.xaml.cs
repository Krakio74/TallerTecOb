namespace ObligatorioTTec;

public partial class Catalogo : ContentPage
{
    public Catalogo()
    {
        InitializeComponent();
    }

    private void BtnVolver_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}