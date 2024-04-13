﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatServer.Models
{
    [Table("UserConnection")]
    public class UserConnection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserConnectionId { get; set; }

        public string UserName { get; set; }
        public string ChatRoom { get; set; }
        public int AccountId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}