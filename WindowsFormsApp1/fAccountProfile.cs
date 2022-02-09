using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; }
        }
        public fAccountProfile(Account acc)
        {
            InitializeComponent();

            this.loginAccount = acc;
            ChangeAccount(loginAccount);
        }
        void ChangeAccount(Account acc)
        {
            this.txtUsername.Text = loginAccount.UserName;
            this.txbDisPlayName.Text = loginAccount.DisplayName;

        }
        void UpdateAccountInfo()
        {
            string displayName = txbDisPlayName.Text;
            string password = textBox1.Text;
            string newPass = txbNewPass.Text;
            string reenterPass = txbRePass.Text;
            string userName = txtUsername.Text;

            if (!newPass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới ");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, password, newPass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAcount != null)
                        updateAcount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }
        private event EventHandler<AccountEvent> updateAcount;
        public event EventHandler<AccountEvent> UpdateAcount
        {
            add { updateAcount += value; }
            remove { updateAcount -= value; }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void bntupdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        private void txbDisPlayName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbRePass_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }
        public AccountEvent(Account acc)
        {
            this.Acc = acc;

        }
    }
}
