//Інтерфейс IProduct і абстрактний клас Product

interface IProduct
{
    string Name { get; set; }
    decimal Price { get; set; }
    decimal CalculateDiscount();
}

abstract class Product : IProduct
{
    public string Name { get; set; }
    public decimal Price
    {
        get { return _price; }
        set
        {
            if (value < 0) throw new ArgumentException("Ціна не може бути негативною.");
            _price = value;
        }
    }

    private decimal _price;

    public Product(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Назва товару не може бути порожньою.");
        Name = name;
        Price = price;
    }

    public abstract decimal CalculateDiscount();
    public abstract decimal CalculateTotalCost();
}


//Класи-нащадки Product (Book, Electronics, Clothing)

class Book : Product
{
    public int PageCount { get; set; }

    public Book(string name, decimal price, int pageCount) : base(name, price)
    {
        PageCount = pageCount;
    }

    public override decimal CalculateDiscount() => Price * 0.10m;

    public override decimal CalculateTotalCost() => Price - CalculateDiscount();
}

class Electronics : Product
{
    public int MemorySize { get; set; }

    public Electronics(string name, decimal price, int memorySize) : base(name, price)
    {
        MemorySize = memorySize;
    }

    public override decimal CalculateDiscount() => Price * 0.15m;

    public override decimal CalculateTotalCost() => Price - CalculateDiscount();
}

class Clothing : Product
{
    public string Size { get; set; }

    public Clothing(string name, decimal price, string size) : base(name, price)
    {
        Size = size;
    }

    public override decimal CalculateDiscount() => Price * 0.05m;

    public override decimal CalculateTotalCost() => Price - CalculateDiscount();
}

//Клас Order

class Order
{
    public int OrderNumber { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public decimal TotalCost => CalculateTotalCost();

    public delegate void OrderStatusChangedHandler(string status);
    public event OrderStatusChangedHandler OnOrderStatusChanged;

    public Order(int orderNumber)
    {
        OrderNumber = orderNumber;
    }

    private decimal CalculateTotalCost()
    {
        decimal total = 0;
        foreach (var product in Products)
        {
            total += product.CalculateTotalCost();
        }
        return total;
    }

    public void ChangeStatus(string status)
    {
        OnOrderStatusChanged?.Invoke(status);
    }
}

//Клас OrderProcessor

class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"Обробка замовлення #{order.OrderNumber}");
        Console.WriteLine($"Загальна вартість: {order.TotalCost} грн");

        order.ChangeStatus("Замовлення оброблено");
    }
}

//Клас NotificationService

class NotificationService
{
    public void SendNotification(string status)
    {
        Console.WriteLine($"Сповіщення: {status}");
    }
}

//Точка входу (клас Main)

class Program
{
    static void Main()
    {
        Product book = new Book("C# для початківців", 250.0m, 400);
        Product laptop = new Electronics("Ноутбук", 15000.0m, 256);
        Product tShirt = new Clothing("Футболка", 300.0m, "M");

        Order order = new Order(1);
        order.Products.Add(book);
        order.Products.Add(laptop);
        order.Products.Add(tShirt);

        NotificationService notificationService = new NotificationService();
        order.OnOrderStatusChanged += notificationService.SendNotification;

        OrderProcessor orderProcessor = new OrderProcessor();
        orderProcessor.ProcessOrder(order);
    }
}