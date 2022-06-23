using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class CompanyUI : ViewModelBase
    {
        public CompanyUI(MainWindowViewModel viewModel)
        {
            Clearing = false;
            ViewModel = viewModel;
        }
        public MainWindowViewModel ViewModel { get; set; }
        public Company Company { get; set; }
        public bool ExpanderActive { get; set; }
        private Shipment activeShipment;
        public Shipment ActiveShipment
        {
            get { return activeShipment; }
            set {
                if (Clearing == false) ViewModel.ClearSelectedShipments();
                activeShipment = value;
                if (activeShipment?.Description == "Add New Shipment") ViewModel.NewShipment();
                OnPropertyChanged(); 
            }
        }
        public void ClearSelected()
        {
            Clearing = true;
            ActiveShipment = null;
            Clearing = false;
        }
        private bool Clearing;
    }
    internal class MainWindowViewModel : ViewModelBase, DbList
    {
        public MainWindowViewModel()
        {
            Update();
        }
        private ObservableCollection<CompanyUI> companies;
        public ObservableCollection<CompanyUI> Companies {
            get { return companies; }
            set { companies = value; OnPropertyChanged(); }
        }
        private int selectedIndex;
        public int SelectedIndex { 
            get 
            { 
                return selectedIndex; 
            } 
            set 
            { 
                selectedIndex = value; 
                if (selectedIndex == 0)
                {
                    Background = "AliceBlue";
                }
                else if (selectedIndex == 1)
                {
                    Background = "LightYellow";
                }
                OnPropertyChanged();
            } 
        }
        private string background;
        public string Background { get { return background; } set { background = value; OnPropertyChanged(); } }
        private ViewModelBase curViewModel;
        public ViewModelBase CurViewModel
        {
            get { return curViewModel; }
            set { curViewModel = value; OnPropertyChanged(); }
        }

        public void Update()
        {
            using (ShipmentsEntities2 db = new ShipmentsEntities2())
            {
                Companies = new ObservableCollection<CompanyUI>();
                foreach (var company in db.Companies.Include("Shipments"))
                {
                    company.Shipments.Add(new Shipment { Description = "Add New Shipment" });
                    CompanyUI comp = new CompanyUI (this) { Company = company, ExpanderActive = true};
                    Companies.Add(comp);
                }
                Companies.Add(new CompanyUI (this) { Company = new Company { Name = "Add New Company" }, ExpanderActive = false });
            }
        }

        private CompanyUI selectedCompany;
        public CompanyUI SelectedCompany { 
            get 
            { 
                return selectedCompany; 
            } 
            set 
            { 
                selectedCompany = value;
                OnPropertyChanged();


                if (selectedCompany?.Company.Name == "Add New Company") CurViewModel = new AddCompanyViewModel(this);
                else { CurViewModel = null; }

            } 
        }
        public void ClearSelectedShipments()
        {
            foreach (var comp in Companies)
            {
                comp.ClearSelected();
            }
        }
        public void NewShipment()
        {
            
        }
    }
}
