using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SayiTahmin.Forms;

namespace SayiTahmin.Class
{
    class Computer
    {
        List<int> mümkünRakamlar = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };//bilgisayarın tahmin edebileceği mümkün rakamlar

        public int numberDigitOfComputer = 4;//basamak sayısı
        public string numberHoldComputer;//bilgisayarın tuttuğu sayı
        public List<string> computerGuess = new List<string>();//bilgisayarın tahminleri
        public bool isFirsGuess = true;//bilgisayarın ilk tahmini mi ?
        public int ComputerTotalGuess = 0;//bilgisayarın yaptığı toplam tahmin sayısı
        bool zeroControl = false;//ilk basamak 0 mı ?
        public List<int> enSonSayilar = new List<int>();

        
        public List<int> enİyiTahmin = new List<int> {-1, -1, -1, -1 };//şu ana kadar yapılmış en iyi tahmini tutar.
        public List<int> enIyiIpicu = new List<int> { 0, 0 };// en iyi ipucu
        public int enIyiSkor = 0;
        public List<int> kesinVar = new List<int>();
        public List<int> kesinYok = new List<int>();

        public List<int> positiveHits = new List<int>();//kullanıcıdan gelen pozitif ipuçları
        public List<int> negativeHits = new List<int>();//kullanıcıdan gelen negatif ipuçları

        public Computer()
        {
            myNumber();
        }

        //bilgisayar oyun başlangıcında bir sayı tutar
        public void myNumber()
        {
            numberHoldComputer = RandomGenerate.startRandomComputerGuess(numberDigitOfComputer);
        }

        //bilgisayar kullanıcının tahmini ile kendi tahminini kontrol eder ve gerekli ipuçlarını verir
        public int[] controlUserGuess(string guess)
        {
            int[] hint = { 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                if (numberHoldComputer[i] == guess[i])
                    hint[0]++;
                else if(numberHoldComputer[i] != guess[i] && numberHoldComputer.Contains(guess[i]))
                {
                    hint[1]++;
                }
            }
            return hint;
        }
        //alınan ipucuna göre skor hesabı
        public int skor(int p, int n)
        {
            if (p == 0 && n == 0)
                return 0;
            else if (p == 0 && n == -1)
                return 1;
            else if (p == 1 && n == 0)
                return 2;
            else if (p == 0 && n == -2)
                return 3;
            else if (p == 1 && n == -1)
                return 4;
            else if (p == 2 && n == 0)
                return 5;
            else if (p == 0 && n == -3)
                return 6;
            else if (p == 1 && n == -2)
                return 7;
            else if (p == 2 && n == -1)
                return 8;
            else if (p == 3 && n == 0)
                return 9;
            else if (p == 0 && n == -4)
                return 10;
            else if (p == 1 && n == -3)
                return 11;
            else if (p == 2 && n == -2)
                return 12;
            else if (p == 4 && n == 0)
                return 13;
            else return 0;
        }

        //bilgisayar, kullanıcının tuttuğu sayıyı tahmin eder ve tahmin ettiği sayıyı verir
        public string computerIsGuessing()
        {
            ComputerTotalGuess++;//toplam tahmin sayısı bir arttırılır            

            //ilk tahmin kullanıcıdan herhangi bir ipucu almadan yapıldığı için tamamen random olarak yapılır
            if (isFirsGuess)
            {
                computerGuess.Add(RandomGenerate.startRandomComputerGuess(numberDigitOfComputer));//bilgisayarın yaptığı ilk tahmin listeye eklenir
                isFirsGuess = false;                
            }
            else
            {
                computerGuess.Add(mantikliTahminUret());//alınan ipucuna göre tahmin
            }
            
            return computerGuess[computerGuess.Count - 1];
        }
        //kullanıcıdan alınan ipucuna göre tahmin üretir
        public string mantikliTahminUret()
        {
            int skor = this.skor(positiveHits[positiveHits.Count - 1], negativeHits[negativeHits.Count - 1]);

            //skor 0 ise tahmindeki sayılar olasılıktan çıkarılır
            if (skor == 0 && zeroControl == false)
            {
                for (int j = 0; j < numberDigitOfComputer; j++)
                {
                    char y = computerGuess[computerGuess.Count - 1][j];
                    mümkünRakamlar.Remove(Int32.Parse(y.ToString()));
                }
                zeroControl = true;
            }

            if (zeroControl == true && skor == 0)
            {
                Random rr = new Random();
                string randomNumber = "";
                int temp;
                bool firsDigit = true;//ilk basamak kontrolü için.

                while (numberDigitOfComputer > 0)
                {
                    while (true)
                    {
                        temp = rr.Next(0, 10);//1-10 arasında random rakam üretilir
                        if (mümkünRakamlar.Contains(temp))
                            break;
                    }

                    if (firsDigit && temp != 0)//sayının ilk basamağı için 0 olma durumu kontrol edilir
                    {
                        randomNumber += temp;
                        firsDigit = false;
                        numberDigitOfComputer--;
                    }
                    else if (randomNumber.Contains(temp.ToString()) != true && firsDigit == false)
                    {
                        randomNumber += temp;
                        numberDigitOfComputer--;
                    }
                }
                computerGuess.Add(randomNumber);

                return randomNumber;
            }
            //en iyi tahmin belirlenir
            if (skor > enIyiSkor)
            {
                enIyiSkor = skor;
                enIyiIpicu[0] = positiveHits[positiveHits.Count - 1];
                enIyiIpicu[1] = negativeHits[negativeHits.Count - 1];

                int kk = 0;
                foreach (var item in computerGuess[computerGuess.Count - 1])
                {
                    enİyiTahmin[kk] = Int32.Parse(item.ToString());
                    kk++;
                }
            }
            //olasılıktan gerekli kesinlikler eklenir veya çıkarılır
            if (enIyiIpicu[0] + Math.Abs(enIyiIpicu[1]) == 3 && positiveHits[positiveHits.Count - 1] + Math.Abs(negativeHits[negativeHits.Count - 1]) < 3)
            {
                for (int n = 0; n < numberDigitOfComputer; n++)
                {
                    if (computerGuess[computerGuess.Count - 1].Contains(mümkünRakamlar[n].ToString()) == false)
                        kesinVar.Add(mümkünRakamlar[n]);
                }

                for (int n = 0; n < numberDigitOfComputer; n++)
                {
                    if (enİyiTahmin.Contains(Int32.Parse(computerGuess[computerGuess.Count - 1][n].ToString())) == false)
                        mümkünRakamlar.Remove(Int32.Parse(computerGuess[computerGuess.Count - 1][n].ToString()));
                }
            }

            bool kont=true;

            while (kont) { 

                string result = "";//sonuç
                int[] yeniTahmin = new int[] { -1, -1, -1, -1 };// yapılacak yeni tahmini tutar
                int[] kullanılmış = { 0, 0, 0, 0 };
                List<int> kullanılanSayi = new List<int>();

                int i = 0;
                //int randomRakamIndex;

                Random r = new Random();

                //pozitif ipucuna göre yeni yerleşim
                while (i < enIyiIpicu[0] && enIyiIpicu[0] != 0)
                {
                    int randomRakamIndex = r.Next(0, 4);//random indis üretilir
                    if (kullanılmış[randomRakamIndex] == 0)//kullanılmamış index
                    {
                        yeniTahmin[randomRakamIndex] = enİyiTahmin[randomRakamIndex];
                        kullanılmış[randomRakamIndex] = 1;
                        kullanılanSayi.Add(enİyiTahmin[randomRakamIndex]);
                        i++;
                    }
                }
                //negatif ipucuna göre yeni yerleşim
                i = 0;

                while (i < Math.Abs(enIyiIpicu[1]) && enIyiIpicu[1] != 0)
                {
                    int m = 0;
                    int sayiIndis = 0;
                    if (enIyiIpicu[1] != -3)
                    {
                        int randomRakamIndex = r.Next(0, 4);//random indis üretilir
                        int randomRakamIndex2;
                        
                        if (kullanılmış[randomRakamIndex] == 0)//kullanılmamış index
                        {
                            while (true)
                            {
                                randomRakamIndex2 = r.Next(0, 4);
                                if (randomRakamIndex != 0)
                                {                                    
                                    if (randomRakamIndex != randomRakamIndex2 && kullanılanSayi.Contains(enİyiTahmin[randomRakamIndex2]) != true)
                                    {
                                        kullanılanSayi.Add(enİyiTahmin[randomRakamIndex2]);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (enİyiTahmin[randomRakamIndex2] != 0)
                                    {
                                        if (randomRakamIndex != randomRakamIndex2 && kullanılanSayi.Contains(enİyiTahmin[randomRakamIndex2]) != true)
                                        {
                                            kullanılanSayi.Add(enİyiTahmin[randomRakamIndex2]);
                                            break;
                                        }
                                    }
                                    else
                                        randomRakamIndex = r.Next(0, 4);                                    
                                }
                            }
                            yeniTahmin[randomRakamIndex] = enİyiTahmin[randomRakamIndex2];
                            kullanılmış[randomRakamIndex] = 1;

                            i++;
                        }
                    }
                    else
                    {
                        for (int n = 0; n < 4; n++)
                        {
                            if (kullanılanSayi.Contains(enİyiTahmin[n]) == false)
                            {
                                sayiIndis = n;
                                break;
                            }
                        }
                        m = sayiIndis + 1;
                        if (m >= 4)
                            m = 0;
                        while (true)
                        {
                            if (kullanılmış[m] == 0)
                            {
                                if (m != 0)
                                {
                                    yeniTahmin[m] = enİyiTahmin[sayiIndis];
                                    kullanılmış[m] = 1;
                                    kullanılanSayi.Add(enİyiTahmin[sayiIndis]);
                                    i++;
                                    break;
                                }
                                else
                                {
                                    if (enİyiTahmin[sayiIndis] != 0)
                                    {
                                        yeniTahmin[m] = enİyiTahmin[sayiIndis];
                                        kullanılmış[m] = 1;
                                        kullanılanSayi.Add(enİyiTahmin[sayiIndis]);
                                        i++;
                                        break;
                                    }
                                    else
                                    {
                                        m++;
                                        continue;
                                    }
                                }
                            }
                            m++;
                            if (m >= 4)
                                m = 0;
                        }
                    }     
                }
                //boş kalan yerlere yerleşim

                List<int> olasılık = new List<int>();
                for (int ii = 0; ii < mümkünRakamlar.Count; ii++)
                {
                    olasılık.Add(mümkünRakamlar[ii]);
                }

                int bosYer = 0;
                if (Math.Abs(enIyiIpicu[0] + Math.Abs(enIyiIpicu[1])) < 4)
                {
                    bosYer = 4 - Math.Abs(enIyiIpicu[0] + Math.Abs(enIyiIpicu[1]));

                    if (mümkünRakamlar.Count >= bosYer+bosYer)
                    {
                        for (int iii = 0; iii < enİyiTahmin.Count; iii++)
                        {
                            if (kullanılanSayi.Contains(enİyiTahmin[iii])==false)
                            {
                                olasılık.Remove(enİyiTahmin[iii]);
                            }
                        }
                    }

                    int bos;
                    while (bosYer != 0)
                    {
                        int bosIndex = r.Next(0, 4);

                        if (kullanılmış[bosIndex] == 0 && bosIndex != 0)
                        {
                            while (true)
                            {
                                bos = r.Next(0, 10);//boşluğa yerleşecek sayi
                                if (olasılık.Contains(bos) && yeniTahmin.Contains(bos) != true)
                                    break;

                            }
                            yeniTahmin[bosIndex] = bos;
                            bosYer--;
                        }

                        else if (kullanılmış[bosIndex] == 0 && bosIndex == 0)
                        {
                            while (true)
                            {
                                bos = r.Next(1, 10);//boşluğa yerleşecek sayi
                                if (olasılık.Contains(bos) && yeniTahmin.Contains(bos) != true)
                                    break;
                            }

                            yeniTahmin[bosIndex] = bos;
                            bosYer--;
                        }
                    }
                }

                kont = false;
                string tekrar = "";
                foreach (var item in yeniTahmin)
                {
                    if (item == -1)
                        kont = true;

                    tekrar += item;
                }

                if (kont == false)
                {
                    if (computerGuess.Contains(tekrar))
                        kont = true;
                }

                if(kont==false)
                {
                    foreach (var item in yeniTahmin)
                    {
                        result += item.ToString();
                    }
                    return result;
                }               
            }
        return "";
        }
    }
}
