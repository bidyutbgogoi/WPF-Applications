using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public class MainWindowViewModel
    {
        public ObservableCollection<Employee> Employees { get; set; }

        private ObservableCollection<String> _enteredNames;

        public ObservableCollection<String> EnteredNames
        {
            get { return _enteredNames; }
            set { _enteredNames = value; }
        }

        
        public MainWindowViewModel()
        {
            //Names = new ObservableCollection<string>() { "Basu","Gogo","Mithu", "Danny" };
            Employees = new ObservableCollection<Employee>() { new Employee(1,"Basu")
                ,new Employee(2,"Gogo")
                ,new Employee(3,"Mithu")
                ,new Employee(4,"Viru")
                ,new Employee(5,"Danny") };

            _enteredNames = new ObservableCollection<string>();

            
            
        }


    }
}
