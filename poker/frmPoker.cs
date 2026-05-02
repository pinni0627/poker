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
    public partial class frmPoker : Form
    {
        #region 欄位
        // 賠率設定
        int totalMoney = 1000000; // 初始總資金
        int currentBet = 0;       // 當前押注金額
        PictureBox[] pic = new PictureBox[5];
        int[] allPoker = new int[52];
        int[] playerPoker = new int[5];

        #endregion
        public frmPoker()
        {
            InitializeComponent();
            InitializePoker();
            txtTotalMoney.Text = totalMoney.ToString(); // 顯示 1,000,000

            txtBetMoney.Enabled = true;                 // 確保押注框可以輸入
            button1.Enabled = true;                      // 啟用押注按鈕
            btnDealCard.Enabled = false;                // 未押注前不能發牌
        }

        #region
        private void InitializePoker()
        {
            // 動態產生5張牌
            for (int i = 0; i < 5; i++)
            {
                pic[i] = new PictureBox();
                pic[i].Image = GetImage("back");
                pic[i].Name = "pic" + i;
                pic[i].SizeMode = PictureBoxSizeMode.AutoSize;
                pic[i].Top = 30;
                pic[i].Left = 10 + ((pic[i].Width + 10) * i);
                pic[i].Visible = true;
                pic[i].Enabled = false;
                pic[i].Tag = "back";
                // 將 pic 丟至到 grpPorker 內
                this.grpPoker.Controls.Add(pic[i]);
                pic[i].MouseClick += new MouseEventHandler(pic_Click);
            }
        }
        private void pic_Click(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            //MessageBox.Show("你選擇了" + pic.Name);
            // 取得 pic 的索引值
            int index = int.Parse(pic.Name.Replace("pic", ""));
            // 如果 pic 的 Tag 為 back，則將顯示撲克牌
            if (pic.Tag.ToString() == "back")
            {
                pic.Tag = "front";
                pic.Image = GetImage(playerPoker[index] + 1);
            }
            else
            {
                pic.Tag = "back";
                pic.Image = GetImage("back");
            }

        }
        private Image GetImage(string name)
        {
            return Properties.Resources.ResourceManager.GetObject(name) as Image;
        }

        private Image GetImage(int num)
        {
            return GetImage($"pic{num}");
        }
        #endregion

        #region 洗牌
        private void Shuffle()
        {
            Random rand = new Random();
            for (int i = 0; i < allPoker.Length; i++)
            {
                int r = rand.Next(allPoker.Length);
                int temp = allPoker[r];
                allPoker[r] = allPoker[0];
                allPoker[0] = temp;
            }
        }
        #endregion
        #region
        private void ShowCards()
        {
            for (int i = 0; i < playerPoker.Length; i++)
            {
                pic[i].Image = this.GetImage($"pic{playerPoker[i] + 1}");
            }
        }
        #endregion
        private async void btnDealCard_Click(object sender, EventArgs e)
        {
            // 先將牌面蓋掉
            for (int i = 0; i < 5; i++)
            {
                pic[i].Image = GetImage("back");
            }
            // 初始化52張牌
            for (int i = 0; i < 52; i++)
            {
                allPoker[i] = i;
            }
            // 洗牌
            Shuffle();
            
            // 暫停500ms
            await Task.Delay(500);

            // 發牌
            for (int i = 0; i < 5; i++)
            {
                pic[i].Image = GetImage("pic" + (allPoker[i] + 1));
                playerPoker[i] = allPoker[i];
            }

            this.ShowCards();

            // 啟用所有牌的點擊事件
            for (int i = 0; i < 5; i++)
            {
                pic[i].Enabled = true;
                pic[i].Tag = "front";
            }
            btnChangeCard.Enabled = true;
            
        }

        private void btnChangeCard_Click(object sender, EventArgs e)
        {
            int cardIndex = 5;
            for (int i = 0; i < pic.Length; i++)
            {
                if (pic[i].Tag.ToString() == "back")
                {
                    playerPoker[i] = allPoker[cardIndex];
                    pic[i].Image = GetImage(playerPoker[i] + 1);
                    pic[i].Tag = "front";
                    cardIndex++;
                }
            }
            // 禁用所有牌的點擊事件
            for (int i = 0; i < pic.Length; i++)
            {
                pic[i].Enabled = false;
            }
            btnChangeCard.Enabled = false;
            btnCheck.Enabled = true;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string[] colorList = { "梅花", "方塊", "愛心", "黑桃" };
            string[] pointList = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q","K" };
            // 計錄目前五張撲克牌的花色和點數的陣列
            int[] pokerColor = new int[5];
            int[] pokerPoint = new int[5];
            // 將每張牌的顏色和點數分別存入 pokerColor 和 pokerPoint 陣列
            for (int i = 0; i < 5; i++)
            {
                pokerColor[i] = playerPoker[i] % 4;
                pokerPoint[i] = playerPoker[i] / 4;
            }
            // 記錄花色和點數出現次數的陣列
            int[] colorCount = new int[4];
            int[] pointCount = new int[13];
            // 統計 color 和 point 出現次數
            for (int i = 0; i < 5; i++)
            {
                int color = pokerColor[i];
                int point = pokerPoint[i];
                colorCount[color]++;
                pointCount[point]++;
            }
            // 排序 colorCount 和 pointCount 由大到小
            Array.Sort(colorCount, colorList);
            Array.Reverse(colorCount);
            Array.Reverse(colorList);
            Array.Sort(pointCount, pointList);
            Array.Reverse(pointCount);
            Array.Reverse(pointList);
            // 判斷是否為同花
            bool isFlush = (colorCount[0] == 5);
            // 判斷是否為五張單張
            bool isSingle = (pointCount[0] == 1 && pointCount[1] == 1 && pointCount[2] == 1 &&
            pointCount[3] == 1 && pointCount[4] == 1);
            // 判斷是否為差四
            bool isDiffFout = (pokerPoint.Max() - pokerPoint.Min() == 4);
            // 判斷是否為大順
            bool isRoyal = pokerPoint.Contains(0) && pokerPoint.Contains(9) &&
            pokerPoint.Contains(10) && pokerPoint.Contains(11) && pokerPoint.Contains(12);
            // 判斷是否為同花大順
            bool isRoyalisFlush = isFlush && isRoyal;
            // 判斷是否為同花順
            bool isStraightFlush = isFlush && isSingle && isDiffFout;
            // 判斷是否為順子
            bool isStraight = isSingle && (isDiffFout || isRoyal);
            // 判斷是否為鐵支
            bool isFourOfAKind = (pointCount[0] == 4);
            // 判斷是否為葫蘆
            bool isFullHouse = (pointCount[0] == 3 && pointCount[1] == 2);
            // 判斷是否為三條
            bool isThreeOfAKind = (pointCount[0] == 3 && pointCount[1] == 1);
            // 判斷是否為兩對
            bool isTwoPair = (pointCount[0] == 2 && pointCount[1] == 2);
            // 判斷是否為一對
            bool isOnePair = (pointCount[0] == 2 && pointCount[1] == 1);
            string result = "";
            if (isRoyalisFlush)
            {
                result = $"{colorList[0]} 同花大順";
            }
            else if (isStraightFlush)
            {
                result = $"{colorList[0]} 同花順";
            }
            else if (isStraight)
            {
                result = "順子";
            }
            else if (isFourOfAKind)
            {
                result = $"{pointList[0]} 鐵支";
            }
            else if (isFullHouse)
            {
                result = $"{pointList[0]}三張{pointList[1]}兩張 葫蘆";
            }
            else if (isFlush)
            {
                result = $"{colorList[0]} 同花";
            }
            else if (isThreeOfAKind)
            {
                result = $"{pointList[0]} 三條";
            }
            else if (isTwoPair)
            {
                result = $"{pointList[0]},{pointList[1]} 兩對";
            }
            else if (isOnePair)
            {
                result = $"{pointList[0]} 一對";
            }
            else
            {
                result = "雜牌";
            }
            lblResult.Text = result;
            btnChangeCard.Enabled = false;
            btnCheck.Enabled = false;
            btnDealCard.Enabled = true;
            // ... 前面判斷牌型的 bool 邏輯保持不變 ...

            int multiplier = 0; // 賠率

            if (isRoyalisFlush) { result = $"{colorList[0]} 同花大順"; multiplier = 250; }
            else if (isStraightFlush) { result = $"{colorList[0]} 同花順"; multiplier = 50; }
            else if (isFourOfAKind) { result = $"{pointList[0]} 鐵支"; multiplier = 25; }
            else if (isFullHouse) { result = $"{pointList[0]}三張{pointList[1]}兩張 葫蘆"; multiplier = 9; }
            else if (isFlush) { result = $"{colorList[0]} 同花"; multiplier = 6; }
            else if (isStraight) { result = "順子"; multiplier = 4; }
            else if (isThreeOfAKind) { result = $"{pointList[0]} 三條"; multiplier = 3; }
            else if (isTwoPair) { result = $"{pointList[0]},{pointList[1]} 兩對"; multiplier = 2; }
            else if (isOnePair) { result = $"{pointList[0]} 一對"; multiplier = 1; }
            else { result = "雜牌"; multiplier = 0; }

            // --- 賠率計算部分 ---
            int winMoney = currentBet * multiplier;
            totalMoney += winMoney; // 將贏得的獎金加回總資金

            lblResult.Text = $"{result} (賠率: {multiplier}x) ";
            txtTotalMoney.Text = totalMoney.ToString();

            // 重設按鈕狀態以便下一局
            btnCheck.Enabled = false;
            button1.Enabled = true;       // 重新啟用押注
            txtBetMoney.Enabled = true;  // 重新啟用輸入框
            btnDealCard.Enabled = false; // 必須先押注才能再發牌
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.lblResult.Text = "";
            if (int.TryParse(txtBetMoney.Text, out currentBet) && currentBet > 0 && currentBet <= totalMoney)
            {
                totalMoney -= currentBet;
                txtTotalMoney.Text = totalMoney.ToString();

                // 押注成功後，鎖定輸入框，防止發牌後又改金額
                txtBetMoney.Enabled = false;
                button1.Enabled = false;
                btnDealCard.Enabled = true;
            }
            else
            {
                MessageBox.Show("金額不足或輸入錯誤！");
            }

        }
    }
}
