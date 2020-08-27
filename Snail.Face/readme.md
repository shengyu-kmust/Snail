## 如何安装FaceRecognitionDotNet
* 在nuget里安装FaceRecognitionDotNet
* 安装后会自动依赖DlibDotNetNative.dll，DlibDotNetNativeDnn.dll,这两个dll的依赖路径为package路径的对应平台下，可以在csproj文件的Platforms下找，如<Platforms>x64</Platforms>
packages\dlibdotnet\19.18.0.20200525\runtimes\win-平台
即DlibDotNetNative.dll，DlibDotNetNativeDnn.dll这两个dll是分平台的，如支持如下平台win-x86；win-x64；centos-x64；linux-x64；osx-x64

## 人脸训练数据去https://github.com/ageitgey/face_recognition_models下载，即models目录下的文件