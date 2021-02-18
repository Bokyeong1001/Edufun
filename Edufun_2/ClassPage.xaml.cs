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
        bool start = false;
        string search = "Name";
        int year = 2021;
        string quarter = "";
        string time ="";
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

            String sql = "SELECT Day,Instructor_ID,Name,Phone,Email,Department1,Department2,Subject FROM Instructor JOIN Class ON Instructor.ID = Class.Instructor_ID GROUP BY(Class.Day)";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Name = rdr["Name"].ToString();
                Console.WriteLine("Name: " + Name);
                string Phone = rdr["Phone"].ToString();
                Console.WriteLine("Phone: " + Phone);
                string day = Dayinttostring(Int32.Parse(rdr["Day"].ToString()));
                //Int32.Parse(string) : string을 int로 변환
                Class_Instructor.GetInstance().Add(new Class_Instructor() { ID = Int32.Parse(rdr["Instructor_ID"].ToString()), Name = rdr["Name"].ToString(), Day = day, Phone = rdr["Phone"].ToString(), Email = rdr["Email"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString() });
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

        private void cb_day_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_day.SelectedItem;
                string cbi_day = cbi.Content.ToString();
                if (cbi_day == "요일전체")
                {
                    day = "";
                }
                else if (cbi_day == "월요일")
                {
                    day = "1";
                }
                else if (cbi_day == "화요일")
                {
                    day = "2";
                }
                else if (cbi_day == "수요일")
                {
                    day = "3";
                }
                else if (cbi_day == "목요일")
                {
                    day = "4";
                }
                else if (cbi_day == "금요일")
                {
                    day = "5";
                }
                else if (cbi_day == "토요일")
                {
                    day = "6";
                }
                else if (cbi_day == "일요일")
                {
                    day = "7";
                }
                reload();
            }
        }

        private void cb_time_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_time.SelectedItem;
                string cbi_time = cbi.Content.ToString();
                if (cbi_time == "교시전체")
                {
                    time = "";
                }
                else if (cbi_time == "1교시")
                {
                    time = "1";
                }
                else if (cbi_time == "2교시")
                {
                    time = "2";
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

            String sql = "SELECT Day,Instructor_ID,Name,Phone,Email,Department1,Department2,Subject FROM Instructor JOIN Class ON Instructor.ID = Class.Instructor_ID WHERE Year = "+year+" AND Quarter LIKE '%"+quarter+"%' AND Day LIKE '%"+day+"%' AND Time LIKE '%"+time+"%' AND "+search+" LIKE '%"+tb_search.Text+ "%' AND Student_count>0 GROUP BY(Class.Day) ";
            Console.WriteLine(sql);
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Name = rdr["Name"].ToString();
                Console.WriteLine("Name: " + Name);
                string Phone = rdr["Phone"].ToString();
                Console.WriteLine("Phone: " + Phone);
                string day = Dayinttostring(Int32.Parse(rdr["Day"].ToString()));
                //Int32.Parse(string) : string을 int로 변환
                Class_Instructor.GetInstance().Add(new Class_Instructor() { ID = Int32.Parse(rdr["Instructor_ID"].ToString()), Name = rdr["Name"].ToString(), Day = day, Phone = rdr["Phone"].ToString(), Email = rdr["Email"].ToString(), Subject = rdr["Subject"].ToString(), Department1 = rdr["Department1"].ToString(), Department2 = rdr["Department2"].ToString() });
            }
            rdr.Close();
            classListView.ItemsSource = Class_Instructor.GetInstance();
        }
    }
}
