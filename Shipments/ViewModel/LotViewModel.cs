using Prism.Commands;
using Shipments.Model;
using Shipments.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class LotViewModel : ViewModelBase
    {
        // Initialize with shipment to link when adding a new lot
        public LotViewModel(ShipmentUI shipment)
        {
            Shipment = shipment;
            // setup add specific UI
            Add();
            Init();
        }
        // Initialize with an existing lot when deleting or updating
        public LotViewModel(LotUI original)
        {
            
            Lot = original;
            Item = original.Lot.Item;
            Shipment = original.Parent;
            // setup update and delete UI
            UpDel();
            Init();
        }
        public DelegateCommand Submit { get; set; }
        public DelegateCommand Delete { get; set; }
        public DelegateCommand Update { get; set; }
        private ShipmentUI Shipment { get; set; }
        public DbList Parent { get; set; }
        public SpecificationViewModel SpecificationViewModel;
        private ViewModelBase curViewModel;
        public ViewModelBase CurViewModel { get { return curViewModel; } set { curViewModel = value; OnPropertyChanged(); } }

        private LotUI lot;
        public LotUI Lot { 
            get { return lot; } 
            set { lot = value; OnPropertyChanged(); } 
        }
        private Item item;
        public Item Item { 
            get { return item; } 
            set { item = value; OnPropertyChanged(); } 
        }
        public void SubmitClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Items.Attach(Lot.Lot.Item);
                db.Entry(Lot.Lot.Item).State = System.Data.Entity.EntityState.Added;
                foreach (Specification spec in SpecificationViewModel.Specifications)
                {
                    db.Specifications.Attach(spec);
                    db.Entry(spec).State = System.Data.Entity.EntityState.Added;
                }
                db.Lots.Attach(Lot.Lot);
                db.Entry(Lot.Lot).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            Shipment.UpdateLots();
        }
        public void UpdateClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Items.Attach(Lot.Lot.Item);
                db.Entry(Lot.Lot.Item).State = System.Data.Entity.EntityState.Modified;
                foreach (Specification spec in SpecificationViewModel.Specifications)
                {
                    db.Specifications.Attach(spec);
                    if (spec.Id == 0)
                        db.Entry(spec).State = System.Data.Entity.EntityState.Added;
                    else
                        db.Entry(spec).State = System.Data.Entity.EntityState.Modified;
                }
                db.Lots.Attach(Lot.Lot);
                db.Entry(Lot.Lot).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Shipment.UpdateLots();
        }
        public void DeleteClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Lots.Attach(Lot.Lot);
                db.Entry(Lot).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            Shipment.UpdateLots();
        }
        private string submitVisibility;
        public string SubmitVisibility { get { return submitVisibility; } set { submitVisibility = value; OnPropertyChanged(); } }
        private bool saveEnabled;
        public bool SaveEnabled { get { return saveEnabled; } set { saveEnabled = value; OnPropertyChanged(); } }
        private string lotTitle;
        public string LotTitle
        {
            get { return lotTitle; }
            set { lotTitle = value; OnPropertyChanged(); }
        }
        private string addVis;
        public string AddVis { get { return addVis; } set { addVis = value; OnPropertyChanged(); } }
        private string upDelVis;
        public string UpDelVis { get { return upDelVis; } set { upDelVis = value; OnPropertyChanged(); } }
        // sets ui elements for update and delete
        private void UpDel()
        {
            UpDelVis = "Visible";
            AddVis = "Collapsed";
            LotTitle = "Edit " + Lot.Lot.Item.Name;
            // get item from existing lot
            Item = Lot.Lot.Item;
            SpecificationViewModel = new SpecificationViewModel(Lot.Lot);
            CurViewModel = SpecificationViewModel;
            UpdateDescription();
        }
        // sets ui elements for adding a new lot
        private void Add()
        {
            UpDelVis = "Collapsed";
            AddVis = "Visible";
            LotTitle = "Add Lot to " + Shipment.Shipment.Description;
            // Make a new lot tied to the shipment and add a new item
            Lot = new LotUI(Shipment, new Lot { ShipmentId = Shipment.Shipment.Id});
            Item = new Item();
            Lot.Lot.Item = Item;
            SaveEnabled = true;
            SpecificationViewModel = new SpecificationViewModel(Item);
            CurViewModel = SpecificationViewModel;
            UpdateDescription();
        }
        private void Init()
        {
            Submit = new DelegateCommand(SubmitClick);
            Delete = new DelegateCommand(DeleteClick);
            Update = new DelegateCommand(UpdateClick);
            UpdateDescription();
        }
        private void UpdateDescription()
        {
            Item.Description = Functions.GetInitials(Item.Name);
            foreach(var specifiaction in Item.Specifications)
            {
                Item.Description += "-" + Functions.GetInitials(specifiaction.Name) + specifiaction.Value;
            }
        }
    }
}
