using System.Configuration;
using System.Data;
using System.Windows;

namespace StorageApp
{
    public partial class App : Application
    {
        public static DataContext Context { get; } = new DataContext();
    }

}
