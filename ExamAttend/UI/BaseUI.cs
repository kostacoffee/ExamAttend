using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ExamAttend
{
	public abstract partial class BaseUI : UserControl
	{
		protected TableLayoutPanel layout;
        public ComboBox searchBox;
        private const int STUDENT_IMAGE_WIDTH = 110;
        delegate void update<T>(List<T> dataSource); 

		protected void addControl(Control c, int col, int row, AnchorStyles anchorStyle, int columnSpan = 1, string text = "")
		{
			c.Text = text;
			layout.Controls.Add(c, col, row);
			layout.SetColumnSpan(c, columnSpan);
			c.Anchor = anchorStyle;
			c.Width = this.Width;
			c.Height = this.Height;
		}

		protected void addControl(Label l, int col, int row, AnchorStyles anchorStyle, int columnSpan = 1, string text = "") 
		{
			addControl((Control)l, col, row, anchorStyle, columnSpan, text);
			l.TextAlign = ContentAlignment.MiddleCenter;
		}

		protected void addControl(Button b, int col, int row, AnchorStyles anchorStyle, int columnSpan = 1, string text = "") {
			int h = b.Height;
			addControl((Control)b, col, row, anchorStyle, columnSpan, text);
			b.Height = h;
		}

		protected void addControl (ComboBox b, int col, int row, AnchorStyles anchorStyle, int columnSpan = 1, string text = "")
		{
			addControl((Control) b, col, row, anchorStyle, columnSpan, text);
            b.DropDownStyle = ComboBoxStyle.DropDown;
            b.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			b.AutoCompleteSource = AutoCompleteSource.ListItems;
		}

        protected void addControl(PictureBox b, int col, int row, AnchorStyles anchorStyle, int columnSpan = 1, string text = "")
        {
            addControl((Control)b, col, row, anchorStyle, columnSpan, text);
            b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            b.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void updateSearchBox<T>(List<T> dataSource)
		{
            if (searchBox.InvokeRequired)
            {
                update<T> d = new update<T>(updateSearchBox<T>);
                this.Invoke(d, new object[] { dataSource });
            }
            else
            {
                searchBox.Items.Clear();
                foreach (T item in dataSource)
                    searchBox.Items.Add(item);
                searchBox.SelectedIndex = -1;
            }

		}

        protected void addControl(RichTextBox b, int col, int row, AnchorStyles anchorStyle, int columnSpan = 1, string text = "")
        {
            //http://stackoverflow.com/questions/700479/how-do-i-give-the-richtextbox-a-flat-look
            b.BorderStyle = System.Windows.Forms.BorderStyle.None;
            b.Dock = DockStyle.Fill;
            Panel p = new Panel();
            p.Controls.Add(b);
            addControl(p, col, row, anchorStyle, columnSpan, text);
            p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            layout.SetRowSpan(p, 2);
        }
	}
}
