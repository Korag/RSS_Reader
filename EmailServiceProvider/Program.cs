using EmailServicePV.Services;
using System;
using System.Threading;

namespace EmailServicePV
{
    class Program
    {
        public static EmailServiceProvider _emailProvider;

        static void Main(string[] args)
        {
            _emailProvider = new EmailServiceProvider();

            #region Local
            //while (true)
            //{
            //    var czas = DateTime.Now;
            //    Console.WriteLine(czas);
            //    if (czas.ToString("HH:mm") == "13:05")
            //    {
            //        Console.WriteLine("Wysyłam maile o godzinie" + czas);
            //        _emailProvider.SendNewsletterToSubscribers();
            //        Thread.Sleep(70000);
            //    }
            //}
            #endregion

            #region Azure
            _emailProvider.SendNewsletterToSubscribers();
            #endregion
        }
    }
}
