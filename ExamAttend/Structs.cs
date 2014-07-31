using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExamAttend
{

    // Record used to hold all data for a student not used in processing.
    public struct StudentData
    {
        public string firstName { get; private set; }
        public string lastName { get; private set; }
        public int year { get; private set; }
        public StudentData(string firstName, string lastName, int year)
            : this()
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.year = year;
        }
        public override string ToString()
        {
            return lastName.ToUpper() + " " + firstName;
        }
    }

    //Main student record containing all information relevant to a particular student.
    public struct Student
    {
        public StudentData data { get; private set; }
        public int id;
		public string imagePath { get; private set; }
        public Student(StudentData data, int id, string imagePath) : this()
        {
            this.data = data;
            this.id = id;
            this.imagePath = imagePath;
        }

		public override string ToString ()
		{
			return data.ToString();
		}
    }

    //Record used to store all information about a subject.
    public struct Subject
    {
        public string name { get; private set; }
        public int year { get; private set; }
        public Subject(string name, int year)
            : this()
        {
            this.name = name;
            this.year = year;
            
        }
        public override string ToString()
        {
            return this.name;
        }
    }

}
