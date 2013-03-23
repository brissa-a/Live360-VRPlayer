using System;
using System.Windows.Forms;

namespace XmpSample
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Sample.Run();
			MessageBox.Show("Sample completed.\r\nApplication closing...", "XmpSample", MessageBoxButtons.OK, MessageBoxIcon.Information);
			Close();
		}
	}
}