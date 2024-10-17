using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.Classes
{
    //формируется на этапе регистрации/авторизации 
    public class User : INotifyPropertyChanged 
    {
        //public int Id_user { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
       
        //public int isActual { get; set; }//если юзер решил удалить учетную запись  = 0 и отправляем на сервер //это у юрера не отображается

        public event PropertyChangedEventHandler PropertyChanged; //телефон, адрес, имя
        public string Name_
        {
            get { return Name; }
            set
            {
                Name = value;
                OnPropertyChanged("Name_");
            }
        }
        public string Phone_
        {
            get { return Phone; }
            set
            {
                Phone = value;
                OnPropertyChanged("Phone_");
            }
        }
        //public string Address_
        //{
        //    get { return Address; }
        //    set
        //    {
        //        Address = value;
        //        OnPropertyChanged("Address_");
        //    }
        //}
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
