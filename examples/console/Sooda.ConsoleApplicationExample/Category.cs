namespace Sooda.ConsoleApplicationExample
{
    using System;
    using Sooda;
    using SoodaConsoleApplicationExampleStubs = Sooda.ConsoleApplicationExample.Stubs;
    using System.Linq;

    public class Category : SoodaConsoleApplicationExampleStubs.Category_Stub
    {

        public Category(SoodaConstructor c) :
                base(c)
        {
            // Do not modify this constructor.
        }

        public Category(SoodaTransaction transaction) :
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }

        public Category() :
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }

        public static void Menu()
        {
            var next = true;
            do
            {
                Console.WriteLine("{0}----- CATEGORY MENU -----{0}", Environment.NewLine);
                Console.WriteLine("1.\tShow all items");
                Console.WriteLine("2.\tAdd item");
                Console.WriteLine("3.\tDelete item");
                Console.WriteLine("4.\tShow products in category");
                Console.WriteLine("--\t-----");
                Console.WriteLine("0.\tBack");

                Console.Write(Environment.NewLine + "Choose option: ");
                var number = Console.ReadLine();
                Console.WriteLine();
                if (int.TryParse(number, out int option))
                {
                    using (var tran = new SoodaTransaction())
                    {
                        switch (option)
                        {
                            case 0:
                                next = false;
                                break;
                            case 1:
                                {
                                    Console.WriteLine("Id\tName\t\tDescription");
                                    Console.WriteLine("--\t----\t\t-----------");
                                    foreach (Category c in Category.AllQuery)
                                    {
                                        Console.WriteLine("{0}.\t{1}\t\t{2}", c.Id, c.Name, c.Description.IsNull ? string.Empty : c.Description.Value);
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    var name = string.Empty;
                                    while (string.IsNullOrEmpty(name))
                                    {
                                        Console.Write("Name: ");
                                        name = Console.ReadLine();
                                    }
                                    Console.Write("Description: ");
                                    var description = Console.ReadLine();

                                    _ = new Category { Name = name, Description = description };
                                    tran.Commit();
                                    Console.WriteLine("A new category has been added.");
                                    break;
                                }
                            case 3:
                                {
                                    var text = string.Empty;
                                    int categoryId;
                                    do
                                    {
                                        Console.Write("Enter the category ID: ");
                                        text = Console.ReadLine();
                                    }
                                    while (!int.TryParse(text, out categoryId));
                                    var c = Category.AllQuery.FirstOrDefault(it => it.Id == categoryId);
                                    if (c != null)
                                    {
                                        c.MarkForDelete();
                                        tran.Commit();
                                    }
                                    Console.WriteLine("The category has been removed");
                                    break;
                                }
                            case 4:
                                {
                                    var text = string.Empty;
                                    int categoryId;
                                    do
                                    {
                                        Console.Write("Enter the category ID: ");
                                        text = Console.ReadLine();
                                    }
                                    while (!int.TryParse(text, out categoryId));
                                    var c = Category.AllQuery.FirstOrDefault(it => it.Id == categoryId);
                                    if (c != null)
                                    {
                                        Product.Menu(c);
                                    }
                                    break;
                                }
                            default:
                                Console.WriteLine("Invalid operation");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid operation");
                }
                Console.ReadLine();
            } while (next);
        }
    }
}
