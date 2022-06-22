using CsvHelper.Configuration.Attributes;

namespace mvcCSV_file.Models
{
    public class Student
    {
        [Index(0)] //reminder *using the csv helper configuration to give them the index atrribute 
        public string Name { get; set; } = "";//getting setting name 
        [Index(2)]
        public string Roll { get; set; } = "";//getting and setting roll
        [Index(1)]
        public string Email { get; set; } = "";// getting and setting email
    }
}
