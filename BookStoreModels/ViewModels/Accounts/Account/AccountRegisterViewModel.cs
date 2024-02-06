using System.ComponentModel.DataAnnotations;

public class AccountRegisterViewModel
{
    [Required(ErrorMessage = "Pole 'Username' jest wymagane.")]
    [RegularExpression("^[a-zA-Z0-9]{5,30}$", ErrorMessage = "Pole 'Username' musi zawierać litery i cyfry, bez znaków specjalnych, od 5 do 30 znaków.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Pole 'Email' jest wymagane.")]
    [EmailAddress(ErrorMessage = "Wprowadź prawidłowy adres e-mail.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Pole 'Password' jest wymagane.")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{5,20}$",
        ErrorMessage = "Hasło musi mieć od 5 do 20 znaków i zawierać co najmniej jedną dużą literę, jedną cyfrę i jeden znak specjalny.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Pole 'Name' jest wymagane.")]
    [RegularExpression("^[a-zA-Z]{2,30}$", ErrorMessage = "Pole 'Name' musi zawierać tylko litery, od 2 do 30 znaków.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Pole 'Surname' jest wymagane.")]
    [RegularExpression("^[a-zA-Z]{2,30}$", ErrorMessage = "Pole 'Surname' musi zawierać tylko litery, od 2 do 30 znaków.")]
    public string Surname { get; set; }

    [Phone(ErrorMessage = "Wprowadź prawidłowy numer telefonu.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Pole 'IsSubscribed' jest wymagane.")]
    public bool IsSubscribed { get; set; }
}
