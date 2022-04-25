using System;
using System.Text;
using WorldFestSolution.XamarinApp.Models;

namespace WorldFestSolution.XamarinApp.Services
{
    public class CredentialsService : ILoginPasswordEncoder
    {
        public string Encode(string login, string password)
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
