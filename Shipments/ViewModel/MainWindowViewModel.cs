using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class LotUI
    {
        public LotUI(ShipmentUI parent, Lot lot)
        {
            Parent = parent;
            Lot = lot;
            if (Lot?.Item.Name != "Add New Lot to Shipment") IsEnabled = true;
            else IsEnabled = false;
        }
        public ShipmentUI Parent { get; set; }
        public Lot Lot { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsEnabled { get; set; }
    }
    internal class ShipmentUI
    {
        public ShipmentUI(CompanyUI parent, Shipment shipment)
        {
            Lots = new ObservableCollection<LotUI>();
            Parent = parent;
            Shipment = shipment;
            if (shipment.Description != "Add New Shipment") IsEnabled = true;
            else IsEnabled = false;
            foreach (Lot lot in shipment.Lots)
            {
                Lots.Add(new LotUI(this, lot));
            }
            Lots.Add(new LotUI(this, new Lot { Item = new Item { Name = "Add Item" } }));
        }
        public ObservableCollection<LotUI> Lots { get; set; }
        public CompanyUI Parent { get; set; }
        public Shipment Shipment { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsEnabled { get; set; }
    }
    internal class CompanyUI : ViewModelBase
    {
        public CompanyUI(MainWindowViewModel viewModel, Company company)
        {
            Company = company;
            ShipmentUIs = new ObservableCollection<ShipmentUI>();
            Clearing = false;
            ViewModel = viewModel;
            foreach(Shipment shipment in Company.Incoming)
            {
                ShipmentUIs.Add(new ShipmentUI(this, shipment));
            }
        }
        public ObservableCollection<ShipmentUI> ShipmentUIs { get; set; }
        public MainWindowViewModel ViewModel { get; set; }
        public Company Company { get; set; }
        private bool isExpanded;
        public bool IsExpanded {
            get
            {
                return isExpanded;
            }
            set 
            { 
                ViewModel.DeselectCompany();
                ViewModel.DeselectShipments();
                isExpanded = value; 
            } 
        }
        
        public bool ExpanderActive { get; set; }
        private ShipmentUI activeShipment;
        public ShipmentUI ActiveShipment
        {
            get { return activeShipment; }
            set {
                if (Clearing != true)
                {
                    ViewModel.DeselectCompany();
                    ViewModel.DeselectShipments();
                }
                activeShipment = value;
                if (activeShipment?.Shipment.Description == "Add New Shipment") ViewModel.NewShipment();
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
            using (ShipmentsEntities3 db = new ShipmentsEntities3())
            {
                Companies = new ObservableCollection<CompanyUI>();
                foreach (var company in db.Companies.Include(s => s.Incoming.Select(l => l.Lots)))
                {
                    company.Incoming.Add(new Shipment { Description = "Add New Shipment" });
                    CompanyUI comp = new CompanyUI (this, company) { ExpanderActive = true};
                    Companies.Add(comp);
                }
                Companies.Add(new CompanyUI (this, new Company { Name = "Add New Company" }) { ExpanderActive = false });
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
                DeselectShipments();
                if (selectedCompany?.Company.Name == "Add New Company") CurViewModel = new AddCompanyViewModel(this);
                else { CurViewModel = null; }

            } 
        }
        public void DeselectShipments()
        {
            foreach (var comp in Companies)
            {
                comp.ClearSelected();
            }
        }
        public void NewShipment()
        {
            CurViewModel = new AddIncomingShipmentViewModel(this, GetSelectedShipment());
        }
        public void DeselectCompany()
        {
            SelectedCompany = null;
        }
        public Company GetSelectedShipment()
        {
            Company company = null;
            foreach (var comp in Companies)
            {
                if (comp.ActiveShipment != null)
                {
                    company = comp.Company;
                }
            }
            return company;
        }
    }
}
