using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Services.Models
{
    public class ClientUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        [Required]
        [StringLength(16)]
        [MinLength(16)]
        [MaxLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        [MaxLength(35)]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public DateTime SuscriptionDate { get; set; }

        [Required]
        [ForeignKey("Subscription")]
        public int SubscriptionId { get; set; }

        /*
        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        */
    }
}

/*
public class ByteArrayConverter : JsonConverter<byte[]>
{
    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        short[] sByteArray = JsonSerializer.Deserialize<short[]>(ref reader);
        byte[] value = new byte[sByteArray.Length];
        for (int i = 0; i < sByteArray.Length; i++)
        {
            value[i] = (byte)sByteArray[i];
        }

        return value;
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var val in value)
        {
            writer.WriteNumberValue(val);
        }

        writer.WriteEndArray();
    }
} 
*/