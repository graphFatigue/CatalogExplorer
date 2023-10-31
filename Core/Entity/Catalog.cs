using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class Catalog
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentCatalogId { get; set; }
        public virtual Catalog? ParentCatalog { get; set; }
        public virtual ICollection<Catalog>? ChildrenCatalogs { get; set; }
    }

}
