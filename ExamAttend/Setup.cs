using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ExamAttend
{
	public partial class Setup : Form
	{
		ExamSetupUI ui;
		int year;
		SubjectRollDatabase studentDatabase;
		GridViewForm studentsGrid;
		List<Subject> subjects;
		List<Subject> selectedSubjects = new List<Subject>();
        Subject subjectToRemove;
        

        //Constructor
		public Setup()
		{
			InitializeComponent();
            this.Text = "Exam Setup";
			studentsGrid = UIBuilder.buildUI("name", "id");
            initDatabase();
			ui = UIBuilder.buildUI<ExamSetupUI>(this);
			ui.lockInBtn.Click += lockInYear;
			ui.addBtn.Click += subjectAdded;
			ui.startBtn.Click += launchExam;
            ui.searchBox.KeyDown += validateAddSubject;
            ui.searchBox.TextChanged += validateAddBtn;
			ui.startBtn.Enabled = false;
            ui.searchBox.Enabled = false;
			ui.addBtn.Enabled = false;
            ui.subjects.MouseDown += selectSubjectToRemove;
            ui.subjects.ContextMenuStrip = new ContextMenuStrip();
            ui.subjects.ContextMenuStrip.ItemClicked += removeSubject;
		}

        //Initialises the Student database by reading the config file for data file paths and constructing it.
        private void initDatabase()
        {
            StreamReader f = new StreamReader("config.txt");
            string subjectPath = f.ReadLine();
            string studentPath = f.ReadLine();
            f.Close();
            this.studentDatabase = new SubjectRollDatabase(studentPath, subjectPath);
        }

        //Checks whether the Enter key was clicked, signalling an adding of subject.
        private void validateAddSubject(object sender, KeyEventArgs e)
        {
            if (e.KeyData.Equals(Keys.Enter))
                subjectAdded(sender, new EventArgs());
        }

        //Add button can only be used when the searchBox has text.
        private void validateAddBtn(object sender, EventArgs e)
        {
            if (ui.searchBox.Text.Length > 0)
                ui.addBtn.Enabled = true;
        }

        //Used to allow for right-clicking to remove subjects
        private void selectSubjectToRemove(object sender, MouseEventArgs e)
        {
            //http://stackoverflow.com/questions/5239913/get-rowindex-via-contextmenu
 	        if (e.Button == MouseButtons.Right) {
                DataGridView.HitTestInfo hit = ui.subjects.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    subjectToRemove = (Subject)ui.subjects.Rows[hit.RowIndex].Cells[0].Value;
                    ui.subjects.ContextMenuStrip.Items.Clear(); 
                    ui.subjects.ContextMenuStrip.Items.Add("Remove " + subjectToRemove.ToString());
                }
            }
        }

        //Removes subject from the SelectedSubjects list.
        private void removeSubject (object sender, ToolStripItemClickedEventArgs e)
		{
			selectedSubjects.Remove(subjectToRemove);
            subjects.Add(subjectToRemove);
			ui.subjects.removeRecord<Subject> (subjectToRemove);
			ui.updateSearchBox<Subject>(subjects);
			removeStudents(subjectToRemove);
            if (selectedSubjects.Count == 0)
                ui.startBtn.Enabled = false;
        }

        //Launches exam
		private void launchExam(object sender, EventArgs e) {
            string question = "This will begin recording attendances of students.\nAre you sure you want to do this?\nThis action cannot be undone.";
            if (SimplifiedMessages.SimplifiedMessageBox.Question(question).Equals(DialogResult.Yes))
            {
                this.Hide();
                new Attender(studentsGrid, selectedSubjects, studentDatabase).Show();
            }
		} 

        //Gets the subjects in the exam by locking in the year and passing it into the database.
        private void lockInYear(object sender, EventArgs e)
        {
            ui.y11Rb.Enabled = false;
			ui.y12Rb.Enabled = false;
            List<RadioButton> years = new List<RadioButton>(new RadioButton[] { ui.y11Rb, ui.y12Rb });
            year = 11 + years.IndexOf(ui.clickedRb);
            ui.lockInBtn.Enabled = false;
            ui.searchBox.Enabled = true;
            subjects = new List<Subject>(studentDatabase.getSubjects(year));
			ui.updateSearchBox<Subject>(subjects);   
        }

        //Adds selected subject to the selectedSubjects List.
		private void subjectAdded(object sender, EventArgs e) {
            if (ui.searchBox.SelectedItem != null)
            {
                ui.startBtn.Enabled = true;
                Subject selectedSubject = (Subject)ui.searchBox.SelectedItem;
                selectedSubjects.Add(selectedSubject);
                ui.subjects.Rows.Add(selectedSubject);
                subjects.Remove(selectedSubject);
                ui.updateSearchBox<Subject>(subjects);
                addStudents(selectedSubject);
            }
		}

        //Adds students to the gridView based on the subject given.
		private void addStudents(Subject subject) {
            Student[] students = studentDatabase.getStudents(subject);
            for (int i = 0; i < students.Length; i++) {
				studentsGrid.addRecord(students[i]);
            }
		}

        //Removes all students for a particular subject from the gridView.
		private void removeStudents(Subject subject) {
			Student[] students = studentDatabase.getStudents(subject);
			foreach (Student s in students) {
				studentsGrid.removeRecord(s);
			}
		}

        protected override void OnClosed(EventArgs e)
        {
            Application.Exit();
        }
	}
}
