using System.ComponentModel.DataAnnotations;

namespace Kviz.Model
{
  public class User
  {
    public int Id { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required, Compare("Password"), DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
  }
}
