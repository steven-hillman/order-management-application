using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steven_Hillman_CPRG200_Lab4
{
    public static class OrdersDB
    {
        // gets one order based on the OrderID given
        public static Orders GetOrderByID(string OrderID)
        {
            Orders order = null; // Orders object to represent the order that is getting returned based on the ID
            using(SqlConnection connection = NorthwindDB.GetConnection()) // creates the SqlConnection with method
            {
                string query = "SELECT OrderID, CustomerID, OrderDate, RequiredDate, ShippedDate " +
                               "FROM Orders " +
                               "WHERE OrderID = @OrderID"; // query with parameter
                using(SqlCommand cmd = new SqlCommand(query, connection)) // uses SqlCommand with query and connection
                {
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    connection.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow)) // initializes reader
                    {
                        if(reader.Read()) // if there is data fill in the values for the order object
                        {
                            order = new Orders();
                            order.OrderID = (int)reader["OrderID"];
                            order.CustomerID = (string)reader["CustomerID"];

                            // check for nulls
                            int col = reader.GetOrdinal("OrderDate");
                            if (reader.IsDBNull(col)) // if reader contains DBNull in this column
                                order.OrderDate = null; // make it null in the object
                            else // it is not null
                                order.OrderDate = Convert.ToDateTime(reader["OrderDate"]);

                            int col2 = reader.GetOrdinal("RequiredDate");
                            if (reader.IsDBNull(col2))
                                order.RequiredDate = null;
                            else
                                order.RequiredDate = Convert.ToDateTime(reader["RequiredDate"]);

                            int col3 = reader.GetOrdinal("ShippedDate");
                            if (reader.IsDBNull(col3))
                                order.ShippedDate = null;
                            else
                                order.ShippedDate = Convert.ToDateTime(reader["OrderDate"]);

                        }
                        return order; // return the order object
                    }
                }
            }
        } // end of GetOrdersByID method

        // get a list of order objects from the database
        public static List<Orders> GetOrdersList()
        {
            List<Orders> ordersList = new List<Orders>(); // empty list of Orders objects
            Orders temp = null;
            using (SqlConnection connection = NorthwindDB.GetConnection()) // creates the connection
            {               
                string query = "SELECT OrderID, CustomerID, OrderDate, RequiredDate, ShippedDate " +
                               "FROM Orders ";
                               //"WHERE OrderID = @OrderID"; // query with parameter
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    // close connection when finished reading
                    while (reader.Read())
                    {
                        temp = new Orders();
                        temp.OrderID = (int)reader["OrderID"];
                        temp.CustomerID = (string)reader["CustomerID"];
                        
                        // check for nulls
                        int col = reader.GetOrdinal("OrderDate");
                        if (reader.IsDBNull(col)) // if reader contains DBNull in this column
                            temp.OrderDate = null; // make it null in the object
                        else // if not null
                            temp.OrderDate = Convert.ToDateTime(reader["OrderDate"]);

                        int col2 = reader.GetOrdinal("RequiredDate");
                        if (reader.IsDBNull(col2))
                            temp.RequiredDate = null;
                        else
                            temp.RequiredDate = Convert.ToDateTime(reader["RequiredDate"]);

                        int col3 = reader.GetOrdinal("ShippedDate");
                        if (reader.IsDBNull(col3))
                            temp.ShippedDate = null;
                        else
                            temp.ShippedDate = Convert.ToDateTime(reader["OrderDate"]);

                        ordersList.Add(temp); // add the order object to the list of order objects
                    }
                } // command object recycled             
            } // connection object recycled
            return ordersList; // return the list of order objects
        } // end of GetOrdersList

        // used to update shipped date of the given order and allows for nulls
        public static void UpdateShippedDate(Orders order)
        {
            using (SqlConnection connection = NorthwindDB.GetConnection()) // create connection
            {
                connection.Open();
                        
                string updateStatement =
                    "UPDATE Orders SET " +
                    "ShippedDate = @ShippedDate " +
                    "WHERE OrderID = @OrderID "; // query with parameter       
              
                using (SqlCommand cmd = new SqlCommand(updateStatement, connection))
                {
                    cmd.Parameters.AddWithValue("@ShippedDate", (object)order.ShippedDate ?? DBNull.Value); // allow for null
                    cmd.Parameters.AddWithValue("@OrderID", order.OrderID);               
                    cmd.ExecuteNonQuery();
                }              
            }
        } // end of UpdateShippedDate method
    } // end of class
} // end of namespace
