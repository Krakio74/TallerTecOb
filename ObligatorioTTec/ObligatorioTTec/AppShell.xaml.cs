namespace ObligatorioTTec
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
        public async void PerfilUsuario(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPanel());
        }
    }
}
