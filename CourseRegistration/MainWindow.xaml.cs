using EntityDataModelLibrary;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseRegistration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SMSEntities dbContext = new SMSEntities();
        public MainWindow()
        {
            InitializeComponent();
            dbContext.Logins.Load();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = loginControl.Username;
            string password = new System.Net.NetworkCredential(string.Empty, loginControl.Password).Password;

            string correctPassword = (from login in dbContext.Logins.Local
                                      where login.LoginName == userName
                                      select login.Password).FirstOrDefault();

            if (correctPassword == null)
            {
                MessageBox.Show("User Name doesn't Exist! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if(correctPassword == password)
            {
                CourseWindow courseWindow = new CourseWindow(userName, dbContext, this);
                this.Hide();
                courseWindow.Show();
            }
            else
            {
                MessageBox.Show("Wrong Password! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
