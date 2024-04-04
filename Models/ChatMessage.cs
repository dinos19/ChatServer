﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ChatServer.Models.Entity;

namespace ChatServer.Models
{
    public enum ChatMessageType : int
    {
        TEXT,
        IMAGE,
        AUDIO,
        VIDEO
    }

    public enum ChatMessageAction : int
    {
        ANNOUNCEMENTS,
        HELLO,
        WHOISON
    }

    [Table("ChatMessage")]
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatMessageId { get; set; }

        public ChatMessageAction Action { get; set; }
        public ChatMessageType Type { get; set; }
        public string Body { get; set; }
        public int FromAccountId { get; set; }

        [ForeignKey("FromAccountId")]
        public virtual Account FromAccount { get; set; }

        public int ToAccountId { get; set; }

        [ForeignKey("ToAccountId")]
        public virtual Account ToAccount { get; set; }
    }
}