using System.Reflection.Metadata.Ecma335;

namespace SimpleTGBot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Text.RegularExpressions;

public class TelegramBot
{
    Stack<string> stack = new Stack<string>();
    private const string BotToken = "5974502960:AAG3QpvQS8n2fBC8ejyH5nwI_KTFfSXyXGo";
    public async Task Run()
    {

        var botClient = new TelegramBotClient(BotToken);// запускаю сервер

        using CancellationTokenSource cts = new CancellationTokenSource();
        ReceiverOptions receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = new[] { UpdateType.Message,
            UpdateType.CallbackQuery}
        };
        botClient.StartReceiving(
            updateHandler: OnMessageReceived,//бот обравбатывевт обновления
            pollingErrorHandler: OnErrorOccured,// если ошибка
            receiverOptions: receiverOptions, //настройки
            cancellationToken: cts.Token//токен
        );


        var me = await botClient.GetMeAsync(cancellationToken: cts.Token);
        Console.WriteLine($"Бот @{me.Username} запущен.\nДля остановки нажмите клавишу Esc...");

        // Ждём, пока будет нажата клавиша Esc, тогда завершаем работу бота
        while (Console.ReadKey().Key != ConsoleKey.Escape) { }

        // Отправляем запрос для остановки работы клиента.
        cts.Cancel();
    }




    async Task OnMessageReceived(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Работаем только с сообщениями. Остальные события игнорируем
        var message = update.Message;
        if (message is null)
        {
            return;
        }

        if (message.Text is not { } messageText)
        {
            return;
        }

        var chatId = message.Chat.Id;
        List<Laptop> laptopList = LaptopList.GetLaptopList(@"./input-files/laptops_train.csv");
        Console.WriteLine($"Получено сообщение в чате {chatId}: '{messageText}'");




        if (message.Text == "/start")
        {
            stack = new Stack<string>();


            await botClient.SendTextMessageAsync(chatId, "Выберите пункт меню:",
            replyMarkup: ForTelegramBot.keyboardMarkup(message.Text),
            cancellationToken: cancellationToken);
            stack.Push(message.Text.ToLower());
        }

        else if (stack.Count > 0)
        {
            if (message.Text.ToLower() == "назад")
            {

                await botClient.SendTextMessageAsync(chatId, "Выберите пункт меню:",
            replyMarkup: ForTelegramBot.keyboardMarkup(message.Text),
            cancellationToken: cancellationToken);
                stack.Push(message.Text.ToLower());
            }
            else if (stack.Peek() == "назад" || stack.Peek() == "/start")
            {
                /////////////////////////// 1 уровень логики
                if (message.Text.ToLower() == "топ 5 ноутбуков")
                {
                    await botClient.SendTextMessageAsync(chatId, "Выберете по какому параметру вывести топ ноутбуков",
           replyMarkup: ForTelegramBot.keyboardMarkup(message.Text),
           cancellationToken: cancellationToken);
                    stack.Push(message.Text.ToLower());
                }
                else if (message.Text.ToLower() == "инфо")
                {
                    await botClient.SendTextMessageAsync(chatId, "Привет. Я удобный бот для подбора ноутбуков по твоим потребностям.\nЯ могу подобрать тебе ноутбук именно по твоему вкусу ",
           replyMarkup: ForTelegramBot.keyboardMarkup(message.Text),
           cancellationToken: cancellationToken);
                    stack.Push(message.Text.ToLower());
                }
                else if (message.Text.ToLower() == "поиск ноутбука по характеристикам")
                {

                    await botClient.SendTextMessageAsync(chatId, "Выберите пункт для поиска ноутбука:",
           replyMarkup: ForTelegramBot.keyboardMarkup(message.Text),
           cancellationToken: cancellationToken);
                    stack.Push(message.Text.ToLower());
                }
                else if (message.Text == "ЛУЧШИЙ НОУТБУК ДЛЯ ПРОГРАММИРОВАНИЯ!!!!")
                {
                    

                    await botClient.SendPhotoAsync(
    chatId: chatId,
    photo: "https://shifter.pt/wp-content/uploads/2018/03/rickrolled.png",
    caption: "Ладно, ладано, по ссылке уж точно будет лучший ноутбук для программирования",
    parseMode: ParseMode.Html,
    replyMarkup: new InlineKeyboardMarkup(
        InlineKeyboardButton.WithUrl(
            text: "Тык сюда",
            url: "https://www.youtube.com/watch?v=dQw4w9WgXcQ")),
    cancellationToken: cancellationToken);
                }
                
            }
            


            /////////////////////////// 2 уровень логики
            if ((message.Text == "Названию" || message.Text == "Категории" || message.Text.ToLower() == "диагонали" || message.Text.ToLower() == "оперативной памяти" || message.Text.ToLower() == "цене" || message.Text.ToLower() == "весу") && stack.Contains("топ 5 ноутбуков"))
            {

                //stack.Push("топ 5 ноутбуков");
                foreach (var item in LaptopList.Top5SortList(laptopList, message.Text))
                {
                    await botClient.SendTextMessageAsync(chatId, item.ToString(),
       replyMarkup: ForTelegramBot.keyboardMarkup("Топ 5 ноутбуков"),
       cancellationToken: cancellationToken);
                    

                }
                stack.Push(message.Text.ToLower());
            }
            else if ( stack.Contains("поиск ноутбука по характеристикам") && (message.Text == "По бренду" || message.Text == "По категории" || message.Text == "По размеру экрана" || message.Text == "По размеру оперативной памяти"  || message.Text == "По весу" || message.Text == "По цене"))
            {
                var filteredLaptopList = LaptopList.HashSetWithCategories(laptopList, message.Text);



                if (message.Text == "По бренду" || message.Text == "По категории" )
               {

                    foreach (var item in filteredLaptopList)
                    {
                        await botClient.SendTextMessageAsync(chatId, item,
                             replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),
                             cancellationToken: cancellationToken);

                    }
                    await botClient.SendTextMessageAsync(chatId, "Выберите что нибудь из предложенных вариантов выше:",
                        replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),
                        cancellationToken: cancellationToken);
                    stack.Push(message.Text.ToLower());
                }
                else
                {
                    var min = filteredLaptopList.Select(x => double.Parse(x)).Min();
                    var max = filteredLaptopList.Select(x => double.Parse(x)).Max();
                    await botClient.SendTextMessageAsync(chatId, $"Минимальное значение: {min}",
                        replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),

                        cancellationToken: cancellationToken);
                    
                    await botClient.SendTextMessageAsync(chatId, $"Максимальное значение: {max}",
                        replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),
                        cancellationToken: cancellationToken);
                    
                    await botClient.SendTextMessageAsync(chatId, $"Выберите диопазон от {min} до {max}\nОтвет должен быть таким:<минимальное значение>-<максимальное значение>. Например: 1,4-2,6",
                        replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),
                        cancellationToken: cancellationToken);
                    stack.Push(message.Text.ToLower());
                }

                

                
            }

            /////////////////////////// 3 уровень логики

         
            if (stack.Contains("поиск ноутбука по характеристикам") && (stack.Peek() == "по бренду" || stack.Peek() == "по категории" || stack.Peek() == "по размеру экрана" || stack.Peek() == "по размеру оперативной памяти" || stack.Peek() == "по весу" || stack.Peek() == "по цене"))
            {
               
                if (stack.Peek() == "по бренду" || stack.Peek() == "по категории")
                {
                   
                        
                        if (!laptopList.Exists(x => x.brand == message.Text || x.category == message.Text)) return;

                    List<Laptop> filteredLaptopList = laptopList.Where(x => x.brand == message.Text || x.category == message.Text).Take(10).ToList();
                        foreach (var item in filteredLaptopList)
                    {
                        await botClient.SendTextMessageAsync(chatId, item.ToString(),
                             replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),
                             cancellationToken: cancellationToken);

                    }
                    
                   
                }
                else
                {
                    
                    if (!(message.Text == "По бренду" || message.Text == "По категории" || message.Text == "По размеру экрана" || message.Text == "По размеру оперативной памяти" || message.Text == "По весу" || message.Text == "По цене"))
                    {
                        
                        List<Laptop> list = LaptopList.ListForSort(laptopList, stack.Peek(), message.Text);
                        if (list.Count > 0) {
                            foreach (var item in list)
                            {
                                await botClient.SendTextMessageAsync(chatId, item.ToString(),
                                replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),

                                cancellationToken: cancellationToken);
                            }
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId, "К сожалению по выбранным вами параметрам не удалось ничего найти(",
                               replyMarkup: ForTelegramBot.keyboardMarkup("поиск ноутбука по характеристикам"),

                               cancellationToken: cancellationToken);
                        }
                        
                        

                        
                        stack.Push(message.Text.ToLower());
                    }
                    
                    
                    


                }
            }




        }


    }




    Task OnErrorOccured(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // В зависимости от типа исключения печатаем различные сообщения об ошибке
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        // Завершаем работу
        return Task.CompletedTask;
    }
}