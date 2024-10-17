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
using System.Windows.Shapes;

namespace Отдохни.Ru
{
    /// <summary>
    /// Логика взаимодействия для Window_reg_or_entry.xaml
    /// </summary>
    public partial class Window_reg_or_entry : Window
    {
        bool reg;
        public bool Reg
        {
            get { return reg; }
            set { reg = value; }
        }
        public Window_reg_or_entry()
        {

            InitializeComponent();
        }

        // вы уже зарегистрированный пользователь // да 
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            reg = true;
        }

        // вы уже зарегистрированный пользователь // нет 
        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            reg = false;
        }

        private void rb_yes_Checked(object sender, RoutedEventArgs e)
        {
            reg = true;
            Close();
        }

        private void rb_no_Checked(object sender, RoutedEventArgs e)
        {
            reg = false;
            Close();
        }
    }
}
