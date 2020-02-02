using EntityDataModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace CourseRegistration
{
    /// <summary>
    /// Interaction logic for CourseWindow.xaml
    /// </summary>
    public partial class CourseWindow : Window
    {
        private string userName;
        private SMSEntities dbContext;
        private MainWindow mainWindow;
        private Student student;
        private ObservableCollection<CourseDetails> registeredCourses;
        private List<CourseDetails> courseList;

        public CourseWindow() { }
        public CourseWindow(string userName, SMSEntities dbContext, MainWindow mainWindow)
        {
            InitializeComponent();
            this.userName = userName;
            this.dbContext = dbContext;
            this.mainWindow = mainWindow;

            dbContext.Courses.Load();
            dbContext.Students.Load();
            dbContext.Logins.Load();
            courseList = (from course in dbContext.Courses.Local
                          select new CourseDetails(course.CourseCode, course.CourseTitle)).ToList();

            student = (from student in dbContext.Students.Local
                       from login in dbContext.Logins.Local
                       where login.LoginName == userName & student.Login == login
                       select student).FirstOrDefault();

            userNameLabel.Content = userName;
            courseCombo.ItemsSource = courseList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource courseDetailsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("courseDetailsViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // courseDetailsViewSource.Source = [generic data source]
            List<CourseDetails> courses = (from course in student.Courses
                                           select new CourseDetails(course.CourseCode, course.CourseTitle)).ToList();
            registeredCourses = new ObservableCollection<CourseDetails>(courses);
            courseDetailsViewSource.Source = registeredCourses;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            CourseDetails addCourse = (CourseDetails) courseCombo.SelectedItem; 

            if(addCourse != null)
            {
                Course newCourse = (from course in dbContext.Courses.Local
                                    where course.CourseCode == addCourse.CourseCode
                                    select course).FirstOrDefault();

                if (registeredCourses.Where(c => c.CourseCode == addCourse.CourseCode).FirstOrDefault() != null)
                {
                    MessageBox.Show($"Student({userName}) has already enrolled {addCourse.CourseCode}");
                }
                else
                {
                    registeredCourses.Add(addCourse);
                    student.Courses.Add(newCourse);
                    dbContext.SaveChanges();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            mainWindow.Close();
        }
    }
}
