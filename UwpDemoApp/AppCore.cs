using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UwpDemoApp
{
    public class AppCore
    {
        protected LiteDB.LiteDatabase Database;
        private readonly LiteDB.ConnectionString ConnectionString;
        private bool IsDisposed;

        public AppCore(string filePath)
        {
            IsDisposed = false;
            ConnectionString = new LiteDB.ConnectionString
            {
                Filename = filePath,
                Password = null
            };
        }

        private class TextInstance
        {
            public string Text { get; set; }
        }

        public async Task OpenAsync(string password)
        {
            ConnectionString.Password = password;
            await Task.Run(() => Database = new LiteDB.LiteDatabase(ConnectionString)).ConfigureAwait(false);
        }

        public async Task WriteText(string text)
        {
            var instanceToInsert = new TextInstance { Text = text };
            await Task.Run(() => Database.GetCollection<TextInstance>().Insert(instanceToInsert)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> ReadText()
        {
            var collectionOfInstances = await Task.Run(() => Database.GetCollection<TextInstance>().FindAll()).ConfigureAwait(false);
            return collectionOfInstances.Select(i => i.Text);
        }

        public async Task ChangePasswordAsync(string newPassword)
        {
            await RebuildStorageAsync(newPassword).ConfigureAwait(false);
            ConnectionString.Password = newPassword;
        }

        public async Task DeleteDbFiles()
        {
            await Task.Run(() =>
                {
                    if (File.Exists(ConnectionString.Filename))
                    {
                        File.Delete(ConnectionString.Filename);
                    }
                }).ConfigureAwait(false);
        }

        private async Task RebuildStorageAsync(string newPassword)
        {
            LiteDB.Engine.RebuildOptions rebuildOptions = new LiteDB.Engine.RebuildOptions
            {
                Password = newPassword
            };
            await Task.Run(() => Database.Rebuild(rebuildOptions)).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                Database?.Dispose();
            }

            IsDisposed = true;
        }

        ~AppCore() => Dispose(false);
    }
}
