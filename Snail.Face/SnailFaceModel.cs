using FaceRecognitionDotNet;
using Snail.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snail.Face
{
    public class SnailFaceModel : IEntityId<string>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public FaceEncoding FaceEncoding { get; set; }
        public string Region { get; set; }
        public byte[] FaceEncodingBytes { get; set; }
    }
}
