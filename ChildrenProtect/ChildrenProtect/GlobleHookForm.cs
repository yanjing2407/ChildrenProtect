/*
 * 由SharpDevelop创建。
 * 用户： HLW
 * 日期: 2018/2/26
 * 时间: 8:55
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ChildrenProtect
{
	/// <summary>
	/// Description of GlobleHookForm.
	/// </summary>
	public partial class GlobleHookForm : Form
	{
		public GlobleHookForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
	
	
	public class UserActivityHook
	{
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

		// Fields
		private static int hKeyboardHook;
		private HookProc KeyboardHookProcedure;
		public const int WH_KEYBOARD_LL = 13;

		// Methods
		static UserActivityHook()
		{
			hKeyboardHook = 0;
		}

		private HashSet<int> code = new HashSet<int>();

		public UserActivityHook()
		{

		}
		
		public void AddKeyHook(int keyCode)
		{		
			this.code.Add(keyCode);
		}
		
		public void Run()
		{
			this.Start();
		}

		private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
		{
			// disable once SuggestUseVarKeywordEvident
			KeyboardHookStruct struct2 = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
			// disable once SuggestUseVarKeywordEvident
			KeyEventArgs args = new KeyEventArgs((Keys)struct2.vkCode);
			Console.WriteLine(args.KeyValue.ToString());
			
			if(this.code.Contains(args.KeyValue))
			{
				return 1;
			}
			
//			if(args.KeyValue == 91 ||
//			   args.KeyValue == 92 ||
//			   args.KeyValue == 9  ||
//			   args.KeyValue == 27)//prevent which key code,  $$$$$$$$
//			{
//				return 1;
//			}
			return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
		}

		public void Start()
		{
			if (hKeyboardHook == 0)
			{
				this.KeyboardHookProcedure = new HookProc(this.KeyboardHookProc);
				Module m = Assembly.GetExecutingAssembly().GetModules()[0];
				IntPtr p = Marshal.GetHINSTANCE(m);
				hKeyboardHook = SetWindowsHookEx(13, this.KeyboardHookProcedure, p, 0);
				if (hKeyboardHook == 0)
				{
					this.Stop();
					throw new Exception("SetWindowsHookEx ist failed.");
				}
			}
		}

		public void Stop()
		{
			bool flag = true;
			if (hKeyboardHook != 0)
			{
				flag = UnhookWindowsHookEx(hKeyboardHook);
				hKeyboardHook = 0;
			}
			if (!flag)
			{
				throw new Exception("UnhookWindowsHookEx failed.");
			}
		}


		~UserActivityHook()
		{
			this.Stop();
		}


		[StructLayout(LayoutKind.Sequential)]
		public class KeyboardHookStruct
		{
			public int vkCode;
			public int scanCode;
			public int flags;
			public int time;
			public int dwExtraInfo;

			public KeyboardHookStruct()
			{
			}
		}
	}

}
