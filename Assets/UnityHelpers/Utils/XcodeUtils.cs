#if UNITY_EDITOR && UNITY_IOS

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace UnityHelpers.Utils
{
    public static class XcodeUtils
    {
        public static void UpdateInfoPlist(string projectPath, Action<PlistDocument> handler)
        {
            var plist = new PlistDocument();
            var plistPath = projectPath + "/Info.plist";
            plist.ReadFromFile(plistPath);
            handler(plist);
            plist.WriteToFile(plistPath);
        }

        public static void AddFileOrDirectory(string projectPath, string sourcePath, bool copy = true)
        {
            Debug.Assert(File.Exists(sourcePath) || Directory.Exists(sourcePath));
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(PBXProject.GetPBXProjectPath(projectPath));
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            // Process source path
            sourcePath = sourcePath.TrimEnd('/');
            string sourceBasePath = null;
            if (copy)
            {
                string origSourcePath = sourcePath;
                sourcePath = Path.Combine(projectPath, Path.GetFileName(origSourcePath));
                sourceBasePath = projectPath;
                FileUtil.DeleteFileOrDirectory(sourcePath);
                FileUtil.CopyFileOrDirectory(origSourcePath, sourcePath);
            }
            else
            {
                sourceBasePath = Directory.GetParent(sourcePath).FullName;
            }

            // Add source to project
            if (File.Exists(sourcePath))
            {
                string pathInProject = sourcePath.Substring(sourceBasePath.Length);
                proj.AddFileToBuild(targetGuid, proj.AddFile(sourcePath, pathInProject, PBXSourceTree.Absolute));
            }
            else
            {
                // Add directory recursive lambda 
                Action<DirectoryInfo> addDirectoryToProject = null;
                addDirectoryToProject = (DirectoryInfo dir) =>
                {
                    foreach (var info in dir.GetFileSystemInfos())
                    {
                        if ((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                            continue;

                        if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory && !PBXProject.IsKnownExtension(info.Extension))
                        {
                            addDirectoryToProject(new DirectoryInfo(info.FullName));
                        }
                        else
                        {
                            string sourceFilePath = info.FullName;
                            string pathInProject = info.FullName.Substring(sourceBasePath.Length);
                            proj.AddFileToBuild(targetGuid, proj.AddFile(sourceFilePath, pathInProject, PBXSourceTree.Absolute));
                        }
                    }
                };
                addDirectoryToProject(new DirectoryInfo(sourcePath));
            }

            proj.WriteToFile(PBXProject.GetPBXProjectPath(projectPath));
        }

        public static void AddFrameworks(string projectPath, string[] requiredFrameworks, string[] weakFrameworks = null)
        {
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(PBXProject.GetPBXProjectPath(projectPath));
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            if (requiredFrameworks != null)
            {
                foreach (var framework in requiredFrameworks)
                    proj.AddFrameworkToProject(targetGuid, framework, false);
            }
            if (weakFrameworks != null)
            {
                foreach (var framework in weakFrameworks)
                    proj.AddFrameworkToProject(targetGuid, framework, true);
            }
            proj.WriteToFile(PBXProject.GetPBXProjectPath(projectPath));
        }

        public static void AddBuildProperties(string projectPath, Dictionary<string, string> properties)
        {
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(PBXProject.GetPBXProjectPath(projectPath));
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            foreach (var pair in properties)
            {
                proj.AddBuildProperty(targetGuid, pair.Key, pair.Value);
            }
            proj.WriteToFile(PBXProject.GetPBXProjectPath(projectPath));
        }
    }
}

#endif