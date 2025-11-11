using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace C_Sharp_var_10_lab_5
{
    public class ProductMovement
    {
        private uint _operationId;      //  ID операции
        private DateOnly _date;         //  Дата
        private string _shopId;         //  ID магазина
        private uint _productArticle;   //  Артикул товара
        private uint _packageCount;     //  Количество упаковок, шт
        private string _operationType;  //  Тип операции

        public uint OperationId
        {
            get 
            { 
                return _operationId; 
            }
            set 
            {
                if (value > 0) 
                { 
                    _operationId = value; 
                } 
            }
        }

        public DateOnly Date
        {
            get 
            { 
                return _date; 
            }
            set 
            { 
                _date = value; 
            }
        }

        public string ShopId
        {
            get 
            { 
                return _shopId; 
            }
            set 
            {
                if (!string.IsNullOrEmpty(value)) 
                { 
                    _shopId = value; 
                } 
            }
        }

        public uint ProductArticle
        {
            get 
            { 
                return _productArticle; 
            }
            set 
            {
                if (value > 0) 
                { 
                    _productArticle = value; 
                } 
            }
        }

        public uint PackageCount
        {
            get 
            { 
                return _packageCount; 
            }
            set 
            {
                if (value > 0) 
                { 
                    _packageCount = value; 
                } 
            }
        }
        
        public string OperationType
        {
            get 
            { 
                return _operationType; 
            }
            set 
            {
                if (value == "Поступление" || value == "Продажа") 
                { 
                    _operationType = value; 
                } 
            }
        }

        public ProductMovement() 
        {
            OperationId = 1;
            Date = DateOnly.MinValue;
            ShopId = "не задано";
            ProductArticle = 1;
            PackageCount = 1;
            OperationType = "Поступление";
        }

        public ProductMovement(
            uint operationId, 
            string date, 
            string shopId, 
            uint productArticle,
            uint packageCount, 
            string operationType)
        {
            if (operationType != "Поступление" && operationType != "Продажа") 
                throw new Exception($"Ошибка валидации: некорректное значение типа операции {operationType}");

            string[] formats = 
            {
                "dd.MM.yyyy", 
                "yyyy-MM-dd", 
                "dd/MM/yyyy",
                "MM/dd/yyyy", 
                "dd.MM.yyyy H:mm:ss", 
                "dd.MM.yyyy HH:mm:ss"
            };

            if (!DateTime.TryParseExact(date.Trim(), formats, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dateTime))
            {
                throw new Exception($"Ошибка валидации: некорректное значение даты {date}");
            }

            OperationId = operationId;
            Date = DateOnly.FromDateTime(dateTime);
            ShopId = shopId;
            ProductArticle = productArticle;
            PackageCount = packageCount;
            OperationType = operationType;
        }

        public ProductMovement(
            uint operationId, 
            DateOnly date, 
            string shopId, 
            uint productArticle,
            uint packageCount,
            string operationType)
        {
            if (operationType != "Поступление" && operationType != "Продажа")
                throw new Exception("Ошибка валидации: некорректное значение типа операции.");

            OperationId = operationId;
            Date = date;
            ShopId = shopId;
            ProductArticle = productArticle;
            PackageCount = packageCount;
            OperationType = operationType;
        }

        public override string ToString()
        {
            return $"ID операции: {OperationId},\tДата: {Date},\t" +
                $"ID магазина: {ShopId},\tАртикул: {ProductArticle},\t" +
                $"Количество упаковок: {PackageCount},\tТип операции: {OperationType}";
        }
    }
}