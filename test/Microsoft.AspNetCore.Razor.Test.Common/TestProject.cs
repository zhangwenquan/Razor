// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public static class TestProject
    {

        public static string GetProjectDirectory(Type type)
        {
            var thisProjectName = type.GetTypeInfo().Assembly.GetName().Name;

            var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

            while (currentDirectory != null &&
                !string.Equals(currentDirectory.Name, thisProjectName, StringComparison.Ordinal))
            {
                currentDirectory = currentDirectory.Parent;
            }

            return currentDirectory.FullName;
        }
    }
}
