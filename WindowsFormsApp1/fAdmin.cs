using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        public Account loginAccount;

        public fAdmin()
        {
            InitializeComponent();
            load();

        }
      void load()
        {
            dtgvFood.DataSource = foodList;
            dtgvacc.DataSource = accountList;
            dtgvCatagory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;

            AddCategoryBinding();
            LoadListCatarory();

            AddAccountBinding();
            LoadAccount();

            LoadListTable();
            AddTableBinding();

            LoadListFood();
            LoadDateTimePickerBill();
            LoadListBillByDate(dtTo.Value, dtFrom.Value);
            AddFoodBinding();
            LoadCategoryIntoCombobox(cbTimCatagory);
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";

        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtdvBill.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
        

        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtTo.Value = new DateTime(today.Year, today.Month, 1);
            dtFrom.Value = dtTo.Value.AddMonths(1).AddDays(-1);
        }
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"Name", true, DataSourceUpdateMode.Never));
            txbFoodId.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nnGiaFood.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        #region event
        private void tbTable_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'quanLyQuanCafeDataSet.Account' table. You can move, or remove it, as needed.
            this.accountTableAdapter.Fill(this.quanLyQuanCafeDataSet.Account);
            // TODO: This line of code loads data into the 'quanLyQuanCafeDataSet.Account' table. You can move, or remove it, as needed.
            this.accountTableAdapter.Fill(this.quanLyQuanCafeDataSet.Account);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodId.Text);
           
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa thức ăn thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi Xóa thức ăn");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tbCatagoryfood_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgcAcc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.accountTableAdapter.Fill(this.quanLyQuanCafeDataSet.Account);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.accountTableAdapter.FillBy(this.quanLyQuanCafeDataSet.Account);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtdvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        #endregion
        private void bntThongKe_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtTo.Value, dtFrom.Value);
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();

        }

        private void bntViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void cbTimCatagory_TextChanged(object sender, EventArgs e)
        {
            
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbTimCatagory.SelectedItem = category;

                    int index = -1;
                    int i = 0;

                    foreach (Category item in cbTimCatagory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbTimCatagory.SelectedIndex = index;
                }
           

        }

        private void txbFoodId_TextChanged(object sender, EventArgs e)
        {
            
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbTimCatagory.SelectedItem = category;

                    int index = -1;
                    int i = 0;

                    foreach (Category item in cbTimCatagory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbTimCatagory.SelectedIndex = index;
                }
           
        }

        private void txbFoodId_TextChanged_1(object sender, EventArgs e)
        {
            try { 
            if (dtgvFood.SelectedCells.Count > 0)
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                Category category = CategoryDAO.Instance.GetCategoryByID(id);
                cbTimCatagory.SelectedItem = category;

                int index = -1;
                int i = 0;

                foreach (Category item in cbTimCatagory.Items)
                {
                    if (item.ID == category.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
                cbTimCatagory.SelectedIndex = index;
            }
            }
            catch { }
        }
            



        private void bntAddFood_Click(object sender, EventArgs e)
        {
            String name = txbFoodName.Text;
            int categoryID = (cbTimCatagory.SelectedItem as Category).ID;
            float price = (float)nnGiaFood.Value;
            
            
            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void bntEditFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodId.Text);
            String name = txbFoodName.Text;
            int categoryID = (cbTimCatagory.SelectedItem as Category).ID;
            float price = (float)nnGiaFood.Value;
            if (FoodDAO.Instance.UpdateFood(id,name, categoryID, price))
            {
                MessageBox.Show("Cập nhật thức ăn thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi Cập nhật thức ăn");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }

        //-------Catarory
        void LoadListCatarory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();

        }
        void LoadCategoryInfoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void AddCategoryBinding()
        {
            tbNameCatagory.DataBindings.Add(new Binding("Text", dtgvCatagory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            tbIDCatarory.DataBindings.Add(new Binding("Text", dtgvCatagory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            
        }

        private void btnaddCatarory_Click(object sender, EventArgs e)
        {
            String name = tbNameCatagory.Text;
                   
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm Loại Thức ăn Thành Công");
                LoadListCatarory();
                if (insertCategory != null)
                    insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi Thêm Loại thức ăn");
            }
        }

        private void btnViewCatarory_Click(object sender, EventArgs e)
        {
            LoadListCatarory();
        }

        private void btnEditCatarory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tbIDCatarory.Text);
            String name = tbNameCatagory.Text;
          
            if (CategoryDAO.Instance.UpdateCategory( name,id))
            {
                MessageBox.Show("Cập Nhật Loại Thức Ăn Thành Công");
                LoadListCatarory();
                if (updateCategory != null)
                    updateCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có Lỗi Khi Cập Nhật Loại Thức Ăn");
            }
        }

        private void btnDeleteCatarory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tbIDCatarory.Text);
            //MessageBox.Show(CategoryDAO.Instance.DeleteCategory(id).ToString());
            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công");
                if (deleteCategory != null)
                    deleteCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Danh mục chứa thức ăn không thể xóa");
            }
            LoadListCatarory();
        }

        private void XoaDanhMuc(int id)
        {
             
        }


        //table


        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler insertAccount;
        public event EventHandler InsertAccount
        {
            add { insertAccount += value; }
            remove { insertAccount -= value; }
        }
        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        private event EventHandler deleteAccount;
        public event EventHandler DeleteAccount
        {
            add { deleteAccount += value; }
            remove { deleteAccount -= value; }
        }
        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        private event EventHandler updateAccount;
        public event EventHandler UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.GetListTable();

        }
        void LoadTableInfoCombobox(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.GetListTable();
            cb.DisplayMember = "Name";
        }
        void AddTableBinding()
        {
            cbTTTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
            tnNameTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            tbIDTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));

        }

        private void bntaddTable_Click(object sender, EventArgs e)
        {
            String name = tnNameTable.Text;
            String status = cbTTTable.Text;

            if (TableDAO.Instance.InsertTable(name, status))
            {
                MessageBox.Show("Thêm bàn thành công");
                if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại");
            }
            LoadListTable();
        }

        private void bntEditTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tbIDTable.Text);
            String name = tnNameTable.Text;
         

            if (TableDAO.Instance.UpdateTable(name, id))
            {
                MessageBox.Show("Cập nhật bàn thành công");
                if (updateTable != null)
                    updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Cập nhật bàn thất bại");
            }
            LoadListTable();
        }

        private void bntDeleteTable_Click(object sender, EventArgs e)
        {

            int  id = Convert.ToInt32(tbIDTable.Text);
            //MessageBox.Show(CategoryDAO.Instance.DeleteCategory(id).ToString());
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công");
                   if (deleteTable != null)
                        deleteTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Bàn chứa hóa đơn không thể xóa");
            }
    
            LoadListTable();
        }
        //Account
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();

        }
        void LoadAccountInfoCombobox(ComboBox cb)
        {
            cb.DataSource = AccountDAO.Instance.GetListAccount();
            cb.DisplayMember = "Name";
        }
        void AddAccountBinding()
        {
            textBox3.DataBindings.Add(new Binding("Text", dtgvacc.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            textBox2.DataBindings.Add(new Binding("Text", dtgvacc.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dtgvacc.DataSource, "Type", true, DataSourceUpdateMode.Never));

        }
        private void bntViewAcc_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void bntAddAcc_Click(object sender, EventArgs e)
        {
            int type = Convert.ToInt32(numericUpDown1.Text);
            String userName = textBox3.Text;
            String displayName = textBox2.Text;
           

            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
                if (insertAccount != null)
                    insertAccount(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
            LoadAccount();
        }

        private void bntEditAcc_Click(object sender, EventArgs e)
        {
            int type = Convert.ToInt32(numericUpDown1.Text);
            String userName = textBox3.Text;
            String displayName = textBox2.Text;
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
                if (updateAccount != null)
                    updateAccount(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }
            LoadAccount();
        }

        private void bntDeleteAcc_Click(object sender, EventArgs e)
        {
           
            String userName = textBox3.Text;


            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
            LoadAccount();
        }

        private void bntResetPass_Click(object sender, EventArgs e)
        {
            String userName = textBox3.Text;
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }








        //
        private void bntTimFood_Click(object sender, EventArgs e)
        {
           foodList.DataSource= SearchFoodByName(tbTimFood.Text);
        }

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtTo.Value, dtFrom.Value);
            int lastPage = sumRecord / 10;
            if(sumRecord % 10 != 0)
            {
                lastPage ++;
            }
            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtdvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtTo.Value, dtFrom.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            if (page > 1)
            {
                page--;
            }
            txbPageBill.Text = page.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtTo.Value, dtFrom.Value);
            if (page < sumRecord)
            {
                page++;
            }
            txbPageBill.Text = page.ToString();
            //Billinfo

        }


        }
}
