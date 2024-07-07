using System.ComponentModel.DataAnnotations;

namespace GidraTopServer.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Имя обязательное для заполнения поле")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия обязательное для заполнения поле")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email обзательное для заполнения поле")]
    [EmailAddress(ErrorMessage = "Некорректный email адрес")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
    [Phone(ErrorMessage = "Некорректный номер телефона")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Пароль обязательное для заполнения поле")]
    [MinLength(6, ErrorMessage = "Пароль должен быть длиной минимум 6 символов")]
    public required string Password { get; set; }

    public string Role { get; set; } = "USER";

    public Basket? Basket { get; set; }

    public ICollection<Rating>? Ratings { get; set; }
}
