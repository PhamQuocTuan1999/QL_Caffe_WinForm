using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Forms;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using WindowsFormsApp1;
namespace WindowsFormsApp1
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwichtTable);



        }

        #region method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            aToolStripMenuItem.Text += "[" + LoginAccount.DisplayName + "]";
        }



        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }


        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }



        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button bnt = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.Tableheight };
                bnt.Text = item.Name + Environment.NewLine ;

                bnt.Click += bnt_Click;
                bnt.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        {
                            bnt.BackColor = Color.Aqua;
                            bnt.ForeColor = Color.Blue;
                            
                            bnt.Font = new Font(bnt.Font, FontStyle.Bold);
                            bnt.TextAlign = ContentAlignment.TopCenter;
                            Image myimage1 = new Bitmap(@"E:\BaiWindo\QuanLyQuanCaffe\WindowsFormsApp1\Resources\table.png");
                            bnt.BackgroundImage = myimage1;
                            bnt.BackgroundImageLayout = ImageLayout.Stretch;

                            break;
                        }
                    default:
                        {
                            bnt.BackColor = Color.PaleVioletRed;
                            bnt.ForeColor = Color.White;
                            bnt.TextAlign = ContentAlignment.TopCenter;
                            bnt.Font = new Font(bnt.Font, FontStyle.Bold);
                            Image myimage1 = new Bitmap(@"E:\BaiWindo\QuanLyQuanCaffe\WindowsFormsApp1\Resources\tableoff.png");
                            bnt.BackgroundImage = myimage1;
                            bnt.BackgroundImage = myimage1;
                            bnt.BackgroundImageLayout = ImageLayout.Stretch;
                            break;

                        }

                }

                flpTable.Controls.Add(bnt);
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo so = new CultureInfo("vi_VN");
            txbTTPrice.Text = totalPrice.ToString("c", so);
            
        }
        void bnt_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }


        #endregion

        #region event


        private void thôngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbSwichtTable.SelectedItem as Table).ID;

            if (MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwichtTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
                
            }
            ShowBill(0);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAcount += f_UpdateAccount;
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.UpdateFood += f_UpdateFood;
            f.DeleteFood += f_DeleteFood;
            f.InsertCategory += f_InsertCategory;
            f.UpdateCategory += f_UpdateCategory;
            f.DeleteCategory += f_DeleteCategory;
            f.InsertTable += f_InsertTable;
            f.UpdateTable += f_UpdateTable;
            f.DeleteTable += f_DeleteTable;
            f.ShowDialog();
            this.Show();
        }

        #region eventadmin
        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            
        }

        private void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        void f_UpdateAccount(object sender, AccountEvent e)
        {
            aToolStripMenuItem.Text = "[" +  e.Acc.DisplayName + "]";
        }

        // Category
        private void f_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_UpdateCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        //Table
        private void f_DeleteTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void f_UpdateTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void f_InsertTable(object sender, EventArgs e)
        {
            LoadTable();
        }


        #endregion













        private void fTableManager_Load(object sender, EventArgs e)
        {

        }
        #endregion

        private void txbTTPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);

        }

        private void bntaddfood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckoutBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nncfood.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }
            ShowBill(table.ID);
            LoadTable();




        }

        private void bntCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int discount = (int)nndiscount.Value;
            double totalPrice = 0;
             totalPrice = Convert.ToDouble(txbTTPrice.Text.Split(',')[0])*1000;
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
               
            }
            int idBill = BillDAO.Instance.GetUncheckoutBillIDByTableID(table.ID);
            if(idBill == -1)
            {
                MessageBox.Show("Không có hóa đơn để thanh toán");
            }
            if (idBill != -1 && totalPrice > 0)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount,(float) finalTotalPrice);

                    ShowBill(table.ID);
                    LoadTable();

                }
            }
            else
            {
                MessageBox.Show("Không có hóa đơn để thanh toán");
            }
            
        }

        private void cbSwichtTable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
