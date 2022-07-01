using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class SpecificationViewModel : ViewModelBase
    {
        public SpecificationViewModel(Item item)
        {
            // new list of specifications
            Specifications = new ObservableCollection<Specification>();
            Item = item;
            Init();
            Add();
        }
        public SpecificationViewModel(Lot lot)
        {
            Item = lot.Item;
            Specifications = new ObservableCollection<Specification>(Item.Specifications);
            Init();
            Add();
        }
        private Item Item { get; set; }
        public ObservableCollection<Item> ItemList { get; set; }
        private ObservableCollection<Specification> specifications;
        public ObservableCollection<Specification> Specifications
        {
            get { return specifications; }
            set { specifications = value; OnPropertyChanged(); }
        }
        public DelegateCommand DelegateAddSpec { get; set; }
        public DelegateCommand DelegateDeleteSpec { get; set; }
        public DelegateCommand DelegateUpdateSpec { get; set; }
        public DelegateCommand DelegateDeselect { get; set; }
        private Specification activeSpec;
        public Specification ActiveSpec
        {
            get { return activeSpec; }
            set
            {
                activeSpec = value;
                // if there is an active specification, set ui for update and delete
                if (activeSpec != null) UpDel();
                // if there is not active specification, set ui for add
                else Add();
                OnPropertyChanged();
            }
        }

        private void AddSpec()
        { 
            Specifications.Add(NewSpec);
            NewSpec = new Specification() { ItemId = Item.Id};
        }
        private void DeleteSpec()
        {
            Specifications.Remove(ActiveSpec);
        }
        private string addVis;
        public string AddVis { get { return addVis; } set { addVis = value; OnPropertyChanged(); } }
        private string upDelVis;
        public string UpDelVis { get { return upDelVis; } set { upDelVis = value; OnPropertyChanged(); } }
        private Specification newSpec;
        public Specification NewSpec { 
            get { 
                return newSpec; 
            } 
            set { 
                newSpec = value; 
                OnPropertyChanged(); 
            } 
        }
        // sets ui elements for updating or deleting an existing specification
        public void UpDel()
        {
            Instruction = "Edit selected specification";
            upDelMode = true;
            AddVis = "Collapsed";
            UpDelVis = "Visible";
            NewSpec = ActiveSpec;
        }
        // sets ui elements for adding a new specification
        public void Add()
        {
            Instruction = "Add new specification";
            upDelMode = false;
            AddVis = "Visible";
            UpDelVis = "Collapsed";
            NewSpec = new Specification() { ItemId = Item.Id };
        }
        private void Init()
        {
            DelegateAddSpec = new DelegateCommand(AddSpec);
            DelegateDeleteSpec = new DelegateCommand(DeleteSpec);
            DelegateDeselect = new DelegateCommand(Deselect);
            ItemList = new ObservableCollection<Item>();
            using (ShipmentsEntities3 db = new ShipmentsEntities3())
            {
                foreach (var item in db.Items.Include(s => s.Specifications))
                {
                    ItemList.Add(item);
                }
            }

        }
        private void Deselect()
        {
            if (!upDelMode) return;
            ActiveSpec = null;
            NewSpec = new Specification() { ItemId = Item.Id };
        }
        private bool upDelMode;
        private string instruction;
        public string Instruction { get { return instruction; } set { instruction = value; OnPropertyChanged(); } }
    }
}
