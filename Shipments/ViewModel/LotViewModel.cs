using Prism.Commands;
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
        public LotViewModel(DbList parent, Shipment shipment)
        {
            Shipment = shipment;
            Parent = parent;
            Init();
            // setup add specific UI
            Add();
        }
        // Initialize with an existing lot when deleting or updating
        public LotViewModel(DbList parent, Lot original)
        {
            Lot = original;
            Parent = parent;
            Init();
            // setup update and delete UI
            UpDel();
        }
        public DelegateCommand Submit { get; set; }
        public DelegateCommand Delete { get; set; }
        public DelegateCommand Update { get; set; }
        private Shipment Shipment { get; set; }
        public DbList Parent { get; set; }
        public SpecificationViewModel SpecificationViewModel;
        private ViewModelBase curViewModel;
        public ViewModelBase CurViewModel { get { return curViewModel; } set { curViewModel = value; OnPropertyChanged(); } }

        private Lot lot;
        public Lot Lot { get { return lot; } set { lot = value; OnPropertyChanged(); } }
        private Item item;
        public Item Item { get { return item; } set { item = value; OnPropertyChanged(); } }
        public void SubmitClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Items.Attach(Lot.Item);
                db.Entry(Lot.Item).State = System.Data.Entity.EntityState.Added;
                foreach (Specification spec in SpecificationViewModel.Specifications)
                {
                    db.Specifications.Attach(spec);
                    db.Entry(spec).State = System.Data.Entity.EntityState.Added;
                }
                db.Lots.Attach(Lot);
                db.Entry(Lot).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            Parent.Update();
        }
        public void UpdateClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Items.Attach(Lot.Item);
                db.Entry(Lot.Item).State = System.Data.Entity.EntityState.Modified;
                foreach (Specification spec in SpecificationViewModel.Specifications)
                {
                    db.Specifications.Attach(spec);
                    db.Entry(spec).State = System.Data.Entity.EntityState.Modified;
                }
                db.Lots.Attach(Lot);
                db.Entry(Lot).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Parent.Update();
        }
        public void DeleteClick()
        {
            using (var db = new ShipmentsEntities3())
            {
                db.Lots.Attach(Lot);
                db.Entry(Lot).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            Parent.Update();
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
            LotTitle = "Edit " + Lot.Item.Name;
            // get item from existing lot
            Item = Lot.Item;
            SpecificationViewModel = new SpecificationViewModel(Lot);
            CurViewModel = SpecificationViewModel;
        }
        // sets ui elements for adding a new lot
        private void Add()
        {
            UpDelVis = "Collapsed";
            AddVis = "Visible";
            LotTitle = "Add Lot to " + Shipment.Description;
            // Make a new lot tied to the shipment and add a new item
            Lot = new Lot { ShipmentId = Shipment.Id };
            Item = new Item();
            Lot.Item = Item;
            SaveEnabled = true;
            SpecificationViewModel = new SpecificationViewModel(Item);
            CurViewModel = SpecificationViewModel;
        }
        private void Init()
        {
            Submit = new DelegateCommand(SubmitClick);
            Delete = new DelegateCommand(DeleteClick);

        }
    }
}
