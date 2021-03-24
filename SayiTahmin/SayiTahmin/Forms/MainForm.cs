using SayiTahmin.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SayiTahmin.Forms
{
    public partial class MainForm : Form
    {
        User user;
        Computer computer;
        int numberOfDigit = 4;//basamak sayısı
        int[] userHit = new int[2];
        bool start = true;

        public MainForm()
        {            
            InitializeComponent();
        }

        private void txtUserGuess_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                txtUserGuess.MaxLength = 4; //textbox karakter sayısı sınırlandırılır.
            }
        }

        private void txtUserGuess_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtUserGuess.Text, "  ^ [0-9]"))
            {
                txtUserGuess.Text = "";
            }
        }

        //kullanıcı sayı tahmini yapar
        private void btnUserGuess_Click(object sender, EventArgs e)
        {
            if (txtUserGuess.Text.Length == 4 && GeneralControl.userGuessControl(txtUserGuess.Text))
            {
                user.UserTotalGuess++;//kullanıcının yaptığı toplam tahmin sayısı arttırılır
                lblUserTotalGuess.Text = user.UserTotalGuess.ToString();//kullanıcının yaptığı toplam tahmin sayısı ekranda gösterilir

                int[] hint = computer.controlUserGuess(txtUserGuess.Text);//kullanıcının girdiği sayı kontrol edilir ve ipuçları hesaplanır
                
                string[] view = { "", "" };
                view[0] = txtUserGuess.Text;

                txtUserGuess.Clear();

                lblUserPHint.Text = "+" + hint[0].ToString();
                lblUserNHint.Text = "-" + hint[1].ToString();

                view[1] = lblUserPHint.Text + " , " + lblUserNHint.Text;                

                listViewUserPastGuess.Items.Add(new ListViewItem(view));

                if (hint[0] == 4)
                {
                    lblTebrikler.Text = "TEBRİKLER";
                    lblTebrikler.Visible = true;
                    panel4.BackColor = Color.Green;
                }
            }
            else
                MessageBox.Show("Lütfen 0 İle Başlamayan Rakamları Farklı 4 Basamaklı Pozitif Bir Sayı Giriniz!");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            listViewUserPastGuess.View = View.Details;
            listViewUserPastGuess.FullRowSelect = true;
            listViewUserPastGuess.Columns.Add("Tahmin");
            listViewUserPastGuess.Columns.Add("İpucu");

            listViewComputerPastGuess.View = View.Details;
            listViewComputerPastGuess.FullRowSelect = true;
            listViewComputerPastGuess.Columns.Add("Tahmin");
            listViewComputerPastGuess.Columns.Add("İpucu");

            panel1.Visible = false;
            lblTebrikler.Visible = false;
            label5.Visible = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (start)
            {
                start = false;
                user = new User();
                computer = new Computer();

                btnStart.Visible = false;
                MessageBox.Show("Oyun başlamak üzeri. Lütfen 4 basamaklı, rakamları farklı ve pozitif bir tam sayı belirleyiniz.");

                panel1.Visible = true;

                lblComputerGuess.Text = computer.computerIsGuessing();//bilgisayar kullanıcının tuttuğu sayıyı ilk başta random tahmin eder.*/
                lblComputerTotalGuess.Text = computer.ComputerTotalGuess.ToString();//bilgisayarın tahmin sayısı
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            userHit[0] = Int32.Parse(lblComputerPHint.Text);
            if (userHit[0] < 4)
                userHit[0]++;
            
            lblComputerPHint.Text = userHit[0].ToString();        
        }

        private void label12_Click(object sender, EventArgs e)
        {
            userHit[0] = Int32.Parse(lblComputerPHint.Text);
            if (userHit[0] > 0)
                userHit[0]--;
            lblComputerPHint.Text = userHit[0].ToString();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            userHit[1] = Int32.Parse(lblComputerNHint.Text);
            if (userHit[1] < 0)
                userHit[1]++;
            lblComputerNHint.Text = userHit[1].ToString();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            userHit[1] = Int32.Parse(lblComputerNHint.Text);
            if (userHit[1] > -4)
                userHit[1]--;
            lblComputerNHint.Text = userHit[1].ToString();
        }

        private void btnComputerHint_Click(object sender, EventArgs e)
        {
            //kullanıcının bilgisayara verdiği ipuçları tutulur
            computer.positiveHits.Add(userHit[0]);
            computer.negativeHits.Add(userHit[1]);           

            string[] view = { "", "" };//bilgisayarın yaptığı tahmin ve kullanıcının verdiği ipuçları listViewe bu dizi ile eklenecektir
            view[0] = computer.computerGuess[computer.computerGuess.Count-1];//bilgisayarın yaptığı son tahmin

            lblComputerGuess.Text = "";

            view[1] = lblComputerPHint.Text + " , " + lblComputerNHint.Text;

            listViewComputerPastGuess.Items.Add(new ListViewItem(view));//bilgisayarın yaptığı son tahmin ve kullanıcının verdiği ipucu listViewe eklenir.

            //lblComputerGuess.Text = computer.computerIsGuessing(numberOfDigit);//bilgisayar kullanıcının tuttuğu sayıyı ilk başta random tahmin eder.

            lblComputerTotalGuess.Text = computer.ComputerTotalGuess.ToString();//bilgisayarın yaptığı toplam tahmin sayısı ekrana basılır

            lblComputerGuess.Text = computer.computerIsGuessing();

            if (userHit[0] == 4)
            {
                label5.Text = "TEBRİKLER";
                label5.Visible = true;
                panel5.BackColor = Color.Green;
            }

            /*label5.Text = "";
            foreach (var item in computer.enİyiTahmin)
            {
                label5.Text += item.ToString();
            }*/
        }
    }
}
