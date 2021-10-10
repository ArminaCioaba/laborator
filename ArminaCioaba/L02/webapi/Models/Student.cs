using System;

namespace Models
{
    public class Student
    {
        public int ID { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Facultate { get; set; }
        public string Sectie { get; set; }
        public int An_studiu { get; set; }
        public double Medie { get; set; }
        public bool Taxa { get; set; }
        public bool Bursa { get; set; }

    }
}