namespace ObligatorioTTec
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void CatalogoBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Catalogo());
        }

        private void PerfilBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PerfilCliente());
        }

        private void LoginBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        private void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistrarCliente());
        }

        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}

        //private void OtraPag_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new NewPage1(Nombre.Text));
        //}
    }

}
