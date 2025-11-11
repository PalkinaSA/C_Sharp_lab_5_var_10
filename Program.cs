// Лабораторная работа №5. Вариант 10.
using ExcelDataReader;
using ConsoleTables;
using C_Sharp_var_10_lab_5;

public class Program
{
    public static void Main(string[] args)
    {
        List<Product> products = new List<Product>();
        List<Shop> shops = new List<Shop>();
        List<ProductMovement> productMovements = new List<ProductMovement>();

        Console.WriteLine("Лабораторная работа №5. Вариант 10.");
        Console.WriteLine("Добро пожаловать в приложение Продукты!");
        Console.WriteLine();

        // Настройка логирования
        Console.WriteLine("Выберите режим логирования:");
        Console.WriteLine("1. Перезаписать/создать новый файл");
        Console.WriteLine("2. Дописывать в существующий файл");

        Constants.LogChoice logChoice = Constants.LogChoice.None;
        do
        {
            Console.Write("Введите номер действия: ");
            switch (Console.ReadLine())
            {
                case "1": 
                    {
                        logChoice = Constants.LogChoice.OverwriteOrCreate;
                        Logger.Init(true);
                        Console.WriteLine("Создан/перезаписан файл логирования");
                        Logger.Log("Создан/перезаписан файл логирования");
                        Logger.Log("Начало сеанса");
                        break;
                    }
                case "2":
                    {
                        logChoice = Constants.LogChoice.Append;
                        if (!File.Exists(Constants.logPath)) goto case "1";
                        else
                        {
                            Console.WriteLine("Запись в уже существующий файл логирования");
                            Logger.Log("Начало сеанса");
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Данной опции не существует");
                        break;
                    }
            }
            Console.WriteLine();
        } while (logChoice == Constants.LogChoice.None);

        // Конец настройки логирования

        while (true)
        {
            Console.WriteLine("ДЕЙСТВИЯ С БАЗОЙ ДАННЫХ:");
            Console.WriteLine("==========================================================================");
            Console.WriteLine("1. Чтение базы данных из excel файла.");
            Console.WriteLine("2. Просмотр базы данных.");
            Console.WriteLine("3. Удаление элементов (по ключу).");
            Console.WriteLine("4. Корректировка элементов (по ключу).");
            Console.WriteLine("5. Добавление элементов.");
            Console.WriteLine("6. Специальные запросы.");
            Console.WriteLine();
            Console.WriteLine("0. Выход из программы.");
            Console.WriteLine("==========================================================================");
            Console.WriteLine();
            Console.Write("Введите номер действия: ");

            string userChoice = Console.ReadLine();

            Console.WriteLine();

            if (userChoice == "0") break;

            switch(userChoice)
            {
                case "1":
                    {
                        Console.WriteLine("ЧТЕНИЕ БАЗЫ ДАННЫХ.");
                        DatabaseController.ReadDatabase(products, shops, productMovements);
                        Console.WriteLine();
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("ПРОСМОТР БАЗЫ ДАННЫХ:");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine("1. Просмотр таблицы 'Товар'.");
                        Console.WriteLine("2. Просмотр таблицы 'Магазин'.");
                        Console.WriteLine("3. Просмотр таблицы 'Движение товаров'.");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine();
                        Console.Write("Введите номер действия: ");

                        switch (Console.ReadLine())
                        {
                            case "1":
                                {
                                    DatabaseController.PrintProducts(products);
                                    break;
                                }
                            case "2":
                                {
                                    DatabaseController.PrintShops(shops);
                                    break;
                                }
                            case "3":
                                {
                                    DatabaseController.PrintProductMovements(productMovements);
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Данной опции не существует.");
                                    Logger.Log("Введена несуществующая опция");
                                    break;
                                }
                        }
                        Console.WriteLine();
                        break;
                    }
                case "3":
                    {
                        Console.WriteLine("УДАЛЕНИЕ ЭЛЕМЕНТОВ (ПО КЛЮЧУ):");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine("1. Удаление товара (каскадное).");
                        Console.WriteLine("2. Удаление магазина (каскадное).");
                        Console.WriteLine("3. Удаление движения товара.");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine();
                        Console.Write("Введите номер действия: ");

                        switch(Console.ReadLine())
                        {
                            case "1":
                                {
                                    Console.Write("Введите ключ: ");
                                    try
                                    {
                                        uint key = Convert.ToUInt32(Console.ReadLine());
                                        if (key == 0)
                                        {
                                            throw new Exception();
                                        }

                                        DatabaseController.DeleteProduct(products, productMovements, key);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Некорректно введён ключ.");
                                        Logger.Log("Некорректно введён ключ для операции удаления товара");
                                    }
                                    break;
                                }
                            case "2":
                                {
                                    Console.Write("Введите ключ (начинается с 'M' английского алфавита): ");
                                    try
                                    {
                                        string key = Console.ReadLine();
                                        if (string.IsNullOrEmpty(key))
                                        {
                                            throw new Exception();
                                        }

                                        DatabaseController.DeleteShop(shops, productMovements, key);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Некорректно введён ключ.");
                                        Logger.Log("Некорректно введён ключ для операции удаления магазина");
                                    }
                                    break;
                                }
                            case "3":
                                {
                                    Console.Write("Введите ключ: ");
                                    try
                                    {
                                        uint key = Convert.ToUInt32(Console.ReadLine());
                                        if (key == 0)
                                        {
                                            throw new Exception();
                                        }

                                        DatabaseController.DeleteProductMovement(productMovements, key);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Некорректно введён ключ.");
                                        Logger.Log("Некорректно введён ключ для операции удаления движения товаров");
                                    }
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Данной опции не существует.");
                                    Logger.Log("Введена несуществующая опция");
                                    break;
                                }
                        }
                        Console.WriteLine();
                        break;
                    }
                case "4":
                    {
                        Console.WriteLine("КОРРЕКТИРОВКА ЭЛЕМЕНТОВ (ПО КЛЮЧУ):");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine("1. Обновление товара.");
                        Console.WriteLine("2. Обновление магазина.");
                        Console.WriteLine("3. Обновление движения товара.");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine();
                        Console.Write("Введите номер действия: ");

                        switch (Console.ReadLine())
                        {
                            case "1":
                                {
                                    Console.Write("Введите ключ: ");
                                    try
                                    {
                                        uint key = Convert.ToUInt32(Console.ReadLine());
                                        if (key == 0)
                                        {
                                            throw new Exception();
                                        }

                                        DatabaseController.UpdateProduct(products, key);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Некорректно введён ключ.");
                                        Logger.Log("Некорректно введён ключ для операции обновления товара");
                                    }
                                    break;
                                }
                            case "2":
                                {
                                    Console.Write("Введите ключ (начинается с 'M' английского алфавита): ");
                                    try
                                    {
                                        string key = Console.ReadLine();
                                        if (string.IsNullOrEmpty(key))
                                        {
                                            throw new Exception();
                                        }

                                        DatabaseController.UpdateShop(shops, key);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Некорректно введён ключ.");
                                        Logger.Log("Некорректно введён ключ для операции обновления магазина");
                                    }
                                    break;
                                }
                            case "3":
                                {
                                    Console.Write("Введите ключ: ");
                                    try
                                    {
                                        uint key = Convert.ToUInt32(Console.ReadLine());
                                        if (key == 0)
                                        {
                                            throw new Exception();
                                        }

                                        DatabaseController.UpdateProductMovement(products, shops, productMovements, key);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Некорректно введён ключ.");
                                        Logger.Log("Некорректно введён ключ для операции обновления движения товаров");
                                    }
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Данной опции не существует.");
                                    Logger.Log("Введена несуществующая опция");
                                    break;
                                }
                        }
                        Console.WriteLine();
                        break;
                    }
                case "5": 
                    {
                        Console.WriteLine("ДОБАВЛЕНИЕ ЭЛЕМЕНТОВ:");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine("1. Добавление товара.");
                        Console.WriteLine("2. Добавление магазина.");
                        Console.WriteLine("3. Добавление движения товара.");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine(); 
                        Console.Write("Введите номер действия: ");

                        switch (Console.ReadLine())
                        {
                            case "1":
                                {
                                    DatabaseController.AddProduct(products);
                                    break;
                                }
                            case "2":
                                {
                                    DatabaseController.AddShop(shops);
                                    break;
                                }
                            case "3":
                                {
                                    DatabaseController.AddProductMovement(products, shops, productMovements);
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Данной опции не существует.");
                                    Logger.Log("Введена несуществующая опция");
                                    break;
                                }
                        }
                        Console.WriteLine();
                        break;
                    }
                case "6":
                    {
                        Console.WriteLine("СПЕЦИАЛЬНЫЕ ЗАПРОСЫ:");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine("1. Найти максимальную цену упаковки среди всех продуктов");
                        Console.WriteLine("2. Найти список операций с товарами определённого отдела");
                        Console.WriteLine("3. Найти список адресов магазинов, в которых продавался определённый товар");
                        Console.WriteLine("4. Найти общую сумму продаж в определённом районе");
                        Console.WriteLine("==========================================================================");
                        Console.WriteLine();
                        Console.Write("Введите номер действия: ");

                        switch (Console.ReadLine())
                        {
                            case "1":
                                {
                                    // Найти максимальную цену упаковки среди всех продуктов
                                    // 1 таблица, 1 число

                                    uint maxProductPrice = DatabaseController.GetMaxProductPrice(products);

                                    // Вывод на экран
                                    Console.WriteLine($"Максимальная цена упаковки равна {maxProductPrice}.");

                                    // Логирование
                                    Logger.Log($"Максимальная цена упаковки равная {maxProductPrice} выведена на экран");
                                    break;
                                }
                            case "2":
                                {
                                    // Найти список операций с товарами определённого отдела
                                    // 2 таблицы, список строк

                                    string department;
                                    Console.Write("Введите название отдела: ");
                                    try
                                    {
                                        department = Console.ReadLine();
                                        if (string.IsNullOrEmpty(department))
                                        {
                                            throw new Exception();
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Неправильно введено название отдела.");
                                        Logger.Log("Список операций не выведен на экран");
                                        break;
                                    }

                                    // Получение списка операций
                                    List<string> operations = DatabaseController.GetDepartmentOperations(
                                        products, productMovements, department);

                                    // Вывод списка на экран
                                    Console.WriteLine($"Список операций по отделу {department}:");
                                    foreach (string operation in operations) 
                                    {
                                        Console.WriteLine(operation);
                                    }

                                    // Логирование
                                    Logger.Log($"Список операций по отделу {department} выведен на экран");
                                    break;
                                }
                            case "3":
                                {
                                    // Найти адреса магазинов, в которых продавался определённый товар
                                    // 3 таблицы, список строк

                                    string productName;
                                    Console.Write("Введите название товара: ");
                                    try
                                    {
                                        productName = Console.ReadLine();
                                        if (string.IsNullOrEmpty(productName)) 
                                        {
                                            throw new Exception();
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Неправильно введено название товара.");
                                        Logger.Log("Список адресов не выведен на экран");
                                        break;
                                    }

                                    // Получение списка адресов
                                    List<string> addresses = DatabaseController
                                            .FindShopAddressesWhereProductWasSold(
                                            products, shops, productMovements, productName);

                                    // Вывод на экран
                                    Console.WriteLine($"Список адресов магазинов, в которых " +
                                        $"продавался товар {productName}: ");
                                    foreach (string address in addresses)
                                    {
                                        Console.WriteLine(address);
                                    }

                                    // Логирование
                                    Logger.Log($"Список адресов магазинов, в которых " +
                                        $"продавался товар {productName} выведен на экран");
                                    break;
                                }
                            case "4":
                                {
                                    // Найти общую сумму продаж в определённом районе
                                    // 3 таблица, 1 число

                                    string district;
                                    Console.Write("Введите название района: ");
                                    try
                                    {
                                        district = Console.ReadLine();
                                        if (string.IsNullOrEmpty(district)) 
                                        { 
                                            throw new Exception();
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Неверно введены данные о районе.");
                                        Logger.Log("Общая сумма продаж не выведена на экран");
                                        break;
                                    }

                                    // Расчёт суммы
                                    int sum = DatabaseController.GetTotalDistrictRevenue(
                                        products, shops, productMovements, district);

                                    // Вывод на экран
                                    Console.WriteLine($"Общая сумма продаж равна {sum}.");

                                    // Логирование
                                    Logger.Log($"Общая сумма продаж равная {sum} выведена на экран");
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Данной опции не существует.");
                                    Logger.Log("Введена несуществующая опция");
                                    break;
                                }
                        }
                        Console.WriteLine();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Данной опции не существует.");
                        Logger.Log("Введена несуществующая опция");
                        break;
                    }
            }
        }
        Console.WriteLine("Конец сеанса");
        Logger.Log("Конец сеанса");
    }
}