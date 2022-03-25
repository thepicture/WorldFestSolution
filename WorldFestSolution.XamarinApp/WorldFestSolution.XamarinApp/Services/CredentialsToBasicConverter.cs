using System;
using System.Text;

namespace WorldFestSolution.XamarinApp.Services
{
    public class CredentialsToBasicConverter
    {
        public static string Encode(string login, string password)
        {
            string loginAndPassword = string.Format("{0}:{1}",
                                           login,
                                           password);
            string encodedPhoneNumberAndPassword = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(loginAndPassword));
            return encodedPhoneNumberAndPassword;
        }
    }
}
