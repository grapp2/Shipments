using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class ShipmentViewModel : ViewModelBase
    {
        public ShipmentViewModel(DbList parent, Company Sender)
        {
            Shipment = new Shipment { Sender = Sender.Id};
            Parent = parent;
            ShipmentTitle = "Add Incoming " + Sender.Name + " Shipment";
            Submit = new DelegateCommand(SubmitClick);
            SaveEnabled = true;
        }
        public ShipmentViewModel(DbList parent, Shipment original)
        {
            Shipment = original;
            Parent = parent;
            ShipmentTitle = "Update Shipment " + Shipment.Description;
            Submit = new DelegateCommand(SubmitClick);
            SaveEnabled=false;
        }
        public DelegateCommand Submit { get; set; }
        private string shipmentTitle;
        public string ShipmentTitle
        {
            get { return shipmentTitle; }
            set { shipmentTitle = value; OnPropertyChanged(); }
        }
        public DbList Parent { get; set; }
        private Shipment shipment;
        public Shipment Shipment { get { return shipment; } set { shipment = value; OnPropertyChanged(); } }
        public void SubmitClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Shipments.Add(Shipment);
                db.SaveChanges();
            }
            Parent.Update();
        }
        private string submitVisibility;
        public string SubmitVisibility { get { return submitVisibility; } set { submitVisibility = value; OnPropertyChanged(); } }
        private bool saveEnabled;
        public bool SaveEnabled { get { return saveEnabled; } set { saveEnabled = value; OnPropertyChanged(); } }
    }
}
