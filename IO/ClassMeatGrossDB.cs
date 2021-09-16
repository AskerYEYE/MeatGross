using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class ClassMeatGrossDB : ClassDbCon
    {
        public ClassMeatGrossDB()
        {

        }


        public List<ClassCustomer> GetAllCustomerFromDB()
        {
            List<ClassCustomer> listCC = new List<ClassCustomer>();

            DataTable dataTable = DbReturnDataTable("SELECT Customer.id, Customer.CompanyName, Customer.Address, " +
                                                    "Customer.ZipCity, Customer.Phone, Customer.Mail, " +
                                                    "Customer.ContactName, Customer.Country, CountryAndRates.CountryName " +
                                                    "FROM CountryAndRates RIGHT OUTER JOIN " +
                                                    "Customer ON CountryAndRates.id = Customer.Country");

            foreach (DataRow row in dataTable.Rows)
            {
                ClassCustomer classCustomer = new ClassCustomer();
                ClassCountry classCountry = new ClassCountry();

                classCustomer.id = Convert.ToInt32(row["id"]);
                classCustomer.companyName = row["CompanyName"].ToString();
                classCustomer.address = row["Address"].ToString();
                classCustomer.zipCity = row["ZipCity"].ToString();
                classCustomer.phone = row["Phone"].ToString();
                classCustomer.mail = row["Mail"].ToString();
                classCustomer.contactName = row["ContactName"].ToString();

                classCustomer.country = row["Country.id"].ToString();

                listCC.Add(classCustomer);
            }
            return listCC;
        }

        public List<ClassCountry> GetAllCountryFromDB()
        {
            List<ClassCountry> listCC = new List<ClassCountry>();

            DataTable dataTable = DbReturnDataTable("SELECT * FROM CountryAndRates");

            foreach (DataRow row in dataTable.Rows)
            {
                ClassCountry CC = new ClassCountry();

                CC.id = Convert.ToInt32(row["id"]);
                CC.countryCode = row["CountryCode"].ToString();
                CC.countryName = row["CountryName"].ToString();
                CC.valutaName = row["ValutaName"].ToString();
                CC.valutaRate = Convert.ToDouble(row["ValutaRate"]);

                CC.updateTime = Convert.ToDateTime(row["UpdateTime"]);

                listCC.Add(CC);
            }
            return listCC;
        }


        public List<ClassMeat> GetAllMeatFromDB()
        {
            List<ClassMeat> listCM = new List<ClassMeat>();

            DataTable dataTable = DbReturnDataTable("SELECT * FROM Meat");

            foreach (DataRow row in dataTable.Rows)
            {
                ClassMeat CM = new ClassMeat();

                CM.id = Convert.ToInt32(row["id"]);
                CM.TypeOfMeat = row["TypeOfMeat"].ToString();
                CM.stock = Convert.ToInt32(row["Stock"]);
                CM.price = Convert.ToInt32(row["Price"]);
                CM.priceTimeStamp = Convert.ToDateTime(row["PriceTimeStamp"]);

                CM.strTimeStamp = Convert.ToString(CM.priceTimeStamp);

                listCM.Add(CM);
            }
            return listCM;
        }

    }
}
