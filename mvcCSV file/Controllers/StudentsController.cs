using CsvHelper;//using this module to try and upload read and write cvs files
using Microsoft.AspNetCore.Mvc;// to use httpGet 
using mvcCSV_file.Models; //getting student from model 
using System.Globalization;

namespace mvcCSV_file.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet] // getting the data 
        public IActionResult Index(List<Student> students = null) // action result is used to tell the sever how to respond to the request 
        {
            students = students == null ? new List<Student>() : students; // student is null create new student otherise just show the students 
            return View(students);
        }

        [HttpPost] //used to post the data
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            #region Upload CSV
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream); //copying the file to the file stream 
                fileStream.Flush(); //Clears buffers for this stream and causes any buffered data to be written to the file.
            }
            #endregion

            var students = this.GetStudentList(file.FileName);
            return Index(students);
        }
        private List<Student> GetStudentList(string fileName) //using private constuctor because we dont need another instance 
        {
            List<Student> students = new List<Student>();

            #region Read CSV
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName; // goes to the file dirrectory
            using (var reader = new StreamReader(path)) //invokes stream reader with the set path 
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) //csv is given the variable reader and fommated with culureinfo
            {
                csv.Read();//envoking read and readheader 
                csv.ReadHeader();
                while (csv.Read())
                {
                    var student = csv.GetRecord<Student>();// assinging the records on to student 
                    students.Add(student);
                }
            }
            #endregion

            #region Create CSV
            path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\FilesTo"}"; // setting the path 
            using (var write = new StreamWriter(path + "\\NewFile.csv")) //setting file name to path 
            using (var csv = new CsvWriter(write, CultureInfo.InvariantCulture)) //assigning a new instance of csv writer to a variable using culture info to fomat it 
            {
                csv.WriteRecords(students); //writing the file 
            }
            #endregion

            return students;
        }
        }
}
