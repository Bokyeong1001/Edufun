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
                string Bank = rdr["Bank"].ToString();
                tb_bank.Text = Bank;
                string Account_num = rdr["Account_num"].ToString();
                tb_account_num.Text = Account_num;
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
                classes[ID] = new Class();
                count = ID;
                string School = rdr2["School"].ToString();
                classes[ID].School = School;
                String Day = "월요일";
                if (Int32.Parse(rdr2["Day"].ToString()) == 1)
                {
                    Day = "월요일";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 2)
                {
                    Day = "화요일";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 3)
                {
                    Day = "수요일";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 4)
                {
                    Day = "목요일";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 5)
                {
                    Day = "금요일";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 6)
                {
                    Day = "토요일";
                }
                else if (Int32.Parse(rdr2["Day"].ToString()) == 7)
                {
                    Day = "일요일";
                }
                classes[ID].Day = Day;
                Console.WriteLine(Day);
                string Time = rdr2["Time"].ToString();
                classes[ID].Time = Time;
                int Year = Int32.Parse(rdr2["Year"].ToString());
                classes[ID].Year = Year;
                string Quarter = rdr2["Quarter"].ToString();
                classes[ID].Quarter = Quarter;
                int Student_count = Int32.Parse(rdr2["Student_count"].ToString());
                total_sum += Student_count;
                classes[ID].Student_count = Student_count;
                //Int32.Parse(string) : string을 int로 변환
            }
            rdr.Close();
            int sum = 0;
            for (int i = 1; i < count + 1; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine(classes[i].Day);
                if (i != count)
                {
                    if ((classes[i].Day == classes[i + 1].Day) && (classes[i].Year == classes[i + 1].Year) && (classes[i].Quarter == classes[i + 1].Quarter))
                    {
                        sum = sum + classes[i].Student_count;
                        Class.GetInstance().Add(new Class() { ID = i, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count });
                    }
                    else
                    {
                        sum = sum + classes[i].Student_count;
                        Class.GetInstance().Add(new Class() { ID = i, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count, Sum = sum });
                        sum = 0;
                    }
                }
                else
                {
                    sum = sum + classes[i].Student_count;
                    Class.GetInstance().Add(new Class() { ID = i, School = classes[i].School, Day = classes[i].Day, Time = classes[i].Time, Year = classes[i].Year, Quarter = classes[i].Quarter, Student_count = classes[i].Student_count, Sum = sum });
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
            string Bank = tb_bank.Text;
            string Account_num = tb_account_num.Text;
            string Address = tb_address.Text;
            string Department1 = tb_department1.Text;
            string Department2 = tb_department2.Text;
            string Ship_Address1 = tb_shipaddress1.Text;
            string Ship_Address2 = tb_shipaddress2.Text;
            string Ship_Method1 = tb_shipmethod1.Text;
            string Ship_Method2 = tb_shipmethod2.Text;
            string Remark = tb_remark.Text;

            String sql = "UPDATE Instructor SET Name= '"+Name+ "' ,Phone= '"+Phone + "' ,Subject= '"+Subject + "' ,Email= '"+Email + "' ,Address= '"+Address + "' ,Department1= '"+Department1 + "' ,Department2= '" + Department2 + "' ,Bank = '"+Bank + "' ,Account_num= '"+Account_num + "' ,Ship_Address1= '"+Ship_Address1 + "' ,Ship_Method1= '"+Ship_Method1 + "' ,Ship_Address2= '" + Ship_Address2 + "' ,Ship_Method2= '" + Ship_Method2 + "' ,Remark= '"+Remark + "' WHERE ID = " + instructor_id;
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
            Console.WriteLine("result: " + result);
            MessageBox.Show("저장됐습니다");
            DetailPage detail = new DetailPage();
            detail.SetLoadCompleted(NavigationService);
            this.NavigationService.Navigate(detail, instructor_id);
        }

    }
}
