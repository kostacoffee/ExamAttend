using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamAttend
{
    class SubjectRollDatabase
    {
        private delegate void FileOperation<T>(string[] line, int index, List<T> dataStore);
        private const int STUDENT_ID_INDEX = 0;
        private const int STUDENT_LAST_NAME_INDEX = 1;
        private const int STUDENT_FIRST_NAME_INDEX = 2;
        private const int STUDENT_YEAR_INDEX = 4;

        //Constants for Student Courses file
        private const int SUBJECT_ENROLLEE_ID_INDEX = 0;
        private const int SUBJECT_NAME_INDEX = 3;
        private const int SUBJECT_YEAR_INDEX = 5;

        private const string NO_IMAGE_PATH = "noImage.jpg";

        //Other
        private Dictionary<Subject, List<Student>> subjectRoll;
        private string currentId;
        private int studentCounter;
        private string studentImageFolder;

        //Constructor
        public SubjectRollDatabase(string studentFilePath, string subjectFilePath)
        {
            this.studentImageFolder = File.ReadAllLines("config.txt")[2];
            List<Student> students = processFile<Student>(studentFilePath, parseStudentLine, new List<Student>());
            this.studentCounter = 0;
            this.currentId = "1"; // since "" is valid, used something else.
            this.subjectRoll = new Dictionary<Subject, List<Student>>();
            processFile<Student>(subjectFilePath, enroll, students); // nothing to assign to, but still does the job.
        }

        // Returns all subjects based on the year provided.
        public Subject[] getSubjects(int year)
        {
            return subjectRoll.Keys.Where(key => key.year == year).ToArray();
        }

        // Returns all students from a specified year which have the specified subject.
        public Student[] getStudents(Subject subject)
        {
            return subjectRoll[subject].ToArray();
        }

        //Main Delegate Function - reads lines from a file and operates on them as per delegate passed.
        private List<T> processFile<T>(string filePath, FileOperation<T> op, List<T> dataStore)
        {
            string[] lines = File.ReadLines(filePath).ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                op(lines[i].Split(','), i, dataStore);
            }
            return dataStore;
        }

        // Returns student ID. If no ID available, returns position in student list.
        private int getStudentId(string studentIdString, int studentNumber) { 
            int studentId = 0;
            if(studentIdString.Length == 0) { // using Student location for no ID students, since they are placed in the same place in Students and subject files, making them retriveable
                studentId = studentNumber;
            } else studentId = Convert.ToInt32(studentIdString);
            return studentId;
        }

        //Creates and adds a student to the student list from the student file. 
        private void parseStudentLine(string[] studentData, int studentNumber, List<Student> students) {
            int studentId = getStudentId(studentData[SUBJECT_ENROLLEE_ID_INDEX],studentNumber);
            string studentImagePath = getStudentImagePath(studentId);
            Student parsedStudent = new Student(new StudentData(studentData[STUDENT_FIRST_NAME_INDEX], studentData[STUDENT_LAST_NAME_INDEX], Convert.ToInt32(studentData[STUDENT_YEAR_INDEX])), studentId, studentImagePath);
            students.Add(parsedStudent);
        }

        //Returns image path for the student. If no iamge found, returns a "noImage.jpg" path
        private string getStudentImagePath(int id) {
            string imagePath = studentImageFolder + "/" + id.ToString() + ".jpg";
            if (File.Exists(imagePath))
                return imagePath;
            else return studentImageFolder + NO_IMAGE_PATH;
        }


        //Enrolls each student into the subject provided by the file current line in file.
        private void enroll(string[] subjectData, int index, List<Student> students) {
            if (!currentId.Equals(subjectData[SUBJECT_ENROLLEE_ID_INDEX])) //file lists all student ID -> subject combos using student ID. Same as in Students file.
            {
                currentId = subjectData[SUBJECT_ENROLLEE_ID_INDEX];
                studentCounter++;
            }
            int studentId = currentId.Equals("") ? studentCounter : Convert.ToInt32(currentId);  
            Subject subject = new Subject(subjectData[SUBJECT_NAME_INDEX], Convert.ToInt32(subjectData[SUBJECT_YEAR_INDEX]));
            if (!subjectRoll.ContainsKey(subject)) subjectRoll.Add(subject, new List<Student>());
            Student student = students.First(s => s.id == studentId);
            subjectRoll[subject].Add(student);
        }
    }
}
