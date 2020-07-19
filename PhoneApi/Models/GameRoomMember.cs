using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneApi.Models
{
    public class GameRoomMember
    {
        public Guid OfferId { get; set; }

        public Guid UserId { get; set; }

        public bool IsHost { get; set; }
    }
}