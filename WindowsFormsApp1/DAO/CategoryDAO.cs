using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "select * from FoodCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }

            return list;

        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;
            string query = "select * from FoodCategory where id = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;
        }
        public DataTable GetListCategoryFood()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT name FROM dbo.FoodCategory");
        }
        public bool InsertCategory(string name)
        {
            string sql = string.Format("SELECT * FROM dbo.FoodCategory WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(sql);
            int result = 0;
            if (data.Rows.Count == 0)
            {

                string query = string.Format("insert dbo.FoodCategory(name) values(N'{0}')", name);
                result = DataProvider.Instance.ExecuteNonQuery(query);
            }
            return result > 0;

          

            
        }

        public bool UpdateCategory(string name, int id)
        {
            string query = string.Format("update dbo.FoodCategory set name = N'{0}' where id ={1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteCategory(int id)
        {
            //DANH MUC COS CHUA THUC AN => KO XOA 
            //Neu danh muc trong => xoa
            string sql = string.Format("SELECT id FROM Food where idCategory = {0}", id);
            DataTable kq = DataProvider.Instance.ExecuteQuery(sql);
            int result = 0;
            if(kq.Rows.Count == 0)
            {
                string query = string.Format("delete FoodCategory where id = {0}", id);
                result = DataProvider.Instance.ExecuteNonQuery(query);
            }
            return result > 0;
        }




    }
}
