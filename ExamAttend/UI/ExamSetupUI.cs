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
	public partial class ExamSetupUI : BaseUI
	{
        private Label selectYearLbl, yearLbl, searchLbl, subjectsLbl;
        public Button lockInBtn, addBtn, startBtn;
		public ExtendedDataGridView subjects;
		public RadioButton y11Rb, y12Rb;
        public RadioButton clickedRb;

		public ExamSetupUI()
		{
			InitializeComponent();
			base.layout = this.layout;
			insertLabels();
			insertButtons();
			insertGridViews();
			insertComboBox();
			insertRadioButtons();
		}

		private void insertRadioButtons()
		{
			addControl((y11Rb = new RadioButton()), 2, 1, AnchorStyles.None,2, "Year 11");
			addControl((y12Rb = new RadioButton()), 4, 1, AnchorStyles.None,2, "Year 12");
            y11Rb.Click += setSelected;
            y12Rb.Click += setSelected;
			y11Rb.Select();
			clickedRb = y11Rb;
		}

		private void setSelected(object sender, EventArgs e) {
			clickedRb = (RadioButton)sender;
		}

		private void insertComboBox()
		{
			addControl((searchBox = new ComboBox()), 0, 4, AnchorStyles.None, 6);
			searchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		}

		private void insertGridViews()
		{
			addControl((subjects = new ExtendedDataGridView()), 0, 7, AnchorStyles.None, 6);
			subjects.Columns.Add("subject", "Subject");
			subjects.AllowUserToAddRows = false;

		}

		private void insertButtons()
		{
			addControl((lockInBtn = new Button()), 0, 2, AnchorStyles.None, 6, "Confirm Year");
			addControl((addBtn = new Button()), 0, 5, AnchorStyles.None,6, "Add Subject");
			addControl((startBtn = new Button()), 0, 8, AnchorStyles.None, 6, "Start Exam");
		}

		private void insertLabels()
		{
			addControl((selectYearLbl = new Label()), 0, 0, AnchorStyles.None, 6, "Select Year");
			addControl((yearLbl = new Label()), 0, 1, AnchorStyles.None, 2, "Year:");
			addControl((searchLbl = new Label()), 0, 3, AnchorStyles.Bottom, 6, "Search Subjects");
			addControl((subjectsLbl = new Label()), 0, 6, AnchorStyles.Bottom, 6, "Subjects in Exam");
		}
	}
}
