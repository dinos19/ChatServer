using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatServer.Models.Entity
{
    [Table("Uploads")]
    public class UploadResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FileName { get; set; }
        public string StoredFileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime DateCreated { get; set; }
    }
}