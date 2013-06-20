﻿using System.Collections.ObjectModel;

namespace ArchiCop.ViewModel
{
    public class MetadataFilesViewModel : CommandListViewModel
    {
        public MetadataFilesViewModel(ObservableCollection<string> metadataFiles)
            : base(Properties.Resources.MetadataFilesViewModel_DisplayName)
        {
            MetadataFiles = metadataFiles;
        }

        public ObservableCollection<string> MetadataFiles { get; set; }
    }
}