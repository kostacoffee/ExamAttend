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
		public Label selectYearLbl, yearLbl, searchLbl, subjectsLbl, enrolledLbl;
		public Button lockIn, add, start, edit;
		public DataGridView subjects, students;
		public ComboBox search;
		private RadioButton y11, y12;
		private RadioButton clickedRB;

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
			addControl((y11 = new RadioButton()), 1, 1, AnchorStyles.None,1, "Year 11");
			addControl((y12 = new RadioButton()), 2, 1, AnchorStyles.None,1, "Year 12");
			y11.Click += setSelected;
			y12.Click += setSelected;
		}

		private void setSelected(object sender, EventArgs e) {
			clickedRB = (RadioButton)sender;
		}

		private void insertComboBox()
		{
			addControl((search = new ComboBox()), 0, 4, AnchorStyles.None, 3);
			search.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		}

		private void insertGridViews()
		{
			addControl((subjects = new DataGridView()), 0, 7, AnchorStyles.None, 3);
			addControl((students = new DataGridView()), 3, 1, AnchorStyles.None);
			layout.SetRowSpan(students, 7);
			students.Height = 400;
		}

		private void insertButtons()
		{
			addControl((lockIn = new Button()), 0, 2, AnchorStyles.None, 3, "Lock In");
			addControl((add = new Button()), 0, 5, AnchorStyles.None,3, "Add");
			addControl((start = new Button()), 0, 8, AnchorStyles.None, 3, "Start Exam");
			addControl((edit = new Button()), 3, 8, AnchorStyles.None, 1,"Edit");

		}

		private void insertLabels()
		{

			addControl((selectYearLbl = new Label()), 0, 0, AnchorStyles.None, 3, "Select Year");
			addControl((yearLbl = new Label()), 0, 1, AnchorStyles.None, 1, "Year:");
			addControl((searchLbl = new Label()), 0, 3, AnchorStyles.Bottom, 3, "Search Subjects");
			addControl((subjectsLbl = new Label()), 0, 6, AnchorStyles.Bottom, 3, "Subjects in Exam");
			addControl((enrolledLbl = new Label()), 3, 0, AnchorStyles.None, 1, "Enrolled Students");

		}
	}
}
