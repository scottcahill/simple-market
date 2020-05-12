using System;

namespace SmWeb.Models
{
    public class TranStateElement
    {
        public DateTime LastUpdated { get; set; }
        public string PreviousState { get; set; }
        public string CurrentState { get; set; }
    }
}
