using Shipments.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.Model
{
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
        public LotUI ActiveLot
        {
            get { return lot; }
            set
            {
                if (Clearing == false) Parent.ViewModel.DeselectAll();
                lot = value;
                if (ActiveLot?.Lot.Item.Name == "Add Item") Parent.ViewModel.NewLot(Shipment);
                else if (ActiveLot != null) Parent.ViewModel.EditLot(ActiveLot);
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
}
