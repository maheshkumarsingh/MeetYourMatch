using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public required string  SenderUserName{ get; set; }
    public required string  RecipientUserName{ get; set; }
    public required string  Content{ get; set; }
    public bool  SenderDeleted{ get; set; }
    public bool  RecipientDeleted{ get; set; }
    //properties like Id, MessageSent = CreatedAt.
    public DateTime? DateRead { get; set; }

    //navigations property
    public int SenderId { get; set; }
    public AppUser Sender { get; set; } = null!;
    public int RecipientId { get; set; }
    public AppUser Recipient { get; set; } = null!;
}
