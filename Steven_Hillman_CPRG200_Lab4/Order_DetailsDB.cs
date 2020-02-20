using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven_Hillman_CPRG200_Lab4
{
    public static class Order_DetailsDB
    {
        // gets a list of order_details objects from the database from only the given order id
        public static List<Order_Details> GetDetailsList(string OrderID)
        {
            List<Order_Details> detailsList = new List<Order_Details>(); // empty list of Order_Details objects
            Order_Details temp = null; // used for adding the details objects in the while loop below
            using (SqlConnection connection = NorthwindDB.GetConnection()) // creates the connection
            {
                string query = "SELECT OrderID, ProductID, UnitPrice, Quantity, Discount " +
                               "FROM [Order Details] " +
                               "WHERE OrderID = " + OrderID; // query with parameter
                using (SqlCommand cmd = new SqlCommand(query, connection)) // create SqlCommand object with query and connection
                {
                    connection.Open(); // connect
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close connection when finished reading
                    while (reader.Read()) // while there is still data to be read
                    {
                        temp = new Order_Details();
                        temp.OrderID = (int)reader["OrderID"];
                        temp.ProductID = (int)reader["ProductID"];
                        temp.UnitPrice = (decimal)reader["UnitPrice"];
                        temp.Quantity = Convert.ToInt32(reader["Quantity"]);
                        temp.Discount = Convert.ToDecimal(reader["Discount"]);
                        temp.LineTotal = temp.UnitPrice * (1 - temp.Discount) * temp.Quantity; // calculate the total for each line                
                        detailsList.Add(temp); // add the details object to the details list
                    }
                } // command object recycled             
            } // connection object recycled
            return detailsList;
        } // end of GetDetailsList method

        // get a list of all details objects
        public static List<Order_Details> GetDetailsListNoID()
        {
            List<Order_Details> detailsList = new List<Order_Details>(); // empty list of Order_Details objects
            Order_Details temp = null; // used for creating the details objects in the while loop to add to the details list
            using (SqlConnection connection = NorthwindDB.GetConnection()) // creates the connection
            {
                string query = "SELECT OrderID, ProductID, UnitPrice, Quantity, Discount " +
                               "FROM [Order Details] ";
                               //"WHERE OrderID = " + OrderID; // query with parameter
                using (SqlCommand cmd = new SqlCommand(query, connection)) // create SqlCommand object with query and connection
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close connection when finished reading                   
                    while (reader.Read()) // while there is still data to be read
                    {
                        temp = new Order_Details();
                        temp.OrderID = (int)reader["OrderID"];
                        temp.ProductID = (int)reader["ProductID"];
                        temp.UnitPrice = (decimal)reader["UnitPrice"];
                        temp.Quantity = Convert.ToInt32(reader["Quantity"]);
                        temp.Discount = Convert.ToDecimal(reader["Discount"]);
                        detailsList.Add(temp); // add details objects to the details list
                    }
                } // command object recycled             
            } // connection object recycled
            return detailsList; // return the list of details
        } // end of GetDetailsList method
    } // end of Order_DetailsDB class
} // end of namespace
