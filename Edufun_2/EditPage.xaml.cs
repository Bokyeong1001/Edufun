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
using System.Windows.Media.Animation;

namespace Edufun_2
{
    /// <summary>
    /// EditPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditPage : Page
    {
        public int instructor_id;
        private SQLiteConnection conn;
        private GridViewColumnHeader lastClickedGridViewColumnHeader = null;
        private ListSortDirection lastListSortDirection = ListSortDirection.Ascending;
        public EditPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }
        public void SetLoadCompleted(NavigationService navigation)
        {
            navigation.LoadCompleted += new LoadCompletedEventHandler(NavigationService_LoadCompleted);
        }
        void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData != null)
            {
                Console.WriteLine("넘겨온 데이터");
                instructor_id = (int)e.ExtraData;
                Console.WriteLine(instructor_id);
            }
            this.NavigationService.LoadCompleted -= new LoadCompletedEventHandler(NavigationService_LoadCompleted);
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
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Connection_Open();
            //MessageBox.Show(instructor_id.ToString());
            String sql = "SELECT * FROM Instructor WHERE ID = " + instructor_id;

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Name = rdr["Name"].ToString();
                tb_name.Text = Name;
                string Phone = rdr["Phone"].ToString();
                tb_phone.Text = Phone;
                string Email = rdr["Email"].ToString();
                tb_email.Text = Email;
                string Subject = rdr["Subject"].ToString();
                tb_subject.Text = Subject;
                string Address = rdr["Address"].ToString();
                tb_address.Text = Address;
                string Department1 = rdr["Department1"].ToString();
                tb_department1.Text = Department1;
                string Department2 = rdr["Department2"].ToString();
                tb_department2.Text = Department2;
                string Ship_Address1 = rdr["Ship_Address1"].ToString();
                tb_shipaddress1.Text = Ship_Address1;
                string Ship_Address2 = rdr["Ship_Address2"].ToString();
                tb_shipaddress2.Text = Ship_Address2;
                string Ship_Method1 = rdr["Ship_Method1"].ToString();
                tb_shipmethod1.Text = Ship_Method1;
                string Ship_Method2 = rdr["Ship_Method2"].ToString();
                tb_shipmethod2.Text = Ship_Method2;
                string Remark = rdr["Remark"].ToString();
                tb_remark.Text = Remark;
            }
            rdr.Close();

            String sql2 = "SELECT * FROM Class WHERE Instructor_ID = " + instructor_id + " ORDER BY Year DESC, Quarter ASC, Day ASC, Time ASC";

            SQLiteCommand cmd2 = new SQLiteCommand(sql2, conn);
            SQLiteDataReader rdr2 = cmd2.ExecuteReader();

            Class.GetInstance().Clear();
            Class[] classes = new Class[100];
            int count = 0;
            int total_sum = 0;
            while (rdr2.Read())
            {
                int ID = Int32.Parse(rdr2["ID"].ToString());
                classes[count] = new Class();
                classes[count].ID = ID;
                string School = rdr2["School"].ToString();
                classes[count].School = School;
                String Day = "월";
                if (Int32.Parse(rdr2["Day"].ToString()) == 1)
                {
                    Day = "월";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 2)
                {
                    Day = "화";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 3)
                {
                    Day = "수";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 4)
                {
                    Day = "목";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 5)
                {
                    Day = "금";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 6)
                {
                    Day = "토";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 7)
                {
                    Day = "일";
                }
                classes[count].Day = Day;
                Console.WriteLine(Day);
                int Time = Int32.Parse(rdr2["Time"].ToString());
                classes[count].Time = Time;
                int Year = Int32.Parse(rdr2["Year"].ToString());
                classes[count].Year = Year;
                int Quarter = Int32.Parse(rdr2["Quarter"].ToString());
                classes[count].Quarter = Quarter;
                int Student_count = Int32.Parse(rdr2["Student_count"].ToString());
                total_sum += Student_count;
                classes[count].Student_count = Student_count;
                //Int32.Parse(string) : string을 int로 변환
                count++;
            }
            rdr2.Close();
            int sum = 0;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine(classes[i].Day);
                if (i != count-1)
                {
                    if ((classes[i].Day == classes[i + 1].Day) && (classes[i].Year == classes[i + 1].Year) && (classes[i].Quarter == classes[i + 1].Quarter))
                    {
                        sum = sum + classes[i].Student_count;
                        Class.GetInstance().Add(new Class() { ID = classes[i].ID, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count });
                    }
                    else
                    {
                        sum = sum + classes[i].Student_count;
                        Class.GetInstance().Add(new Class() { ID = classes[i].ID, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count, Sum = sum });
                        sum = 0;
                    }
                }
                else
                {
                    sum = sum + classes[i].Student_count;
                    Class.GetInstance().Add(new Class() { ID = classes[i].ID, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count, Sum = sum });
                    sum = 0;
                }
            }
            classListView.ItemsSource = Class.GetInstance();
            tb_totalcount.Text = total_sum.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DetailPage detail = new DetailPage();
            detail.SetLoadCompleted(NavigationService);
            this.NavigationService.Navigate(detail, instructor_id);
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

            String sql = "UPDATE Instructor SET Name= '"+Name+ "' ,Phone= '"+Phone + "' ,Subject= '"+Subject + "' ,Email= '"+Email + "' ,Address= '"+Address + "' ,Department1= '"+Department1 + "' ,Department2= '" + Department2 +  "' ,Ship_Address1= '"+Ship_Address1 + "' ,Ship_Method1= '"+Ship_Method1 + "' ,Ship_Address2= '" + Ship_Address2 + "' ,Ship_Method2= '" + Ship_Method2 + "' ,Remark= '"+Remark + "' WHERE ID = " + instructor_id;
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("result: " + result);
            MessageBox.Show("저장됐습니다");
            DetailPage detail = new DetailPage();
            detail.SetLoadCompleted(NavigationService);
            this.NavigationService.Navigate(detail, instructor_id);
        }
        private void ShowHideDetail(string Storyboard, Grid pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);
        }
        private void classListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            Class selectedItem = (Class)classListView.SelectedItem;
            if (selectedItem != null)
            {
                //MessageBox.Show(selectedItem.ID.ToString());
                tb_edit_school.Text = selectedItem.School.ToString();
                tb_edit_day.Text = selectedItem.Day.ToString();
                tb_edit_time.Text = selectedItem.Time.ToString();
                tb_edit_year.Text = selectedItem.Year.ToString();
                tb_edit_quarter.Text = selectedItem.Quarter.ToString();
                tb_edit_student_count.Text = selectedItem.Student_count.ToString();

                ShowHideDetail("sbShowClassDetail", ClassDetail);
            }
        }

        private void editclass_cancel_Click(object sender, RoutedEventArgs e)
        {
            ShowHideDetail("sbHideClassDetail", ClassDetail);
        }

        private void editclass_save_Click(object sender, RoutedEventArgs e)
        {
            int day = 1;
            if (tb_edit_day.Text ==  "월")
            {
                day = 1;
            }
            else if (tb_edit_day.Text == "화")
            {
                day = 2;
            }
            else if (tb_edit_day.Text == "수")
            {
                day = 3;
            }
            else if (tb_edit_day.Text == "목")
            {
                day = 4;
            }
            else if (tb_edit_day.Text == "금")
            {
                day = 5;
            }
            else if (tb_edit_day.Text == "토")
            {
                day = 6;
            }
            else if (tb_edit_day.Text == "일")
            {
                day = 7;
            }
            else
            {
                MessageBox.Show("요일 입력을 다시 해주세요");
                return;
            }
            String sql = "UPDATE Class SET Instructor_ID= '" + instructor_id + "' ,School= '" + tb_edit_school.Text + "' ,Day= '" + day + "' ,Time= '" + tb_edit_time.Text + "' ,Year= '" + tb_edit_year.Text + "' ,Quarter= '" + tb_edit_quarter.Text + "' ,Student_count= '" + tb_edit_student_count.Text + "' WHERE ID = " + instructor_id;
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("update result: " + result);
            reload();
            ShowHideDetail("sbHideClassDetail", ClassDetail);
        }

        private void editclass_delete_Click(object sender, RoutedEventArgs e)
        {
            Class selectedItem = (Class)classListView.SelectedItem;
            int id = selectedItem.ID;
            String sql = "DELETE FROM Class WHERE ID = "+ id;
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("delete result: " + result);
            MessageBox.Show("삭제됐습니다");
            reload();
            ShowHideDetail("sbHideClassDetail", ClassDetail);
        }
        private void reload()
        {
            String sql2 = "SELECT * FROM Class WHERE Instructor_ID = " + instructor_id + " ORDER BY Year DESC, Quarter ASC, Day ASC, Time ASC";

            SQLiteCommand cmd2 = new SQLiteCommand(sql2, conn);
            SQLiteDataReader rdr2 = cmd2.ExecuteReader();
            classListView.ItemsSource = null;
            Class.GetInstance().Clear();
            Class[] classes = new Class[100];
            int count = 0;
            int total_sum = 0;
            while (rdr2.Read())
            {
                int ID = Int32.Parse(rdr2["ID"].ToString());
                classes[count] = new Class();
                classes[count].ID = ID;
                string School = rdr2["School"].ToString();
                classes[count].School = School;
                String Day = "월";
                if (Int32.Parse(rdr2["Day"].ToString()) == 1)
                {
                    Day = "월";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 2)
                {
                    Day = "화";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 3)
                {
                    Day = "수";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 4)
                {
                    Day = "목";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 5)
                {
                    Day = "금";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 6)
                {
                    Day = "토";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 7)
                {
                    Day = "일";
                }
                classes[count].Day = Day;
                Console.WriteLine(Day);
                int Time = Int32.Parse(rdr2["Time"].ToString());
                classes[count].Time = Time;
                int Year = Int32.Parse(rdr2["Year"].ToString());
                classes[count].Year = Year;
                int Quarter = Int32.Parse(rdr2["Quarter"].ToString());
                classes[count].Quarter = Quarter;
                int Student_count = Int32.Parse(rdr2["Student_count"].ToString());
                total_sum += Student_count;
                classes[count].Student_count = Student_count;
                //Int32.Parse(string) : string을 int로 변환
                count++;
            }
            rdr2.Close();
            int sum = 0;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine(classes[i].Day);
                if (i != count - 1)
                {
                    if ((classes[i].Day == classes[i + 1].Day) && (classes[i].Year == classes[i + 1].Year) && (classes[i].Quarter == classes[i + 1].Quarter))
                    {
                        sum = sum + classes[i].Student_count;
                        Class.GetInstance().Add(new Class() { ID = classes[i].ID, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count });
                    }
                    else
                    {
                        sum = sum + classes[i].Student_count;
                        Class.GetInstance().Add(new Class() { ID = classes[i].ID, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count, Sum = sum });
                        sum = 0;
                    }
                }
                else
                {
                    sum = sum + classes[i].Student_count;
                    Class.GetInstance().Add(new Class() { ID = classes[i].ID, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count, Sum = sum });
                    sum = 0;
                }
            }
            classListView.ItemsSource = Class.GetInstance();
            tb_totalcount.Text = total_sum.ToString();
        }

        private void add_class_Click(object sender, RoutedEventArgs e)
        {
            ShowHideDetail("sbShowClassAdd", ClassAdd);
        }

        private void addclass_cancel_Click(object sender, RoutedEventArgs e)
        {
            ShowHideDetail("sbHideClassAdd", ClassAdd);
        }

        private void addclass_save_Click(object sender, RoutedEventArgs e)
        {
            int day = 1;
            if (tb_add_day.Text == "월")
            {
                day = 1;
            }
            else if (tb_add_day.Text == "화")
            {
                day = 2;
            }
            else if (tb_add_day.Text == "수")
            {
                day = 3;
            }
            else if (tb_add_day.Text == "목")
            {
                day = 4;
            }
            else if (tb_add_day.Text == "금")
            {
                day = 5;
            }
            else if (tb_add_day.Text == "토")
            {
                day = 6;
            }
            else if (tb_add_day.Text == "일")
            {
                day = 7;
            }
            else
            {
                MessageBox.Show("요일 입력을 다시 해주세요");
                return;
            }
            String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','" + tb_add_time.Text + "','" + tb_add_year.Text + "','" + tb_add_quarter.Text + "'," + tb_add_student_count.Text + ")";
            Console.WriteLine(sql2);
            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            int result2 = command2.ExecuteNonQuery();
            Console.WriteLine("result2: " + result2);
            if (result2 == 1)
            {
                MessageBox.Show("저장됐습니다");
                reload();
                ShowHideDetail("sbHideClassAdd", ClassAdd);
            }
            else
            {
                MessageBox.Show("입력을 확인해주세요");
                return;
            }
        }
    }
}
