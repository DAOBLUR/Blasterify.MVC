using System.Collections.Generic;

namespace Blasterify.Models.Yuno
{
    public class ErrorResponse
    {
        public string Code { get; set; }
        public List<string> Messages { get; set; }
    }
}