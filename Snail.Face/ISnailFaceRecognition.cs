namespace Snail.Face
{
    public interface ISnailFaceRecognition
    {
        void AddUserFace(byte[] facePicBytes, string userId, string region = "default");
        void RemoveUserFace(string userId, string region = "default");
        string RecognizeUser(byte[] facePicBytes, string region = "default");
        int DetectUserNum(byte[] facePicBytes);
    }
}
