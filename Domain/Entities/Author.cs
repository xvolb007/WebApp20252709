
namespace Domain.Entities
{
    public class Author
    {
        public long Id { get; set; } // Primary key
        // Unique index couldn't be nullable, marked as required in the model to avoid future confusion
        public required string Name { get; set; } // Unique index
        public virtual Image Image { get; set; } // One-To-One
    }

}
