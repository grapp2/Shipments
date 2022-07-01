using Shipments.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.Model
{
    internal class CompanyUI : ViewModelBase
    {
        // initializes company UI with existing company data
        public CompanyUI(MainWindowViewModel viewModel, Company company)
        {
            Company = company;
            ShipmentUIs = new ObservableCollection<ShipmentUI>();
            Clearing = false;
            ViewModel = viewModel;
        }
        private ObservableCollection<ShipmentUI> shipmentUIs;
        public ObservableCollection<ShipmentUI> ShipmentUIs {
            get { return shipmentUIs; }
            set { shipmentUIs = value; OnPropertyChanged(); }
        }
        public MainWindowViewModel ViewModel { get; set; }
        private Company company;
        public Company Company
        {
            get { return company; }
            set { 
                company = value;
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
                    ViewModel.CondenseCompanies();
                    if (Clearing == false) ViewModel.DeselectAll();
                }
                isExpanded = value;
                if (isExpanded == true) UpdateShipments();
                OnPropertyChanged();
            }
        }

        private bool expanderActive;
        public bool ExpanderActive {
            get { return expanderActive; }
            set { expanderActive = value; OnPropertyChanged(); } 
        }
        private ShipmentUI activeShipment;
        public ShipmentUI ActiveShipment
        {
            get { return activeShipment; }
            set
            {
                if (Clearing == false)
                {
                    ViewModel.DeselectAll();
                }
                activeShipment = value;
                if (activeShipment?.Shipment.Description == "Add New Shipment")
                {
                    ViewModel.NewShipment();
                    CondenseShipments();
                }
                else if (activeShipment != null) ViewModel.UpdateShipment(activeShipment);
                OnPropertyChanged();
            }
        }
        public bool Clearing { get; set; }
        public bool Condensing { get; set; }
        public void UpdateShipments()
        {
            Company company = null;
            using (ShipmentsEntities3 db = new ShipmentsEntities3())
            {
                company = db.Companies.Find(this.Company.Id);
                if (company == null) return;
                // update the Lots with new companies loaded
                ShipmentUIs = new ObservableCollection<ShipmentUI>();

                foreach (var ship in company.Incoming)
                {
                    ShipmentUIs.Add(new ShipmentUI(this, ship));
                }
                ShipmentUIs.Add(new ShipmentUI(this, new Shipment() { Description="Add New Shipment" }));
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

    }
}
