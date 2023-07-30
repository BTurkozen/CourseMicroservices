using System;

namespace Course.Web.Models.CatalogVMs
{
    public sealed class CourseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description; }

        public string Picture { get; set; }

        public DateTime CreatedTime { get; set; }

        public FeatureViewModel Feature { get; set; }

        public string CategoryId { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}
