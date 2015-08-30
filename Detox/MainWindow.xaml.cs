using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup.Primitives;

using SharpTox;
using SharpTox.Core;

using Detox.ViewModel;

namespace Detox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ToxManager tox = new ToxManager("./data.tox");

        public MainWindow()
        {
            InitializeComponent();

            Friends.ItemsSource = tox.List;
            Profile.DataContext = tox.User;

            tox.Init();
        }
    }


}
