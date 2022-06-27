using Core.Models;

namespace Core.Views;

public class Item {
   public long   Id          { get; set; }
   public string Description { get; set; }

   public Item() { }
   public Item(long id, string description) { Id = id; Description = description; }
}
