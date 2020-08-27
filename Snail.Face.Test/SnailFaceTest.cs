using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Snail.Face.Test
{
    public class SnailFaceTest
    {
        [Fact]
        public void Test()
        {
            try
            {
                var provider = TestServiceProviderBuilder.Build();
                var db = provider.GetService<DbContext>();
                db.Database.EnsureCreated();
                var faceService = provider.GetService<ISnailFaceRecognition>();
                faceService.AddUserFace(File.ReadAllBytes(@"C:\Users\37308\Desktop\pic\obama1.jpg"), "001");
                faceService.AddUserFace(File.ReadAllBytes(@"C:\Users\37308\Desktop\pic\obama2.jpg"), "002");
                faceService.AddUserFace(File.ReadAllBytes(@"C:\Users\37308\Desktop\pic\obama3.jpg"), "003");
                var us = faceService.RecognizeUser(File.ReadAllBytes(@"C:\Users\37308\Desktop\pic\obama3.jpg"));
                
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }
    }
}
