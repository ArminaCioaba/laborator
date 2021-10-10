//using System;
using System.Collections.Generic;
using Models;




namespace Repo
{
    public static class StudentsRepo
    {
        public static List<Student> Students= new List<Student> (){
            new Student() {ID= 1, Nume="Cioaba", Prenume="Armina", Facultate="AC", Sectie="IS", An_studiu= 4, Medie=8.57, Taxa=false, Bursa=false},
            new Student() {ID= 2, Nume="Bosoanca", Prenume="Dana", Facultate="AC", Sectie="CTI", An_studiu= 4, Medie=10.00, Taxa=false, Bursa=true},
            new Student() {ID= 3, Nume="Popescu", Prenume="Dan", Facultate="AC", Sectie="IS", An_studiu= 1, Medie=6.66, Taxa=true, Bursa=false},
            new Student() {ID= 4, Nume="Ionescu", Prenume="Diana", Facultate="AC", Sectie="CTI", An_studiu= 2, Medie=7.30, Taxa=false, Bursa=false}

        };

    }
}