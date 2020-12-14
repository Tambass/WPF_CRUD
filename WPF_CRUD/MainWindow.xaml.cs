using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WPF_CRUD
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection mysqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionMySQL"].ConnectionString);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_Show(object sender, RoutedEventArgs e)
        {
            try
            {
                mysqlConn.Open(); // DataBinding
                MySqlCommand products = new MySqlCommand("SELECT * FROM Product", mysqlConn);
                MySqlDataAdapter adp = new MySqlDataAdapter(products);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                dataGridProduct.DataContext = ds;
                mysqlConn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                mysqlConn.Open();
                if(addBtn.Content.Equals("Change"))
                {
                    MySqlCommand editProd = new MySqlCommand("UPDATE Product SET name = ?name, price = ?price WHERE id = ?id", mysqlConn);
                    editProd.Parameters.AddWithValue("?id", label.Content.ToString().Trim());
                    editProd.Parameters.AddWithValue("?name", name.Text.Trim());
                    editProd.Parameters.AddWithValue("?price", price.Text.Trim());
                    editProd.ExecuteNonQuery();
                    Clear();
                    MessageBox.Show("Produit modifié avec succès !");
                } else
                {
                MySqlCommand addProd = new MySqlCommand("INSERT INTO Product (name, price) VALUES (?name, ?price)", mysqlConn);
                addProd.Parameters.AddWithValue("?name", name.Text.Trim());
                addProd.Parameters.AddWithValue("?price", price.Text.Trim());
                addProd.ExecuteNonQuery();
                Clear();
                MessageBox.Show("Produit ajouté avec succès !");
                }
                mysqlConn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void Clear()
        {
            name.Text = "";
            price.Text = "";
        }

        private void dataGridProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null){
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    DataRowView dr = (DataRowView)dgr.Item;
                    label.Content = dr[0].ToString();
                    name.Text = dr[1].ToString();
                    price.Text = dr[2].ToString();
                    addBtn.Content = "Change";
                }
            }
        }
    }
}
