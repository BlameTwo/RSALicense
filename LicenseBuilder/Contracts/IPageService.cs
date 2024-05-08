using System;

namespace License.Contracts;

public interface IPageService
{
    public Type GetPage(string key);
}
