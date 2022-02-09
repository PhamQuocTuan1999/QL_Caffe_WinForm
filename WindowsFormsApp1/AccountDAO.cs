using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyQuanCafe.DAO;
using System.Security.Cryptography;
using System.Data;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string userName, string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }
            ;

            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName,hasPass/*list*/});

            return result.Rows.Count > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT UserName, DisplayName, Type FROM dbo.Account");
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from account where userName = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }
        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(pass);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            byte[] temp1 = ASCIIEncoding.ASCII.GetBytes(newPass);
            byte[] hasData1 = new MD5CryptoServiceProvider().ComputeHash(temp1);

            string hasPass1 = "";

            foreach (byte item in hasData1)
            {
                hasPass1 += item;
            }
            int data = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAcount @userName , @displayName , @password , @newPassword ", new object[] { userName, displayName, hasPass, hasPass1 });
            return data > 0;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {

            string sql = string.Format("SELECT * FROM dbo.Account WHERE dbo.fuConvertToUnsign1(UserName) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(sql);
            int result = 0;
            if (data.Rows.Count == 0)
            {

                string query = string.Format("insert dbo.Account(UserName, displayName, Type,passWord) values(N'{0}',N'{1}',{2}, N'{3}')", name, displayName, type, "1962026656160185351301320480154111117132155");
                 result = DataProvider.Instance.ExecuteNonQuery(query);
            }
            return result > 0;

            

            
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("update dbo.Account set displayName = N'{1}', Type={2} where UserName =N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteAccount(string name)
        {

            string query = string.Format("delete Account where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPassword(string name)
        {
            string query = string.Format("update Account set password = N'1962026656160185351301320480154111117132155' where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
