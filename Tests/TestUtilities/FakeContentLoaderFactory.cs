using FakeItEasy;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.TestUtilities
{
    public static class FakeContentLoaderFactory
    {
        public static IContentLoader<T> CreateFakeLoader<T>(T content)
        {
            IContentLoader<T> creation = A.Fake<IContentLoader<T>>();
            A.CallTo(() => creation.LoadAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<T>(content, null, 200)));
            return creation;
        }

        public static IContentLoader<T> CreateFakeFailLoader<T>()
        {
            IContentLoader<T> creation = A.Fake<IContentLoader<T>>();
            A.CallTo(() => creation.LoadAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<T>("This is a simulated fail", 404)));
            return creation;
        }
    }
}
