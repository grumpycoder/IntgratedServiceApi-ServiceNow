namespace IntegratedServicesApi.Nuvolo.Models;

public class ServiceCall
{
    public string ServiceCallId { get; set; }
    public string ShortDescription { get; set; }
    public string DetailedDescription { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public Location Location { get; set; }
    public string Priority { get; set; }
    public string Status { get; set; }
    public DateTime ScheduledStartDate { get; set; }
    public DateTime ScheduledEndDate { get; set; }
    public string AssignedTo { get; set; }
    public ContactInfo ContactInfo { get; set; }
    public List<ServiceItem> ServiceItems { get; set; }
    public List<Comment> Comments { get; set; }
}

public class Location
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}

public class ContactInfo
{
    public string Email { get; set; }
    public string Phone { get; set; }
}

public class ServiceItem
{
    public string ItemId { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
}

public class Comment
{
    public string CommentId { get; set; }
    public string Text { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
