using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class AddShipmentViewModel : ViewModelBase
    {
        public AddShipmentViewModel(DbList parent)
        {
            Shipment = new Shipment();
            using (var db = new ShipmentsEntities2())
            {
                Companies = new ObservableCollection<Company>(db.Companies);
            }
            Parent = parent;
        }
        public DbList Parent { get; set; }

        private Shipment shipment;
        public Shipment Shipment { get { return shipment; } set { shipment = value; OnPropertyChanged(); } }
        private ObservableCollection<Company> companies;
        public ObservableCollection<Company> Companies { get { return companies; } set { companies = value; OnPropertyChanged(); } }
        public void Submit()
        {
            using (var db = new ShipmentsEntities2())
            {
                db.Shipments.Add(Shipment);
                db.SaveChanges();
            }
            Parent.Update();
        }
    }
}
