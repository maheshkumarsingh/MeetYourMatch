namespace API.Entities;

public class Message : BaseModel
{
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
