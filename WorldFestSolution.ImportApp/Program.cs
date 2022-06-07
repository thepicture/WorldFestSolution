using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WorldFestSolution.ImportApp
{
    public static class UserRoleIds
    {
        public const int ParticipantId = 1;
        public const int OrganizerId = 2;
    }

    internal class Program
    {
        private const string DefaultPlainPassword = "123";
        private static readonly int ImageWidth = 500;
        private static readonly int ImageHeight = 500;
        private static readonly int ImageQuality = 50;
        private static readonly string[] firstNames = new string[]
        {
            "Иван",
            "Александр",
            "Андрей",
            "Макс",
            "Кирилл",
            "Николай"
        };
        private static readonly string[] lastNames = new string[]
        {
            "Иванов",
            "Александров",
            "Андреев",
            "Мариев",
            "Светланов",
            "Максов",
            "Кириллов"
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
            "Катаемся на скейтбордах",
            "Отдых в парке"
        };

        private static readonly string[] festivalProgramDescriptions = new string[]
        {
            "Недолго, но интересно",
            "Можно с друзьями",
            "Напитки включены",
            "Зовите всех!",
        };

        private static readonly string[] festivalComments = new string[]
        {
            "Прикольно",
            "Ну ничего так!",
            "Вполне сойдёт",
            "Мюзикл был класс",
            "Ещё будет?",
            "Суперский фестиваль",
        };

        private static string[] UserImages;
        private static string[] FestivalImages;

        private static ImageTransformService ImageTransformService;

        [STAThread]
        private static void Main()
        {
            ImageTransformService = new ImageTransformService();
            FolderBrowserDialog userImagesFolderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Выберите папку с фотографиями пользователей"
            };
            if (userImagesFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                UserImages = Directory.GetFiles(userImagesFolderBrowserDialog.SelectedPath);
            }
            else
            {
                return;
            }
            FolderBrowserDialog festivalImagesFolderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Выберите папку с фотографиями фестивалей"
            };
            if (festivalImagesFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FestivalImages = Directory.GetFiles(festivalImagesFolderBrowserDialog.SelectedPath);
            }
            else
            {
                return;
            }
            ImportUsers(count: 16, roleId: UserRoleIds.ParticipantId);
            ImportUsers(count: 2, roleId: UserRoleIds.OrganizerId);
            ImportFestivalsForEachOrganizer(count: 2);
            ImportProgramsForEachFestival(minCount: 2, maxCount: 3);
            ImportParticipantsForEachFestival(minCount: 3, maxCount: 6);
            ImportFestivalsCommentsForEachFestival(minCount: 3, maxCount: 5);
            Console.ReadKey();
        }

        private static void ImportFestivalsCommentsForEachFestival(int minCount, int maxCount)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                foreach (Festival festival in entities.Festivals)
                {
                    for (int i = 0;
                        i < random.Next(minCount, maxCount + 1);
                        i++)
                    {
                        festival.FestivalComments.Add(new FestivalComment
                        {
                            UserId = festival.Users
                                .ToList()
                                .ElementAt(
                                    random.Next(0, festival.Users.Count())).Id,
                            Text = festivalComments[random.Next(0, festivalComments.Length)],
                            CreationDateTime = festival.FromDateTime + TimeSpan.FromHours(
                                random.Next(6, 12))
                        });
                    }
                }
                entities.SaveChanges();
            }
        }

        private static void ImportParticipantsForEachFestival(int minCount, int maxCount)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                List<User> participants = entities.Users
               .Where(u => u.UserType.Title == "Участник")
               .ToList();
                foreach (Festival festival in entities.Festivals)
                {
                    for (int i = 0;
                        i < random.Next(minCount, maxCount + 1);
                        i++)
                    {
                        try
                        {
                            festival.Users.Add(
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

        private static void ImportProgramsForEachFestival(int minCount, int maxCount)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                foreach (Festival festival in entities.Festivals)
                {
                    for (int i = 0;
                        i < random.Next(minCount, maxCount + 1);
                        i++)
                    {
                        festival.FestivalPrograms.Add(new FestivalProgram
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

        private static void ImportFestivalsForEachOrganizer(int count)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                foreach (User user in entities.Users
                    .Where(u => u.UserType.Title == "Организатор"))
                {
                    for (int i = 0; i < count; i++)
                    {
                        Festival festival = new Festival
                        {
                            Title = festivalTitles[random.Next(0, festivalTitles.Length)],
                            Description = festivalDescriptions[random.Next(0, festivalDescriptions.Length)],
                            FromDateTime = DateTime.Now.AddDays(random.Next(2, 8)),
                            Image = ImageTransformService.Transform(
                                File.ReadAllBytes(
                                    FestivalImages[random.Next(0, FestivalImages.Length)]),
                                ImageWidth,
                                ImageHeight,
                                ImageQuality)
                        };
                        festival.Users.Add(user);
                        entities.Festivals.Add(festival);
                    }
                }
                entities.SaveChanges();
            }
        }

        private static void ImportUsers(int count, int roleId)
        {
            using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
            {
                for (int i = 0; i < count; i++)
                {
                    byte[] salt = Guid.NewGuid()
                        .ToByteArray();
                    string password = DefaultPlainPassword;
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
                        UserTypeId = roleId,
                        Image = ImageTransformService.Transform(
                            File.ReadAllBytes(
                                UserImages[random.Next(0, UserImages.Length)]),
                            ImageWidth,
                            ImageHeight,
                            ImageQuality)
                    };
                    Console.WriteLine(user.Login
                        + " : "
                        + password
                        + ", роль: "
                        + (user.UserTypeId == 1 ? "участник" : "организатор"));
                    entities.Users.Add(user);
                }
                entities.SaveChanges();
            }
        }
    }
}
