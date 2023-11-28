using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MultiMarketing.Context.Domain
{
    public class UserRoles
    {
            [Key]
            public Guid Id { get; set; }
            public Guid? UserId { get; set; }
            [ForeignKey("UserId")]
            public virtual UserRegister? Customer { get; set; }
            public string? Role { get; set; }  
        
    }
}
