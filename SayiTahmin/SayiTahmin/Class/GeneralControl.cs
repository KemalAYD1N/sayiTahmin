using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SayiTahmin.Class
{
    static class GeneralControl
    {
        //kullanıcının girdiği sayı istenen kuralları sağlıyor mu kontrol edilir.
        public static bool userGuessControl(string guess)
        {
            if (guess[0] == '0')
                return false;

            for (int i = 0; i < 3; i++)
            { 
                for (int j = i+1; j < 4; j++)
                {
                    if (guess[i] == guess[j])
                        return false;
                }
            }

            return true;
        }
    }
}
