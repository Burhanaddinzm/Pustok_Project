using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pustok_Project.ViewModels
{
    public class LoginVM
    {
        [DisplayName("Username")]
        public string UserName { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
