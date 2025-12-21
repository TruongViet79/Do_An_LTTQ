using System.Configuration;
using System.Data;
using System.Windows;

namespace Do_An_LTTQ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string CurrentUsername { get; set; } = "Guest";
        public static string CurrentEmail { get; set; } = "";
    }

}
