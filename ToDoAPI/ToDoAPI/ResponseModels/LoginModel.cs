using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.ResponseModels
{
    /// <summary>
    /// Defines the <see cref="LoginModel" />.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Minimum eight characters, at least one uppercase letter,"
             + "one lowercase letter, one number and one special character")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
