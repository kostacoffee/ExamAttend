using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamAttend
{
	public partial class StudentSelectForm : Form
	{
		
		//layout constants
		const int VERTICAL_MARGIN = 30;
		const int RECORD_DIVIDE = 10;
		const int HORIZONTAL_MARGIN = 30;
		const int PICTURE_WIDTH = 110;
		const int PICTURE_HEIGHT = 120;
		//end layout constants
		
		private Student selectedStudent;
		private Student[] students;

		internal StudentSelectForm(Student[] students)
		{
			InitializeComponent();
			this.students = students;
			this.Width = 300;
			for (int i = 0; i < students.Length; i++) {
				PictureBox studentImage = new PictureBox();
				studentImage.ImageLocation = students[i].imagePath;
				studentImage.Width = PICTURE_WIDTH;
				studentImage.Height = PICTURE_HEIGHT;
				studentImage.Left = HORIZONTAL_MARGIN;
				studentImage.Top = VERTICAL_MARGIN + (RECORD_DIVIDE * i);
				studentImage.Click += selectStudent;
				this.Controls.Add(studentImage);
			}

		}

		private void selectStudent(object sender, EventArgs e) {
			PictureBox box = (PictureBox) sender;
			this.selectedStudent = students.First(student => student.imagePath.Equals(box.ImageLocation));
			this.Close();
		}

		internal Student getSelectedStudent()
		{
			return selectedStudent;
		}

	}
}
