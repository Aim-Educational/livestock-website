using System;

namespace Livestock.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string AdditionalInfo { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}