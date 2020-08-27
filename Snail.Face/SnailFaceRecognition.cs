using FaceRecognitionDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Snail.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Snail.Face
{
    public class SnailFaceRecognition : ISnailFaceRecognition
    {
        private FaceRecognition _faceRecognition;
        private IOptionsMonitor<SnailFaceRecognitionOption> _optionsMonitor;
        private static object _faceRecognitionLock = new object();
        private DbContext _db;
        private List<SnailFaceModel> _allFace = new List<SnailFaceModel>();

        public SnailFaceRecognition(DbContext db, IOptionsMonitor<SnailFaceRecognitionOption> optionsMonitor)
        {
            _db = db;
            _optionsMonitor = optionsMonitor;
        }
        public void AddUserFace(byte[] facePicBytes, string userId, string region = "default")
        {
            EnsureFaceRecognition();
            if (!_db.Set<SnailFaceModel>().Any(a => a.Region == region && a.UserId == userId))
            {
                using (var stream = new MemoryStream(facePicBytes))
                {
                    var img = FaceRecognition.LoadImage(new Bitmap(stream));
                    var locations = _faceRecognition.FaceLocations(img).ToList();
                    var encodings = _faceRecognition.FaceEncodings(img, locations).ToList();
                    if (encodings == null || encodings.Count != 1)
                    {
                        throw new Exception("xx");
                    }
                    _allFace.Add(new SnailFaceModel { UserId = userId, FaceEncoding = encodings.First(), Region = region });
                    _db.Set<SnailFaceModel>().Add(new SnailFaceModel
                    {
                        Id = IdGenerator.Generate<string>(),
                        FaceEncodingBytes = SerializeFace(encodings.First()),
                        Region = region,
                        UserId = userId
                    });
                    _db.SaveChanges();
                }
            }
            else
            {
                throw new Exception("xx");
            }

        }

        public int DetectUserNum(byte[] facePicBytes)
        {
            EnsureFaceRecognition();
            using (var stream = new MemoryStream(facePicBytes))
            {
                var img = FaceRecognition.LoadImage(new Bitmap(stream));
                var locations = _faceRecognition.FaceLocations(img).ToList();
                var encodings = _faceRecognition.FaceEncodings(img, locations).ToList();
                return encodings.Count;
            }
        }

        public string RecognizeUser(byte[] facePicBytes, string region = "default")
        {
            EnsureFaceRecognition();
            var userId = "";
            using (var stream = new MemoryStream(facePicBytes))
            {
                var img = FaceRecognition.LoadImage(new Bitmap(stream));
                var locations = _faceRecognition.FaceLocations(img).ToList();
                var encodings = _faceRecognition.FaceEncodings(img, locations).ToList();
                if (encodings == null || encodings.Count != 1)
                {
                    throw new Exception("xx");
                }
                foreach (var face in _allFace.Where(a => a.Region == region))
                {
                    if (FaceRecognition.CompareFace(face.FaceEncoding, encodings.First(), _optionsMonitor.CurrentValue.Tolerance))
                    {
                        userId = face.UserId;
                        break;
                    }
                }
            }
            return userId;
        }
        public void RemoveUserFace(string userId, string region = "default")
        {
            var face = _allFace.FirstOrDefault(a => a.Region == region && a.UserId == userId);
            if (face != null)
            {
                _allFace.Remove(face);
            }
            var faceEntity = _db.Set<SnailFaceModel>().FirstOrDefault(a => a.Region == region && a.UserId == userId);
            if (faceEntity != null)
            {
                _db.Set<SnailFaceModel>().Remove(faceEntity);
            }
            _db.SaveChanges();
        }

        private void EnsureFaceRecognition()
        {
            if (_faceRecognition == null)
            {
                lock (_faceRecognitionLock)
                {
                    if (_faceRecognition == null)
                    {
                        _faceRecognition = FaceRecognition.Create(_optionsMonitor.CurrentValue.ModelsDirectory);
                    }
                    LoadFaceFromDb();
                }
            }
        }
        private void LoadFaceFromDb()
        {
            var allFaceEntities = _db.Set<SnailFaceModel>().AsNoTracking().ToList();
            foreach (var faceEntity in allFaceEntities)
            {
                try
                {
                    var faceEncoding = DeserializeFace(faceEntity.FaceEncodingBytes);
                    _allFace.Add(new SnailFaceModel
                    {
                        FaceEncoding = faceEncoding,
                        Id = faceEntity.Id,
                        Region = faceEntity.Region,
                        UserId = faceEntity.UserId
                    });
                }
                catch (Exception ex)
                {
                    // log
                }
            }
        }

        private byte[] SerializeFace(FaceEncoding faceEncoding)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, faceEncoding);
                return stream.ToArray();
            }
        }
        private FaceEncoding DeserializeFace(byte[] facePicBytes)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream(facePicBytes))
            {
                return (FaceEncoding)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
