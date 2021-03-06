﻿using System;
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
        List<School_student> students = new List<School_student>();
        bool start = false;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            start = true;
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
            ComboBoxItem cbi = (ComboBoxItem)cb_year.SelectedItem;
            string year = cbi.Content.ToString();
            Console.WriteLine(year);

            students = new List<School_student>();
            classListView.ItemsSource = null;

            String sql2 = "SELECT * FROM Class WHERE Instructor_ID = " + instructor_id + " AND Year = '" + year + "' ORDER BY School ASC, Day ASC, Quarter ASC, Time ASC ";
            Console.WriteLine(sql2);

            SQLiteCommand cmd2 = new SQLiteCommand(sql2, conn);
            SQLiteDataReader rdr2 = cmd2.ExecuteReader();

            School_student.GetInstance().Clear();

            int total_sum = 0;
            string school = "";
            int day = 0;
            bool change = false;
            bool hasrows = false;
            School_student stud = new School_student();
            while (rdr2.Read())
            {
                hasrows = true;
                if ((school != rdr2["School"].ToString()) || (day != Int32.Parse(rdr2["Day"].ToString())))
                {
                    if (change)
                    {
                        students.Add(stud);
                        Console.WriteLine("start added");
                    }
                    stud = new School_student();
                    stud.School = rdr2["School"].ToString();
                    stud.Day = Dayinttostring(Int32.Parse(rdr2["Day"].ToString()));
                    school = rdr2["School"].ToString();
                    day = Int32.Parse(rdr2["Day"].ToString());
                    change = true;
                }
                int Time = Int32.Parse(rdr2["Time"].ToString());
                int Quarter = Int32.Parse(rdr2["Quarter"].ToString());
                int student_count = Int32.Parse(rdr2["Student_count"].ToString());
                Console.WriteLine(rdr2["Day"].ToString() + "요일 " + Quarter + "분기 " + Time + "교시 " + student_count + "명 ");
                if (Quarter == 1)
                {
                    if (Time == 1)
                    {
                        Console.WriteLine("1교시");
                        Console.WriteLine(student_count);
                        stud.q1t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        Console.WriteLine("2교시");
                        stud.q1t2 = student_count;
                        Console.WriteLine(stud.q1t2);
                    }
                }
                else if (Quarter == 2)
                {
                    if (Time == 1)
                    {
                        stud.q2t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        stud.q2t2 = student_count;
                    }
                }
                else if (Quarter == 3)
                {
                    if (Time == 1)
                    {
                        stud.q3t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        stud.q3t2 = student_count;
                    }
                }
                else if (Quarter == 4)
                {
                    if (Time == 1)
                    {
                        stud.q4t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        stud.q4t2 = student_count;
                    }
                }
                total_sum += student_count;
                //Int32.Parse(string) : string을 int로 변환
                stud.q1sum = stud.q1t1 + stud.q1t2;
                stud.q2sum = stud.q2t1 + stud.q2t2;
                stud.q3sum = stud.q3t1 + stud.q3t2;
                stud.q4sum = stud.q4t1 + stud.q4t2;
            }
            if (hasrows)
            {
                students.Add(stud);
                Console.WriteLine("added");
            }

            rdr2.Close();
            for (int i = 0; i < students.Count(); i++)
            {
                Console.WriteLine("sdsdsdf");
                Console.WriteLine(students[i].q1t2);
                School_student.GetInstance().Add(new School_student() { ID = i, School = students[i].School, Day = students[i].Day, Year = Int32.Parse(year), q1t1 = students[i].q1t1, q1t2 = students[i].q1t2, q1sum = students[i].q1t1 + students[i].q1t2, q2t1 = students[i].q2t1, q2t2 = students[i].q2t2, q2sum = students[i].q2t1 + students[i].q2t2, q3t1 = students[i].q3t1, q3t2 = students[i].q3t2, q3sum = students[i].q3t1 + students[i].q3t2, q4t1 = students[i].q4t1, q4t2 = students[i].q4t2, q4sum = students[i].q4t1 + students[i].q4t2 });
            }

            classListView.ItemsSource = School_student.GetInstance();

            String sql3 = "SELECT SUM(Student_count),Quarter FROM Class WHERE Year = " + year + " AND Instructor_ID = " + instructor_id + " GROUP BY(Quarter) ";
            SQLiteCommand cmd3 = new SQLiteCommand(sql3, conn);
            SQLiteDataReader rdr3 = cmd3.ExecuteReader();

            while (rdr3.Read())
            {
                Console.WriteLine(rdr3["SUM(Student_count)"]);
                if (rdr3["Quarter"].ToString() == "1")
                {
                    tb_q1count.Text = rdr3["SUM(Student_count)"].ToString();
                }
                else if (rdr3["Quarter"].ToString() == "2")
                {
                    tb_q2count.Text = rdr3["SUM(Student_count)"].ToString();
                }
                else if (rdr3["Quarter"].ToString() == "3")
                {
                    tb_q3count.Text = rdr3["SUM(Student_count)"].ToString();
                }
                else if (rdr3["Quarter"].ToString() == "4")
                {
                    tb_q4count.Text = rdr3["SUM(Student_count)"].ToString();
                }
            }
            rdr3.Close();
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

            String sql = "UPDATE Instructor SET Name= '" + Name + "' ,Phone= '" + Phone + "' ,Subject= '" + Subject + "' ,Email= '" + Email + "' ,Address= '" + Address + "' ,Department1= '" + Department1 + "' ,Department2= '" + Department2 + "' ,Ship_Address1= '" + Ship_Address1 + "' ,Ship_Method1= '" + Ship_Method1 + "' ,Ship_Address2= '" + Ship_Address2 + "' ,Ship_Method2= '" + Ship_Method2 + "' ,Remark= '" + Remark + "' WHERE ID = " + instructor_id;
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

            School_student selectedItem = (School_student)classListView.SelectedItem;
            if (selectedItem != null)
            {
                //MessageBox.Show(selectedItem.ID.ToString());
                tb_edit_school.Text = selectedItem.School.ToString();
                tb_edit_day.Text = selectedItem.Day.ToString();
                tb_edit_year.Text = selectedItem.Year.ToString();
                tb_edit_q1t1.Text = selectedItem.q1t1.ToString();
                tb_edit_q1t2.Text = selectedItem.q1t2.ToString();
                tb_edit_q2t1.Text = selectedItem.q2t1.ToString();
                tb_edit_q2t2.Text = selectedItem.q2t2.ToString();
                tb_edit_q3t1.Text = selectedItem.q3t1.ToString();
                tb_edit_q3t2.Text = selectedItem.q3t2.ToString();
                tb_edit_q4t1.Text = selectedItem.q4t1.ToString();
                tb_edit_q4t2.Text = selectedItem.q4t2.ToString();

                ShowHideDetail("sbShowClassDetail", ClassDetail);
            }
        }

        private void editclass_cancel_Click(object sender, RoutedEventArgs e)
        {
            ShowHideDetail("sbHideClassDetail", ClassDetail);
        }

        private void editclass_save_Click(object sender, RoutedEventArgs e)
        {
            bool day_chk = false;
            int day = 1;
            Console.WriteLine(tb_edit_day.Text);
            if (tb_edit_day.Text == "월")
            {
                day_chk = true;
                day = 1;
            }
            else if (tb_edit_day.Text == "화")
            {
                day_chk = true;
                day = 2;
            }
            else if (tb_edit_day.Text == "수")
            {
                day_chk = true;
                day = 3;
            }
            else if (tb_edit_day.Text == "목")
            {
                day_chk = true;
                day = 4;
            }
            else if (tb_edit_day.Text == "금")
            {
                day_chk = true;
                day = 5;
            }
            else if (tb_edit_day.Text == "토")
            {
                day_chk = true;
                day = 6;
            }
            else if (tb_edit_day.Text == "일")
            {
                day_chk = true;
                day = 7;
            }
            else
            {
                MessageBox.Show("요일 입력을 다시 해주세요");
            }
            if (day_chk)
            {
                String sql = "DELETE FROM Class WHERE Instructor_ID = " + instructor_id + " AND School = '" + tb_edit_school.Text + "' AND Day = " + day + " AND year = " + tb_edit_year.Text;
                Console.WriteLine(sql);
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                int result = command.ExecuteNonQuery();
                Console.WriteLine("delete result: " + result);

                if (tb_edit_q1t1.Text.Length > 0)
                {
                    //"INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (1,'에릭초등학교',1,1, 2021 ,2,10)";
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "'," + day + ",1," + tb_edit_year.Text + ",1," + tb_edit_q1t1.Text + ")";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_edit_q1t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "','" + day + "','2'," + tb_edit_year.Text + ",'1','" + tb_edit_q1t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_edit_q2t1.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "','" + day + "','1'," + tb_edit_year.Text + ",'2','" + tb_edit_q2t1.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_edit_q2t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "','" + day + "','2'," + tb_edit_year.Text + ",'2','" + tb_edit_q2t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_edit_q3t1.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "','" + day + "','1'," + tb_edit_year.Text + ",'3','" + tb_edit_q3t1.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_edit_q3t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "','" + day + "','2'," + tb_edit_year.Text + ",'3','" + tb_edit_q3t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_edit_q4t1.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "','" + day + "','1'," + tb_edit_year.Text + ",'4','" + tb_edit_q4t1.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_edit_q4t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_edit_school.Text + "','" + day + "','2'," + tb_edit_year.Text + ",'4','" + tb_edit_q4t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                ComboBoxItem cbi = (ComboBoxItem)cb_year.SelectedItem;
                string year = cbi.Content.ToString();
                reload(year);
                ShowHideDetail("sbHideClassDetail", ClassDetail);
            }

        }

        private void editclass_delete_Click(object sender, RoutedEventArgs e)
        {
            School_student selectedItem = (School_student)classListView.SelectedItem;
            string school = selectedItem.School;
            int year = selectedItem.Year;
            int day = 1;
            if (selectedItem.Day == "월")
            {
                day = 1;
            }
            else if (selectedItem.Day == "화")
            {
                day = 2;
            }
            else if (selectedItem.Day == "수")
            {
                day = 3;
            }
            else if (selectedItem.Day == "목")
            {
                day = 4;
            }
            else if (selectedItem.Day == "금")
            {
                day = 5;
            }
            else if (selectedItem.Day == "토")
            {
                day = 6;
            }
            else if (selectedItem.Day == "일")
            {
                day = 7;
            }
            String sql = "DELETE FROM Class WHERE Instructor_ID = " + instructor_id + " AND School = '" + school + "' AND Day = " + day + " AND year = " + year;
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("delete result: " + result);
            MessageBox.Show("삭제됐습니다");
            ComboBoxItem cbi = (ComboBoxItem)cb_year.SelectedItem;
            string year_cb = cbi.Content.ToString();
            reload(year_cb);
            ShowHideDetail("sbHideClassDetail", ClassDetail);
        }
        private void reload(string year)
        {
            students = new List<School_student>();
            classListView.ItemsSource = null;

            String sql2 = "SELECT * FROM Class WHERE Instructor_ID = " + instructor_id + " AND Year = '" + year + "' ORDER BY School ASC, Day ASC, Quarter ASC, Time ASC ";
            Console.WriteLine(sql2);

            SQLiteCommand cmd2 = new SQLiteCommand(sql2, conn);
            SQLiteDataReader rdr2 = cmd2.ExecuteReader();

            School_student.GetInstance().Clear();

            int total_sum = 0;
            string school = "";
            int day = 0;
            bool change = false;
            bool hasrows = false;
            School_student stud = new School_student();
            while (rdr2.Read())
            {
                hasrows = true;
                if ((school != rdr2["School"].ToString()) || (day != Int32.Parse(rdr2["Day"].ToString())))
                {
                    if (change)
                    {
                        students.Add(stud);
                        Console.WriteLine("start added");
                    }
                    stud = new School_student();
                    stud.School = rdr2["School"].ToString();
                    stud.Day = Dayinttostring(Int32.Parse(rdr2["Day"].ToString()));
                    school = rdr2["School"].ToString();
                    day = Int32.Parse(rdr2["Day"].ToString());
                    change = true;
                }
                int Time = Int32.Parse(rdr2["Time"].ToString());
                int Quarter = Int32.Parse(rdr2["Quarter"].ToString());
                int student_count = Int32.Parse(rdr2["Student_count"].ToString());
                Console.WriteLine(rdr2["Day"].ToString() + "요일 " + Quarter + "분기 " + Time + "교시 " + student_count + "명 ");
                if (Quarter == 1)
                {
                    if (Time == 1)
                    {
                        Console.WriteLine("1교시");
                        Console.WriteLine(student_count);
                        stud.q1t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        Console.WriteLine("2교시");
                        stud.q1t2 = student_count;
                        Console.WriteLine(stud.q1t2);
                    }
                }
                else if (Quarter == 2)
                {
                    if (Time == 1)
                    {
                        stud.q2t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        stud.q2t2 = student_count;
                    }
                }
                else if (Quarter == 3)
                {
                    if (Time == 1)
                    {
                        stud.q3t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        stud.q3t2 = student_count;
                    }
                }
                else if (Quarter == 4)
                {
                    if (Time == 1)
                    {
                        stud.q4t1 = student_count;
                    }
                    else if (Time == 2)
                    {
                        stud.q4t2 = student_count;
                    }
                }
                total_sum += student_count;
                //Int32.Parse(string) : string을 int로 변환
                stud.q1sum = stud.q1t1 + stud.q1t2;
                stud.q2sum = stud.q2t1 + stud.q2t2;
                stud.q3sum = stud.q3t1 + stud.q3t2;
                stud.q4sum = stud.q4t1 + stud.q4t2;
            }
            if (hasrows)
            {
                students.Add(stud);
                Console.WriteLine("added");
            }

            rdr2.Close();
            for (int i = 0; i < students.Count(); i++)
            {
                Console.WriteLine("sdsdsdf");
                Console.WriteLine(students[i].q1t2);
                School_student.GetInstance().Add(new School_student() { ID = i, School = students[i].School, Year = Int32.Parse(year), Day = students[i].Day, q1t1 = students[i].q1t1, q1t2 = students[i].q1t2, q1sum = students[i].q1t1 + students[i].q1t2, q2t1 = students[i].q2t1, q2t2 = students[i].q2t2, q2sum = students[i].q2t1 + students[i].q2t2, q3t1 = students[i].q3t1, q3t2 = students[i].q3t2, q3sum = students[i].q3t1 + students[i].q3t2, q4t1 = students[i].q4t1, q4t2 = students[i].q4t2, q4sum = students[i].q4t1 + students[i].q4t2 });
            }

            classListView.ItemsSource = School_student.GetInstance();

            String sql3 = "SELECT SUM(Student_count),Quarter FROM Class WHERE Year = " + year + " AND Instructor_ID = " + instructor_id + " GROUP BY(Quarter) ";
            SQLiteCommand cmd3 = new SQLiteCommand(sql3, conn);
            SQLiteDataReader rdr3 = cmd3.ExecuteReader();

            while (rdr3.Read())
            {
                Console.WriteLine(rdr3["SUM(Student_count)"]);
                if (rdr3["Quarter"].ToString() == "1")
                {
                    tb_q1count.Text = rdr3["SUM(Student_count)"].ToString();
                }
                else if (rdr3["Quarter"].ToString() == "2")
                {
                    tb_q2count.Text = rdr3["SUM(Student_count)"].ToString();
                }
                else if (rdr3["Quarter"].ToString() == "3")
                {
                    tb_q3count.Text = rdr3["SUM(Student_count)"].ToString();
                }
                else if (rdr3["Quarter"].ToString() == "4")
                {
                    tb_q4count.Text = rdr3["SUM(Student_count)"].ToString();
                }
            }
            rdr3.Close();
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
            bool day_chk = false;
            int day = 1;
            if (tb_add_day.Text == "월")
            {
                day_chk = true;
                day = 1;
            }
            else if (tb_add_day.Text == "화")
            {
                day_chk = true;
                day = 2;
            }
            else if (tb_add_day.Text == "수")
            {
                day_chk = true;
                day = 3;
            }
            else if (tb_add_day.Text == "목")
            {
                day_chk = true;
                day = 4;
            }
            else if (tb_add_day.Text == "금")
            {
                day_chk = true;
                day = 5;
            }
            else if (tb_add_day.Text == "토")
            {
                day_chk = true;
                day = 6;
            }
            else if (tb_add_day.Text == "일")
            {
                day_chk = true;
                day = 7;
            }
            else
            {
                MessageBox.Show("요일 입력을 다시 해주세요");
            }
            if (day_chk)
            {
                if (tb_q1t1.Text.Length > 0)
                {
                    //"INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (1,'에릭초등학교',1,1, 2021 ,2,10)";
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "'," + day + ",1," + tb_add_year.Text + ",1," + tb_q1t1.Text + ")";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_q1t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','2'," + tb_add_year.Text + ",'1','" + tb_q1t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_q2t1.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','1'," + tb_add_year.Text + ",'2','" + tb_q2t1.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_q2t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','2'," + tb_add_year.Text + ",'2','" + tb_q2t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_q3t1.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','1'," + tb_add_year.Text + ",'3','" + tb_q3t1.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_q3t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','2'," + tb_add_year.Text + ",'3','" + tb_q3t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_q4t1.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','1'," + tb_add_year.Text + ",'4','" + tb_q4t1.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                if (tb_q4t2.Text.Length > 0)
                {
                    String sql2 = "INSERT INTO Class (Instructor_ID,School,Day,Time,Year,Quarter,Student_count) VALUES (" + instructor_id + ",'" + tb_add_school.Text + "','" + day + "','2'," + tb_add_year.Text + ",'4','" + tb_q4t2.Text + "')";
                    Console.WriteLine(sql2);
                    SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
                    int result2 = command2.ExecuteNonQuery();
                    Console.WriteLine("result2: " + result2);
                    if (result2 == -1)
                    {
                        MessageBox.Show("입력을 확인해주세요");
                        return;
                    }
                }
                ComboBoxItem cbi = (ComboBoxItem)cb_year.SelectedItem;
                string year = cbi.Content.ToString();
                reload(year);
                ShowHideDetail("sbHideClassAdd", ClassAdd);
                tb_add_school.Text = "";
                tb_add_year.Text = "";
                tb_add_day.Text = "";
                tb_q1t1.Text = "";
                tb_q1t2.Text = "";
                tb_q2t1.Text = "";
                tb_q2t2.Text = "";
                tb_q3t1.Text = "";
                tb_q3t2.Text = "";
                tb_q4t1.Text = "";
                tb_q4t2.Text = "";
            }
        }
        private void cb_year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start)
            {
                ComboBoxItem cbi = (ComboBoxItem)cb_year.SelectedItem;
                string year = cbi.Content.ToString();
                reload(year);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM Class WHERE Instructor_ID = " + instructor_id;
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("delete result: " + result);

            String sql2 = "DELETE FROM Instructor WHERE ID = " + instructor_id;
            Console.WriteLine(sql2);
            SQLiteCommand command2 = new SQLiteCommand(sql2, conn);
            int result2 = command2.ExecuteNonQuery();
            Console.WriteLine("delete result: " + result2);

            MainPage main = new MainPage();
            this.NavigationService.Navigate(main);
        }
    }
}