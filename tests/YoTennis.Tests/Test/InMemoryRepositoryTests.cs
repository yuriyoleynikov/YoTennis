using System;
using System.Collections.Generic;
using System.Text;
using YoTennis.Services;

namespace YoTennis.Tests.Test
{
    public class InMemoryRepositoryTests : RepositoryTests
    {
        private IMatchListService _repository = new InMemoryMatchListService();

        public override IMatchListService GetRepository() => _repository;
    }
}
