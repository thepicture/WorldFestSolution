using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WorldFestSolution.ImportApp
{
    class Program
    {
        private readonly static string[] firstNames = new string[]
        {
            "Иван",
            "Александр",
            "Андрей",
            "Ивана",
            "Александра",
            "Андрия",
            "Мария",
            "Светлана",
            "Макс"
        };
        private readonly static string[] lastNames = new string[]
        {
            "Иванова",
            "Александрова",
            "Андреева",
            "Иванова",
            "Александрова",
            "Андреева",
            "Мариева",
            "Светланова",
            "Максова"
        };

        private static readonly Random random = new Random();
        private static readonly string[] festivalTitles = new string[]
        {
            "Понедельничный фестиваль",
            "Звездная ночь",
            "Фест лунных ночей",
            "Бой Кот",
            "Полет фантазии",
            "Вдохновенный фест",
            "Приходи еще",
            "Всю ночь"
        };

        private static readonly string[] festivalDescriptions = new string[]
        {
            "Ждём всех",
            "Оплата по мере надобности",
            "Записывайся уже сейчас",
            "Лучший фестиваль этой ночи!",
            "Отдохни у нас!",
            "Мы создатели лучших фестов!",
            "Напитки в комплекте",
            "Не описать словами"
        };

        private static readonly string[] festivalProgramTitles = new string[]
        {
            "Пьём мохито",
            "Пробуем смузи",
            "Концерт известного певца",
            "Прослушивание мюзикла",
            "Прогулка по парку",
            "Совместное рисование",
            "Катание на скейтбордах",
            "Отдых на траве"
        };

        private static readonly string[] festivalProgramDescriptions = new string[]
        {
            "Недолго, но интересно",
            "Можно с друзьями",
            "Напитки включены",
            "Зовите всех!",
        };

        private static string[] UserImages => Directory.GetFiles(
            Path.Combine(
                Path.GetFullPath("../.."), "UserImages"));
        private static string[] FestivalImages => Directory.GetFiles(
           Path.Combine(
               Path.GetFullPath("../.."), "FestivalImages"));

        static void Main(string[] args)
        {
            ImportUsers(10);
            ImportFestivals(10);
            ImportFestivalsPrograms(4, 6);
            ImportParticipants(2, 4);
            Console.ReadKey();
        }

        private static void ImportParticipants(int minCount, int maxCount)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                List<User> participants = entities.User
               .Where(u => u.UserType.Title == "Участник")
               .ToList();
                foreach (Festival festival in entities.Festival)
                {
                    for (int i = 0;
                        i < random.Next(minCount, maxCount + 1);
                        i++)
                    {
                        try
                        {
                            festival.User.Add(
                                participants.ElementAt(
                                    random.Next(0, participants.Count)));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                entities.SaveChanges();
            }
        }

        private static void ImportFestivalsPrograms(int minCount, int maxCount)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                foreach (Festival festival in entities.Festival)
                {
                    for (int i = 0;
                        i < random.Next(minCount, maxCount + 1);
                        i++)
                    {
                        festival.FestivalProgram.Add(new FestivalProgram
                        {
                            DurationInMinutes = random.Next(30, 120),
                            Title = festivalProgramTitles[random.Next(0, festivalProgramTitles.Length)],
                            Description = festivalProgramDescriptions[random.Next(0, festivalProgramDescriptions.Length)],
                        });
                    }
                }
                entities.SaveChanges();
            }
        }

        private static void ImportFestivals(int count)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                for (int i = 0; i < count; i++)
                {
                    Festival festival = new Festival
                    {
                        Title = festivalTitles[random.Next(0, festivalTitles.Length)],
                        Description = festivalDescriptions[random.Next(0, festivalDescriptions.Length)],
                        FromDateTime = DateTime.Now.AddDays(random.Next(2, 8)),
                        Image = File.ReadAllBytes(
                            UserImages[random.Next(0, UserImages.Length)])
                    };
                    List<User> organizers = entities.User
                        .Where(u => u.UserType.Title == "Организатор")
                        .ToList();
                    festival.User.Add(
                        organizers.ElementAt(
                            random.Next(0, organizers.Count)));
                    entities.Festival.Add(festival);
                }
                entities.SaveChanges();
            }
        }

        private static void ImportUsers(int count)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                for (int i = 0; i < count; i++)
                {
                    byte[] salt = Guid.NewGuid()
                        .ToByteArray();
                    string password = Guid.NewGuid()
                        .ToString()
                        .Substring(30);
                    List<byte> passwordBytesAndHash = Encoding.UTF8
                        .GetBytes(password)
                        .ToList();
                    passwordBytesAndHash.AddRange(salt);
                    byte[] passwordHash = SHA256.Create()
                        .ComputeHash(
                        passwordBytesAndHash.ToArray());
                    User user = new User
                    {
                        Login = Guid
                            .NewGuid()
                            .ToString()
                            .Substring(30),
                        PasswordHash = passwordHash,
                        Salt = salt,
                        FirstName = firstNames[random.Next(0, firstNames.Length)],
                        LastName = lastNames[random.Next(0, lastNames.Length)],
                        UserTypeId = random.Next(1, 3),
                        Image = File.ReadAllBytes(
                            UserImages[random.Next(0, UserImages.Length)])
                    };
                    Console.WriteLine(user.Login
                        + " : "
                        + password);
                    entities.User.Add(user);
                }
                entities.SaveChanges();
            }
        }
    }
}
