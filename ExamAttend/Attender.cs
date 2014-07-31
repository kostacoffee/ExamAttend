using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using SimplifiedMessages;

namespace ExamAttend
{
	public partial class Attender : Form
	{
		GridViewForm studentGrid;
		List<Student> students;
        List<Student> attendees;
		ExamAttenderUI ui;
		Student currentLookup;
		SerialPort[] scanners;
		delegate void searchType(string searchTerm); 

        //Constructor
		internal Attender(GridViewForm studentGrid, List<Subject> subjects, SubjectRollDatabase database) {
			InitializeComponent();
            this.attendees = new List<Student>();
            this.Text = "Exam Attender";
			initScanners(SerialPort.GetPortNames());
			this.studentGrid = studentGrid;
			this.ui = UIBuilder.buildUI<ExamAttenderUI>(this);
			this.studentGrid.Show();
			this.students = retrieveAllStudents(subjects, database);
			ui.lookupBtn.Enabled = true;
            ui.updateSearchBox<Student>(students);
			ui.searchBox.TextChanged += checkButtonValidity;
			ui.searchBox.KeyDown += searchBoxEnterDown;
			ui.lookupBtn.Click += lookupBtnClicked;
			ui.recordBtn.Click += recordBtnClicked;
			ui.finaliseBtn.Click += finalise;
			ui.recordBtn.Enabled = false;
            displaySubjects(subjects);
		}

        //Adds all subjects to the subject gridview.
        private void displaySubjects(List<Subject> subjects)
        {
            ui.subjects.Columns.Add("subject", "Subject");
            foreach (Subject s in subjects)
                ui.subjects.Rows.Add(s);
        }

        //Initialises all valid COM Ports as possible scanners.
		private void initScanners (string[] validPorts)
		{
			scanners = new SerialPort[validPorts.Length];
			for (int i = 0; i < validPorts.Length; i++){
				scanners[i] = new SerialPort(validPorts[i], 9600,Parity.None,8,StopBits.One);
				scanners[i].Handshake = Handshake.None;
				scanners[i].DataReceived += autoAttend;
				scanners[i].Close();
				scanners[i].Open();
			}
		}

        //Retreves all students from the database based on the subjects chosen. Remvoes Duplicates.
		private List<Student> retrieveAllStudents(List<Subject> subjects, SubjectRollDatabase database) {
			List<Student> allStudents = new List<Student>();
			for (int i = 0; i < subjects.Count; i++)
				allStudents.AddRange(database.getStudents(subjects[i]));
            return allStudents.Distinct().ToList();
		}

        //Selects Student if enter key is pressed.
		private void searchBoxEnterDown(object sender, KeyEventArgs e) {
			if(e.KeyData.Equals(Keys.Enter) && !ui.searchBox.Text.Equals(""))
				displayLookup((Student)ui.searchBox.SelectedItem);
		}

        //Selects student if lookup button is clicked
		private void lookupBtnClicked (object sender, EventArgs e)
		{
			displayLookup((Student)ui.searchBox.SelectedItem);
		}

        //Records attendance of student if record button is clicked
		private void recordBtnClicked (object sender, EventArgs e)
		{
			recordAttendance(currentLookup);
			resetLookup();
		}

        //Lookup button can only be clicked if the searchbox has text.
		private void checkButtonValidity(object sender, EventArgs e)
		{
			ui.lookupBtn.Enabled = !(ui.searchBox.Text.Equals(""));
		}

        //Attends a student using one of the scanners
		private void autoAttend(object sender, EventArgs e)
		{
            SerialPort serial = (SerialPort)sender;
			string strid = serial.ReadLine();
            int id = Convert.ToInt32(strid);
            if (students.Count(s => s.id == id) == 0) return;
			Student attendingStudent = students.First(student => student.id == id);	
			ui.studentPic.ImageLocation = attendingStudent.imagePath;
			recordAttendance(attendingStudent);
		}
        
        //Records attendance of Student.
		private void recordAttendance(Student student) {
			studentGrid.removeRecord(student);
			students.Remove(student);
			ui.updateSearchBox<Student>(students);
            ui.studentPic.ImageLocation = student.imagePath;
            attendees.Add(student);
            ui.writeStatus(student.ToString() + " [" + student.id.ToString() + "] " + "attended");
		}

        //Resets lookup parameters when attendance is recorded.
		private void resetLookup ()
		{
			ui.recordBtn.Enabled = false;
			ui.lookupPic.Image = null;
			ui.searchBox.Text = "";
		}

        //Displayes student being looked up
		private void displayLookup(Student student) {
			ui.lookupPic.ImageLocation = student.imagePath;
			ui.recordBtn.Enabled = true;
			currentLookup = student;
		}

        //Finalises exam by outputting all attendeees and absentees to files.
		private void finalise(object sender, EventArgs e){
            string question = "This will output attendees and absentees into a file and close this program.\nAre you sure you want to do this?\nThis action cannot be undone.";
			if (SimplifiedMessages.SimplifiedMessageBox.Question(question).Equals(DialogResult.Yes)) {
                if (!Directory.Exists("FinalisedFiles")) Directory.CreateDirectory("FinalisedFiles");
                writeStudents(students.ToArray(), "FinalisedFiles/Absentees.txt");
                writeStudents(attendees.ToArray(), "FinalisedFiles/Attendees.txt");
				Application.Exit();
			}
		}

        //Function to write the students to a file
		private void writeStudents(Student[] students, string filePath){
			StreamWriter file = new StreamWriter(filePath);
			foreach (Student s in students) {
				file.WriteLine (s.ToString () + '\t' + s.id.ToString());
			}
			file.Close();
		}

        protected override void OnClosed(EventArgs e)
        {
            Application.Exit();
        }
	}
}
