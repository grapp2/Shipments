using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.ViewModel
{
    internal class CompanyViewModel : ViewModelBase
    {
        public CompanyViewModel()
        {

        }
        public CompanyViewModel(DbList parent)
        {
            CompanyText = "Add New Company";
            Parent = parent;
            Company = new Company();
            Submit = new DelegateCommand(SubmitClick);
            SubmitEnable = true;
        }
        public CompanyViewModel(DbList parent, Company company)
        {
            Company = company;
            Parent = parent;
            CompanyText = Company.Name;
            Submit = new DelegateCommand(UpdateClick);
            SubmitEnable = true;
        }

        public DbList Parent { get; set; }
        private Company company;
        public Company Company
        {
            get { return company; }
            set { 
                company = value; 
                OnPropertyChanged();
            }
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
        public void UpdateClick()
        {
            using (ShipmentsEntities3 db = new ShipmentsEntities3())
            {
                Shipment ship = Company.Incoming.ToList()[Company.Incoming.Count - 1];
                Company.Incoming.Remove(ship);
                db.Companies.Attach(Company);
                db.Entry(Company).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Parent.Update();
        }
        private string companyText;
        public string CompanyText
        {
            get
            {
                return companyText;
            }
            set
            {
                companyText = value;
                OnPropertyChanged();
            }
        }
        private bool submitEnable;
        public bool SubmitEnable
        {
            get
            {
                return submitEnable;
            }
            set
            {
                submitEnable = value; OnPropertyChanged();
            }
        }

    }
}
