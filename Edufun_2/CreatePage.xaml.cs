using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace Edufun_2
{
    /// <summary>
    /// CreatePAge.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CreatePage : Page
    {
        private SQLiteConnection conn;

        public CreatePage()
        {
            InitializeComponent();
            Connection_Open();
        }
        public void Connection_Open()
        {
            string DBfile = "Edufun.sqlite";

            conn = new SQLiteConnection("Data Source=Edufun.sqlite;Version=3;");
            if (!System.IO.File.Exists(DBfile))
            {
                Console.WriteLine("created!");
                SQLiteConnection.CreateFile("Edufun.sqlite");
            }
            conn.Open();

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage main = new MainPage();
            this.NavigationService.Navigate(main);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string Name = tb_name.Text;
            string Phone = tb_phone.Text;
            string Email = tb_email.Text;
            string Subject = tb_subject.Text;
            string Address = tb_address.Text;
            string Department1 = tb_department1.Text;
            string Department2 = tb_department2.Text;
            string Ship_Address1 = tb_shipaddress1.Text;
            string Ship_Address2 = tb_shipaddress2.Text;
            string Ship_Method1 = tb_shipmethod1.Text;
            string Ship_Method2 = tb_shipmethod2.Text;
            string Remark = tb_remark.Text;

            String sql = "INSERT INTO Instructor (Name,Phone,Email,Subject,Address,Department1,Department2,Ship_Address1,Ship_Address2,Ship_Method1,Ship_Method2,Remark) VALUES ('"+Name+"','"+Phone+ "','" + Email+"','" +Subject+"','" +Address+ "','" +Department1+ "','" +Department2+ "','" +Ship_Address1+ "','" +Ship_Address2+ "','" +Ship_Method1+ "','" +Ship_Method2+ "','" +Remark+"')";
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            MessageBox.Show("저장됐습니다");

            String sql2 = "select* from Instructor where rowid = last_insert_rowid();";
            Console.WriteLine(sql2);
            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            SQLiteDataReader rdr2 = command2.ExecuteReader();
            while (rdr2.Read())
            {
                DetailPage detail = new DetailPage();
                detail.SetLoadCompleted(NavigationService);
                this.NavigationService.Navigate(detail, Int32.Parse(rdr2["ID"].ToString()));
            }
                

        }
    }
}
