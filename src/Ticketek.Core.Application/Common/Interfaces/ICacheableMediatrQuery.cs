namespace Ticketek.Core.Application.Common.Interfaces;

public interface ICacheableMediatrQuery
{
    bool BypassCache { get; }
    string CacheKey { get; }
}