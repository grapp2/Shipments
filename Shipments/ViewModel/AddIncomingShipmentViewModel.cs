using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class AddIncomingShipmentViewModel : ViewModelBase
    {
        public AddIncomingShipmentViewModel(DbList parent, Company Sender)
        {
            Shipment = new Shipment { Sender = Sender.Id};
            Parent = parent;
            ShipmentTitle = "New Incoming " + Sender.Name + " Shipment";
            Submit = new DelegateCommand(SubmitClick);
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
    }
}
