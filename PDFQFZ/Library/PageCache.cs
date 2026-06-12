using System;
using System.Collections.Generic;

namespace PDFQFZ.Library
{
    internal sealed class PageCache<T> : IDisposable where T : class, IDisposable
    {
        private readonly Func<int, T> pageLoader;
        private readonly Dictionary<int, T> pages = new Dictionary<int, T>();
        private bool disposed;

        public PageCache(Func<int, T> pageLoader)
        {
            if (pageLoader == null)
            {
                throw new ArgumentNullException(nameof(pageLoader));
            }

            this.pageLoader = pageLoader;
        }

        public T GetPage(int pageIndex)
        {
            ThrowIfDisposed();

            T cachedPage;
            if (pages.TryGetValue(pageIndex, out cachedPage))
            {
                return cachedPage;
            }

            T page = pageLoader(pageIndex);
            pages.Add(pageIndex, page);
            return page;
        }

        public void Clear()
        {
            foreach (T page in pages.Values)
            {
                page.Dispose();
            }

            pages.Clear();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            Clear();
            disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}
