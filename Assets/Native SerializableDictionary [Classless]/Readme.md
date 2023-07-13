If you're using a Unity version <2018.4 where Newtonsoft.JSON was internalised into Unity's assembly, you can add it using this process:

1. Go to https://github.com/JamesNK/Newtonsoft.Json/releases
2. Download the .zip for the version of your choosing
3. Inside of the .zip file, navigate to the Bin folder
4. Go into the folder which is the same .Net version as your Unity project
5. Extract the .dll file labelled "Newtonsoft.Json.dll"
6. Put the .dll in your project (consider making a folder for it; perhaps named "Newtonsoft")
7. Right click inside of the Unity Editor in the same folder you have the .dll in and Navigate to Create -> Assembly Definition
8. In the Assembly Definition, you will find Assembly  Definition References
9. Press the + icon and drag or select from the pop-up menu the Newtonsoft.Json.dll file
10. This should allow you to now use the Newtonsoft.JSON package
11. Now remove the #if UNITY_2018_4_or_newer #endif dependent compilation I've added for compilation safety measures
12. Now you can use Newtonsoft.JSON :)
