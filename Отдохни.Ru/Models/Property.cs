using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.Models
{
    [Table(name: "Properties")]
    public class Property
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string City { get; set; } //город
        [Required]
        public string Street { get; set; } //улица
        [Required]
        public string NumderOfHome { get; set; } //номер дома
        [Required]
        public bool IsBanyaOrSauna { get; set; } //есть ли баня или сауна
        [Required]
        public int QuantityOfFloorsInBuilding { get; set; } //количество этажей в здании
        [Required]
        public int QuantityOfRooms { get; set; } //количество комнат
        [Required]
        public double SquareOfProperty { get; set; } //площадь помещения 
        [Required]
        public double SquareOfLand { get; set; } //площадь участка        
        [Required]
        public bool IsParking { get; set; } //есть парковка
        [Required]
        public bool IsWaterpool { get; set; } //есть бассейн
        [Required]
        public int Floor { get; set; } //этаж
        [Required]
        public bool IsBalconyOrLoggia { get; set; } //балкон или лоджия
        [Required]
        public int QuantityOfBed { get; set; } //количество спальных мест
        [Required]
        public string TypeOfBed { get; set; } //тип спальных мест
        [Required]
        public double Cost { get; set; }//стоймость
        public Landlord LandlordID { get; set; } //айди арендодателя
        public TypeOfProperty TypeOfPropertyID { get; set; } //айди типа объекта
        [Required]
        public double Deposit { get; set; } //залог
        [Required]
        public bool IsConditioner { get; set; } //есть кондиционер
        [Required]
        public bool IsTV { get; set; } //телевизор        
        [Required]
        public bool IsWiFi { get; set; }//вай-фай        
        [Required]
        public bool IsBedLinen { get; set; }//постельное бельё
        [Required]
        public bool IsTowel { get; set; }//полотенца        
        [Required]
        public int MaxGuests { get; set; } //максимальное количество гостей
        [Required]
        public bool IsAvailablePets { get; set; } //разрешено с эивотными
        [Required]
        public bool IsAvailableChildren { get; set; } //разрешено с детьми
        [Required]
        public bool IsAvailableSmoking { get; set; } //разрешено курить
        [Required]
        public bool IsAvailableParties { get; set; } //разрешены вечеринки
        [Required]
        public bool IsAccountingDocs { get; set; } //отчётные документы
        [Required]
        public string Description { get; set; }//описание
    }
}
