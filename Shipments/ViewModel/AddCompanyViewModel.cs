using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class AddCompanyViewModel : ViewModelBase
    {
        public AddCompanyViewModel()
        {

        }
        public AddCompanyViewModel(DbList parent)
        {
            Parent = parent;
            Company = new Company();
            Submit = new DelegateCommand(SubmitClick);
        }
        public DbList Parent { get; set; }
        private Company company;

        public Company Company
        {
            get { return company; }
            set { company = value; OnPropertyChanged(); }
        }
        public DelegateCommand Submit { get; set; }
        public void SubmitClick()
        {
            using (ShipmentsEntities3 db = new ShipmentsEntities3())
            {
                db.Companies.Add(Company);
                db.SaveChanges();
            }
            Parent.Update();
        }
    }
}
