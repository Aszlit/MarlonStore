using System.Configuration;
using System.Data;
using System.Security.Policy;
using System.Windows;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static class UserContext 
        {
            public static MainPage mainPage { get; set; }
        }

    }

    

}
