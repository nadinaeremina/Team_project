using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class Property_mdl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string NumderOfHome { get; set; }
        public bool IsBanyaOrSauna { get; set; }
        public int QuantityOfFloorsInBuilding { get; set; }
        public int QuantityOfRooms { get; set; }
        public double SquareOfProperty { get; set; }
        public double SquareOfLand { get; set; }
        public bool IsParking { get; set; }
        public bool IsWaterpool { get; set; }
        public int Floor { get; set; }
        public bool IsBalconyOrLoggia { get; set; }
        public int QuantityOfBed { get; set; }
        public string TypeOfBed { get; set; }
        public double Cost { get; set; }
        public int LandlordID { get; set; }
        public int TypeOfPropertyID { get; set; }
        public double Deposit { get; set; }
        public bool IsConditioner { get; set; }
        public bool IsTV { get; set; }
        public bool IsWiFi { get; set; }
        public bool IsBedLinen { get; set; }
        public bool IsTowel { get; set; }
        public int MaxGuests { get; set; }
        public bool IsAvailablePets { get; set; }
        public bool IsAvailableChildren { get; set; }
        public bool IsAvailableSmoking { get; set; }
        public bool IsAvailableParties { get; set; }
        public bool IsAccountingDocs { get; set; }
        public string Description { get; set; }

        public Property_mdl(int id, string name, string city, string street, string numderOfHome, bool isBanyaOrSauna, int quantityOfFloorsInBuilding, int quantityOfRooms, double squareOfProperty, double squareOfLand, bool isParking, bool isWaterpool, int floor, bool isBalconyOrLoggia, int quantityOfBed, string typeOfBed, double cost, int landlordID, int typeOfPropertyID, double deposit, bool isConditioner, bool isTV, bool isWiFi, bool isBedLinen, bool isTowel, int maxGuests, bool isAvailablePets, bool isAvailableChildren, bool isAvailableSmoking, bool isAvailableParties, bool isAccountingDocs, string description)
        {
            Id = id;
            Name = name;
            City = city;
            Street = street;
            NumderOfHome = numderOfHome;
            IsBanyaOrSauna = isBanyaOrSauna;
            QuantityOfFloorsInBuilding = quantityOfFloorsInBuilding;
            QuantityOfRooms = quantityOfRooms;
            SquareOfProperty = squareOfProperty;
            SquareOfLand = squareOfLand;
            IsParking = isParking;
            IsWaterpool = isWaterpool;
            Floor = floor;
            IsBalconyOrLoggia = isBalconyOrLoggia;
            QuantityOfBed = quantityOfBed;
            TypeOfBed = typeOfBed;
            Cost = cost;
            LandlordID = landlordID;
            TypeOfPropertyID = typeOfPropertyID;
            Deposit = deposit;
            IsConditioner = isConditioner;
            IsTV = isTV;
            IsWiFi = isWiFi;
            IsBedLinen = isBedLinen;
            IsTowel = isTowel;
            MaxGuests = maxGuests;
            IsAvailablePets = isAvailablePets;
            IsAvailableChildren = isAvailableChildren;
            IsAvailableSmoking = isAvailableSmoking;
            IsAvailableParties = isAvailableParties;
            IsAccountingDocs = isAccountingDocs;
            Description = description;
        }
        public override string ToString()
        {
            return $"{Id,-5} {Name,-30} {Description,-150}";
        }
    }
}
