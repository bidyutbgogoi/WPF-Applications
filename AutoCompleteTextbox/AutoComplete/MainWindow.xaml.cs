using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
using System.Windows.Threading;

namespace AutoComplete
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IEnumerable<string> _employeeFromDb;
        public IObservable<string> AllEmployees { get { return _employeeFromDb.ToObservable(); } }

        //private IObservable<string> input;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();

            _employeeFromDb = new List<string>() { "Basu","Nikhil","Nandu","Bhola","Seema"
                ,"Sandy","Shakuni","Priya","Preetie"};
        }

        private T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                    child = GetVisualChild<T>(v);
                if (child != null)
                    break;
            }
            return child;
        }


        private void cmbAutoComplete_Loaded(object sender, RoutedEventArgs e)
        {
            //This is bind to listbox
            var names = ((MainWindowViewModel)(this.DataContext)).EnteredNames;
            names.Clear();

            TextBox textBox = GetVisualChild<TextBox>(cmbAutoComplete);

            //Get user input
            var input = Observable.FromEventPattern(textBox, "TextChanged")
                        .Select(evt => ((TextBox)evt.Sender).Text)
                        .Throttle(TimeSpan.FromMilliseconds(600)) //This is more efficient to search when user pauses or finish typing
                        .DistinctUntilChanged();


            //Looks for the entered user input over the list which is to be searched, in our case if could be that observable sequence of Takara information
            var res = input.Select(term => _employeeFromDb
                                                .Where(name => name.StartsWith(term)));


            res
             .ObserveOnDispatcher(DispatcherPriority.Normal) //needs to execute this piece on dispatcher to avoid Cross thread application
             .Subscribe(words =>
             {
                 names.Clear(); //Clear and add listbox content i.e. Hint

                 foreach (var word in words)
                 {
                    names.Add(word);
                 }
             });
        }
    }
}
