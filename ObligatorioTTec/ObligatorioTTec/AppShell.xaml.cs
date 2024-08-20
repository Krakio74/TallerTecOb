namespace ObligatorioTTec
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //<FlyoutItem Title="Locations">
            //    < ShellContent Title = "Localizaciones" ContentTemplate = "{DataTemplate local:Localizaciones}" />
            //</ FlyoutItem >

        }
        public async void PerfilUsuario(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPanel());
        }

    }
}
