using System;
using System.Collections.Generic;

namespace Blasterify.Models.Model
{
    public class RentDetailModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public List<RentItemDetailModel> RentItemDetailModels { get; set; } = new List<RentItemDetailModel>();
    }
}