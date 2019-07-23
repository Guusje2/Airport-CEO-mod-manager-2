using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACEOMM2
{
    public class Franchise : Business
    {
        string franchiseType;
        public List<string> products;

        public Franchise(string _name, string _id, string _countryCode, string _description, string _CEO, string _businessclass, string _type, string _imgURL, string _franchiseType, string[] _products)
        {
            name = _name;
            // id = _id;
            countryCode = _countryCode;
            description = _description;
            CEOName = _CEO;
            businessClass = BusinessClass(_businessclass);
            businessType = BusinessType(_type);
            imgURL = _imgURL;
            franchiseType = _franchiseType;
            products = new List<string>();
            products.AddRange(_products);
        }
    }
}
