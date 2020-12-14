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

        private void Btn_Save(object sender, RoutedEventArgs e)
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
    }
}
