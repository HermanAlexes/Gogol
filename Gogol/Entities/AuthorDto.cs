using System.Web.Mvc;

namespace Gogol.Entities
{
    public class AuthorDto
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; } 
        public string Name { get; set; }
    }
}