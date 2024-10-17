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
using System.Configuration;
using System.Data;
using System.Security.Authentication;
using System.Xml.Linq;
using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Collections.Generic;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;
using System.Drawing;
using Отдохни.Ru.Classes;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Отдохни.Ru
{
    public partial class MainWindow : Window
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["OtdohnyBD_add"].ConnectionString;
        string procedure, fileName = "";
        public byte[] img_land;
        public int user_id, landlord_id = 0, tenant_id = 0, kol = 0, square_of_land = 0, number_of_floor = 0, np = 0;
        bool give = false, rent = false;

        // 'Bitmap' - это класс, с пом. кот. мы работаем с изображениями
        List<BitmapImage> images = new List<BitmapImage>();

        SqlConnection conn = null;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        DataSet ds_s = new DataSet();
        public DataTable dt_events = new DataTable();
        SqlDataAdapter events_adapter = new SqlDataAdapter();
        public SqlCommandBuilder cmd_events = new SqlCommandBuilder();

        Landlord landlord;
        Tenant tenant;

        public MainWindow()
        {
            InitializeComponent();
        }

        ///////////////////////////////////// заполнение обьекта недвижимости ///////////////////

        // выбрана улица - появляется поле 'дом'
        private void combo_street_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combo_number_of_house.Visibility = Visibility.Visible;
            label_number_of_house.Visibility = Visibility.Visible;
        }

        // выбран тип обьекта - появляются определенные поля
        private void combo_type_of_object_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo_type_of_object.SelectedIndex == 0)
            {
                label_square_of_region.Visibility = Visibility.Collapsed;
                txt_square_of_region.Visible = false;
                label_metres.Visibility = Visibility.Collapsed;
                label_floor.Visibility = Visibility.Visible;
                combo_number_of_floor.Visibility = Visibility.Visible;
            }
            else
            {
                label_square_of_region.Visibility = Visibility.Visible;
                txt_square_of_region.Visible = true;
                label_metres.Visibility = Visibility.Visible;
                label_floor.Visibility = Visibility.Collapsed;
                combo_number_of_floor.Visibility = Visibility.Collapsed;
            }
        }

        // выбран город - появляются определенные города
        private void combo_city_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combo_street.Items.Clear();

            if (combo_city.SelectedIndex == 0)
            {
                combo_street.Items.Add("Большая окружная");
                combo_street.Items.Add("Московский проспект");
                combo_street.Items.Add("Советский проспект");
                combo_street.Items.Add("Комсомольская");
                combo_street.Items.Add("Фрунзе");
                combo_street.Items.Add("Галковского");
                combo_street.Items.Add("Дмитрия Донского");
                combo_street.Items.Add("Мореходная");
                combo_street.Items.Add("Вагнера");
                combo_street.Items.Add("Бежецкая");
            }
            else if (combo_city.SelectedIndex == 1)
            {
                combo_street.Items.Add("Малышева");
                combo_street.Items.Add("Ленина");
                combo_street.Items.Add("Вайнера");
                combo_street.Items.Add("Крестинского");
                combo_street.Items.Add("Родонитовая");
                combo_street.Items.Add("8 Марта");
                combo_street.Items.Add("Радищева");
                combo_street.Items.Add("Степана Разина");
                combo_street.Items.Add("Академика Шварца");
                combo_street.Items.Add("Белинского");
            }
            else
            {
                combo_street.Items.Add("Карла Маркса");
                combo_street.Items.Add("Тимирязева");
                combo_street.Items.Add("Дзержинского");
                combo_street.Items.Add("Байкальская");
                combo_street.Items.Add("Урицкого");
                combo_street.Items.Add("Гагарина");
                combo_street.Items.Add("Карла Либкнехта");
                combo_street.Items.Add("Советская");
                combo_street.Items.Add("Марата");
                combo_street.Items.Add("Декабрьских Событий");
            }

            combo_street.Visibility = Visibility.Visible;
            label_street.Visibility = Visibility.Visible;
        }

        // загрузить фото обьявления
        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            // получаем выбранный файл
            string filename = openFileDialog1.FileName;

            BitmapImage pt = new BitmapImage();

            pt.BeginInit();
            pt.UriSource = new Uri(filename);
            pt.EndInit();
            img_give.Source = pt;

            MessageBox.Show("Фото загружено!");
            np++;
            images.Add(pt);
        }

        // создаем мини - копию
        private byte[] CreateCopy()
        {
            // получаем выбранный файл
            fileName = images[kol].UriSource.OriginalString;

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

        // выборка - первое окно
        private void B_Search_Click(object sender, RoutedEventArgs e)
        {
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                // выбираем хранимую процедуру, принимающую пар-ры: город, кол-во человек и тип недвижимости
                procedure = "dbo.stp_ChoseTypesAndBeds";

                cmd = new SqlCommand(procedure, conn);

                // сюда сгружаем данные // сколько у нас табличек, которые мы хотим считывать - столько адаптеров будем создавать
                events_adapter = new SqlDataAdapter();

                //// ничего не вписали в SqlDataAdapter(), тк можем воспользоваться спец командой 'InsertCommand'
                events_adapter.InsertCommand = cmd;
                events_adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                events_adapter.SelectCommand = events_adapter.InsertCommand;

                cmd.Parameters.AddWithValue("@City", CB_City.Text);
                cmd.Parameters.AddWithValue("@QuantityOfBed", int.Parse(CB_People.Text));
                cmd.Parameters.AddWithValue("@NameOfType", CB_type_of_object.Text);

                cmd_events = new SqlCommandBuilder(events_adapter);

                // создаем об-т 'DataSet'
                ds_s = new DataSet();

                // наш адаптер яв-ся кол-цией, у них есть метод 'Fill', создаем массив (DataSet) и придумываем ему имя
                // наши данные из 'DataAdapter' передадутся в 'DataSet' ('ds_s')
                ///DataSet - хранилище данных из таблиц из БД, получаем мы их с помощью Adapter
                events_adapter.Fill(ds_s, "Prop");

                dt_events = ds_s.Tables["Prop"];
                data_main.DataContext = null;

                // заполняем наш DataGrid полученной таблицей изподходящих обектов
                data_main.DataContext = ds_s.Tables["Prop"];

                if (data_main.Items.Count == 1)
                    System.Windows.MessageBox.Show("К сожалению, таких обьектов сейчас нет!");
            }
        }

        // добавить обьявление
        private async void place_an_add_Click(object sender, RoutedEventArgs e)
        {
            // все поля должны быть заполнены // проверка 
            // сообщения о том, какое именно поле не заполнено - курсор переходи на него
            if (combo_city.SelectedItem == null)
            {
                MessageBox.Show("Выберите город!");
                combo_city.Focus();
                return;
            }

            if (combo_street.SelectedItem == null)
            {
                MessageBox.Show("Выберите улицу!");
                combo_street.Focus();
                return;
            }

            if (combo_number_of_house.SelectedItem == null)
            {
                MessageBox.Show("Выберите номер дома!");
                combo_number_of_house.Focus();
                return;
            }

            if (combo_type_of_object.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип обьекта!");
                combo_type_of_object.Focus();
                return;
            }

            if (combo_number_of_floor.SelectedItem == null && combo_number_of_floor.Visibility != Visibility.Collapsed)
            {
                MessageBox.Show("Выберите этаж!");
                combo_number_of_floor.Focus();
                return;
            }

            if (combo_count_of_floors.SelectedItem == null)
            {
                MessageBox.Show("Выберите количество этажей в доме!");
                combo_count_of_floors.Focus();
                return;
            }

            if (count_rooms.SelectedItem == null)
            {
                MessageBox.Show("Выберите количество комнат!");
                count_rooms.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_square_of_object.Text))
            {
                MessageBox.Show("Выберите площадь обьекта!");
                txt_square_of_object.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_square_of_region.Text) && txt_square_of_region.Visible == true)
            {
                MessageBox.Show("Выберите площадь участка!");
                txt_square_of_region.Focus();
                return;
            }

            if (count_of_beds.SelectedItem == null)
            {
                MessageBox.Show("Выберите количество кроватей!");
                count_of_beds.Focus();
                return;
            }

            if (type_of_bed.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип кровати!");
                type_of_bed.Focus();
                return;
            }

            if (combo_max_guests.SelectedItem == null)
            {
                MessageBox.Show("Выберите максимальное количество гостей!");
                combo_max_guests.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_description.Text))
            {
                MessageBox.Show("Добавьте описание!");
                txt_description.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_cost.Text))
            {
                MessageBox.Show("Укажите стоимость!");
                txt_cost.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_deposit.Text))
            {
                MessageBox.Show("Укажите размер депозита!");
                img_give.Focus();
                return;
            }

            if (images.Count == 0)
            {
                MessageBox.Show("Добавьте фотографии!");
                txt_deposit.Focus();
                return;
            }

            // порядковый номер картинки изначально 0 - тк их еще нет
            np = 0;
            // во вкладку "ЛК Админа" попадает инф-ия об обяв-ии, которое хотят опубликовать
            admin_txt_desc.Text = txt_description.Text;
            // а также картинки обьявления
            admin_image.Source = new BitmapImage(images[np].UriSource);
            // сколько всего картинок у обьявления
            label_end.Content = images.Count;
            // начальный номер кариник
            label_begin.Content = 1;

            // Формируем сообщение
            string message = "Новое обьявление:";

            string recipientPhoneNumber = "79039127706"; // Номер получателя
            string messageToSend = message;

            // Асинхронная отправка email
            await SendEmailAsync(recipientPhoneNumber, messageToSend);

            MessageBox.Show("Обьявление отправлено на проверку!");

            // переходим на вкладку "ЛК Админа"
            tab_control.SelectedItem = TI_K_Admin;
            // вкладка "ЛК Админа" становится видимой
            TI_K_Admin.Visibility = Visibility.Visible;
        }

        // отправка уведомления
        private async Task SendEmailAsync(string recipientPhoneNumber, string message)
        {
            //try
            //{
            //    // Создаем новое сообщение
            //    var emailMessage = new MimeMessage();
            //    emailMessage.From.Add(new MailboxAddress("Ваше имя", "kiina82@bk.ru")); // Ваш адрес
            //    emailMessage.To.Add(new MailboxAddress("", $"{recipientPhoneNumber}@sms.beemail.ru")); // Адрес получателя через SMS Gateway
            //    emailMessage.Subject = "SMS"; // Заголовок
            //    emailMessage.Body = new TextPart("plain")
            //    {
            //        Text = message // Текст сообщения
            //    };

            //    // Используем SMTP-клиент для отправки сообщения
            //    using (var client = new SmtpClient())
            //    {
            //        // Подключаемся к SMTP-серверу
            //        await client.ConnectAsync("smtp.mail.ru", 587, false); //адрес  SMTP-сервера и порт
            //        await client.AuthenticateAsync("kiina82@bk.ru", "KmWfGa1wgjwhhmF4JVUJ"); // email и пароль
            //        await client.SendAsync(emailMessage); // отправляем сообщение
            //        await client.DisconnectAsync(true); // отключаемся от сервера
            //    }
            //    if (message == "Новое обьявление:")
            //    {
            //        admin_txt_desc.Text = txt_description.Text;
            //        label_end.Content = images.Count.ToString();
            //        label_begin.Content = kol + 1;

            //        MessageBox.Show("Ваше сообщение отправлено на проверку!");
            //    }
            //}
            //catch (AuthenticationException ex)
            //{
            //    MessageBox.Show($"Ошибка аутентификации: {ex.Message}");
            //}
            //catch (SmtpCommandException ex)
            //{
            //    MessageBox.Show($"Ошибка SMTP: {ex.Message}");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Общая ошибка: {ex.Message}");
            //}
        }

        // очистка всех полей, которые были заполнены арендодателм для того,
        // чтобы он смог подавать еще обьявления
        private void ClearFields()
        {
            combo_type_of_object.SelectedIndex = -1;
            combo_city.SelectedIndex = -1;
            combo_street.SelectedIndex = -1;
            combo_number_of_house.SelectedIndex = -1;
            rb_sauna.IsChecked = false;
            rb_swimming_pool.IsChecked = false;
            combo_number_of_floor.SelectedIndex = -1;
            combo_count_of_floors.SelectedIndex = -1;
            count_rooms.SelectedIndex = -1;
            rb_balcony.IsChecked = false;
            txt_square_of_object.Text = string.Empty;
            txt_square_of_region.Text = string.Empty;
            rb_parking.IsChecked = false;
            count_of_beds.SelectedIndex = -1;
            type_of_bed.SelectedIndex = -1;
            rb_conditioner.IsChecked = false;
            rb_television.IsChecked = false;
            rb_wi_fi.IsChecked = false;
            rb_bed_linen.IsChecked = false;
            rb_towels.IsChecked = false;
            combo_max_guests.SelectedIndex = -1;
            rb_children.IsChecked = false;
            rb_pets.IsChecked = false;
            rb_smoking.IsChecked = false;
            rb_parties.IsChecked = false;
            rb_docs.IsChecked = false;
            txt_description.Text = string.Empty;
            txt_cost.Text = string.Empty;
            txt_deposit.Text = string.Empty;
            img_give.Source = null;
        }

        // загружаем данные по арендатору и выгружаем все обьявления для просмотра
        private void Load_Tenant()
        {
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                int number_of_row = 1;

                // находим все объявления по обьектам - заполняем ими вкладку "Снять"
                while (true)
                {
                    // все происходит в цикле - находим первое обьявление - выводим информацию о нем

                    // выбираем хранимую процедуру
                    procedure = "dbo.Show_prop_on_id";

                    cmd = new SqlCommand(procedure, conn);

                    // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                    cmd.CommandType = CommandType.StoredProcedure;

                    // входящие пар-ры
                    cmd.Parameters.AddWithValue("@id", number_of_row);

                    // достаем строку, а это не одно значение, а целый массив записей
                    dr = cmd.ExecuteReader();

                    // если есть обь-т с таким id
                    if (dr.HasRows == true)
                    {
                        // создаем наши элементы управления

                        // 1
                        StackPanel main_stack = new StackPanel();
                        main_stack.Orientation = System.Windows.Controls.Orientation.Horizontal;

                        // 2
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        image.Margin = new Thickness(10);
                        StackPanel in_main_stack = new StackPanel();

                        // 3
                        StackPanel mini_stack_price = new StackPanel();
                        mini_stack_price.Orientation = System.Windows.Controls.Orientation.Horizontal;

                        StackPanel mini_stack_rating = new StackPanel();
                        mini_stack_rating.Orientation = System.Windows.Controls.Orientation.Horizontal;

                        StackPanel mini_stack_conn = new StackPanel();
                        mini_stack_conn.Orientation = System.Windows.Controls.Orientation.Horizontal;

                        // 4 
                        System.Windows.Controls.Label label_price = new System.Windows.Controls.Label();
                        label_price.Content = "Стоимость:";
                        System.Windows.Controls.Label label_rating = new System.Windows.Controls.Label();
                        label_rating.Content = "Рейтинг:";


                        System.Windows.Controls.TextBox txt_header = new System.Windows.Controls.TextBox();
                        System.Windows.Controls.TextBox txt_description = new System.Windows.Controls.TextBox();

                        System.Windows.Controls.Label txt_price = new System.Windows.Controls.Label();
                        System.Windows.Controls.Label txt_rating = new System.Windows.Controls.Label();

                        System.Windows.Controls.Button btn_call = new System.Windows.Controls.Button();
                        btn_call.Content = "Позвонить";
                        System.Windows.Controls.Button btn_write = new System.Windows.Controls.Button();
                        btn_write.Content = "Написать";
                        System.Windows.Controls.Button btn_booking = new System.Windows.Controls.Button();
                        btn_booking.Content = "Забронировать";

                        mini_stack_conn.Children.Add(btn_call);
                        mini_stack_conn.Children.Add(btn_write);
                        mini_stack_conn.Children.Add(btn_booking);

                        mini_stack_rating.Children.Add(label_rating);
                        mini_stack_rating.Children.Add(txt_rating);

                        mini_stack_price.Children.Add(label_price);
                        mini_stack_price.Children.Add(txt_price);

                        in_main_stack.Children.Add(txt_header);
                        in_main_stack.Children.Add(mini_stack_price);
                        in_main_stack.Children.Add(txt_description);
                        in_main_stack.Children.Add(mini_stack_rating);
                        in_main_stack.Children.Add(mini_stack_conn);

                        main_stack.Children.Add(image);
                        main_stack.Children.Add(in_main_stack);

                        // создаем строку для добавления в грид
                        RowDefinition rd = new RowDefinition();

                        // добавляем ее
                        all_prop_grid.RowDefinitions.Add(rd);

                        // указываем ей расположение
                        Grid.SetRow(main_stack, number_of_row - 1);

                        all_prop_grid.Children.Add(main_stack);

                        while (dr.Read())
                        {
                            txt_header.Text = (string)dr[1] + ", " + (string)dr[2] + ", " + (string)dr[3];
                            txt_price.Content = dr[8].ToString();
                            txt_description.Text = "Площадь: " + dr[5].ToString() + "\nКоличество комнат: " + dr[6].ToString() + "\nКоличество кроватей: "
                                + dr[7].ToString() + "\nДепозит:" + dr[9].ToString() + "\nСобственник: " + (string)dr[0];
                        }

                        dr.Close();

                        // находим фото обьявления - все это вставляем в созданные по ходу элементы управления

                        // выбираем хранимую процедуру
                        procedure = "dbo.Show_photo_on_prop";

                        cmd = new SqlCommand(procedure, conn);

                        // сюда сгружаем данные // сколько у нас табличек, которые мы хотим считывать - столько адаптеров будем создавать
                        events_adapter = new SqlDataAdapter();

                        //// ничего не вписали в SqlDataAdapter(), тк можем воспользоваться спец командой 'InsertCommand'
                        events_adapter.InsertCommand = cmd;
                        events_adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        events_adapter.SelectCommand = events_adapter.InsertCommand;

                        // входяшие пар-ры
                        cmd.Parameters.AddWithValue("@id", number_of_row);

                        cmd_events = new SqlCommandBuilder(events_adapter);

                        // создаем об-т 'DataSet'
                        ds_s = new DataSet();

                        // наш адаптер яв-ся кол-цией, у них есть метод 'Fill', создаем массив (DataSet) и придумываем ему имя
                        // наши данные из 'DataAdapter' передадутся в 'DataSet' ('ds_s')
                        ///DataSet - хранилище данных из таблиц из БД, получаем мы их с помощью Adapter
                        events_adapter.Fill(ds_s, "Img");

                        try
                        {
                            // получаем картинку ввиде массива байт
                            img_land = (byte[])ds_s.Tables[0].Rows[0]["Description"];

                            MemoryStream stream = new MemoryStream(img_land);
                            System.Drawing.Image imgg = System.Drawing.Image.FromStream(stream);
                            BitmapImage returnImage = new BitmapImage();
                            returnImage.BeginInit();

                            MemoryStream mss = new MemoryStream();
                            imgg.Save(mss, System.Drawing.Imaging.ImageFormat.Bmp);
                            mss.Seek(0, SeekOrigin.Begin);
                            returnImage.StreamSource = mss;
                            returnImage.EndInit();

                            // загружаем картинку в элемент управления
                            image.Source = returnImage;
                        }
                        catch (Exception)
                        {

                        }

                        dr.Close();

                        // переходим к следующему элементу управления
                        number_of_row++;
                    }
                    // если такого id не сущ-ет - выходим из цикла-обьявлений больше нет
                    else
                    {
                        dr.Close();
                        break;
                    }
                }
            }

            if (tenant_id == 0)
            {
                using (conn = new SqlConnection(connectionstring))
                {
                    conn.Open();

                    procedure = "dbo.Find_tenant_id"; // выбираем хранимую процедуру
                    cmd = new SqlCommand(procedure, conn);
                    // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                    cmd.CommandType = CommandType.StoredProcedure;
                    // нужно быть уверенным, что второй пар-р нужного типа
                    cmd.Parameters.AddWithValue("@login", LoginText.Text);

                    dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                    while (dr.Read())
                        tenant_id = (int)dr[0];

                    dr.Close();
                }
            }

            // теперь получаем информацию об арендаторе для того, чтобы заполнить его ЛК
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                // выбираем хранимую процедуру
                procedure = "dbo.Info_tenant_id";

                cmd = new SqlCommand(procedure, conn);

                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;

                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@id", tenant_id);

                // достаем строку, а это не одно значение, а целый массив записей
                dr = cmd.ExecuteReader();

                // создаем арендатора
                tenant = new Tenant();

                while (dr.Read())
                {
                    tenant.LastName = (string)dr[1];
                    tenant.FirstName = (string)dr[2];
                    tenant.SurName = (string)dr[3];
                    tenant.Email = (string)dr[4];
                    tenant.PhoneNumber = (string)dr[5];
                    tenant.DateBirth = (DateTime)dr[6];
                    tenant.PhotoUserId = (int)dr[7];
                    tenant.UserId = (int)dr[8];
                }

                dr.Close();
            }

            // достаем данные об арендаторе и помещаем их в элементы управления
            guest_name.Text = tenant.FirstName;
            guest_lastname.Text = tenant.LastName;
            guest_surname.Text = tenant.SurName;
            guest_tel.Text = tenant.PhoneNumber;
            guest_email.Text = tenant.Email;
            guest_birth.Text = tenant.DateBirth.ToString();

            // нам нужно еще фото арендатора
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                // выбираем хранимую процедуру
                procedure = "dbo.Show_photo_tenant";

                cmd = new SqlCommand(procedure, conn);

                // сюда сгружаем данные // сколько у нас табличек, которые мы хотим считывать - столько адаптеров будем создавать
                events_adapter = new SqlDataAdapter();

                //// ничего не вписали в SqlDataAdapter(), тк можем воспользоваться спец командой 'InsertCommand'
                events_adapter.InsertCommand = cmd;
                events_adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                events_adapter.SelectCommand = events_adapter.InsertCommand;

                cmd.Parameters.AddWithValue("@id", tenant_id);

                cmd_events = new SqlCommandBuilder(events_adapter);

                // создаем об-т 'DataSet'
                ds_s = new DataSet();

                // наш адаптер яв-ся кол-цией, у них есть метод 'Fill', создаем массив (DataSet) и придумываем ему имя
                // наши данные из 'DataAdapter' передадутся в 'DataSet' ('ds_s')
                ///DataSet - хранилище данных из таблиц из БД, получаем мы их с помощью Adapter
                events_adapter.Fill(ds_s, "Img");

                // получаем его в виде массива байт
                img_land = (byte[])ds_s.Tables[0].Rows[0]["Description"];

                MemoryStream stream = new MemoryStream(img_land);
                System.Drawing.Image imgg = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();

                MemoryStream mss = new MemoryStream();
                imgg.Save(mss, System.Drawing.Imaging.ImageFormat.Bmp);
                mss.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = mss;
                returnImage.EndInit();

                // помещаем фото в элемент управления
                img_guest.Source = returnImage;

                dr.Close();
            }

            // переходим на вкладку - "Снять"
            tab_control.SelectedItem = TI_Rent;
        }

        // загружаем данные по арендодателю 
        private void Load_Landloard()
        {
            if (landlord_id == 0)
            {
                using (conn = new SqlConnection(connectionstring))
                {
                    conn.Open();

                    procedure = "dbo.Find_landlord_id"; // выбираем хранимую процедуру
                    cmd = new SqlCommand(procedure, conn);
                    // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                    cmd.CommandType = CommandType.StoredProcedure;
                    // нужно быть уверенным, что второй пар-р нужного типа
                    cmd.Parameters.AddWithValue("@login", LoginText.Text);

                    dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                    while (dr.Read())
                        landlord_id = (int)dr[0];

                    dr.Close();
                }
            }

            // получаем информацию об арендодателе, чтобы заполнить его ЛК
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                // выбираем хранимую процедуру
                procedure = "dbo.Info_landlord_id";

                cmd = new SqlCommand(procedure, conn);

                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;

                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@id", landlord_id);

                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                // создаем нового арендодателя
                landlord = new Landlord();

                while (dr.Read())
                {
                    landlord.LastName = (string)dr[1];
                    landlord.FirstName = (string)dr[2];
                    landlord.SurName = (string)dr[3];
                    landlord.Email = (string)dr[4];
                    landlord.PhoneNumber = (string)dr[5];
                    landlord.QuantityOfProperty = (int)dr[6];
                    landlord.PhotoUserId = (int)dr[7];
                    landlord.UserId = (int)dr[8];
                }

                dr.Close();
            }

            // заполняем информацию об арендодателе в элементах управления
            renter_name.Text = landlord.FirstName;
            renter_lastname.Text = landlord.LastName;
            renter_surname.Text = landlord.SurName;
            renter_phone.Text = landlord.PhoneNumber;
            renter_email.Text = landlord.Email;

            // получаем фото арендодателя
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                procedure = "dbo.Show_photo_landlord"; // выбираем хранимую процедуру

                cmd = new SqlCommand(procedure, conn);

                // сюда сгружаем данные // сколько у нас табличек, которые мы хотим считывать - столько адаптеров будем создавать
                events_adapter = new SqlDataAdapter();

                //// ничего не вписали в SqlDataAdapter(), тк можем воспользоваться спец командой 'InsertCommand'
                events_adapter.InsertCommand = cmd;
                events_adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                events_adapter.SelectCommand = events_adapter.InsertCommand;

                cmd.Parameters.AddWithValue("@id", landlord_id);

                cmd_events = new SqlCommandBuilder(events_adapter);
                // создаем об-т 'DataSet'
                ds_s = new DataSet();

                // наш адаптер яв-ся кол-цией, у них есть метод 'Fill', создаем массив (DataSet) и придумываем ему имя
                // наши данные из 'DataAdapter' передадутся в 'DataSet' ('ds_s')
                ///DataSet - хранилище данных из таблиц из БД, получаем мы их с помощью Adapter
                events_adapter.Fill(ds_s, "Img");

                try
                {
                    img_land = (byte[])ds_s.Tables[0].Rows[0]["Description"];

                    MemoryStream stream = new MemoryStream(img_land);
                    System.Drawing.Image imgg = System.Drawing.Image.FromStream(stream);
                    BitmapImage returnImage = new BitmapImage();
                    returnImage.BeginInit();

                    MemoryStream mss = new MemoryStream();
                    imgg.Save(mss, System.Drawing.Imaging.ImageFormat.Bmp);
                    mss.Seek(0, SeekOrigin.Begin);
                    returnImage.StreamSource = mss;
                    returnImage.EndInit();

                    img_landlord.Source = returnImage;
                }
                catch (Exception)
                {
                }

                dr.Close();
            }

            Show_all_prop_of_landlord();

            // переходим на вкладку - "Сдать"
            tab_control.SelectedItem = TI_Give;
        }

        // создаем арендатора
        private void Create_Tenant()
        {
            // создаем арендатора в новом окне
            Window_tenant window_Tenant = new Window_tenant();
            window_Tenant.ShowDialog();

            // эти два id нам понадобятся в дальнейшем
            // запоминаем id арендатора (номер в табличке "users" логин и пароль для входа)
            user_id = window_Tenant.UserId;
            // запоминаем id арендатора
            tenant_id = window_Tenant.TenantUserId;

            // если в окне для создания арендатора все прошло успешно, пользователь зарегистрировался, 
            // заполнил все поля, не нажимал крестик - 'sucess' = true
            if (window_Tenant.Sucess == true)
            {
                // попадаем на вкладку - "Снять"
                tab_control.SelectedItem = TI_Rent;

                // открываем видимость двух вкладок - "Снять" и "ЛК гостя"
                TI_Rent.Visibility = Visibility.Visible;
                TI_K_Guest.Visibility = Visibility.Visible;

                Load_Tenant();
            }
        }

        // создаем арендодателя
        private void Create_Landlord()
        {
            // создаем в отдельном окне арендодателя
            Window_landlord window_Landlord = new Window_landlord();
            window_Landlord.ShowDialog();

            // если в окне для создания арендодателя все прошло успешно, пользователь зарегистрировался, 
            // заполнил все поля, не нажимал крестик - 'sucess' = true
            if (window_Landlord.Sucess == true)
            {
                // эти два id нам понадобятся в дальнейшем
                // запоминаем id арендодателя (номер в табличке "users" логин и пароль для входа)
                user_id = window_Landlord.UserId;
                landlord_id = window_Landlord.LandlordUserId;

                // переходим на вкладку "Сдать"
                tab_control.SelectedItem = TI_Give;

                // вкладки "Сдать" и  "ЛК Дателя" становятся видимыми
                TI_Give.Visibility = Visibility.Visible;
                TI_K_Renter.Visibility = Visibility.Visible;

                Load_Landloard();
            }
        }

        // если выбрано - 'арендатор'
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Create_Tenant();
        }

        // если выбрано - 'арендодатель'
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Create_Landlord();
        }

        // кнопка 'сдать'
        private void btn_give_Click_2(object sender, RoutedEventArgs e)
        {
            give = true;

            // всплывает дополнительоне окно, чтобы получить инф-ию о том, яв-ся ли пользователь зарегистрированным
            Window_reg_or_entry window_reg = new Window_reg_or_entry();
            window_reg.ShowDialog();

            // если да - переходим на вкладку - "Вход"
            if (window_reg.Reg == true)
                tab_control.SelectedItem = TI_Entry;
            // если нет - переходим на вкладку - "Регистрация"
            else
            {
                tab_control.SelectedItem = TI_Reg;
                Create_Landlord();
            }
        }

        // кнопка 'снять'
        private void btn_rent_Click_1(object sender, RoutedEventArgs e)
        {
            rent = true;

            // всплывает дополнительоне окно, чтобы получить инф-ию о том, яв-ся ли пользователь зарегистрированным
            Window_reg_or_entry window_reg = new Window_reg_or_entry();
            window_reg.ShowDialog();

            // если да - переходим на вкладку - "Вход"
            if (window_reg.Reg == true)
                tab_control.SelectedItem = TI_Entry;
            // если нет - переходим на вкладку - "Регистрация"
            else
            {
                tab_control.SelectedItem = TI_Reg;
                Create_Tenant();
            }
        }

        // отклонить обьявление
        private async void btn_regect_Click(object sender, RoutedEventArgs e)
        {
            // Формируем сообщение
            string message = "Ваше обьявление отклонено!";

            string recipientPhoneNumber = "79039127706"; // Номер получателя
            string messageToSend = message;

            // Асинхронная отправка email
            await SendEmailAsync(recipientPhoneNumber, messageToSend);

            tab_control.SelectedItem = TI_Give;

            MessageBox.Show("Ваше обьявление отклонено!");

            TI_K_Admin.Visibility = Visibility.Visible;
        }

        // опубликовать обьявление
        private async void btn_publish_Click(object sender, RoutedEventArgs e)
        {
            int type_id_int, prop_id_int;

            bool sauna = false, parking = false, swiming_pool = false, balcony = false, condition = false,
                tv = false, wifi = false, bedlinen = false, towel = false, pets = false, children = false,
                smoking = false, party = false, docs = false;

            if (rb_sauna.IsChecked == true)
                sauna = true;

            if (rb_parking.IsChecked == true)
                parking = true;

            if (rb_swimming_pool.IsChecked == true)
                swiming_pool = true;

            if (rb_balcony.IsChecked == true)
                balcony = true;

            if (rb_conditioner.IsChecked == true)
                condition = true;

            if (rb_television.IsChecked == true)
                tv = true;

            if (rb_wi_fi.IsChecked == true)
                wifi = true;

            if (rb_bed_linen.IsChecked == true)
                bedlinen = true;

            if (rb_towels.IsChecked == true)
                towel = true;

            if (rb_pets.IsChecked == true)
                pets = true;

            if (rb_children.IsChecked == true)
                children = true;

            if (rb_smoking.IsChecked == true)
                smoking = true;

            if (rb_parties.IsChecked == true)
                party = true;

            if (rb_docs.IsChecked == true)
                docs = true;

            // прежде чем добавить обьявление - сначала создадим тип обявления
            // тк у нас отдельно есть табличка с типами
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                // выбираем хранимую процедуру
                procedure = "dbo.stp_TypesOfProperties_add";
                cmd = new SqlCommand(procedure, conn);

                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;

                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@Name", combo_type_of_object.Text);

                SqlParameter type_id = cmd.Parameters.Add("@TypesOfPropertiesID", System.Data.SqlDbType.Int);

                // если наша процедура вернет нам айди об-та, кот. мы добавим
                type_id.Direction = System.Data.ParameterDirection.Output;// говори что этот пар-р типа 'output'
                                                                          // здесь не 'value', а 'direction', тк у нас возвращать должно
                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                type_id_int = (int)type_id.Value;

                dr.Close();
            }

            // теперь добавим обьявление с существующим типом в табличку обьявлений 
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                if (label_square_of_region.Visibility != Visibility.Collapsed)
                    square_of_land = txt_square_of_region.Text.Length;

                if (label_floor.Visibility != Visibility.Collapsed)
                    number_of_floor = int.Parse(combo_number_of_floor.Text);

                procedure = "dbo.stp_Properties_add"; // выбираем хранимую процедуру
                cmd = new SqlCommand(procedure, conn);
                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;
                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@City", combo_city.Text);
                cmd.Parameters.AddWithValue("@Street", combo_street.Text);
                cmd.Parameters.AddWithValue("@NumderOfHome", combo_number_of_house.Text);
                cmd.Parameters.AddWithValue("@IsBanyaOrSauna", sauna);
                cmd.Parameters.AddWithValue("@QuantityOfFloorsInBuilding", int.Parse(combo_count_of_floors.Text));
                cmd.Parameters.AddWithValue("@QuantityOfRooms", float.Parse(count_rooms.Text));
                cmd.Parameters.AddWithValue("@SquareOfProperty", float.Parse(txt_square_of_object.Text));
                cmd.Parameters.AddWithValue("@SquareOfLand", square_of_land);
                cmd.Parameters.AddWithValue("@IsParking", parking);
                cmd.Parameters.AddWithValue("@IsWaterpool", swiming_pool);
                cmd.Parameters.AddWithValue("@Floor", number_of_floor);
                cmd.Parameters.AddWithValue("@IsBalconyOrLoggia", balcony);
                cmd.Parameters.AddWithValue("@QuantityOfBed", int.Parse(count_of_beds.Text));
                cmd.Parameters.AddWithValue("@TypeOfBed", type_of_bed.Text);
                cmd.Parameters.AddWithValue("@Cost", int.Parse(txt_cost.Text));
                cmd.Parameters.AddWithValue("@Deposit", int.Parse(txt_deposit.Text));
                cmd.Parameters.AddWithValue("@IsConditioner", condition);
                cmd.Parameters.AddWithValue("@IsTV", tv);
                cmd.Parameters.AddWithValue("@IsWiFi", wifi);
                cmd.Parameters.AddWithValue("@IsBedLinen", bedlinen);
                cmd.Parameters.AddWithValue("@IsTowel", towel);
                cmd.Parameters.AddWithValue("@MaxGuests", int.Parse(combo_max_guests.Text));
                cmd.Parameters.AddWithValue("@IsAvailablePets", pets);
                cmd.Parameters.AddWithValue("@IsAvailableChildren", children);
                cmd.Parameters.AddWithValue("@IsAvailableSmoking", smoking);
                cmd.Parameters.AddWithValue("@IsAvailableParties", party);
                cmd.Parameters.AddWithValue("@IsAccountingDocs", docs);
                cmd.Parameters.AddWithValue("@Description", txt_description.Text);
                cmd.Parameters.AddWithValue("@LandlordID_Id", landlord_id);
                cmd.Parameters.AddWithValue("@TypeOfPropertyID_Id", type_id_int);

                SqlParameter prop_id = cmd.Parameters.Add("@PropertiesID", System.Data.SqlDbType.Int);

                // если наша процедура вернет нам айди об-та, кот. мы добавим
                prop_id.Direction = System.Data.ParameterDirection.Output;// говори что этот пар-р типа 'output'
                                                                          // здесь не 'value', а 'direction', тк у нас возвращать должно

                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                prop_id_int = (int)prop_id.Value;

                dr.Close();
            }

            // теперь добавим все фото обьявления в табличку с фото и привяжем их к обьявлению
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                while (kol < images.Count)
                {
                    byte[] bytes = CreateCopy();

                    procedure = "dbo.stp_Photos_add"; // выбираем хранимую процедуру
                    cmd = new SqlCommand(procedure, conn);
                    // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                    cmd.CommandType = CommandType.StoredProcedure;
                    // нужно быть уверенным, что второй пар-р нужного типа
                    cmd.Parameters.AddWithValue("@Title", $"photo_of + {prop_id_int} property");
                    cmd.Parameters.AddWithValue("@Description", bytes);
                    cmd.Parameters.AddWithValue("@PropertyID_Id", prop_id_int);

                    SqlParameter photo_id = cmd.Parameters.Add("@PhotoID", System.Data.SqlDbType.Int);

                    // если наша процедура вернет нам айди об-та, кот. мы добавим
                    photo_id.Direction = System.Data.ParameterDirection.Output;// говори что этот пар-р типа 'output'

                    dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                    dr.Close();

                    kol++;
                }
            }

            TI_K_Admin.Visibility = Visibility.Collapsed;
            admin_image.Source = null;
            images.Clear();

            kol = 0;

            tab_control.SelectedItem = TI_Give;

            MessageBox.Show("Обьявление опубликовано!");

            ClearFields();

            // Формируем сообщение
            string message = "Ваше обьявление опубликовано!";

            string recipientPhoneNumber = "79039127706"; // Номер получателя
            string messageToSend = message;

            // Асинхронная отправка email
            await SendEmailAsync(recipientPhoneNumber, messageToSend);

            Show_all_prop_of_landlord();
        }

        private void Show_all_prop_of_landlord()
        {
            // покажем все обьявления арендодателя в его ЛК
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                // выбираем хранимую процедуру, принимающую пар-ры: город, кол-во человек и тип недвижимости
                procedure = "dbo.Show_all_prop_of_land";

                cmd = new SqlCommand(procedure, conn);

                // сюда сгружаем данные // сколько у нас табличек, которые мы хотим считывать - столько адаптеров будем создавать
                events_adapter = new SqlDataAdapter();

                //// ничего не вписали в SqlDataAdapter(), тк можем воспользоваться спец командой 'InsertCommand'
                events_adapter.InsertCommand = cmd;
                events_adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                events_adapter.SelectCommand = events_adapter.InsertCommand;

                cmd.Parameters.AddWithValue("@id", landlord_id);

                cmd_events = new SqlCommandBuilder(events_adapter);

                // создаем об-т 'DataSet'
                ds_s = new DataSet();

                // наш адаптер яв-ся кол-цией, у них есть метод 'Fill', создаем массив (DataSet) и придумываем ему имя
                // наши данные из 'DataAdapter' передадутся в 'DataSet' ('ds_s')
                ///DataSet - хранилище данных из таблиц из БД, получаем мы их с помощью Adapter
                events_adapter.Fill(ds_s, "Prop_land");

                dt_events = ds_s.Tables["Prop_land"];
                Data_grid.DataContext = null;

                // заполняем наш DataGrid полученной таблицей изподходящих обектов
                Data_grid.DataContext = ds_s.Tables["Prop_land"];
            }
        }

        // авторизация
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
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

            using (conn = new SqlConnection(connectionstring))
            {
                string password = "";

                conn.Open();

                // выбираем хранимую процедуру // есть ли такой логи
                procedure = "dbo.Check_logo";

                cmd = new SqlCommand(procedure, conn);
                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                cmd.CommandType = CommandType.StoredProcedure;
                // нужно быть уверенным, что второй пар-р нужного типа
                cmd.Parameters.AddWithValue("@login", LoginText.Text);

                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        password = (string)dr[2];
                        user_id = (int)dr[0];
                    }

                    // правильно ли введен пароль
                    if (password == PasswordText.Text)
                    {
                        dr.Close();

                        MessageBox.Show("Авторизация прошла успешно!");

                        give = false;
                        rent = false;

                        // узнаем арендатором или арендодателем является пользователь
                        if (give == false && rent == false)
                        {
                            // проверка на арендатора
                            using (conn = new SqlConnection(connectionstring))
                            {
                                conn.Open();

                                // выбираем хранимую процедуру
                                procedure = "dbo.stp_IsTenant";
                                cmd = new SqlCommand(procedure, conn);

                                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                                cmd.CommandType = CommandType.StoredProcedure;

                                // нужно быть уверенным, что второй пар-р нужного типа
                                cmd.Parameters.AddWithValue("@login", LoginText.Text);

                                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                                if (dr.HasRows == true)
                                    rent = true;

                                dr.Close();
                            }

                            // проверка на арендодателя
                            using (conn = new SqlConnection(connectionstring))
                            {
                                conn.Open();

                                // выбираем хранимую процедуру
                                procedure = "stp_IsLandlord";
                                cmd = new SqlCommand(procedure, conn);

                                // определаем у нашей команды тип - 'StoredProcedure', дефолтно - текст (как раньше делали)
                                cmd.CommandType = CommandType.StoredProcedure;

                                // нужно быть уверенным, что второй пар-р нужного типа
                                cmd.Parameters.AddWithValue("@login", LoginText.Text);

                                dr = cmd.ExecuteReader(); // достаем строку, а это не одно значение, а целый массив записей

                                if (dr.HasRows == true)
                                    give = true;

                                dr.Close();
                            }
                        }

                        // если арендатор
                        if (give == true)
                        {
                            TI_K_Renter.Visibility = Visibility.Visible;
                            tab_control.SelectedItem = TI_Give;
                            TI_Give.Visibility = Visibility.Visible;

                            Load_Landloard();

                            LoginText.Text = "";
                            PasswordText.Text = "";

                        }
                        // если арендодатель
                        else if (rent == true)
                        {
                            TI_K_Guest.Visibility = Visibility.Visible;
                            tab_control.SelectedItem = TI_Rent;
                            TI_Rent.Visibility = Visibility.Visible;

                            Load_Tenant();

                            LoginText.Text = "";
                            PasswordText.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неправильный пароль!\nПопробуйте еще раз!");
                        PasswordText.Text = "";
                        dr.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином не существует!\nПожалуйста, зарегистрируйтесь!");
                    LoginText.Text = "";
                    PasswordText.Text = "";
                    dr.Close();
                    tab_control.SelectedItem = TI_Reg;
                    choose_type_user.Visibility = Visibility.Visible;
                }
            }
        }

        // листаем фото вперед
        private void btn_forward_Click(object sender, RoutedEventArgs e)
        {
            if (np < images.Count - 1)
            {
                np++;
                admin_image.Source = new BitmapImage(images[np].UriSource);
                label_begin.Content = np + 1;
            }
        }

        // листаем фото назад
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            if (np > 0)
            {
                np--;
                admin_image.Source = new BitmapImage(images[np].UriSource);
                label_begin.Content = np + 1;
            }
        }

        // Telegram
        private void StartTGinside_Click(object sender, RoutedEventArgs e)
        {
            TelegramVW.Url = "https://web.telegram.org/a/#7657815829";
        }

        // Telegram - web
        private void StartTGoutside_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://web.telegram.org/a/#7657815829") { UseShellExecute = true });
        }

        // Wats'Up
        private void StartWAoutside_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://wa.me/+79039127706") { UseShellExecute = true });
        }

        /////////////////////////////////// недоделки /////////////////////////////////////////////

        // посмотреть выбранное обьявление целиком
        private void btn_show_info_of_prop_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        // редактировать данные арендодателя
        private void edit_info_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        // удалить профиль арендодателя
        private void del_info_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        // редактировать данные арендатора
        private void info_edit_Click_1(object sender, RoutedEventArgs e)
        {
            //
        }

        // удалить данные арендатора
        private void info_del_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
