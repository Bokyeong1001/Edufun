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
using Excel = Microsoft.Office.Interop.Excel;

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
        List<Instructor> instructors = new List<Instructor>();
        bool start = false;
        string department = "";
        string subject = "";
        string search = "Name";
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
            start = true;
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
                Instructor inst = new Instructor();
                inst.ID = Int32.Parse(rdr["ID"].ToString());
                inst.Name= rdr["Name"].ToString();
                inst.Phone = rdr["Phone"].ToString();
                inst.Subject = rdr["Subject"].ToString();
                inst.Department1 = rdr["Department1"].ToString();
                inst.Department2 = rdr["Department2"].ToString();
                instructors.Add(inst);
                //Int32.Parse(string) : string을 int로 변환
                Instructor.GetInstance().Add(new Instructor() { ID = Int32.Parse(rdr["ID"].ToString()), Name = rdr["Name"].ToString(), Phone = rdr["Phone"].ToString(), Email = rdr["Email"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString() });
            }
            rdr.Close();
            myListView.ItemsSource = Instructor.GetInstance();

        }

        public void InsertTable()
        {
         /*
            String sql = "INSERT INTO Instructor (Name,Phone,Subject,Email,Address,Department1,Ship_Address1,Ship_Method1,Remark) VALUES ('윤보경','01023320000','실험과학','ybk@edufun.com','서울시 광진구','직영','서울집','택배','얼굴')";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("result: " + result);
            */
            String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (1,'에릭초등학교',1,1, 2021 ,2,10)";

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

        

        private void cb_subject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_subject.SelectedItem;
                string cbi_subject = cbi.Content.ToString();
                if (cbi_subject == "과목전체") {
                    subject = "";
                }
                else
                {
                    subject = cbi_subject;
                }
                reload();
            }
        }

        private void cb_department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_department.SelectedItem;
                string cbi_department = cbi.Content.ToString();
                if (cbi_department == "소속전체")
                {
                    department = "";
                }
                else
                {
                    department = cbi_department;
                }
                reload();
            }
        }
        private void reload()
        {
            instructors = new List<Instructor>();

            myListView.ItemsSource = null;
            Instructor.GetInstance().Clear();

            String sql = "SELECT * FROM Instructor WHERE subject LIKE '%" + subject + "%' AND (department1 LIKE '%" + department + "%' OR department2 LIKE '%" + department + "%')  AND " + search + " LIKE '%" + tb_search.Text + "%'";
            Console.WriteLine(sql);
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Instructor inst = new Instructor();
                inst.ID = Int32.Parse(rdr["ID"].ToString());
                inst.Name = rdr["Name"].ToString();
                inst.Phone = rdr["Phone"].ToString();
                inst.Subject = rdr["Subject"].ToString();
                inst.Department1 = rdr["Department1"].ToString();
                inst.Department2 = rdr["Department2"].ToString();
                instructors.Add(inst);
                //Int32.Parse(string) : string을 int로 변환
                Instructor.GetInstance().Add(new Instructor() { ID = Int32.Parse(rdr["ID"].ToString()), Name = rdr["Name"].ToString(), Phone = rdr["Phone"].ToString(), Email = rdr["Email"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString() });
            }
            rdr.Close();
            myListView.ItemsSource = Instructor.GetInstance();
        }

        private void cb_search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_search.SelectedItem;
                string cbi_search = cbi.Content.ToString();
                if (cbi_search == "이름")
                {
                    search = "Name";
                }
                if (cbi_search == "폰번호")
                {
                    search = "Phone";
                }
                if (cbi_search == "이메일")
                {
                    search = "Email";
                }
            }
        }
        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            reload();
        }

        private void bt_class_Click(object sender, RoutedEventArgs e)
        {
            ClassPage classpage = new ClassPage();
            this.NavigationService.Navigate(classpage);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = true;

            // Create a new, empty workbook and add it to the collection returned
            // by property Workbooks. The new workbook becomes the active workbook.
            // Add has an optional parameter for specifying a praticular template.
            // Because no argument is sent in this example, Add creates a new workbook.
            excelApp.Workbooks.Add();

            // This example uses a single workSheet. The explicit type casting is
            // removed in a later procedure.
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            workSheet.Cells[1, "A"] = "ID";
            workSheet.Cells[1, "B"] = "이름";
            workSheet.Cells[1, "C"] = "폰번호";
            workSheet.Cells[1, "D"] = "과목";
            workSheet.Cells[1, "E"] = "소속1";
            workSheet.Cells[1, "F"] = "소속2";
            var row = 1;
            foreach (var inst in instructors)
            {
                row++;
                workSheet.Cells[row, "A"] = inst.ID;
                workSheet.Cells[row, "B"] = inst.Name;
                workSheet.Cells[row, "C"] = inst.Phone;
                workSheet.Cells[row, "D"] = inst.Subject;
                workSheet.Cells[row, "E"] = inst.Department1;
                workSheet.Cells[row, "F"] = inst.Department2;
            }
        }
    }
}
