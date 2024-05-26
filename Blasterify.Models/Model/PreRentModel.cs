using Blasterify.Models.Response;
using System;
using System.Collections.Generic;

namespace Blasterify.Models.Model
{
    public class PreRentModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public DateTime? Date { get; set; } = DateTime.MinValue;
        public string Name { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public bool IsEnabled { get; set; }
        public int ClientUserId { get; set; }
        public int StatusId { get; set; }
        public Dictionary<int, PreRentItemModel> PreRentItems { get; set; } = new Dictionary<int, PreRentItemModel>();

        public PreRentModel() { }

        public PreRentModel(PreRentResponse preRentResponse)
        {
            Id = preRentResponse.Id;
            Date = preRentResponse.Date;
            Name = preRentResponse.Name;
            Address = preRentResponse.Address;
            CardNumber = preRentResponse.CardNumber;
            IsEnabled = preRentResponse.IsEnabled;
            ClientUserId = preRentResponse.ClientUserId;
            StatusId = preRentResponse.StatusId;

            foreach (var item in preRentResponse.PreRentItems)
            {
                PreRentItems.Add(item.MovieId, new PreRentItemModel(item));
            }
        }
    }
}