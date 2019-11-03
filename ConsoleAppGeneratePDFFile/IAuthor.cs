using System;

namespace ConsoleAppGeneratePDFFile
{
    public interface IAuthor
    {
        int Age { get; set; }
        string BookTitle { get; set; }
        bool IsMVP { get; set; }
        string Name { get; set; }
        DateTime PublishedDate { get; set; }
    }
}