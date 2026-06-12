using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class PageCacheTests
{
    [Fact]
    public void GetPage_LoadsOnlyRequestedPage()
    {
        var requestedPages = new List<int>();
        using var cache = new PageCache<FakePage>(pageIndex =>
        {
            requestedPages.Add(pageIndex);
            return new FakePage(pageIndex);
        });

        var page = cache.GetPage(0);

        Assert.Equal(0, page.Index);
        Assert.Equal(new[] { 0 }, requestedPages);
    }

    [Fact]
    public void GetPage_ReusesCachedPage()
    {
        var callCount = 0;
        using var cache = new PageCache<FakePage>(pageIndex =>
        {
            callCount++;
            return new FakePage(pageIndex);
        });

        var first = cache.GetPage(3);
        var second = cache.GetPage(3);

        Assert.Same(first, second);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void Clear_DisposesCachedPages()
    {
        var first = new FakePage(1);
        var second = new FakePage(2);
        var pages = new Queue<FakePage>(new[] { first, second });
        using var cache = new PageCache<FakePage>(_ => pages.Dequeue());

        cache.GetPage(0);
        cache.GetPage(1);

        cache.Clear();

        Assert.True(first.IsDisposed);
        Assert.True(second.IsDisposed);
    }

    private sealed class FakePage : IDisposable
    {
        public FakePage(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
