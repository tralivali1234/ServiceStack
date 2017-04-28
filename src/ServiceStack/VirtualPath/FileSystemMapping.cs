﻿using System;
using System.Collections.Generic;
using System.IO;
using ServiceStack.IO;

namespace ServiceStack.VirtualPath
{
    public class FileSystemMapping : AbstractVirtualPathProviderBase
    {
        protected DirectoryInfo RootDirInfo;
        protected FileSystemVirtualDirectory RootDir;
        public string Alias { get; private set; }

        public override IVirtualDirectory RootDirectory => RootDir;
        public override string VirtualPathSeparator => "/";
        public override string RealPathSeparator => Convert.ToString(Path.DirectorySeparatorChar);

        public FileSystemMapping(IAppHost appHost, string alias, string rootDirectoryPath)
            : this(appHost, alias, new DirectoryInfo(rootDirectoryPath))
        { }

        public FileSystemMapping(IAppHost appHost, string alias, DirectoryInfo rootDirInfo)
            : base(appHost)
        {
            if (alias == null)
                throw new ArgumentNullException(nameof(alias));

            if (alias.IndexOfAny(new []{ '/', '\\' }) >= 0)
                throw new ArgumentException($"Alias '{alias}' cannot contain directory separators");

            if (rootDirInfo == null)
                throw new ArgumentNullException(nameof(rootDirInfo));

            this.Alias = alias;
            this.RootDirInfo = rootDirInfo;
            Initialize();
        }

        protected sealed override void Initialize()
        {
            if (!RootDirInfo.Exists)
                throw new Exception($"RootDir '{RootDirInfo.FullName}' for virtual path does not exist");

            RootDir = new FileSystemVirtualDirectory(this, null, RootDirInfo);
        }

        public string GetRealVirtualPath(string virtualPath)
        {
            virtualPath = virtualPath.TrimStart('/');
            return virtualPath.StartsWith(Alias)
                ? virtualPath.Substring(Alias.Length)
                : null;
        }

        public override IVirtualFile GetFile(string virtualPath)
        {
            var nodePath = GetRealVirtualPath(virtualPath);
            return nodePath != null 
                ? base.GetFile(nodePath)
                : null;
        }

        public override IVirtualDirectory GetDirectory(string virtualPath)
        {
            var nodePath = GetRealVirtualPath(virtualPath);
            return nodePath != null
                ? base.GetDirectory(nodePath)
                : null;
        }

        public override IEnumerable<IVirtualDirectory> GetRootDirectories()
        {
            return new[] { new InMemoryVirtualDirectory(new InMemoryVirtualPathProvider(base.AppHost), Alias), };
        }

        public override IEnumerable<IVirtualFile> GetRootFiles()
        {
            return new IVirtualFile[0];
        }
    }
}
