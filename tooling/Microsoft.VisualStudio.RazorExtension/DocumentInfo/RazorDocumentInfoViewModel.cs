// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if RAZOR_EXTENSION_DEVELOPER_MODE

<<<<<<< 0688cd3ef73ea50999e7a95aa53a12e7e03f3e93
using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;
=======
using Microsoft.CodeAnalysis.Razor.Workspaces;
>>>>>>> wip

namespace Microsoft.VisualStudio.RazorExtension.DocumentInfo
{
    public class RazorDocumentInfoViewModel : NotifyPropertyChanged
    {
<<<<<<< 0688cd3ef73ea50999e7a95aa53a12e7e03f3e93
        private readonly RazorDocumentTracker _documentTracker;

        public RazorDocumentInfoViewModel(RazorDocumentTracker documentTracker)
        {
            if (documentTracker == null)
            {
                throw new ArgumentNullException(nameof(documentTracker));
            }

            _documentTracker = documentTracker;
        }

        public bool IsSupportedDocument => _documentTracker.IsSupportedDocument;

        public Project Project
        {
            get
            {
                if (Workspace != null && ProjectId != null)
                {
                    return Workspace.CurrentSolution.GetProject(ProjectId);
                }

                return null;
            }
        }

        public ProjectId ProjectId => _documentTracker.ProjectId;

        public SourceTextContainer TextContainer => _documentTracker.TextContainer;

        public Workspace Workspace => _documentTracker.Workspace;
=======
        private readonly RazorEditorWorker _worker;

        public RazorDocumentInfoViewModel(RazorEditorWorker worker)
        {
            _worker = worker;
        }
>>>>>>> wip
    }
}

#endif