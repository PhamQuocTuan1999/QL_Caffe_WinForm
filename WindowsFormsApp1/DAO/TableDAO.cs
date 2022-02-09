using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QuanLyQuanCafe.DTO;
namespace QuanLyQuanCafe.DAO
{
   public class TableDAO
    {

        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return instance; }
            private set { TableDAO.instance = value; }
        }
        public static int TableWidth = 110;
        public static int Tableheight = 110;
        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }


            return tableList;

        }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });

        }
        public DataTable GetListTable()
        {
            return DataProvider.Instance.ExecuteQuery("select id, name , status from [dbo].TableFood");
        }
        public bool InsertTable(string name, string status)
        {
            string sql = string.Format("SELECT * FROM dbo.TableFood WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(sql);
            int result = 0;
            if (data.Rows.Count == 0)
            {
                
                string query = string.Format("insert dbo.TableFood(name) values(N'{0}')", name);
                result = DataProvider.Instance.ExecuteNonQuery(query);
            }
            return result > 0;
            

           
        }

        public bool UpdateTable(string name, int id)
        {
            string query = string.Format("update dbo.TableFood set name = N'{0}' where id ={1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteTable(int id)
        {
            //DANH MUC COS CHUA THUC AN => KO XOA 
            //Neu danh muc trong => xoa
            string sql = string.Format("SELECT id FROM Bill where idTable = {0}", id);
            DataTable kq = DataProvider.Instance.ExecuteQuery(sql);
            int result = 0;
            if (kq.Rows.Count == 0)
            {
                string query = string.Format("delete TableFood where id = {0}", id);
                result = DataProvider.Instance.ExecuteNonQuery(query);
            }
            return result > 0;






        
            
        }



    }
}
