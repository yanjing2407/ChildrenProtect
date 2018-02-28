/*
 * 由SharpDevelop创建。
 * 用户： HLW
 * 日期: 2018/2/24
 * 时间: 9:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChildrenProtect
{
	/// <summary>
	/// Description of License.
	/// </summary>
	public partial class License : Form
	{
		public License()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public event MainForm.DCloseWindow EventCloseWindow;
		 
		void Button1Click(object sender, EventArgs e)
		{
			if (EventCloseWindow != null)
            {
                if (this.textBox1.Text.Trim() == "000000")
                {
                    EventCloseWindow("yes");
                    this.Close();
                }
            }
		}
		void KeyDownFun(object sender, KeyEventArgs e)
		{
			 //在输入完成密码后按下enter键进入系统
			 if (e.KeyCode == Keys.Enter)//如果输入的是回车键
			 {
			    this.Button1Click(sender, e);//触发button事件
			 }
		}
		
	}
}
