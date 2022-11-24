using AmigoWalletDAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AmigoWalletDAL
{
    public class AmigoWalletRepositoy
    {
        AmigoWalletDBContext context;
        public AmigoWalletRepositoy()
        {
            context = new AmigoWalletDBContext();
        }

        public List<Customer> getAllCustomerDetails()
        {
            var customerList = (from customer in context.Customer
                                orderby customer.EmailId
                                select customer).ToList();
            return customerList;
        }
        
        public int addCustomerUsingUSP(string Name,string EmailID, string password,string contactNo)
        {
            int result = -1;
            int returnResult = 0;
            SqlParameter prmName = new SqlParameter("@Name", Name);
            SqlParameter prmEmailId = new SqlParameter("@EmailId", EmailID);
            SqlParameter prmPassword = new SqlParameter("@Password", password);
            SqlParameter prmContactNo = new SqlParameter("@ContactNo", contactNo);
            SqlParameter prmReturnResult = new SqlParameter("@ReturnResult", System.Data.SqlDbType.Int);
            prmReturnResult.Direction = System.Data.ParameterDirection.Output;
            try
            {
                result = context.Database.ExecuteSqlRaw("EXEC @ReturnResult = usp_RegisterCustomer @EmailId,@Password,@ContactNo,@Name",
                    prmReturnResult, prmEmailId, prmPassword, prmContactNo, prmName);
                returnResult = Convert.ToInt32(prmReturnResult.Value);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                result = -99;
                returnResult = -99;
            }
            Console.WriteLine(result);
            return returnResult;
        }

        public int updateCustomerPasswordUsingUSP(string EmailID, string password)
        {
            int result = -1;
            int returnResult = 0;
            SqlParameter prmEmailId = new SqlParameter("@EmailId", EmailID);
            SqlParameter prmPassword = new SqlParameter("@Password", password);
            SqlParameter prmReturnResult = new SqlParameter("@ReturnResult", System.Data.SqlDbType.Int);
            prmReturnResult.Direction = System.Data.ParameterDirection.Output;
            try
            {
                result = context.Database.ExecuteSqlRaw("EXEC @ReturnResult = usp_UpdatePassword @EmailId,@Password",
                    prmReturnResult, prmEmailId, prmPassword);
                returnResult = Convert.ToInt32(prmReturnResult.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = -99;
                returnResult = -99;
            }
            Console.WriteLine(result);
            return returnResult;
        }

        public List<Customer> getCustomerDetailsByEmailID(string emailID)
        {
            List<Customer> customerDetails = null;
            Console.WriteLine(emailID);
            SqlParameter prmEmailId = new SqlParameter("@EmailId", emailID);
            try
            {
                customerDetails = context.Customer.FromSqlRaw("SELECT * FROM ufn_GetUserDetails(@EmailId)", prmEmailId
                                                       ).ToList();
                
            }
            catch (Exception ex)
            {
                throw;
            }
            return customerDetails;
        }
    }
}
