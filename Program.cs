using System;
using AmigoWalletDAL;

namespace AmigoWalletConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AmigoWallet");
            AmigoWalletRepositoy repository = new AmigoWalletRepositoy();
            Console.WriteLine("----------------------------------");
            Console.WriteLine("GetCustomerDetails using EmailID");
            var customer1 = repository.getCustomerDetailsByEmailID("krishna@gmail.com");
            foreach (var cus in customer1)
            {
                Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", cus.EmailId, cus.Name, cus.Password,cus.ContactNo);
            }
            Console.WriteLine("----------------------------------");
            Console.WriteLine("AddCustomerDetails\n");
            int returnResult = repository.addCustomerUsingUSP("Krishna","krishna@gmail.com","Krishna@5d2","9603115715");
            if (returnResult > 0)
            {
                Console.WriteLine("Category details added successfully with CategoryId = " );
            }
            else if (returnResult == -1) 
            {
                Console.WriteLine("Contains Customer with same EmailID");
            }
            else
            {
                Console.WriteLine("Error Occured");
            }

            var customers = repository.getAllCustomerDetails();
            Console.WriteLine("----------------------------------");
            Console.WriteLine("CustomerEmailID\tName");
            Console.WriteLine("----------------------------------");
            foreach (var customer in customers)
            {
                Console.WriteLine("{0}\t\t{1}", customer.EmailId, customer.Name);
            }

            returnResult = repository.updateCustomerPasswordUsingUSP("krishna@gmail.com", "papana@5d2");
            if (returnResult > 0)
            {
                Console.WriteLine("Password updated succesfully");
            }
            else if (returnResult == -1)
            {
                Console.WriteLine("Customer detalis doesn't exists");
            }
            else
            {
                Console.WriteLine("Error Occured");
            }

            customers = repository.getAllCustomerDetails();
            Console.WriteLine("----------------------------------");
            Console.WriteLine("CustomerEmailID\tName");
            Console.WriteLine("----------------------------------");
            foreach (var customer in customers)
            {
                Console.WriteLine("{0}\t\t{1}", customer.EmailId, customer.Password);
            }
        }
    }
}
