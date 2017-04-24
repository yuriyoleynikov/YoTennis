using System;
using System.Collections.Generic;
using System.Text;
using YoTennis.Services;

namespace YoTennis.Tests.Test
{
    public class InMemoryMatchListServiceTests : MatchListServiceTests
    {
        private IMatchListService _matchListService = new InMemoryMatchListService();

        public override IMatchListService GetMatchListService() => _matchListService;
    }
}
