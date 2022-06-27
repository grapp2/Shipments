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
    internal class ShipmentUI : ViewModelBase
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
        private LotUI lot;
        public LotUI ActiveLot { get { return lot; } 
            set 
            {
                if (Clearing == false) Parent.ViewModel.DeselectAll();
                lot = value;
                if (ActiveLot?.Lot.Item.Name == "Add Item") Parent.ViewModel.NewItem();
                OnPropertyChanged();
            } 
        }
        private bool isExpanded;
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (Condensing == false)
                {
                    Parent.CondenseShipments();
                    if (Clearing == false) Parent.ViewModel.DeselectAll();
                }
                isExpanded = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnabled { get; set; }
        public bool Clearing { get; set; }
        public bool Condensing { get; set; }

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
        public void CondenseShipments()
        {
            foreach (ShipmentUI shipmentUI in ShipmentUIs)
            {
                shipmentUI.Condensing = true;
                shipmentUI.IsExpanded = false;
                shipmentUI.Condensing = false;
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
                if (Condensing == false)
                {
                    ViewModel.CondenseCompanies();
                    if (Clearing == false) ViewModel.DeselectAll();
                }
                isExpanded = value;
                OnPropertyChanged();
            } 
        }
        
        public bool ExpanderActive { get; set; }
        private ShipmentUI activeShipment;
        public ShipmentUI ActiveShipment
        {
            get { return activeShipment; }
            set {
                if (Clearing == false)
                {
                    ViewModel.DeselectAll();
                }
                activeShipment = value;
                if (activeShipment?.Shipment.Description == "Add New Shipment") ViewModel.NewShipment();
                OnPropertyChanged();
            }
        }
        public bool Clearing { get; set; }
        public bool Condensing { get; set; }
    }

    internal class MainWindowViewModel : ViewModelBase, DbList
    {
        public MainWindowViewModel()
        {
            Update();
            Clearing = false;
        }
        private ObservableCollection<CompanyUI> companies;
        public ObservableCollection<CompanyUI> Companies {
            get { return companies; }
            set { companies = value; OnPropertyChanged(); }
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
                if (Clearing == false) DeselectAll();
                selectedCompany = value;
                if (selectedCompany?.Company.Name == "Add New Company") CurViewModel = new AddCompanyViewModel(this);
                else { CurViewModel = null; }
                OnPropertyChanged();
            }
        }
        public void NewShipment()
        {
            CurViewModel = new ShipmentViewModel(this, GetSender().Company);
        }
        public void NewItem()
        {
            CurViewModel = null;
        }
        public ShipmentUI GetShipment()
        {
            foreach (var comp in Companies)
            {
                foreach (var ship in comp.ShipmentUIs)
                {
                    if (ship.ActiveLot != null)
                    {
                        return ship;
                    }
                }
            }
            return null;
        }
        public CompanyUI GetSender()
        {
            foreach(var comp in Companies)
            {
                if (comp.ActiveShipment != null)
                {
                    return comp.ActiveShipment.Parent;
                }
            }
            return null;
        }
        public void DeselectAll()
        {
            DeselectCompany();
            DeselectShipments();
            DeselectLots();
        }
        public void DeselectCompany()
        {
            Clearing = true;
            SelectedCompany = null;
            Clearing = false;
        }
        public void DeselectShipments()
        {
            foreach (var comp in Companies)
            {
                comp.Clearing = true;
                comp.ActiveShipment = null;
                comp.Clearing = false;
            }
        }
        public void DeselectLots()
        {
            foreach (var comp in Companies)
            {
                foreach (var shipment in comp.ShipmentUIs)
                {
                    shipment.Clearing = true;
                    shipment.ActiveLot = null;
                    shipment.Clearing = false;
                }
            }
        }
        public void CondenseCompanies()
        {
            foreach (var comp in Companies)
            {
                comp.Condensing = true;
                comp.IsExpanded = false;
                comp.Condensing = false;
            }
        }
        public void CondenseShipments()
        {
            foreach (var comp in Companies)
            {
                foreach( var shipment in comp.ShipmentUIs)
                {
                    shipment.Condensing = true;
                    shipment.IsExpanded = false;
                    shipment.Condensing = false;
                }
            }
        }
        public bool Clearing { get; set; }
    }
}
