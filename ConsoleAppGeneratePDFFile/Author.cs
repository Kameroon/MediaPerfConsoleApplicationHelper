﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppGeneratePDFFile
{
    public class Author : IAuthor
    {
        private string name;
        private int age;
        private string title;
        private bool mvp;
        private DateTime pubdate;

        public Author(string name, int age, string title, bool mvp, DateTime pubdate)
        {
            this.name = name;
            this.age = age;
            this.title = title;
            this.mvp = mvp;
            this.pubdate = pubdate;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public string BookTitle
        {
            get { return title; }
            set { title = value; }
        }
        public bool IsMVP
        {
            get { return mvp; }
            set { mvp = value; }
        }
        public DateTime PublishedDate
        {
            get { return pubdate; }
            set { pubdate = value; }
        }
    }
}
