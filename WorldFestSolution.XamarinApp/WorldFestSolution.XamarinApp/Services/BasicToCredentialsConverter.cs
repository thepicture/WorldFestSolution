using System;
using System.Text;
using WorldFestSolution.XamarinApp.Models;

namespace WorldFestSolution.XamarinApp.Services
{
    public static class BasicToCredentialsConverter
    {
        public static string[] Decode()
        {
            string phoneNumberAndPassword = Encoding.UTF8.GetString(
                Convert.FromBase64String(Identity.AuthorizationValue));
            return phoneNumberAndPassword.Split(':');
        }
    }
}
