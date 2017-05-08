using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Web.Mvc;

namespace Gogol.Entities
{
    public class BookDto
    {
        [HiddenInput(DisplayValue = false)]
        public Nullable<int> Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public double Price { get; set; }
        public string Illustrator { get; set; }
        public string Weight { get; set; }
        public string Size { get; set; }
        public string Series { get; set; }
        public string Language { get; set; }
        public List<String> Photos { get; set; }
        public Nullable<DateTime> PublishDate { get; set; }
    }
}