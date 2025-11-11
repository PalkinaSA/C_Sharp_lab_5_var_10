using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_var_10_lab_5
{
    public class Shop
    {
        private string _id;         //  ID магазина
        private string _district;   //  Район
        private string _address;    //  Адрес

        public string Id
        {
            get 
            { 
                return _id; 
            }
            set 
            {
                if (!string.IsNullOrEmpty(value)) 
                { 
                    _id = value; 
                } 
            }
        }

        public string District
        {
            get 
            { 
                return _district; 
            }
            set 
            {
                if (!string.IsNullOrEmpty(value)) 
                { 
                    _district = value; 
                } 
            }
        }

        public string Address
        {
            get 
            { 
                return _address; 
            }
            set 
            {
                if (!string.IsNullOrEmpty(value)) 
                { 
                    _address = value; 
                } 
            }
        }

        public Shop() 
        {
            Id = "не задано";
            District = "не задано";
            Address = "не задано";
        }

        public Shop(string id, string district, string address) 
        {
            Id = id;
            District = district;
            Address = address;
        }

        public override string ToString()
        {
            return $"ID магазина: {Id},\tРайон: {District},\tАдрес: {Address}";
        }
    }
}
