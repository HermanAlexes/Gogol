using System.Web.Mvc;

namespace Gogol.Entities
{
    public class PublisherDto
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}