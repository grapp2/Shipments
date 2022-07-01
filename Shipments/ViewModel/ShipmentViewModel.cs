using Prism.Commands;
using Shipments.Model;
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
        public ShipmentViewModel(DbList parent, CompanyUI Sender)
        {
            Shipment = new Shipment { Sender = Sender.Company.Id};
            Parent = parent;
            ShipmentTitle = "Add Incoming " + Sender.Company.Name + " Shipment";
            SaveEnabled = true;
            this.Sender = Sender;
            AddUI();
            Init();
        }
        public ShipmentViewModel(DbList parent, ShipmentUI original)
        {
            Sender = original.Parent;
            Shipment = original.Shipment;
            Parent = parent;
            ShipmentTitle = "Update Shipment " + Shipment.Description;
            SaveEnabled=false;
            UpDelUI();
            Init();
        }
        public CompanyUI Sender { get; set; }
        public DelegateCommand Add { get; set; }
        public DelegateCommand Update { get; set; }
        public DelegateCommand Delete { get; set; }
        private string shipmentTitle;
        public string ShipmentTitle
        {
            get { return shipmentTitle; }
            set { shipmentTitle = value; OnPropertyChanged(); }
        }
        public DbList Parent { get; set; }
        private Shipment shipment;
        public Shipment Shipment { get { return shipment; } set { shipment = value; OnPropertyChanged(); } }
        private string submitVisibility;
        public string SubmitVisibility { get { return submitVisibility; } set { submitVisibility = value; OnPropertyChanged(); } }
        private bool saveEnabled;
        public bool SaveEnabled { get { return saveEnabled; } set { saveEnabled = value; OnPropertyChanged(); } }
        private string addVis;
        public string AddVis { get { return addVis; } set { addVis = value; OnPropertyChanged(); } }
        private string upDelVis;
        public string UpDelVis { get { return upDelVis; } set { upDelVis = value; OnPropertyChanged(); } }
        private void AddUI()
        {
            AddVis = "Visible";
            UpDelVis = "Collapsed";
        }
        private void UpDelUI()
        {
            AddVis = "Collapsed";
            UpDelVis = "Visible";
        }
        private void Init()
        {
            Add = new DelegateCommand(AddClick);
            Update = new DelegateCommand(UpdateClick);
            Delete = new DelegateCommand(DeleteClick);
        }
        public void AddClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Shipments.Add(Shipment);
                db.SaveChanges();
            }
            Sender.UpdateShipments();
        }
        public void UpdateClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Shipments.Attach(Shipment);
                db.Entry(Shipment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Sender.UpdateShipments();
        }
        public void DeleteClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Shipments.Attach(Shipment);
                db.Entry(Shipment).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            Sender.UpdateShipments();
        }
    }
}
