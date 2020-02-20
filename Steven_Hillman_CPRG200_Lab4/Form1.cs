using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// allows for display of orders and order details and editing of shipped data entries from the northwind database using ado.net
// January 2020
// Author: Steven Hillman
// SAIT OOSD Project 4

namespace Steven_Hillman_CPRG200_Lab4
{
    public partial class Form1 : Form
    {
        // lists that contain sql data
        private List<Orders> orderList { get; set; } // list to store orders objects
        private List<Order_Details> detailsList { get; set; } // list to store order details objects

        public Form1()
        {
            InitializeComponent();
        }

        // fills form lists with sql data and fills combo box with order ID's by calling PopulateComboBox method when form is loaded 
        private void Form1_Load(object sender, EventArgs e)
        {
            // fill form lists with sql data
            orderList = OrdersDB.GetOrdersList();
            detailsList = Order_DetailsDB.GetDetailsListNoID();

            // fill combo box with order ID's
            PopulateComboBox();
        }

        // loop through the orderList, grab OrderID for each order object and add it to the combo box
        private void PopulateComboBox()
        {         
            foreach(Orders o in orderList)
            {
                cbxOrderID.Items.Add(o.OrderID);
            }
        }

        // display all necessary data for the selected id
        private void Update(string OrderID)
        {
            Orders order = OrdersDB.GetOrderByID(OrderID); // the order object found with OrderID

            txtCustomerID.Text = order.CustomerID;

            // these variables are used so that ToShortDateString() can be called when setting text boxes
            // they are being converted to DateTime from DateTime? because ToShortDateString() doesnt allow for nullable variables
            DateTime OrderDateOnly = Convert.ToDateTime(order.OrderDate);
            DateTime RequiredDateOnly = Convert.ToDateTime(order.RequiredDate);
            DateTime ShippedDateOnly = Convert.ToDateTime(order.ShippedDate);

            // displays dates without the time of day in the text boxes
            txtOrderDate.Text = Convert.ToString(OrderDateOnly.ToShortDateString());
            txtRequiredDate.Text = Convert.ToString(RequiredDateOnly.ToShortDateString());
            txtShippedDate.Text = Convert.ToString(ShippedDateOnly.ToShortDateString());

            //if (order.ShippedDate == null)
            //{
            //    txtShippedDate.Text = "NULL";
            //}
            //else
            //{
            //    txtShippedDate.Text = Convert.ToString(ShippedDateOnly.ToShortDateString());
            //}
            
            PopulateDataGridView(OrderID);
        }

        // fills the data grid view with the order details of a given order id
        private void PopulateDataGridView(string OrderID)
        {
            List<Order_Details> list = Order_DetailsDB.GetDetailsList(OrderID);

            dataGridView.Rows.Clear();

            // loop through the list of details objects and fill the data grid with details data for the specific order
            foreach (Order_Details details in list)
            {
                dataGridView.Rows.Add(new string[] {details.OrderID.ToString(), details.ProductID.ToString(), details.UnitPrice.ToString(),
                                                    details.Quantity.ToString(), details.Discount.ToString(), details.LineTotal.ToString()});
            }

            decimal OrderTotal = CalculateOrderTotal(list);
            Convert.ToString(OrderTotal);

            // display the calculated order total for the given order
            txtOrderTotal.Text = OrderTotal.ToString("c");
        }

        // calculate the order total
        private decimal CalculateOrderTotal(List<Order_Details> list)
        {
            decimal OrderTotal = 0; // holds order total

            // loop through each order details object and add on the line total to the order total
            foreach (Order_Details details in list)
            {
                OrderTotal += details.LineTotal;
            }

            return OrderTotal;
        }

        // when an order id is selected, display the info for that order
        private void cbxOrderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update(Convert.ToString(cbxOrderID.SelectedItem));
        }

        // update the shipped date in the database
        private void btnUpdate_Click(object sender, EventArgs e)
        {           
            Orders order = OrdersDB.GetOrderByID(cbxOrderID.Text);

            if (txtShippedDate.Text == "")
            {
                order.ShippedDate = null;
            }
            else
            {
                order.ShippedDate = Convert.ToDateTime(txtShippedDate.Text);
            }

            // check if shipped date is later than order date and if shipped date is earlier than the required date
            if (order.ShippedDate == null)
            {
                OrdersDB.UpdateShippedDate(order);
            }
            else if ((DateTime.Compare(Convert.ToDateTime(order.ShippedDate), Convert.ToDateTime(order.OrderDate))) < 0 ||
                (DateTime.Compare(Convert.ToDateTime(order.ShippedDate), Convert.ToDateTime(order.RequiredDate))) > 0)
            {
                OrdersDB.UpdateShippedDate(order);
            }
            else
            {
                OrdersDB.UpdateShippedDate(order);
            }
        }
    }
}
