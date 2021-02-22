using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace Edufun_2
{
    /// <summary>
    /// ClassPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClassPage : Page
    {
        private SQLiteConnection conn;
        private GridViewColumnHeader lastClickedGridViewColumnHeader = null;
        private ListSortDirection lastListSortDirection = ListSortDirection.Ascending;
        List<Class_Instructor> instructors = new List<Class_Instructor>();

        bool start = false;
        string search = "Name";
        int year = 2021;
        string quarter = "";
        string time = "";
        string day = "";
        public ClassPage()
        {
            InitializeComponent();
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

        private void classListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Class_Instructor selectedItem = (Class_Instructor)classListView.SelectedItem;
            //MessageBox.Show(selectedItem.Name.ToString());
            if (selectedItem != null)
            {
                DetailPage detail = new DetailPage();
                detail.SetLoadCompleted(NavigationService);
                this.NavigationService.Navigate(detail, selectedItem.ID);
            }
        }



        private void classListView_Click(object sender, RoutedEventArgs e)
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
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.classListView.ItemsSource);
            collectionView.SortDescriptions.Clear();
            SortDescription sortDescription = new SortDescription(header, listSortDirection);
            collectionView.SortDescriptions.Add(sortDescription);
            collectionView.Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connection_Open();
            start = true;

            classListView.ItemsSource = null;
            Class_Instructor.GetInstance().Clear();

            String sql = "SELECT Instructor_ID,Name,Phone,Department1,Department2,Subject,Quarter,SUM(Student_count) FROM Instructor JOIN Class ON Instructor.ID = Class.Instructor_ID WHERE Year = " + year + " AND Quarter LIKE '%" + quarter + "%' AND " + search + " LIKE '%" + tb_search.Text + "%' AND Student_count>0 GROUP BY(Class.Quarter) ";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Class_Instructor inst = new Class_Instructor();
                inst.ID = Int32.Parse(rdr["Instructor_ID"].ToString());
                inst.Name = rdr["Name"].ToString();
                inst.Quarter = Int32.Parse(rdr["Quarter"].ToString());
                inst.Phone = rdr["Phone"].ToString();
                inst.Subject = rdr["Subject"].ToString();
                inst.Department1 = rdr["Department1"].ToString();
                inst.Department2 = rdr["Department2"].ToString();
                inst.Student_count = Int32.Parse(rdr["SUM(Student_count)"].ToString());
                instructors.Add(inst);
                Console.WriteLine(rdr["SUM(Student_count)"]);
                Class_Instructor.GetInstance().Add(new Class_Instructor() { ID = Int32.Parse(rdr["Instructor_ID"].ToString()), Name = rdr["Name"].ToString(), Quarter = Int32.Parse(rdr["Quarter"].ToString()), Phone = rdr["Phone"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString(), Student_count = Int32.Parse(rdr["SUM(Student_count)"].ToString()) });

            }
            rdr.Close();
            classListView.ItemsSource = Class_Instructor.GetInstance();
        }
        private string Dayinttostring(int day)
        {
            String Day = "월";
            if (day == 1)
            {
                Day = "월";
            }
            else if (day == 2)
            {
                Day = "화";
            }
            else if (day == 3)
            {
                Day = "수";
            }
            else if (day == 4)
            {
                Day = "목";
            }
            else if (day == 5)
            {
                Day = "금";
            }
            else if (day == 6)
            {
                Day = "토";
            }
            else if (day == 7)
            {
                Day = "일";
            }
            return Day;
        }
        private int Daystringtoint(string day)
        {
            int Day = 1;
            if (day == "월")
            {
                Day = 1;
            }
            else if (day == "화")
            {
                Day = 2;
            }
            else if (day == "수")
            {
                Day = 3;
            }
            else if (day == "목")
            {
                Day = 4;
            }
            else if (day == "금")
            {
                Day = 5;
            }
            else if (day == "토")
            {
                Day = 6;
            }
            else if (day == "일")
            {
                Day = 7;
            }
            return Day;
        }
        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            MainPage main = new MainPage();
            this.NavigationService.Navigate(main);
        }

        private void cb_year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_year.SelectedItem;
                string cbi_year = cbi.Content.ToString();
                if (cbi_year == "2021")
                {
                    year = 2021;
                }
                else if (cbi_year == "2020")
                {
                    year = 2020;
                }
                else if (cbi_year == "2019")
                {
                    year = 2019;
                }
                reload();
            }
        }

        private void cb_quarter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_quarter.SelectedItem;
                string cbi_quarter = cbi.Content.ToString();
                if (cbi_quarter == "분기전체")
                {
                    quarter = "";
                }
                else if (cbi_quarter == "1분기")
                {
                    quarter = "1";
                }
                else if (cbi_quarter == "2분기")
                {
                    quarter = "2";
                }
                else if (cbi_quarter == "3분기")
                {
                    quarter = "3";
                }
                else if (cbi_quarter == "4분기")
                {
                    quarter = "4";
                }
                reload();
            }
        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            reload();
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
        private void reload()
        {
            classListView.ItemsSource = null;
            Class_Instructor.GetInstance().Clear();

            String sql = "SELECT Instructor_ID,Name,Phone,Department1,Department2,Subject,Quarter,SUM(Student_count) FROM Instructor JOIN Class ON Instructor.ID = Class.Instructor_ID WHERE Year = " + year + " AND Quarter LIKE '%" + quarter + "%' AND " + search + " LIKE '%" + tb_search.Text + "%' AND Student_count>0 GROUP BY(Class.Quarter) ";

            Console.WriteLine(sql);
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Class_Instructor inst = new Class_Instructor();
                inst.ID = Int32.Parse(rdr["ID"].ToString());
                inst.Name = rdr["Name"].ToString();
                inst.Quarter = Int32.Parse(rdr["Quarter"].ToString());
                inst.Phone = rdr["Phone"].ToString();
                inst.Subject = rdr["Subject"].ToString();
                inst.Department1 = rdr["Department1"].ToString();
                inst.Department2 = rdr["Department2"].ToString();
                inst.Student_count = Int32.Parse(rdr["SUM(Student_count)"].ToString());
                instructors.Add(inst);
                Console.WriteLine(rdr["SUM(Student_count)"]);
                Class_Instructor.GetInstance().Add(new Class_Instructor() { ID = Int32.Parse(rdr["Instructor_ID"].ToString()), Name = rdr["Name"].ToString(), Quarter = Int32.Parse(rdr["Quarter"].ToString()), Phone = rdr["Phone"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString(), Student_count = Int32.Parse(rdr["SUM(Student_count)"].ToString()) });
            }
            rdr.Close();
            classListView.ItemsSource = Class_Instructor.GetInstance();
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
            ComboBoxItem cbi = (ComboBoxItem)cb_year.SelectedItem;
            string cbi_year = cbi.Content.ToString();
            workSheet.Cells[1, "A"] = cbi_year;
            workSheet.Cells[2, "A"] = "ID";
            workSheet.Cells[2, "B"] = "이름";
            workSheet.Cells[2, "C"] = "분기";
            workSheet.Cells[2, "D"] = "폰번호";
            workSheet.Cells[2, "E"] = "과목";
            workSheet.Cells[2, "F"] = "소속1";
            workSheet.Cells[2, "G"] = "소속2";
            workSheet.Cells[2, "H"] = "학생수";
            var row = 2;
            foreach (var inst in instructors)
            {
                row++;
                workSheet.Cells[row, "A"] = inst.ID;
                workSheet.Cells[row, "B"] = inst.Name;
                workSheet.Cells[row, "C"] = inst.Quarter;
                workSheet.Cells[row, "D"] = inst.Phone;
                workSheet.Cells[row, "E"] = inst.Subject;
                workSheet.Cells[row, "F"] = inst.Department1;
                workSheet.Cells[row, "G"] = inst.Department2;
                workSheet.Cells[row, "H"] = inst.Student_count;
            }
        }
    }
}