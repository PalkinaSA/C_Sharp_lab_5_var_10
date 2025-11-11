using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace C_Sharp_var_10_lab_5
{
    public class Product
    {
        private uint _article;          //  Артикул
        private string _department;     //  Отдел
        private string _name;           //  Название
        private string _unit;           //  Единица измерения
        private uint _itemsPerPackage;  //  Кол-во в упаковке
        private uint _price;            //  Цена за упаковку

        public uint Article
        {
            get 
            { 
                return _article; 
            }
            set 
            {
                if (value > 0) 
                { 
                    _article = value; 
                } 
            }
        }

        public string Department
        {
            get 
            { 
                return _department; 
            }
            set 
            {
                if (!string.IsNullOrEmpty(value)) 
                { 
                    _department = value; 
                } 
            }
        }

        public string Name
        {
            get 
            {
                return _name; 
            }
            set 
            {
                if (!string.IsNullOrEmpty(value)) 
                { 
                    _name = value; 
                }
            }
        }

        public string Unit
        {
            get 
            { 
                return _unit; 
            }
            set 
            {
                if (!string.IsNullOrEmpty(value)) 
                { 
                    _unit = value; 
                }
            }
        }

        public uint ItemsPerPackage
        {
            get 
            { 
                return _itemsPerPackage; 
            }
            set 
            {
                if (value >= 0) 
                { 
                    _itemsPerPackage = value; 
                } 
            }
        }

        public uint Price
        {
            get 
            { 
                return _price; 
            }
            set 
            {
                if (value > 0) 
                { 
                    _price = value; 
                } 
            }
        }

        public Product() 
        {
            Article = 1;
            Department = "не задано";
            Name = "не задано";
            Unit = "не задано";
            ItemsPerPackage = 1;
            Price = 1;
        }

        public Product(
            uint article, 
            string department, 
            string name, 
            string unit, 
            uint itemsPerPackage, 
            uint price)
        {
            Article = article;
            Department = department;
            Name = name;
            Unit = unit;
            ItemsPerPackage = itemsPerPackage;
            Price = price;
        }

        public override string ToString()
        {
            return $"Артикул: {Article},\tОтдел: {Department},\tНаименование: {Name},\t" +
                $"Единица измерения: {Unit},\tКоличество в упаковке: {ItemsPerPackage},\t" +
                $"Цена за упаковку: {Price}";
        }
    }
}
