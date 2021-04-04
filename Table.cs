using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bakery.Models.Tables
{
    public abstract class Table : ITable
    {
        protected List<IBakedFood> foodOrders;
        protected List<IDrink> drinkOrders;

        private int tableNumber;
        private int capacity;
        private int numberOfPeople;
        private decimal pricePerPerson;
        private bool isReserved;


        public Table(int tableNumber, int capacity, decimal pricePerPerson)
        {
            this.TableNumber = tableNumber;
            this.Capacity = capacity;
            this.PricePerPerson = pricePerPerson;

            foodOrders = new List<IBakedFood>();
            drinkOrders = new List<IDrink>();

        }


        //Data
        public int TableNumber
        {
            get => this.tableNumber;

            private set
            {
                this.tableNumber = value;
            }

        }

        public int Capacity
        {
            get => this.capacity;

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Capacity has to be greater than 0");

                }

                this.capacity = value;
            }

        }

        public int NumberOfPeople
        {
            get => this.numberOfPeople;

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cannot place zero or less people!");

                }

                this.numberOfPeople = value;
            }

        }

        public decimal PricePerPerson
        {
            get => this.pricePerPerson;
            private set
            {
                this.pricePerPerson = value;
            }
        }

        public bool IsReserved
        {
            get => this.isReserved;
            private set
            {
                this.isReserved = false;
            }
        }

        public decimal Price
        {
            get => this.numberOfPeople * this.pricePerPerson;

        }


        //Behavior

        public void Clear()
        {
            foodOrders.Clear();
            drinkOrders.Clear();
            this.IsReserved = false;
            this.numberOfPeople = 0;
        }

        public decimal GetBill()
        {
            decimal totalSum = 0;

            //totalSum += foodOrders.Sum(x => x.Price);
            //totalSum += drinkOrders.Sum(x => x.Price);

            foreach (var food in foodOrders)
            {
                totalSum += food.Price;
            }
            foreach (var drink in drinkOrders)
            {
                totalSum += drink.Price;
            }

            totalSum += Price;
            return totalSum;
        }

        public string GetFreeTableInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Table: {this.TableNumber}");
            sb.AppendLine($"Type: {GetType().Name}");
            sb.AppendLine($"Capacity: {this.Capacity}");
            sb.AppendLine($"Price per Person: {this.PricePerPerson}");

            return sb.ToString().Trim();

        }

        public void OrderDrink(IDrink drink)
        {
            this.drinkOrders.Add(drink);
        }

        public void OrderFood(IBakedFood food)
        {
            this.foodOrders.Add(food);
        }

        public void Reserve(int numberOfPeople)
        {

            this.isReserved = true;

            this.numberOfPeople = numberOfPeople;

        }
    }
}
