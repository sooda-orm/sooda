namespace Sooda.ConsoleApplicationExample
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Data;
    using Sooda;
    using SoodaConsoleApplicationExampleStubs = Sooda.ConsoleApplicationExample.Stubs;
    using System.Linq;

    public class Product : SoodaConsoleApplicationExampleStubs.Product_Stub
    {

        public Product(SoodaConstructor c) :
                base(c)
        {
            // Do not modify this constructor.
        }

        public Product(SoodaTransaction transaction) :
                base(transaction)
        {
            // 
            // TODO: Add construction logic here.
            // 
        }

        public Product() :
                this(SoodaTransaction.ActiveTransaction)
        {
            // Do not modify this constructor.
        }

        public static void Menu(Category category = null)
        {
            var next = true;
            do
            {
                Console.WriteLine(
                    "{0}----- PRODUCT MENU {1}-----{0}", 
                    Environment.NewLine, 
                    category == null ? string.Empty : string.Format("FOR CATEGORY: {0} ", category.Name.ToUpper())
                );
                Console.WriteLine("1.\tShow all items");
                //Console.WriteLine("2.\tAdd item");
                //Console.WriteLine("3.\tDelete item");
                //Console.WriteLine("--\t-----");
                //Console.WriteLine("0.\tBack");

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
                                    Console.WriteLine("Id\tName\t\t{0}Price", category == null ? "category\t\t" : string.Empty);
                                    Console.WriteLine("--\t----\t\t{0}-----", category == null ? "--------\t\t" : string.Empty);
                                    var products = category == null ? Product.AllQuery : Product.AllQuery.Where(it => it.Category == category);
                                    foreach (Product c in products)
                                    {
                                        Console.WriteLine("{0}.\t{1}\t\t{2}{3}", c.Id, c.Name, category == null ? c.Category.Name + "\t\t" : string.Empty, c.UnitPrice);
                                    }
                                    break;
                                }
                            //case 2:
                            //    {
                            //        Console.WriteLine("Create a new category.");
                            //        var name = string.Empty;
                            //        while (string.IsNullOrEmpty(name))
                            //        {
                            //            Console.Write("Name: ");
                            //            name = Console.ReadLine();
                            //        }
                            //        Console.Write("Description: ");
                            //        var description = Console.ReadLine();

                            //        _ = new Category { Name = name, Description = description };
                            //        tran.Commit();
                            //        Console.WriteLine("A new category has been added.");
                            //        break;
                            //    }
                            //case 3:
                            //    {
                            //        Console.WriteLine("Delete category.");
                            //        var text = string.Empty;
                            //        int categoryId;
                            //        do
                            //        {
                            //            Console.Write("Enter the category ID: ");
                            //            text = Console.ReadLine();
                            //        }
                            //        while (!int.TryParse(text, out categoryId));
                            //        var c = Category.AllQuery.FirstOrDefault(it => it.Id == categoryId);
                            //        if (c != null)
                            //        {
                            //            c.MarkForDelete();
                            //            tran.Commit();
                            //        }
                            //        Console.WriteLine("The category has been removed");
                            //        break;
                            //    }
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
