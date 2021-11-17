using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.ResponseModels
{
    /// <summary>
    /// Defines the <see cref="RegisterModel" />.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the EmailId.
        /// </summary>
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
            + "@"
            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
            ErrorMessage = "You have entered an invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
