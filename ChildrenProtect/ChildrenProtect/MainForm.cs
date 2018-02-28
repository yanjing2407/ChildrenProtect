/*
 * 由SharpDevelop创建。
 * 用户： ymh
 * 日期: 2018/2/23
 * 时间: 20:17
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChildrenProtect
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		int mint = 29;
        int scss = 59;
        bool first = true;
       	string close = "";
       	string startup = "";
       	
       	UserActivityHook actHook = null;

        public delegate void DCloseWindow(string pwd);
        
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			//1.将Form属性ShowInTaskbar改为false，这样程序将不会在任务栏中显示。
         	//2.将Form属性WindowState选择为 Minimized，以便起来自动最小化隐藏。
            startup = Application.ExecutablePath;       //取得程序路径   
            int pp  = startup.LastIndexOf("\\");
            startup = startup.Substring(0, pp);
			            
			this.actHook  = new UserActivityHook();
			this.actHook.AddKeyHook(91);	// 91 ==> 左win键的keyCode
			this.actHook.AddKeyHook(92);	// 92 ==> 右win键的keyCode
			this.actHook.AddKeyHook(9);		// 9  ==> TAB键
			this.actHook.AddKeyHook(27);	// 27 ==> ESC键
			this.actHook.Run();
       		
			if(File.Exists(GetFlgPath()))
            {
            	Control();
            }
			else
			{
	            label1.Text = mint + "分";
	
	            label2.Text = scss + "秒";
	
	            this.timer1.Interval = 1000; //设置间隔时间，为毫秒；
	            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);////设置每间隔3000毫秒（3秒）执行一次函数timer1_Tick
	
	            this.timer1.Start();
			}
            
		}
		
		private void timer1_Tick(object sender, EventArgs e)
        {
            if (mint >= 0)
            {
                scss--;
                if (scss == 0)
                {
                    mint--;

                    label1.Text = mint.ToString() + "分";

                    scss = 59;

                }

                label2.Text = scss.ToString() + "秒";
                
                if(mint == 5 && first)
                {
					//气球提示           
                   this.notifyIcon1.ShowBalloonTip(3, "提示", "还有5分钟", ToolTipIcon.Info);
                   first = false;
                }
            }
            
            if(mint < 0)
            {

		            Control();

            }
        }
		
		void Control()
		{
			label1.Text = "时间到了，";
			label2.Text = "休息一下吧";
			if(this.WindowState != FormWindowState.Maximized)
			{
				string flg = GetFlgPath();
				if(!File.Exists(flg))	// 不存在就创建
				{
					FileStream fs = new FileStream(flg, FileMode.OpenOrCreate);
					StreamWriter sw = new StreamWriter(fs);
					sw.Close();
				}
				
				this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
				this.WindowState = FormWindowState.Maximized;    //最大化窗体
				this.Show();
				this.Activate();
			}
		}
		
		string GetFlgPath()
		{
			string date =  DateTime.Now.ToString("yyyyMMdd");
	        string flg = startup + "\\" + date + ".flg";
	        return flg;
		}
		void MainFormSizeChanged(object sender, EventArgs e)
		{
	        if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); //或者是this.Visible = false;
                this.notifyIcon1.Visible = true;
            }
		}
		
		void exitMenuItem_Click(object sender, EventArgs e)
		{
              this.Close();
		}
		void hideMenuItem_Click(object sender, EventArgs e)
		{
			this.Hide();
		}
		void showMenuItem_Click(object sender, EventArgs e)
		{
	        this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
		}
		void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
		}
		
		void FormClosingFun(object sender, FormClosingEventArgs e)
		{
			// disable once SuggestUseVarKeywordEvident
			License doc = new License();
            doc.EventCloseWindow += new DCloseWindow(doc_EventCloseWindow);
            doc.ShowDialog();

            if (close == "yes")
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
		}
		
		void doc_EventCloseWindow(string pwd)
        {
            if (pwd == "yes")
            {
                close = "yes";
            }
        }
	}
}
