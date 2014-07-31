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
	public partial class ExamAttenderUI : BaseUI
    {
		public Button lookupBtn, recordBtn, finaliseBtn;
		public PictureBox lookupPic, studentPic;
        public RichTextBox statusConsole;
        public ExtendedDataGridView subjects;
        delegate void SetTextCallback(string text);

		public ExamAttenderUI()
		{
			InitializeComponent();
			base.layout = this.layout;
            //this.layout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            insertLabels();
            insertStatusConsole();
			insertButtons();
			insertTextBox();
			insertPictureBox();
            insertSubjects();
            
		}

        public void writeStatus(string status) {
            //http://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
            if (statusConsole.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(writeStatus);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                statusConsole.Text += status + "\n";
                statusConsole.SelectionStart = statusConsole.Text.Length;
                statusConsole.ScrollToCaret(); ;
            }
        }

        private void insertSubjects()
        {
            addControl((statusConsole = new RichTextBox()), 0, 9, AnchorStyles.None, 2);
            layout.SetRowSpan(statusConsole, 2);
        }

        private void insertStatusConsole() {
            addControl((subjects = new ExtendedDataGridView()), 1, 6, AnchorStyles.None);
            layout.SetRowSpan(subjects, 2);
        }

    	private void insertPictureBox()
		{
			addControl((lookupPic = new PictureBox()), 0, 4, AnchorStyles.None);
			layout.SetRowSpan(lookupPic, 4);

			addControl((studentPic = new PictureBox()), 1, 1, AnchorStyles.None);
			layout.SetRowSpan(studentPic, 4);
		}

		private void insertTextBox()
		{
			addControl((searchBox = new ComboBox()),0,1, AnchorStyles.None);
		}

		private void insertButtons()
		{
			addControl((lookupBtn = new Button()), 0, 2, AnchorStyles.None, 1, "Lookup Student");
			addControl((recordBtn = new Button()), 0, 8, AnchorStyles.None, 1, "Record Attendance");
			addControl((finaliseBtn = new Button()), 1, 8, AnchorStyles.None, 1, "Output Absentees and Attendees");
		}

		private void insertLabels()
		{
			addControl(new Label(), 0, 0, AnchorStyles.None, 1, "Enter Student Name");
			addControl(new Label(), 1, 0, AnchorStyles.None, 1, "Latest Attendance");
			addControl(new Label(), 1, 5, AnchorStyles.None, 1, "Subjects in Exam");
			addControl(new Label(), 0, 3, AnchorStyles.None, 1, "Searched Student");
        }
	}
}
