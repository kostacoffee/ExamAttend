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
	public partial class GridViewForm : Form
	{
		private ExtendedDataGridView grid;
	
		public GridViewForm(params string[] fields) {
			InitializeComponent();
			grid = new ExtendedDataGridView();
			for (int i = 0; i < fields.Length; i++)
				grid.Columns.Add(fields[i], Char.ToUpper(fields[i][0]) + fields[i].Substring(1));
			grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AllowDrop = false;
            grid.ScrollBars = ScrollBars.Vertical;
			this.Width = 500;
			this.Height = 500;
			resize(this, new EventArgs());
			this.Controls.Add(grid);
			this.Text = "Students";
			this.Show();
			this.Resize += resize;
		}

		private void resize(object sender, EventArgs e) {
			grid.Width = this.Width-15;
			grid.Height = this.Height-38;
			grid.Columns[0].Width = grid.Width - 200;
			grid.Columns[1].Width = 200;
		}

        public void removeRecord(Student student) {
            grid.removeRecord<Student>(student);
        }

		internal void addRecord(Student student)
		{
			grid.Rows.Add(student, student.id);
		}

        protected override void OnClosed(EventArgs e)
        {
            Application.Exit();
        }
	}
}
