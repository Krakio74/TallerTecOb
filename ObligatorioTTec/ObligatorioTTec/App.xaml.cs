using ObligatorioTTec.Models;

namespace ObligatorioTTec
{
    public partial class App : Application
    {
        public static DBL CineDB {  get; set; }

        public App(DBL dbl)
        {
            InitializeComponent();
            MainPage = new AppShell();
            CineDB = dbl;
        }

    }
}
