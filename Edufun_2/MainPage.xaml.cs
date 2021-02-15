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
using System.Data.SQLite;
using System.ComponentModel;

namespace Edufun_2
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        private SQLiteConnection conn;
        private GridViewColumnHeader lastClickedGridViewColumnHeader = null;
        private ListSortDirection lastListSortDirection = ListSortDirection.Ascending;


        public MainPage()
        {
            InitializeComponent();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connection_Open();
            CreateTable();
            //InsertTable();
            SelectTable();
            //reload();
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
        public void CreateTable()
        {
            string sql = "create table if not exists Instructor (ID INTEGER PRIMARY KEY AUTOINCREMENT, Name varchar(10), Phone varchar(12), Email varchar(30), Subject varchar(10), Address varchar(30), Department1 varchar(10), Department2 varchar(10), Ship_Address1 varchar(30),Ship_Method1 varchar(20), Ship_Address2 varchar(30),Ship_Method2 varchar(20),Remark varchar(100))";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("result: " + result); 

            string sql2 = "create table if not exists Class (ID INTEGER PRIMARY KEY AUTOINCREMENT, Instructor_ID INTEGER , School varchar(20),Day INTEGER,Time INTEGER,Student_count INTEGER, Year INTEGER, Quarter INTEGER, FOREIGN KEY(Instructor_ID) REFERENCES Instructor(ID))";
            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            int result2 = command2.ExecuteNonQuery();
            Console.WriteLine("result2: " + result2);
        }

        public void SelectTable()
        {
            myListView.ItemsSource = null;
            Instructor.GetInstance().Clear();

            String sql = "SELECT * FROM Instructor";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            
            while (rdr.Read())
            {
                string Name = rdr["Name"].ToString();
                Console.WriteLine("Name: " + Name);
                string Phone = rdr["Phone"].ToString();
                Console.WriteLine("Phone: " + Phone);
                //Int32.Parse(string) : string을 int로 변환
                Instructor.GetInstance().Add(new Instructor() { ID = Int32.Parse(rdr["ID"].ToString()), Name = rdr["Name"].ToString(), Phone = rdr["Phone"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString() });
            }
            rdr.Close();
            myListView.ItemsSource = Instructor.GetInstance();

        }

        public void InsertTable()
        {
         
            String sql = "INSERT INTO Instructor (Name,Phone,Subject,Email,Address,Department1,Ship_Address1,Ship_Method1,Remark) VALUES ('윤보경','01023320000','실험과학','ybk@edufun.com','서울시 광진구','직영','택배','얼굴')";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("result: " + result);
            
            String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (1,'에릭초등학교',1,1, 2020 ,1,20)";

            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            int result2 = command2.ExecuteNonQuery();
            Console.WriteLine("result2: " + result2);
        }
        /*
         public void DeleteTable()
         {
             String sql = "delete from table이름 where url='" + url + "'";

             SQLiteCommand command = new SQLiteCommand(sql, conn);
             int result = command.ExecuteNonQuery();
         }*/

        private void myListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Instructor selectedItem = (Instructor)myListView.SelectedItem;
            //MessageBox.Show(selectedItem.Name.ToString());
            if (selectedItem != null)
            {
                DetailPage detail = new DetailPage();
                detail.SetLoadCompleted(NavigationService);
                this.NavigationService.Navigate(detail, selectedItem.ID);
            }
        }

        

        private void myListView_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader clickedGridViewColumnHeader = e.OriginalSource as GridViewColumnHeader;

            ListSortDirection listSortDirection;

            if (clickedGridViewColumnHeader != null)
            {
                if (clickedGridViewColumnHeader.Role != GridViewColumnHeaderRole.Padding)
                {
                    // 마지막으로 클릭한 그리브 뷰 컬럼 헤더가 아닌 경우
                    if (clickedGridViewColumnHeader != this.lastClickedGridViewColumnHeader)
                    {
                        listSortDirection = ListSortDirection.Ascending;
                    }
                    else // 마지막으로 클릭한 그리드 뷰 컬럼 헤더인 경우
                    {
                        if (this.lastListSortDirection == ListSortDirection.Ascending)
                        {
                            listSortDirection = ListSortDirection.Descending;
                        }
                        else
                        {
                            listSortDirection = ListSortDirection.Ascending;
                        }
                    }
                    string header = clickedGridViewColumnHeader.Column.Header as string;
                    Sort(header, listSortDirection);
                    this.lastClickedGridViewColumnHeader = clickedGridViewColumnHeader;
                    this.lastListSortDirection = listSortDirection;
                }
            }
        }
        private void Sort(string header, ListSortDirection listSortDirection)

        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.myListView.ItemsSource);
            collectionView.SortDescriptions.Clear();
            SortDescription sortDescription = new SortDescription(header, listSortDirection);
            collectionView.SortDescriptions.Add(sortDescription);
            collectionView.Refresh();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CreatePage create = new CreatePage();
            this.NavigationService.Navigate(create);
        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cb_subject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cb_department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void reload()
        {
            myListView.ItemsSource = null;
            Instructor.GetInstance().Clear();

            String sql = "SELECT * FROM Instructor";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Name = rdr["Name"].ToString();
                Console.WriteLine("Name: " + Name);
                string Phone = rdr["Phone"].ToString();
                Console.WriteLine("Phone: " + Phone);
                //Int32.Parse(string) : string을 int로 변환
                Instructor.GetInstance().Add(new Instructor() { ID = Int32.Parse(rdr["ID"].ToString()), Name = rdr["Name"].ToString(), Phone = rdr["Phone"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString() });
            }
            rdr.Close();
            myListView.ItemsSource = Instructor.GetInstance();
        }
    }
}
