using Shipments.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
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
        }
        private ObservableCollection<LotUI> lots;
        public ObservableCollection<LotUI> Lots
        {
            get { return lots; }
            set { lots = value; OnPropertyChanged(); }
        }
        public CompanyUI Parent { get; set; }
        private Shipment shipment;
        public Shipment Shipment { 
            get { return shipment; } 
            set { 
                shipment = value;
                OnPropertyChanged();
            } 
        }
        private LotUI lot;
        public LotUI ActiveLot
        {
            get { return lot; }
            set
            {
                if (Clearing == false) Parent.ViewModel.DeselectAll();
                lot = value;
                if (ActiveLot?.Lot.Item.Name == "Add Item") Parent.ViewModel.NewLot(this);
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
                if (isExpanded == true) UpdateLots();
                OnPropertyChanged();
            }
        }
        public bool IsEnabled { get; set; }
        public bool Clearing { get; set; }
        public bool Condensing { get; set; }
        public void UpdateLots()
        {
            Shipment shipment = null;
            using (ShipmentsEntities3 db = new ShipmentsEntities3())
            {
                shipment = db.Shipments.Include(l => l.Lots.Select(i => i.Item.Specifications)).Where(s => s.Id == Shipment.Id).FirstOrDefault();
                if (shipment == null) return;
                // update the Lots with new companies loaded
                Lots = new ObservableCollection<LotUI>();

                foreach (var lot in shipment.Lots)
                {
                    Lots.Add(new LotUI(this, lot));
                }
                Lots.Add(new LotUI(this, new Lot { Item = new Item { Name = "Add Item" } }));
            }
        }
    }
}
