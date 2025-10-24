using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace _7HW_FindExpertsTG;

class Program
{
    static async Task Main(string[] args)
    {
        var bot = new TelegramBotClient("");

        var portfolios = new ConcurrentDictionary<long, Portfolio>(); // список доступных портфолио
        var creationStates = new ConcurrentDictionary<long, CreateStep>(); // шаг создания портфолио
        var tempPortfolios = new ConcurrentDictionary<long, Portfolio>(); // список портфолио на этапе создания
        var states =  new ConcurrentDictionary<long, State>(); // состояния взаимодействия пользователей с ботом
        var userPhones = new ConcurrentDictionary<long, string>(); // храним номера телефонов
        var searchFilters = new ConcurrentDictionary<long, SearchFilter>(); // храним фильтры каждого пользователя
        
        bot.StartReceiving(async (botClient, update, token) =>
        {
            if (update.Type != UpdateType.Message)
                return;

            var msg = update.Message;
            var userId = msg!.From!.Id;
            
            // Запоминаем номер телефона при подтверждении пользователем
            if (msg.Contact != null)
            {
                userPhones[userId] = msg.Contact.PhoneNumber;
                await botClient.SendMessage(msg.Chat.Id, "Номер успешно сохранен!", cancellationToken: token);
                await botClient.SendMessage(msg.Chat.Id, "Привет, это сервис для Экспертов и их поиска!", cancellationToken: token);
                await SendHelp(botClient, msg, token);
                return;
            }
            
            if (update.Message?.Text is null)
                return;
            
            var text = msg.Text!.Trim();

            // Помещаем подтвержденный номер телефона в переменную phone, иначе запрашиваем его
            if (!userPhones.TryGetValue(userId, out var phone))
            {
                await botClient.SendMessage(
                    chatId: msg.Chat.Id, 
                    text: "Для использования сервиса подтвердите ваш номер телефона.",
                    replyMarkup: new ReplyKeyboardMarkup(
                        new[]
                        {
                            KeyboardButton.WithRequestContact("Подтвердить номер телефона"),
                        })
                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true
                    }, cancellationToken: token);
                return;
            }
            
            // Создание портфолио
            if (text.StartsWith("/create") || text.StartsWith("Cоздать портфолио"))
            {
                states[userId] = State.Create;
                tempPortfolios[userId] = new Portfolio { UserID = userId };
                creationStates[userId] = CreateStep.AskName;
                await botClient.SendMessage(msg.Chat.Id, "Как вас зовут?", cancellationToken: token);
                return;
            }

            // Поиск портфолио или исполнителя
            if (text.StartsWith("/search") || text.StartsWith("Найти эксперта"))
            {
                states[userId] = State.Search;
                await botClient.SendMessage(msg.Chat.Id, "Введите название услуги или имя исполнителя: ", cancellationToken: token);
                return;
            }
            
            // Настройка фильтра поиска исполнителей
            if (text.StartsWith("/filter") || text.StartsWith("Добавить фильтр поиска"))
            {
                states[userId] = State.Filter;
                await botClient.SendMessage(msg.Chat.Id, "Введите максимальную цену за час (60 минут) услуги: ", cancellationToken: token);
                return;
            }
            
            // Процесс поиска исполнителя
            if (states[userId] == State.Create && creationStates.TryGetValue(userId, out var step))
            {
                var p = tempPortfolios[userId];
                
                p.PhoneNumber = phone;
                
                // Этапы поиска исполнителя
                switch (step)
                {
                    case CreateStep.AskName:
                        p.Name = text;
                        creationStates[userId] = CreateStep.AskService;
                        await botClient.SendMessage(msg.Chat.Id, "Напишите название предлагаемой услуги:", cancellationToken: token);
                        return;
                    case CreateStep.AskService:
                        p.Service = text;
                        creationStates[userId] = CreateStep.AskPrice;
                        await botClient.SendMessage(msg.Chat.Id, "Укажите цену за услугу (в рублях, за 60 минут).", cancellationToken: token);
                        return;
                    case CreateStep.AskPrice:
                        if (!decimal.TryParse(text, out var price))
                        {
                            await botClient.SendMessage(msg.Chat.Id, "Цена должна быть указана числом. Попробуйте снова!", cancellationToken: token);
                            return;
                        }
                        
                        p.Price = price;
                        creationStates[userId] = CreateStep.AskDescription;
                        await botClient.SendMessage(msg.Chat.Id, "Придумайте описание:", cancellationToken: token);
                        return;
                    case CreateStep.AskDescription:
                        p.Description = text;
                        portfolios[userId] = p;
                        creationStates.TryRemove(userId, out _);
                        tempPortfolios.TryRemove(userId, out _);
                        await botClient.SendMessage(msg.Chat.Id,
                                $"Портфолио создано!\n\n" +
                                $"Имя исполнителя: {p.Name}\n" +
                                $"Номер телефона: {p.PhoneNumber}\n" +
                                $"Услуга: {p.Service}\n" +
                                $"Цена (за 60 минут): {p.Price}руб.\n" +
                                $"Описание: {p.Description}", cancellationToken: token);
                        states[userId] = State.None;
                        return;
                }
            }

            // Настройка фильтра
            if (states[userId] == State.Filter)
            {
                if (!decimal.TryParse(text, out var price))
                {
                    await botClient.SendMessage(msg.Chat.Id, "Ошибка. Цена должна быть указана числом. Попробуйте снова!", cancellationToken: token);
                    return;
                }

                searchFilters[userId] = new SearchFilter
                {
                    MaxPrice = price
                };
                
                await botClient.SendMessage(msg.Chat.Id, "Фильтр применен!", cancellationToken: token);
                states[userId] = State.None;
                return;
            }
            
            // В итоговой выборке будут сначала портфолио с совпадением в названии услуги, затем в имени исполнителя, затем в описании услуги
            if (states[userId] == State.Search)
            {
                var query = text.ToLower();
                var isUserHasFilter = searchFilters.TryGetValue(userId, out var filter);
                var maxPrice = isUserHasFilter ? filter!.MaxPrice : decimal.MaxValue;
                var selectByService = portfolios.Values
                    .Where(p => p.Service.ToLower().Contains(query) && p.Price <= maxPrice);
                var selectByName = portfolios.Values
                    .Where(p => p.Name.ToLower().Contains(query) && p.Price <= maxPrice);
                var selectByDescription = portfolios.Values
                    .Where(p => p.Description.ToLower().Contains(query) && p.Price <= maxPrice);
                var result = selectByService.Union(selectByName).Union(selectByDescription).ToList();
                    
                if (result.Count == 0)
                {
                    await botClient.SendMessage(msg.Chat.Id, "Ничего не найдено.", cancellationToken: token);
                }
                else
                {
                    var resultString = string.Join("\n\n", result.Select(p =>
                        $"Имя исполнителя: {p.Name}\nНомер телефона: {p.PhoneNumber}\nУслуга: {p.Service}\nЦена (за 60 минут): {p.Price}руб.\nОписание услуги: {p.Description}"));
                    await botClient.SendMessage(msg.Chat.Id, $"Найдено {result.Count} шт.\n\n{resultString}", cancellationToken: token);
                }
                
                states[userId] = State.None;
                return;
            }
            
            await SendHelp(botClient, msg, token);
        }, (bot, ex, token) =>
        {
            Console.WriteLine(ex);
            return Task.CompletedTask;
        });
        
        Console.WriteLine("Бот запущен.");
        await Task.Delay(-1);
    }

    private static async Task SendHelp(ITelegramBotClient botClient, Message msg, CancellationToken token)
    {
        await botClient.SendMessage(
            chatId: msg.Chat.Id,
            text: "Доступные команды: \n" +
                  "/create - создать портфолио\n" +
                  "/search - найти эксперта (исполнителя)\n" +
                  "/filter - добавить фильтр поиска",
            replyMarkup: new ReplyKeyboardMarkup(new KeyboardButton("Cоздать портфолио"), new KeyboardButton("Найти эксперта (исполнителя)"), new KeyboardButton("Добавить фильтр поиска"))
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            }, cancellationToken: token);
    }
}