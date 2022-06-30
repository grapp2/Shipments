using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Shipments.Model;

namespace Shipments.ViewModel
{
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
                foreach (var company in db.Companies.Include(s => s.Incoming.Select(l => l.Lots.Select(i => i.Item.Specifications))))
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
                if (selectedCompany?.Company.Name == "Add New Company") CurViewModel = new CompanyViewModel(this);
                else if (selectedCompany != null) { CurViewModel = new CompanyViewModel(this, SelectedCompany.Company); }
                else { CurViewModel = null; }
                OnPropertyChanged();
            }
        }
        public void NewShipment()
        {
            CurViewModel = new ShipmentViewModel(this, GetSender().Company);
        }
        public void UpdateShipment(ShipmentUI shipment)
        {
            CurViewModel = new ShipmentViewModel(this, shipment.Shipment);
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
        public void NewLot(Shipment shipment)
        {
            CurViewModel = new LotViewModel(this, shipment);
        }
        public void EditLot(LotUI lot)
        {
            CurViewModel = new LotViewModel(this, lot.Lot);
        }
        public bool Clearing { get; set; }
    }
}
