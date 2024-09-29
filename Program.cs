using System;
using BookstoreDAL;
using BookstoreDAL.Models;

namespace BookstoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var unitOfWork = new UnitOfWork(new BookstoreContext());
            if (!Login(unitOfWork))
            {
                Console.WriteLine("Invalid login. Exiting the application.");
                return;
            }
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n1. Add Book");
                Console.WriteLine("2. Delete Book");
                Console.WriteLine("3. Edit Book");
                Console.WriteLine("4. Search Books");
                Console.WriteLine("5. Sell Book");
                Console.WriteLine("6. Display New Arrivals");
                Console.WriteLine("7. Display Popular Books");
                Console.WriteLine("8. Create User");
                Console.WriteLine("9. Exit");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddBook(unitOfWork);
                        break;
                    case "2":
                        DeleteBook(unitOfWork);
                        break;
                    case "3":
                        EditBook(unitOfWork);
                        break;
                    case "4":
                        SearchBooks(unitOfWork);
                        break;
                    case "5":
                        SellBook(unitOfWork);
                        break;
                    case "6":
                        DisplayNewArrivals(unitOfWork);
                        break;
                    case "7":
                        DisplayPopularBooks(unitOfWork);
                        break;
                    case "8":
                        CreateUser(unitOfWork);
                        break;
                    case "9":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static bool Login(UnitOfWork unitOfWork)
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            var user = unitOfWork.Users.GetByUsername(username);

            return user != null && user.Password == password;
        }
        static void CreateUser(UnitOfWork unitOfWork)
        {
            var user = new User();
            Console.WriteLine("Enter username:");
            user.Username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            user.Password = Console.ReadLine();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();
            Console.WriteLine("User created successfully!");
        }
        static void AddBook(UnitOfWork unitOfWork)
        {
            var book = new Book();
            Console.WriteLine("Enter title:");
            book.Title = Console.ReadLine();
            Console.WriteLine("Enter author:");
            book.Author = Console.ReadLine();
            Console.WriteLine("Enter publisher:");
            book.Publisher = Console.ReadLine();
            Console.WriteLine("Enter number of pages:");
            book.Pages = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter genre:");
            book.Genre = Console.ReadLine();
            Console.WriteLine("Enter year of publication:");
            book.Year = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter cost price:");
            book.CostPrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter sale price:");
            book.SalePrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Is this a continuation? (true/false):");
            book.IsContinuation = bool.Parse(Console.ReadLine());
            unitOfWork.Books.Add(book);
            unitOfWork.Complete();
            Console.WriteLine("Book added successfully!");
        }
        static void DeleteBook(UnitOfWork unitOfWork)
        {
            Console.WriteLine("Enter book ID to delete:");
            int id = int.Parse(Console.ReadLine());
            unitOfWork.Books.Delete(id);
            unitOfWork.Complete();
            Console.WriteLine("Book deleted successfully!");
        }
        static void EditBook(UnitOfWork unitOfWork)
        {
            Console.WriteLine("Enter book ID to edit:");
            int id = int.Parse(Console.ReadLine());
            var book = unitOfWork.Books.GetById(id);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }
            Console.WriteLine("Enter new title:");
            book.Title = Console.ReadLine();
            Console.WriteLine("Enter new author:");
            book.Author = Console.ReadLine();
            Console.WriteLine("Enter new publisher:");
            book.Publisher = Console.ReadLine();
            Console.WriteLine("Enter new number of pages:");
            book.Pages = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new genre:");
            book.Genre = Console.ReadLine();
            Console.WriteLine("Enter new year of publication:");
            book.Year = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter new cost price:");
            book.CostPrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter new sale price:");
            book.SalePrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Is this a continuation? (true/false):");
            book.IsContinuation = bool.Parse(Console.ReadLine());
            unitOfWork.Books.Update(book);
            unitOfWork.Complete();
            Console.WriteLine("Book updated successfully!");
        }
        static void SearchBooks(UnitOfWork unitOfWork)
        {
            Console.WriteLine("Enter search term (title, author, genre):");
            string searchTerm = Console.ReadLine();
            var books = unitOfWork.Books.GetAll()
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm) || b.Genre.Contains(searchTerm))
                .ToList();
            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
            }
            else
            {
                Console.WriteLine("Search Results:");
                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}");
                }
            }
        }
        static void SellBook(UnitOfWork unitOfWork)
        {
            Console.WriteLine("Enter book ID to sell:");
            int id = int.Parse(Console.ReadLine());
            var book = unitOfWork.Books.GetById(id);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }
            Console.WriteLine($"Book {book.Title} sold for {book.SalePrice}!");
            unitOfWork.Books.Delete(id);
            unitOfWork.Complete();
        }
        static void DisplayNewArrivals(UnitOfWork unitOfWork)
        {
            var newArrivals = unitOfWork.Books.GetAll()
                .OrderByDescending(b => b.Year)
                .Take(5)
                .ToList();
            Console.WriteLine("New Arrivals:");
            foreach (var book in newArrivals)
            {
                Console.WriteLine($"{book.Title} by {book.Author} ({book.Year})");
            }
        }
        static void DisplayPopularBooks(UnitOfWork unitOfWork)
        {
            Console.WriteLine("Popular Books:");
            var popularBooks = unitOfWork.Books.GetAll()
                .Take(5)
                .ToList();
            foreach (var book in popularBooks)
            {
                Console.WriteLine($"{book.Title} by {book.Author}");
            }
        }
    }
}