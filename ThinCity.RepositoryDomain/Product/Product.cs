using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProdCategory = ThinCity.RepositoryDomain.ProductCategory;

namespace ThinCity.RepositoryDomain.Product
{
    [Table("")]
    public class Product : TEntity<Guid>, IAggregateRoot
    {
        [Key]
        [Column("ProductID")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public int ProductCategoryId { get; set; }

        [ForeignKey(nameof(ProductCategoryId))]
        public virtual ProdCategory.ProductCategory ProductCategory { get; set; }

        public override Guid GetId()
        {
            return Id;
        }
    }
}
