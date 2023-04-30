using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleTGBot
{
    public class Laptop
    {

        public string brand;
        public string model_name;
        public string category;
        public double screen_size;
        public string screen;
        public string cpu;
        public string ram;
        public string storage;
        public string video_card;
        public string operating_system;
        public string? operating_system_version;
        public double weight;
        public double price;
        //public string link;
        
        

        public Laptop(string brand, string model_name, string category, double screen_size, string screen, string cpu, string ram, string storage, string video_card, string operating_system, string? operating_system_version, double weight, double price )
        {
            this.brand = brand;
            this.model_name = model_name;
            this.category = category;
            this.screen_size = screen_size;
            this.screen = screen;
            this.cpu = cpu;
            this.ram = ram;
            this.storage = storage;
            
            this.video_card = video_card;
            this.operating_system = operating_system;
            this.operating_system_version = operating_system_version;
            this.weight = weight;
            this.price = Math.Round(price*0.009);
            //this.link = 
        }
        public override string ToString()
        {
            return $"Название: {brand} {model_name}" +
                $"\nКатегория: {category}" +
                $"\nДиагональ экрана: {screen_size} дюймов" +
                $"\nРазрешение экрана: {screen} пикселей" +
                $"\nПроцессор: {cpu}" +
                $"\nОперативная память: {ram} ГБ" +
                $"\nРазмер и тип памяти: {storage}" +
                $"\nВидеокарта: {video_card}" +
                $"\nОперационная система: {operating_system} {operating_system_version}" +
                $"\nВес: {weight} КГ" +
                $"\nЦена: {price} рублей";
        }
       









    }
}
