using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Entities
{
    public class EntityBase
    {
        [Required, Key]
        public int Id { get; set; }

        [Column(TypeName = "timestampTz")]
        public DateTime CreationDate { get; set; }
    }
}
