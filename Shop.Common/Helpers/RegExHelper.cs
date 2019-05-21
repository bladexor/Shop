using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Helpers
{
    using System;
    using System.Net.Mail;

    public static class RegexHelper
    {
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                var mail = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

}
