using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class CustomerDAOMSSQL : ICustomerDAO
    {
        public static SqlCommand cmd = new SqlCommand();
        private static object key = new object();
        public long ADD(Customer t)
        {
            SqlCommand cmd2 = new SqlCommand();
            
            lock (key)
            {

                {
                    using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd.Connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"SELECT COUNT(*) FROM Customers WHERE USER_NAME = '{t.UserName}' OR EXISTS (SELECT USER_NAME FROM AirlineCompanies WHERE USER_NAME = '{t.UserName}')";
                        string res = cmd.ExecuteScalar().ToString();
                        if (res != "0")
                            throw new AlreadyExistException($"Customers user name {t.UserName} already exists");
                        cmd.Connection.Close();

                    }
                    using (cmd2.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd2.Connection.Open();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = $"INSERT INTO Customers(FIRST_NAME, LAST_NAME, USER_NAME, PASSWORD, ADDRESS, PHONE_NUMBER, CREDIT_CARD_NUMBER )" +
                        $"values('{ t.FirstName}', '{ t.LastName}', '{ t.UserName}', '{ t.Password}','{ t.Address}', '{ t.PhoneNumber}', '{ t.CreditCardNumber}');"+
                        $"SELECT ID FROM Customers WHERE USER_NAME = '{t.UserName}'";
          
                        t.ID = (long)cmd2.ExecuteScalar();
                    }

                    return t.ID;
                }
            }
        }

        public Customer Get(long id)
        {
            Customer customer = new Customer();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Customers WHERE(ID = {id})";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Customer a = new Customer
                        {
                            ID = (long)reader["ID"],
                            FirstName = (string)reader["FIRST_NAME"],
                            LastName = (string)reader["LAST_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            Address = (string)reader["ADDRESS"],
                            PhoneNumber = (string)reader["PHONE_NUMBER"],
                            CreditCardNumber = (string)reader["CREDIT_CARD_NUMBER"]

                        };


                        customer = a;
                    }
                }
                return customer;
            }
        }

        public IList<Customer> GetAll()
        {
            IList<Customer> customers = new List<Customer>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Csutomres";

                using (SqlDataReader reader = cmd.ExecuteReader())

                {
                    while (reader.Read())
                    {

                        Customer a = new Customer
                        {
                            ID = (long)reader["ID"],
                            FirstName = (string)reader["FIRST_NAME"],
                            LastName = (string)reader["LAST_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            Address = (string)reader["ADDRESS"],
                            PhoneNumber = (string)reader["PHONE_NUMBER"],
                            CreditCardNumber = (string)reader["CREDIT_CARD_NUMBER"]

                        };


                        customers.Add(a);
                    }
                }
                return customers;
            }
        }

        public Customer GetCustomerByUserName(string username)
        {
            Customer c = null;
            c = null;
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Customers WHERE (USER_NAME = '{username}')";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Customer a = new Customer
                        {

                            ID = (long)reader["ID"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            FirstName = (string)reader["FIRST_NAME"],
                            LastName = (string)reader["LAST_NAME"],
                            Address = (string)reader["ADDRESS"],
                            PhoneNumber = (string)reader["PHONE_NUMBER"],
                            CreditCardNumber = (string)reader["CREDIT_CARD_NUMBER"]
                        };
                        c = a;
                    }
                    
                }
            }
            if(c == null)
            {
                return null;
            }
            return c;
      
        }
        

        public void Remove(Customer t)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"DELETE FROM Tickets WHERE Tickets.CUSTOMER_ID = {t.ID};" +
                    $"DELETE FROM Customers WHERE (ID = {t.ID} );";

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Customer t)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"UPDATE Customers SET FIRST_NAME = '{t.FirstName}',LAST_NAME = '{t.LastName}',USER_NAME = '{t.UserName}',PASSWORD = '{t.Password}',ADDRESS = '{t.Address}',PHONE_NUMBER = '{t.PhoneNumber}',CREDIT_CARD_NUMBER = '{t.CreditCardNumber}' ";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
