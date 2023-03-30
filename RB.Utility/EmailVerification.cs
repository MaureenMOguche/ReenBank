using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Utility
{
    public static class EmailVerification
    {
        public static int GenerateCode()
        {
            Random random = new Random();
            int number = random.Next(10000, 999999);
            return number;
        }

        public static bool VerifyCode(int code, int verifyCode)
        {
            if (code == verifyCode)
                return true;
            else return false;
        }
    }
}
