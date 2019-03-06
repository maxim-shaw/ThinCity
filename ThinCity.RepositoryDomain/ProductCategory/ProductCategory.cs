using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinCity.RepositoryDomain.ProductCategory
{
    [Table("ProductCategory")]
    public class ProductCategory : TEntity<int>, IAggregateRoot
    {
        [Key]
        [Column("ProductCategoryId")]
        public int Id { get; set; }
        public int ParentCategoryId { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(ParentCategoryId))]
        public virtual ProductCategory ParentProductCategory { get; set; }

        public override int GetId()
        {
            return Id;
        }
    }
}
