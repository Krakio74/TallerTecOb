using ObligatorioTTec.Models;
using Microsoft.Maui.Controls.Compatibility;

namespace ObligatorioTTec
{
    public partial class App : Application
    {
        public static DBL CineDB { get; set; }

        public App(DBL dbl)
        {
            InitializeComponent();

            MainPage = new AppShell();
            CineDB = dbl;
        }

    }
}
