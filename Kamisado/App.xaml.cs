using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kamisado
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_Startup(object sender, StartupEventArgs e)
        {
            Bot bot = new Bot();

            GameState gs = new GameState();
            GamePlayViewModel gpvm = new GamePlayViewModel(gs, bot);
            GamePlayPage gpp = new GamePlayPage();
            gpp.DataContext = gpvm;

            MainWindow window = new MainWindow();
            window.Navigate(gpp);
            window.Show();
        }
    }
}
