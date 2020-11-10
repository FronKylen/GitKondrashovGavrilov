using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lab1.Entities;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            string email;
            string date;
            string login;
            string password;
            string phone;
            string site;
            CheckFields checkFields = new CheckFields();
            Console.Write("Введите E-Mail: ");
            email = Console.ReadLine();
            if (checkFields.IsEmailValid(email) == true) Console.WriteLine("E-Mail правильный"); 
            else Console.WriteLine("E-Mail неправильный");
            Console.Write("Введите дату: ");
            date = Console.ReadLine();
            if (checkFields.IsDateValid(date) == true) Console.WriteLine("Дата правильная");
            else Console.WriteLine("Дата неправильная");
            Console.Write("Введите логин: ");
            login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            password = Console.ReadLine();
            if (checkFields.IsPasswordValid(password) == true) Console.WriteLine("Пароль правильный");
            else Console.WriteLine("Пароль неправильный");
            if (checkFields.IsUserExists(login, password)) Console.WriteLine("Пользователь существует");
            else Console.WriteLine("Пользователь не существует");
            Console.Write("Введите телефон: ");
            phone = Console.ReadLine();
            if (checkFields.IsPhoneValid(phone) == true) Console.WriteLine("Номер телефона правильный");
            else Console.WriteLine("Номер телефона не правильный");
            Console.Write("Введите сайт: ");
            site = Console.ReadLine();
            if (checkFields.IsWebPageAvailable(site) == true) Console.WriteLine("Сайт доступен");
            else Console.WriteLine("Сайт не доступен");
            Console.ReadKey();
        }
    }

    public class CheckFields : Validator
    {
        public override string HashPassword(string password)
        {
            var Md5 = MD5.Create();
            var hash = Md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        public override bool IsDatabaseAccessible(string connectionString)
        {
            throw new NotImplementedException();
        }

        //Правильность даты
        public override bool IsDateValid(string date)
        {
            DateTime ValidDate;
            if (DateTime.TryParse(date, out ValidDate) == true)
            {
                return true;
            }
            else return false;
        }

        public override bool IsEmailValid(string email)
        {
            if (new EmailAddressAttribute().IsValid(email) == true)
            {
                return true;
            }
            else return false;
        }

        public override bool IsPasswordValid(string password)
        {
            bool IsLength = false;
            bool IsSymbol = false;
            bool IsWhite = false;
            if (password.Length > 5 && password.Length < 11)
            {
                IsLength = true;
            }
            foreach (var item in password)
            {
                
                if (Char.IsSymbol(item))
                {
                    IsSymbol = true;
                }
                if (Char.IsWhiteSpace(item))
                {
                    IsWhite = true;
                }
            }
            if (IsSymbol == false && IsWhite == false && IsLength == true)
            {
                return true;
            }
            else return false;
        }

        public override bool IsPhoneValid(string phone)
        {
            if (phone.Length == 11)
            {
                return true;
            }
            else return false;
        }

        public override bool IsUserExists(string login, string password)
        {
            var user = AppData.DB.User.Where(c => login == c.Login && password == c.Password).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            else return false;
        }

        public override bool IsUserRoot()
        {
            throw new NotImplementedException();
        }

        public override bool IsWebPageAvailable(string url)
        {
            Uri uri = new Uri(url);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }


        public override void Log()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Validator
    {
        public abstract bool IsPasswordValid(string password);
        public abstract string HashPassword(string password);
        public abstract bool IsUserExists(string login, string password);
        public abstract bool IsEmailValid(string email);
        public abstract bool IsPhoneValid(string phone);
        public abstract bool IsWebPageAvailable(string url);
        public abstract bool IsDatabaseAccessible(string connectionString);
        public abstract bool IsDateValid(string date);
        public abstract bool IsUserRoot();
        public abstract void Log();
    }
}
