using ConsoleTables;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Pipelines;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace C_Sharp_var_10_lab_5
{
    public static class DatabaseController
    {
        // Чтение базы данных из файла excel
        public static void ReadDatabase(
            List<Product> products, 
            List<Shop> shops, 
            List<ProductMovement> productMovements)
        {
            if (products.Count > 0 || shops.Count > 0 || productMovements.Count > 0) 
            {
                Console.WriteLine("База данных уже заполнена. Вы хотите её перезаписать?");
                Console.Write("Ответьте ДА или НЕТ: ");

                switch (Console.ReadLine().ToLower())
                {
                    case "да":
                        {
                            Console.WriteLine("База данных будет успешно перезаписана.");
                            products.Clear();
                            shops.Clear();
                            productMovements.Clear();
                            break;
                        }
                    case "нет":
                        {
                            Console.WriteLine("База данных останется такой, какой была раньше.");
                            Logger.Log("База данных не прочитана");
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Ваш ответ не является словом ДА или НЕТ.");
                            Console.WriteLine("Завершаю работу с утилитой.");
                            Logger.Log("База данных не прочитана");
                            return;
                        }
                }
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // Для поддержки старых кодировок

            // Проверка существования файла базы данных
            if (!File.Exists(Constants.dbPath))
            {
                Console.WriteLine("Файл базы данных отсутствует.");
                return;
            }

            // Чтение Excel-файла
            using (var stream = File.Open(Constants.dbPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // Обработка листа "Товар"
                    var productTable = result.Tables["Товар"];
                    if (productTable != null)
                    {
                        for (int i = 1; i < productTable.Rows.Count; i++) // Пропускаем заголовок
                        {
                            products.Add(new Product
                            (
                                Convert.ToUInt32(productTable.Rows[i][0]),
                                productTable.Rows[i][1].ToString(),
                                productTable.Rows[i][2].ToString(),
                                productTable.Rows[i][3].ToString(),
                                Convert.ToUInt32(productTable.Rows[i][4]),
                                Convert.ToUInt32(productTable.Rows[i][5])
                            ));
                        }
                    }

                    // Обработка листа "Магазин"
                    var shopTable = result.Tables["Магазин"];
                    if (shopTable != null) 
                    { 
                        for (int i = 1; i < shopTable.Rows.Count; i++)  // Пропускаем заголовок
                        {
                            shops.Add(new Shop
                            (
                                shopTable.Rows[i][0].ToString(),
                                shopTable.Rows[i][1].ToString(),
                                shopTable.Rows[i][2].ToString()
                            ));
                        }
                    }

                    // Обработка листа "Движение товаров"
                    var productMovementTable = result.Tables["Движение товаров"];
                    if (productMovementTable != null)
                    {
                        for (int i = 1; i < productMovementTable.Rows.Count; i++) // Пропускаем заголовок
                        {
                            productMovements.Add(new ProductMovement
                            (
                                Convert.ToUInt32(productMovementTable.Rows[i][0]),
                                productMovementTable.Rows[i][1].ToString(),
                                productMovementTable.Rows[i][2].ToString(),
                                Convert.ToUInt32(productMovementTable.Rows[i][3]),
                                Convert.ToUInt32(productMovementTable.Rows[i][4]),
                                productMovementTable.Rows[i][5].ToString()
                            ));
                        }
                    }
                }
            }
            Console.WriteLine("База данных успешно прочитана.");
            Logger.Log("База данных успешно прочитана");
        }

        
        // Вывод таблицы товаров
        public static void PrintProducts(List<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("\tТаблица 'Товар' пуста.");
                return;
            }

            Console.WriteLine("\tТаблица 'Товар':");

            var table = new ConsoleTable("Артикул", "Отдел", "Наименование товара", 
                "Единица измерения", "Количество в упаковке", "Цена за упаковку");
            foreach (var product in products)
            {
                table.AddRow(product.Article, product.Department, product.Name, 
                    product.Unit, product.ItemsPerPackage, product.Price);
            }

            table.Write();
            Console.WriteLine();

            Logger.Log("Таблица 'Товар' выведена на экран");
        }

        // Вывод таблицы магазинов
        public static void PrintShops(List<Shop> shops) 
        {
            if (shops.Count == 0)
            {
                Console.WriteLine("\tТаблица 'Магазин' пуста.");
                return;
            }

            Console.WriteLine("\tТаблица 'Магазин':");

            var table = new ConsoleTable("ID магазина", "Район", "Адрес");
            foreach (var shop in shops)
            {
                table.AddRow(shop.Id, shop.District, shop.Address);
            }
            table.Write();
            Console.WriteLine();

            Logger.Log("Таблица 'Магазин' выведена на экран");
        }

        // Вывод таблицы движения товаров
        public static void PrintProductMovements(List<ProductMovement> productMovements)
        {
            if (productMovements.Count == 0)
            {
                Console.WriteLine("\tТаблица 'Движение товаров' пуста.");
                return;
            }
            
            Console.WriteLine("\tТаблица 'Движение товаров':");

            var table = new ConsoleTable("ID операции", "Дата", "ID магазина", 
                "Артикул", "Количество упаковок, шт.", "Тип операции");
            foreach (var productMovement in productMovements)
            {
                table.AddRow(productMovement.OperationId, productMovement.Date, 
                    productMovement.ShopId, productMovement.ProductArticle, 
                    productMovement.PackageCount, productMovement.OperationType);
            }
            table.Write();
            Console.WriteLine();

            Logger.Log("Таблица 'Движение товаров' выведена на экран");
        }

        // Удаление товаров (по ключу)
        public static void DeleteProduct(
            List<Product> products, 
            List<ProductMovement> productMovements, 
            uint key)
        {
            Product? product = products.Find(x => x.Article == key);
            if (product == null)
            {
                Console.WriteLine("Данный товар не найден.");
                Logger.Log("Удаление товара не произведено");
                return;
            }

            productMovements.RemoveAll(x => x.ProductArticle == key);
            products.Remove(product);

            Console.WriteLine("Товар успешно удалён.");
            Logger.Log($"Удаление товара {product.ToString()} успешно проведено");
        }

        // Удаление магазинов (по ключу)
        public static void DeleteShop(
            List<Shop> shops, 
            List<ProductMovement> productMovements, 
            string key)
        {
            Shop? shop = shops.Find(x => x.Id == key);
            if (shop == null)
            {
                Console.WriteLine("Данный магазин не найден.");
                Logger.Log("Удаление магазина не произведено");
                return;
            }

            productMovements.RemoveAll(x => x.ShopId == key);
            shops.Remove(shop);

            Console.WriteLine("Магазин успешно удалён.");
            Logger.Log($"Удаление магазина {shop.ToString()} успешно проведено");
        }

        // Удаление движений товаров (по ключу)
        public static void DeleteProductMovement(List<ProductMovement> productMovements, uint key)
        {
            ProductMovement? productMovement = productMovements.Find(x => x.OperationId == key);
            if (productMovement == null)
            {
                Console.WriteLine("Данное движение товаров не найдено.");
                Logger.Log("Удаление движения товаров не произведено");
                return;
            }

            productMovements.Remove(productMovement);

            Console.WriteLine("Движение товаров успешно удалено.");
            Logger.Log($"Удаление движения товаров {productMovement.ToString()} успешно проведено");
        }

        // Изменение товаров (по ключу)
        public static void UpdateProduct(List<Product> products, uint key) 
        {
            Product? product = products.Find(x => x.Article == key);
            if (product == null)
            {
                Console.WriteLine("Данный товар не найден.");
                Logger.Log("Обновление товара не произведено");
                return;
            }

            Console.WriteLine("Занесите новые данные: ");
            string department, name, unit;
            uint itemsPerPackage, price;

            try
            {
                Console.Write("Введите отдел товара: ");
                department = Console.ReadLine();
                if (string.IsNullOrEmpty(department)) 
                {
                    throw new Exception();
                }

                Console.Write("Введите название товара: ");
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception();
                }

                Console.Write("Введите единицу измерения товара: ");
                unit = Console.ReadLine();
                if (string.IsNullOrEmpty(unit))
                {
                    throw new Exception();
                }

                Console.Write("Введите кол-во в упаковке товара: ");
                itemsPerPackage = Convert.ToUInt32(Console.ReadLine());
                if (itemsPerPackage == 0)
                {
                    throw new Exception();
                }

                Console.Write("Введите цену за упаковку товара: ");
                price = Convert.ToUInt32(Console.ReadLine());
                if (price == 0) 
                { 
                    throw new Exception();
                }
            }
            catch
            {
                Console.WriteLine("Неправильно введены данные о товаре.");
                Logger.Log("Обновление товара не произведено");
                return;
            }

            Logger.Log("Обновление товара " + product.ToString() + " успешно произведено");

            product.Department = department;
            product.Name = name;
            product.Unit = unit;
            product.ItemsPerPackage = itemsPerPackage;
            product.Price = price;

            Console.WriteLine("Товар успешно изменён.");
        }

        // Изменение магазинов (по ключу)
        public static void UpdateShop(List<Shop> shops, string key)
        {
            Shop? shop = shops.Find(x => x.Id == key);
            if (shop == null)
            {
                Console.WriteLine("Данный магазин не найден.");
                Logger.Log("Обновление магазина не произведено");
                return;
            }

            Console.WriteLine("Занесите новые данные: ");
            string district, address;
            try
            {
                Console.Write("Введите район магазина: ");
                district = Console.ReadLine();
                if (string.IsNullOrEmpty(district))
                {
                    throw new Exception();
                }

                Console.Write("Введите адрес магазина: ");
                address = Console.ReadLine();
                if (string.IsNullOrEmpty(address))
                {
                    throw new Exception();
                }
            }
            catch
            {
                Console.WriteLine("Неправильно введены данные о магазине.");
                Logger.Log("Обновление магазина не произведено");
                return;
            }

            Logger.Log("Обновление магазина " + shop.ToString() + " успешно произведено");

            shop.District = district;
            shop.Address = address;

            Console.WriteLine("Магазин успешно изменён.");
        }

        // Изменение движений товаров (по ключу)
        public static void UpdateProductMovement(
            List<Product> products, 
            List<Shop> shops,
            List<ProductMovement> productMovements, 
            uint key)
        {
            ProductMovement? productMovement = productMovements.Find(x => x.OperationId == key);
            if (productMovement == null)
            {
                Console.WriteLine("Данное движение товаров не найдено.");
                Logger.Log("Обновление движения товаров не произведено");
                return;
            }

            Console.WriteLine("Занесите новые данные: ");

            DateOnly date;
            string shopId;
            uint productArticle;
            uint packageCount;
            string operationType;

            try
            {
                Console.Write("Введите дату движения товаров (dd.mm.yyyy): ");
                date = DateOnly.Parse(Console.ReadLine());

                Console.Write("Введите ID магазина движения товаров (начинается с 'M' английского алфавита): ");
                shopId = Console.ReadLine();
                if (string.IsNullOrEmpty(shopId) || shops.Find(x => x.Id == shopId) == null)
                {
                    throw new Exception();
                }

                Console.Write("Введите артикул товара движения товаров: ");
                productArticle = Convert.ToUInt32(Console.ReadLine());
                if (products.Find(x => x.Article == productArticle) == null) 
                { 
                    throw new Exception();
                }

                Console.Write("Введите кол-во упаковок движения товаров: ");
                packageCount = Convert.ToUInt32(Console.ReadLine());
                if (packageCount == 0)
                {
                    throw new Exception();
                }

                Console.Write("Введите тип операции движения товаров (Продажа/Поступление): ");
                string operationTypeString = Console.ReadLine().ToLower().Trim();
                if (string.IsNullOrEmpty(operationTypeString) || 
                    (operationTypeString != "продажа" && operationTypeString != "поступление")) 
                {
                    throw new Exception();
                }
                operationType = operationTypeString == "продажа" ? "Продажа" : "Поступление";
            }
            catch
            {
                Console.WriteLine("Неправильно введены данные о движении товаров.");
                Logger.Log("Обновление движения товаров не произведено");
                return;
            }

            Logger.Log("Обновление движения товаров " + productMovement.ToString() + " успешно произведено");

            productMovement.Date = date;
            productMovement.ShopId = shopId;
            productMovement.ProductArticle = productArticle;
            productMovement.PackageCount = packageCount;
            productMovement.OperationType = operationType;

            Console.WriteLine("Движение товаров успешно изменено.");
        }

        // Добавление товаров
        public static void AddProduct(List<Product> products)
        {
            Console.WriteLine("Занесите данные для нового товара: ");

            uint article = (products.Count > 0) ? products.Max(x => x.Article) + 1 : 1;
            string department, name, unit;
            uint itemsPerPackage, price;

            try
            {
                Console.Write("Введите отдел товара: ");
                department = Console.ReadLine();
                if (string.IsNullOrEmpty(department))
                {
                    throw new Exception();
                }

                Console.Write("Введите название товара: ");
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception();
                }

                Console.Write("Введите единицу измерения товара: ");
                unit = Console.ReadLine();
                if (string.IsNullOrEmpty(unit))
                {
                    throw new Exception();
                }

                Console.Write("Введите кол-во в упаковке товара: ");
                itemsPerPackage = Convert.ToUInt32(Console.ReadLine());
                if (itemsPerPackage == 0)
                {
                    throw new Exception();
                }

                Console.Write("Введите цену за упаковку товара: ");
                price = Convert.ToUInt32(Console.ReadLine());
                if (price == 0)
                {
                    throw new Exception();
                }
            }
            catch
            {
                Console.WriteLine("Неправильно введены данные о товаре.");
                Logger.Log("Добавление товара не произведено");
                return;
            }

            Product product = new Product(article, department, name, unit, itemsPerPackage, price);
            products.Add(product);

            Console.WriteLine("Товар успешно добавлен.");
            Logger.Log($"Добавление товара {product.ToString()} успешно произведено");
        }

        // Добавление магазина
        public static void AddShop(List<Shop> shops)
        {
            Console.WriteLine("Занесите данные о новом магазине: ");

            int shopMaxId = shops
                .Select(shop => Convert.ToInt32(shop.Id.Substring(1)))
                .Max();

            string id = "M" + ((shops.Count > 0) ? shopMaxId + 1 : 1);
            string district, address;
            try
            {
                Console.Write("Введите район магазина: ");
                district = Console.ReadLine();
                if (string.IsNullOrEmpty(district))
                {
                    throw new Exception();
                }

                Console.Write("Введите адрес магазина: ");
                address = Console.ReadLine();
                if (string.IsNullOrEmpty(address))
                {
                    throw new Exception();
                }
            }
            catch
            {
                Console.WriteLine("Неправильно введены данные о магазине.");
                Logger.Log("Добавление магазина не произведено");
                return;
            }

            Shop shop = new Shop(id, district, address);
            shops.Add(shop);

            Console.WriteLine("Магазин успешно добавлен.");
            Logger.Log("Добавление магазина " + shop.ToString() + " успешно произведено");
        }

        // Добавление движения продуктов
        public static void AddProductMovement(
            List<Product> products, 
            List<Shop> shops, 
            List<ProductMovement> productMovements)
        {
            if (products.Count == 0 || shops.Count == 0)
            {
                Console.WriteLine("Нельзя ввести данные о движении товаров без данных о товарах и магазинах");
                Logger.Log("Обновление движения товаров не произведено");
                return;
            }

            Console.WriteLine("Занесите данные о новом движении товаров: ");
            uint operationId = (productMovements.Count > 0) ? productMovements.Max(x => x.OperationId) + 1 : 1;
            DateOnly date;
            string shopId;
            uint productArticle;
            uint packageCount;
            string operationType;

            try
            {
                Console.Write("Введите дату движения товаров (dd.mm.yyyy): ");
                date = DateOnly.Parse(Console.ReadLine());

                Console.Write("Введите ID магазина движения товаров (начинается с 'M' английского алфавита): ");
                shopId = Console.ReadLine();
                if (string.IsNullOrEmpty(shopId) || shops.Find(x => x.Id == shopId) == null)
                {
                    throw new Exception();
                }

                Console.Write("Введите артикул товара движения товаров: ");
                productArticle = Convert.ToUInt32(Console.ReadLine());
                if (products.Find(x => x.Article == productArticle) == null)
                {
                    throw new Exception();
                }

                Console.Write("Введите кол-во упаковок движения товаров: ");
                packageCount = Convert.ToUInt32(Console.ReadLine());
                if (packageCount == 0)
                {
                    throw new Exception();
                }

                Console.Write("Введите тип операции движения товаров (Продажа/Поступление): ");
                string operationTypeString = Console.ReadLine().ToLower().Trim();
                if (string.IsNullOrEmpty(operationTypeString) ||
                    (operationTypeString != "продажа" && operationTypeString != "поступление"))
                {
                    throw new Exception();
                }
                operationType = operationTypeString == "продажа" ? "Продажа" : "Поступление";
            }
            catch
            {
                Console.WriteLine("Неправильно введены данные о движении товаров.");
                Logger.Log("Обновление движения товаров не произведено");
                return;
            }

            ProductMovement productMovement = new ProductMovement(operationId, date, shopId, 
                productArticle, packageCount, operationType);
            productMovements.Add(productMovement);

            Logger.Log("Добавление движения товаров " + productMovement.ToString() + " успешно произведено");
            Console.WriteLine("Движение товаров успешно добавлено.");
        }

        // Нахождение максимальной цены упаковки среди всех продуктов
        // 1 таблица, 1 число
        public static uint GetMaxProductPrice(List<Product> products)
        {
            if (products == null || products.Count == 0)
            {
                return 0;
            }

            return products.Max(p => p.Price);
        }

        // Нахождение списка операций с товарами определённого отдела
        // 2 таблицы, список строк
        public static List<string> GetDepartmentOperations(
            List<Product> products,
            List<ProductMovement> productMovements,
            string department)
        {
            if (products == null || productMovements == null ||
                products.Count == 0 || productMovements.Count == 0) 
            {
                return ["пусто"];
            }

            return (from pm in productMovements
                    join p in products on pm.ProductArticle equals p.Article
                    where p.Department == department
                    select $"[{pm.Date}] {pm.OperationType}: {p.Name} — {pm.PackageCount} уп.")
                    .ToList();
        }

        // Нахождение адресов магазинов, в которых когда-либо продавался товар с названием productName
        // 3 таблицы, список строк
        public static List<string> FindShopAddressesWhereProductWasSold(
            List<Product> products,
            List<Shop> shops, 
            List<ProductMovement> productMovements, 
            string productName)
        {
            if (products == null || shops == null || productMovements == null ||
                products.Count == 0 || shops.Count == 0 || productMovements.Count == 0)
            {
                return ["пусто"];
            }

            return (from pm in productMovements
                    join p in products on pm.ProductArticle equals p.Article
                    join s in shops on pm.ShopId equals s.Id
                    where pm.OperationType == "Продажа" && p.Name == productName
                    select s.Address)
                    .Distinct()
                    .ToList();
        }

        // Нахождение общей суммы продаж
        // 3 таблицы, 1 число
        public static int GetTotalDistrictRevenue(
            List<Product> products, 
            List<Shop> shops, 
            List<ProductMovement> productMovements,
            string shopDistrict)
        {
            if (products == null || shops == null || productMovements == null ||
                products.Count == 0 || shops.Count == 0 || productMovements.Count == 0)
            {
                return 0;
            }

            return (from pm in productMovements
                    join p in products on pm.ProductArticle equals p.Article
                    join s in shops on pm.ShopId equals s.Id
                    where s.District == shopDistrict && pm.OperationType == "Продажа"
                    select (int)p.Price * (int)pm.PackageCount)
                    .Sum();
        }
    }
}

