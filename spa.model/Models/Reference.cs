using System.ComponentModel.DataAnnotations.Schema;

namespace spa.model
{
    public class Reference
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReferenceId { get; set; }
        public string Title { get; set; }
        public string Pages { get; set; }
        public ReferenceType Type { get; set; }
        public Language Language { get; set; }
    }

    public enum ReferenceType
    {
        Software, Website
    }
}
