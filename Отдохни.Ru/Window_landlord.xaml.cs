using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Security.Authentication;
using System.Xml.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Отдохни.Ru.Classes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using System.Drawing.Imaging;

namespace Отдохни.Ru
{
    /// <summary>
    /// Логика взаимодействия для Window_landlord.xaml
    /// </summary>
    public partial class Window_landlord : Window
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["OtdohnyBD_add"].ConnectionString;
        string procedure = "", fileName = "", emailPattern = @"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$";
        int user_id = 0, landlord_id = 0, photo_user_id = 0;
        bool sucess = false;

        SqlConnection conn = null;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        BitmapImage photo_user;
        Landlord landlord;

        //public Landlord Landlord_
        //{
        //    get { return landlord; }
        //    set { landlord = value; }
        //}
        public bool Sucess
        {
            get { return sucess; }
            set { sucess = value; }
        }
        public int UserId
        {
            get { return user_id; }
            set { user_id = value; }
        }
        public int LandlordUserId
        {
            get { return landlord_id; }
            set { landlord_id = value; }
        }
        public int PhotoUserId
        {
            get { return photo_user_id; }
            set { photo_user_id = value; }
        }
        public BitmapImage PhotoUser
        {
            get { return photo_user; }
            set { photo_user = value; }
        }
        public Window_landlord()
        {
            InitializeComponent();
        }
        private void LastNameText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void LastNameText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0))
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void FirstNameText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void FirstNameText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0))
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void SurnameText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void SurnameText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, 0))
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void PhoneText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void PhoneText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void LoginText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void LoginText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.Text, 0))
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void PasswordText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Графические файлы |*.bmp; *.png; *.gif; *.jpg; *JPG";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            // получаем выбранный файл
            fileName = openFileDialog1.FileName;

            photo_user = new BitmapImage();

            photo_user.BeginInit();
            photo_user.UriSource = new Uri(fileName);
            photo_user.EndInit();
            photo.Source = photo_user;
        }

        private void PasswordText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.Text, 0))
                e.Handled = true; // тогда не обрабатывать введенный символ(и, следовательно, не выводить его в TextBox)
        }

        private byte[] CreateCopy()
        {
            // это часть для того, чтобы картинки меньше веcили
            System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);

            int maxWidth = 300, maxHeight = 300;

            double rationX = (double)maxWidth / img.Width;
            double rationY = (double)maxHeight / img.Height;

            double ratio = Math.Min(rationX, rationY);

            int newWidth = (int)(img.Width * ratio);
            int newHeight = (int)(img.Height * ratio);

            // обязательная часть
            System.Drawing.Image im = new Bitmap(newWidth, newHeight); //наша картинка по новым размерам
            Graphics g = Graphics.FromImage(im);
            g.DrawImage(img, 0, 0, newWidth, newHeight); // прорисовываем нашу картинку
            MemoryStream ms = new MemoryStream();
            im.Save(ms, ImageFormat.Jpeg);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(ms);
            byte[] buf = br.ReadBytes((int)ms.Length);

            return buf;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (LastNameText.Text.Length == 0)
            {
                MessageBox.Show("Введите фамилию!");
                return;
            }
            if (FirstNameText.Text.Length == 0)
            {
                MessageBox.Show("Введите имя!");
                return;
            }
            if (SurnameText.Text.Length == 0)
            {
                MessageBox.Show("Введите отчество!");
                return;
            }
            if (EmailText.Text.Length == 0)
            {
                MessageBox.Show("Введите почту!");
                return;
            }
            if (PhoneText.Text.Length == 0)
            {
                MessageBox.Show("Введите телефон!");
                return;
            }
            if (LoginText.Text.Length == 0)
            {
                MessageBox.Show("Введите логин!");
                return;
            }
            if (PasswordText.Text.Length == 0)
            {
                MessageBox.Show("Введите пароль!");
                return;
            }
            if (photo.Source == null)
            {
                MessageBox.Show("Установите фото!");
                return;
            }

            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                procedure = "dbo.Check_logo"; // выбираем хранимую процедуру
                cmd = new SqlCommand(procedure, conn);
                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;
                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@login", LoginText.Text);

                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                if (dr.HasRows == true)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!\nПожалуйста, придумайте новый логин!");
                    LoginText.Text = "";
                    dr.Close();
                    return;
                }

                dr.Close();
            }

            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                procedure = "dbo.stp_Users_add"; // выбираем хранимую процедуру
                cmd = new SqlCommand(procedure, conn);
                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;
                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@Login", LoginText.Text);
                cmd.Parameters.AddWithValue("@Password", PasswordText.Text);

                SqlParameter u_id = cmd.Parameters.Add("@UsersID", System.Data.SqlDbType.Int);

                // если наша процедура вернет нам айди об-та, кот. мы добавим
                u_id.Direction = System.Data.ParameterDirection.Output;// говори что этот пар-р типа 'output'
                // здесь не 'value', а 'direction', тк у нас возвращать должно

                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                user_id = (int)u_id.Value;

                dr.Close();
            }

            byte[] bytes = CreateCopy();

            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                procedure = "dbo.stp_PhotosUsers_add"; // выбираем хранимую процедуру
                cmd = new SqlCommand(procedure, conn);
                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;
                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@Title", LoginText.Text);
                cmd.Parameters.AddWithValue("@Description", bytes);
                cmd.Parameters.AddWithValue("@UserID_Id", user_id);

                SqlParameter ph_u_id = cmd.Parameters.Add("@PhotoUserID", System.Data.SqlDbType.Int);

                // если наша процедура вернет нам айди об-та, кот. мы добавим
                ph_u_id.Direction = System.Data.ParameterDirection.Output;// говори что этот пар-р типа 'output'
                // здесь не 'value', а 'direction', тк у нас возвращать должно

                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                photo_user_id = (int)ph_u_id.Value;

                dr.Close();
            }

            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                procedure = "stp_Landlords_add"; // выбираем хранимую процедуру
                cmd = new SqlCommand(procedure, conn);
                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;
                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@LastName", LastNameText.Text);
                cmd.Parameters.AddWithValue("@FirstName", FirstNameText.Text);
                cmd.Parameters.AddWithValue("@Surname", SurnameText.Text);
                cmd.Parameters.AddWithValue("@Email", EmailText.Text);
                cmd.Parameters.AddWithValue("@PhoneNumber", PhoneText.Text);
                cmd.Parameters.AddWithValue("@QuantityOfProperty", int.Parse(CountCombo.Text));
                cmd.Parameters.AddWithValue("@PhotoUserID_Id", photo_user_id);
                cmd.Parameters.AddWithValue("@UserID_Id", user_id);

                SqlParameter land_id = cmd.Parameters.Add("@LandlordID", System.Data.SqlDbType.Int);

                // если наша процедура вернет нам айди об-та, кот. мы добавим
                land_id.Direction = System.Data.ParameterDirection.Output;// говори что этот пар-р типа 'output'
                // здесь не 'value', а 'direction', тк у нас возвращать должно

                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                dr.Close();

                landlord = new Landlord
                {
                    Email = EmailText.Text,
                    FirstName = FirstNameText.Text,
                    ID = int.Parse(land_id.Value.ToString()),
                    LastName = LastNameText.Text,
                    PhoneNumber = PhoneText.Text,
                    PhotoUserId = photo_user_id,
                    UserId = user_id,
                    QuantityOfProperty = int.Parse(CountCombo.Text),
                    SurName = SurnameText.Text
                };

                user_id = landlord.UserId;
                landlord_id = landlord.ID;
            }

            MessageBox.Show("Пользователь создан");

            sucess = true;

            Close();
        }
    }
}
