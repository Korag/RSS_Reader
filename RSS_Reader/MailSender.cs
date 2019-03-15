using EmailServicePV.Services;

namespace RSS_Reader
{
    public static class MailSender
    {
        public static void SendCombinationStringToEmail(string combinationString, string emailAddress)
        {
            EmailServiceProvider ep = new EmailServiceProvider(true);
            ep.SendEmailWithCombinationString(combinationString, emailAddress);
        }
    }
}