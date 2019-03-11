using EmailServicePV.Services;
using System;

namespace EmailServicePV
{
    class Program
    {
        public static EmailServiceProvider _emailProvider;

        static void Main(string[] args)
        {
                _emailProvider = new EmailServiceProvider();
                _emailProvider.SendNewsletterToSubscribers();

            Console.ReadLine();
        }
    }
}
