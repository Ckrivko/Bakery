using Bakery.Models.BakedFoods;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables;
using Bakery.Models.Tables.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bakery.Core.Contracts
{
    public class Controller : IController
    {
        private List<IBakedFood> bakedFoods;
        private List<IDrink> drinks;
        private List<ITable> tables;
        private decimal totalIncome = 0;


        public Controller()
        {
            bakedFoods = new List<IBakedFood>();
            drinks = new List<IDrink>();
            tables = new List<ITable>();
        }


        public string AddDrink(string type, string name, int portion, string brand)  //ok ?
        {

            IDrink drink = null;

            if (type == nameof(Tea))
            {
                drink = new Tea(name, portion, brand);
            }
            if (type == nameof(Water))
            {
                drink = new Water(name, portion, brand);
            }

            drinks.Add(drink);
            return $"Added {name} ({brand}) to the drink menu";


        }

        public string AddFood(string type, string name, decimal price)   //ok ?
        {

            IBakedFood food = null;

            if (type == nameof(Bread))
            {
                food = new Bread(name, price);
            }
            if (type == nameof(Cake))
            {
                food = new Cake(name, price);
            }

            bakedFoods.Add(food);
            return $"Added {name} ({type}) to the menu";

        }

        public string AddTable(string type, int tableNumber, int capacity) //ок?
        {

            ITable table = null;

            if (type == nameof(InsideTable))
            {
                table = new InsideTable(tableNumber, capacity);
            }
            if (type == nameof(OutsideTable))
            {
                table = new OutsideTable(tableNumber, capacity);
            }

            tables.Add(table);
            return $"Added table number {tableNumber} in the bakery";

        }

        public string GetFreeTablesInfo() //ok?
        {

            List<ITable> freeTables = tables.Where(x => x.IsReserved == false).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var table in freeTables)
            {
                sb.AppendLine(table.GetFreeTableInfo());
            }

            return sb.ToString().Trim();
        }

        public string GetTotalIncome() // ok?
        {
            //totalIncome += tables.Sum(x => x.GetBill());
            //totalIncome += tables.Sum(x => x.Price);

            return $"Total income: {totalIncome:f2}lv";

        }

        public string LeaveTable(int tableNumber) // ok  ?
        {

            ITable table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);

            decimal currIncome = 0;

            currIncome += table.GetBill();            

            totalIncome += currIncome;
            //?                        
            table.Clear();

            return $"Table: {tableNumber}\r\n" +
                   $"Bill: { currIncome:f2}";

        }

        public string OrderDrink(int tableNumber, string drinkName, string drinkBrand)  // ok ?
        {
            ITable table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);
            IDrink drink = drinks.FirstOrDefault(x => x.Name == drinkName && x.Brand == drinkBrand);


            if (table == null)
            {
                return $"Could not find table {tableNumber}";
            }

            if (drink == null)
            {
                return $"There is no {drinkName} {drinkBrand} available";

            }

            table.OrderDrink(drink);
            return $"Table {tableNumber} ordered {drinkName} {drinkBrand}";

        }

        public string OrderFood(int tableNumber, string foodName) // ok ?
        {
            ITable table = tables.FirstOrDefault(x => x.TableNumber == tableNumber);
            IBakedFood food = bakedFoods.FirstOrDefault(x => x.Name == foodName);

            if (table == null)
            {
                return $"Could not find table {tableNumber}";
            }

            if (food == null)
            {
                return $"No {foodName} in the menu";

            }

            table.OrderFood(food);
            return $"Table {table.TableNumber} ordered {foodName}";

        }

        public string ReserveTable(int numberOfPeople) // ok?
        {

            ITable table = tables.FirstOrDefault(x => x.Capacity >= numberOfPeople && x.IsReserved == false);

            if (table == null)
            {
                return $"No available table for {numberOfPeople} people";
            }

            table.Reserve(numberOfPeople);
            return $"Table {table.TableNumber} has been reserved for {numberOfPeople} people";

        }
    }
}
