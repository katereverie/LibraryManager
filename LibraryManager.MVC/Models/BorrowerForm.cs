using LibraryManager.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.MVC.Models;

public class BorrowerForm
{
    public int? BorrowerID { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }

    /// <summary>
    /// initialize default BorrowerForm DTO
    /// </summary>
    public BorrowerForm() { }

    /// <summary>
    /// transport data from Borrower entity to BorrowerForm DTO
    /// </summary>
    /// <param name="entity"></param>
    public BorrowerForm(Borrower entity)
    {
        BorrowerID = entity.BorrowerID;
        FirstName = entity.FirstName;
        LastName = entity.LastName;
        Email = entity.Email;
        Phone = entity.Phone;
    }

    /// <summary>
    /// transport data from BorrowerForm DTO to Borrower entity
    /// </summary>
    /// <returns></returns>
    public Borrower ToEntity()
    {
        return new Borrower
        {
            BorrowerID = BorrowerID ?? 0,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone = Phone,
        };
    }
}
