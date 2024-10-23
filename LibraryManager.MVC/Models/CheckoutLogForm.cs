using LibraryManager.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.MVC.Models;

public class CheckoutLogForm
{
    public int? CheckoutLogID { get; set; }
    [Required]
    public int BorrowerID { get; set; }
    [Required]
    public int MediaID { get; set; }
    [Required]
    public DateTime CheckoutDate { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// initialize default CheckoutLogForm DTO
    /// </summary>
    public CheckoutLogForm() { }

    /// <summary>
    /// transport data from CheckoutLog entity to CheckoutLogForm DTO
    /// </summary>
    /// <param name="entity"></param>
    public CheckoutLogForm(CheckoutLog entity)
    {
        CheckoutLogID = entity.CheckoutLogID;
        BorrowerID = entity.BorrowerID;
        MediaID = entity.MediaID;
        CheckoutDate = entity.CheckoutDate;
        DueDate = entity.DueDate;
        ReturnDate = entity.ReturnDate;
    }

    /// <summary>
    /// transport data from CheckoutLog DTO to CheckoutLog entity
    /// </summary>
    /// <returns></returns>
    public CheckoutLog ToEntity()
    {
        return new CheckoutLog
        {
            CheckoutLogID = CheckoutLogID ?? 0,
            BorrowerID = BorrowerID,
            MediaID = MediaID,
            CheckoutDate = CheckoutDate,
            DueDate = DueDate,
            ReturnDate = ReturnDate
        };
    }
}
