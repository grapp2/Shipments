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
        public CompanyUI(MainWindowViewModel viewModel, Company company)
        {
            Company = company;
            ShipmentUIs = new ObservableCollection<ShipmentUI>();
            Clearing = false;
            ViewModel = viewModel;
            foreach (Shipment shipment in Company.Incoming)
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
                OnPropertyChanged();
            }
        }

        public bool ExpanderActive { get; set; }
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
                if (activeShipment?.Shipment.Description == "Add New Shipment") ViewModel.NewShipment();
                else if (activeShipment != null) ViewModel.UpdateShipment(activeShipment);
                OnPropertyChanged();
            }
        }
        public bool Clearing { get; set; }
        public bool Condensing { get; set; }
    }
}
