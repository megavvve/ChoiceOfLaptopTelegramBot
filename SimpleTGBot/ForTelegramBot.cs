using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace SimpleTGBot
{
    internal class ForTelegramBot
    {
        public static ReplyKeyboardMarkup keyboardMarkup(string action)
        {
            switch (action.ToLower())
            {
                case "/start":
                    return  new ReplyKeyboardMarkup(new[]
                    {
new[]
{
new KeyboardButton("Инфо"),
new KeyboardButton("Топ 5 ноутбуков"),
new KeyboardButton("Поиск ноутбука по характеристикам"),

},
new[]
{

new KeyboardButton("ЛУЧШИЙ НОУТБУК ДЛЯ ПРОГРАММИРОВАНИЯ!!!!"),
}

});
                case "назад":
                    return new ReplyKeyboardMarkup(new[]
                    {
new[]
{
new KeyboardButton("Инфо"),
new KeyboardButton("Топ 5 ноутбуков"),
new KeyboardButton("Поиск ноутбука по характеристикам"),

},
new[]
{

new KeyboardButton("ЛУЧШИЙ НОУТБУК ДЛЯ ПРОГРАММИРОВАНИЯ!!!!"),
}

});
                case "топ 5 ноутбуков":
                    return new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Названию"),
                            new KeyboardButton("Категории"),
                            new KeyboardButton("Диагонали"),
                        },
                        new[]
                        {
                            new KeyboardButton("Оперативной памяти"),
                            new KeyboardButton("Весу"),
                            new KeyboardButton("Цене"),
                        },
                        new[]
                        {
                            new KeyboardButton("Назад"),

                        }
                    });
                case "инфо":
                    return new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {

                            new KeyboardButton("Назад")
                        }
                    });
                case "поиск ноутбука по характеристикам":
                    return  new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("По категории"),
                            new KeyboardButton("По размеру оперативной памяти"),
                            new KeyboardButton("По размеру экрана"),
                            


                        },
                        new[]
                        {
                             
                             new KeyboardButton("По весу"),
                             new KeyboardButton("По цене"),
                             new KeyboardButton("По бренду"),
                        },
                        new[]
                        {
                             new KeyboardButton("Назад"),
                        }
                    });
                default:
                    return new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {

                            new KeyboardButton("Назад")
                        }
                    });
                   
            }
        }
    }
}
