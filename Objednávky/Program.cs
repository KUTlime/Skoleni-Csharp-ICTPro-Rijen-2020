using System;
using System.Diagnostics;
using System.Transactions;
using System.Xml.Schema;

namespace Objednávky
{
	enum Status
	{
		New,
		Canceled,
		Processed,
		Paid,
		UnPaid,
		Fullfilled
	}

	class ItemName
	{ 
		public string Value { get; }
		public ItemName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Name should not be a empty string.");
			}
			Value = name;
		}
	}

	class UnitPrice
	{
		public double Value { get; }
		public UnitPrice(double unitPrice)
		{
			if (unitPrice < 0 || double.IsNaN(unitPrice))
			{
				throw new ArgumentException("Unit price should be positive number.");
			}
			Value = unitPrice;
		}
	}

	class Quantity
	{
		public double Value { get; }

		public Quantity(double quantity)
		{
			Value = quantity;
		}
	}

	class Item : Entity
	{
		public Item(ItemName name, UnitPrice unitPrice) : this(name, unitPrice, new Quantity(1))
		{
		}
		public Item(ItemName name, UnitPrice unitPrice, Quantity quantity)
		{
			Name = name;
			UnitPrice = unitPrice;
			Quantity = quantity;
		}
		public ItemName Name { get; set; }
		public UnitPrice UnitPrice { get; set; }
		public double TotalPrice { get { return UnitPrice.Value * Quantity.Value; } }
		public Quantity Quantity { get; set; }

		public int MyProperty { get; set; }
		
	}

	class Order : Entity
	{
		private Item[] items;
		public Item[] Items { 
			get 
			{ 
				return items; 
			} 
			set 
			{ 
				items = value;
				TotalAmount = 0;
				foreach (var item in Items)
				{
					TotalAmount += item.TotalPrice;
				}
			} 
		}
		public string Customer { get; set; }
		public DateTime DateTime { get; }
		public DateTime PaymentDate { get; private set;}
		public double PaidAmount {get; private set;}
		public Status Status { get; private set; }
		public double TotalAmount { get; set; }
		
		public Order(string customer, Item[] items)
		{
			Customer = customer;
			Items = items;
			DateTime = DateTime.UtcNow;
			Status = Status.New;
			PaymentDate = DateTime.MinValue;
			PaidAmount = 0;
		}

		public override string ToString()
		{
			return $"Order {Id} with total amount {TotalAmount} for customer {Customer}, order status: {Status}, order payment: {PaidAmount}";
		}

		public void Process()
		{
			Status = Status.Processed;
		}

		public void DeliverOrder()
		{
			Status = Status.UnPaid;
		}

		public void Paid(DateTime paymentDate, double paymentAmount)
		{
			PaymentDate = paymentDate;
			PaidAmount = paymentAmount;
			if (PaidAmount >= TotalAmount)
			{
				Status = Status.Paid;
			}
			
		}
	}

	class Entity
	{
		public Int32 Id { get; } = new Random().Next();
	}

	class Program
	{
		static void Main(string[] args)
		{
			var order = new Order("Radek Zahradník", new Item[] { new Item(new ItemName("Školení C#"), new UnitPrice(1000), new Quantity(5)), new Item(new ItemName("Doprava"), new UnitPrice(3), new Quantity(100)) });
			var order2 = new Order(
				"Radek Zahradník",
				new Item[]
				{
					new Item(new ItemName("Školení C#"), new UnitPrice(1), new Quantity(10)),
					new Item(new ItemName("Doprava"), new UnitPrice(5),new Quantity(500))
				});
			Console.WriteLine(order.ToString());
			Console.WriteLine(order2.ToString());
			order.Items = new Item[] { };
			order.Process();
			order2.DeliverOrder();

			Console.WriteLine(order.ToString());
			Console.WriteLine(order2.ToString());

			order.Paid(DateTime.Now, 100000);
			order2.Paid(DateTime.Now, 1000);

			Console.WriteLine(order.ToString());
			Console.WriteLine(order2.ToString());

		}
	}
}
