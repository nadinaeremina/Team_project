using Отдохни.Ru.ModelsForDataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.DataLayer
{
    public class DL
    {
        public static string connectionstring { get; set; } = ConfigurationManager.ConnectionStrings["OtdohnyBD_add"].ConnectionString;

        public static class Admin
        {
            public static void Insert(Admin_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string admin_command = "dbo.stp_Admins_add";
                    SqlCommand cmd = new SqlCommand(admin_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[7].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.LastName;
                    cmd.Parameters[2].Value = tmp.FirstName;
                    cmd.Parameters[3].Value = tmp.Surname;
                    cmd.Parameters[4].Value = tmp.Email;
                    cmd.Parameters[5].Value = tmp.PhoneNumber;
                    cmd.Parameters[6].Value = tmp.UserID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Booking
        {
            public static void Insert(Booking_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string booking_command = "dbo.stp_Bookings_add";
                    SqlCommand cmd = new SqlCommand(booking_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[5].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.DateOfEntry;
                    cmd.Parameters[2].Value = tmp.DateOfDeparture;
                    cmd.Parameters[3].Value = tmp.LandlordID;
                    cmd.Parameters[4].Value = tmp.PropertyID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Feedback
        {
            public static void Insert(Feedback_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string feedback_command = "dbo.stp_Feedbacks_add";
                    SqlCommand cmd = new SqlCommand(feedback_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[5].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.Title;
                    cmd.Parameters[2].Value = tmp.QuanytityOfStars;
                    cmd.Parameters[3].Value = tmp.LandlordID;
                    cmd.Parameters[4].Value = tmp.PropertyID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Landlord
        {
            public static void Insert(Landlord_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string landlord_command = "dbo.stp_Landlords_add";
                    SqlCommand cmd = new SqlCommand(landlord_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[9].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.LastName;
                    cmd.Parameters[2].Value = tmp.FirstName;
                    cmd.Parameters[3].Value = tmp.Surname;
                    cmd.Parameters[4].Value = tmp.Email;
                    cmd.Parameters[5].Value = tmp.PhoneNumber;
                    cmd.Parameters[6].Value = tmp.QuantityOfProperty;
                    cmd.Parameters[7].Value = tmp.PhotoUserID;
                    cmd.Parameters[8].Value = tmp.UserID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Photo
        {
            public static void Insert(Photo_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string photo_command = "dbo.stp_Photos_add";
                    SqlCommand cmd = new SqlCommand(photo_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[4].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.Title;
                    cmd.Parameters[2].Value = tmp.Description;
                    cmd.Parameters[3].Value = tmp.PropertyID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class PhotoUser
        {
            public static void Insert(PhotoUser_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string photousers_command = "dbo.stp_PhotosUsers_add";
                    SqlCommand cmd = new SqlCommand(photousers_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[4].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.Title;
                    cmd.Parameters[2].Value = tmp.Description;
                    cmd.Parameters[3].Value = tmp.UserID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Property
        {
            public static void Insert(Property_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string property_command = "stp_Properties_add";
                    SqlCommand cmd = new SqlCommand(property_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[31].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.City;
                    cmd.Parameters[2].Value = tmp.Street;
                    cmd.Parameters[3].Value = tmp.NumderOfHome;
                    cmd.Parameters[4].Value = tmp.IsBanyaOrSauna;
                    cmd.Parameters[5].Value = tmp.QuantityOfFloorsInBuilding;
                    cmd.Parameters[6].Value = tmp.QuantityOfRooms;
                    cmd.Parameters[7].Value = tmp.SquareOfProperty;
                    cmd.Parameters[8].Value = tmp.SquareOfLand;
                    cmd.Parameters[9].Value = tmp.IsParking;
                    cmd.Parameters[10].Value = tmp.IsWaterpool;
                    cmd.Parameters[11].Value = tmp.Floor;
                    cmd.Parameters[12].Value = tmp.IsBalconyOrLoggia;
                    cmd.Parameters[13].Value = tmp.QuantityOfBed;
                    cmd.Parameters[14].Value = tmp.TypeOfBed;
                    cmd.Parameters[15].Value = tmp.Cost;
                    cmd.Parameters[16].Value = tmp.Deposit;
                    cmd.Parameters[17].Value = tmp.IsConditioner;
                    cmd.Parameters[18].Value = tmp.IsTV;
                    cmd.Parameters[19].Value = tmp.IsWiFi;
                    cmd.Parameters[20].Value = tmp.IsBedLinen;
                    cmd.Parameters[21].Value = tmp.IsTowel;
                    cmd.Parameters[22].Value = tmp.MaxGuests;
                    cmd.Parameters[23].Value = tmp.IsAvailablePets;
                    cmd.Parameters[24].Value = tmp.IsAvailableChildren;
                    cmd.Parameters[25].Value = tmp.IsAvailableSmoking;
                    cmd.Parameters[26].Value = tmp.IsAvailableParties;
                    cmd.Parameters[27].Value = tmp.IsAccountingDocs;
                    cmd.Parameters[28].Value = tmp.Description;
                    cmd.Parameters[29].Value = tmp.LandlordID;
                    cmd.Parameters[30].Value = tmp.TypeOfPropertyID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Tenant
        {
            public static void Insert(Tenant_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string tenant_command = "stp_Tenants_add";
                    SqlCommand cmd = new SqlCommand(tenant_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[9].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.LastName;
                    cmd.Parameters[2].Value = tmp.FirstName;
                    cmd.Parameters[3].Value = tmp.Surname;
                    cmd.Parameters[4].Value = tmp.Email;
                    cmd.Parameters[5].Value = tmp.PhoneNumber;
                    cmd.Parameters[6].Value = tmp.DayOfBirth;
                    cmd.Parameters[7].Value = tmp.PhotoUserID;
                    cmd.Parameters[8].Value = tmp.UserID;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class TypeOfPropery
        {
            public static void Insert(TypeOfProperty_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string typeofproperty_command = "stp_TypesOfProperties_add";
                    SqlCommand cmd = new SqlCommand(typeofproperty_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[2].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.Name;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Users
        {
            public static void Insert(User_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string user_command = "stp_Users_add";
                    SqlCommand cmd = new SqlCommand(user_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[3].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.Login;
                    cmd.Parameters[2].Value = tmp.Password;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static class Video
        {
            public static void Insert(Video_mdl tmp)
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string video_command = "dbo.stp_Videos_add";
                    SqlCommand cmd = new SqlCommand(video_command, conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);

                    cmd.Parameters[4].Value = DBNull.Value;
                    cmd.Parameters[1].Value = tmp.Title;
                    cmd.Parameters[2].Value = tmp.Description;
                    cmd.Parameters[3].Value = tmp.PropertyID;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
