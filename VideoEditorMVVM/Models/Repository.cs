using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Data;
using Windows.Storage;

namespace VideoEditorMVVM.Models
{
    public class Repository
    {
        public Repository()
        {
            LoadFromLocal();
        }

        private ProjectData projectData = new ProjectData();
        public ProjectData ProjectData { get { return projectData; } set { projectData = value; } }

        public LibraryModel LibraryModel { get { return new LibraryModel(ProjectData.Library); } }
        public TimingModel TimingModel { get { return new TimingModel(ProjectData.Timing); } }
        public TimelineModel TimelineModel { get { return new TimelineModel(ProjectData); } }

        public XDocument ExportToXDoc()
        {
            XDocument doc = new XDocument(projectData.ToXElement());
            doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
            return doc;
        }

        public void LoadFromXDoc(XDocument xDocument)
        {
            ProjectData p = new ProjectData();
            p.LoadFromXElement(xDocument.Root);
            projectData = p;
        }
        // This example code can be used to read or write to an ApplicationData folder of your choice.

        // Change this to Windows.Storage.StorageFolder roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
        // to use the RoamingFolder instead, for example.
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        // Write data to a file
        public async Task SaveToLocal()
        {
            StorageFile file = await localFolder.CreateFileAsync("dataStore.VidU", CreationCollisionOption.ReplaceExisting);
            XDocument doc = ExportToXDoc();
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                await Task.Run(() => {
                    stream.SetLength(0);
                    doc.Save(stream);
                    stream.Close();
                });
            }
        }

        // Read data from a file
        private async void LoadFromLocal()
        {
            try
            {
                StorageFile file = await localFolder.GetFileAsync("dataStore.VidU");
                XDocument doc = null;
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    await System.Threading.Tasks.Task.Run(() => {
                        try
                        {
                            doc = XDocument.Load(stream);
                            LoadFromXDoc(doc);
                        }catch (Exception) { }
                    });
                }
            }
            catch (Exception e)
            {
                MainPage.Status = "Repository: "+ e.Message;
            }
        }
    }
}
