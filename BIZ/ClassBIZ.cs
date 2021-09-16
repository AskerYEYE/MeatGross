using IO;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class ClassBIZ : ClassNotify
    {
        ClassMeatGrossDB CMG;
        ClassCallWebAPI CCWA;

        private List<ClassCustomer> _listCustomer;
        private List<ClassCountry> _listCountry;
        private List<ClassMeat> _listMeat;
        private ClassApiRates _apiRates;
        private ClassCustomer _selectedCustomer;
        private ClassCustomer _editOrNewCustomer;
        private ClassOrder _order;
        private ClassMeat _selectedMeat;
        private ClassCountry _selectedCountry;
        private bool _isEnabled;


        public ClassBIZ()
        {
            listCustomer = new List<ClassCustomer>();
            listCountry = new List<ClassCountry>();
            listMeat = new List<ClassMeat>();
            apiRates = new ClassApiRates();
            selectedCustomer = new ClassCustomer();
            editOrNewCustomer = new ClassCustomer();
            order = new ClassOrder();
            selectedMeat = new ClassMeat();
            selectedCountry = new ClassCountry();
            isEnabled = true;
            CMG = new ClassMeatGrossDB();
            CCWA = new ClassCallWebAPI();

            listCustomer = CMG.GetAllCustomerFromDB();
            listCountry = CMG.GetAllCountryFromDB();
            listMeat = CMG.GetAllMeatFromDB();
        }


        public ClassCountry selectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                }
                Notify("selectedCountry");
            }
        }


        public ClassMeat selectedMeat
        {
            get { return _selectedMeat; }
            set
            {
                if (_selectedMeat != value)
                {
                    _selectedMeat = value;
                }
                Notify("selectedMeat");
            }
        }



        public bool isEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                }
                Notify("isEnabled");
            }
        }


        public ClassOrder order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    _order = value;
                }
                Notify("order");
            }
        }


        public ClassCustomer editOrNewCustomer
        {
            get { return _editOrNewCustomer; }
            set
            {
                if (_editOrNewCustomer != value)
                {
                    _editOrNewCustomer = value;
                }
                Notify("editOrNewCustomer");
            }
        }


        public ClassCustomer selectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                }
                Notify("selectedCustomer");
            }
        }


        public ClassApiRates apiRates
        {
            get { return _apiRates; }
            set
            {
                if (_apiRates != value)
                {
                    _apiRates = value;
                }
                Notify("apiRates");
            }
        }


        public List<ClassMeat> listMeat
        {
            get { return _listMeat; }
            set
            {
                if (_listMeat != value)
                {
                    _listMeat = value;
                }
                Notify("listMeat");
            }
        }


        public List<ClassCountry> listCountry
        {
            get { return _listCountry; }
            set
            {
                if (_listCountry != value)
                {
                    _listCountry = value;
                }
                Notify("listCountry");
            }
        }


        public List<ClassCustomer> listCustomer
        {
            get { return _listCustomer; }
            set
            {
                if (_listCustomer != value)
                {
                    _listCustomer = value;
                }
                Notify("listCustomer");
            }
        }


        public void UpdateListCustomer()
        {

        }

        public async Task<ClassApiRates> GetApiRates()
        {
            try
            {
                while (true)
                {
                    string strJson = await CCWA.GetURLContentsAsync("https://openexchangerates.org/api/latest.json?app_id=ab99a24a526b42089c85876ce136ccf0&base=USD");

                    apiRates = JsonConvert.DeserializeObject<ClassApiRates>(strJson);
                    await Task.Delay(60000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetUpListCustomer()
        {

        }

        public int SaveNewCustomer()
        {
            int res = 0;


            return res;
        }

        public void UpdateCustomer()
        {

        }

    }
}
