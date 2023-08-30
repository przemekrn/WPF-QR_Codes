using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Printing;
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
using QRCoder;
using QRCoder.Xaml;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using iTextSharp;
using iTextSharp.text;
using Microsoft.Win32;
using System.IO;
using GemBox.Document;
namespace KodyQR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Update();
        }
        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(txt.Text, QRCodeGenerator.ECCLevel.H);
            XamlQRCode qrCode = new XamlQRCode(qrCodeData);
            DrawingImage qrCodeAsXaml = qrCode.GetGraphic(20);
            qrImage.Source = qrCodeAsXaml;
            //System.Windows.Media.DrawingImage
            SaveToDatabase();
        }

        [Obsolete]
        public void SaveToDatabase()
        {
            string conn = "SERVER=localhost; DATABASE=kodyqr; UID=root; PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(conn);
            MySqlCommand cmd = new MySqlCommand("INSERT INTO zapisane (`ID`, `Tekst`, `Data Utworzenia`,`QR` ) VALUES (NULL, '" + txt.Text + "', CURRENT_TIME() ,'"+qrImage.Source+"');", connection);

            MySqlCommand cmd2 = new MySqlCommand("ALTER TABLE zapisane AUTO_INCREMENT = 1", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd2.ExecuteReader());
            dt.Load(cmd.ExecuteReader());

            connection.Close();
            Update();
        }
        void Update()
        {
            string conn = "SERVER=localhost; DATABASE=kodyqr; UID=root; PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(conn);
            MySqlCommand cmd3 = new MySqlCommand("SELECT * FROM zapisane ", connection);
            connection.Open();
            DataTable dt3 = new DataTable();
            dt3.Load(cmd3.ExecuteReader());
            connection.Close();
            dtGrid.DataContext = dt3;
        }
        private void BtnGenerate2_Click(object sender, RoutedEventArgs e)
        {
            string conn = "SERVER=localhost; DATABASE=kodyqr; UID=root; PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(conn);
            String Query = "UPDATE zapisane SET Tekst = '"+txt.Text+"' WHERE id=" + (dtGrid.SelectedIndex + 1) + ";";
            MySqlCommand zapytanie = new MySqlCommand(Query, connection);

            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(zapytanie.ExecuteReader());

            connection.Close();
            Update();
        }

        private void BtnGenerate3_Click(object sender, RoutedEventArgs e)
        {
            string conn = "SERVER=localhost; DATABASE=kodyqr; UID=root; PASSWORD=;";
            MySqlConnection connection = new MySqlConnection(conn);
            String Query = "DELETE FROM zapisane WHERE id=" + (dtGrid.SelectedIndex + 1) + ";";
            MySqlCommand zapytanie = new MySqlCommand(Query, connection);

            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(zapytanie.ExecuteReader());

            connection.Close();
            Update();
        }

    }
}

