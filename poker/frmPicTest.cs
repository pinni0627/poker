using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace poker
{
    public partial class frmPicTest : Form
    {
        public frmPicTest()
        {
            InitializeComponent();
        }

        #region 自定義方法
        // 用字串讀取圖片
        private Image GetImage(string name)
        {
            return Properties.Resources.ResourceManager.GetObject(name) as Image;
        }
        // 用撲克牌編號讀取圖片
        private Image GetImage(int num)
        {
            return GetImage($"pic{num}");
        }

        #endregion
        private void btnTest_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int r = random.Next(1, 53);//(a,b)-> a >= 1, b < 53

            picTest.Image = GetImage(r);
            lblNum.Text = r.ToString();

        }
    }
}