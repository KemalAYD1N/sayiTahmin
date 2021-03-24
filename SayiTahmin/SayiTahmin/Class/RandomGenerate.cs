using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SayiTahmin.Class
{
    public static class RandomGenerate
    {
        //static string computerGuess = "";
        static Random r = new Random();

        //kullanıcının tuttuğu sayı için ve kendi tutacağı sayı için ilk random tahminler yapılır.
        public static string startRandomComputerGuess(int numberOfDigits) {

            string randomNumber = "";
            int temp;
            bool firsDigit = true;//ilk basamak kontrolü için.

            while(numberOfDigits > 0){//tüm basamak değerleri atanana kadar devam eder
                temp = r.Next(0, 10);//1-10 arasında random rakam üretilir

                if (firsDigit && temp != 0)//sayının ilk basamağı için 0 olma durumu kontrol edilir
                {
                    randomNumber += temp;
                    firsDigit = false;
                    numberOfDigits--;
                }
                else if(randomNumber.Contains(temp.ToString()) != true && firsDigit == false)
                {
                    randomNumber += temp;
                    numberOfDigits--;
                }                
            }
            return randomNumber;
        }
    }
}
